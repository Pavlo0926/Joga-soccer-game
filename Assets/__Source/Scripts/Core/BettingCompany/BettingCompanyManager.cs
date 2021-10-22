using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BettingCompanyManager : MonoBehaviour
{
	public Transform bettingCompanyDataParent;

	public GameObject bettingCompanyDataPrefab;

	public static BettingCompanyManager instance;

	void Awake ()
	{
		if(!instance)
		instance = this;
	}

	// Use this for initialization
	// public void AssignBettingCompanyData ()
	// {
	
	// 	for (int j = 0; j < WebServicesHandler.SharedInstance.bettingCompanyDetails.Count; j++) {
	// 		GameObject gm = Instantiate (bettingCompanyDataPrefab, Vector3.zero, Quaternion.identity)as GameObject;
	// 		gm.name = "Company" + j;
	// 		gm.transform.SetParent(bettingCompanyDataParent);
	// 		gm.transform.localScale = Vector3.one;
	// 		gm.GetComponent <RectTransform> ().anchoredPosition3D = Vector3.zero;

	// 		gm.GetComponent <AssignBettingCompanyDetails> ().email = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].email;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().companyURL = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].companyURL;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().companyRP = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].companyRP;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().companyName = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].companyName;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().companyId = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].companyId;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().registeredDate = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].registeredDate;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().registerStatus = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].registerStatus;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().RP = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].RP;
	// 		gm.GetComponent <AssignBettingCompanyDetails> ().selectStatus = WebServicesHandler.SharedInstance.bettingCompanyDetails [j].status;
	// 	}
	// }



	// public void DeSelectCompany (string Companyid)
	// {
	// 	for (int j = 0; j < bettingCompanyDataParent.childCount; j++) {
		
	// 		if (Companyid == bettingCompanyDataParent.GetChild (j).GetComponent <AssignBettingCompanyDetails> ().companyId) {
	// 			bettingCompanyDataParent.GetChild (j).GetComponent <Button> ().interactable = false;
	// 			bettingCompanyDataParent.GetChild (j).GetComponent <AssignBettingCompanyDetails> ().selectStatus = "Active";
	// 		} else {
	// 			bettingCompanyDataParent.GetChild (j).GetComponent <Button> ().interactable = true;
	// 			bettingCompanyDataParent.GetChild (j).GetComponent <AssignBettingCompanyDetails> ().selectStatus = "Inactive";

	// 		}
	// 	}
	// }

	// public void DeSelectAllCompany ()
	// {
	// 	for (int j = 0; j < bettingCompanyDataParent.childCount; j++) {
	// 		bettingCompanyDataParent.GetChild (j).GetComponent <Button> ().interactable = true;
	// 	}
	// }

	// public void ClearBettingCompanyData ()
	// {
	// 	WebServicesHandler.SharedInstance.bettingCompanyDetails.Clear ();
	// 	for (int j = 0; j < bettingCompanyDataParent.childCount; j++) {
	// 		Destroy (bettingCompanyDataParent.GetChild (j).gameObject);
	// 	}
	// }

}
