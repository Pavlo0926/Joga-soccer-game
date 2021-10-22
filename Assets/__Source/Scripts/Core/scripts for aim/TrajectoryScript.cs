using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrajectoryScript : MonoBehaviour {
    public int numberOfDots;
    public static TrajectoryScript instance;


  public GameObject[] dot;                    //The array of points that make up the trajectory
 
    private void Awake()
    {
        instance = this;
    }
    void Start () {

      
    }

 public void collided(GameObject dotts){

     for (int k = 0; k < numberOfDots; k++) {
           if (dotts.name == "dot (" + k + ")") {
               
               for (int i = k + 1; i < numberOfDots; i++) {
                   
                   dot[i].gameObject.GetComponent<MeshRenderer> ().enabled = false;
                    dot[i].gameObject.GetComponent<BoxCollider>().enabled = false;
               }

         }

     }
  }
  public void uncollided(GameObject dotts){
        for (int k = 0; k < numberOfDots; k++) {
           if (dotts.name == "dot (" + k + ")") {

              //for (int i = k-1; i > 0; i--) {
                
                   //if (dot [i].gameObject.GetComponent<SpriteRenderer> ().enabled == false) {
                    //    Debug.Log ("nigggssss");
                    //   return;
                    //}
             // }

             if (dot [k].gameObject.GetComponent<Renderer> ().enabled == false && dot[k].gameObject.GetComponent<Collider>().enabled == false) {
                    for (int i = k; i < numberOfDots; i++) {
                      
                       dot[i].gameObject.GetComponent<MeshRenderer> ().enabled = true;
                        dot[i].gameObject.GetComponent<BoxCollider>().enabled = true;

                   }

             }
          }

     }
  }
}


