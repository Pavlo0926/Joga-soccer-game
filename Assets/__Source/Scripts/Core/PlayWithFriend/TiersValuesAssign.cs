using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TiersValuesAssign : MonoBehaviour
{

    public Transform ParentObject;

    public GameObject TiersDetailsPrefab;

    public string opponentFriendID;

    public static TiersValuesAssign instance;

    void Awake()
    {
        instance = this;
    }

    // Use this for initialization
    void Start()
    {
        // commented --@sud

        // for (int i = 0; i < WebServicesHandler.SharedInstance.tiersDetailsLists.Count; i++) { 
        // 	GameObject gm = Instantiate (TiersDetailsPrefab, Vector3.zero, Quaternion.identity)as GameObject;
        // 	//			gm.GetComponent <Image> ().sprite = WebServicesHandler.SharedInstance.formations [i].formationImage;
        // 	gm.name = "TiersObject" + i;
        // 	gm.transform.parent = ParentObject;
        // 	gm.transform.localScale = Vector3.one;
        // 	gm.GetComponent <RectTransform> ().anchoredPosition3D = Vector3.zero;

        // 	gm.GetComponent <TiersDetailsStore> ().tierBackGroundimg = WebServicesHandler.SharedInstance.tiersDetailsLists [i].backgorundImg;
        // 	gm.GetComponent <TiersDetailsStore> ().tierChallengeId = WebServicesHandler.SharedInstance.tiersDetailsLists [i].ChallengeId;
        // 	gm.GetComponent <TiersDetailsStore> ().tierEntryFee = WebServicesHandler.SharedInstance.tiersDetailsLists [i].entryFee;
        // 	gm.GetComponent <TiersDetailsStore> ().tierGoalsNeed = WebServicesHandler.SharedInstance.tiersDetailsLists [i].goalsNeed;
        // 	gm.GetComponent <TiersDetailsStore> ().tierLogoImg = WebServicesHandler.SharedInstance.tiersDetailsLists [i].logoImg;
        // 	gm.GetComponent <TiersDetailsStore> ().tierName = WebServicesHandler.SharedInstance.tiersDetailsLists [i].name;
        // 	gm.GetComponent <TiersDetailsStore> ().tierPointType = WebServicesHandler.SharedInstance.tiersDetailsLists [i].pointType;
        // 	gm.GetComponent <TiersDetailsStore> ().tierPrize = WebServicesHandler.SharedInstance.tiersDetailsLists [i].prize;
        // 	gm.GetComponent <TiersDetailsStore> ().tierStatus = WebServicesHandler.SharedInstance.tiersDetailsLists [i].status;

        // }

    }
}
