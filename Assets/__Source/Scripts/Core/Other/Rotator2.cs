using UnityEngine;
using System.Collections;

public class Rotator2 : MonoBehaviour
{

	public int dir = 1;
	public int speed = 40;
	
	// Update is called once per frame
	void FixedUpdate ()
	{
		transform.rotation = Quaternion.Euler (0, 180, Time.time * speed * dir);
		
	}
}
