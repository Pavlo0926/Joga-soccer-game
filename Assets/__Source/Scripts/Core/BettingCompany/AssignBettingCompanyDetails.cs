using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignBettingCompanyDetails : MonoBehaviour
{

	public string email;
	public string companyURL;
	public string registeredDate;
	public string registerStatus;
	public string RP;
	public string companyRP;
	public string companyId;
	public string companyName;
	public string selectStatus;

	public Text companyNameText;
	public Text companyRPText;



	// Use this for initialization
	void Start ()
	{
		// companyNameText.text = "" + companyName;
		// companyRPText.text = "" + companyRP;

		// if (selectStatus == "Active") {
		// 	GameManager.SharedInstance.PlayerRPAmountValue = int.Parse (RP);
		// 	UIController.SharedInstance.PlayerRPText.text = "" + RP;
		// 	this.GetComponent <Button> ().interactable = false;
		// } else {
		// 	this.GetComponent <Button> ().interactable = true;
		// }
	}

	// public void SelectBettingCompany ()
	// {
	// 	BettingCompanyManager.instance.DeSelectAllCompany ();
	// 	this.GetComponent <Button> ().interactable = false;
	// 	if (registerStatus == "false") {
	// 		WebServicesHandler.SharedInstance.GetSelectBettingCompany (companyId);
	// 	} else {
	// 		WebServicesHandler.SharedInstance.GetUpdateRPUserBettingCompany (companyId, RP, "Active");
	// 	}
	// 	PlayerPrefs.SetInt ("BettingCompanyID", int.Parse (companyId));
	// }
}
