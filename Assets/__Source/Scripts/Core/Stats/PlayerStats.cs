using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;



public class PlayerStats : MonoBehaviour
{/*

    public float count;

    public bool PrimaryAttackingDisc = false;
    public bool SecondaryAttackingDisc = false;
    public bool DirectOpponentDisc = false;
    public bool FirstIndirectOpponentDisc = false;
    public bool SecondaryIndirectOpponentDisc = false;
    public bool SecondaryIndirectAttackingDisc = false;

    public float maxPercentage_bs=10;
    public float maxPercentage_gs=20;

    public float zeroPercentage = 0f;
    public float twentyfivePercentage = 25f;
    public float fiftyPercentage = 50f;
    public float hundredPercentage = 100f;

    public float AttackingPlayerPercentage = 20f;
    public float MidfieldPlayerPercentage = 10f;
    public float DefensivePlayerPercentage = -15f;
    public float BoxToBoxPlayerPercentage = -20f;

    private static PlayerStats _instance = null;
   

    public List<string> touchDiscName;

    public bool isAttackingPlayer;
    public bool isMidfieldPlayer;
    public bool isDefensivePlayer;
    public bool isBoxToBoxPlayer;

    float betaFactorResult;

    public bool isChromeCap;
    public bool isGoldCap;
    public bool isSilverCap;

    public bool isElephant;
    public bool isRhino;
    public bool isLion;

    public float chromeCap_value = -0.5f;
    public float goldCap_value = -0.3f;
    public float silverCap_value = -0.2f;

    public float elephantCap_value = 0.5f;
    public float rhinoCap_value = 0.3f;
    public float lionCap_value = 0.2f;

    float addinditionOfValue;
    float BumperCap;

    public int protectioncap_Count;
    public int attackingcap_Count;

    public float hydrotherapy_value;
    public float massage_therapy_value;
    public float cryotherapy_value ;

  // public Button hydrotheropy;
  //  public Button massagetheropy;
  //  public Button cryotherapy;

  
public static PlayerStats SharedInstance {
        get {
            // if the instance hasn't been assigned then search for it
            if (_instance == null) {
                _instance = GameObject.FindObjectOfType(typeof(PlayerStats)) as PlayerStats;
            }
            return _instance;
        }

	}

	// Use this for initialization
	void Start ()
	{
       hydrotherapy_value = 0;
       massage_therapy_value = 0;
       cryotherapy_value = 0;
       count = 0;
        
	}

    public void HydroTheropy_use()
    {
        hydrotherapy_value += 0.2f;
        // Debug.Log("hydro use" + hydrotherapy_value);
    }


    public void MassageTheropy_use()

    {
            massage_therapy_value += 0.3f;
            // Debug.Log("massage done" + massage_therapy_value);

    }
    public void CryoTheropy_use()
    {
            cryotherapy_value += 0.5f;
            // Debug.Log("cryo used" + cryotherapy_value);
            
    }
       

  

    public void Gs_calculation (float time_percentage)
	{
		float time = time_percentage / 100;
		float gs_percentage = time * (maxPercentage_gs / 100);

       float gs_result = gs_percentage - (gs_percentage * hydrotherapy_value) - (gs_percentage * massage_therapy_value) - (gs_percentage * cryotherapy_value);

        Debug.LogError ("time in perc--" + time + "hydrotherapy_value---" + hydrotherapy_value + "gs_percentage---" + gs_percentage + "gs_result-------" + gs_result + "force---" + this.GetComponent <playerController> ().Force);

        float gf = this.GetComponent <playerController> ().Force - (this.GetComponent <playerController> ().Force * gs_result);
		float ga = this.GetComponent <playerController> ().mDisc_aim - (this.GetComponent <playerController> ().mDisc_aim * gs_result);
		float gt = this.GetComponent <playerController> ().timePlayer - (this.GetComponent <playerController> ().timePlayer * gs_result);
		Debug.LogError ("gsforce-------" + gf + " gsaim-------" + ga + "gstime-------" + gt);


		this.GetComponent <playerController> ().Force = gf;
		this.GetComponent <playerController> ().mDisc_aim = ga;
		this.GetComponent <playerController> ().timePlayer = gt;

		WebServicesHandler.SharedInstance.updateDisk (gf, gt, 100, ga, 
			this.GetComponent <playerController> ().mDisc_name,
			this.GetComponent <playerController> ().mDisc_id,
			this.GetComponent <playerController> ().mDisc_status);

		Debug.LogError ("name-------" + this.GetComponent <playerController> ().mDisc_name + "disc id-------" + this.GetComponent <playerController> ().mDisc_id);
	}


	public void Calculation (float value)
	{
       
        float result = value; 

		if (result == 0)
			return;

		if (isAttackingPlayer) {
			betaFactorResult = AttackingPlayerPercentage;  //20
		} else if (isMidfieldPlayer) {
			betaFactorResult = MidfieldPlayerPercentage;   //10
        } else if (isDefensivePlayer){
			betaFactorResult = DefensivePlayerPercentage;  //-15
		} else if (isBoxToBoxPlayer) {
			betaFactorResult = BoxToBoxPlayerPercentage;   //-20
		}


		addinditionOfValue = result + betaFactorResult;
	
		if (isChromeCap) { 
			
			float chromeValue = addinditionOfValue / 100;     // = 20/100, 10/100 , -15/100 , -20/100
			BumperCap = chromeCap_value * chromeValue;        // = -0.5*2= -1, -0.5*1 =-1 , -0.5*-1.5=0.75, -0.5*-2 =1
			BumperCap = BumperCap * 100;

			addinditionOfValue = addinditionOfValue + BumperCap;

			addinditionOfValue = (addinditionOfValue / 100) * (maxPercentage_bs / 100);

			protectioncap_Count++;
            StatsManager.instance.Defhits.text = (3-protectioncap_Count).ToString();  // hits remain.
		
            	if (protectioncap_Count >= 3) {
				protectioncap_Count = 0;
				isChromeCap = false;
				
                BumpStaminaManager.instance.isChrome_btn = false;
                StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                if (this.gameObject.tag == "Player") { 
					this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
				} else if (this.gameObject.tag == "Player_2") { 
					this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
				}

			}
		} else if (isGoldCap) {
			
			float chromeValue = addinditionOfValue / 100;
			BumperCap = goldCap_value * chromeValue;
			BumperCap = BumperCap * 100;

			addinditionOfValue = addinditionOfValue + BumperCap;

			addinditionOfValue = (addinditionOfValue / 100) * (maxPercentage_bs / 100);

			protectioncap_Count++;
            StatsManager.instance.Defhits.text = (3-protectioncap_Count).ToString();

            if (protectioncap_Count >= 3) {
				protectioncap_Count = 0;
				isGoldCap = false;

				BumpStaminaManager.instance.isGold_btn = false;
                StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                if (this.gameObject.tag == "Player") { 
					this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
				} else if (this.gameObject.tag == "Player_2") { 
					this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
				}
			}

		} else if (isSilverCap) {
			
			float chromeValue = addinditionOfValue / 100;
			BumperCap = silverCap_value * chromeValue;
			BumperCap = BumperCap * 100;

			addinditionOfValue = addinditionOfValue + BumperCap;

			addinditionOfValue = (addinditionOfValue / 100) * (maxPercentage_bs / 100);

			protectioncap_Count++;
            StatsManager.instance.Defhits.text =(3- protectioncap_Count).ToString();

            if (protectioncap_Count >= 3) {
				protectioncap_Count = 0;
				isSilverCap = false;
				
                BumpStaminaManager.instance.isSilver_btn = false;
                StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                if (this.gameObject.tag == "Player") { 
					this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
				} else if (this.gameObject.tag == "Player_2") { 
					this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
				}
			}

		} else {

			addinditionOfValue = (addinditionOfValue / 100) * (maxPercentage_bs / 100);

		}

		if (this.gameObject.tag == "Player") { 
			Debug.LogError ("playerName=" + this.gameObject + " addinditionOfValue=" + addinditionOfValue * this.GetComponent <playerController> ().Force);

			this.GetComponent <playerController> ().Force -= addinditionOfValue * this.GetComponent <playerController> ().Force;
			this.GetComponent <playerController> ().timePlayer -= addinditionOfValue * this.GetComponent <playerController> ().timePlayer;
			this.GetComponent <playerController> ().mDisc_aim -= addinditionOfValue * this.GetComponent <playerController> ().mDisc_aim;

			if (this.GetComponent <playerController> ().Force <= 0) {
				this.GetComponent <playerController> ().Force = 0;
			}
			if (this.GetComponent <playerController> ().timePlayer <= 0) {
				this.GetComponent <playerController> ().timePlayer = 0;
			}
			if (this.GetComponent <playerController> ().mDisc_aim <= 0) {
				this.GetComponent <playerController> ().mDisc_aim = 0;
			}

			this.GetComponent <playerController> ().UpdateStatsValue ();

			WebServicesHandler.SharedInstance.updateDisk (this.GetComponent <playerController> ().Force, 
				this.GetComponent <playerController> ().timePlayer, 100, 
				this.GetComponent <playerController> ().mDisc_aim, 
				this.GetComponent <playerController> ().mDisc_name,
				this.GetComponent <playerController> ().mDisc_id,
				this.GetComponent <playerController> ().mDisc_status
			);
		} else if (this.gameObject.tag == "Player_2") {
			this.GetComponent <Player2Controller> ().mDisc_aim -= addinditionOfValue * this.GetComponent <Player2Controller> ().mDisc_aim;
			this.GetComponent <Player2Controller> ().Force -= addinditionOfValue * this.GetComponent <Player2Controller> ().Force;
			this.GetComponent <Player2Controller> ().timeplayer2 -= addinditionOfValue * this.GetComponent <Player2Controller> ().timeplayer2;

			if (this.GetComponent <Player2Controller> ().Force <= 0) {
				this.GetComponent <Player2Controller> ().Force = 0;
			}
			if (this.GetComponent <Player2Controller> ().timeplayer2 <= 0) {
				this.GetComponent <Player2Controller> ().timeplayer2 = 0;
			}
			if (this.GetComponent <Player2Controller> ().mDisc_aim <= 0) {
				this.GetComponent <Player2Controller> ().mDisc_aim = 0;
			}

			this.GetComponent <Player2Controller> ().UpdateStatsValue ();

		}
		return;
	}


	float result2;

	public void Calculation_Onattack (float value, string attackValue)
	{
		if (value == 0 && attackValue == "ZERO") {
			
			if (isChromeCap) {

				protectioncap_Count++;
                StatsManager.instance.Defhits.text = (3-protectioncap_Count).ToString();

                if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isChromeCap = false;

					BumpStaminaManager.instance.isChrome_btn = false;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}
			
			} else if (isGoldCap) {

				protectioncap_Count++;
                StatsManager.instance.Defhits.text =(3- protectioncap_Count).ToString();

                if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isGoldCap = false;

					BumpStaminaManager.instance.isGold_btn = false;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}
			
			} else if (isSilverCap) {

				protectioncap_Count++;
                StatsManager.instance.Defhits.text = (3-protectioncap_Count).ToString();

                if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isSilverCap = false;
					
                    BumpStaminaManager.instance.isSilver_btn = false;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}
			}
		}



		if (value == 0)
			return;

		if (isAttackingPlayer) {
			betaFactorResult = AttackingPlayerPercentage;
		} else if (isMidfieldPlayer) {
			betaFactorResult = MidfieldPlayerPercentage;
		} else if (isDefensivePlayer) {
			betaFactorResult = DefensivePlayerPercentage;
		} else if (isBoxToBoxPlayer) {
			betaFactorResult = BoxToBoxPlayerPercentage;
		}

		result2 = value + betaFactorResult;      // 35

		switch (attackValue) {

		case "isElephant":
			if (isChromeCap) {
				
				float valuePersantange = result2 / 100;

				float attackingPersantage = elephantCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = chromeCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isChromeCap = false;

					BumpStaminaManager.instance.isChrome_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}


			} else if (isGoldCap) {
				float valuePersantange = result2 / 100;

				float attackingPersantage = elephantCap_value * valuePersantange; //60
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = goldCap_value * valuePersantange; //60
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange; //240
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));
                     
				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isGoldCap = false;

					BumpStaminaManager.instance.isGold_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}


			} else if (isSilverCap) {

				float valuePersantange = result2 / 100;

				float attackingPersantage = elephantCap_value * valuePersantange; //60
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = silverCap_value * valuePersantange; //60
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange; //240
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isSilverCap = false;

					BumpStaminaManager.instance.isSilver_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}

			} else {

				float valuePersantange = result2 / 100; //0.7

				BumperCap = elephantCap_value * valuePersantange; //0.35
				BumperCap = BumperCap * 100; //35
				float CapAttack = BumperCap + value; // 85

				result2 = result2 + CapAttack;  //155
				result2 = ((result2 / 100) * (maxPercentage_bs / 100)); //0.155

			}
			break;
		case "isRhino":
			if (isChromeCap) {
				float valuePersantange = result2 / 100;

				float attackingPersantage = rhinoCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = chromeCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));


				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isChromeCap = false;
					BumpStaminaManager.instance.isChrome_btn = false;
					if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}


			} else if (isGoldCap) {

				float valuePersantange = result2 / 100;

				float attackingPersantage = rhinoCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = goldCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));


				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isGoldCap = false;

					BumpStaminaManager.instance.isGold_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}

		
			} else if (isSilverCap) {
				float valuePersantange = result2 / 100;

				float attackingPersantage = rhinoCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = silverCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isSilverCap = false;

					BumpStaminaManager.instance.isSilver_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}


			} else {

				float valuePersantange = result2 / 100;

				BumperCap = rhinoCap_value * valuePersantange;
				BumperCap = BumperCap * 100;
				float CapAttack = BumperCap + value;

				result2 = result2 + CapAttack;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));
			}
			break;
		case "isLion":
			if (isChromeCap) {

				float valuePersantange = result2 / 100;

				float attackingPersantage = lionCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = chromeCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isChromeCap = false;

					BumpStaminaManager.instance.isChrome_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}


			} else if (isGoldCap) {

				float valuePersantange = result2 / 100;

				float attackingPersantage = lionCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = goldCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isGoldCap = false;

					BumpStaminaManager.instance.isGold_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}

			
			} else if (isSilverCap) {
				float valuePersantange = result2 / 100;

				float attackingPersantage = lionCap_value * valuePersantange;
				attackingPersantage = attackingPersantage * 100;

				float protectingPersantange = silverCap_value * valuePersantange;
				protectingPersantange = protectingPersantange * 100;

				result2 = result2 + attackingPersantage + protectingPersantange;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));

				protectioncap_Count++;
                    StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                    if (protectioncap_Count >= 3) {
					protectioncap_Count = 0;
					isSilverCap = false;

					BumpStaminaManager.instance.isSilver_btn = false;
                        StatsManager.instance.Defhits.text = protectioncap_Count.ToString();

                        if (this.gameObject.tag == "Player") { 
						this.GetComponent<playerController> ().proctective_Cap.SetActive (false);
					} else if (this.gameObject.tag == "Player_2") { 
						this.GetComponent<Player2Controller> ().proctective_Cap.SetActive (false);
					}
				}

			
			} else {

				float valuePersantange = result2 / 100;

				BumperCap = lionCap_value * valuePersantange;
				BumperCap = BumperCap * 100;
				float CapAttack = BumperCap + value;

				result2 = result2 + CapAttack;
				result2 = ((result2 / 100) * (maxPercentage_bs / 100));
			}
			break;


		default:
			break;
		}


		if (this.gameObject.tag == "Player") { 
			this.GetComponent <playerController> ().Force -= result2 * this.GetComponent <playerController> ().Force;
			this.GetComponent <playerController> ().timePlayer -= result2 * this.GetComponent <playerController> ().timePlayer;
			this.GetComponent <playerController> ().mDisc_aim -= result2 * this.GetComponent <playerController> ().mDisc_aim;

			if (this.GetComponent <playerController> ().Force <= 0) {
				this.GetComponent <playerController> ().Force = 0;
			}
			if (this.GetComponent <playerController> ().timePlayer <= 0) {
				this.GetComponent <playerController> ().timePlayer = 0;
			}
			if (this.GetComponent <playerController> ().mDisc_aim <= 0) {
				this.GetComponent <playerController> ().mDisc_aim = 0;
			}

			this.GetComponent <playerController> ().UpdateStatsValue ();

			WebServicesHandler.SharedInstance.updateDisk (this.GetComponent <playerController> ().Force, 
				this.GetComponent <playerController> ().timePlayer, 100, 
				this.GetComponent <playerController> ().mDisc_aim, 
				this.GetComponent <playerController> ().mDisc_name,
				this.GetComponent <playerController> ().mDisc_id,
				this.GetComponent <playerController> ().mDisc_status
			);
		} else if (this.gameObject.tag == "Player_2") {
			this.GetComponent <Player2Controller> ().Force -= result2 * this.GetComponent <Player2Controller> ().Force;
			this.GetComponent <Player2Controller> ().timeplayer2 -= result2 * this.GetComponent <Player2Controller> ().timeplayer2;
			this.GetComponent <Player2Controller> ().mDisc_aim -= result2 * this.GetComponent <Player2Controller> ().mDisc_aim;

			if (this.GetComponent <Player2Controller> ().Force <= 0) {
				this.GetComponent <Player2Controller> ().Force = 0;
			}
			if (this.GetComponent <Player2Controller> ().timeplayer2 <= 0) {
				this.GetComponent <Player2Controller> ().timeplayer2 = 0;
			}
			if (this.GetComponent <Player2Controller> ().mDisc_aim <= 0) {
				this.GetComponent <Player2Controller> ().mDisc_aim = 0;
			}

			this.GetComponent <Player2Controller> ().UpdateStatsValue ();
		} 

		Debug.LogError ("playerName=" + this.gameObject + " result=" + result2);
		return;
	}

	public void ResetAllDis ()
	{
		touchDiscName.Clear ();
		PrimaryAttackingDisc = false;
		SecondaryAttackingDisc = false;
		DirectOpponentDisc = false;
		FirstIndirectOpponentDisc = false;
		SecondaryIndirectOpponentDisc = false;

   }
   public void ResetTheropyvalue()
    {

        PlayerStats.SharedInstance.hydrotherapy_value = 0;
        PlayerStats.SharedInstance.massage_therapy_value = 0;
        PlayerStats.SharedInstance.cryotherapy_value = 0;
    }
    */
}

