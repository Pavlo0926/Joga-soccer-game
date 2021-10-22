using UnityEngine;
using System.Collections;

public class MaskScriptChildren : MonoBehaviour {

    public GameObject playerObjecctFind;


    void Start(){
		
       
	}

	void OnTriggerStay(Collider cl){
		if (cl.GetComponent<Collider> ().isTrigger == true ) {

          
            //TrajectoryScript.instance.collided(gameObject);

		}
	}
	void OnTriggerExit(Collider cl){
		if (cl.GetComponent<Collider> ().isTrigger == false) {

			TrajectoryScript.instance.uncollided (gameObject);

		}
	}
}
