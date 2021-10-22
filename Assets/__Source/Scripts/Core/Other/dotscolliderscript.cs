using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dotscolliderscript : MonoBehaviour
{
   // public GameObject playerObjectfind;
   // public GameObject[] dots;
   // public Vector3 previous_position;
    public float aim;
    // Use this for initialization
    void Start()
    {
       // playerObjectfind = gameObject;
        //dots = GameObject.FindGameObjectsWithTag("dots");
        aim = PlayerController.Instance.mDisc_aim;
      //  previous_position = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
       

    }
    public void OnCollisionEnter(Collision collision)
    {
        gameObject.GetComponent<BoxCollider>().enabled = false;
        gameObject.SetActive(false);
    }
    public void OnCollisionExit(Collision collision)
    {
        gameObject.GetComponent<BoxCollider>().enabled = true;
        gameObject.SetActive(true);
    }
}
