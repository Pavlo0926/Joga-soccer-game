using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AssignAchievementData : MonoBehaviour
{
	public string achievementId;
	public string achievementName;
	public string achievementType;
	public string amount;
	public string reward;
	public string description;
	public string rewardType;
	public string status;

	public Text achievementNameText;
	public Text descriptionText;
	public Text rewardText;
	public Text targetText;

	public Button ClaimRewardButton;

	// Use this for initialization
	void Start ()
	{
		achievementNameText.text = "" + achievementName;
		descriptionText.text = "" + description;
		rewardText.text = "" + reward;
		targetText.text = "0" + "/" + amount;
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}
}
