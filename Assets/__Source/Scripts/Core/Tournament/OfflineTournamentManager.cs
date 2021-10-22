using FastSkillTeam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OfflineTournamentManager : MonoBehaviour
{
	public Round1 round1;
	public Round2 round2;
	public Round3 round3;
	public List<Team> aimainTeams;

	public List<Team> aiTempTeamsList;
	public List<Team> aiSecondRoundTeamsList;
	public Team aiThirdTeam;

	public Sprite playerSprite;

	public Sprite DefaultSprite;

	public static OfflineTournamentManager Instance;

	void Awake ()
	{
		Instance = this;
	}

	// Use this for initialization
	void Start ()
	{
		
	}

	public void AssignFirstRound ()
	{
		int[] myRandomList = getUniqueRandomArray (0, aimainTeams.Count, 7);

		aiTempTeamsList.Clear ();

		for (int i = 0; i < myRandomList.Length; i++) {
			aiTempTeamsList.Add (aimainTeams [myRandomList [i]]); 
		}
		GameManager.Instance.storeAITeamsList = aiTempTeamsList;

		round1.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
		round1.tournamentImgaes [0].sprite = playerSprite;
		round1.tournamentRating [0].text = "2";

		for (int i = 1; i < aiTempTeamsList.Count + 1; i++) {
			round1.tournamentTexts [i].text = "" + aiTempTeamsList [i - 1].teamName;
			round1.tournamentImgaes [i].sprite = aiTempTeamsList [i - 1].playerImage;
			round1.tournamentRating [i].text = "" + aiTempTeamsList [i - 1].teamRating;
		}
	}

	public void AddSecondRoundList ()
	{
		round1.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
        round1.tournamentImgaes [0].sprite = playerSprite;
		round1.tournamentRating [0].text = "2";

		for (int i = 1; i < aiTempTeamsList.Count + 1; i++) {
			round1.tournamentTexts [i].text = "" + aiTempTeamsList [i - 1].teamName;
			round1.tournamentImgaes [i].sprite = aiTempTeamsList [i - 1].playerImage;
			round1.tournamentRating [i].text = "" + aiTempTeamsList [i - 1].teamRating;
		}


		aiSecondRoundTeamsList.Clear ();
		if (GameManager.Instance.isPlayerRound1Win) {
			round2.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
            round2.tournamentImgaes [0].sprite = playerSprite;
			round2.tournamentRating [0].text = "2";

			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [1], aiTempTeamsList [2]));
			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [3], aiTempTeamsList [4]));
			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [5], aiTempTeamsList [6]));
			
			for (int i = 1; i < aiSecondRoundTeamsList.Count + 1; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i - 1].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i - 1].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i - 1].teamRating;
			}
		} else {
			aiSecondRoundTeamsList.Add (aiTempTeamsList [0]);
			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [1], aiTempTeamsList [2]));
			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [3], aiTempTeamsList [4]));
			aiSecondRoundTeamsList.Add (CheckWinner (aiTempTeamsList [5], aiTempTeamsList [6]));

			for (int i = 0; i < aiSecondRoundTeamsList.Count; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i].teamRating;
			}
		}

		GameManager.Instance.storeAISecondRoundTeamsList = aiSecondRoundTeamsList;
	}

	public void AddThirdRoundList ()
	{
		round1.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
        round1.tournamentImgaes [0].sprite = playerSprite;
		round1.tournamentRating [0].text = "2";

		for (int i = 1; i < aiTempTeamsList.Count + 1; i++) {
			round1.tournamentTexts [i].text = "" + aiTempTeamsList [i - 1].teamName;
			round1.tournamentImgaes [i].sprite = aiTempTeamsList [i - 1].playerImage;
			round1.tournamentRating [i].text = "" + aiTempTeamsList [i - 1].teamRating;
		}
			
		if (GameManager.Instance.isPlayerRound1Win) {
			round2.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
            round2.tournamentImgaes [0].sprite = playerSprite;
			round2.tournamentRating [0].text = "2";

			for (int i = 1; i < aiSecondRoundTeamsList.Count + 1; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i - 1].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i - 1].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i - 1].teamRating;
			}
		} else {

			for (int i = 0; i < aiSecondRoundTeamsList.Count; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i].teamRating;
			}
		}

		if (GameManager.Instance.isPlayerRound2Win) {
			round3.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
            round3.tournamentImgaes [0].sprite = playerSprite;
			round3.tournamentRating [0].text = "2";

			aiThirdTeam = CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]);
			round3.tournamentTexts [1].text = "" + aiThirdTeam.teamName;
			round3.tournamentImgaes [1].sprite = aiThirdTeam.playerImage;
			round3.tournamentRating [1].text = "" + aiThirdTeam.teamRating;
		} else {
			if (aiSecondRoundTeamsList.Count == 4) {
				round3.tournamentTexts [0].text = "" + CheckWinner (aiSecondRoundTeamsList [0], aiSecondRoundTeamsList [1]).teamName;
				round3.tournamentImgaes [0].sprite = CheckWinner (aiSecondRoundTeamsList [0], aiSecondRoundTeamsList [1]).playerImage;
				round3.tournamentRating [0].text = "" + CheckWinner (aiSecondRoundTeamsList [0], aiSecondRoundTeamsList [1]).teamRating;

				round3.tournamentTexts [1].text = "" + CheckWinner (aiSecondRoundTeamsList [2], aiSecondRoundTeamsList [3]).teamName;
				round3.tournamentImgaes [1].sprite = CheckWinner (aiSecondRoundTeamsList [2], aiSecondRoundTeamsList [3]).playerImage;
				round3.tournamentRating [1].text = "" + CheckWinner (aiSecondRoundTeamsList [2], aiSecondRoundTeamsList [3]).teamRating;
			
			} else if (aiSecondRoundTeamsList.Count == 3) {
				round3.tournamentTexts [0].text = "" + aiSecondRoundTeamsList [0].teamName;
				round3.tournamentImgaes [0].sprite = aiSecondRoundTeamsList [0].playerImage;
				round3.tournamentRating [0].text = "" + aiSecondRoundTeamsList [0].teamRating;

				round3.tournamentTexts [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamName;
				round3.tournamentImgaes [1].sprite = CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).playerImage;
				round3.tournamentRating [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamRating;
			}
		}

	}

	public void LostFirstRoundTournament ()
	{
		AddSecondRoundList ();
		AddThirdRoundList ();
	}

	public void LostSecondRoundTournament ()
	{
		AddThirdRoundList ();
	}


	public void LostThirdRoundTournament ()
	{
		ShowAllTournamentData ();
	}


	public void ShowAllTournamentData ()
	{
		round1.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
        round1.tournamentImgaes [0].sprite = playerSprite;
		round1.tournamentRating [0].text = "2";

		for (int i = 1; i < aiTempTeamsList.Count + 1; i++) {
			round1.tournamentTexts [i].text = "" + aiTempTeamsList [i - 1].teamName;
			round1.tournamentImgaes [i].sprite = aiTempTeamsList [i - 1].playerImage;
			round1.tournamentRating [i].text = "" + aiTempTeamsList [i - 1].teamRating;
		}



		if (GameManager.Instance.isPlayerRound1Win) {
			round2.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
            round2.tournamentImgaes [0].sprite = playerSprite;
			round2.tournamentRating [0].text = "2";

			for (int i = 1; i < aiSecondRoundTeamsList.Count + 1; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i - 1].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i - 1].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i - 1].teamRating;
			}
		} else {

			for (int i = 0; i < aiSecondRoundTeamsList.Count; i++) {
				round2.tournamentTexts [i].text = "" + aiSecondRoundTeamsList [i].teamName;
				round2.tournamentImgaes [i].sprite = aiSecondRoundTeamsList [i].playerImage;
				round2.tournamentRating [i].text = "" + aiSecondRoundTeamsList [i].teamRating;
			}

		}



		if (GameManager.Instance.isPlayerRound2Win) {
			round3.tournamentTexts [0].text = FST_SettingsManager.PlayerName;
            round3.tournamentImgaes [0].sprite = playerSprite;
			round3.tournamentRating [0].text = "2";

			round3.tournamentTexts [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamName;
			round3.tournamentImgaes [1].sprite = CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).playerImage;
			round3.tournamentRating [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamRating;
		} else {
			round3.tournamentTexts [0].text = "" + aiSecondRoundTeamsList [0].teamName;
			round3.tournamentImgaes [0].sprite = aiSecondRoundTeamsList [0].playerImage;
			round3.tournamentRating [0].text = "" + aiSecondRoundTeamsList [0].teamRating;

			round3.tournamentTexts [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamName;
			round3.tournamentImgaes [1].sprite = CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).playerImage;
			round3.tournamentRating [1].text = "" + CheckWinner (aiSecondRoundTeamsList [1], aiSecondRoundTeamsList [2]).teamRating;
		}
	}



	Team CheckWinner (Team firstPlayerTeam, Team secondPlayerTeam)
	{
		if (firstPlayerTeam.teamRating > secondPlayerTeam.teamRating) {
			return firstPlayerTeam;
		} else {
			return secondPlayerTeam;
		}
	}

	public static int[] getUniqueRandomArray (int min, int max, int count)
	{
		int[] result = new int[count];
		List<int> numbersInOrder = new List<int> ();
		for (var x = min; x < max; x++) {
			numbersInOrder.Add (x);
		}
		for (var x = 0; x < count; x++) {
			var randomIndex = Random.Range (0, numbersInOrder.Count);
			result [x] = numbersInOrder [randomIndex];
			numbersInOrder.RemoveAt (randomIndex);
		}

		return result;
	}

	public void ResetTournament ()
	{
		for (int i = 0; i < round1.tournamentImgaes.Length; i++) {
			round1.tournamentImgaes [i].sprite = DefaultSprite;
			round1.tournamentRating [i].text = "";
			round1.tournamentTexts [i].text = "";
		}
		for (int i = 0; i < round2.tournamentImgaes.Length; i++) {
			round2.tournamentImgaes [i].sprite = DefaultSprite;
			round2.tournamentRating [i].text = "";
			round2.tournamentTexts [i].text = "";
		}
		for (int i = 0; i < round3.tournamentImgaes.Length; i++) {
			round3.tournamentImgaes [i].sprite = DefaultSprite;
			round3.tournamentRating [i].text = "";
			round3.tournamentTexts [i].text = "";
		}
	}

}

[System.Serializable]
public class Round1
{
	public Image[] tournamentImgaes;
	public Text[] tournamentTexts;
	public Text[] tournamentRating;
}

[System.Serializable]
public class Round2
{
	public Image[] tournamentImgaes;
	public Text[] tournamentTexts;
	public Text[] tournamentRating;
}

[System.Serializable]
public class Round3
{
	public Image[] tournamentImgaes;
	public Text[] tournamentTexts;
	public Text[] tournamentRating;
}

[System.Serializable]
public class Team
{
	public string teamName;
	public float teamRating;
	public Sprite playerImage;
}