using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IngameFormation : MonoBehaviour {

	/// <summary>
	/// In-game formation manager
	/// </summary>

	//we need to know if player-1 or player-2 are changing their formation!
	//so we assign an ID for each one to know who is sending the request.
	// ID 1 = Player-1
	// ID 2 = Player-2
	public static int formationChangeRequestID;

	public GameObject[] availableFormationButtons;	//array containing all formation buttons we are using inisde "newFormationPlane" game object

	public GameObject tickGo;
	public GameObject requestByLabel;

	private int player1Formation;
	private int player2Formation;


	void Start () {

		//fetchFormations ();

	}

	void OnEnable() {

		fetchFormations ();
	}


	/// <summary>
	/// Restore previous formation settings for each player upon formation change
	/// </summary>
	void fetchFormations() {
		
		//get current formation
		player1Formation = PlayerPrefs.GetInt("PlayerFormation");
		player2Formation = PlayerPrefs.GetInt("Player2Formation");

		print ("formationChangeRequestID: " + formationChangeRequestID);
		print ("player1FormationID: " + player1Formation + " | player2FormationID: " + player2Formation);

		if (formationChangeRequestID == 1)
			moveTickToPosition ( availableFormationButtons[player1Formation].transform.position );
		else if (formationChangeRequestID == 2)
			moveTickToPosition ( availableFormationButtons[player2Formation].transform.position );	

	}
		
	
	void FixedUpdate () {

		touchManager ();

		//monitor request label text
		requestByLabel.GetComponent<TextMesh> ().text = "(Player " + formationChangeRequestID + ")";
	}


	private RaycastHit hitInfo;
	private Ray ray;
	void touchManager (){

		//Mouse of touch?
		if(	Input.touches.Length > 0 && Input.touches[0].phase == TouchPhase.Ended)  
			ray = Camera.main.ScreenPointToRay(Input.touches[0].position);
		else if(Input.GetMouseButtonUp(0))
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		else
			return;

		if (Physics.Raycast(ray, out hitInfo)) {
			GameObject objectHit = hitInfo.transform.gameObject;
			switch(objectHit.name) {

			case "BtnFormation-01":								
				moveTickToPosition (objectHit.transform.position);
				saveNewFormationSetting (0);
				break;

			case "BtnFormation-02":								
				moveTickToPosition (objectHit.transform.position);
				saveNewFormationSetting (1);
				break;

			case "BtnFormation-03":								
				moveTickToPosition (objectHit.transform.position);
				saveNewFormationSetting (2);
				break;

			case "BtnFormation-04":								
				moveTickToPosition (objectHit.transform.position);
				saveNewFormationSetting (3);
				break;

			case "BtnFormation-05":								
				moveTickToPosition (objectHit.transform.position);
				saveNewFormationSetting (4);
				break;
			
			}	
		}
	}


	/// <summary>
	/// move tick over the selected formation image
	/// </summary>
	/// <param name="newPos">New position.</param>
	void moveTickToPosition(Vector3 newPos) {

		//unhide tick
		tickGo.GetComponent<Renderer>().enabled = true;

		//move tick over the selected formation image
		tickGo.transform.position = newPos + new Vector3 (0, 0, -1);

	}

	/// <summary>
	/// Saves the new formation setting for the player who requested it
	/// </summary>
	void saveNewFormationSetting(int newFormationID) {

		//save
		if(formationChangeRequestID == 1)
			PlayerPrefs.SetInt("PlayerFormation", newFormationID);
		if(formationChangeRequestID == 2)
			PlayerPrefs.SetInt("Player2Formation", newFormationID);

		//debug
		print("New formation setting has been saved for Player " + formationChangeRequestID + " | Formation ID: " + newFormationID);

	}

}
