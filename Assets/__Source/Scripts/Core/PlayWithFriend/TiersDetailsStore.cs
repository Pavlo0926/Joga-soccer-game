using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Jiweman;

public class TiersDetailsStore : MonoBehaviour
{
	public string tierBackGroundimg;
	public string tierChallengeId;
	public string tierEntryFee;
	public string tierGoalsNeed;
	public string tierLogoImg;
	public string tierName;
	public string tierPointType;
	public string tierPrize;
	public string tierStatus;

	public void SendChallegeToPlayer ()
	{
		// commented --@sud
		// WebServicesHandler.SharedInstance.GetSendChallenge (TiersValuesAssign.instance.opponentFriendID, tierChallengeId);
		StartCoroutine (ActiveSentChallengePopup ());
	}

	IEnumerator ActiveSentChallengePopup ()
	{
		Joga_FriendsManager.Instance.ScrollerPanelInSelectTier.SetActive (false);
		Joga_FriendsManager.Instance.SendSuccessPanelInSelectTier.SetActive (true);
		yield return new WaitForSeconds (2.0f);

		Joga_FriendsManager.Instance.SendSuccessPanelInSelectTier.SetActive (false);
		Joga_FriendsManager.Instance.TiersPanel.SetActive (false);
	}
}
