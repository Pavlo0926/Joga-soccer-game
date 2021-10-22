using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnlineTournamentManager : MonoBehaviour
{
    /*
	public OnlineRound1 round1;
	public OnlineRound2 round2;
	public OnlineRound3 round3;

	public List<string> level1Player;
	public List<string> level2Player;
	public List<string> level3Player;
	public static OnlineTournamentManager instance;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	public void AssignLevel1PlayerName ()
	{
		Debug.Log ("enter ZZZZZZZZZZZZZZZZZZZZ");
		level1Player = MainSocketConnection.instance.allRound1PlayerID;
		GameManager.SharedInstance.OnlineTournamentLevel1PlayerID = level1Player;
		for (int i = 0; i < level1Player.Count; i++) {
			if (level1Player [i].Contains ("000")) {
				round1.tournamentTexts [i].text = "";
			} else {
				round1.tournamentTexts [i].text = "" + level1Player [i];
			}
		}
	}

	public void AssignLevel2PlayerName ()
	{
		Debug.Log ("enter XXXXXXXXXXXXX");
		level2Player = MainSocketConnection.instance.allRound2PlayerID;
		GameManager.SharedInstance.OnlineTournamentLevel2PlayerID = level2Player;
		for (int i = 0; i < level2Player.Count; i++) {
			if (level2Player [i].Contains ("000")) {
				round2.tournamentTexts [i].text = "";
			} else {
				round2.tournamentTexts [i].text = "" + level2Player [i];
			}
		}
	}

	public void AssignLevel3PlayerName ()
	{
		Debug.Log ("enter YYYYYYYYYYYYYYYYYYYYYYYYY");

		level3Player = MainSocketConnection.instance.allRound3PlayerID;
		GameManager.SharedInstance.OnlineTournamentLevel3PlayerID = level3Player;
		for (int i = 0; i < level3Player.Count; i++) {
			if (level3Player [i].Contains ("000")) {
				round3.tournamentTexts [i].text = "";
			} else {
				round3.tournamentTexts [i].text = "" + level3Player [i];
			}
		}
	}

	public void Assign1RoundPlayer ()
	{
		for (int i = 0; i < level1Player.Count; i++) {
			if (level1Player [i].Contains ("000")) {
				round1.tournamentTexts [i].text = "";
			} else {
				round1.tournamentTexts [i].text = "" + level1Player [i];
			}
		}
	}

	public void Assign2RoundPlayer ()
	{
		for (int i = 0; i < level1Player.Count; i++) {
			if (level1Player [i].Contains ("000")) {
				round1.tournamentTexts [i].text = "";
			} else {
				round1.tournamentTexts [i].text = "" + level1Player [i];
			}
		}

		for (int i = 0; i < level2Player.Count; i++) {
			if (level2Player [i].Contains ("000")) {
				round2.tournamentTexts [i].text = "";
			} else {
				round2.tournamentTexts [i].text = "" + level2Player [i];
			}
		}

	}

	public void AssignAllLevelPlayer ()
	{
		for (int i = 0; i < level1Player.Count; i++) {
			if (level1Player [i].Contains ("000")) {
				round1.tournamentTexts [i].text = "";
			} else {
				round1.tournamentTexts [i].text = "" + level1Player [i];
			}
		}

		for (int i = 0; i < level2Player.Count; i++) {
			if (level2Player [i].Contains ("000")) {
				round2.tournamentTexts [i].text = "";
			} else {
				round2.tournamentTexts [i].text = "" + level2Player [i];
			}
		}

		for (int i = 0; i < level3Player.Count; i++) {
			if (level3Player [i].Contains ("000")) {
				round3.tournamentTexts [i].text = "";
			} else {
				round3.tournamentTexts [i].text = "" + level3Player [i];
			}
		}
	}

	public void ResetOnlineTournament ()
	{
		for (int i = 0; i < round1.tournamentTexts.Length; i++) {
			round1.tournamentTexts [i].text = "";
		}
		for (int i = 0; i < round2.tournamentTexts.Length; i++) {
			round2.tournamentTexts [i].text = "";
		}
		for (int i = 0; i < round3.tournamentTexts.Length; i++) {
			round3.tournamentTexts [i].text = "";
		}
	}
    */
}
/*
[System.Serializable]
public class OnlineRound1
{
	public Text[] tournamentTexts;
}

[System.Serializable]
public class OnlineRound2
{
	public Text[] tournamentTexts;
}

[System.Serializable]
public class OnlineRound3
{
	public Text[] tournamentTexts;
}
*/