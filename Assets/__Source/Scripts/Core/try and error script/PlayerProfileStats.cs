using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerProfileStats : MonoBehaviour
{
    public GameObject TotalMatches;
    public GameObject PlayerGoal;
    public GameObject Goalagainst;
    public GameObject Cleansheet;
    public GameObject MatchesWin;
    public GameObject MatchesLoss;
    public static PlayerProfileStats instance;
    
   // public GameObject PlayerGoal;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        
    }

    // Update is called once per frame
    void start()
    {
        //TotalMatches.GetComponent<Text>().text = 
    }
}
