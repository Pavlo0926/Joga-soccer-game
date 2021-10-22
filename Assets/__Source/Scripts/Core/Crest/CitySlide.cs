using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CitySlide : MonoBehaviour
{


	public GameObject[] sliderObject;
	public int counter2;

	public GameObject leftObj, rightObj, centerObj, btnNext, btnPrevious;


	public GameObject errorTextObject;


	public InputField crestNameInputFiled;
	// This boolean is used for sliding after x times. It is used in timer.

	private static CitySlide _instance = null;

	public static CitySlide SharedInstance {
		get {
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(CitySlide)) as CitySlide; 	
			}

			return _instance; 
		}
		set {

			_instance = value;

		}

	}

	void Start ()
	{
		sliderObject [0].transform.localPosition = centerObj.transform.localPosition;			//    Setting first slider object on initial or vector3(0,0,0) position.

		for (int i = 1; i < sliderObject.Length; i++) {
			sliderObject [i].transform.localPosition = rightObj.transform.localPosition;	//    Setting other four slider object on right position.
		}
	
		btnNext.SetActive (true);
		btnPrevious.SetActive (false);
	
	}

	/// <summary>
	/// This method used to slide on click next button on "On Boarding Screen". On Click next increase the counter2 variable and this number of object is moved like slide using DoMove.
	/// </summary>
	public void OnNextClick ()
	{
		if (counter2 < sliderObject.Length - 1) {
			sliderObject [counter2].transform.DOMoveX (leftObj.transform.position.x, 0.5f, false);
			counter2++;
			sliderObject [counter2].transform.DOMoveX (centerObj.transform.position.x, 0.5f, false);

			if (counter2 == sliderObject.Length - 1) {
				btnNext.SetActive (false);
				btnPrevious.SetActive (true);
			} else {
				btnPrevious.SetActive (true);
			}

		}
	}

	/// <summary>
	/// This method used to slide on click next button on "On Boarding Screen". On Click next increase the counter2 variable and this number of object is moved like slide using DoMove.
	/// </summary>
	public void OnPreviousClick ()
	{
		if (counter2 > 0) {
			sliderObject [counter2].transform.DOMoveX (rightObj.transform.position.x, 0.5f, false);
			counter2--;
			sliderObject [counter2].transform.DOMoveX (centerObj.transform.position.x, 0.5f, false);


			if (counter2 == 0) {
				btnNext.SetActive (true);
				btnPrevious.SetActive (false);
			} else {
				btnNext.SetActive (true);
			}
		} 
	}



	public void NextButton ()
	{
		if (crestNameInputFiled.text != "") {
			string _crestName = crestNameInputFiled.text;
			string _crestSymbolCount = counter2.ToString ();
// commented --@sud
//			WebServicesHandler.SharedInstance.GetUpdateCrestName (_crestName, _crestSymbolCount);
			GameStates.SetCurrent_State_TO (GAME_STATE.MAIN_MENU);
		} else {
			errorTextObject.SetActive (true);
			Invoke ("DisableErrortext", 1f);
		}
	}

	public void DisableErrortext ()
	{
		errorTextObject.SetActive (false);
	}

}
