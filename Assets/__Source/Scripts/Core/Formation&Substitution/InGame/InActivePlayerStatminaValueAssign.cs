using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InActivePlayerStatminaValueAssign : MonoBehaviour
{
    /*
	public float forceData;
	public float timeData;
	public float aimData;
	public string diskName;
	public string statusData;
	public float idDisc;
	public float staminaData;

	public bool isSelectedDisc;

	public Text discNameText;
	public Text discpositionText;
	public Text discRatingText;
	public Image discFlagImage;

	public float player_timer_Inact;

	public static InActivePlayerStatminaValueAssign instance;

	void Awake ()
	{
		instance = this;
	}

	void Start ()
	{
		AssignValueofDisk ();
	}

	public void SelectDisc ()
	{
		
//		for (int i = 0; i < AssignFormationData.instance.InActiveDiscs.Count; i++) {
//			AssignFormationData.instance.InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().DeSelectDisc ();
//		}
		for (int i = 0; i < SubStitutionManager.instance.InActiveDiscs.Count; i++) {
			SubStitutionManager.instance.InActiveDiscs [i].GetComponent <InActivePlayerStatminaValueAssign> ().DeSelectDisc ();
		}
		isSelectedDisc = true;
		SubStitutionManager.instance.PlayerDetailLabel.SetActive (true);
		AssignSubDiscDetailsShow.instance.ParentDetailObject = this.gameObject;
		AssignSubDiscDetailsShow.instance.assignValue ();
	}

	public void AssignValueofDisk ()
	{
		discNameText.text = "" + diskName;
	}

	public void DeSelectDisc ()
	{
		SubStitutionManager.instance.SwipeButton.SetActive (false);
		isSelectedDisc = false;
	}
    */
}
