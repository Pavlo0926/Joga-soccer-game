using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FATchange : MonoBehaviour {
    /*

    public float disc_id;
    public Text F_set;
    public Text A_set;
    public Text T_set;

    public float Forceset = 3.5f;
    public float Timeset = 20f;
    public float Aimset = 10f;

    public static FATchange instance;


    // Use this for initialization
    void Start() {

        //Forceset = float.Parse(F_set.text);
        //Timeset = float.Parse(T_set.text);
        //Aimset = float.Parse(A_set.text);

        // allPlayer [i].GetComponent <playerController> ().timePlayer = WebServicesHandler.SharedInstance.ActivePlayerList [i].mTime;
        //   allPlayer [i].GetComponent <playerController> ().Force = WebServicesHandler.SharedInstance.ActivePlayerList [i].mForceData;

        // allPlayer [i].GetComponent <playerController> ().mDisc_aim = WebServicesHandler.SharedInstance.ActivePlayerList [i].mAim;
    }



    // Update is called once per frame
    void Update() 
    {
        F_set.text = BumpStaminaManager.instance.initialvalueForce.ToString();
        T_set.text = BumpStaminaManager.instance.initialvalueTime.ToString();
        A_set.text = BumpStaminaManager.instance.initialvalueAIM.ToString();


    }

    public void BtnPress(string name)
    {

        switch (name)
        {
            case "force":
               BumpStaminaManager.instance.initialvalueForce += 0.1000f;
               
                 if (BumpStaminaManager.instance.initialvalueForce > 10)
                {
                    BumpStaminaManager.instance.initialvalueForce = 10;
                }

                break;

            case "aim":
                BumpStaminaManager.instance.initialvalueAIM += 0.1000f;
               
                 if (BumpStaminaManager.instance.initialvalueAIM > 20)
                {
                    BumpStaminaManager.instance.initialvalueAIM = 20;
                }

                break;

            case "time":
                BumpStaminaManager.instance.initialvalueTime += 0.1000f;
               
                 if (BumpStaminaManager.instance.initialvalueTime > 50)
                {
                    BumpStaminaManager.instance.initialvalueTime = 50;
                }

                break;

            case "mforce":
                BumpStaminaManager.instance.initialvalueForce -= 0.1000f;
               
                 if (BumpStaminaManager.instance.initialvalueForce < 1)
                {
                    BumpStaminaManager.instance.initialvalueForce = 1;
                }

                break;

            case "maim":
               BumpStaminaManager.instance.initialvalueAIM -= 0.1000f;
               
                if (BumpStaminaManager.instance.initialvalueAIM < 1)
                {
                    BumpStaminaManager.instance.initialvalueForce = 1;
                }
                break;

            case "mtime":
                BumpStaminaManager.instance.initialvalueTime -= 0.1000f;
               
                if (BumpStaminaManager.instance.initialvalueTime < 1)
                {
                    BumpStaminaManager.instance.initialvalueTime = 1;
                }
                break;

        }

        //public void MinsPress(string nn)
        //{
            //switch (nn)
            //{
            //    case "mforce":
            //        Forceset -= 0.5f;
            //        break;

            //    case "maim":
            //        Aimset -= 0.5f;
            //        break;
            //    case "mtime":
            //        Timeset -= 0.5f;
            //        break; 
            //}


        }



        public void MaxValue()
   
     {
       
        for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++)
        {

            GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force = BumpStaminaManager.instance.initialvalueForce;
          
            //  if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force > 10)
            //{
            //    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force = 10;
            //}


            GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim = BumpStaminaManager.instance.initialvalueAIM;

            //if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim > 20)
            //{
            //    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim = 20;
            //}


            GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer = BumpStaminaManager.instance.initialvalueTime;

            //if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer > 50)
            //{
            //    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer = 50;
            //}
            //WebServicesHandler.SharedInstance.ActivePlayerList[i].mForceData = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force;
            //WebServicesHandler.SharedInstance.ActivePlayerList[i].mAim = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim;
            //WebServicesHandler.SharedInstance.ActivePlayerList[i].mTime = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer;

            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_id = disc_id;

            //WebServicesHandler.SharedInstance.updateDisk(GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force,
            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer, 100,
            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim,
            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_name,
            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_id,
            //GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_status);
             //MainSocketConnection.instance.SendApplyStaminaValue(stminaStr);

            //StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force / BumpStaminaManager.instance.initialvalueForce;
            //StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim/ BumpStaminaManager.instance.initialvalueAIM;
            //StatsManager.instance.plaForceImage.fillAmount = GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer/ BumpStaminaManager.instance.initialvalueTime;

           // GlobalGameManager.SharedInstance.allPlayer [i].GetComponent<playerController>().UpdateStatsValue();
        }  
       
  }
    //allPlayer[i].GetComponent<playerController>().timePlayer = WebServicesHandler.SharedInstance.ActivePlayerList[i].mTime;
    //allPlayer[i].GetComponent<playerController>().Force = WebServicesHandler.SharedInstance.ActivePlayerList[i].mForceData;
    //allPlayer[i].GetComponent<playerController>().mDisc_name = WebServicesHandler.SharedInstance.ActivePlayerList[i].mDiskName;
    //allPlayer[i].GetComponent<playerController>().mDisc_aim = WebServicesHandler.SharedInstance.ActivePlayerList[i].mAim;

    //if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force > BumpStaminaManager.instance.initialvalueForce)
    //{
    //            GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().Force = BumpStaminaManager.instance.initialvalueForce;
    //}

    //if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim > BumpStaminaManager.instance.initialvalueAIM)
    //{
    //    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().mDisc_aim= BumpStaminaManager.instance.initialvalueAIM;
    //}
    //if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer > BumpStaminaManager.instance.initialvalueTime)
    //{
    //    GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().timePlayer = BumpStaminaManager.instance.initialvalueTime;
    //}
    */

    // for disable player stats eneble it when player stats and bumpstat enable
}



