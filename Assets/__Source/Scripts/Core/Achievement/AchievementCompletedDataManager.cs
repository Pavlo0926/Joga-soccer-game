using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementCompletedDataManager : MonoBehaviour
{
	public static AchievementCompletedDataManager instance;

	void Awake ()
	{
		instance = this;
	}

	public void CheckCompleteAchievement ()
	{
		// commented --@sud
		// for (int j = 0; j < WebServicesHandler.SharedInstance.allAchievementDetails.Count; j++) {
		// 	if (int.Parse (WebServicesHandler.SharedInstance.allAchievementDetails [j].amount) <= GameManager.SharedInstance.goal && WebServicesHandler.SharedInstance.allAchievementDetails [j].status == "Locked") {
		// 		WebServicesHandler.SharedInstance.GetUnlockAchievement (WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementId);
		// 		Debug.Log ("Enter The condition" + WebServicesHandler.SharedInstance.allAchievementDetails [j].achievementId);
		// 	}

		// }

	}


}
