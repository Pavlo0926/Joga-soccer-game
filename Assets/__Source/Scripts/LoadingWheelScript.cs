using UnityEngine;
using System.Collections;

public class LoadingWheelScript : MonoBehaviour {

		private static LoadingWheelScript _instance = null;

		public static LoadingWheelScript SharedInstance {
				get {
						// if the instance hasn't been assigned then search for it
						if (_instance == null) {
								_instance = GameObject.FindObjectOfType (typeof(LoadingWheelScript)) as LoadingWheelScript; 	
						}

						return _instance; 
				}
		}

}
