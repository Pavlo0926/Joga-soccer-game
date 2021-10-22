using UnityEngine;

namespace Lean.Touch
{
	// This script will zoom the main camera based on finger gestures
	public class LeanSideCamera3D : MonoBehaviour
	{
		[Tooltip("The camera we will be moving")]
		public Camera Camera;

		[Tooltip("The minimum field of view angle we want to zoom to")]
		public float FovMin = 10.0f;
		
		[Tooltip("The maximum field of view angle we want to zoom to")]
		public float FovMax = 60.0f;

		[Tooltip("The distance from the camera the world positions will be sampled from (e.g. if your perspective camera is 100 units away from the game plane, set this to 100)")]
		public float Distance = 10.0f;
		
		protected virtual void LateUpdate()
		{
			// If camera is null, try and get the main camera, return true if a camera was found
			if (LeanTouch.GetCamera(ref Camera) == true)
			{
				// Get the world delta of all the fingers
				var worldDelta = LeanGesture.GetWorldDelta(Distance);
				
				// Subtract the delta to the position
				Camera.transform.position -= worldDelta;
				
				// Store the old FOV in a temp variable
				var fieldOfView = Camera.fieldOfView;

				// Scale the FOV based on the pinch scale
				fieldOfView *= LeanGesture.GetPinchRatio();
					
				// Clamp the FOV to out min/max values
				fieldOfView = Mathf.Clamp(fieldOfView, FovMin, FovMax);

				// Set the new FOV
				Camera.fieldOfView = fieldOfView;
			}
		}
	}
}