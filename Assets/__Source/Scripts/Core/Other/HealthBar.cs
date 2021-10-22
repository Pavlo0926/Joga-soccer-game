using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
	#region PRIVATE_VARIABLES

	private Vector2 positionCorrection = new Vector2 (0, 100);

	#endregion

	#region PUBLIC_REFERENCES

	public RectTransform targetCanvas;
	public RectTransform healthBar;
	public Transform objectToFollow;

	#endregion

	#region UNITY_CALLBACKS

	void Update ()
	{
		RepositionHealthBar ();
	}

	#endregion

	#region PRIVATE_METHODS

	private void RepositionHealthBar ()
	{
		Vector2 ViewportPosition = Camera.main.WorldToViewportPoint (objectToFollow.position);
		Vector2 WorldObject_ScreenPosition = new Vector2 (
			                                     ((ViewportPosition.x * targetCanvas.sizeDelta.x) - (targetCanvas.sizeDelta.x * 0.5f)),
			                                     ((ViewportPosition.y * targetCanvas.sizeDelta.y) - (targetCanvas.sizeDelta.y * 0.5f)) + 6f);
		//now you can set the position of the ui element
		healthBar.anchoredPosition = WorldObject_ScreenPosition;
	}

	#endregion
}