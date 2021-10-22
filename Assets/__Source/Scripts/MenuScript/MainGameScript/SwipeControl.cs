///////////////////////////////////////////////
//// 		   SwipeControl.cs             ////
////  copyright (c) 2010 by Markus Hofer   ////
////          for GameAssets.net           ////
///////////////////////////////////////////////

using UnityEngine;
using System.Collections;

public class SwipeControl : MonoBehaviour {


	public bool controlEnabled = true; //turn this control on or off
	public bool skipAutoSetup = false; //If you set it up from another script you can skip the auto-setup! - Don't forget to call Setup() though!!
	public bool allowInput = true; //If you don't want to allow mouse or touch input and only want to use this control for animating a value, set this to false.
	public bool clickEdgeToSwitch = true; //When mouse-controlled, should a simple click on either side of the control increases/decreases the value by one?

	public float partWidth = 0f; // width
	private float partFactor = 1.0f; // calculated once in the beginning, so we don't have to do costly divisions every frame
	public int startValue = 0; // start with this value
	public int currentValue; // current value
	public int maxValue; //max value

	public Rect mouseRect; //where you can click to start the swipe movement (once you clicked you can drag outside as well)
	public Rect leftEdgeRectForClickSwitch;
	public Rect rightEdgeRectForClickSwitch;

	public Matrix4x4 matrix = Matrix4x4.identity;

	private bool touched = false; //dragging operation in progress?
	private int[] fingerStartArea = new int[5]; //set to 1 for each finger that starts touching the screen within our touchRect
	private int mouseStartArea = 0; //set to 1 if mouse starts clicking within touchRect
	public float smoothValue = 0.0f; //current smooth value between 0 and maxValue
	private float smoothStartPos; //
	private float smoothDragOffset = 0.1f; //how far (% of the width of one element) do we have to drag to set it to change currentValue?
	private float lastSmoothValue;
	private float[] prevSmoothValue = new float[5];
	private float realtimeStamp; //needed to make Mathf.SmoothDamp work even if Time.timeScale == 0
	private float xVelocity; //current velocity of Mathf.SmoothDamp()
	public float maxSpeed = 20.0f; //Clamp the maximum speed of Mathf.SmoothDamp()

	private Vector2 mStartPos;
	private Vector3 pos; //Touch/Mouse Position turned into a Vector3
	private Vector2 tPos; //transformed Position

	public bool debug = false;

	private bool calonce = false;

	public bool isSwiped, istouch;

	private static SwipeControl _instance = null;

	public static SwipeControl SharedInstance {
		get {
			// if the instance hasn't been assigned then search for it
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(SwipeControl)) as SwipeControl;

			}
			return _instance; 
		}
	}


	IEnumerator Start() {

		if(clickEdgeToSwitch && !allowInput) {
			Debug.LogWarning("You have enabled clickEdgeToSwitch, but it will not work because allowInput is disabled!", this);
		}

		yield return new WaitForSeconds(0.2f);

		if(!skipAutoSetup) {
			Setup();
		}
	}


	public void Setup() {
		partFactor = 1.0f/partWidth;
		smoothValue = (float) currentValue; //Set smoothValue to the currentValue - tip: setting currentValue to -1 and startValue to 0 makes the first image appear at the start...

		if (PlayerPrefs.GetInt ("currentSelectedCharact") == 1) {
			currentValue = 1; 
		}
		else if (PlayerPrefs.GetInt ("currentSelectedCharact") == 0) {
			currentValue = 0;
		}
		else if (PlayerPrefs.GetInt ("currentSelectedCharact") == 2) {
			currentValue = 2;
		}


		if(mouseRect != new Rect(0,0,0,0)) {
			SetMouseRect(mouseRect);	
		}

		if(leftEdgeRectForClickSwitch == new Rect(0,0,0,0)) CalculateEdgeRectsFromMouseRect(); //Only do this if not set in the inspector	

		if(matrix == Matrix4x4.zero) matrix = Matrix4x4.identity.inverse;
	}


	public void SetMouseRect(Rect myRect) {
		mouseRect = myRect;
	}


	public void CalculateEdgeRectsFromMouseRect () { CalculateEdgeRectsFromMouseRect(mouseRect); }
	public void CalculateEdgeRectsFromMouseRect (Rect myRect) {
		leftEdgeRectForClickSwitch.x = myRect.x;
		leftEdgeRectForClickSwitch.y = myRect.y;
		leftEdgeRectForClickSwitch.width = myRect.width * 0.5f;
		leftEdgeRectForClickSwitch.height = myRect.height;

		rightEdgeRectForClickSwitch.x = myRect.x + myRect.width * 0.5f;
		rightEdgeRectForClickSwitch.y = myRect.y;
		rightEdgeRectForClickSwitch.width = myRect.width * 0.5f;
		rightEdgeRectForClickSwitch.height = myRect.height;	
	}


	public void SetEdgeRects(Rect leftRect, Rect rightRect) {
		leftEdgeRectForClickSwitch = leftRect;
		rightEdgeRectForClickSwitch = rightRect;		
	}


	float GetAvgValue(float[] arr) {

		float sum = 0.0f;
		for(int i = 0; i < arr.Length; i++) {
			sum += arr[i];
		}

		return sum / arr.Length;

	}



	void FillArrayWithValue(float[] arr, float val) {

		for(int i = 0; i < arr.Length; i++) {
			arr[i] = val;	
		}

	}





	void FixedUpdate () {

		if(controlEnabled) {

			touched = false;

			if (allowInput) {

				if (Input.GetMouseButton (0) || Input.GetMouseButtonUp (0)) 
				{
					

					if (Input.mousePosition.y < (Screen.height / 2) / 2f || Input.mousePosition.y > ((Screen.height / 2) + 160f)) {
						if (Input.GetMouseButtonUp (0)) {
							if (calonce) {
								isSwiped = true;
								istouch = true;
								calonce = false;
							}
						} 
						if (Input.GetMouseButton (0)) {
							istouch = false;
						}
					} else {
						istouch = true;
						calonce = true;
						pos = new Vector3 (Input.mousePosition [0], Screen.height - Input.mousePosition [1], 0.0f);
						tPos = matrix.inverse.MultiplyPoint3x4 (pos);

						//BEGAN
						if (Input.GetMouseButtonDown (0) && mouseRect.Contains (tPos)) {
							mouseStartArea = 1;	
						}

						//WHILE MOUSEDOWN
						if (mouseStartArea == 1) {
							touched = true;
							//START
							if (Input.GetMouseButtonDown (0)) {
								mStartPos = tPos;
								smoothStartPos = smoothValue + tPos.x * partFactor;	
								FillArrayWithValue (prevSmoothValue, smoothValue);
							}
							//DRAGGING

							smoothValue = smoothStartPos - tPos.x * partFactor;

							if (smoothValue < -0.12f) {
								smoothValue = -0.12f;
							} else if (smoothValue > maxValue + 0.12f) {
								smoothValue = maxValue + 0.4f;

							}
							//END




							SwipeCharacters.SharedInstance.moveObj (smoothValue);


							if (Input.GetMouseButtonUp (0)) {
								calonce = false;
								SwipeCharacters.SharedInstance.ismoving = false;
								if ((tPos - mStartPos).sqrMagnitude < 25) {
									if (clickEdgeToSwitch) {
										if (leftEdgeRectForClickSwitch.Contains (tPos)) {
											currentValue--;
											if (currentValue < 0)
												currentValue = 0;
										} else if (rightEdgeRectForClickSwitch.Contains (tPos)) {
											currentValue++;
											if (currentValue > maxValue)
												currentValue = maxValue;
										}
									}
								} else {
									if (currentValue - (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue))) > smoothDragOffset || currentValue - (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue))) < -smoothDragOffset) { //dragged beyond dragOffset to the right
										currentValue = (int)Mathf.Round (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue)));
										xVelocity = (smoothValue - GetAvgValue (prevSmoothValue)); // * -0.10 ;
										if (currentValue > maxValue)
											currentValue = maxValue;
										else if (currentValue < 0)
											currentValue = 0;	
										
									}	
								}		
								mouseStartArea = 0;
								isSwiped = true;
							}
							for (int i = 1; i < prevSmoothValue.Length; i++) {
								prevSmoothValue [i] = prevSmoothValue [i - 1];
							}
							prevSmoothValue [0] = smoothValue;
						}


					}


		
				}

				//#if UNITY_IPHONE or UNITY_ANDROID

				if (istouch) {
					foreach (Touch touch in Input.touches) {
						pos = new Vector3 (touch.position.x, Screen.height - touch.position.y, 0.0f);
						tPos = matrix.inverse.MultiplyPoint3x4 (pos);		


						//BEGAN
						if (touch.phase == TouchPhase.Began && mouseRect.Contains (tPos)) {
							fingerStartArea [touch.fingerId] = 1;
						}
						//WHILE FINGER DOWN
						if (fingerStartArea [touch.fingerId] == 1) { // no touchRect.Contains check because once you touched down you're allowed to drag outside...
							touched = true;
							//START
							if (touch.phase == TouchPhase.Began) {
								smoothStartPos = smoothValue + tPos.x * partFactor;
								FillArrayWithValue (prevSmoothValue, smoothValue);
							}
							//DRAGGING
							smoothValue = smoothStartPos - tPos.x * partFactor; //print("smoothValue : " + smoothValue);
							if (smoothValue < -0.12f) {
								smoothValue = -0.12f;
							} else if (smoothValue > maxValue + 0.12f) {
								smoothValue = maxValue + 0.4f;
							}


							SwipeCharacters.SharedInstance.moveObj (smoothValue);
							//END
							if (touch.phase == TouchPhase.Ended) {
								SwipeCharacters.SharedInstance.ismoving = false;
								if (currentValue - (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue))) > smoothDragOffset || currentValue - (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue))) < -smoothDragOffset) { //dragged beyond dragOffset to the right
									currentValue = (int)Mathf.Round (smoothValue + (smoothValue - GetAvgValue (prevSmoothValue)));
									xVelocity = (smoothValue - GetAvgValue (prevSmoothValue)); // * -0.10 ;
									if (currentValue > maxValue)
										currentValue = maxValue;
									else if (currentValue < 0)
										currentValue = 0;					
								}							
							}

							for (int i = 1; i < prevSmoothValue.Length; i++) {
								prevSmoothValue [i] = prevSmoothValue [i - 1];
							}

							prevSmoothValue [0] = smoothValue;
						}


						if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
							fingerStartArea [touch.fingerId] = 0;

					}
					//#endif

				}



			}

			if(!touched) {
				smoothValue = Mathf.SmoothDamp(smoothValue, (float) currentValue, ref xVelocity, 0.15f, maxSpeed, Time.realtimeSinceStartup - realtimeStamp);
			} 

			realtimeStamp = Time.realtimeSinceStartup;	

		}

	}


	void OnGUI () {
		if(debug) {
			if(Input.touchCount > 0) {
				GUI.Label(new Rect(Input.GetTouch(0).position.x + 15, Screen.height - Input.GetTouch(0).position.y - 60, 200, 100), "pos : " + pos + "\ntPos: " + tPos);
			}

			//		GUI.Label(new Rect(Input.mousePosition.x + 15, Screen.height - Input.mousePosition.y - 60, 200, 100), "mPos : " + mPos + "\ntmPos: " + tmPos + "\ntPos: " + tPos);
			GUI.matrix = matrix;
			GUI.Box(mouseRect, GUIContent.none);
		}
	}

}
