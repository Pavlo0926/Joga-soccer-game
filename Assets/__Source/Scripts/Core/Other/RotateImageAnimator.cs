using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateImageAnimator : MonoBehaviour
{
	public bool isForward;
	public float speed;
	Vector3 rotationEuler;

	void Update ()
	{
		if (isForward) {
			rotationEuler += Vector3.forward * speed * Time.deltaTime; //increment 30 degrees every second
		} else {
			rotationEuler += Vector3.back * speed * Time.deltaTime; //increment 30 degrees every second
		}
		transform.rotation = Quaternion.Euler (rotationEuler);
	}
}
