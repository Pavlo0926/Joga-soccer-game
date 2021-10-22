using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playfriends : MonoBehaviour
{



	public List<GameObject> friendsPlayer = new List<GameObject> ();

	public Scrollbar scrollbar;

	private static Playfriends _instance = null;

	public static Playfriends SharedInstance {
		get {
			// if the instance hasn't been assigned then search for it
			if (_instance == null) {
				_instance = GameObject.FindObjectOfType (typeof(Playfriends)) as Playfriends; 	
			}
			return _instance; 
		}
	}


	// Use this for initialization
	void Start ()
	{

		for (int i = 0; i < transform.childCount; i++) {
			friendsPlayer.Add (transform.GetChild (i).gameObject);
		}

		scrollbar.size = 0f;

	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}




}
