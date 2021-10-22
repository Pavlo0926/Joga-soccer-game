using UnityEngine;
using System.Collections;

public class playerColliderManager : MonoBehaviour
{

    ///*************************************************************************///
    /// Optional controller for collision of player units vs other items in the scene like ball or opponent units
    ///*************************************************************************///

    public GameObject TopCircleGO;

    Rigidbody rb = null;
    Rigidbody topCircleRb = null;

    //super quick optimisation here ->FST

    void OnCollisionEnter(Collision other)
    {
        if (!rb)
            rb = transform.GetComponent<Rigidbody>();
        if (!topCircleRb)
            topCircleRb = TopCircleGO.GetComponent<Rigidbody>();

        switch (other.gameObject.tag)
        {
            case "Border":
                topCircleRb.AddRelativeTorque(new Vector3(0, rb.velocity.magnitude / 2, 0), ForceMode.Impulse);
                break;
            case "Opponent":
                topCircleRb.AddRelativeTorque(new Vector3(0, rb.velocity.magnitude / 2, 0), ForceMode.Impulse);
                break;
            case "ball":
                topCircleRb.AddRelativeTorque(new Vector3(0, rb.velocity.magnitude / 2, 0), ForceMode.Impulse);
                break;
            case "Player":
                topCircleRb.AddRelativeTorque(new Vector3(0, rb.velocity.magnitude / 2, 0), ForceMode.Impulse);
                break;
            case "Player_2":
                topCircleRb.AddRelativeTorque(new Vector3(0, rb.velocity.magnitude / 2, 0), ForceMode.Impulse);
                break;
        }
    }
}