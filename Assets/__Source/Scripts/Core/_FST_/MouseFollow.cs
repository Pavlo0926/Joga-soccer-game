using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using Lean.Touch;
using FastSkillTeam;

public class MouseFollow : MonoBehaviour
{
    public Transform parentTransform;

    private const float zOffset = -10f;

    Vector3 firstTouchPos, secondTouchPos, secondTouchDeltaPos, firstTouchDeltaPos;
#if UNITY_ANDROID && !UNITY_EDITOR
    private bool isDrag = false;

	float firstTouchDeltaDistance, secondTouchDeltaDistance;
	private float speedX = 0.2f, speedY = 0.2f;
	private float ScaleFactor = 0.05f;
	private float minimumDistance = 0.3f;
	private const float MAXIMUM_DISTANCE = 35f;
	private bool isFirstTouch = true;
	Vector3 tampPosFirst;
	bool ismoveXaxis;
#endif

    [Tooltip("Ignore fingers with StartedOverGui?")]
    public bool IgnoreGuiFingers;

    [Tooltip("Allows you to force rotation with a specific amount of fingers (0 = any)")]
    public int RequiredFingerCount;

    [Tooltip("Does scaling require an object to be selected?")]
    public LeanSelectable RequiredSelectable;

    [Tooltip("If you want the mouse wheel to simulate pinching then set the strength of it here")]
    [Range(-1.0f, 1.0f)]
    public float WheelSensitivity;

    [Tooltip("The camera that will be used to calculate the zoom")]
    public Camera Camera;

    [Tooltip("Should the scaling be performanced relative to the finger center?")]
    public bool Relative;

    [Tooltip("The rotation axis used for non-relative rotations")]
    public Vector3 RotateAxis = Vector3.forward;


#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (RequiredSelectable == null)
            RequiredSelectable = GetComponent<LeanSelectable>();
    }
#endif

    void Start()
    {
        transform.localPosition = Vector3.zero;
    }

    void Update()
    {
#if UNITY_EDITOR || (DEBUG && !UNITY_ANDROID)

        Vector3 tmpPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
        transform.position = new Vector3(tmpPosition.x, tmpPosition.y, zOffset);

#elif UNITY_ANDROID

        if (FST_Gameplay.IsMultiplayer && !FST_DiskPlayerManager.Instance.IsOwner)
            return;

        if (Input.touchCount > 0)
        {
            // If we require a selectable and it isn't selected, cancel scaling
            if (RequiredSelectable != null && RequiredSelectable.IsSelected == false)
                return;

            if (Input.touchCount < 2)
            {
                if (isFirstTouch)
                {

                    // Get the fingers we want to use
                    var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

                    // Calculate the screenDelta value based on these fingers
                    var screenDelta = LeanGesture.GetScreenDelta(fingers);

                    // Perform the translation
                    Translate(screenDelta);

                }
                else
                {

                    // Get the fingers we want to use
                    var fingers2 = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount, RequiredSelectable);

                    // Calculate the screenDelta value based on these fingers
                    var screenDelta = LeanGesture.GetScreenDelta(fingers2);

                    // Perform the translation
                    Translate(screenDelta);

                }
            }
            else
            {
                isFirstTouch = false;

                // Get the fingers we want to use
                var fingers = LeanTouch.GetFingers(IgnoreGuiFingers, RequiredFingerCount);

                // Calculate the scaling values based on these fingers
                var scale = LeanGesture.GetPinchScale(fingers, WheelSensitivity);
                var screenCenter = LeanGesture.GetScreenCenter(fingers);

                // Perform the scaling
                Scale(scale, screenCenter);

                // Calculate the rotation values based on these fingers
                var center = LeanGesture.GetScreenCenter(fingers);
                var degrees = LeanGesture.GetTwistDegrees(fingers);

                // Perform the rotation
                Rotate(center, degrees);
            }
        }
#endif
    }

    public void Rotate(Vector3 center, float degrees)
    {
        if (Relative == true)
        {
            // If camera is null, try and get the main camera, return true if a camera was found
            if (LeanTouch.GetCamera(ref Camera) == true)
            {
                // World position of the reference point
                var worldReferencePoint = Camera.ScreenToWorldPoint(center);

                // Rotate the transform around the world reference point
                transform.RotateAround(worldReferencePoint, Camera.transform.forward, degrees);
            }
        }
        else
        {
            parentTransform.Rotate(0, 0, degrees);
        }
    }

    public void Translate(Vector2 screenDelta)
    {
        // If camera is null, try and get the main camera, return true if a camera was found
        if (LeanTouch.GetCamera(ref Camera) == true)
        {
            // Screen position of the transform
            var screenPosition = Camera.WorldToScreenPoint(transform.localPosition);

            // Add the deltaPosition
            screenPosition += (Vector3)screenDelta;

            // Convert back to world space
            var v = Camera.ScreenToWorldPoint(screenPosition);
            v = Vector3.ClampMagnitude(v, 16);
            Vector3 distance2 = v;
            distance2.z = -10f;
            transform.localPosition = distance2;
        }
    }

    public void Scale(float scale, Vector2 screenCenter)
    {
        if (!Camera)
            return;
        // Make sure the scale is valid
        if (scale > 0.0f)
        {
            if (Relative == true)
            {
                // If camera is null, try and get the main camera, return true if a camera was found
                if (LeanTouch.GetCamera(ref Camera) == true)
                {
                    // Screen position of the transform
                    var screenPosition = Camera.WorldToScreenPoint(transform.position);

                    // Push the screen position away from the reference point based on the scale
                    screenPosition.x = screenCenter.x + (screenPosition.x - screenCenter.x) * scale;
                    screenPosition.y = screenCenter.y + (screenPosition.y - screenCenter.y) * scale;

                    // Convert back to world space
                    transform.localPosition = Camera.ScreenToWorldPoint(screenPosition);

                    Vector3 v = new Vector3(transform.localPosition.x * scale, 0, 0);
                    v = Vector3.ClampMagnitude(v, 12);
                    Vector3 distance2 = v;
                    distance2.z = zOffset;
                    transform.localPosition = distance2;
                }
            }
            else
            {

                var screenPosition = Camera.WorldToScreenPoint(transform.localPosition);

                screenPosition.y = screenCenter.y + (screenPosition.y - screenCenter.y) * scale;
                transform.localPosition = Camera.ScreenToWorldPoint(screenPosition);

                Vector3 v = new Vector3(transform.localPosition.x * scale, transform.localPosition.y, 0);

                v = Vector3.ClampMagnitude(v, 12);
                Vector3 distance2 = v;
                distance2.z = zOffset;
                transform.localPosition = distance2;
            }
        }
    }

    public void SetTurn(Transform t)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        isFirstTouch = true;
#endif
        parentTransform.position = t.position;
        parentTransform.rotation = Quaternion.identity;
        transform.localPosition = new Vector3(0, 0, -10f);
    }
}



