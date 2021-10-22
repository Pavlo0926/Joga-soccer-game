using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class StatsManager : MonoBehaviour
{
    /*
    //float AllTime;
    public float persantangeValue;



    //	public Image[] playerUIImage;
    //	public Text[] playerForceText;
    //	public Text[] playerTimeText;
    //	public Text[] playerAimText;
    //	public Image[] playerForceImage;
    //	public Image[] playerTimeImage;
    //	public Image[] playerAimImage;

    public Image plaForceImage;
	public Image plaTimeImage;
	public Image plaAimImage;

    // public Text Team_time;
    public Text Atthits;
    public Text Defhits;
   

	public Text statsValueText;
    public Text ForceValueText;
    public Text AimValueText;
    public Text TimeValueText;

	public Image helathImage;

	public Image protectiveCapImage;
	public Image attackingCapImage;

	public GameObject inner_tabs;

	public GameObject stats_revival_btn;
	public GameObject stats_firstaid_btn;
	public GameObject stats_protective_btn;
	public GameObject stats_attacking_btn;

    public GameObject first_aid_disble;
   

    public Button Energize_button;
    public Button statsrevival_btn;
    public Button force_btn;
    public Button Aim_btn;
    public Button time_btn;

   // public GameObject substi_btn;

    public GameObject stats_revival_panel;
	public GameObject stats_firstaid_panel;
	public GameObject stats_protective_panel;
	public GameObject stats_attacking_panel;

	public GameObject stats_force_panel;
	public GameObject stats_aim_panel;
	public GameObject stats_time_panel;

    public GameObject substitution_Panel;

   
	public static StatsManager instance;

	void Awake ()
	{
		instance = this;
      
    }

    private void Start()
    {
       //stats value text is used in display script.

      //  statsValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueForce * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().Force, 1).ToString();
        ForceValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueForce * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().Force, 1).ToString();
        AimValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueAIM * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().mDisc_aim, 1).ToString();
        TimeValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueTime * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().timePlayer, 1).ToString();
              
    }
    // Use this for initialization
    void OnEnable ()
	{
        if (force_btn != null || Energize_button != null || statsrevival_btn != null)
        {
            Energize_button.Select();
            statsrevival_btn.Select();
            force_btn.Select();
          
        }
		for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++) {
//			
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().a1.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueAIM * GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().mDisc_aim, 1).ToString ();
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().percentage_aim.fillAmount = GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().t1.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueTime * GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().timePlayer, 1).ToString ();
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().percentage_time.fillAmount = GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().timePlayer / BumpStaminaManager.instance.initialvalueTime;
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().f1.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueForce * GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().Force, 1).ToString ();
//			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().percentage_force.fillAmount = GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().Force / BumpStaminaManager.instance.initialvalueForce;

			GlobalGameManager.SharedInstance.allPlayer [i].GetComponent <playerController> ().UpdateStatsValue ();
		}
		AssignMainObjectValue ();
        Stats_revivel();
       // if (BumpStaminaManager.instance.)
        //Atthits.text = (3-PlayerStats.SharedInstance.attackingcap_Count).ToString();
      
        //Defhits.text = (3- PlayerStats.SharedInstance.protectioncap_Count).ToString();
    

    }


    public void Update()
    {
        ActiveProtacativeCap();
        ActiveAttackingCap();
    }

    public void AssignMainObjectValue ()
	{
		AssignHeathSignColorValue ();
        //ActiveProtacativeCap();
        //ActiveAttackingCap();

    

        // statsValueText.text = Math.Round (persantangeValue , 1).ToString ();
        ForceValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueForce * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().Force, 1).ToString();
        AimValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueAIM * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().mDisc_aim, 1).ToString();
        TimeValueText.text = Math.Round(100 / BumpStaminaManager.instance.initialvalueTime * BumpStaminaManager.instance.playerObjecctFind.GetComponent<playerController>().timePlayer, 1).ToString();

        plaForceImage.fillAmount = BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().Force / BumpStaminaManager.instance.initialvalueForce;
		plaTimeImage.fillAmount = BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().timePlayer / BumpStaminaManager.instance.initialvalueTime;
		plaAimImage.fillAmount = BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().mDisc_aim / BumpStaminaManager.instance.initialvalueAIM;


    }

	public void ForceStats_values ()
	{
	    ForceValueText.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueForce * BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().Force, 1).ToString ();
	}

	public void AimStats_values ()
	{
		AimValueText.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueAIM * BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().mDisc_aim, 1).ToString ();
	}

	public void TimeStats_values ()
	{
		TimeValueText.text = Math.Round (100 / BumpStaminaManager.instance.initialvalueTime * BumpStaminaManager.instance.playerObjecctFind.GetComponent <playerController> ().timePlayer, 1).ToString ();
	}


    public void AssignHeathSignColorValue()
    {
        //statsValueText.text = Math.Round (max_aid, 1).ToString ();

//        float max_aid = playerController.Instance.injury_value; //Injury stet value

        // max_aid = injury value from player controller.

        //  persantangeValue = ((playerController.Instance.max_aid - max_aid )/ (BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime) )* 100;

        // if (persantangeValue >= 85)
        // {
        //     helathImage.color = new Color(0f, 0.7450f, 0f, 1f);
        // }
        // else if (persantangeValue < 85 && persantangeValue >= 65)   
        // {
        //     helathImage.color = new Color(0f, 1f, 1f, 1f);            //new Color(0f, 255f, 255f, 0.9f); // new Color (0f, 1f, 1f, 1f);
        // }
        // else if (persantangeValue < 65 && persantangeValue >= 45)
        // {
        //     helathImage.color = new Color(1f, 1f, 0.02f, 1f);         //new Color(255f, 255f, 0f, 1f);// new Color (1f, 1f, 0.02f, 1f);

        // }
        // else if (persantangeValue < 45 && persantangeValue >= 25)
        // {
        //     helathImage.color = new Color(1f, 0.25f, 0.019f, 1f);     //new Color(255f, 0f, 0f, 0.5f);// new Color (1f, 0.55f, 0.019f, 1f);

        // }
        // else if (persantangeValue < 25 && persantangeValue >= 5)
        // {
        //     helathImage.color = new Color(1f, 0.55f, 0.019f, 1f);      // new Color(255f, 0f, 0f, 1f);// new Color (1f, 0f, 0.018f, 1f);

        // }
        // else if (persantangeValue < 5)
        // {
        //     helathImage.color = new Color(1f, 0f, 0.018f, 1f);

        // }
      
    }

	public void ActiveProtacativeCap ()
	{
		if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isChromeCap) {
			protectiveCapImage.gameObject.SetActive (true);
            protectiveCapImage.sprite = display_.instance.Protecting_Cap[0];


		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isGoldCap) {
			protectiveCapImage.gameObject.SetActive (true);
            protectiveCapImage.sprite = display_.instance.Protecting_Cap[1];

		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isSilverCap) {
			protectiveCapImage.gameObject.SetActive (true);
            protectiveCapImage.sprite = display_.instance.Protecting_Cap[2];

		} else 
        {
			protectiveCapImage.gameObject.SetActive (false);

		}
	}

	public void ActiveAttackingCap ()
	{
		if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isElephant) {
			attackingCapImage.gameObject.SetActive (true);
            attackingCapImage.sprite = display_.instance.Attacking_Cap[0];

		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isRhino) {
			attackingCapImage.gameObject.SetActive (true);
            attackingCapImage.sprite = display_.instance.Attacking_Cap[1];

        } else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isLion) {
			attackingCapImage.gameObject.SetActive (true);
            attackingCapImage.sprite = display_.instance.Attacking_Cap[2];

        }
        else {
			attackingCapImage.gameObject.SetActive (false);

        }
	}


	public void Stats_revivel ()
	{
       
		// stats_revival_panel.SetActive (true);
		// stats_firstaid_panel.SetActive (false);
		// stats_protective_panel.SetActive (false);
		// stats_attacking_panel.SetActive (false);
		// stats_force_panel.SetActive (true);
		// stats_aim_panel.SetActive (false);
		// stats_time_panel.SetActive (false);
		// inner_tabs.SetActive (true);
        // first_aid_disble.SetActive(false);
        // if (playerController.Instance.max_aid > playerController.Instance.injury_value)
        // {
        //     first_aid_disble.SetActive(false);
        // }
        // else
        // {
        //     first_aid_disble.SetActive(true);
        // }
     
    }

    public void Stats_firstaid()
    {
        // stats_revival_panel.SetActive(false);
        // stats_firstaid_panel.SetActive(true);
        // stats_protective_panel.SetActive(false);
        // stats_attacking_panel.SetActive(false);
        // inner_tabs.SetActive(false);
        // if (playerController.Instance.max_aid > playerController.Instance.injury_value)
        // {
        //     first_aid_disble.SetActive(true);
        // }
        // else
        // {
        //     first_aid_disble.SetActive(false);
        // }
    }

	public void Stats_protectivcap ()
	{
		stats_revival_panel.SetActive (false);
		stats_firstaid_panel.SetActive (false);
		stats_protective_panel.SetActive (true);
		stats_attacking_panel.SetActive (false);
		inner_tabs.SetActive (false);
        first_aid_disble.SetActive(false);
	}

	public void Stats_attackingcap ()
	{
		stats_revival_panel.SetActive (false);
		stats_firstaid_panel.SetActive (false);
		stats_protective_panel.SetActive (false);
		stats_attacking_panel.SetActive (true);
		inner_tabs.SetActive (false);
        first_aid_disble.SetActive(false);
	}

	public void Force_tab ()
	{
		stats_force_panel.SetActive (true);
		stats_aim_panel.SetActive (false);
		stats_time_panel.SetActive (false);
        // if (playerController.Instance.max_aid > playerController.Instance.injury_value)
        // {
        //     first_aid_disble.SetActive(false);
        // }
        // else
        // {
        //     first_aid_disble.SetActive(true);
        // }


    }

	public void Aim_tab ()
	{
		// stats_force_panel.SetActive (false);
		// stats_aim_panel.SetActive (true);
		// stats_time_panel.SetActive (false);
        // if (playerController.Instance.max_aid > playerController.Instance.injury_value)
        // {
        //     first_aid_disble.SetActive(false);
        // }
        // else
        // {
        //     first_aid_disble.SetActive(true);
        // }
    }

	public void Time_tab ()
	{
		// stats_force_panel.SetActive (false);
		// stats_aim_panel.SetActive (false);
		// stats_time_panel.SetActive (true);
        // if (playerController.Instance.max_aid > playerController.Instance.injury_value)
        // {
        //     first_aid_disble.SetActive(false);
        // }
        // else 
        // {
        //     first_aid_disble.SetActive(true); 
        // }
    }
   
  */

}
