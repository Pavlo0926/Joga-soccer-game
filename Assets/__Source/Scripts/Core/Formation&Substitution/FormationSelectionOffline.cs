using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Hashtable = ExitGames.Client.Photon.Hashtable;
using FastSkillTeam;
//NOTE: THIS DOUBLES FOR ONLINE NOW!
public class FormationSelectionOffline : MonoBehaviour
{
    public float time;
   // public Text tm;

	// Use this for initialization
	void Start ()
	{
        time = 0;
        if(SceneManager.GetActiveScene().name ==("InGame") )
        UIManager.Instance.RemainingTime.enabled = true;
	}

    public void formationSelect(int index_number)
    {
        /*GameManager.p1FormationCounter = index_number;
        Debug.Log("Formation = " + index_number);

        if (FST_Gameplay.IsMultiplayer)
        {
            Hashtable ht = new Hashtable
            {

            { FST_PlayerProps.FORMATION, index_number },
            { FST_PlayerProps.READY, false }

            };
            Debug.LogFormat("Set PlayerProps > Formation: {0} ... Ready {1}", index_number, false);
            Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
        }
        FST_SettingsManager.Formation = index_number;


        if (SceneManager.GetActiveScene().name == "InGame")
        {
            if (FST_Gameplay.IsMultiplayer)
            {
              //  GlobalGameManager.SharedInstance.CloseInGameFormation();
            }
            else
            {
                UIManager.Instance.CloseFormationPanel();
                UIManager.Instance.SecondPlayerFormation();
            }
        }
        else
        {
            if (FST_Gameplay.IsMultiplayer)
            {
                GameManager.Instance.MainMenuEvents("formation_online");
            }
            else
                GameManager.Instance.MainMenuEvents("formation");//Hook this up logically!
        }*/

    }

	public void formationSelect2 (int id)
	{       
       /* GameManager.p2FormationCounter = id;
        FST_SettingsManager.FormationOpponent = id;

        Debug.Log("Formation2 = " + id);
        
        if (SceneManager.GetActiveScene ().name == "MainMenu") {
             Debug.Log("ActiveLoading2Panel-19");
             UIController.Instance.ActiveLoading2Panel();
			 GameManager.Instance.MainMenuEvents("formation2");
			
        }
        else {
             UIManager.Instance.CloseFormationPanel ();
            //UIManager.instace.LoadingInGame.SetActive(true);
            UIManager.Instance.LoadingInGame.SetActive(true);
            GameManager.Instance.LoadLevel("InGame");
            //if (SceneManager.GetActiveScene().name == ("InGame"))
            //    UIManager.instace.RemainingTime.enabled = false;
           
		}*/
	}

}
