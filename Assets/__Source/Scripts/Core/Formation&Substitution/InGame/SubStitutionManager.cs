using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text;
using System;
public class SubStitutionManager : MonoBehaviour
{
    /*
    public GameObject[] myActiveGamePlayDisc;

    public List<GameObject> ActiveDiscs;
    public List<GameObject> InActiveDiscs;

    public Transform inActiveObjectParent;

    public GameObject inActiveObjectPrefab;

    public Text fValue;
    public Text tValue;
    public Text aValue;

    public Text SubAtkHit;
    public Text subDefHit;

    public GameObject SwipeButton;

    public Image forceValue, aimValue, timeValue, healthValue, attackingCap, protectiveCap;

    public Image SubprotectiveCapImage;
    public Image SubattackingCapImage;

    public Text totalStatsValue;

    public GameObject PlayerDetailLabel;

    bool isFirstTimeDataAssign;

    public static SubStitutionManager instance;

    void Awake ()
    {
        instance = this;
    }
    // Use this for initialization
    void OnEnable ()
    {
        assignActiveNInActiveObjValue ();
        FirstAssignData ();
      
    }

    public void FirstAssignData ()
    {
        for (int i = 0; i < myActiveGamePlayDisc.Length; i++) {
            myActiveGamePlayDisc [i].SetActive (false);
        }
        myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].SetActive (true);
        updateStatsValue ();
    }

    public void updateStatsValue ()
    {
        forceValue.fillAmount = myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().forceData / BumpStaminaManager.instance.initialvalueForce;
        aimValue.fillAmount   = myActiveGamePlayDisc  [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().aimData / BumpStaminaManager.instance.initialvalueAIM;
        timeValue.fillAmount  = myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().timeData / BumpStaminaManager.instance.initialvalueTime;
        AssignHeathSignColorValue ();
        //ActiveProtacativeCap ();
        //ActiveAttackingCap ();
        fValue.text = Math.Round ((myActiveGamePlayDisc[BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent<ActivePlayerStatminaValueAssign>().forceData / BumpStaminaManager.instance.initialvalueForce)*100).ToString();
        aValue.text = Math.Round((myActiveGamePlayDisc[BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent<ActivePlayerStatminaValueAssign>().aimData / BumpStaminaManager.instance.initialvalueAIM)*100).ToString ();
        tValue.text = Mathf.Round((myActiveGamePlayDisc[BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent<ActivePlayerStatminaValueAssign>().timeData / BumpStaminaManager.instance.initialvalueTime)*100).ToString();
   
     }

    public void AssignHeathSignColorValue ()
    {
        float max_aid = (myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().forceData + myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().timeData + myActiveGamePlayDisc [BumpStaminaManager.instance.playerObjectCouterFromArray].GetComponent <ActivePlayerStatminaValueAssign> ().aimData) ;

        float persantangeValue = (max_aid / (BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime))*100;

        if (85 <= persantangeValue) {
            healthValue.color = new Color (0f, 0.7450f, 0f, 1f);
        } else if (85 > persantangeValue && 65 <= persantangeValue) {
            healthValue.color = new Color (0f, 1f, 1f, 1f);
        } else if (65 > persantangeValue && 45 <= persantangeValue) {
            healthValue.color = new Color (1f, 1f, 0.02f, 1f);
        } else if (45 > persantangeValue && 25 <= persantangeValue) {
            healthValue.color = new Color (1f, 0.25f, 0.019f, 1f);
        } else if (25 > persantangeValue && 5<= persantangeValue) {
            healthValue.color = new Color(1f, 0.55f, 0.019f, 1f);
        }else if(5> persantangeValue)
            healthValue.color = new Color(1f, 0f, 0f, 1f);

        totalStatsValue.text = "" + persantangeValue;
    }

    public void ActiveProtacativeCap ()
    {
        if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isElephant) {
            protectiveCap.gameObject.SetActive (true);
            protectiveCap.sprite = BumpStaminaManager.instance.protectingCapSprites [0];
        } else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isRhino) {
            protectiveCap.gameObject.SetActive (true);
            protectiveCap.sprite = BumpStaminaManager.instance.protectingCapSprites [1];
        } else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isLion) {
            protectiveCap.gameObject.SetActive (true);
            protectiveCap.sprite = BumpStaminaManager.instance.protectingCapSprites [2];
        } else {
            protectiveCap.gameObject.SetActive (false);
        }
    }

    public void ActiveAttackingCap ()
    {
        if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isChromeCap) {
            attackingCap.gameObject.SetActive (true);
            attackingCap.sprite = BumpStaminaManager.instance.acttackingCapSprites [0];
        } else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isGoldCap) {
            attackingCap.gameObject.SetActive (true);
            attackingCap.sprite = BumpStaminaManager.instance.acttackingCapSprites [1];
        } else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isSilverCap) {
            attackingCap.gameObject.SetActive (true);
            attackingCap.sprite = BumpStaminaManager.instance.acttackingCapSprites [2];
        } else {
            attackingCap.gameObject.SetActive (false);
        }
    }

    public void assignActiveNInActiveObjValue ()
    {

        if (!isFirstTimeDataAssign) {
            isFirstTimeDataAssign = true;
            WebServicesHandler.SharedInstance.ActivePlayerList.Clear ();
            WebServicesHandler.SharedInstance.InActivePlayerList.Clear ();
            
            for (int i = 0; i < WebServicesHandler.SharedInstance.statsValue.discList.Count; i++) {
                
                if (WebServicesHandler.SharedInstance.statsValue.discList [i].mStatus == "Active") {
                    WebServicesHandler.SharedInstance.ActivePlayerList.Add (WebServicesHandler.SharedInstance.statsValue.discList [i]);
                }
                
                if (WebServicesHandler.SharedInstance.statsValue.discList [i].mStatus == "InActive") {
                    WebServicesHandler.SharedInstance.InActivePlayerList.Add (WebServicesHandler.SharedInstance.statsValue.discList [i]);
                }
            }
        }

        for (int i = 0; i < WebServicesHandler.SharedInstance.ActivePlayerList.Count; i++) {
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().idDisc = WebServicesHandler.SharedInstance.ActivePlayerList [i].mDiskId;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().diskName = WebServicesHandler.SharedInstance.ActivePlayerList [i].mDiskName;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().forceData = WebServicesHandler.SharedInstance.ActivePlayerList [i].mForceData;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().aimData = WebServicesHandler.SharedInstance.ActivePlayerList [i].mAim;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().timeData = WebServicesHandler.SharedInstance.ActivePlayerList [i].mTime;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().staminaData = WebServicesHandler.SharedInstance.ActivePlayerList [i].mStamina;
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().statusData = WebServicesHandler.SharedInstance.ActivePlayerList [i].mStatus;

            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().idDisc = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_id;
            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().diskName = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_name;
            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().forceData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force;
            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().aimData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim;
            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().timeData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer;
            //// ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().staminaData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().sta
            //ActiveDiscs[i].GetComponent<ActivePlayerStatminaValueAssign>().statusData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_status;

           // GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_id;


        }


        if (inActiveObjectParent.childCount == 1) {

            for (int j = 0; j < WebServicesHandler.SharedInstance.InActivePlayerList.Count; j++) {
                GameObject gm = Instantiate (inActiveObjectPrefab, Vector3.zero, Quaternion.identity)as GameObject;
                gm.name = "SwipePlayer" + j;
                gm.transform.parent = inActiveObjectParent;
                gm.transform.localScale = Vector3.one;
                gm.GetComponent <RectTransform> ().anchoredPosition3D = Vector3.zero;

                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().idDisc = WebServicesHandler.SharedInstance.InActivePlayerList [j].mDiskId;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().diskName = WebServicesHandler.SharedInstance.InActivePlayerList [j].mDiskName;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().forceData = WebServicesHandler.SharedInstance.InActivePlayerList [j].mForceData;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().aimData = WebServicesHandler.SharedInstance.InActivePlayerList [j].mAim;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().timeData = WebServicesHandler.SharedInstance.InActivePlayerList [j].mTime;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().staminaData = WebServicesHandler.SharedInstance.InActivePlayerList [j].mStamina;
                gm.GetComponentInChildren <InActivePlayerStatminaValueAssign> ().statusData = WebServicesHandler.SharedInstance.InActivePlayerList [j].mStatus;

                InActiveDiscs.Add (gm);

                if (SceneManager.GetActiveScene ().name == "InGame") {
                    BumpStaminaManager.instance.SwapPlayer_timerList.Add (gm);
                }
            }
        } else {

            for (int i = 0; i < InActiveDiscs.Count; i++) {
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().idDisc = WebServicesHandler.SharedInstance.InActivePlayerList [i].mDiskId;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().diskName = WebServicesHandler.SharedInstance.InActivePlayerList [i].mDiskName;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().forceData = WebServicesHandler.SharedInstance.InActivePlayerList [i].mForceData;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().aimData = WebServicesHandler.SharedInstance.InActivePlayerList [i].mAim;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().timeData = WebServicesHandler.SharedInstance.InActivePlayerList [i].mTime;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().staminaData = WebServicesHandler.SharedInstance.InActivePlayerList [i].mStamina;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().statusData = WebServicesHandler.SharedInstance.InActivePlayerList [i].mStatus;
            }

        }
    }
    public void SubActiveProtacativeCap()
    {
        if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isChromeCap)
        {
            SubprotectiveCapImage.gameObject.SetActive(true);
            SubprotectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites[0];


        }
        else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isGoldCap)
        {
            SubprotectiveCapImage.gameObject.SetActive(true);
            SubprotectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites[1];

         
        }
        else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isSilverCap)
        {
            SubprotectiveCapImage.gameObject.SetActive(true);
            SubprotectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites[2];

        }
        else
        {
            SubprotectiveCapImage.gameObject.SetActive(false);

        }
    }

    public void SubActiveAttackingCap()
    {
        if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isElephant)
        {
            SubattackingCapImage.gameObject.SetActive(true);
         
           SubattackingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites[0];
        }
        else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isRhino)
        {
           SubattackingCapImage.gameObject.SetActive(true);
           SubattackingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites[1];

        
        }
        else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent<PlayerStats>().isLion)
        {
           SubattackingCapImage.gameObject.SetActive(true);


          
          SubattackingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites[2];

        }
        else
        {
            SubattackingCapImage.gameObject.SetActive(false);

         
        }
    }

    int SwipePlayerid, OtherPlayerid;

    public void SwipePlayer ()
    {
        SwipePlayerid = 0;
        OtherPlayerid = 0;

        for (int i = 0; i < ActiveDiscs.Count; i++) {
            Debug.Log (ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().diskName);
            if (ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().isSelectedDisc) {
                SwipePlayerid = i;
                ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().isSelectedDisc = false;
            }
        }
        for (int i = 0; i < InActiveDiscs.Count; i++) {
            if (InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().isSelectedDisc) {
                OtherPlayerid = i;
                InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().isSelectedDisc = false;
            }
        }

        playerSwipping (SwipePlayerid, OtherPlayerid);
        SwipeButton.SetActive (false);
    }

    public void playerSwipping (int player1, int player2)
    {
        if (SceneManager.GetActiveScene ().name == "InGame") {
            float tempTimer = GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().obj.GetComponent <ActivePlayerStatminaValueAssign> ().player_timer_act;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().obj.GetComponent <ActivePlayerStatminaValueAssign> ().player_timer_act = BumpStaminaManager.instance.SwapPlayer_timerList [player2].GetComponent <InActivePlayerStatminaValueAssign> ().player_timer_Inact;
            BumpStaminaManager.instance.SwapPlayer_timerList [player2].GetComponent <InActivePlayerStatminaValueAssign> ().player_timer_Inact = tempTimer;
        }

        float id = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().idDisc;
        string diskName = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().diskName;
        float force = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().forceData;
        float aim = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().aimData;
        float time = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().timeData;
        float stamina = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().staminaData;
        string status = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().statusData;


        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().idDisc = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().idDisc;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().diskName = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().diskName;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().forceData = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().forceData;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().aimData = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().aimData;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().timeData = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().timeData;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().staminaData = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().staminaData;
        ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().statusData = "InActive";

        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().idDisc = id;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().diskName = diskName;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().forceData = force;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().aimData = aim;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().timeData = time;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().staminaData = stamina;
        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().statusData = "Active";

        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mDiskId = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().idDisc;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mDiskName = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().diskName;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mForceData = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().forceData;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mAim = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().aimData;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mTime = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().timeData;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mStamina = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().staminaData;
        WebServicesHandler.SharedInstance.ActivePlayerList [player1].mStatus = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().statusData;

        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mDiskId = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().idDisc;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mDiskName = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().diskName;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mForceData = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().forceData;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mAim = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().aimData;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mTime = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().timeData;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mStamina = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().staminaData;
        WebServicesHandler.SharedInstance.InActivePlayerList [player2].mStatus = InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().statusData;

        if (SceneManager.GetActiveScene ().name == "InGame") {
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().mDisc_id = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().idDisc;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().discName = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().diskName;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().Force = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().forceData;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().mDisc_aim = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().aimData;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().timePlayer = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().timeData;
            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().mDisc_status = ActiveDiscs [player1].GetComponent <ActivePlayerStatminaValueAssign> ().statusData;

            GlobalGameManager.SharedInstance.allPlayer [player1].GetComponent <playerController> ().UpdateStatsValue ();
        }

        WebServicesHandler.SharedInstance.updateDisk (
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mForceData, 
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mTime,
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mStamina, 
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mAim, 
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mDiskName,
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mDiskId,
            WebServicesHandler.SharedInstance.ActivePlayerList [player1].mStatus
        );

        WebServicesHandler.SharedInstance.updateDisk (
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mForceData, 
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mTime,
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mStamina, 
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mAim, 
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mDiskName,
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mDiskId,
            WebServicesHandler.SharedInstance.InActivePlayerList [player2].mStatus
        );

        InActiveDiscs [player2].GetComponent <InActivePlayerStatminaValueAssign> ().AssignValueofDisk ();
        updateStatsValue ();
        AssignSubDiscDetailsShow.instance.assignValue ();
    }



    public void DeselectAllButton ()
    {
        SwipeButton.SetActive (false);

        PlayerDetailLabel.SetActive (false);

        for (int i = 0; i < ActiveDiscs.Count; i++) {
            ActiveDiscs [i].GetComponent <ActivePlayerStatminaValueAssign> ().isSelectedDisc = false;
        }
        for (int i = 0; i < InActiveDiscs.Count; i++) {
            InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().isSelectedDisc = false;
        }
    }
    */
}
