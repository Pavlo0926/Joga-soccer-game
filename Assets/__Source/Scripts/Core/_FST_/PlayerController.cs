using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using FastSkillTeam;
//using System.Collections.Generic;

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    public float Force;
    public float timePlayer;
    public string mDisc_name;
    public float mDisc_aim;
    public string mDisc_status;
    public float mDisc_id;

    public Image percentage_force, percentage_time, percentage_aim;

    public Image InGameForce_Percentage, InGameAim_Percentage, InGameTime_Percentage;

    //This unit's ID (given automatically by PlayerAI class)
    internal int unitIndex;

    //visual helpers
    public GameObject selectionCircle;
    public GameObject arrowPlane;
    private GameObject helperBegin;
    private GameObject helperEnd;
    private GameObject shootCircle;

    //shoot power
    public float playerPowerFactor = 15;
    public float pwr;

    private Vector3 shootDirectionVector;    //this vector holds shooting direction
  
    //prevent player to shoot twice in a round
    public static bool CanShoot { get; set; } = false;
    public bool IsReadytoShoot { get; private set; } = false;

    public GameObject ballTransform;
    public PhysicMaterial currentDiscMaterial;
    public GameObject attacking_Cap;
    public GameObject proctective_Cap;
    public GameObject firstAid_Cap;
    public GameObject[] dots;
    public GameObject obj;
    public float max_aid;
    private MouseFollow mouseFollowScript;
    private Vector3 discReachPos;

    private float currentDistance;    //real distance of our touch/mouse position from initial drag position
    private float safeDistance;    //A safe distance value which is always between min and max to avoid supershoots

    private Collider m_FieldCollider;

    #region Getters

    private Renderer m_MeshRender = null;
    private Renderer MeshRender { get { if (!m_MeshRender) m_MeshRender = GetComponent<Renderer>(); return m_MeshRender; } }

    private Renderer m_ChildMeshRender = null;
    private Renderer ChildMeshRender { get { if (!m_ChildMeshRender) m_ChildMeshRender = transform.GetChild(1).GetComponent<Renderer>(); return m_ChildMeshRender; } }

    private Renderer m_BallRender = null;
    private Renderer BallRender { get { if (!m_BallRender) m_BallRender = ballTransform.GetComponent<Renderer>(); return m_BallRender; } }

    private Renderer m_ArrowPlaneRenderer = null;
    private Renderer ArrowPlaneRenderer { get { if (!m_ArrowPlaneRenderer) m_ArrowPlaneRenderer = arrowPlane.GetComponent<Renderer>(); return m_ArrowPlaneRenderer; } }

    private Renderer m_ShootCircleRenderer = null;
    private Renderer ShootCircleRenderer { get { if (!m_ShootCircleRenderer) m_ShootCircleRenderer = shootCircle.GetComponent<Renderer>(); return m_ShootCircleRenderer; } }

    private Renderer m_SelectionCircleRenderer = null;
    public Renderer SelectionCircleRenderer { get { if (!m_SelectionCircleRenderer) m_SelectionCircleRenderer = selectionCircle.GetComponent<Renderer>(); return m_SelectionCircleRenderer; } }

    private Rigidbody m_Rigidbody = null;
    public Rigidbody GetRigidbody { get { if (!m_Rigidbody) m_Rigidbody = GetComponent<Rigidbody>(); return m_Rigidbody; } }

    private MeshCollider m_MeshCol = null;
    private MeshCollider MeshCol { get { if (!m_MeshCol) m_MeshCol = GetComponent<MeshCollider>(); return m_MeshCol; } }

    private GlobalGameManager m_GlobalGm = null;
    private GlobalGameManager GlobalGm { get { if (!m_GlobalGm) m_GlobalGm = GlobalGameManager.Instance; return m_GlobalGm; } }

    #endregion

    #region Monobehaviour Callbacks

    void Awake()
    {
        if (!Instance)//only one ->FST
            Instance = this;

        //Find and cache important gameObjects
        helperBegin = GameObject.FindGameObjectWithTag("mouseHelperBegin");
        mouseFollowScript = helperBegin.GetComponent<MouseFollow>();
        helperEnd = GameObject.FindGameObjectWithTag("mouseHelperEnd");
        arrowPlane = GameObject.FindGameObjectWithTag("helperArrow");
        shootCircle = GameObject.FindGameObjectWithTag("shootCircle");

        //Init Variables
        pwr = 0f;
        currentDistance = 0;
        shootDirectionVector = new Vector3(0, 0, 0);
    //    CanShoot = true;
        ArrowPlaneRenderer.enabled = false; //hide arrowPlane
        ShootCircleRenderer.enabled = false; //hide shoot Circle
        SetDotsActive(false);
    }

    void Start()
    {
        SelectionCircleRenderer.enabled = false;
        // UpdateStatsValue(); // for disable player stats enable it when player stats enable
    }

    void Update()
    {
        if (!CanShoot)
            IsReadytoShoot = false;

        if(CanShoot && GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.RoundStarted && 
            FST_Gameplay.IsMultiplayer && !FST_DiskPlayerManager.Instance.IsOwner && GlobalGameManager.Instance.IsAllPlayersConnected)
        {
            SelectionCircleRenderer.enabled = true;
            return;
        }

        SelectionCircleRenderer.enabled = ((FST_Gameplay.IsMultiplayer && FST_DiskPlayerManager.Instance.IsOwner) || !FST_Gameplay.IsMultiplayer) &&
            ((GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.RoundStarted && IsReadytoShoot) || 
            (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.RoundStarted && CanShoot && GlobalGameManager.IsMastersTurn));
    }
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

    //    return new float[] { total == 0 ? 0 : total / count, max, min };
    //}

    //private Collider _last = null;

    private void OnCollisionEnter(Collision other)
    {
        if (!m_FieldCollider)
        {
            if (other.gameObject.name == "Field")
            {
              //  Debug.Log("Got field Col");
                m_FieldCollider = other.collider;
            }
        }

        if (other.collider == m_FieldCollider)
            return;

        //if (other.collider == _last)
        //    return;

        //_last = other.collider;

        Vector2 vel = GetRigidbody.velocity;

        float force = vel.magnitude;//avg is around 15

        //avgs.Add(force);

        //Debug.Log("disk 1 force: " + force + " / AVG = " + AverageMaxMinCols()[0] + " / MAX = " + AverageMaxMinCols()[1] + " / MIN = " + AverageMaxMinCols()[2]);
        float hardForce = FST_DiskPlayerManager.Instance.GetPhysicsData.HardDiskForce;

        if (FST_Gameplay.IsMultiplayer && force > hardForce / 2)
        {
      //      Debug.Log("SEND COLLISION EVENT!");
            FST_DiskPlayerManager.Instance.TransmitCollison(FST_DiskPlayerManager.Instance.GetDiskIdByTransform(transform), transform.position, other.GetContact(0).point, GetRigidbody.velocity);
        }

        if (force > hardForce)
            FST_ParticlePooler.Instance.DiskHit(transform, other.GetContact(0).point);

        switch (other.gameObject.tag)
        {
            case "Border":
                //   Debug.Log("disc to wall force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToWall_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToWall_Medium : FST_AudioManager.AudioID.SFX_DiscToWall_Soft);

                break;

            case "Player":
                //  Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);
                CollideWithDisk(other);

                if (!FST_Gameplay.IsMultiplayer)
                    return;

                //ugly commented old player stats hidden ->FST
               // Comment1();

                break;

            case "Player_2":
                //  Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);
                CollideWithDisk(other);

                if (!FST_Gameplay.IsMultiplayer)
                    return;

                //ugly commented old player stats hidden ->FST
               // Comment2();
                break;

            case "Opponent":
                //  Debug.Log("disc to disc force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToDisc_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToDisc_Medium : FST_AudioManager.AudioID.SFX_DiscToDisc_Soft);
                CollideWithDisk(other);

                break;

            case "ball":
                // Debug.Log("disc to ball force = " + force);
                PlayAudio(force > hardForce ? FST_AudioManager.AudioID.SFX_DiscToBall_Hard : force > (hardForce / 2) ? FST_AudioManager.AudioID.SFX_DiscToBall_Medium : FST_AudioManager.AudioID.SFX_DiscToBall_Soft);

                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (FST_Gameplay.IsMultiplayer && !FST_DiskPlayerManager.Instance.IsOwner)
            return;

        //if (other == _last)
        //    return;

        //_last = other;

        switch (other.tag)
        {
            case "gatePost":
                float force = GetRigidbody.velocity.magnitude * Time.fixedDeltaTime;
               // Debug.Log("disk to gatepost force = " + force);
                PlayAudio(force > 0.25f ? FST_AudioManager.AudioID.SFX_BallToPost_Hard : force > 0.125f ? FST_AudioManager.AudioID.SFX_BallToPost_Medium : FST_AudioManager.AudioID.SFX_BallToPost_Soft);
                break;
        }
    }

    #endregion

    private void CollideWithDisk(Collision other)
    {
        /*        Vector3 orthogonalVector = other.contacts[0].point - transform.position;
                float collisionAngle = Vector3.Angle(orthogonalVector, GetRigidbody.velocity);

                Debug.Log("collisionAngle: " + collisionAngle);

                if (collisionAngle < 70)
                    return;*/

        /* float force = other.relativeVelocity.magnitude * Time.fixedDeltaTime;
         Vector3 impactVector = other.contacts[0].normal * force * FST_DiskPlayerManager.Instance.GetPhysicsData.DiskToDiskForceMultiplier;
         other.rigidbody.AddForce(new Vector3(-impactVector.x, -impactVector.y, 0));*/
        //other.rigidbody.AddForceAtPosition(other.rigidbody.velocity * FST_DiskPlayerManager.Instance.GetPhysicsData.DiskToDiskForceMultiplier, other.GetContact(0).point);
        other.rigidbody.velocity *= FST_DiskPlayerManager.Instance.GetPhysicsData.DiskToDiskForceMultiplier;
    }
    /*
        public void UpdateStatsValue()
        {

            InGameForce_Percentage.fillAmount = Force / BumpStaminaManager.instance.initialvalueForce;
            InGameAim_Percentage.fillAmount = mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
            InGameTime_Percentage.fillAmount = timePlayer / BumpStaminaManager.instance.initialvalueTime;
        }
    */
    public void SetDotsActive(bool active)
    {
        //     Debug.Log(Mathf.RoundToInt(mDisc_aim) + " is the dots power");

        if (!active || arrowPlane.transform.localScale.x <= 0.01)
        {
            for (int i = 0; i < dots.Length; i++)
            {
                dots[i].transform.localScale = Vector3.zero;
                dots[i].SetActive(false);
            }

            return;
        }

        float scaleX = 0.025f;
        for (int i = 0; i < dots.Length; i++)
        {
            if (i > 0)
            {
                if (i < 2)
                    scaleX -= 0.003f;
                else scaleX -= 0.002f;
            }
            float scale = scaleX * 10;

            dots[i].transform.localScale = new Vector3(scaleX, scale, scale);

            dots[i].SetActive(Mathf.RoundToInt(arrowPlane.transform.localScale.x * 2) > i);
        }
    }

    /// <summary>
    /// Works fine with mouse and touch
    /// <br>This is the main function used to manage drag on units, calculating the power and debug vectors, and set the final parameters to shoot.</br>
    /// </summary>
    public void PlayerMouseDrag()
    {
        if (GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.RoundStarted)
            return;

        //not our turn
        if (!GlobalGameManager.Instance.IsMyTurn)
            return; 

        if (!CanShoot)
            return;

        if (!IsReadytoShoot)
            return;

        ChildMeshRender.sortingOrder = 2;
        MeshRender.sortingOrder = 2;
        BallRender.sortingOrder = 1;
        ArrowPlaneRenderer.sortingOrder = 2;

        currentDistance = Vector3.Distance(helperBegin.transform.localPosition, new Vector3(0, 0, -10f));

        if (currentDistance <= GlobalGameManager.maxArrowDragDistance)
            safeDistance = currentDistance;
        else safeDistance = GlobalGameManager.maxArrowDragDistance;

        FST_BallManager.Instance.TrailActiveValue = safeDistance;

        pwr = Mathf.Abs(safeDistance) * Force; //this is very important. change with extreme caution.

        ManageArrowTransform();

        //position of helperEnd
        //HelperEnd is the exact opposite (mirrored) version of our helperBegin object 
        //and help us to calculate debug vectors and lines for a perfect shoot.
        //Please refer to the basic geometry references of your choice to understand the math.
        Vector3 dxy = helperBegin.transform.localPosition - transform.position;
        float diff = dxy.magnitude;

        helperEnd.transform.position = transform.position + ((dxy / diff) * currentDistance * -1);
        helperEnd.transform.position = new Vector3(helperEnd.transform.position.x, helperEnd.transform.position.y, -0.5f);
        /*
        #if DEBUG
                    //debug line from initial position to our current touch position
                    Debug.DrawLine(transform.position, helperBegin.transform.position, Color.red);
                    //debug line from initial position to maximum power position (mirrored)
                    Debug.DrawLine(transform.position, arrowPlane.transform.position, Color.blue);
                    //debug line from initial position to the exact opposite position (mirrored) of our current touch position
                    Debug.DrawLine(transform.position, (2 * transform.position) - helperBegin.transform.position, Color.yellow);
                    //cast ray forward and collect informations
                    CastRay();

                    //Not used! You can extend this function to have more precise control over physics of the game
                    SweepTest();
        #endif
        */
        Vector3 pwrAndDir = helperBegin.transform.position - transform.position;
        mDisc_aim = pwrAndDir.magnitude * 0.25f;
        //final vector used to shoot the unit.
        shootDirectionVector = pwrAndDir.normalized;

    }

    /// <summary>
    /// Unhide and process the transform and scale of the power Arrow object
    /// </summary>
    void ManageArrowTransform()
    {
        //Show helpers
        ArrowPlaneRenderer.enabled = true;
        ShootCircleRenderer.enabled = true;
        SetDotsActive(true);

        arrowPlane.transform.position = new Vector3(transform.position.x, transform.position.y, -1.5f);
        shootCircle.transform.position = transform.position + new Vector3(0, 0, 0.05f);

        //Calculate rotation
        Vector3 dir = helperBegin.transform.position - transform.position;
        float angleOfArrow = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        arrowPlane.transform.localEulerAngles = new Vector3(arrowPlane.transform.eulerAngles.x, arrowPlane.transform.eulerAngles.y, -angleOfArrow + 90);
        arrowPlane.transform.localScale = new Vector3(safeDistance + 0.5f, 1f, 0.001f); //default scale
        shootCircle.transform.localScale = new Vector3(safeDistance + 0.5f, safeDistance + 0.5f, 0.001f); //default scale

        if (FST_Gameplay.IsMultiplayer)
            FST_DiskPlayerManager.Instance.TransmitShootHelperActive(true);
    }

    public void PlayerMouseDown()
    {
        if (GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.RoundStarted)
            return;
        //not our turn
        if (!GlobalGameManager.Instance.IsMyTurn)
            return;

        if (!CanShoot)
            return;

        IsReadytoShoot = true;
        mouseFollowScript.SetTurn(transform);
        arrowPlane.transform.position = transform.position;
        arrowPlane.transform.eulerAngles = new Vector3(arrowPlane.transform.eulerAngles.x, arrowPlane.transform.eulerAngles.y, -180);

        if (FST_Gameplay.IsMultiplayer)
        {
            FST_DiskPlayerManager.Instance.TransferOwnerShip(Photon.Pun.PhotonNetwork.LocalPlayer);
            FST_DiskPlayerManager.Instance.TransmitShootHelperActive(true);
        }
    }

    /// <summary>
    /// Actual shoot function
    /// </summary>
    public void PlayerMouseUp()
    {
        if (GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.RoundStarted)
            return;
        //not our turn
        if (!GlobalGameManager.Instance.IsMyTurn)
            return;
        //prevent double shooting in a round
        if (!IsReadytoShoot || !CanShoot)
            return;

        ChildMeshRender.sortingOrder = 0;
        MeshRender.sortingOrder = 0;
        BallRender.sortingOrder = 0;
        ArrowPlaneRenderer.sortingOrder = 0;

        //give the player a second chance to choose another ball if drag on the unit is too low.
        if (currentDistance <=
#if !UNITY_EDITOR
            1.5f
#else
            28.1f
#endif
            )
        {
            DisableArrow();
            if (FST_Gameplay.IsMultiplayer)
                FST_DiskPlayerManager.Instance.TransmitShootHelperActive(false);
            return;
        }

        //no more shooting is possible	
        CanShoot = false;
        IsReadytoShoot = false;

        //hide helper arrow object
        DisableArrow();

        StartCoroutine(WaitForOwnerShip());
    }

    private IEnumerator WaitForOwnerShip()
    {
        GlobalGm.DoKick();

        yield return new WaitUntil(() => !FST_Gameplay.IsMultiplayer || FST_DiskPlayerManager.Instance.photonView.IsMine);

        //do the physics calculations and shoot the ball 

        if (pwr < GlobalGameManager.minPower)
            pwr = GlobalGameManager.minPower;

        Vector3 outPower = shootDirectionVector * pwr * -1;

        GlobalGm.CurrentPlayerName = gameObject.name;

        //add team power bonus
        outPower *= (1 + (TeamsManager.getTeamSettings(FST_SettingsManager.Team).x / playerPowerFactor));

        //Bug fix. Avoid shoot powers over 40, or the ball might fly off the level bounds.
        //Introduced in version 1.5+
        //		if (outPower.magnitude >= 45)
        //			outPower *= 0.65f; 

        GetRigidbody.AddForce(new Vector3(outPower.x, outPower.y, 0), ForceMode.Impulse);

        pwr = 0;

        StartCoroutine(AssignPlayer());
    }

    private void DisableArrow()
    {
        ArrowPlaneRenderer.enabled = false;
        ShootCircleRenderer.enabled = false;
        arrowPlane.transform.localScale = new Vector3(0f, 0f, 0.001f);
        SetDotsActive(false);
    }

    private IEnumerator AssignPlayer()
    {
        yield return new WaitForSeconds(0.1f); //() => GlobalGameManager.shootHappened == false;
        GlobalGm.CurrentPlayer = GetRigidbody;
        //gameObject.GetComponent <PlayerStats> ().PrimaryAttackingDisc = true; //for player state disable enable it after enable player stats
    }

    private void PlayAudio(FST_AudioManager.AudioID audioID) => FST_AudioManager.Instance.PlayAudio(audioID);

    //these comments are only a touch of how it used to look LOL.
    void Comment1()
    {
        /*
               for (int i = 0; i < gameObject.GetComponent <PlayerStats> ().touchDiscName.Count; i++) {
                   if (gameObject.GetComponent <PlayerStats> ().touchDiscName [i] == other.gameObject.name) {
                       Debug.LogError ("return-in---" + gameObject.GetComponent <PlayerStats> ().touchDiscName [i] + "" + other.gameObject.name);
                       return;

                   }
               }
               */
        /*
                    if (gameObject.GetComponent <PlayerStats> ().PrimaryAttackingDisc) {

                        other.gameObject.GetComponent <PlayerStats> ().SecondaryAttackingDisc = true;
                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isElephant");
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isRhino");
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isLion");
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        } 
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);

                    } else if (gameObject.GetComponent <PlayerStats> ().SecondaryAttackingDisc && !other.gameObject.GetComponent <PlayerStats> ().PrimaryAttackingDisc) {
                        other.gameObject.GetComponent <PlayerStats> ().SecondaryAttackingDisc = true;
                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isElephant");
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isRhino");
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "isLion");
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        }

                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);
                    } else if (gameObject.GetComponent <PlayerStats> ().FirstIndirectOpponentDisc) {
                        other.gameObject.GetComponent <PlayerStats> ().SecondaryIndirectOpponentDisc = true;

                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isElephant");
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isRhino");
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isLion");
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.twentyfivePercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage); /////////Change..........................................
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);


                    } else if (gameObject.GetComponent <PlayerStats> ().DirectOpponentDisc) {
                        other.gameObject.GetComponent <PlayerStats> ().FirstIndirectOpponentDisc = true;

                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isElephant");
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isRhino");
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isLion");
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.fiftyPercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);
                    } else if (gameObject.GetComponent <PlayerStats> ().SecondaryIndirectOpponentDisc) {
                        other.gameObject.GetComponent <PlayerStats> ().SecondaryIndirectOpponentDisc = true;

                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isElephant");
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isRhino");
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isLion");
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.twentyfivePercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage); /////////Change..........................................
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);


                    }

                        */

    }
    void Comment2()
    {

        /*
                    for (int i = 0; i < gameObject.GetComponent <PlayerStats> ().touchDiscName.Count; i++) {
                        if (gameObject.GetComponent <PlayerStats> ().touchDiscName [i] == other.gameObject.name) {
                            return;
                        }
                    }
                    */
        /*
                    if (gameObject.GetComponent <PlayerStats> ().PrimaryAttackingDisc && GlobalGameManager.playersTurn) {
                        other.gameObject.GetComponent <PlayerStats> ().DirectOpponentDisc = true;
                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.hundredPercentage, "isElephant");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isElephant = false;
                                BumpStaminaManager.instance.isElephant_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.hundredPercentage, "isRhino");

                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isRhino = false;
                                BumpStaminaManager.instance.isRhino_btn = false;
                                attacking_Cap.SetActive (false);
                            }

                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.hundredPercentage, "isLion");

                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isLion = false;
                                BumpStaminaManager.instance.isLion_btn = false;
                                attacking_Cap.SetActive (false);
                            }

                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.hundredPercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.fiftyPercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);

        //				if (other.gameObject.GetComponent <PlayerStats> ().isElephant) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isElephant = false;
        //						BumpStaminaManager.instance.isElephant_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isRhino) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isRhino = false;
        //						BumpStaminaManager.instance.isRhino_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isLion) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isLion = false;
        //						BumpStaminaManager.instance.isLion_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				}


                    } else if (gameObject.GetComponent <PlayerStats> ().SecondaryAttackingDisc && GlobalGameManager.playersTurn) {
                        other.gameObject.GetComponent <PlayerStats> ().FirstIndirectOpponentDisc = true;
                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isElephant");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                Debug.LogError ("pl first in direct-11-" + gameObject.GetComponent <PlayerStats> ().attackingcap_Count);
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isElephant = false;
                                BumpStaminaManager.instance.isElephant_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isRhino");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isRhino = false;
                                BumpStaminaManager.instance.isRhino_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.fiftyPercentage, "isLion");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isLion = false;
                                BumpStaminaManager.instance.isLion_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.fiftyPercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.twentyfivePercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);

        //				if (other.gameObject.GetComponent <PlayerStats> ().isElephant) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isElephant = false;
        //						BumpStaminaManager.instance.isElephant_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isRhino) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isRhino = false;
        //						BumpStaminaManager.instance.isRhino_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isLion) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isLion = false;
        //						BumpStaminaManager.instance.isLion_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				}

                    } else if ((gameObject.GetComponent <PlayerStats> ().FirstIndirectOpponentDisc || gameObject.GetComponent <PlayerStats> ().DirectOpponentDisc || gameObject.GetComponent <PlayerStats> ().SecondaryIndirectOpponentDisc) && GlobalGameManager.players2Turn) {
                        other.gameObject.GetComponent <PlayerStats> ().SecondaryIndirectAttackingDisc = true;

                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "ZERO");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                Debug.LogError ("pl first in direct-11-" + gameObject.GetComponent <PlayerStats> ().attackingcap_Count);
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isElephant = false;
                                BumpStaminaManager.instance.isElephant_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "ZERO");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isRhino = false;
                                BumpStaminaManager.instance.isRhino_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.zeroPercentage, "ZERO");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isLion = false;
                                BumpStaminaManager.instance.isLion_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.zeroPercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);

        //				if (other.gameObject.GetComponent <PlayerStats> ().isElephant) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isElephant = false;
        //						BumpStaminaManager.instance.isElephant_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isRhino) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isRhino = false;
        //						BumpStaminaManager.instance.isRhino_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isLion) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isLion = false;
        //						BumpStaminaManager.instance.isLion_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				}

                    } else if (gameObject.GetComponent <PlayerStats> ().SecondaryIndirectAttackingDisc && GlobalGameManager.playersTurn) {
                        other.gameObject.GetComponent <PlayerStats> ().SecondaryIndirectOpponentDisc = true;

                        if (gameObject.GetComponent <PlayerStats> ().isElephant) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isElephant");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                Debug.LogError ("pl first in direct-11-" + gameObject.GetComponent <PlayerStats> ().attackingcap_Count);
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isElephant = false;
                                BumpStaminaManager.instance.isElephant_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isRhino) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isRhino");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isRhino = false;
                                BumpStaminaManager.instance.isRhino_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else if (gameObject.GetComponent <PlayerStats> ().isLion) {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation_Onattack (PlayerStats.SharedInstance.twentyfivePercentage, "isLion");
                            gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
                            if (gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
                                gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
                                gameObject.GetComponent <PlayerStats> ().isLion = false;
                                BumpStaminaManager.instance.isLion_btn = false;
                                attacking_Cap.SetActive (false);
                            }
                        } else {
                            other.gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.twentyfivePercentage);
                        }
                        gameObject.GetComponent <PlayerStats> ().Calculation (PlayerStats.SharedInstance.twentyfivePercentage);
                        other.gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (gameObject.name);
                        gameObject.GetComponent <PlayerStats> ().touchDiscName.Add (other.gameObject.name);

        //				if (other.gameObject.GetComponent <PlayerStats> ().isElephant) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isElephant = false;
        //						BumpStaminaManager.instance.isElephant_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isRhino) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isRhino = false;
        //						BumpStaminaManager.instance.isRhino_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				} else if (other.gameObject.GetComponent <PlayerStats> ().isLion) {					
        //					other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count++;
        //					if (other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count >= 2) {
        //						other.gameObject.GetComponent <PlayerStats> ().attackingcap_Count = 0;
        //						other.gameObject.GetComponent <PlayerStats> ().isLion = false;
        //						BumpStaminaManager.instance.isLion_btn = false;
        //						attacking_Cap.SetActive (false);
        //					}
        //				}

                    }
                     */
    }
    /*
#if DEBUG
        /// <summary>
        /// Cast the rigidbody's shape forward to see if it is about to hit anything.
        /// </summary>
        void SweepTest()
        {
            if (GetRigidbody.SweepTest((helperEnd.transform.position - transform.position).normalized, out RaycastHit hit, 15))
                Debug.Log("if hit ??? : " + hit.distance + " - " + hit.transform.gameObject.name);
        }

        private RaycastHit hitInfo;
        private Ray ray;
        /// <summary>
        /// Cast a ray forward and collect informations like if it hits anything...
        /// </summary>
        void CastRay()
        {
            // cast the ray from units position with a normalized direction out of it which is mirrored to our current drag vector.
            ray = new Ray(transform.position, (helperEnd.transform.position - transform.position).normalized);

            if (Physics.Raycast(ray, out hitInfo, currentDistance))
            {
                //GameObject objectHit = hitInfo.transform.gameObject;// UNUSED ->FST

                //debug line whenever the ray hits something.
                //Debug.DrawLine (ray.origin, hitInfo.point, Color.cyan);

                //draw reflected vector like a billiard game. this is the out vector which reflects from targets geometry.
                Vector3 reflectedVector = Vector3.Reflect(hitInfo.point - ray.origin, hitInfo.normal);
                Debug.DrawRay(hitInfo.point, reflectedVector * -1, Color.gray, 0.2f);

                //draw inverted reflected vector (useful for fine-tuning the final shoot)
                Debug.DrawRay(hitInfo.transform.position, reflectedVector * -1, Color.white, 0.2f);

                //draw the inverted normal which is more likely to be similar to real world response.
                Debug.DrawRay(hitInfo.transform.position, hitInfo.normal * -3, Color.red, 0.2f);

                //Debug
                ////print("Ray hits: " + objectHit.name + " At " + Time.time + " And Reflection is: " + reflectedVector);
            }
        }
#endif
    */
}
