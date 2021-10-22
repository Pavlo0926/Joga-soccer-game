// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.UI;
// using UnityEngine;
// using System;


// public class TestingSimulationStats : MonoBehaviour
// {
    
   

//     /// <summary>
//     ///  To change Disc physics value. For testing purpose.
//     /// </summary>
//     public GameObject adjust_btn;
//     public float originalStaticFriction1;
//     public float originaldynamicFriction1;
//     public float originalbounce1;
//     public float originalDrag1;
//     public float originalMass1;

//     /// <summary>
//     /// To change Ball Physics value. For testing purpose.
//     /// </summary>

//     public float BallStaticFriction;
//     public float BallDynamicFriction;
//     public float BallBounciness;

//     public List<GameObject> ActiveDiscs;
//     public List<GameObject> InActiveDiscs;

//     /// <summary>
//     /// To display Stats value of player.
//     /// </summary>

//     public Text Ftext_1;
//     public Text Atext_1;
//     public Text Ttext_1;

//     public Text Ftext_2;
//     public Text Atext_2;
//     public Text Ttext_2;

//     public Text Ftext_3;
//     public Text Atext_3;
//     public Text Ttext_3;

//     public Text Ftext_4;
//     public Text Atext_4;
//     public Text Ttext_4;

//     public Text Ftext_5;
//     public Text Atext_5;
//     public Text Ttext_5;

//     /// <summary>
//     /// To display stats value of opponent.
//     /// </summary>
//     public Text O_Ftext_1;
//     public Text O_Atext_1;
//     public Text O_Ttext_1;

//     public Text O_Ftext_2;
//     public Text O_Atext_2;
//     public Text O_Ttext_2;

//     public Text O_Ftext_3;
//     public Text O_Atext_3;
//     public Text O_Ttext_3;

//     public Text O_Ftext_4;
//     public Text O_Atext_4;
//     public Text O_Ttext_4;

//     public Text O_Ftext_5;
//     public Text O_Atext_5;
//     public Text O_Ttext_5;

//     /// <summary>
//     /// To display physics value for player and Ball
//     /// </summary>


//     public Text PlayerDynamic;
//     public Text PlayerStatic;
//     public Text PlayerBounce;
//     public Text PlayerDrag;
//     public Text PlayerMass;

//     public Text BallDynamic;
//     public Text BallStatic;
//     public Text BallBounce;


//     //  public Button add;
//     //  public Button diduce;
//     public Text bs;
//     public float BS; //= PlayerStats.SharedInstance.maxPercentage_bs;

//     // public Button GSadd;
//     //  public Button GSdiduce;
//     public Text gs;
//     public float GS; //= PlayerStats.SharedInstance.maxPercentage_gs;

//     public GameObject distancePanel;
//     public GameObject Physicspanel;
//     public GameObject adjustpanel;

//     // public Text dragValue;
//     //public Text Drag_Value;
//     public Text Drag_Value1;


//     public static TestingSimulationStats instance;


//     private void Awake()
//     {
//         instance = this;
//         // originalStaticFriction1 = playerController.Instance.originalStaticFriction;
//         // originaldynamicFriction1 = playerController.Instance.originaldynamicFriction;
//         // originalbounce1 = playerController.Instance.originalbounce;
//         // originalDrag1 = playerController.Instance.originalDrag;
//         // originalMass1 = playerController.Instance.originalMass;

//         // BallDynamicFriction = BallManager.instace.originalDynamicFriction;
//         // BallStaticFriction = BallManager.instace.originalStaticFriction;
//         // BallBounciness = BallManager.instace.originalBounciness;
//         adjust_btn = GameObject.FindGameObjectWithTag("adjust");
//     }

//     void Start()
//     { 
//         Physicspanel.gameObject.SetActive(false);
//         distancePanel.gameObject.SetActive(false);
//         adjustpanel.gameObject.SetActive(false);
//         distancePanel.gameObject.SetActive(false);
//         adjust_btn.SetActive(true);
//     }

//     public void Active_physics_panel()
//     {
//         Physicspanel.gameObject.SetActive(true);
//         distancePanel.gameObject.SetActive(false);
//         adjustpanel.gameObject.SetActive(false);
//         distancePanel.gameObject.SetActive(false);

//     }

//     public void DeActive_physics_panel()
//     {
//         Physicspanel.gameObject.SetActive(false);

//     }

//     public void Active_adjust_panel()
//     {
//         adjustpanel.gameObject.SetActive(true);
//     }

//     public void DeActive_adjust_panel()
//     {
//         adjustpanel.gameObject.SetActive(false);
//     }

//     public void Active_Disatance_panel()
//     {
//         distancePanel.gameObject.SetActive(true);

//     }

//     public void DeAct_distance_Panel()
//     {
//         distancePanel.gameObject.SetActive(false);

//     }


//     // Update is called once per frame

//     public void OnAddBS()

//     {
//         BS = BS + 1;
//         PlayerStats.SharedInstance.maxPercentage_bs = BS;
//         bs.text = PlayerStats.SharedInstance.maxPercentage_bs.ToString();
//     }

//     public void OnMinusBS()
//     {
//         if (BS > 0)
//         {
//             BS = BS - 1;
//             PlayerStats.SharedInstance.maxPercentage_bs = BS;
//             bs.text = PlayerStats.SharedInstance.maxPercentage_gs.ToString();
//         }
//     }


//     public void OnGSAdd()

//     {
//         GS = GS + 1;
//         PlayerStats.SharedInstance.maxPercentage_gs = GS;
//         gs.text = GS.ToString();
//     }

//     public void OnGSMinus()
//     {
//         if (GS > 0)
//         {
//             GS = GS - 1;
//             PlayerStats.SharedInstance.maxPercentage_gs = GS;
//             gs.text = GS.ToString();
//         }
//     }
//     public void OnStatic_In()
//     {

//         originalStaticFriction1 = originalStaticFriction1 + 0.1f;
//         Debug.Log("Static increased" + originalStaticFriction1);

//     }

//     public void OnStatic_Dec()
//     {
//         if (originalStaticFriction1 > 0)
//         {
//             originalStaticFriction1 = originalStaticFriction1 - 0.1f;
//             Debug.Log("Static decreased" + originalStaticFriction1);
//         }
//     }

//     public void OnDynamic_In()
//     {

//         originaldynamicFriction1 = originaldynamicFriction1 + 0.1f;
//         Debug.Log("Dynamic increased" + originaldynamicFriction1);
//     }

//     public void OnDynamic_Dec()
//     {
//         if (originaldynamicFriction1 > 0)
//         {
//             originaldynamicFriction1 = originaldynamicFriction1 - 0.1f;
//             Debug.Log("Dynamic decreased" + originaldynamicFriction1);
//         }
//     }

//     public void OnBouncy_In()
//     {
//         if (originalbounce1 < 0.9f)
//         {
//             originalbounce1 = originalbounce1 + 0.1f;
//         }
//         if ((BallBounciness + 0.1) < 1)
//         {
//             BallBounciness = 1;
//         }
//         Debug.Log("Bounce increased" + originalbounce1);

//     }

//     public void OnBouncy_Dec()
//     {
//         if (originaldynamicFriction1 > 0)
//         {
//             originalbounce1 = originalbounce1 - 0.1f;
//             Debug.Log("Bounce decreased" + originalbounce1);
//         }
//     }

//     public void OnMass_In()
//     {
//         originalMass1 = originalMass1 + 0.1f;

//     }
//     public void OnMass_Dec()
//     {
//         originalMass1 = originalMass1 - 0.1f;
//     }

//     public void OnDraG_In()
//     {
//         originalDrag1 = originalDrag1 + 0.1f;
//     }
//     public void OnDraG_Dec()
//     {
//         originalDrag1 = originalDrag1 - 0.1f;

//     }


//     public void BallStatic_In()
//     {

//         BallStaticFriction = BallStaticFriction + 0.1f;
//         Debug.Log("Static increased" + BallStaticFriction);

//     }

//     public void BallStatic_Dec()
//     {
//         if (BallStaticFriction > 0)
//         {
//             BallStaticFriction = BallStaticFriction - 0.1f;
//             Debug.Log("Static decreased" + BallStaticFriction);
//         }
//     }

//     public void BallDynamic_In()
//     {

//         BallDynamicFriction = BallDynamicFriction + 0.1f;
//         Debug.Log("Dynamic increased" + BallDynamicFriction);
//     }

//     public void BallDynamic_Dec()
//     {
//         if (BallDynamicFriction > 0)
//         {
//             BallDynamicFriction = BallDynamicFriction - 0.1f;
//             Debug.Log("Dynamic decreased" + BallDynamicFriction);
//         }
//     }

//     public void BallBouncy_In()
//     {
//         if (BallBounciness < 1.0f)
//         {
//             BallBounciness = BallBounciness + 0.1f;
//         }

//         if ((BallBounciness + 0.1) < 1)
//         {
//             BallBounciness = 1;
//         }

//         Debug.Log("Bounce increased" + BallBounciness);

//     }

//     public void BallBouncy_Dec()
//     {
//         if (BallBounciness > 0)
//         {
//             BallBounciness = BallBounciness - 0.1f;
//             Debug.Log("Bounce decreased" + BallBounciness);
//         }
//     }



//     private void Update()

//     {

//         PlayerDynamic.text = originaldynamicFriction1.ToString();
//         PlayerStatic.text = originalStaticFriction1.ToString();
//         PlayerBounce.text = originalbounce1.ToString();
//         PlayerDrag.text = originalDrag1.ToString();
//         PlayerMass.text = originalMass1.ToString();

//         BallStatic.text = BallStaticFriction.ToString();
//         BallDynamic.text = BallDynamicFriction.ToString();
//         BallBounce.text = BallBounciness.ToString();     

//     }

    



//     public void Fill_FAT_value(string name)
//     {
//         for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++)

//             switch (name)
//             {
//                 case "forceless":

//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force -= BumpStaminaManager.instance.initialvalueForce / 100;

//                     if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force < BumpStaminaManager.instance.initialvalueForce/100)
//                     {
//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force = BumpStaminaManager.instance.initialvalueForce/100;
//                     }
//                     break;


//                 case "aimless":

//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim -= BumpStaminaManager.instance.initialvalueAIM / 100;
//                     if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim < BumpStaminaManager.instance.initialvalueAIM/100)
//                     {

//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim = BumpStaminaManager.instance.initialvalueAIM/100;
//                     }

//                     break;

//                 case "timeless":

//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer -= BumpStaminaManager.instance.initialvalueTime / 100;

//                     if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer < BumpStaminaManager.instance.initialvalueTime/100)
//                     {

//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer = BumpStaminaManager.instance.initialvalueTime/100;

//                     }

//                     break;

//                 case "fadd":
//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force += BumpStaminaManager.instance.initialvalueForce / 100;
//                     if(GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force > BumpStaminaManager.instance.initialvalueForce)
//                     {
//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force = BumpStaminaManager.instance.initialvalueForce; 
//                     }
//                     break;
//                 case "aadd":
//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim += BumpStaminaManager.instance.initialvalueAIM / 100;
//                     if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim > BumpStaminaManager.instance.initialvalueAIM)
//                     {
//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim = BumpStaminaManager.instance.initialvalueAIM;
//                     }
//                     break;
//                 case "tadd":
//                     GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer += BumpStaminaManager.instance.initialvalueTime/ 100;
//                     if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer > BumpStaminaManager.instance.initialvalueTime)
//                     {
//                         GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer = BumpStaminaManager.instance.initialvalueTime;
//                     }
//                     break;
//             }

//     }
    
// }








      
    