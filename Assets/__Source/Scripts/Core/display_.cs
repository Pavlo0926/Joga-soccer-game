using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class display_ : MonoBehaviour
{
      /*
      public Text time_no;
      public Text Sub_panel_time;

      public Sprite[] Attacking_Cap;
      public Sprite[] Protecting_Cap;

      private float force1;
      private float aim1;
      private float time1;
      // public float Initial_average;

      public float injury_value;
      public float Remain_percent;
      public float current_Stat_add;
      public float Initial_Add;

      public Text injury_remain;
      public Text sub_panel_injury_remain;

      public Text player_nm;
  private BumpStaminaManager m_dataBumpstat=null;
  private BumpStaminaManager dataBumpstat {get{if(!m_dataBumpstat)m_dataBumpstat =BumpStaminaManager.instance;return m_dataBumpstat;}}
      public static display_ instance;

      void Awake()
      {
          instance = this;
      }
      // Use this for initialization
      void Start () {

        }

        // Update is called once per frame
        void Update () {
          Average();

      }
   public void Average()
      {
          player_nm.text = dataBumpstat.playerObjecctFind.GetComponent<playerController>().name;

          time_no.text = Math.Round(GlobalGameManager.SharedInstance.baseShootTimePlayer1,1).ToString();
          Sub_panel_time.text = Math.Round(GlobalGameManager.SharedInstance.baseShootTimePlayer1,1).ToString();


          force1 = dataBumpstat.playerObjecctFind.GetComponent<playerController>().Force  ;
          aim1 = dataBumpstat.playerObjecctFind.GetComponent<playerController>().mDisc_aim ;
          time1 = dataBumpstat.playerObjecctFind.GetComponent<playerController>().timePlayer ;

          current_Stat_add = force1 + aim1 + time1;

          Initial_Add = dataBumpstat.initialvalueForce + dataBumpstat.initialvalueAIM + dataBumpstat.initialvalueTime;



           StatsManager.instance.statsValueText.text = Math.Round((current_Stat_add/Initial_Add) *100,1) .ToString();


          injury_value = Initial_Add * (dataBumpstat.MinX_Percentage /100);


          Remain_percent = (current_Stat_add - injury_value) ;

          injury_remain.text = Math.Round(Remain_percent/Initial_Add * 100,1).ToString();

          sub_panel_injury_remain.text = Math.Round(Remain_percent/Initial_Add * 100,1).ToString();
      }
      */

}
