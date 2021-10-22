using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PenaltyController : MonoBehaviour {

	/// <summary>
	/// This simple class manages the lights (UI) and scores of the players of penalty kicks
	/// </summary>

	public static int penaltyRound;

	//UI Bulbs
	public GameObject[] player1Lights;
	public GameObject[] player2Lights;

	//Available UI materials (colors)
	/// <summary>
	/// 0 : inactive
	/// 1 : Green (Score)
	/// 2 : Red (No Score)
	/// </summary>
	public Material[] resultMat;

	//arrays holding the result of the penalty kicks
	public static List<int> p1ResultArray;
	public static List<int> p2ResultArray;

	public GameObject playerGoalTrigger;
	public GameObject opponentGoalTrigger;

	void Awake () {

		penaltyRound = 1;
		print ("Penalty Round: " + penaltyRound);

		//reset the result arrays
		p1ResultArray = new List<int>();
		p2ResultArray = new List<int>();

		//init all score lights
		for(int i = 0; i < player1Lights.Length; i++) {
			player1Lights[i].GetComponent<Renderer>().material = resultMat[0];
			player2Lights[i].GetComponent<Renderer>().material = resultMat[0];
		}
	}
	
	void FixedUpdate () {

		//just one goal trigger object needs to be active at a time
		if(penaltyRound % 2 == 1) {
			playerGoalTrigger.GetComponent<BoxCollider>().enabled = false;
			opponentGoalTrigger.GetComponent<BoxCollider>().enabled = true;
		} else {
			playerGoalTrigger.GetComponent<BoxCollider>().enabled = true;
			opponentGoalTrigger.GetComponent<BoxCollider>().enabled = false;
		}
	
	}


	public IEnumerator updateResultArray(string player, int result) {

		penaltyRound++;
		print ("Penalty Round: " + penaltyRound);

		//update the array
		switch(player) {
		case GlobalGameManager.PLAYER1_FLAG:
			p1ResultArray.Add(result);
			break;

		case GlobalGameManager.PLAYER2_FLAG:
		case GlobalGameManager.OPPONENT_FLAG:
			p2ResultArray.Add(result);
			break;
		}

		//render the changes on UI
		for(int i = 0; i < p1ResultArray.Count; i++) {
			if(p1ResultArray[i] == 1)
				player1Lights[i].GetComponent<Renderer>().material = resultMat[1]; 	//green light
			else if(p1ResultArray[i] == 0)	
				player1Lights[i].GetComponent<Renderer>().material = resultMat[2];	//red light
		}

		//render the changes on UI
		for(int i = 0; i < p2ResultArray.Count; i++) {
			if(p2ResultArray[i] == 1)
				player2Lights[i].GetComponent<Renderer>().material = resultMat[1]; 	//green light
			else if(p2ResultArray[i] == 0)	
				player2Lights[i].GetComponent<Renderer>().material = resultMat[2];	//red light
		}

		//check game ending everytime
		if(penaltyRound > 10) {
			yield return new WaitForSeconds(0.2f);
            Debug.Log("GetComponent<GlobalGameManager>().GameOver() has been commented out here.");
		//	GetComponent<GlobalGameManager>().GameOver();
			yield break;
		}
	}



}
