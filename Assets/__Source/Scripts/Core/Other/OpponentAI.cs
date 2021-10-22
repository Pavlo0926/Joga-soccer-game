using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using FastSkillTeam;

public class OpponentAI : MonoBehaviour
{

	///*************************************************************************///
	/// Main AI Controller.
	/// This class manages the shooting process of AI (CPU opponent).
	/// This also handles the rendering of AI debug lines in editor.
	///*************************************************************************///

	public float Force;
	public float timeOpponent;
	public float mDisc_aim;

	public Texture2D[] availableFlags;
	public Texture2D[] availableDownFlags;

	//array of all available teams
	public static OpponentAI instance;
	public static GameObject[] myTeam;
	//List of all AI units
	private GameObject target;
	//reference to main Ball
	private float distanceToTarget;
	//Distance of selected unit to ball
	private Vector3 directionToTarget;
	//Direction of selected unit to ball
	private float shootPower;
	//AI shoot power. Edit with extreme caution!!!!
	public Vector3 appliedPower;
	public static bool opponentCanShoot;
	//Allowed to shoot? flag
	private float shootTime;
	//Allowed time to perform the shoot
	private bool isReadyToShoot;
	//if all processes are done, flag

	//Reference to main game controller
	private GameObject PlayerBasketCenter;
	//helper object which shows the center of player gate to the AI
	//static int scoreQueue;				//
	private int opponentFormation;
	//Selected formation for AI
	internal bool canChangeFormation;
	//Is allowed to change formation on the fly?

	public float OpponentPowerFactor = 15;

    private float difficulty = 0;

    private GameObject bestShooter;
    private bool isShooting = false;
    /// <summary>
    /// Init. Updates the 3d texts with saved values fetched from playerprefs.
    /// </summary>
    private void Awake ()
	{
		target = GameObject.FindGameObjectWithTag ("ball");
		PlayerBasketCenter = GameObject.FindGameObjectWithTag ("PlayerBasketCenter");
		isReadyToShoot = false;
		opponentCanShoot = true;
		
		canChangeFormation = true;
        opponentFormation = FastSkillTeam.FST_SettingsManager.FormationOpponent;
		
		//cache all available units
		myTeam = GameObject.FindGameObjectsWithTag ("Opponent");
		//debug
		int i = 1;
		foreach (GameObject unit in myTeam) {
			//Optional
			unit.name = "Opponent-Player-" + i;
			unit.GetComponent<OpponentUnitController> ().unitIndex = i;
			unit.GetComponent<Renderer> ().material.mainTexture = availableFlags [PlayerPrefs.GetInt ("OpponentFlag", 1)];
			unit.transform.GetChild (1).GetComponent <Renderer> ().material.mainTexture = availableDownFlags [PlayerPrefs.GetInt ("OpponentFlag", 1)];
			i++;		
			//print("My Team: " + unit.name);
		}
	}

    private void Start()
    {
        instance = this;
        if (!GlobalGameManager.isPenaltyKick)
        {
            StartCoroutine(ChangeFormation(opponentFormation, 1));
            canChangeFormation = false;
        }
        else
        {
            StartCoroutine(GoToPosition(myTeam, GlobalGameManager.AIDestination, 1));
        }

        difficulty = FST_SettingsManager.Difficulty;

        difficulty++;//add to it so we have 1,2,3 for multipliers
    }

    public void SetFormation()
    {
        if (!GlobalGameManager.isPenaltyKick)
        {
            //get a new random formation everytime
            StartCoroutine(ChangeFormation(Random.Range(0, FormationManager.formations), 0.6f));
        }
        else
        {
            //go to correct penalty position
            StartCoroutine(GoToPosition(myTeam,GlobalGameManager.AIDestination, 1));
        }
    }

    /// <summary>
    /// AI can change it's formation to a new one after it delivers or receives a goal.
    /// </summary>
    /// <param name="_formationIndex"></param>
    /// <param name="_speed"></param>
    /// <returns></returns>
    private IEnumerator ChangeFormation(int _formationIndex, float _speed)
    {

        //cache the initial position of all units
        List<Vector3> unitsSartingPosition = new List<Vector3>();
        foreach (var unit in myTeam)
        {
            unitsSartingPosition.Add(unit.transform.position);  //get the initial postion of this unit for later use.
            unit.GetComponent<MeshCollider>().enabled = false;  //no collision for this unit till we are done with re positioning.
        }

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _speed;
            for (int cnt = 0; cnt < myTeam.Length; cnt++)
            {
                myTeam[cnt].transform.position = new Vector3(
                    Mathf.SmoothStep(unitsSartingPosition[cnt].x,FormationManager.GetPositionInFormation(_formationIndex, cnt).x * -1, t),
                    Mathf.SmoothStep(unitsSartingPosition[cnt].y, FormationManager.GetPositionInFormation(_formationIndex, cnt).y, t),
                    FormationManager.fixedZ
                    );
            }
            yield return 0;
        }

        canChangeFormation = true;
        foreach (var unit in myTeam)
            unit.GetComponent<MeshCollider>().enabled = true;   //collision is now enabled.
    }


    private void FixedUpdate ()
	{
        if (FST_Gameplay.IsMultiplayer)
            return;
		//prepare to shoot
		if (!isShooting && !GlobalGameManager.IsMastersTurn && opponentCanShoot && FST_DiskPlayerManager.Instance.AllMovingObjectsHaveStopped()) {
			opponentCanShoot = false;
			StartCoroutine (Shoot());	
		}
	}

    /// <summary>
    /// Shoot the selected unit.
    /// All AI steps are described and fully commented.
    /// </summary>
    private IEnumerator Shoot()
    {
        isShooting = true;
        //wait for a while to fake thinking process 
        if (GlobalGameManager.isPenaltyKick)
            yield return new WaitForSeconds(Random.Range(2.5f, 5.0f));
        else yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));

        //init
        bestShooter = null;

        if (UIManager.Instance.curveLoftBtn.interactable)
        {
            if (Random.Range(0f, 1f) > 0.5f)
            {
                //fake some more thinking, allowing player to see curve button active easier
                yield return new WaitForSeconds(Random.Range(1.0f, 3.0f));
                UIManager.Instance.OnClickCurveShot();
            }
        }

        //select the best unit to shoot.

        //1. find units with good position to shoot
        //Units that are in the right hand side of the ball are considered a better options. 
        //They can have better angle to the player's gate.
        List<GameObject> shooters = new List<GameObject>();     //list of all good units
        List<float> distancesToBall = new List<float>();    //distance of these good units to the ball
        foreach (GameObject shooter in myTeam)
        {
            if (shooter.transform.position.x > target.transform.position.x + 1.5f)
            {
                shooters.Add(shooter);
                distancesToBall.Add(Vector3.Distance(shooter.transform.position, target.transform.position));
            }
        }

        //if we found atleast one good unit...
        float minDistance = 1000;
        int minDistancePlayerIndex = 0;
        if (shooters.Count > 0)
        {
            //print("we have " + shooters.Count + " unit(s) in a good shoot position");
            for (int i = 0; i < distancesToBall.Count; i++)
            {
                if (distancesToBall[i] <= minDistance)
                {
                    minDistance = distancesToBall[i];
                    minDistancePlayerIndex = i;
                }
                //print(shooters[i] + " distance to ball is " + distancesToBall[i]);
            }
            //find the unit which is most closed to ball.
            bestShooter = shooters[minDistancePlayerIndex];
            //print("MinDistance to ball is: " + minDistance + " by opponent " + bestShooter.name);	
        }
        else
        {
            //print("no player is availabe for a good shoot!");
            //Select a random unit
            bestShooter = myTeam[Random.Range(0, myTeam.Length)];
        }

        //calculate direction and power and add a little randomness to the shoot (can be used to make the game easy or hard)
        float distanceCoef = 0;
        if (minDistance <= 5 && minDistance >= 0)
            distanceCoef = Random.Range(1.0f, 2.5f);
        else
            distanceCoef = Random.Range(2.0f, 4.0f);

        //////////////////////////////////////////////////////////////////////////////////////////////////
        // Detecting the best angle for the shoot
        //////////////////////////////////////////////////////////////////////////////////////////////////
        Vector3 vectorToGate;                   //direct vector from shooter to gate
        Vector3 vectorToBall;                   //direct vector from shooter to ball
        float straightAngleDifferential;        //angle between "vectorToGate" and "vectorToBall" vectors
        vectorToGate = PlayerBasketCenter.transform.position - bestShooter.transform.position;
        vectorToBall = target.transform.position - bestShooter.transform.position;
        straightAngleDifferential = Vector3.Angle(vectorToGate, vectorToBall);
        //if angle between these two vector is lesser than 10 for example, we have a clean straight shot to gate.
        //but if the angle is more, we have to calculate the correct angle for the shoot.
        //////////////////////////////////////////////////////////////////////////////////////////////////
        //////////////////////////////////////////////////////////////////////////////////////////////////

        float shootPositionDifferential = bestShooter.transform.position.y - target.transform.position.y;

        if (straightAngleDifferential <= 10)
        {

            //direct shoot
            directionToTarget = target.transform.position - bestShooter.transform.position;

        }
        else if (Mathf.Abs(shootPositionDifferential) <= 0.5f)
        {

            //direct shoot
            directionToTarget = target.transform.position - bestShooter.transform.position;

        }
        else if (Mathf.Abs(shootPositionDifferential) > 0.5f && Mathf.Abs(shootPositionDifferential) <= 1)
        {

            if (shootPositionDifferential > 0)
                directionToTarget = (target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / 2.5f, 0)) - bestShooter.transform.position;
            else
                directionToTarget = (target.transform.position + new Vector3(0, bestShooter.transform.localScale.z / 2.5f, 0)) - bestShooter.transform.position;

        }
        else if (Mathf.Abs(shootPositionDifferential) > 1 && Mathf.Abs(shootPositionDifferential) <= 2)
        {

            if (shootPositionDifferential > 0)
                directionToTarget = (target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / 2, 0)) - bestShooter.transform.position;
            else
                directionToTarget = (target.transform.position + new Vector3(0, bestShooter.transform.localScale.z / 2, 0)) - bestShooter.transform.position;

        }
        else if (Mathf.Abs(shootPositionDifferential) > 2 && Mathf.Abs(shootPositionDifferential) <= 3)
        {

            if (shootPositionDifferential > 0)
                directionToTarget = (target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / 1.6f, 0)) - bestShooter.transform.position;
            else
                directionToTarget = (target.transform.position + new Vector3(0, bestShooter.transform.localScale.z / 1.6f, 0)) - bestShooter.transform.position;

        }
        else if (Mathf.Abs(shootPositionDifferential) > 3)
        {

            if (shootPositionDifferential > 0)
                directionToTarget = (target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / 1.25f, 0)) - bestShooter.transform.position;
            else
                directionToTarget = (target.transform.position + new Vector3(0, bestShooter.transform.localScale.z / 1.25f, 0)) - bestShooter.transform.position;

        }

        shootPower = Random.Range(18f, 23.4f) * Force;

        if (shootPower < 10)
            shootPower = 12f;

        //set the shoot power based on direction and distance to ball
        appliedPower = Vector3.Normalize(directionToTarget) * shootPower;
        //add team power bonus
        //print ("OLD appliedPower: " + appliedPower.magnitude);
        appliedPower *= (1 + (TeamsManager.getTeamSettings(PlayerPrefs.GetInt("Player2Flag")).x / OpponentPowerFactor));
        //print ("NEW appliedPower: " + appliedPower.magnitude);

        //Bug fix. Avoid shoot powers over 40, or the ball might fly off the level bounds.
        //Introduced in version 1.5+
        //		if(appliedPower.magnitude >= 40)
        //			appliedPower *= 0.82f;

        //if this is a penalty shoot, we sure have a direct shoot. 
        //So we add an extra layer for randomness in shoot direction
        if (GlobalGameManager.isPenaltyKick)
        {

            int dir;
            float rnd = UnityEngine.Random.value;
            if (rnd > 0.5f)
                dir = 1;
            else
                dir = -1;

            directionToTarget = (target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / Random.Range(1.55f, 4.75f), 0) * dir) - bestShooter.transform.position;
            appliedPower = Vector3.Normalize(directionToTarget) * shootPower;

            appliedPower *= 0.95f;
        }

        appliedPower *= difficulty;

        bestShooter.GetComponent<Rigidbody>().AddForce(new Vector3(appliedPower.x, appliedPower.y, 0), ForceMode.Impulse);
        GlobalGameManager.Instance.CurrentOpponentName = bestShooter.gameObject.name;

        for (int i = 0; i < myTeam.Length; i++)
        {
            myTeam[i].GetComponent<OpponentUnitController>().canShowSelectionCircle = false;
        }
#if UNITY_EDITOR
        StartCoroutine(VisualDebug());
#endif

        appliedPower = new Vector3(0, 0, 0);

        StartCoroutine(AssignPlayer());

        GlobalGameManager.Instance.DoKick(false);

        isShooting = false;
    }


  private IEnumerator AssignPlayer()
    {

		yield return new WaitForSeconds (0.1f);

		GlobalGameManager.Instance.CurrentOpponent = bestShooter.GetComponent<Rigidbody> ();

	}

    /// <summary>
    /// move the unit to its position
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pos"></param>
    /// <param name="_speed"></param>
    /// <returns></returns>
    public IEnumerator GoToPosition(GameObject[] unit, Vector3 pos, float _speed)
    {

        Vector3 unitsSartingPosition = unit[0].transform.position;
        unit[0].GetComponent<SphereCollider>().enabled = false; //no collision for this unit till we are done with re positioning.

        float t = 0;
        while (t < 1)
        {
            t += Time.deltaTime * _speed;
            unit[0].transform.position = new Vector3(Mathf.SmoothStep(unitsSartingPosition.x, pos.x, t),
                Mathf.SmoothStep(unitsSartingPosition.y, pos.y, t),
                unitsSartingPosition.z);

            yield return 0;
        }

        if (t >= 1)
        {
            unit[0].GetComponent<SphereCollider>().enabled = true;
        }
    }

#if UNITY_EDITOR
    /// <summary>
    /// Draw the debug lines of AI controller in editor
    /// </summary>
    private IEnumerator VisualDebug()
    {
        //Visual debug
        while (!isReadyToShoot && bestShooter)
        {
            //draw helper line from shooter unit to ball
            Debug.DrawLine(bestShooter.transform.position, target.transform.position, Color.green);

            //draw helper line which gets out of ball after direct impact
            Debug.DrawLine(target.transform.position, (target.transform.position * 2 - bestShooter.transform.position), Color.gray);

            //draw helper line from shooter unit to ball with ball's tangent in mind
            Debug.DrawLine(bestShooter.transform.position, target.transform.position + new Vector3(0, target.transform.localScale.z / 2, 0), Color.red);
            Debug.DrawLine(bestShooter.transform.position, target.transform.position - new Vector3(0, target.transform.localScale.z / 2, 0), Color.red);

            //draw helper line from shooter unit to ball with shooter's tangent in mind
            Debug.DrawLine(bestShooter.transform.position, target.transform.position + new Vector3(0, bestShooter.transform.localScale.z / 2, 0), Color.yellow);
            Debug.DrawLine(bestShooter.transform.position, target.transform.position - new Vector3(0, bestShooter.transform.localScale.z / 2, 0), Color.yellow);

            //draw helper line from shooter unit to player's gate		
            Debug.DrawLine(bestShooter.transform.position, PlayerBasketCenter.transform.position, Color.cyan);

            //draw helper line from ball to player's gate
            Debug.DrawLine(target.transform.position, PlayerBasketCenter.transform.position, Color.green);

            yield return 0;
        }
    }
#endif
}