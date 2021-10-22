using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardDeatilsPanel : MonoBehaviour
{

	public string playerName;
	public float playerValue;
	public float playerForce;
	public float playerAim;
	public float playerTime;
	public float playerGS;
	public float playerBS;
	public float playerExpPoint;
	public float playerEoU;
	public float playerAge;
	public float playerRetirementAge;
	public float playerSalary;
	public float playerAllowances;
	public string playerPosition;
	public float playerRating;
	public string playerFlagURl;

	public Text playerNameText;
	public Text playerValueText;
	public Text forceValueText;
	public Text aimValueText;
	public Text timeValueText;
	public Text gsValueText;
	public Text bsValueText;
	public Text exppointValueText;
	public Text expirencevalueText;
	public Text agevalueText;
	public Text retirementAgevalueText;
	public Text salaryvalueText;
	public Text allowancesvalueText;
	public Text positionValueText;
	public Text ratingValueText;

	public Image playerFlagImage;

	public Image playerDiskImage;

	public GameObject playerValueObject;

	public bool isActiveObject;

	void OnEnable ()
	{
		if (isActiveObject) {
			playerName = playerValueObject.GetComponent <MM_ActivePlayerStatminaValue> ().diskName;
			playerForce = playerValueObject.GetComponent <MM_ActivePlayerStatminaValue> ().forceData;
			playerTime = playerValueObject.GetComponent <MM_ActivePlayerStatminaValue> ().timeData;
			playerAim = playerValueObject.GetComponent <MM_ActivePlayerStatminaValue> ().aimData;
		} else {
			//playerName = playerValueObject.GetComponent <MM_InActivePlayerStatminaValue> ().diskName;
			//playerForce = playerValueObject.GetComponent <MM_InActivePlayerStatminaValue> ().forceData;
			//playerTime = playerValueObject.GetComponent <MM_InActivePlayerStatminaValue> ().timeData;
			//playerAim = playerValueObject.GetComponent <MM_InActivePlayerStatminaValue> ().aimData;
		}


		playerNameText.text = "" + playerName;
		playerValueText.text = "" + playerValue;
		forceValueText.text = "" + playerForce;
		aimValueText.text = "" + playerAim;
		timeValueText.text = "" + playerTime;
		gsValueText.text = "" + playerGS;
		bsValueText.text = "" + playerBS;
		exppointValueText.text = "" + playerExpPoint;
		expirencevalueText.text = "" + playerEoU;
		agevalueText.text = "" + playerAge;
		retirementAgevalueText.text = "" + playerRetirementAge;
		salaryvalueText.text = "" + playerSalary;
		allowancesvalueText.text = "" + playerAllowances;
		positionValueText.text = "" + playerPosition;
		ratingValueText.text = "" + playerRating;
	}

}
