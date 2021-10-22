using UnityEngine;
using System.Collections;

public class GoalKeeperController : MonoBehaviour {

	/// <summary>
	/// This class gives the goalkeepers a simple AI which moves them inside the gate 
	/// to avoid the shooter to have an easy direct shot. You can edit the moveSpeed to come up with a 
	/// smarter/dumber goalkeeper.
	/// </summary>

	public bool isGoalkeeper = false;
	[Range(0.7f, 2.0f)]
	public float moveSpeed = 1.2f;//increasing this parameter will result in a better reflex of goalkeeper
	private bool canMove = false;
	private float startDelay = 3.0f;
	IEnumerator Start () {
		//This class only works in Penalty Mode
		if(!GlobalGameManager.isPenaltyKick)
			this.enabled = false;

		yield return new WaitForSeconds(startDelay);
		canMove = true;
	}
	
	void FixedUpdate () {

		checkIsGoalKeeper();

		if(isGoalkeeper && canMove && GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.GoalIntermission && !GlobalGameManager.Instance.IsGameOver)
			StartCoroutine(moveGoalkeeper());

	}

	/// <summary>
	/// Checks if this object is the goal keeper
	/// </summary>
	void checkIsGoalKeeper () {

		//in even rounds, player 1 is the goalkeeper
		//in odd rounds, player-2/AI is goalkeeper

		if(PenaltyController.penaltyRound % 2 == 1) {
			if(this.gameObject.tag == "Opponent" || this.gameObject.tag == "Player_2")
				isGoalkeeper = true;
			else 
				isGoalkeeper = false;
		}


		if(PenaltyController.penaltyRound % 2 == 0) {
			if(this.gameObject.tag == "Player")
				isGoalkeeper = true;
			else 
				isGoalkeeper = false;
		}
	}


	/// <summary>
	/// Moves the goalkeeper inside the gate
	/// </summary>
	public IEnumerator moveGoalkeeper() {

		if(canMove)
			canMove = false;

		Vector3 cPos = transform.position;
		Vector3 dest = getNewDestination(transform.position);
		//print ("Destination: " + dest);

		float t = 0;
		while(t < 1) {
			t += Time.deltaTime * moveSpeed;
			transform.position = new Vector3(dest.x,
			                                 Mathf.SmoothStep(cPos.y, dest.y, t),
			                                 dest.z);
			yield return 0;
		}

		if(t >= 1) {
			canMove = true;
			yield break;
		}
	}


	/// <summary>
	/// Gets a new destination after each move
	/// </summary>
	Vector3 getNewDestination(Vector3 p) {

		int dir = 1;

		if(p.y >= 0)
			dir = -1;
		else 
			dir = 1;

		return new Vector3(13, Mathf.Abs(UnityEngine.Random.Range(-4.0f, 4.0f)) * dir, p.z);
	}

}
 