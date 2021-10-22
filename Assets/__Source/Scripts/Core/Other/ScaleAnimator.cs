using UnityEngine;
using System.Collections;

public class ScaleAnimator : MonoBehaviour
{
		
	///*************************************************************************///
	/// This function animates the halo object around units when they have the trun.
	///*************************************************************************///

	private float intensity = 0.8f;//1.2f when scale was 1, for this I make scale of object 1.25
	//scale ratio
	private float animSpeed = 0.1f;
	//scale animation speed
	//animation
	private bool animationFlag;
	private float startScaleX;
	private float startScaleY;
	private float endScaleX;
	private float endScaleY;

	void Start ()
	{
		animationFlag = true;
		startScaleX = transform.localScale.x;
		startScaleY = transform.localScale.y;
		endScaleX = startScaleX * intensity;
		endScaleY = startScaleY * intensity;
	}

	void FixedUpdate ()
	{
		if (animationFlag) {
			animationFlag = false;
			StartCoroutine (animatePulse (this.gameObject));
		}
	}

	IEnumerator animatePulse (GameObject _btn)
	{
		// yield return new WaitForSeconds (0.01f);
		yield return new WaitForSeconds (0.1f);
		float t = 0.0f; 
		while (t <= 5.0f) {
			t += Time.deltaTime * 1.5f * animSpeed;
			_btn.transform.localScale = new Vector3 (Mathf.SmoothStep (startScaleX, endScaleX, t),
				Mathf.SmoothStep (startScaleY, endScaleY, t),
				_btn.transform.localScale.z);
			yield return 0;
		}

        //Uncomment bellow for heartbeat effect

        float r = 0.0f;
        if (_btn.transform.localScale.x >= endScaleX)
        {
            while (r <= 1.0f)
            {
                r += Time.deltaTime * 1.5f * animSpeed;
                _btn.transform.localScale = new Vector3(Mathf.SmoothStep(endScaleX, startScaleX, r),
                    Mathf.SmoothStep(endScaleY, startScaleY, r),
                    _btn.transform.localScale.z);
                yield return 0;
            }
        }

        if (_btn.transform.localScale.x <= startScaleX) {
			// yield return new WaitForSeconds (0.01f);
			yield return new WaitForSeconds (2f);
			animationFlag = true;
		}
	}

}