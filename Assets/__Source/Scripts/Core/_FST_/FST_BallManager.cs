/////////////////////////////////////////////////////////////////////////////////
//
//  FST_BallManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class controls ball collision with Goal triggers and gatePoles,
//                  and also stops the ball when the speed is too low.
//
/////////////////////////////////////////////////////////////////////////////////

//using System.Collections.Generic;
using UnityEngine;

namespace FastSkillTeam
{
    public class FST_BallManager : MonoBehaviour
    {
        public static FST_BallManager Instance;

        #region Inspector Vars
#pragma warning disable CS0649
        [SerializeField] private Collider m_FieldBoundsCollider;
        [SerializeField] LayerMask BorderLayer;
#pragma warning restore CS0649
        public TrailRenderer TrailRenderBall;
        public BallShadow Ball_Shadow_DiskRePosition;
        public Collider BorderTopCollider, BorderBottomCollider, BorderTopLeftCornerCollider, BorderTopRightCornerCollider, BorderBottomLeftCornerCollider, BorderBottonRightCornerCollider;

        #endregion

        #region Getters

        FST_PhysicsData m_FST_Physics = null;
        FST_PhysicsData FST_Physics { get { if (!m_FST_Physics) m_FST_Physics = Resources.Load("FST_PhysicsData") as FST_PhysicsData; return m_FST_Physics; } }

        public float TrailActiveValue { get; set; } = 8;
        //Note below has been made to make use of, not yet implemented ->FST
        private Vector3 m_LastPos = Vector3.positiveInfinity;// cache this so we can retrieve it no matter what ->FST 
        public Vector3 Position { get { if (m_LastPos != transform.position) m_LastPos = transform.position; return m_LastPos; } }//we can add a condition here to return cached pos first if other player drop and reconnect ->FST

        // The ball rigid body.
        private Rigidbody m_RigidBody = null;
        //many of getcomponent<rigidbody>() all should have pointed to this cached var, this has been fixed, an a getter made ->FST
        public Rigidbody GetRigidBody { get { if (!m_RigidBody) m_RigidBody = GetComponent<Rigidbody>(); return m_RigidBody; } }

        //Replace need for finding gamecontroller just for the oublic functions/vars from GlobalGameManager.cs ->FST
        private GlobalGameManager m_GlobalGm = null;
        private GlobalGameManager GlobalGm { get { if (!m_GlobalGm) m_GlobalGm = GlobalGameManager.Instance; return m_GlobalGm; } }

        // find scripting instances and store them
        private EnableManualGravitiy m_Enablegravi = null;
        private EnableManualGravitiy Enablegravi { get { if (!m_Enablegravi) m_Enablegravi = EnableManualGravitiy.Instance; return m_Enablegravi; } }

        private Collider[] m_AllBorderCols = null;
        private Collider[] AllBorderCols { get { if (m_AllBorderCols == null || m_AllBorderCols.Length < 1) m_AllBorderCols = new Collider[] { BorderTopCollider, BorderBottomCollider, BorderTopLeftCornerCollider, BorderTopRightCornerCollider, BorderBottomLeftCornerCollider, BorderBottonRightCornerCollider }; return m_AllBorderCols; } }

        private Collider m_Collider = null;
        public Collider GetCollider { get { if (!m_Collider) m_Collider = GetComponent<Collider>(); return m_Collider;  } }

        // private working index var
        private int TouchToBorder
        {
            get
            {
                int closest = 99;
                float lastDist = Mathf.Infinity;
                for (int i = 0; i < AllBorderCols.Length; i++)
                {
                    float dist = Vector3.Distance(AllBorderCols[i].ClosestPointOnBounds(transform.position), transform.position);
                    if (dist < lastDist)
                    {
                        lastDist = dist;
                        closest = i;
                    }
                }
                return closest;
            }
        }

        public bool IsBallStopped
        {
            get
            {
                GetRigidBody.angularDrag = FST_Physics.BallStopThreshold_AngularVelocity - Mathf.Clamp(GetRigidBody.velocity.magnitude, 0, FST_Physics.BallStopThreshold_AngularVelocity);

                Vector3 torque = transform.InverseTransformVector(GetRigidBody.angularVelocity);
                return GetRigidBody.velocity.magnitude < FST_Physics.BallStopThreshold_Velocity && GetRigidBody.angularVelocity.magnitude < FST_Physics.BallStopThreshold_AngularVelocity && torque.magnitude < FST_Physics.BallStopThreshold_Torque;
            }
        }

        #endregion

        #region private working vars

        // a readonly array to read from using index var, one time creation of string, not each update ->FST
        private readonly string[] r_BorderStrings = new string[] { "BorderTop", "BorderBottom", "BorderTopRightCorner", "BorderTopLeftCorner", "BorderBottomLeftCorner", "BorderBottonRightCorner" };//note check and repair this misleading misspell - BorderBottonRightCorner ->FST

        private string lastBallTouchBorder;
        private bool isBallinCurve; //made private - no need to be public ->FST

        private SphereCollider m_SphereCol = null;
        private SphereCollider Collider { get { if (!m_SphereCol) m_SphereCol = GetComponent<SphereCollider>(); return m_SphereCol; } }

        //NOTE: hook these to custom phys inspector WIP
        private Color trailPowerColour = new Color(1f, 0.792f, 0f);
        private float origTrailWidthMultiplier;

        #endregion

        #region MonoBehaviour CallBacks

        void Awake()
        {
            Instance = this;
            origTrailWidthMultiplier = TrailRenderBall.widthMultiplier;
            ApplyPhysicsParameters();
        }

#if UNITY_EDITOR
        //NOTE: we only run this in editor, this could eat up performance on mobile, its purely for tuning purposes
        private void Update()
        {
            ApplyPhysicsParameters();
        }
#endif

        private bool inGoal = false;
        void FixedUpdate()
        {
            float velMag = GetRigidBody.velocity.magnitude;

            Color c = Color.Lerp(Color.white, trailPowerColour, Time.fixedDeltaTime * velMag);
            TrailRenderBall.startColor = c;

            TrailRenderBall.widthMultiplier = origTrailWidthMultiplier + Time.fixedDeltaTime * velMag;

            if (TrailActiveValue > 7 && GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.GoalIntermission)
                StartTrailRender();
            else StopTrailRender();

            if (FST_Gameplay.IsMultiplayer && !FST_DiskPlayerManager.Instance.IsOwner)
                return;

            if (velMag > FST_Physics.BallMaxVelocity)
                GetRigidBody.velocity = GetRigidBody.velocity.normalized * FST_Physics.BallMaxVelocity;

            if (inGoal)
            {
                if (velMag > FST_Physics.BallStopThreshold_Velocity * 2)
                    GetRigidBody.velocity *= 0.25f;
                if (IsBallStopped)
                    inGoal = false;
            }

            //if we dont pass this rule return and dont execute any further ->FST
            if (!IsBallStopped)
                return;

            isBallinCurve = false;
            Enablegravi.IsForceAddContinuous = false;

            StopBall(); //new method ->FST

            //ref once to save repeating a for loop in the getter ->FST
            int index = TouchToBorder;
            // crazy or checks replaced ->FST
            if (index < 99)
                lastBallTouchBorder = r_BorderStrings[index];// note: Getter default returns 99 if no touch, so 99 = no touch ->FST
        }

        //for debug purposes
        //private List<float> avgs = new List<float>();
        //float[] AverageMaxMinCols()
        //{
        //    float max = 0;
        //    float min = float.MaxValue;
        //    float total = 0;
        //    int count = 0;
        //    if (avgs.Count > 0)
        //    {
        //        for (int i = 0; i < avgs.Count; i++)
        //        {
        //            if (avgs[i] > max)
        //                max = avgs[i];

        //            if (avgs[i] < min)
        //                min = avgs[i];

        //            total += avgs[i];
        //            count++;
        //        }
        //    }

        //    return new float[] { total == 0 ? 0 : total / count, max, min};
        //}
        //private Collider _last = null;
        private Collider m_FieldCollider;
        const float k_HardForce = 15f;//avg is 15 so we use it for checking above max, medium is just this halved...
        /// <summary>
        /// Check ball Enter Collision With Wall,corner,and Disk...
        /// Check Second Touch on Disk if yes then give some force
        /// check if ball touch the border and player take shot on ball then generate curve shot.
        /// </summary>
        void OnCollisionEnter(Collision other)
        {
            if (!m_FieldCollider)
            {
                if (other.gameObject.name == "Field")
                {
               //     Debug.Log("Got field Col");
                    m_FieldCollider = other.collider;
                }
            }

            if (other.collider == m_FieldCollider)
                return;

            //if (other.collider == _last)
            //    return;

            //_last = other.collider;

            //get reference first!!! all lines that read getcomponent  have been replaced with this quick cached var ->FST
            Rigidbody otherRB = other.transform.GetComponent<Rigidbody>();

            Vector2 velBall = GetRigidBody.velocity;

            float force = velBall.magnitude;//avg is around 15

            //  avgs.Add(force);

            //  Debug.Log("ball force: " + force + " / AVG = " + AverageMaxMinCols()[0] + " / MAX = " + AverageMaxMinCols()[1] + " / MIN = " + AverageMaxMinCols()[2]);

            //  float collisionAngle = Vector3.Angle(other.GetContact(0).normal, otherRB ? otherRB.velocity : lastFrameVelocity);

            //  Debug.Log("collisionAngleBall: " + collisionAngle);

            if (force > k_HardForce / 2)
            {
                FST_ParticlePooler.Instance.BallHitWallDust(other.GetContact(0).point);

              if(FST_Gameplay.IsMultiplayer)
                //if (collisionAngle > 70)
                FST_DiskPlayerManager.Instance.TransmitBallCollison(transform.position, other.GetContact(0).point, velBall);
            }

            switch (other.gameObject.tag)
            {
                case "corner":
                 //   Debug.Log("ball to corner force = " + force);
                    FST_AudioManager.Instance.PlayAudio(force > k_HardForce ? FST_AudioManager.AudioID.SFX_BallToWall_Hard : force > k_HardForce / 2 ? FST_AudioManager.AudioID.SFX_BallToWall_Medium : FST_AudioManager.AudioID.SFX_BallToWall_Soft);
                  //  Vector3 impactVector = other.contacts[0].normal * force * FST_Physics.BallInCornerForceMultiplier;
                  //  GetRigidBody.AddForce(new Vector3(-impactVector.x, -impactVector.y, 0));
                    break;

                case "Border":
                    //  Debug.Log("ball to wall force = " + force);
                    FST_AudioManager.Instance.PlayAudio(force > k_HardForce ? FST_AudioManager.AudioID.SFX_BallToWall_Hard : force > k_HardForce / 2 ? FST_AudioManager.AudioID.SFX_BallToWall_Medium : FST_AudioManager.AudioID.SFX_BallToWall_Soft);
                    CheckBallInCurve();
                    break;

                case "Player":
                    //NOTE: audio triggered by disc
                    SetBallParams(other.gameObject.name);
                    CheckBallInCurve();
                    CheckForceAndCurveActive(other, otherRB);
                    break;

                case "Player_2":
                    //NOTE: audio triggered by disc
                    SetBallParams(other.gameObject.name);
                    CheckBallInCurve();
                    CheckForceAndCurveActive(other, otherRB);
                    break;
                case "Opponent":
                    //NOTE: audio triggered by disc
                    SetBallParams(other.gameObject.name);
                    CheckBallInCurve();
                    CheckForceAndCurveActive(other, otherRB);
                    break;
            }

            //cheat reflections option
         //   if (lastFrameVelocity.magnitude > 0.01f)
            //    Bounce(other.GetContact(0).normal);
        }
        //private void Bounce(Vector3 collisionNormal)
        //{
        //    var speed = lastFrameVelocity.magnitude;
        //    var direction = Vector3.Reflect(lastFrameVelocity.normalized, collisionNormal);

        //    Debug.Log("Out Direction: " + direction);
        //    GetRigidBody.velocity = direction * Mathf.Max(speed, 0.001f);
        //}
        /// <summary>
        /// Check ball is in goal zone 
        /// </summary>
        void OnTriggerEnter(Collider other)//NOTE: this can be handled by extending the bounds code a little in FST_DiskPlayerManager.cs, which will gain us performance.. TODO
        {
            if (FST_Gameplay.IsMultiplayer && !FST_DiskPlayerManager.Instance.IsOwner)
                return;

            if (other == m_FieldCollider)
                return;

            //if (other == _last)
            //    return;

            //_last = other;

            string otherTag = other.gameObject.tag;

            switch (otherTag)
            {
                case "opponentGoalTrigger":
                    inGoal = true;
                    GlobalGm.GoalStatus(false);
                    break;

                case "playerGoalTrigger":
                    inGoal = true;
                    GlobalGm.GoalStatus(true);
                    break;

                case "gatePost":
                    float force = GetRigidBody.velocity.magnitude * Time.fixedDeltaTime;
                //    Debug.Log("ball to gatepost force = " + force);
                    FST_AudioManager.Instance.PlayAudio(force > 0.25f ? FST_AudioManager.AudioID.SFX_BallToPost_Hard : force > 0.15f ? FST_AudioManager.AudioID.SFX_BallToPost_Medium : FST_AudioManager.AudioID.SFX_BallToPost_Soft);
                    break;
            }
        }

        #endregion

        #region Methods


        /// <summary>
        /// Applys the physical properties contained in FST_PhysicsData object
        /// </summary>
        private void ApplyPhysicsParameters()
        {
            //rigid props
            GetRigidBody.drag = FST_Physics.BallDrag;
            GetRigidBody.angularDrag = FST_Physics.BallAngularDrag;
            GetRigidBody.mass = FST_Physics.BallMass;
            GetRigidBody.maxAngularVelocity = FST_Physics.BallMaxAngularVelocity;

            //material props
            Collider.material.bounceCombine = FST_Physics.BallBounceCombineMode;
            Collider.material.frictionCombine = FST_Physics.BallFrictionCombineMode;
            Collider.material.staticFriction = FST_Physics.BallStaticFriction;
            Collider.material.dynamicFriction = FST_Physics.BallDynamicFriction;
            Collider.material.bounciness = FST_Physics.BallBounciness;
        }

        /// <summary>
        /// Stops and prepares the ball and curve shot button for its next hit
        /// </summary>
        public void StopBall()
        {
            // Should the curve shot be active?
            UIManager.Instance.curveLoftBtn.interactable = Physics.CheckSphere(transform.position, FST_Physics.CurveShotActivationRange, BorderLayer);        //  TouchToBorder < 99;
            //   Debug.LogFormat("StopBall({0}),TouchToBorder < 99");

            TrailRenderBall.enabled = false;
            GetRigidBody.velocity = Vector3.zero;
            GetRigidBody.angularVelocity = Vector3.zero;
        }

        /// <summary>
        /// This method should be called for penalty kicks
        /// </summary>
        /// <param name="penaltyKickBallPosition">the postion to move the ball to</param>
        public void PenaltyBallMove(Vector3 penaltyKickBallPosition)
        {
            StopBall();
            transform.position = penaltyKickBallPosition;  //GO TO PENALTY POSITION!
        }

        public void StopTrailRender()
        {
            if (TrailRenderBall.enabled)
                TrailRenderBall.enabled = false;
        }

        public void StartTrailRender()
        {
            if (!TrailRenderBall.enabled)
                TrailRenderBall.enabled = true;
        }

        private void CheckBallInCurve()
        {
            if (isBallinCurve)
            {
                GetRigidBody.angularVelocity = GetRigidBody.velocity * 2f;
                isBallinCurve = false;
            }
        }

        private bool SetBallParams(string name)
        {//NOTE: we may make this happed one frame late, to get some better phys sim
            if (GlobalGm.CurrentPlayerName != name)
            {
                GetRigidBody.velocity = GetRigidBody.velocity * FST_Physics.DiskToBallForceMultiplier;
                return true;
            }
            return false;
        }

        private void CheckForceAndCurveActive(Collision other, Rigidbody otherRB)
        {
            if (Enablegravi.IsForceAddContinuous)
                Enablegravi.StopGiveForce();

            if (otherRB.velocity != Vector3.zero && UIManager.Instance.IsCurveActive)
            {
                UIManager.Instance.curveLoftBtn.interactable = false;
                UIManager.Instance.CurveShotDeActive();

                float addForcex = otherRB.velocity.magnitude * FST_Physics.CurveShotForceMultiplier;
                Vector3 ballDir = other.contacts[0].point - transform.position;
                ballDir.x *= -3;
                ballDir.y *= -0.5f;
                if (addForcex >= 10)
                {
                    GetRigidBody.velocity = ballDir * addForcex;
                    SetDirection(addForcex);
                }
            }
        }

        private void SetDirection(float force)
        {
            isBallinCurve = true;

            if (lastBallTouchBorder == "BorderTop")
                Enablegravi.objDir = EnableManualGravitiy.Direction.DOWN;
            else if (lastBallTouchBorder == "BorderBottom")
                Enablegravi.objDir = EnableManualGravitiy.Direction.UP;
            else if (lastBallTouchBorder == "BorderTopLeftCorner")
                Enablegravi.objDir = EnableManualGravitiy.Direction.RIGHT;
            else if (lastBallTouchBorder == "BorderBottomLeftCorner")
                Enablegravi.objDir = EnableManualGravitiy.Direction.RIGHT;
            else if (lastBallTouchBorder == "BorderTopRightCorner")
                Enablegravi.objDir = EnableManualGravitiy.Direction.LEFT;
            else if (lastBallTouchBorder == "BorderBottonRightCorner")
                Enablegravi.objDir = EnableManualGravitiy.Direction.LEFT;

            Enablegravi.StartGiveForce(force);
        }

        #endregion
    }
}