using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class AssignSubDiscDetailsShow : MonoBehaviour
{
    /*

	public Text discNameText;
	public Text discRatingText;
	public Text discpositionText;
	public Image discFlagImage;

	public Image discForceImage;
	public Image discAimImage;
	public Image discTimeImage;

	public Image discProtectiveCapImage;
	public Image discAttckingCapImage;

	public Image dischealthImage;
	public Text discTotalStaminavalue;

	public GameObject ParentDetailObject;

	public static AssignSubDiscDetailsShow instance;

	void Awake ()
	{
		instance = this;
	}

	// Use this for initialization
	void Start ()
	{

	}

	void OnEnable ()
	{
		
	}

	public void assignValue ()
	{
		discNameText.text = "" + ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().diskName;
		
		discForceImage.fillAmount = ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().forceData / AssignFormationData.instance.initialvalueForce;
		discAimImage.fillAmount = ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().aimData / AssignFormationData.instance.initialvalueAIM;
		discTimeImage.fillAmount = ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().timeData / AssignFormationData.instance.initialvalueTime;


		AssignHeathSignColorValue ();
		ActiveProtacativeCap ();
		ActiveAttackingCap ();
		
	}

	public void AssignHeathSignColorValue ()
	{
		float max_aid = (ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().forceData + ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().timeData + ParentDetailObject.GetComponent <InActivePlayerStatminaValueAssign> ().aimData) * 100;

		float persantangeValue = max_aid / (BumpStaminaManager.instance.initialvalueForce + BumpStaminaManager.instance.initialvalueAIM + BumpStaminaManager.instance.initialvalueTime);

		if (80 <= persantangeValue) {
			dischealthImage.color = new Color (0f, 0.7450f, 0f, 1f);
		} else if (80 > persantangeValue && 60 <= persantangeValue) {
			dischealthImage.color = new Color (0f, 1f, 1f, 1f);
		} else if (60 > persantangeValue && 40 <= persantangeValue) {
			dischealthImage.color = new Color (1f, 1f, 0.02f, 1f);
		} else if (40 > persantangeValue && 20 <= persantangeValue) {
			dischealthImage.color = new Color (1f, 0.55f, 0.019f, 1f);
		} else if (20 >= persantangeValue) {
			dischealthImage.color = new Color (1f, 0f, 0.018f, 1f);
		}

		discTotalStaminavalue.text = "" + persantangeValue;
	}

	public void ActiveProtacativeCap ()
	{
		if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isElephant) {
			discProtectiveCapImage.gameObject.SetActive (true);
			discProtectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites [0];
		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isRhino) {
			discProtectiveCapImage.gameObject.SetActive (true);
			discProtectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites [1];
		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isLion) {
			discProtectiveCapImage.gameObject.SetActive (true);
			discProtectiveCapImage.sprite = BumpStaminaManager.instance.protectingCapSprites [2];
		} else {
			discProtectiveCapImage.gameObject.SetActive (false);
		}
	}

	public void ActiveAttackingCap ()
	{
		if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isChromeCap) {
			discAttckingCapImage.gameObject.SetActive (true);
			discAttckingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites [0];
		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isGoldCap) {
			discAttckingCapImage.gameObject.SetActive (true);
			discAttckingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites [1];
		} else if (BumpStaminaManager.instance.playerObjecctFind.GetComponent <PlayerStats> ().isSilverCap) {
			discAttckingCapImage.gameObject.SetActive (true);
			discAttckingCapImage.sprite = BumpStaminaManager.instance.acttackingCapSprites [2];
		} else {
			discAttckingCapImage.gameObject.SetActive (false);
		}
	}
    */
	// Update is called once per frame
	void Update ()
	{
		
	}
}
