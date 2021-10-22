using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollImage : MonoBehaviour
{

	public float scrollSpeed;
	public float tileSizeZ;

	private Vector3 startPosition;

	public bool isRotateImage;

	public static ScrollImage Instance;

	public Sprite DefaultImage;
	RectTransform myRectTransform;

	void Awake ()
	{
		Instance = this;
	}

	void Start ()
	{
		myRectTransform = GetComponent<RectTransform> ();
		startPosition = transform.localPosition;
	}

	void Update ()
	{
		if (!isRotateImage) {
			float newPosition = Mathf.Repeat (Time.time * scrollSpeed, tileSizeZ);
			myRectTransform.localPosition = startPosition + Vector3.up * newPosition;
		} else {
			this.gameObject.SetActive (false);
			// myRectTransform.localPosition = startPosition;
		}
	}
	
}
