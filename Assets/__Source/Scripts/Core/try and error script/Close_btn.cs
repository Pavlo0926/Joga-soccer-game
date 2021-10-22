using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Close_btn : MonoBehaviour
{

   
    public GameObject HealthPanel;
    private void Start()
    {
        HealthPanel = GameObject.Find("PlayerHealthPanel");
    }
    // Use this for initialization
  public void CloseBtn()
    {
        HealthPanel.SetActive(false);

//        for (int i = 0; i<GlobalGameManager.SharedInstance.allPlayer.Length; i++)
//        {
////            ChangePlayer.instance.playerCount = i;

                //BumpStaminaManager.instance.playerNumberForStaminaIncrease = i;
                //BumpStaminaManager.instance.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer[i];

                //BumpStaminaManager.instance.playerObjectCouterFromArray = i;
                //break;
           
       //}
		
	}
}
