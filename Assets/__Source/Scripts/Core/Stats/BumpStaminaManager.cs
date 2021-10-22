using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class BumpStaminaManager : MonoBehaviour
{/*
    public float BSForceDrink_A_Percentage = 0.25f;
    public float BSForceDrink_B_Percentage = 0.20f;
    public float BSForceDrink_C_Percentage = 0.10f;

    public float BSTimeDrink_A_Percentage = 0.25f;
    public float BSTimeDrink_B_Percentage = 0.20f;
    public float BSTimeDrink_C_Percentage = 0.10f;

    public float BSAIMDrink_A_Percentage = 0.25f;
    public float BSAIMDrink_B_Percentage = 0.20f;
    public float BSAIMDrink_C_Percentage = 0.10f;

    public float SuperAidBox_Percentage = 25f;
    public float RegularAidBox_Percentage = 20f;
    public float PatchAidBox_Percentage = 10f;

    public float MinX_Percentage = 5f;

    public int BSForceDrink_A_Count;
    public int BSForceDrink_B_Count;
    public int BSForceDrink_C_Count;

    public int Max_elephantCount;
    public int Max_rhinoCount;
    public int Max_linoCount;

    // Initial values for FAT...

    public float initialvalueTime = 20f;
    public float initialvalueForce = 3.5f;
    public float initialvalueAIM = 10f;

    public bool isElephant_btn;
    public bool isRhino_btn;
    public bool isLion_btn;

    public bool isChrome_btn;
    public bool isGold_btn;
    public bool isSilver_btn;

    public bool isForceA_btn;
    public bool isForceB_btn;
    public bool isForceC_btn;

    public bool isTimeA_btn;
    public bool isTimeB_btn;
    public bool isTimeC_btn;

    public bool isAimA_btn;
    public bool isAimB_btn;
    public bool isAimC_btn;

    public bool isSuper_btn;
    public bool isRegular_btn;
    public bool isPatch_btn;

    public bool isAttacking;
    public bool isMidFilder;
    public bool isDefence;
    public bool isBoxtoBox;


    public Sprite[] acttackingCapSprites;
    public Sprite[] protectingCapSprites;

    public List<GameObject> IngamePlayer_timerList = new List<GameObject>();
    public List<GameObject> SwapPlayer_timerList = new List<GameObject>();

    public int playerNumberForStaminaIncrease;

    public GameObject playerObjecctFind;
    public int playerObjectCouterFromArray;


    public static BumpStaminaManager instance;

    void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        MinX_Percentage = 5f;
        initialvalueTime = 20f;
        initialvalueForce = 3.5f;
        initialvalueAIM = 10f;
        BSForceDrink_A_Percentage = 0.25f;
        BSForceDrink_B_Percentage = 0.20f;
        BSForceDrink_C_Percentage = 0.10f;

        BSTimeDrink_A_Percentage = 0.25f;
        BSTimeDrink_B_Percentage = 0.20f;
        BSTimeDrink_C_Percentage = 0.10f;

        BSAIMDrink_A_Percentage = 0.25f;
        BSAIMDrink_B_Percentage = 0.20f;
        BSAIMDrink_C_Percentage = 0.10f;

        SuperAidBox_Percentage = 25f;
        RegularAidBox_Percentage = 20f;
        PatchAidBox_Percentage = 10f;

    }

    public float FillBSForceValueOfDrinkA(float forceA)
    {
        Debug.LogError("force ----" + forceA + "---" + BSForceDrink_A_Percentage);
        float incresePersantange = BSForceDrink_A_Percentage * forceA;
        return incresePersantange;
    }

    public float FillBSForceValueOfDrinkB(float forceB)
    {
        float incresePersantange = BSForceDrink_B_Percentage * forceB;
        return incresePersantange;
    }

    public float FillBSForceValueOfDrinkC(float forceC)
    {
        float incresePersantange = BSForceDrink_C_Percentage * forceC;
        return incresePersantange;
    }

    public float FillBSTimeValueOfDrinkA(float timeA)
    {
        float incresePersantange = BSTimeDrink_A_Percentage * timeA;
        return incresePersantange;

    }

    public float FillBSTimeValueOfDrinkB(float timeB)
    {
        float incresePersantange = BSTimeDrink_B_Percentage * timeB;
        return incresePersantange;
    }

    public float FillBSTimeValueOfDrinkC(float timeC)
    {
        float incresePersantange = BSTimeDrink_C_Percentage * timeC;
        return incresePersantange;
    }

    public float FillBSAIMValueOfDrinkA(float aimA)
    {
        float incresePersantange = BSAIMDrink_A_Percentage * aimA;
        return incresePersantange;
    }

    public float FillBSAIMValueOfDrinkB(float aimB)
    {
        float incresePersantange = BSAIMDrink_B_Percentage * aimB;
        return incresePersantange;
    }

    public float FillBSAIMValueOfDrinkC(float aimC)
    {
        float incresePersantange = BSAIMDrink_C_Percentage * aimC;
        return incresePersantange;
    }

    public void stamina(string cap)
    {
        Debug.LogError("CapName----" + cap);
        switch (cap) {
            case "elephantcap":
                isElephant_btn = true;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "rhinocap":
                isElephant_btn = false;
                isRhino_btn = true;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "linocap":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = true;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "chromecap":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = true;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "goldcap":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = true;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "silvercap":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = true;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "forceA":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = true;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "forceB":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = true;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "forceC":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = true;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "timeA":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = true;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "timeB":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = true;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;
            case "timeC":
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = true;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;

                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                break;

            case "attack":
                isAttacking = true;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                break;

            case "midfilder":
                isAttacking = false;
                isMidFilder = true;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                break;

            case "defence":

                isAttacking = false;
                isMidFilder = false;
                isDefence = true;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                break;

            case "boxtobox":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = true;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                break;

            case "aimA":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = true;
                isAimB_btn = false;
                isAimC_btn = false;
                break;

            case "aimB":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = true;
                isAimC_btn = false;
                break;

            case "aimC":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = true;
                break;

            case "SuperA":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                isSuper_btn = true;
                isRegular_btn = false;
                isPatch_btn = false;
                break;

            case "SuperB":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                isSuper_btn = false;
                isRegular_btn = true;
                isPatch_btn = false;
                break;

            case "SuperC":
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                isSuper_btn = false;
                isRegular_btn = false;
                isPatch_btn = true;
                break;

        }
    }


    public void ApplyStatsOnPlayer()
    {

        ApplyOnplayer(playerNumberForStaminaIncrease);
    }


    public void ApplyOnplayer(int player_no)
    {

       
            if (isElephant_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.GetComponent<Image>().sprite = acttackingCapSprites[0];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isElephant = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isRhino = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isLion = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isElephant_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                    Debug.Log("Cap Applied " + player_no + "isElephant_btn");
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isElephant_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                    Debug.Log("Cap Applied " + player_no + "isElephant_btn");
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isElephant_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                    Debug.Log("Cap Applied " + player_no + "isElephant_btn");
                }
                StatsManager.instance.ActiveProtacativeCap();
            }
            else if (isRhino_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.GetComponent<Image>().sprite = acttackingCapSprites[1];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isRhino = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isElephant = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isLion = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isRhino_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                    Debug.Log("Cap Applied on" + player_no + "isRhino_btn");
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isRhino_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isRhino_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                StatsManager.instance.ActiveProtacativeCap();

            }
            else if (isLion_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().attacking_Cap.GetComponent<Image>().sprite = acttackingCapSprites[2];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isLion = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isElephant = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isRhino = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isLion_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isLion_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isLion_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                StatsManager.instance.ActiveProtacativeCap();

            }
            else if (isChrome_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.GetComponent<Image>().sprite = protectingCapSprites[0];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isChromeCap = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isSilverCap = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isGoldCap = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isChrome_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isChrome_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isChrome_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                StatsManager.instance.ActiveAttackingCap();

            }
            else if (isGold_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.GetComponent<Image>().sprite = protectingCapSprites[1];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isGoldCap = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isSilverCap = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isChromeCap = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isGold_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isGold_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isGold_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                StatsManager.instance.ActiveAttackingCap();

            }
            else if (isSilver_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.SetActive(true);
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().proctective_Cap.GetComponent<Image>().sprite = protectingCapSprites[2];
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isSilverCap = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isChromeCap = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isGoldCap = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isSilver_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isSilver_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isSilver_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                StatsManager.instance.ActiveAttackingCap();
            }

            else if (isForceA_btn)
            {
                //if(playerController.Instance.max_aid<=)
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += FillBSForceValueOfDrinkA(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force);

                Debug.LogError("force applied----" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force);

                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;

                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isForceA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isForceA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isForceA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isForceB_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += FillBSForceValueOfDrinkB(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isForceB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isForceB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isForceB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isForceC_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += FillBSForceValueOfDrinkC(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isForceC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isForceC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isForceC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isTimeA_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += FillBSTimeValueOfDrinkA(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isTimeA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isTimeA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isTimeA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isTimeB_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += FillBSTimeValueOfDrinkB(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isTimeB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isTimeB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isTimeB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isTimeC_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += FillBSTimeValueOfDrinkC(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isTimeC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isTimeC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isTimeC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isAimA_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += FillBSAIMValueOfDrinkA(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isAimA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isAimA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isAimA_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isAimB_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += FillBSAIMValueOfDrinkB(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isAimB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isAimB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isAimB_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isAimC_btn)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += FillBSAIMValueOfDrinkC(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim);
                if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                {
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                }

                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                //			GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isAimC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isAimC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isAimC_btn";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
                WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer, 100,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_name,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_id,
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_status
                );
            }
            else if (isAttacking)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isAttackingPlayer = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;

                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isAttacking";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isAttacking";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isAttacking";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
            }
            else if (isMidFilder)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isMidFilder";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isMidFilder";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isMidFilder";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
            }
            else if (isDefence)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isDefensivePlayer = true;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isDefence";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isDefence";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isDefence";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
            }
            else if (isBoxtoBox)
            {
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = true;
                if (GlobalGameManager.MatchType == 2)
                {
                    string mystring = player_no + "," + "isBoxtoBox";
                    MainSocketConnection.instance.SendApplyCapNDrink(mystring);
                }
                else if (GlobalGameManager.MatchType == 5)
                {
                    string mystring = player_no + "," + "isBoxtoBox";
                    MainSocketConnection.instance.SendApplyCapNDrinkPWF(mystring);
                }
                else if (GlobalGameManager.MatchType == 3)
                {
                    string mystring = player_no + "," + "isBoxtoBox";
                    MainSocketConnection.instance.SendApplyCapNDrinkTour(mystring);
                }
            }
            else if (isSuper_btn)
            {
                Debug.LogError("max-aid_superA----" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid + "--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid / 3);


                if ((GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid) <= ((BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime) * BumpStaminaManager.instance.MinX_Percentage / 100))
                {
                    Debug.LogError("--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force + "--" + SuperAidBox_Percentage);

                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force * SuperAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer * SuperAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim * SuperAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                    }

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                    StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                    StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                    StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();


                }
            }
            else if (isRegular_btn)
            {
                Debug.LogError("max-aid_regularB----" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid + "--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid / 3);


                if ((GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid) <= ((BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime) * BumpStaminaManager.instance.MinX_Percentage / 100))
                {
                    Debug.LogError("--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force + "--" + RegularAidBox_Percentage);

                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force * RegularAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer * RegularAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim * RegularAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                    }


                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                    StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                    StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                    StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();


                }
            }
            else if (isPatch_btn)
            {
                Debug.LogError("max-aid_patchC----" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid + "--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid / 3);


                if ((GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().max_aid) <= ((BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime) * BumpStaminaManager.instance.MinX_Percentage / 100))
                {
                    Debug.LogError("--" + GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force + "--" + PatchAidBox_Percentage);

                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force * PatchAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force > initialvalueForce)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force = initialvalueForce;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer * PatchAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer > initialvalueTime)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer = initialvalueTime;
                    }
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim += (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim * PatchAidBox_Percentage) / 100f;
                    if (GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim > initialvalueAIM)
                    {
                        GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim = initialvalueAIM;
                    }

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().f1.text = Math.Round (100 / initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().Force / initialvalueForce;

                    StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().a1.text = Math.Round (100 / initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().mDisc_aim / initialvalueAIM;

                    StatsManager.instance.plaAimImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().t1.text = Math.Round (100 / initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer, 1).ToString ();
                    //				GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [player_no].GetComponent <playerController> ().timePlayer / initialvalueTime;

                    StatsManager.instance.plaTimeImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().timePlayer / BumpStaminaManager.instance.initialvalueTime;
                    GlobalGameManager.SharedInstance.allPlayer[player_no].GetComponent<playerController>().UpdateStatsValue();

                }
            }
            ResetAllValue();
            StatsManager.instance.AssignHeathSignColorValue();
        }
        

            public void ResetAllValue()
            {
                isAttacking = false;
                isMidFilder = false;
                isDefence = false;
                isBoxtoBox = false;
                isElephant_btn = false;
                isRhino_btn = false;
                isLion_btn = false;
                isChrome_btn = false;
                isGold_btn = false;
                isSilver_btn = false;
                isForceA_btn = false;
                isForceB_btn = false;
                isForceC_btn = false;
                isTimeA_btn = false;
                isTimeB_btn = false;
                isTimeC_btn = false;
                isAimA_btn = false;
                isAimB_btn = false;
                isAimC_btn = false;
                isSuper_btn = false;
                isRegular_btn = false;
                isPatch_btn = false;
            }



            #region SWAP INDEX

            /// <summary>
            /// swap the index(0 to 10)
            /// if index is 10 it is ball's index
            /// if index is less than 10 then its swap it, if index is greater than 4 then add 5 more otherwise substract 5.
            /// </summary>

            private int swapIndex(int index)
            {

                if (index == 10) { // 10 index number is for ball 
                    return index;
                }

                if (index > 4) {
                    index -= 5;
                } else {
                    index += 5;
                }

                return index;
            }

            #endregion

            public void AssignStaminaOnPlayer2(string myString)
            {
                string[] posData = myString.Split(new string[] { "," }, System.StringSplitOptions.RemoveEmptyEntries);
                ApplyOnPlayer2(swapIndex(int.Parse(posData[1])), posData[2]);
            }

            public void ApplyOnPlayer2(int player_no, string activedName)
            {
                if (activedName == "isElephant_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.GetComponent<Image>().sprite = protectingCapSprites[0];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isElephant = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isRhino = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isLion = false;
                } else if (activedName == "isRhino_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.GetComponent<Image>().sprite = protectingCapSprites[1];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isRhino = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isElephant = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isLion = false;
                } else if (activedName == "isLion_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().attacking_Cap.GetComponent<Image>().sprite = protectingCapSprites[2];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isLion = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isRhino = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isElephant = false;
                } else if (activedName == "isChrome_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.GetComponent<Image>().sprite = acttackingCapSprites[0];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isChromeCap = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isGoldCap = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isSilverCap = false;
                } else if (activedName == "isGold_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.GetComponent<Image>().sprite = acttackingCapSprites[1];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isGoldCap = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isChromeCap = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isSilverCap = false;
                } else if (activedName == "isSilver_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.SetActive(true);
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().proctective_Cap.GetComponent<Image>().sprite = acttackingCapSprites[2];
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isSilverCap = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isGoldCap = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isChromeCap = false;
                } else if (activedName == "isForceA_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force += FillBSForceValueOfDrinkA(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force > initialvalueForce) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force = initialvalueForce;
                    }
                } else if (activedName == "isForceB_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force += FillBSForceValueOfDrinkB(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force > initialvalueForce) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force = initialvalueForce;
                    }
                } else if (activedName == "isForceC_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force += FillBSForceValueOfDrinkC(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force > initialvalueForce) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().Force = initialvalueForce;
                    }
                } else if (activedName == "isTimeA_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 += FillBSTimeValueOfDrinkA(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 > initialvalueTime) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 = initialvalueTime;
                    }
                } else if (activedName == "isTimeB_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 += FillBSTimeValueOfDrinkB(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 > initialvalueTime) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 = initialvalueTime;
                    }
                } else if (activedName == "isTimeC_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 += FillBSTimeValueOfDrinkC(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 > initialvalueTime) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().timeplayer2 = initialvalueTime;
                    }
                } else if (activedName == "isAimA_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim += FillBSAIMValueOfDrinkA(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim > initialvalueAIM) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim = initialvalueAIM;
                    }
                } else if (activedName == "isAimB_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim += FillBSAIMValueOfDrinkB(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim > initialvalueAIM) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim = initialvalueAIM;
                    }
                } else if (activedName == "isAimC_btn") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim += FillBSAIMValueOfDrinkC(OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim);
                    if (OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim > initialvalueAIM) {
                        OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<Player2Controller>().mDisc_aim = initialvalueAIM;
                    }
                } else if (activedName == "isAttacking") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isAttackingPlayer = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;

                } else if (activedName == "isMidFilder") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;
                } else if (activedName == "isDefence") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isDefensivePlayer = true;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = false;
                } else if (activedName == "isBoxtoBox") {
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isAttackingPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isMidfieldPlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isDefensivePlayer = false;
                    OnlineGamePlayHandller.instance.playerHandllers[player_no].GetComponent<PlayerStats>().isBoxToBoxPlayer = true;
                }
            }

            public void GS_stats()
            {
                for (int i = 0; i < 5; i++) {
                    float time = (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().obj.GetComponent<ActivePlayerStatminaValueAssign>().player_timer_act / GlobalGameManager.SharedInstance.gameplay_timer) * 100;
                    Debug.LogError("time-of-act" + i + "" + time);
                    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<PlayerStats>().Gs_calculation(time);
                }


                for (int i = 0; i < SwapPlayer_timerList.Count; i++) {
                    float time = (BumpStaminaManager.instance.SwapPlayer_timerList[i].GetComponent<InActivePlayerStatminaValueAssign>().player_timer_Inact / GlobalGameManager.SharedInstance.gameplay_timer) * 100;
                    Debug.LogError("time-of-inact" + i + "" + time);
                    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<PlayerStats>().Gs_calculation(time);
                }
            }

            public void Reset_theropy()
            {

                for (int i = 0; i < 5; i++)
                {
                    float time = (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().obj.GetComponent<ActivePlayerStatminaValueAssign>().player_timer_act / GlobalGameManager.SharedInstance.gameplay_timer) * 100;
                    Debug.LogError("time-of-act" + i + "" + time);
                    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<PlayerStats>().ResetTheropyvalue();
                }


                for (int i = 0; i < SwapPlayer_timerList.Count; i++)
                {
                    float time = (BumpStaminaManager.instance.SwapPlayer_timerList[i].GetComponent<InActivePlayerStatminaValueAssign>().player_timer_Inact / GlobalGameManager.SharedInstance.gameplay_timer) * 100;
                    Debug.LogError("time-of-inact" + i + "" + time);
                    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<PlayerStats>().ResetTheropyvalue();
                }


            }
            */
}
    




