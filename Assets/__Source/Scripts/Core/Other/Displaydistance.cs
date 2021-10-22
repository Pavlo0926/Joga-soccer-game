using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity;
public class Displaydistance : MonoBehaviour {

    public static Displaydistance instance;

    public Text Disc1_dist;
    public Text Disc2_dist;
    public Text Disc3_dist;
    public Text Disc4_dist;
    public Text Disc5_dist;

    public Text ODisc1_dist;
    public Text ODisc2_dist;
    public Text ODisc3_dist;
    public Text ODisc4_dist;
    public Text ODisc5_dist;

    public Text BallDistance;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
    {
//        BallDistance.text = BallManager.instace.BalldistanceTravel.ToString();

        // Disc1_dist.text = GlobalGameManager.SharedInstance.allPlayer[0].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();
        // Disc2_dist.text = GlobalGameManager.SharedInstance.allPlayer[1].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();
        // Disc3_dist.text = GlobalGameManager.SharedInstance.allPlayer[2].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();
        // Disc4_dist.text = GlobalGameManager.SharedInstance.allPlayer[3].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();
        // Disc5_dist.text = GlobalGameManager.SharedInstance.allPlayer[4].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();


        // ODisc1_dist.text = GlobalGameManager.SharedInstance.all2Player[0].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();

        // ODisc2_dist.text = GlobalGameManager.SharedInstance.all2Player[1].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();

        // ODisc3_dist.text = GlobalGameManager.SharedInstance.all2Player[2].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();

        // ODisc4_dist.text = GlobalGameManager.SharedInstance.all2Player[3].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();

        // ODisc5_dist.text = GlobalGameManager.SharedInstance.all2Player[4].GetComponent<PlayerDistanceCalculator>().distanceTravelled.ToString();



    }

  

}
