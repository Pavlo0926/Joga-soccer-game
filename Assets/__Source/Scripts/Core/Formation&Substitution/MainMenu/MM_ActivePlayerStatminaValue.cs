using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MM_ActivePlayerStatminaValue : MonoBehaviour
{
	public float forceData;
	public float timeData;
	public float aimData;
	public string diskName;
	public string statusData;
	public float idDisc;
	public float staminaData;

    public Text Captain;

	public bool isSelectedDisc;

	public float player_timer_act;

	public float firstTapTime = 0f;
	public float timeBetweenTaps = 0.3f;
	// time between taps to be resolved in double tap
	public bool doubleTapInitialized;

	public GameObject PlayerCardDetailsObject;

	public static MM_ActivePlayerStatminaValue instance;

	void Awake ()
	{
		instance = this;
	}

	public void SelectDisc ()
	{

		if (!doubleTapInitialized) {
			// init double tapping
			doubleTapInitialized = true;
			firstTapTime = Time.time;
			Invoke ("CancelTimer", timeBetweenTaps);

		} else if (Time.time - firstTapTime < timeBetweenTaps) {
			CancelInvoke ("CancelTimer");
			doubleTapInitialized = false;
			//AssignFormationData.instance.ActiveGS_RevatioliztionPanel (); //As per MVP task list
			return;	
		} 
	}

	public void CancelTimer ()
	{
		doubleTapInitialized = false;

		for (int j = 0; j < AssignFormationData.Instance.ActiveDiscs.Count; j++) {
			AssignFormationData.Instance.ActiveDiscs [j].GetComponent <MM_ActivePlayerStatminaValue> ().DeSelectDisc ();
			AssignFormationData.Instance.ActiveDiscs [j].GetComponent <MM_ActivePlayerStatminaValue> ().PlayerCardDetailsObject.SetActive (false);
		}
		isSelectedDisc = true;

        //Comment for MVP task
        //for (int i = 0; i < AssignFormationData.instance.InActiveDiscs.Count; i++) {
        //	if (AssignFormationData.instance.InActiveDiscs [i].GetComponent <MM_InActivePlayerStatminaValue> ().isSelectedDisc) {
        //		AssignFormationData.instance.SecondPlayerTap ();
        //	} else {
        //		AssignFormationData.instance.FirstPlayerTap ();
        //	}
        //}

        if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x < -80f && this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y > 110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x < -80f && this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y < -110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x > 65f && this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y > 110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (1f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (1f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x > 65f && this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y < -110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (1f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (1f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x > 65f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (1f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (1f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.x < -80f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y > 110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0.5f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0.5f, 1f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else if (this.gameObject.GetComponent <RectTransform> ().anchoredPosition.y < -110f) {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0.5f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0.5f, 0f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		} else {
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMin = new Vector2 (0.5f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().anchorMax = new Vector2 (0.5f, 0.5f);
			PlayerCardDetailsObject.GetComponent <RectTransform> ().pivot = new Vector2 (0.5f, 0.5f);
		}

		PlayerCardDetailsObject.SetActive (true);
	}


	public void DeSelectDisc ()
	{
		PlayerCardDetailsObject.SetActive (false);
		AssignFormationData.Instance.NonePlayerTap ();
		isSelectedDisc = false;
	}

  
}
