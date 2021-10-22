using UnityEngine;
using System.Collections;

public class TickMisson : MonoBehaviour {

		public int Count;
		private int endCount;
		public GameObject[] missionImage;





		private static TickMisson _instance = null;

		public static TickMisson SharedInstance {
				get {
						// if the instance hasn't been assigned then search for it
						if (_instance == null) {
								_instance = GameObject.FindObjectOfType (typeof(TickMisson)) as TickMisson; 	
						}

						return _instance; 
				}
				set {

						_instance = value;

				}	
		}




	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
	}



		public void CheckMissionComplete ()
		{

				if (GameStates.currentState == GAME_STATE.ENDMISSION) {
						int i;


						for (i = 1; i < 4; i++) {
								if (PlayerPrefs.GetInt ("mission" + i) == 1) {
										endCount = i;

										missionImage [endCount].transform.localScale = Vector3.one;


										//showCompletedMission (missionCount);
								} else {
										missionImage [i].transform.localScale = Vector3.zero;
								}
						}
				}


		}

}
