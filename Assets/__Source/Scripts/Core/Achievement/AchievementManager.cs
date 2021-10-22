using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementManager : MonoBehaviour
{

	public Transform achievementDataParent;

	public GameObject[] achievementDataPrefab;

	public static AchievementManager instance;

	void Awake ()
	{
		instance = this;
	}

/*
	public void AssignAchievementData ()
	{
		for (int j = 0; j < WebServicesHandler.SharedInstance.allAchievementDetails.Count; j++) {

			GameObject gm;

			if (WebServicesHandler.SharedInstance.allAchievementDetails [j].rewardType == "Gold") {

				gm = Instantiate (achievementDataPrefab [0], Vector3.zero, Quaternion.identity)as GameObject;
				gm.name = "Achievement" + j;
				gm.transform.SetParent(achievementDataParent);
				gm.transform.localScale = Vector3.one;
				gm.GetComponent <RectTransform> ().anchoredPosition3D = Vector3.zero;

				gm.GetComponent <AssignAchievementData> ().achievementId = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementId;
				gm.GetComponent <AssignAchievementData> ().achievementName = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementName;
				gm.GetComponent <AssignAchievementData> ().achievementType = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementType;
				gm.GetComponent <AssignAchievementData> ().amount = WebServicesHandler.SharedInstance.allAchievementDetails [j].amount;
				gm.GetComponent <AssignAchievementData> ().description = WebServicesHandler.SharedInstance.allAchievementDetails [j].description;
				gm.GetComponent <AssignAchievementData> ().reward = WebServicesHandler.SharedInstance.allAchievementDetails [j].reward;
				gm.GetComponent <AssignAchievementData> ().rewardType = WebServicesHandler.SharedInstance.allAchievementDetails [j].rewardType;
				gm.GetComponent <AssignAchievementData> ().status = WebServicesHandler.SharedInstance.allAchievementDetails [j].status;

			} else {
				gm = Instantiate (achievementDataPrefab [1], Vector3.zero, Quaternion.identity)as GameObject;
				gm.name = "Achievement" + j;
				gm.transform.SetParent(achievementDataParent);
				gm.transform.localScale = Vector3.one;
				gm.GetComponent <RectTransform> ().anchoredPosition3D = Vector3.zero;

				gm.GetComponent <AssignAchievementData> ().achievementId = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementId;
				gm.GetComponent <AssignAchievementData> ().achievementName = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementName;
				gm.GetComponent <AssignAchievementData> ().achievementType = WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementType;
				gm.GetComponent <AssignAchievementData> ().amount = WebServicesHandler.SharedInstance.allAchievementDetails [j].amount;
				gm.GetComponent <AssignAchievementData> ().description = WebServicesHandler.SharedInstance.allAchievementDetails [j].description;
				gm.GetComponent <AssignAchievementData> ().reward = WebServicesHandler.SharedInstance.allAchievementDetails [j].reward;
				gm.GetComponent <AssignAchievementData> ().rewardType = WebServicesHandler.SharedInstance.allAchievementDetails [j].rewardType;
				gm.GetComponent <AssignAchievementData> ().status = WebServicesHandler.SharedInstance.allAchievementDetails [j].status;
			}

			if (gm.GetComponent <AssignAchievementData> ().status == "Locked") {
				gm.GetComponent <AssignAchievementData> ().ClaimRewardButton.interactable = false;
			} else {
				gm.GetComponent <AssignAchievementData> ().ClaimRewardButton.interactable = true;
			}
		}
	}

	public void DeleteAllAchievement ()
	{
		WebServicesHandler.SharedInstance.allAchievementDetails.Clear ();
		for (int j = 0; j < achievementDataParent.childCount; j++) {
			Destroy (achievementDataParent.GetChild (j).gameObject);
		}
	}
*/
}
