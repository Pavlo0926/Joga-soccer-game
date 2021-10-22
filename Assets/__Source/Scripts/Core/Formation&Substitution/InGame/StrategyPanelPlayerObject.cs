using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StrategyPanelPlayerObject : MonoBehaviour
{
      int playerCount;
      int playerOn;

      public int ObjectActive;
      public Image ImageTime;
      public float ImgeTime;
      public Text TimeText;
      public float time = 5;

      public int time1 = 5;
      public bool timest;

      // private BumpStaminaManager m_dataBumpstat=null;
      // private BumpStaminaManager dataBumpstat {get{if(!m_dataBumpstat)m_dataBumpstat =BumpStaminaManager.instance ;return m_dataBumpstat;}}
      public static StrategyPanelPlayerObject instance;

      //public GameObject strategypanel;
      void Awake()
      {
            if(!instance)
            instance = this;

      }

      void OnEnable()
      {
            timest = true;
            time = 5f;
            playerCount = 0;
            ObjectActive = 0;
            /*
            dataBumpstat.playerNumberForStaminaIncrease = playerCount;
            dataBumpstat.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer[playerCount];
            dataBumpstat.playerObjectCouterFromArray = playerCount;
            // for disable player stats,bumpstamina eneble it when player stats enable
            */
            //	FormationStratgy.instance.AssignDefeceFormation();
            // FormationStratgy.instance.Active_Btn_sprite[0].SetActive(true);	//def
            // FormationStratgy.instance.Active_Btn_sprite[1].SetActive(false);  //attack			
            // FormationStratgy.instance.Active_Btn_sprite[2].SetActive(false);	//bal

      }
      // Use this for initialization
      void Start()

      {
            // Debug.Log("timest time0" + time);
            //		AdultLink.HealBar.instance.Onstart();
            time = 5f;
            timest = true;
            // Debug.Log("timest time0" + time);

      }


      void Update()
      {
            if (timest)
            {
                   TimeText.text = "Time : " + "00" + ":0" + time1.ToString();
                  if (time >= 0)
                  {
                        TimeText.text = "Time : " + "00" + ":0" + time1.ToString();
                        time -= Time.fixedDeltaTime;
                        time1 = (int)time;

                        ImgeTime += Time.fixedDeltaTime / 4f;
                        ImageTime.fillAmount = ImgeTime;
                  }

                  //  time = 5f;         
            }

      }

      public void ChangePlayer()
      {
            playerCount++;

            if (playerCount >= 5)
            {
                  playerCount = 0;
            }
            /*
            dataBumpstat.playerNumberForStaminaIncrease = playerCount;
            dataBumpstat.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer[playerCount];
            dataBumpstat.playerObjectCouterFromArray = playerCount;

            if (ObjectActive == 0)
            {
                StatsManager.instance.AssignMainObjectValue();
            }
            else if (ObjectActive == 1)
            {
                SubStitutionManager.instance.FirstAssignData();
            }
            */
            // for disable player stats,bump stamina eneble it when player stats enable
      }



      public void ActiveObject(int noOfActiveObj)
      {
            ObjectActive = noOfActiveObj;
      }
      public void CBack()
      {

            playerOn--;

            if (playerOn <= 0)
            {
                  playerOn = 5;
            }
            /*
            dataBumpstat.playerNumberForStaminaIncrease = playerOn;
            dataBumpstat.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer[playerOn];
            dataBumpstat.playerObjectCouterFromArray = playerOn;


            if (ObjectActive == 0)
            {
                StatsManager.instance.AssignMainObjectValue();
            }
            else if (ObjectActive == 1)
            {
                SubStitutionManager.instance.FirstAssignData();
            }
            */
            // for disable player stats,bumpstamina eneble it when player stats enable
      }


}