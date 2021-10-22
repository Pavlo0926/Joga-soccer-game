using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[ExecuteAlways]
public class camera_ortho : MonoBehaviour
{
   
   // public SpriteRenderer rink;
    public GameObject rink;
	// Use this for initialization
	void Start () {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        // float targetRatio = rink.size.x / rink.bounds.size.y;
        float targetRatio = rink.GetComponent<Transform>().localScale.x / rink.GetComponent<Transform>().localScale.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = rink.GetComponent<Transform>().localScale.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rink.GetComponent<Transform>().localScale.y / 2 * differenceInSize;
        }
    }

#if UNITY_EDITOR
    private void Update()
    {
        float screenRatio = (float)Screen.width / (float)Screen.height;
        // float targetRatio = rink.size.x / rink.bounds.size.y;
        float targetRatio = rink.GetComponent<Transform>().localScale.x / rink.GetComponent<Transform>().localScale.y;

        if (screenRatio >= targetRatio)
        {
            Camera.main.orthographicSize = rink.GetComponent<Transform>().localScale.y / 2;
        }
        else
        {
            float differenceInSize = targetRatio / screenRatio;
            Camera.main.orthographicSize = rink.GetComponent<Transform>().localScale.y / 2 * differenceInSize;
        }
    }
#endif
}
