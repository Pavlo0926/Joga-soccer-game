using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserClick : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}



	public void ClickOnUser(GameObject parentObj)
	{
		for (int i = 0; i < Playfriends.SharedInstance.friendsPlayer.Count; i++) {
			Playfriends.SharedInstance.friendsPlayer [i].gameObject.transform.Find ("InviteGiftBG").gameObject.SetActive (false);
		}
		parentObj.transform.Find ("InviteGiftBG").gameObject.SetActive (true);
	}



}
