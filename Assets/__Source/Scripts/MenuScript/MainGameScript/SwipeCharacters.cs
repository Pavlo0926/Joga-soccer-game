using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SwipeCharacters : MonoBehaviour
{

	public SwipeControl swipeCtrl;
	public  Transform[] obj = new Transform[0];
	
	public GameObject movePlayer;

	public float minXPos = 100;
	//min x position of the camera
	public float maxXPos = 225;
	//max x position of the camera
	public float xDist;
	//distance between camMinXPos and camMaxXPos
	private float xDistFactor;
	// = 1/camXDist

	private float swipeSmoothFactor = 1.0f;
	// 1/swipeCtrl.maxValue

	private float rememberYPos;
	public Animator[] animPlayer;
	bool callonce;
	public Animator animMove;

	private static SwipeCharacters _instance = null;

	public static SwipeCharacters SharedInstance {
		get {
			// if the instance hasn't been assigned then search for it
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(SwipeCharacters)) as SwipeCharacters;

			}
			return _instance; 
		}
	}

	void Start ()
	{

			
		xDist = maxXPos - minXPos; //calculate distance between min and max
		xDistFactor = 1.0f / xDist;


		swipeCtrl.skipAutoSetup = true; //skip auto-setup, we'll call Setup() manually once we're done changing stuff
		swipeCtrl.clickEdgeToSwitch = false; //only swiping will be possible
		swipeCtrl.SetMouseRect (new Rect (0, 0, Screen.width, Screen.height)); //entire screen
		swipeCtrl.maxValue = obj.Length - 1; //max value
		swipeCtrl.currentValue = 0; //current value set to max, so it starts from the end
		swipeCtrl.startValue = Mathf.RoundToInt (swipeCtrl.maxValue * 0.5f); //when Setup() is called it will animate from the end to the middle
		swipeCtrl.partWidth = Screen.width / swipeCtrl.maxValue; //how many pixels do you have to swipe to change the value by one? in this case we make it dependent on the screen-width and the maxValue, so swiping from one edge of the screen to the other will scroll through all values.
		swipeCtrl.Setup ();

		swipeSmoothFactor = 1.0f / swipeCtrl.maxValue; //divisions are expensive, so we'll only do this once in start

		rememberYPos = obj [0].position.y;

		callonce = false;

		if (PlayerPrefs.GetInt ("currentSelectedCharact") == 1) {
			swipeCtrl.currentValue = 1;
		}
		else if (PlayerPrefs.GetInt ("currentSelectedCharact") == 0) {
			swipeCtrl.currentValue = 0;
		}
		else if (PlayerPrefs.GetInt ("currentSelectedCharact") == 2) {
			swipeCtrl.currentValue = 2;
		}


	}

	float value;
	public bool ismoving;

	void FixedUpdate ()
	{

		if (!ismoving) {

			for (int i = 0; i < obj.Length; i++) {



				value = minXPos + i * (xDist * swipeSmoothFactor) - swipeCtrl.smoothValue * swipeSmoothFactor * xDist;

				obj [i].localPosition = new Vector3 (value, -7f, obj [i].localPosition.z);


				obj [i].localScale = new Vector3 (1f, 1f, 1f);

				obj [swipeCtrl.currentValue].localScale = new Vector3 (1.25F, 1.25f, 1.25f);
				obj [swipeCtrl.currentValue].localPosition = new Vector3 (obj [swipeCtrl.currentValue].localPosition.x, -7f, obj [swipeCtrl.currentValue].localPosition.z);

				//callonce = false;
				//obj[i].position.y = 1.0 * (1 - Mathf.Clamp(Mathf.Abs(i - swipeCtrl.smoothValue), 0.0, 1.0)); //move selected one up a little
			}	

			//if (swipeCtrl.currentValue == 0) {
				if (SwipeControl.SharedInstance.isSwiped) {
					callonce = false;

				}
			//}


			if (swipeCtrl.currentValue == 0) {

				if (!callonce) {
					callonce = true;



				}

			}
			else if (swipeCtrl.currentValue == 1) {
								
				if (!callonce) {
					callonce = true;



			

				}
			}
			else if (swipeCtrl.currentValue == 2) {
								
				if (!callonce) {
					callonce = true;

				

				

				}
			}



		
		}




//		if (Input.GetMouseButton (0)) {
//			Vector3 worlPos = Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 10f));
//			//print (worlPos);
//
//				
//			}


	
		
	}
		


	

	public void moveObj (float currentValue1)
	{

		ismoving = true;

		int currentValue = (int)Mathf.Round (currentValue1);

		for (int i = 0; i < obj.Length; i++) {


			value = minXPos + i * (xDist * swipeSmoothFactor) - swipeCtrl.smoothValue * swipeSmoothFactor * xDist;

			obj [i].localPosition = new Vector3 (value, -7f, obj [i].localPosition.z);


			obj [i].localScale = new Vector3 (1f, 1f, 1f);

			obj [currentValue].localScale = new Vector3 (1.25F, 1.25f, 1.25f);
			obj [currentValue].localPosition = new Vector3 (obj [currentValue].localPosition.x, -7f, obj [currentValue].localPosition.z);


			//obj[i].position.y = 1.0 * (1 - Mathf.Clamp(Mathf.Abs(i - swipeCtrl.smoothValue), 0.0, 1.0)); //move selected one up a little
		}	
	}


}
