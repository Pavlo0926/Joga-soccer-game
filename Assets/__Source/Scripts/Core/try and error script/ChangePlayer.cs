using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangePlayer : MonoBehaviour {

    //public List<GameObject> discPlayer = new List<GameObject> 
    public GameObject currentDisc;
    public GameObject[] totalDisc;
    public int discIndex;
    public int selectedDisc;


    private void Start()
    {
      // currentDisc = BumpStaminaManager.instance.playerObjecctFind;
        totalDisc = GlobalGameManager.Instance.allPlayer;
    }

public void BtnNext()
    {
        ChangeDisc(1);
    }

    void ChangeDisc(int incer)
    {
        discIndex += incer;
        if (discIndex == totalDisc.Length)
            discIndex = 0;
        if (discIndex < 0)
            discIndex = totalDisc.Length - 1;
        currentDisc = totalDisc[discIndex];
        //BumpStaminaManager.instance.playerObjecctFind = currentDisc;
      
    }


}
