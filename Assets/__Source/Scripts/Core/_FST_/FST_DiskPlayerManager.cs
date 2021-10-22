///////////////////////////////////////////////////////////////////////////////////////////
//
//  FST_DiskPlayerManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class is designed to:
//                              a) stop disks when they are below speed threshold.
//                              b) move disks out of goal positions to the nearest available
//                                 free space
//                              c) keep disks in sync across the network and smooth out lag
//                                 using interpolation.
//                              d) contains a method to call to change the ownerships and
//                                 allow the disk to be moved by current player playing turn.
//                                 TransferOwnership(player)
//                              e) sync arrow helper visuals
//                  
//
//////////////////////////////////////////////////////////////////////////////////////////

using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
namespace FastSkillTeam
{
    public class FST_DiskPlayerManager : MonoBehaviourPun, IPunObservable
    {
        public static FST_DiskPlayerManager Instance;

        #region Inspector Vars
#pragma warning disable CS0649
        [SerializeField] private Transform FreeSpacesPlayerParent;
        [SerializeField] private Transform FreeSpacesOpponentParent;
        [SerializeField] private LayerMask LayerMask;
        [SerializeField] private Collider PlayerGoalTrigger;
        [SerializeField] private Collider OpponentGoalTrigger;
        [SerializeField] private Transform ShootCircle;
        [SerializeField] private Transform ArrowPlane;
        [SerializeField] private FST_GhostBall BallGhost;
        // [SerializeField] private ParticleSystem PlayerGoalGlow;
        // [SerializeField] private ParticleSystem OpponentGoalGlow;

        public Renderer[] PlayerGoalMats;
        public Renderer[] OpponentGoalMats;
        [SerializeField] private float m_GlowSpeed = 0.1f;

#pragma warning restore


        private float curGlowIntensity = 0;
        private bool isUpGlow = true;


        public List<DiskPlayerData> DiskPlayerDatas = new List<DiskPlayerData>();

        #endregion

        #region Expected Renderers

        private Renderer m_ShootCircleRenderer = null;
        private Renderer ShootCircleRenderer { get { if (!m_ShootCircleRenderer) m_ShootCircleRenderer = ShootCircle.GetComponent<Renderer>(); return m_ShootCircleRenderer; } }
        private Renderer m_ArrowPlaneRenderer = null;
        private Renderer ArrowPlaneRenderer { get { if (!m_ArrowPlaneRenderer) m_ArrowPlaneRenderer = ArrowPlane.GetComponent<Renderer>(); return m_ArrowPlaneRenderer; } }

        private List<Renderer> m_P1SelectionCircles = new List<Renderer>();
        private List<Renderer> GetP1SelectionCircles
        {
            get
            {
                if (m_P1SelectionCircles.Count < 1)
                {
                    for (int i = 0; i < NetPlayerDatas.Count; i++)
                    {
                        if (NetPlayerDatas[i].DiskType == DiskPlayerData.Type.Player1)
                            m_P1SelectionCircles.Add(NetPlayerDatas[i].SelectionCircle);
                    }
                }
                return m_P1SelectionCircles;
            }
        }

        private List<Renderer> m_P2SelectionCircles = new List<Renderer>();
        private List<Renderer> GetP2SelectionCircles
        {
            get
            {
                if (m_P2SelectionCircles.Count < 1)
                {
                    for (int i = 0; i < NetPlayerDatas.Count; i++)
                    {
                        if (NetPlayerDatas[i].DiskType == DiskPlayerData.Type.Player2)
                            m_P2SelectionCircles.Add(NetPlayerDatas[i].SelectionCircle);
                    }
                }
                return m_P2SelectionCircles;
            }
        }

        #endregion

        #region Disk Data

        /// <summary>
        /// This class is a container for all disks in the scene
        /// </summary>
        [System.Serializable]
        public class DiskPlayerData
        {
            public enum Type { Player1, Player2, AI }

            public Type DiskType = Type.Player1;

            /// <summary>
            /// for online only
            /// </summary>
            public Renderer SelectionCircle { get { return DiskType == Type.Player1 ? Player1Controller.SelectionCircleRenderer : DiskType == Type.Player2 ? Player2Controller.SelectionCircleRenderer : OpponentController.SelectionCircleRenderer; } }

            public PlayerController Player1Controller;
            public Player2Controller Player2Controller;
            public OpponentUnitController OpponentController;
            //Store position locally, default 0s ->FST
            private Vector3 m_CurrentPositionVector = Vector3.zero;

            //now any time we can request the position, and if it so happens that fixed update (physics) is too slow, it will not matter as we return the stored var ->FST
            public Vector3 CurrentPositionVector
            {
                get
                {
                    //always return the last stored var
                    return m_CurrentPositionVector;
                }

                set
                {
                    // set via fixed update so if its late it does not matter as the last stored position will be unnoticibly close
                    m_CurrentPositionVector = value;
                }
            }

            public bool DidCollide { get; set; } = false;

            public bool ShouldMoveToWasFromPlayerGoalEnd = false;

            public string CurrentPositionAsString { get { return CurrentPositionVector.ToString(); } }

            public Vector2 ShouldMoveTo = Vector3.zero;

            public Vector3 LastCollisionPosition = Vector3.zero;

            private MeshCollider m_Collider = null;
            public MeshCollider GetCollider { get { if (!m_Collider) m_Collider = GetRigidbody.GetComponent<MeshCollider>(); return m_Collider; } }

            private Transform m_DiskTop;
            public Transform DiskTop { get { if (!m_DiskTop) m_DiskTop = Transform.GetChild(1); return m_DiskTop; } }

            private Transform m_Transform = null;
            public Transform Transform { get { if (!m_Transform) m_Transform = GetRigidbody.transform; return m_Transform; } }

            private Rigidbody m_RigidBody;

            public Rigidbody GetRigidbody
            {
                get
                {
                    switch (DiskType)
                    {
                        case Type.Player1:
                            //if we dont have rb, get it and cache it->FST
                            if (!m_RigidBody)
                                if (Player1Controller)
                                    m_RigidBody = Player1Controller.GetComponent<Rigidbody>();

                            //if we still dont have rb, make it and cache it ->FST
                            if (!m_RigidBody)
                                m_RigidBody = Player1Controller.gameObject.AddComponent<Rigidbody>();
                            break;

                        case Type.Player2:
                            //if we dont have rb, get it and cache it->FST
                            if (!m_RigidBody)
                                if (Player2Controller)
                                    m_RigidBody = Player2Controller.GetComponent<Rigidbody>();

                            //if we still dont have rb, make it and cache it ->FST
                            if (!m_RigidBody)
                                m_RigidBody = Player2Controller.gameObject.AddComponent<Rigidbody>();
                            break;

                        case Type.AI:
                            //if we dont have rb, get it and cache it->FST
                            if (!m_RigidBody)
                                if (OpponentController)
                                    m_RigidBody = OpponentController.GetComponent<Rigidbody>();

                            //if we still dont have rb, make it and cache it ->FST
                            if (!m_RigidBody)
                                m_RigidBody = OpponentController.gameObject.AddComponent<Rigidbody>();
                            break;
                    }

                    //return cached var ->FST
                    return m_RigidBody;
                }
            }
            //TODO add torque to this check...
            public bool IsDiskStop
            {
                get
                {
                    if (GetRigidbody.velocity.magnitude < Instance.GetPhysicsData.DiskStopThreshold_Velocity && GetRigidbody.angularVelocity.magnitude < Instance.GetPhysicsData.DiskStopThreshold_AngularVelocity)
                    {
                        GetRigidbody.velocity = Vector3.zero;
                        GetRigidbody.angularVelocity = Vector3.zero;
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>
        /// cached filtered list of DiskPlayerData for only network disks
        /// </summary>
        private List<DiskPlayerData> m_NetworkDiskPlayerDatas = null;

        /// <summary>
        /// A filtered list containing only disks we use for network
        /// </summary>
        public List<DiskPlayerData> NetPlayerDatas
        {
            get
            {
                if (m_NetworkDiskPlayerDatas == null || m_NetworkDiskPlayerDatas.Count < 1)
                {
                    m_NetworkDiskPlayerDatas = new List<DiskPlayerData>();
                    for (int i = 0; i < DiskPlayerDatas.Count; i++)
                        if (!m_NetworkDiskPlayerDatas.Contains(DiskPlayerDatas[i]))
                            if (DiskPlayerDatas[i].DiskType != DiskPlayerData.Type.AI)
                                m_NetworkDiskPlayerDatas.Add(DiskPlayerDatas[i]);
                }

                return m_NetworkDiskPlayerDatas;
            }
        }

        #endregion


        // just to keep code clean and fast to write
        private Rigidbody Ball { get { return FST_BallManager.Instance.GetRigidBody; } }

        #region Network vars

        // Ball values that will be synced over network
        private Vector2 latestBallPos;
        private Quaternion latestBallRot = Quaternion.identity;
        private Vector2 latestBallVelocity;

        private bool isShootHelperActive = false;
        private Vector2 shootCirclePosition = Vector2.zero;
        private float arrowPlaneZRotation = -180f;//NOTE: for objects that rotate on one axis only, we only use the axis that is affected, saving on data transfer, later this will be in (bytes)
        private Vector2 arrowPlaneScale = Vector2.zero;
        private Vector2 shootCircleScale = Vector2.zero;

        // Disk values that will be synced over network
        private Vector2[]/*byte[][]*/ latestPositions;
        private float[]/*byte[][]*/ latestDiskTopYAngles = new float[0]/*byte[0][]*/;//NOTE: for objects that rotate on one axis only, we only use the axis that is affected, saving on data transfer, later this will be in (bytes)
        private Vector2[]/*byte[][]*/ latestVelocities;

        // Lag compensation time cache
        private float currentTime = 0;
        private double currentPacketTime = 0;
        private double lastPacketTime = 0;

        // Lag compensation disk values cache
        private Vector2[] positionsAtLastPacket = new Vector2[0];
        private float[] diskTopYRotationsAtLastPacket = new float[0];
        private Vector2[] velocitiesAtLastPacket = new Vector2[0];

        // Lag compensation ball values cache
        private Vector2 ballPosAtLastPacket;
        private Quaternion ballRotAtLastPacket = Quaternion.identity;

        #endregion

        public bool IsOwner { get { return photonView.Owner != null && photonView.IsMine && GlobalGameManager.Instance.IsMyTurn; } }

        private const float CHECK_SIZE = 0.985f;//fine tuning for finding a position to move to when disk is in goal pos

        //private working vars
        private bool m_AllStopped = true;
        private bool m_ForceMove = false;

        PhotonStreamQueue queue = new PhotonStreamQueue(60);
        private bool MovementInterpolationByPing = false;
        private bool RotationInterpolationByPing = false;

        private bool useCollisionPrediction = false;

        private int fakeMoveSpeed = 10;
        private int fakeRotSpeed = 10;

        //      private bool isSendCollision = true;

        private FST_PhysicsData m_PhysicsData = null;
        public FST_PhysicsData GetPhysicsData { get { if (!m_PhysicsData) m_PhysicsData = Resources.Load("FST_PhysicsData") as FST_PhysicsData; return m_PhysicsData; } }

        private DiskPlayerData mainLoopDiskData = null;

        #region MonoBehaviour Callbacks

        private void Awake()
        {
            Instance = this;

            if (FST_Gameplay.IsMaster && ChanceOfRain > 0)
                InvokeRepeating("Weather", 0, 30);
        }
        [Range(0,100)]
        public int ChanceOfRain = 30;
        private void Weather()
        {
            if (Random.Range(0f, 100f) <= ChanceOfRain)
                Rain.RainIntensity = Random.Range(0.1f, 1f);
            else Rain.RainIntensity = 0;

            if (FST_Gameplay.IsMultiplayer)
                TransmitRain();
        }

        public DigitalRuby.RainMaker.RainScript2D Rain;
 
        private void TransmitRain()
        {
            if (!PhotonNetwork.InRoom)
                return;

            if (FST_Gameplay.IsMaster)
                photonView.RPC("RPC_ReceiveRain", RpcTarget.Others, Rain.RainIntensity);
        }

        [PunRPC]
        private void RPC_ReceiveRain(float intensity)
        {
            Rain.RainIntensity = intensity;
        }


        private void Start()
        {
            foreach (Renderer _renderer in PlayerGoalMats)
            {
                if (_renderer)
                    if (_renderer.material)
                        _renderer.material.EnableKeyword("_EMISSION");
            }
            foreach (Renderer _renderer in OpponentGoalMats)
            {
                if (_renderer)
                    if (_renderer.material)
                        _renderer.material.EnableKeyword("_EMISSION");
            }

            ApplyPhysicsParameters();

            if (FST_Gameplay.IsMultiplayer && FST_Gameplay.IsMaster)
                photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
        }

        private void OnDisable()
        {
            Instance = null;
            StopAllCoroutines();
            CancelInvoke();
        }
       
        private void KinematicCheck()
        {
            if (FST_Gameplay.IsMultiplayer)
            {
                if (!IsOwner)
                {
                    if (Ball.isKinematic == false)
                        Ball.isKinematic = true;

                    for (int i = 0; i < NetPlayerDatas.Count; i++)
                    {
                        NetPlayerDatas[i].GetRigidbody.detectCollisions = false;

                        if (NetPlayerDatas[i].GetRigidbody.collisionDetectionMode != CollisionDetectionMode.ContinuousSpeculative)
                            NetPlayerDatas[i].GetRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;

                        if (NetPlayerDatas[i].GetRigidbody.isKinematic == false)
                            NetPlayerDatas[i].GetRigidbody.isKinematic = true;
                    }
                }
                else
                {
                    if (Ball.isKinematic == true)
                        Ball.isKinematic = false;

                    for (int i = 0; i < NetPlayerDatas.Count; i++)
                    {
                        NetPlayerDatas[i].GetRigidbody.detectCollisions = true;

                        if (NetPlayerDatas[i].GetRigidbody.isKinematic == true)
                            NetPlayerDatas[i].GetRigidbody.isKinematic = false;

                        if (NetPlayerDatas[i].GetRigidbody.collisionDetectionMode != GetPhysicsData.DiskCollisionDetectionMode)
                            NetPlayerDatas[i].GetRigidbody.collisionDetectionMode = GetPhysicsData.DiskCollisionDetectionMode;
                    }
                }

                return;
            }

            for (int i = 0; i < DiskPlayerDatas.Count; i++)
            {
                if (DiskPlayerDatas[i].GetRigidbody.isKinematic)
                {
                    DiskPlayerDatas[i].GetRigidbody.detectCollisions = false;
              
                    if (DiskPlayerDatas[i].GetRigidbody.collisionDetectionMode != CollisionDetectionMode.ContinuousSpeculative)
                        DiskPlayerDatas[i].GetRigidbody.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
                }
                else
                {
                    DiskPlayerDatas[i].GetRigidbody.detectCollisions = true;

                    if (DiskPlayerDatas[i].GetRigidbody.collisionDetectionMode != GetPhysicsData.DiskCollisionDetectionMode)
                        DiskPlayerDatas[i].GetRigidbody.collisionDetectionMode = GetPhysicsData.DiskCollisionDetectionMode;
                }
            }
        }
        private void MainLoop()
        {
            if (FST_Gameplay.IsMultiplayer)
            {
                for (int i = 0; i < NetPlayerDatas.Count; i++)
                {
                    mainLoopDiskData = DiskPlayerDatas[i];
                    LimitSpeed(mainLoopDiskData);//1
                    CheckIfGoalContainsDisk(mainLoopDiskData);//2
                    SetMoving(mainLoopDiskData);//3
                }

                return;
            }


            for (int i = 0; i < DiskPlayerDatas.Count; i++)
            {
                mainLoopDiskData = DiskPlayerDatas[i];
                LimitSpeed(mainLoopDiskData);//1
                CheckIfGoalContainsDisk(mainLoopDiskData);//2
                SetMoving(mainLoopDiskData);//3
            }
        }

        private void Update()
        {
#if UNITY_EDITOR
            ApplyPhysicsParameters();//NOTE: only in editor as this can produce lag on mobile

            //debug, NOTE: wrap this before packaging for release..
            RunDebugGUICheck();
#endif

            Glow();//optionally make this only for owner/turn player

            MainLoop();

            SetStopped();//4

            KinematicCheck();

            if (FST_Gameplay.IsMultiplayer)
            {
                if (photonView.Owner == null || GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.NotStarted || GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.FormationSelect)
                {
                    //  Debug.Log("Clear Queue!");
                    if (queue.HasQueuedObjects())
                        queue.Reset();
                    return;
                }

                if (IsOwner)//we need to write the data, we are the player playing turn
                {
                    WriteQueue();
                }
                else
                {
                    ReadQueue();//we need to read data, we are not the player playing turn

                    UIManager.Instance.curveLoftBtn.interactable = false;

                    RemoteMovements();
                }
            }
            BallBoundsCheck();

            DiskBoundsCheck();
        }

        #endregion

        #region Methods

        //     bool[] selections;

        public int GetDiskIdByTransform(Transform t)
        {
            int id = 0;
            for (int i = 0; i < NetPlayerDatas.Count; i++)
            {
                if (t == NetPlayerDatas[i].Transform)
                {
                    id = i;
                    break;
                }
            }
            return id;
        }

        public DiskPlayerData GetDiskByID(int id)
        {
            for (int i = 0; i < NetPlayerDatas.Count; i++)
                if (i == id)
                    return NetPlayerDatas[i];

            return null;
        }

        public void ResetBall(Vector3 pos)
        {
            Ball.transform.position = pos;
            latestBallPos = pos;
            ballPosAtLastPacket = pos;
            latestBallVelocity = Vector2.zero;
            Ball.velocity = Vector3.zero;
        }

        private void RemoteMovements()
        {
            // Lag compensation
            double timeToReachGoal = currentPacketTime - lastPacketTime;
            currentTime += Time.deltaTime;

            float interp = MovementInterpolationByPing ? 
                (float)(currentTime / timeToReachGoal) :
                Time.deltaTime * fakeMoveSpeed;
            //(float)(currentTime / timeToReachGoal);
            float rInterp = RotationInterpolationByPing ? 
                (float)(currentTime / timeToReachGoal) :
                Time.deltaTime * fakeRotSpeed;
            //(float)(currentTime / timeToReachGoal);

            //Apply datas
            if (positionsAtLastPacket.Length == NetPlayerDatas.Count)
            {
                for (int i = 0; i < NetPlayerDatas.Count; i++)
                {
                    DiskPlayerData d = NetPlayerDatas[i];

                    if (d.DidCollide)
                    {
                        d.DidCollide = false;
                        //if (lastColTimeStampDisks > lastDeserialiseTimeStamp)
                        //{
                        //    Debug.Log("EARLY DISK COLLISION DETECTED");
                        //    if (MovementInterpolationByPing)
                        //        positionsAtLastPacket[i] += (Vector2)d.GetRigidbody.velocity * Time.deltaTime;
                        //    else
                        //        d.Transform.position += d.GetRigidbody.velocity * /*lag **/ Time.deltaTime;
                        //    continue;
                        //}
                    }

                    // Vector2 lagDist = (Vector2)NetPlayerDatas[i].GetRigidbody.position - latestPositions[i];//usage > lagDist.magnitude

                    //apply recieved velocity
                    d.GetRigidbody.velocity = latestVelocities[i];

                    if (MovementInterpolationByPing)
                        positionsAtLastPacket[i] += (Vector2)d.GetRigidbody.velocity * Time.deltaTime;
                    else
                        d.Transform.position += d.GetRigidbody.velocity * /*lag **/ Time.deltaTime;

                    //smoothly move the disk
                    Vector2 p = Vector2.Lerp(MovementInterpolationByPing ? positionsAtLastPacket[i] : (Vector2)d.Transform.position, latestPositions[i], interp);
                    //apply the calculated position to the disk
                    d.Transform.position = new Vector3(p.x, p.y, d.Transform.position.z);

                    float y = Mathf.LerpAngle(RotationInterpolationByPing ? diskTopYRotationsAtLastPacket[i] : d.DiskTop.localRotation.eulerAngles.y, latestDiskTopYAngles[i], rInterp);
                    d.DiskTop.localRotation = Quaternion.Euler(y * Vector3.up);

                    if ((Vector2)d.Transform.position != positionsAtLastPacket[i])
                        m_AllStopped = false;
                }
            }

            //for (int i = 0; i < NetPlayerDatas.Count; i++)
            //{
            //    NetPlayerDatas[i].SelectionCircle.enabled = selections[i];
            //}

            //if (lastDeserialiseTimeStamp > lastColTimeStampBall)
            //{
                Ball.velocity = latestBallVelocity;

                //apply some network prediction using the velocity as the predictor, but first check the ball is going fast enough to warrant this need of prediction..
                if (latestBallVelocity.magnitude > GetPhysicsData.BallStopThreshold_Velocity)
                {
                    if (MovementInterpolationByPing)
                        ballPosAtLastPacket += (Vector2)Ball.velocity * Time.deltaTime;
                    else
                        Ball.transform.position += Ball.velocity * /*lag **/ Time.deltaTime;
                }

            //smooth ball pos with interpolation
            Vector2 bp = Vector2.Lerp(MovementInterpolationByPing ? ballPosAtLastPacket : (Vector2)Ball.transform.position, latestBallPos, interp);
                //apply ballpos as v3
                Ball.transform.position = new Vector3(bp.x, bp.y, Ball.transform.position.z);
                //apply smooth rotation to ball
                Ball.transform.rotation = Quaternion.Lerp(RotationInterpolationByPing ? ballRotAtLastPacket : Ball.transform.rotation, latestBallRot, rInterp);

                if ((Vector2)Ball.transform.position != ballPosAtLastPacket)
                    m_AllStopped = false;
            //}
            //else
            //{
            //    Debug.Log("EARLY BALL COLLISION DETECTED");
            //}

            //handle the selection gizmo
            //NOTE: here we only take position and enable data from one object, the circle renderer, we then use its properties to enable/disable and position the arrow plane
            ShootCircleRenderer.enabled = isShootHelperActive;
            ShootCircle.position = new Vector3(shootCirclePosition.x, shootCirclePosition.y, ShootCircle.position.z);
            ArrowPlaneRenderer.enabled = isShootHelperActive;
            ArrowPlane.position = ShootCircle.position + new Vector3(0, 0, -1.55f);

            //now we apply the streamed values for scale and rotation of the arrowplane
            float z = Mathf.LerpAngle(ArrowPlane.localRotation.eulerAngles.z, arrowPlaneZRotation, rInterp);
            ArrowPlane.localRotation = Quaternion.Euler(ArrowPlane.localRotation.eulerAngles.x, ArrowPlane.localRotation.eulerAngles.y, z);
            ArrowPlane.localScale = new Vector3(arrowPlaneScale.x, arrowPlaneScale.y, 0.001f);
            ShootCircleRenderer.transform.localScale = new Vector3(shootCircleScale.x, shootCircleScale.y, ShootCircleRenderer.transform.localScale.z);
        }

        private void DiskBoundsCheck()
        {
            Vector4 bounds = new Vector4(GetPhysicsData.FieldBoundsDisk.x, GetPhysicsData.FieldBoundsDisk.y, GetPhysicsData.FieldBoundsDisk.z, GetPhysicsData.FieldBoundsDisk.w);
            float goalTop = GetPhysicsData.GoalMouthTopDisk;
            float goalBottom = GetPhysicsData.GoalMouthBottomDisk;
            float goalLeftBack = GetPhysicsData.LeftGoalNetBackDisk;
            float goalRightBack = GetPhysicsData.RightGoalNetBackDisk;
#if UNITY_EDITOR
            DrawDebugBoundary(bounds, new Vector2(goalBottom, goalTop), new Vector2(goalLeftBack, goalRightBack), false);
#endif

            //run a bounds check on each disk, ensuring that the disk stays in our defined bounds (collisions can be too slow)
            for (int i = 0; i < DiskPlayerDatas.Count; i++)
            {
                DiskPlayerData disk = DiskPlayerDatas[i];

                if (disk.ShouldMoveTo != Vector2.zero)//disk is being moved forcefully from goal bounds
                    continue;

                if (disk.Transform.position.y > bounds.x)
                {
                    disk.Transform.position = new Vector3(disk.Transform.position.x, bounds.x, disk.Transform.position.z);
                    //Debug.Log("TOP");
                }
                if (disk.Transform.position.y < bounds.z)
                {
                    disk.Transform.position = new Vector3(disk.Transform.position.x, bounds.z, disk.Transform.position.z);
                    //Debug.Log("BOTTOM");
                }
                if (disk.Transform.position.x > bounds.y)
                {
                    if (disk.Transform.position.y > goalTop || disk.Transform.position.y < goalBottom)
                    {
                        disk.Transform.position = new Vector3(bounds.y, disk.Transform.position.y, disk.Transform.position.z);
                        //Debug.Log("RIGHT");
                    }
                    else
                    {
                        //keep disk in the right goal bounds
                        if (disk.Transform.position.y > goalTop)
                        {
                            disk.Transform.position = new Vector3(disk.Transform.position.x, goalTop, disk.Transform.position.z);
                            //  Debug.Log("RIGHT GOAL NET TOP");
                        }
                        else if (disk.Transform.position.y < goalBottom)
                        {
                            disk.Transform.position = new Vector3(disk.Transform.position.x, goalBottom, disk.Transform.position.z);
                            //  Debug.Log("RIGHT GOAL NET BOTTOM");
                        }
                        else if (disk.Transform.position.x > goalRightBack)
                        {
                            disk.Transform.position = new Vector3(goalRightBack, disk.Transform.position.y, disk.Transform.position.z);
                            //  Debug.Log("RIGHT GOAL NET BACK");
                        }
                    }
                }
                if (disk.Transform.position.x < bounds.w)
                {
                    if (disk.Transform.position.y > goalTop || disk.Transform.position.y < goalBottom)
                    {
                        disk.Transform.position = new Vector3(bounds.w, disk.Transform.position.y, disk.Transform.position.z);
                        //Debug.Log("LEFT");
                    }
                    else
                    {
                        //keep disk in the left goal bounds
                        if (disk.Transform.position.y > goalTop)
                        {
                            disk.Transform.position = new Vector3(disk.Transform.position.x, goalTop, disk.Transform.position.z);
                            //  Debug.Log("LEFT GOAL NET TOP");
                        }
                        else if (disk.Transform.position.y < goalBottom)
                        {
                            disk.Transform.position = new Vector3(disk.Transform.position.x, goalBottom, disk.Transform.position.z);
                            //  Debug.Log("LEFT GOAL NET BOTTOM");
                        }
                        else if (disk.Transform.position.x < goalLeftBack)
                        {
                            disk.Transform.position = new Vector3(goalLeftBack, disk.Transform.position.y, disk.Transform.position.z);
                            //  Debug.Log("LEFT GOAL NET BACK");
                        }
                    }
                }
            }
        }

        private void BallBoundsCheck()
        {
            Vector4 bounds = new Vector4(GetPhysicsData.FieldBoundsBall.x, GetPhysicsData.FieldBoundsBall.y, GetPhysicsData.FieldBoundsBall.z, GetPhysicsData.FieldBoundsBall.w);

            float goalTop = GetPhysicsData.GoalMouthTopBall;
            float goalBottom = GetPhysicsData.GoalMouthBottomBall;
            float goalLeftBack = GetPhysicsData.LeftGoalNetBackBall;
            float goalRightBack = GetPhysicsData.RightGoalNetBackBall;
#if UNITY_EDITOR
            DrawDebugBoundary(bounds, new Vector2(goalBottom, goalTop), new Vector2(goalLeftBack, goalRightBack), true);
#endif
            //run a bounds check on the ball, ensuring that it stays in our defined bounds (collisions can be too slow)
            if (Ball.transform.position.y > bounds.x)
            {
                Ball.transform.position = new Vector3(Ball.transform.position.x, bounds.x, Ball.transform.position.z);
                //Debug.Log("TOP BORDER");
            }
            else if (Ball.transform.position.y < bounds.z)
            {
                Ball.transform.position = new Vector3(Ball.transform.position.x, bounds.z, Ball.transform.position.z);
                //Debug.Log("BOTTOM BORDER");
            }
            else if (Ball.transform.position.x > bounds.y)
            {
                if (Ball.transform.position.y > goalTop || Ball.transform.position.y < goalBottom)
                {
                    Ball.transform.position = new Vector3(bounds.y, Ball.transform.position.y, Ball.transform.position.z);
                    //Debug.Log("RIGHT BORDER");
                }
                else
                {
                    //keep ball in the right goal bounds
                    if (Ball.transform.position.y > goalTop)
                    {
                        Ball.transform.position = new Vector3(Ball.transform.position.x, goalTop, Ball.position.z);
                        //  Debug.Log("RIGHT GOAL NET TOP");
                    }
                    else if (Ball.transform.position.y < goalBottom)
                    {
                        Ball.transform.position = new Vector3(Ball.transform.position.x, goalBottom, Ball.position.z);
                        //  Debug.Log("RIGHT GOAL NET BOTTOM");
                    }
                    else if (Ball.transform.position.x > goalRightBack)
                    {
                        Ball.transform.position = new Vector3(goalRightBack, Ball.transform.position.y, Ball.position.z);
                        //  Debug.Log("RIGHT GOAL NET BACK");
                    }
                }
            }
            else if (Ball.transform.position.x < bounds.w)
            {
                // if ball is higher than the goal mouth top || the ball is lower than the goal mouth bottom
                if (Ball.transform.position.y > goalTop || Ball.transform.position.y < goalBottom)
                {
                    // the ball is hitting the wall
                    Ball.transform.position = new Vector3(bounds.w, Ball.transform.position.y, Ball.position.z);
                    //Debug.Log("LEFT");
                }
                else
                {
                    //keep ball in the left goal bounds
                    if (Ball.transform.position.y > goalTop)
                    {
                        Ball.transform.position = new Vector3(Ball.transform.position.x, goalTop, Ball.position.z);
                        //  Debug.Log("LEFT GOAL NET TOP");
                    }
                    else if (Ball.transform.position.y < goalBottom)
                    {
                        Ball.transform.position = new Vector3(Ball.transform.position.x, goalBottom, Ball.position.z);
                        //  Debug.Log("LEFT GOAL NET BOTTOM");
                    }
                    else if (Ball.transform.position.x < goalLeftBack)
                    {
                        Ball.transform.position = new Vector3(goalLeftBack, Ball.transform.position.y, Ball.position.z);
                        //  Debug.Log("LEFT GOAL NET BACK");
                    }
                }
            }
        }

        private void SetStopped()
        {
            bool b = true;

            if (FST_Gameplay.IsMultiplayer)
            {
                for (int i = 0; i < NetPlayerDatas.Count; i++)
                {
                    if (!NetPlayerDatas[i].IsDiskStop)
                    { b = false; break; }
                }
            }
            else for (int i = 0; i < DiskPlayerDatas.Count; i++)
            {
                if (GlobalGameManager.IsOfflineAIMatch == false)
                {
                    if (DiskPlayerDatas[i].DiskType == DiskPlayerData.Type.AI)
                        continue;
                }
                else if (DiskPlayerDatas[i].DiskType == DiskPlayerData.Type.Player2)
                    continue;

                if (!DiskPlayerDatas[i].IsDiskStop)
                { b = false; break; }
            }

            m_AllStopped = !m_ForceMove && b;
        }

        private void SetMoving(DiskPlayerData disk)
        {
            bool b = false;

            if (disk.ShouldMoveTo != Vector2.zero)
            {
                ForceMove(disk);
                b = true;

                for (int x = 0; x < DiskPlayerDatas.Count; x++)
                {
                    if (DiskPlayerDatas[x].ShouldMoveTo == Vector2.zero)
                        continue;

                    if (DiskPlayerDatas[x] == disk)
                        continue;

                    if (Vector2.Distance(DiskPlayerDatas[x].ShouldMoveTo, disk.ShouldMoveTo) < 0.1f)
                        DiskPlayerDatas[x].ShouldMoveTo = FindClosestFreeSpace(DiskPlayerDatas[x].ShouldMoveToWasFromPlayerGoalEnd, DiskPlayerDatas[x].CurrentPositionVector);
                }
            }

            m_ForceMove = b;
        }

        public bool AllMovingObjectsHaveStopped()
        {
            return m_AllStopped && !m_ForceMove && FST_BallManager.Instance.IsBallStopped;
        }

        private void LimitSpeed(DiskPlayerData disk)
        {
            //loop through all data and do whats required ->FST
            if (FST_Gameplay.IsMultiplayer)
            {

                if (disk.GetRigidbody.velocity.magnitude > GetPhysicsData.DiskMaxVelocity)
                    disk.GetRigidbody.velocity = disk.GetRigidbody.velocity.normalized * GetPhysicsData.DiskMaxVelocity;
                //now update our stored variable
                disk.CurrentPositionVector = disk.Transform.position;

            }
            else
            {

                if (disk.GetRigidbody.velocity.magnitude > GetPhysicsData.DiskMaxVelocity)
                    disk.GetRigidbody.velocity = disk.GetRigidbody.velocity.normalized * GetPhysicsData.DiskMaxVelocity;
                //now update our stored variable
                disk.CurrentPositionVector = disk.Transform.position;

            }
        }

        private void CheckIfGoalContainsDisk(DiskPlayerData disk)
        {
            // let the master handle this
            if (FST_Gameplay.IsMultiplayer && !IsOwner)
                return;

            if (disk.IsDiskStop)
            {
                if (PlayerGoalTrigger.bounds.Contains(disk.CurrentPositionVector))
                {
                    disk.ShouldMoveToWasFromPlayerGoalEnd = true;
                    disk.ShouldMoveTo = FindClosestFreeSpace(true, disk.CurrentPositionVector);
                }
                if (OpponentGoalTrigger.bounds.Contains(disk.CurrentPositionVector))
                {
                    disk.ShouldMoveToWasFromPlayerGoalEnd = false;
                    disk.ShouldMoveTo = FindClosestFreeSpace(false, disk.CurrentPositionVector);
                }
            }
        }
        private List<Transform> triedGroups = new List<Transform>();
        private List<Vector3> triedPositions = new List<Vector3>();
        /// <summary>
        /// Finds an returns the closest most suitable position to move a disc to
        /// </summary>
        /// <param name="playerSide">is the disc in the player1 goal?</param>
        /// <param name="currentPos">what is the position of the disc now</param>
        /// <returns>free position for disc</returns>
        public Vector3 FindClosestFreeSpace(bool playerSide, Vector3 currentPos)
        {
            //store a temp to use for reroll, allowing us to keep the current pos for group search upon reroll
            Vector3 tempPos = currentPos;
        reroll:
            bool fail = true;
            float closestGroup = Mathf.Infinity;
            Vector3 bestPos = Vector3.zero;
            float closest = Mathf.Infinity;
            float dist;
            Transform bestGroup = playerSide ? FreeSpacesPlayerParent.GetChild(0) : FreeSpacesOpponentParent.GetChild(0);
            if (playerSide)
            {
                for (int i = 0; i < FreeSpacesPlayerParent.childCount; i++)
                {
                    Transform t = FreeSpacesPlayerParent.GetChild(i);
                    if (triedGroups.Contains(t))
                        continue;
                    Vector3 pos = t.position;

                    dist = Vector3.Distance(currentPos, pos);
                    if (dist < closestGroup)
                    {
                        closestGroup = dist;
                        bestGroup = t;
                    }
                }

                for (int i = 0; i < bestGroup.childCount; i++)
                {
                    Vector3 pos = bestGroup.GetChild(i).position;

                    if (triedPositions.Contains(pos))
                        continue;

                    bool c = false;

                    for (int x = 0; x < DiskPlayerDatas.Count; x++)
                    {
                        if (DiskPlayerDatas[x].ShouldMoveTo == (Vector2)pos)
                            c = true;
                    }

                    if (c)
                        continue;

                    dist = Vector3.Distance(tempPos, pos);
                    if (dist < closest)
                    {
                        closest = dist;
                        bestPos = pos;
                        fail = false;
                    }
                }
            }
            else
            {
                for (int i = 0; i < FreeSpacesOpponentParent.childCount; i++)
                {
                    Transform t = FreeSpacesOpponentParent.GetChild(i);
                    if (triedGroups.Contains(t))
                        continue;
                    Vector3 pos = t.position;

                    dist = Vector3.Distance(currentPos, pos);
                    if (dist < closestGroup)
                    {
                        closestGroup = dist;
                        bestGroup = t;
                    }
                }

                for (int i = 0; i < bestGroup.childCount; i++)
                {
                    Vector3 pos = bestGroup.GetChild(i).position;

                    if (triedPositions.Contains(pos))
                        continue;

                    bool c = false;

                    for (int x = 0; x < DiskPlayerDatas.Count; x++)
                    {
                        if (DiskPlayerDatas[x].ShouldMoveTo == (Vector2)pos)
                            c = true;
                    }

                    if (c)
                        continue;

                    dist = Vector3.Distance(tempPos, pos);
                    if (dist < closest)
                    {
                        closest = dist;
                        bestPos = pos;
                        fail = false;
                    }
                }
            }

            if (fail)
            {
                Debug.Log("FAIL");
                triedGroups.Add(bestGroup);
                tempPos = currentPos;
                goto reroll;
            }

            if (Physics.CheckSphere(bestPos, CHECK_SIZE, LayerMask))
            {
                tempPos = bestPos;
                triedPositions.Add(bestPos);
                goto reroll;
            }

            triedGroups.Clear();
            triedPositions.Clear();
            return bestPos;
        }

        /// <summary>
        /// called when a disk is within a goal zone. Force moves the disk to the best position
        /// </summary>
        /// <param name="disk">the disk to be moved forcefully</param>
        private void ForceMove(DiskPlayerData disk)
        {
            if (!FST_BallManager.Instance.IsBallStopped || !m_AllStopped)
                return;

            disk.GetCollider.enabled = false;
            disk.GetRigidbody.velocity = Vector3.zero;
            disk.GetRigidbody.angularVelocity = Vector3.zero;

            Vector3 pos = disk.ShouldMoveTo;

            for (int x = 0; x < DiskPlayerDatas.Count; x++)
            {
                if (DiskPlayerDatas[x] == disk)
                    continue;

                if ((Vector2) pos == DiskPlayerDatas[x].ShouldMoveTo) 
                    pos = FindClosestFreeSpace(disk.ShouldMoveToWasFromPlayerGoalEnd, DiskPlayerDatas[x].ShouldMoveTo);
            }

            pos.z = -0.5f;

            disk.Transform.position = Vector3.MoveTowards(disk.Transform.position, pos, 0.2f);
            disk.Transform.rotation = Quaternion.Euler(disk.Transform.eulerAngles.x, -90f, -90f);

            if (Vector2.Distance(disk.Transform.position, pos) < 0.05f)
            {
                disk.GetCollider.enabled = true;
                disk.ShouldMoveTo = Vector2.zero;
            }
        }

        private void Glow()
        {
            if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.NotStarted)
                return;

            if (isUpGlow)
            {
                curGlowIntensity += Time.deltaTime * m_GlowSpeed;
                if (curGlowIntensity >= 0.2f)
                    isUpGlow = false;
            }
            else
            {
                curGlowIntensity -= Time.deltaTime * m_GlowSpeed;
                if (curGlowIntensity <= 0)
                    isUpGlow = true;
            }
            //NOTE: Timebar check removed as requested, but leave until further testing and feedback.
            if (GlobalGameManager.IsMastersTurn)
            {
                // if (GlobalGameManager.SharedInstance.Player1TimeBar.fillAmount < 0.5f)
                //  {
                foreach (Renderer _renderer in PlayerGoalMats)
                {
                    if (_renderer && _renderer.material)
                    {
                        //  float emissionAmount = Mathf.PingPong(Time.time, k_GlowControl);
                        Color baseColor = Color.yellow;

                        Color finalColor = baseColor * Mathf.LinearToGammaSpace(/*emissionAmount*/curGlowIntensity);

                        _renderer.material.SetColor("_EmissionColor", finalColor);// glow
                    }
                }
                //  }
                //   else { curGlowIntensity = 0; isUpGlow = true; }


                foreach (Renderer _renderer in OpponentGoalMats)
                    if (_renderer)
                        if (_renderer.material)
                            _renderer.material.SetColor("_EmissionColor", Color.black); //stop glow
            }
            else
            {
                foreach (Renderer _renderer in PlayerGoalMats)
                    if (_renderer)
                        if (_renderer.material)
                            _renderer.material.SetColor("_EmissionColor", Color.black); //stop glow

                //  if (GlobalGameManager.SharedInstance.Player2TimeBar.fillAmount < 0.5f)
                //  {
                foreach (Renderer _renderer in OpponentGoalMats)
                {
                    if (_renderer && _renderer.material)
                    {
                        //  float emissionAmount = Mathf.PingPong(Time.time, k_GlowControl);
                        Color baseColor = Color.yellow;

                        Color finalColor = baseColor * Mathf.LinearToGammaSpace(/*emissionAmount*/curGlowIntensity);

                        _renderer.material.SetColor("_EmissionColor", finalColor);// glow

                    }
                }
                // }
                // else { curGlowIntensity = 0; isUpGlow = true; }
            }

            //for particles...
            /* if (GlobalGameManager.IsMastersTurn)
             {
                 if (!PlayerGoalGlow.isPlaying)
                     PlayerGoalGlow.Play();
                 if (OpponentGoalGlow.isPlaying)
                 {
                     OpponentGoalGlow.Stop();
                     OpponentGoalGlow.Clear(true);
                 }
             }
             else
             {
                 if (PlayerGoalGlow.isPlaying)
                 {
                     PlayerGoalGlow.Stop();
                     PlayerGoalGlow.Clear();
                 }
                 if (!OpponentGoalGlow.isPlaying)
                     OpponentGoalGlow.Play();
             }*/
        }

        #endregion

        private void WriteQueue()
        {
          //  if (m_AllStopped)
           // {
               // queue.Reset();
              //  return;
           // }
            int count = NetPlayerDatas.Count;

            //update latest for disks
            latestPositions = new Vector2[count];
            latestDiskTopYAngles = new float[count];
            latestVelocities = new Vector2[count];

            for (int i = 0; i < count; i++)
            {
                DiskPlayerData d = NetPlayerDatas[i];
                latestPositions[i] = new Vector2(d.Transform.position.x, d.Transform.position.y);
                latestDiskTopYAngles[i] = d.DiskTop.localRotation.eulerAngles.y;
                latestVelocities[i] = d.GetRigidbody.velocity;
            }

            //update latest for ball
            latestBallPos = Ball.transform.position;
            latestBallRot = Ball.transform.rotation;
            latestBallVelocity = Ball.velocity;

            //send to remote/client converted to bytes
            queue.SendNext(latestBallPos);
            queue.SendNext(latestBallRot);
            queue.SendNext(latestBallVelocity);

            // here we send off the positions at the frame we call this function.
            queue.SendNext(latestPositions);
            queue.SendNext(latestDiskTopYAngles);
            queue.SendNext(latestVelocities);


            //if (selections == null)
            //    selections = new bool[NetPlayerDatas.Count];

            //for (int i = 0; i < NetPlayerDatas.Count; i++)
            //{
            //    selections[i] = NetPlayerDatas[i].SelectionCircle.enabled;
            //}
            //    queue.SendNext(selections);
        }

        bool hasData = false;
        private void ReadQueue()
        {
            if (!queue.HasQueuedObjects() || !hasData)
                return;

            try
            {
                // Network player, receive data
                latestBallPos = (Vector2)queue.ReceiveNext();
                latestBallRot = (Quaternion)queue.ReceiveNext();
                latestBallVelocity = (Vector2)queue.ReceiveNext();

                latestPositions = (Vector2[])queue.ReceiveNext();
                latestDiskTopYAngles = (float[])queue.ReceiveNext();
                latestVelocities = (Vector2[])queue.ReceiveNext();
                // selections = (bool[])stream.ReceiveNext();

                int count = NetPlayerDatas.Count;

                positionsAtLastPacket = new Vector2[count];
                diskTopYRotationsAtLastPacket = new float[count];
                velocitiesAtLastPacket = new Vector2[count];

                for (int i = 0; i < count; i++)
                {
                    DiskPlayerData d = NetPlayerDatas[i];
                    positionsAtLastPacket[i] = new Vector2(d.Transform.position.x, d.Transform.position.y);
                    diskTopYRotationsAtLastPacket[i] = d.DiskTop.localRotation.eulerAngles.y;
                    velocitiesAtLastPacket[i] = d.GetRigidbody.velocity;
                }

                ballPosAtLastPacket = new Vector2(Ball.transform.position.x, Ball.transform.position.y);
                ballRotAtLastPacket = Ball.transform.rotation;

            }
            catch
            {
                if (queue.HasQueuedObjects())
                {
                    Debug.Log("QUEUE BUGGED OUT! resetting queue...");
                    queue.Reset();
                }
            }
        }

        public void TransmitShootHelperActive(bool active, bool transmit = true)
        {
            if (!PhotonNetwork.InRoom)
                return;
            isShootHelperActive = active;
            photonView.RPC("RPC_ReceiveShootHelperActive", RpcTarget.Others, isShootHelperActive, (Vector2)ShootCircle.position, ArrowPlane.localRotation.eulerAngles.z, (Vector2)ArrowPlane.localScale, (Vector2)ShootCircleRenderer.transform.localScale);
        }
        [PunRPC]
        public void RPC_ReceiveShootHelperActive(bool active, Vector2 shootCirclePos, float arrowZRot, Vector2 arrowScale, Vector2 circleScale )
        {
            isShootHelperActive = active;
            shootCirclePosition = shootCirclePos;
            arrowPlaneZRotation = arrowZRot;
            arrowPlaneScale = arrowScale;
            shootCircleScale = circleScale;
            PlayerController.Instance.SetDotsActive(active);
        }

        /// <summary>
        /// IMPORTANT, After kick only, and only on recieving device (we use the received kick rpc, so we can turn it off here with no extra sending of data)
        /// </summary>
        public void DisableShootHelperAfterKick()
        {
            isShootHelperActive = false;
            PlayerController.Instance.SetDotsActive(false);
        }

        private float m_NextAllowedHit = 0;
        /// </summary>
        /// <param name="objectPosition"></param>
        public void TransmitCollison(int index, Vector3 objectPosition, Vector3 contactPosition, Vector2 velocity)
        {
            if (m_NextAllowedHit < Time.time)
            {
                //   Debug.Log("TransmitCollison!");
                m_NextAllowedHit = Time.time + 0.005f;
                photonView.RPC("RPC_ReceiveCollision", RpcTarget.Others, index, objectPosition, contactPosition, velocity);
            }
        }

     //   double lastColTimeStampDisks = -1;
        [PunRPC]
        private void RPC_ReceiveCollision(int index, Vector3 pos, Vector3 contactPos, Vector2 velocity, PhotonMessageInfo info)
        {
          //  lastColTimeStampDisks = info.SentServerTime;

            if (latestPositions == null || latestPositions.Length != NetPlayerDatas.Count)
            {
                latestPositions = new Vector2[NetPlayerDatas.Count];
                for (int i = 0; i < NetPlayerDatas.Count; i++)
                    latestPositions[i] = NetPlayerDatas[i].Transform.position;
            }

            if (positionsAtLastPacket == null || positionsAtLastPacket.Length != NetPlayerDatas.Count)
            {
                positionsAtLastPacket = new Vector2[NetPlayerDatas.Count];
                for (int i = 0; i < NetPlayerDatas.Count; i++)
                    positionsAtLastPacket[i] = NetPlayerDatas[i].Transform.position;
            }

          //  float _lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
            for (int i = 0; i < NetPlayerDatas.Count; i++)
            {
                if (i == index)
                {
                    FST_ParticlePooler.Instance.DiskHit(NetPlayerDatas[i].Transform, contactPos);

                    if (useCollisionPrediction)
                    {
                        NetPlayerDatas[i].DidCollide = true;
                        latestVelocities[i] = velocity;
                        positionsAtLastPacket[i] = pos;
                        latestPositions[i] = pos;
                        //apply prediction
                      //  positionsAtLastPacket[i] += velocity * _lag * Time.fixedDeltaTime;
           
                    }

                    break;
                }
            }
        }

        public void TransmitShot(Transform disk, Vector3 force)
        {
            //   Debug.Log("TransmitShot!");
            photonView.RPC("RPC_ReceiveShot", RpcTarget.AllViaServer, GetDiskIdByTransform(disk), force);
        }

        [PunRPC]
        private void RPC_ReceiveShot(int index, Vector3 force)
        {
            GetDiskByID(index).GetRigidbody.AddForce(force, ForceMode.Impulse);
        }

        public void TransmitBallCollison(Vector3 objectPosition, Vector3 contactPos, Vector2 velocity)
        {
            photonView.RPC("RPC_ReceiveBallCollision", RpcTarget.Others, objectPosition, contactPos, velocity);
        }

        public void TransmitAllPositions()
        {
            Debug.Log("TransmitAllPositions");
            FST_MPDebug.Log("TransmitAllPositions");
            ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
            {
                [0] = latestBallPos,
                [1] = latestBallRot,
                [2] = latestBallVelocity,
                [3] = latestPositions,
                [4] = latestDiskTopYAngles,
                [5] = latestVelocities
            };

            photonView.RpcSecure("RPC_ReceiveAllPositions", RpcTarget.Others, false, ht);
        }


        [PunRPC]
        private void RPC_ReceiveAllPositions(ExitGames.Client.Photon.Hashtable ht)
        {
            Debug.Log("RPC_ReceiveAllPositions");
            FST_MPDebug.Log("RPC_ReceiveAllPositions");

            ballPosAtLastPacket = latestBallPos = (Vector2)ht[0];
            Ball.transform.position = new Vector3(latestBallPos.x, latestBallPos.y, Ball.transform.position.z);
            Ball.transform.rotation = ballRotAtLastPacket = latestBallRot = (Quaternion)ht[1];
            Ball.velocity = latestBallVelocity = (Vector2)ht[2];

            positionsAtLastPacket = latestPositions = (Vector2[])ht[3];
            diskTopYRotationsAtLastPacket = latestDiskTopYAngles = (float[])ht[4];
            velocitiesAtLastPacket = latestVelocities = (Vector2[])ht[5];

            for (int i = 0; i < NetPlayerDatas.Count; i++)
            {
                DiskPlayerData d = NetPlayerDatas[i];
                d.DidCollide = false;

                //apply recieved velocity
                d.GetRigidbody.velocity = latestVelocities[i];

                //apply the recieved position to the disk
                d.Transform.position = new Vector3(latestPositions[i].x, latestPositions[i].y, d.Transform.position.z);

                d.DiskTop.localRotation = Quaternion.Euler(latestDiskTopYAngles[i] * Vector3.up);
            }

            //PhotonNetwork.OpCleanRpcBuffer(photonView);
        }

        //   private readonly List<FST_GhostBall> ghostBalls = new List<FST_GhostBall>();
        //  double lastColTimeStampBall = -1;
        //   private Transform ghostPool = null;
        [PunRPC]
        private void RPC_ReceiveBallCollision(Vector3 pos, Vector3 contactPos, Vector2 velocity, PhotonMessageInfo info)
        {
        //    lastColTimeStampBall = info.SentServerTime;

            FST_ParticlePooler.Instance.BallHitWallDust(contactPos);

            //if(!ghostPool)
            //{
            //    ghostPool = new GameObject("GhostPool").transform;
            //    BallGhost.transform.SetParent(ghostPool);
            //}

            //if (ghostBalls.Count < 1)
            //    ghostBalls.Add(BallGhost);

            //for (int i = 0; i < ghostBalls.Count; i++)
            //{
            //    FST_GhostBall ghostBall = ghostBalls[i];
            //    if (ghostBall.isActiveAndEnabled)
            //    {
            //        ghostBall = Instantiate(BallGhost);
            //        ghostBall.transform.SetParent(ghostPool);
            //    }
            //    else
            //    {
            //        ghostBall.Ghost(pos, Ball.transform.position);
            //        break;
            //    }
            //}
            BallGhost.Ghost(pos/*, Ball.transform.position*/);
            if (!useCollisionPrediction)
                return;

           // float _lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));

            //  Debug.Log("COLLISION BALL LAG = " + _lag);

            ballPosAtLastPacket = pos;
            latestBallPos = pos;
            Ball.velocity = velocity;
            //apply prediction
           // ballPosAtLastPacket += velocity * _lag * Time.fixedDeltaTime;
        }

        public void TransferOwnerShip(Photon.Realtime.Player player)
        {
            if (photonView.Owner == player)
                return;

            if (player == null)
                Debug.Log("No player to change ownership to!");
            else photonView.TransferOwnership(player);

            for (int i = 0; i < DiskPlayerDatas.Count; i++)
                DiskPlayerDatas[i].ShouldMoveTo = Vector2.zero;

            //NOTE: this maybe needed again later
           // StartCoroutine(Transfer(player));
        }
        //NOTE: this maybe needed again later
        private IEnumerator Transfer(Photon.Realtime.Player player)
        {
            yield return new WaitUntil(() => AllMovingObjectsHaveStopped());
            //TransmitAllPositions();
            if (player == null)
                Debug.Log("No player to change ownership to!");
            else photonView.TransferOwnership(player);

            //NOTE: maybe include this in handover
            for (int i = 0; i < DiskPlayerDatas.Count; i++)
                DiskPlayerDatas[i].ShouldMoveTo = Vector2.zero;
        }
       // double lastDeserialiseTimeStamp = 0f;
       // float lag;
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (!FST_Gameplay.IsMultiplayer)
                return;

            if (stream.IsWriting)
            {
                hasData = false;
                queue.Serialize(stream);
            }
            else
            {
                //  Lag compensation
                //  lag = Mathf.Abs((float)(PhotonNetwork.Time - info.SentServerTime));
                //  usage > position += (velocity * lag);
                //  Debug.Log(lag);

                currentTime = 0.0f;
                lastPacketTime = currentPacketTime;
                currentPacketTime = info.SentServerTime;

                queue.Deserialize(stream);

                //lastDeserialiseTimeStamp = info.SentServerTime;

                hasData = true;
            }
        }

        private void RunDebugGUICheck()
        {

#if UNITY_ANDROID && !UNITY_EDITOR && DEBUG
            if (Input.touchCount > 5)
            {
                useCollisionPrediction = !useCollisionPrediction;
                SSTools.ShowMessage("Collision Prediction is: " + (useCollisionPrediction ? "ON" : "OFF"), SSTools.Position.bottom, SSTools.Time.twoSecond);
            }
#endif

            if (Input.GetKeyUp(KeyCode.Keypad0))
                drawDebug = !drawDebug;

            if (drawDebug)
            {
                if (Input.GetKeyUp(KeyCode.W))
                    GlobalGameManager.Instance.GoalStatus(true);
                if (Input.GetKeyUp(KeyCode.E))
                    GlobalGameManager.Instance.GoalStatus(false);

                if (Input.GetKeyUp(KeyCode.M))
                    MovementInterpolationByPing = !MovementInterpolationByPing;
                if (Input.GetKeyUp(KeyCode.R))
                    RotationInterpolationByPing = !RotationInterpolationByPing;
                if (Input.GetKeyUp(KeyCode.Space))
                {
                    useCollisionPrediction = !useCollisionPrediction;

                    SSTools.ShowMessage("Collision Prediction is: " + (useCollisionPrediction ? "ON" : "OFF"), SSTools.Position.bottom, SSTools.Time.twoSecond);
                }

                if (Input.GetKeyUp(KeyCode.KeypadPlus))
                    fakeMoveSpeed += 1;
                if (Input.GetKeyUp(KeyCode.KeypadMinus))
                    fakeMoveSpeed -= 1;
                if (Input.GetKeyUp(KeyCode.Keypad8))
                    fakeRotSpeed += 1;
                if (Input.GetKeyUp(KeyCode.Keypad2))
                    fakeRotSpeed -= 1;
            }
        }

        private bool drawDebug = false;
#if DEBUG
        private void OnGUI()
        {
            float posy = Screen.height;

            posy -= 20;

            if (!drawDebug)
            {
                GUI.Label(new Rect(30, posy, 500, 30), string.Format("Press 0 (numPad) for debug options", fakeMoveSpeed));
                return;
            }

            if (!MovementInterpolationByPing)
            {
                GUI.Label(new Rect(30, posy, 500, 30), string.Format("Fake Movement buff = {0} (+ or - numPad)", fakeMoveSpeed));
                posy -= 15;
            }
            if (!RotationInterpolationByPing)
            {
                GUI.Label(new Rect(30, posy, 500, 30), string.Format("Fake Rotations buff = {0} (8 or 2 numPad)", fakeRotSpeed));
                posy -= 15;
            }

            GUI.Label(new Rect(30, posy, 500, 30), (MovementInterpolationByPing ? "AccurateMovementsByPing" : "FakedMovements") + " (M key) ");
            posy -= 15;
            GUI.Label(new Rect(30, posy, 500, 30), (RotationInterpolationByPing ? "AccurateRotationsByPing" : "FakedRotations") + " (R key) ");
            posy -= 15;
            GUI.Label(new Rect(30, posy, 500, 30), (useCollisionPrediction ? "Using Collision Prediction" : "NOT Using Collision Prediction") + " (Spacebar key) ");
            posy -= 15;
            GUI.Label(new Rect(30, posy, 500, 30), ("Score Goal Left or Right (W or E key)"));
        }
#endif
#if UNITY_EDITOR
        private void DrawDebugBoundary(Vector4 bounds, Vector2 goalBottomAndTop, Vector2 leftAndRightGoalBacks, bool ball)
        {
            Color c = ball ? Color.red : Color.blue;

            int h = -5;
            //top 
            Debug.DrawLine(new Vector3(bounds.w, bounds.x, h), new Vector3(bounds.y, bounds.x, h), c);
            //top right
            Debug.DrawLine(new Vector3(bounds.y, bounds.x, h), new Vector3(bounds.y, goalBottomAndTop.y, h), c);
            //bottom right
            Debug.DrawLine(new Vector3(bounds.y, bounds.z, h), new Vector3(bounds.y, goalBottomAndTop.x, h), c);
            //bottom
            Debug.DrawLine(new Vector3(bounds.y, bounds.z, h), new Vector3(bounds.w, bounds.z, h), c);
            //bottom left
            Debug.DrawLine(new Vector3(bounds.w, bounds.z, h), new Vector3(bounds.w, goalBottomAndTop.x, h), c);
            //top left
            Debug.DrawLine(new Vector3(bounds.w, bounds.x, h), new Vector3(bounds.w, goalBottomAndTop.y, h), c);

            //top left goal
            Debug.DrawLine(new Vector3(bounds.w, goalBottomAndTop.y, h), new Vector3(leftAndRightGoalBacks.x, goalBottomAndTop.y, h), c);
            //top right goal
            Debug.DrawLine(new Vector3(bounds.y, goalBottomAndTop.y, h), new Vector3(leftAndRightGoalBacks.y, goalBottomAndTop.y, h), c);

            //bottom left goal
            Debug.DrawLine(new Vector3(bounds.w, goalBottomAndTop.x, h), new Vector3(leftAndRightGoalBacks.x, goalBottomAndTop.x, h), c);
            //bottom right goal
            Debug.DrawLine(new Vector3(bounds.y, goalBottomAndTop.x, h), new Vector3(leftAndRightGoalBacks.y, goalBottomAndTop.x, h), c);

            //left goal back
            Debug.DrawLine(new Vector3(leftAndRightGoalBacks.x, goalBottomAndTop.x, h), new Vector3(leftAndRightGoalBacks.x, goalBottomAndTop.y, h), c);
            //right goal back
            Debug.DrawLine(new Vector3(leftAndRightGoalBacks.y, goalBottomAndTop.x, h), new Vector3(leftAndRightGoalBacks.y, goalBottomAndTop.y, h), c);
        }
#endif

        readonly float distLimiter = 0.45f;
        private bool IsCloseToAnything(Collider c)
        {
            float dist = Mathf.Infinity;

            if (c != FST_BallManager.Instance.GetCollider)
                dist = Vector2.Distance(FST_BallManager.Instance.GetCollider.ClosestPoint(c.transform.position), c.ClosestPoint(Ball.transform.position));

            if (dist < distLimiter)
                return true;

            for (int i = 0; i < NetPlayerDatas.Count; i++)
            {
                if (c == NetPlayerDatas[i].GetCollider)
                    continue;

                dist = Vector2.Distance(NetPlayerDatas[i].GetCollider.ClosestPoint(c.transform.position), c.ClosestPoint(NetPlayerDatas[i].Transform.position));

                if (dist < distLimiter)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Applys the physical properties contained in FST_PhysicsData object
        /// </summary>
        private void ApplyPhysicsParameters()
        {
            GetPhysicsData.GroundPhysMat.staticFriction = GetPhysicsData.GroundStaticFriction;
            GetPhysicsData.GroundPhysMat.dynamicFriction = GetPhysicsData.GroundDynamicFriction;
            GetPhysicsData.GroundPhysMat.bounciness = GetPhysicsData.GroundBounciness;
            GetPhysicsData.GroundPhysMat.bounceCombine = GetPhysicsData.GroundBounceCombineMode;
            GetPhysicsData.GroundPhysMat.frictionCombine = GetPhysicsData.GroundFrictionCombineMode;

            GetPhysicsData.BorderPhysMat.staticFriction = GetPhysicsData.BorderStaticFriction;
            GetPhysicsData.BorderPhysMat.dynamicFriction = GetPhysicsData.BorderDynamicFriction;
            GetPhysicsData.BorderPhysMat.bounciness = GetPhysicsData.BorderBounciness;
            GetPhysicsData.BorderPhysMat.bounceCombine = GetPhysicsData.BorderBounceCombineMode;
            GetPhysicsData.BorderPhysMat.frictionCombine = GetPhysicsData.BorderFrictionCombineMode;

            for (int i = 0; i < DiskPlayerDatas.Count; i++)
            {
                //quick ref
                DiskPlayerData disk = DiskPlayerDatas[i];

                //rigid props
                disk.GetRigidbody.drag = GetPhysicsData.DiskDrag;
                disk.GetRigidbody.mass = GetPhysicsData.DiskMass;
                disk.GetRigidbody.angularDrag = GetPhysicsData.DiskAngularDrag;
                disk.GetRigidbody.maxAngularVelocity = GetPhysicsData.DiskMaxAngularVelocity;
                if (!disk.GetRigidbody.isKinematic)//just a check for the sake of warnings when forcing kinematic
                    disk.GetRigidbody.collisionDetectionMode = GetPhysicsData.DiskCollisionDetectionMode;
                //material props //note we could use shared material, it will be more optimal, but in the case we want some custom disk, ie back field/goal keeper...
                disk.GetCollider.material.bounceCombine = GetPhysicsData.DiskBounceCombineMode;
                disk.GetCollider.material.frictionCombine = GetPhysicsData.DiskFrictionCombineMode;
                disk.GetCollider.material.bounciness = GetPhysicsData.DiskBounciness;
                disk.GetCollider.material.staticFriction = GetPhysicsData.DiskStaticFriction;
                disk.GetCollider.material.dynamicFriction = GetPhysicsData.DiskDynamicFriction;
            }
        }
    }
}
