using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;



public class PlayerDistanceCalculator : MonoBehaviour
{


    public float distanceTravelled;
    public Vector3 previousPosition;
   // public Rigidbody Ball;
    public float angularVelocity;
    public Text BallAngularVel;

    public void Start()
    {
        previousPosition = transform.position;
        
    }

   public void Update()
    {
        // // distanceTravelled +=(transform.position - previousPosition).magnitude;
        // //Vector3.Distance(transform.position, previousPosition);

        // distanceTravelled += Vector3.Distance(previousPosition, transform.position);
        // //distanceTravelled += Vector3.Distance(transform.position, previousPosition);
        // previousPosition = transform.position;
        // angularVelocity = BallManager.instace.ballRigidBody.angularVelocity.magnitude;
        // BallAngularVel.text = angularVelocity.ToString();


    }


}