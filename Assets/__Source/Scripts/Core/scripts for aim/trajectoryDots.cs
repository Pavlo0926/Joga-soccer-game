using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class trajectoryDots : MonoBehaviour {
#pragma warning disable CS0649
    [SerializeField]
    private GameObject dotPrefab;
#pragma warning restore CS0649
    public float numberOfDots;
    public static trajectoryDots instance;

    private List<Transform> trajectoryDot;

    //private RaycastHit ray;
    //private Ray r;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {

        numberOfDots = 10;

    }

    public void CastRay()
    {
        trajectoryDot = new List<Transform>();
        for (int i = 0; i < numberOfDots; i++)
        {
            GameObject dot = Instantiate(dotPrefab);
            //dot.transform.localScale *= BallScale;
            dot.transform.position = Vector3.zero;
            // dot.SetActive(false);
            trajectoryDot.Add(dot.transform);
        }
  
 
       
    }

    


    void SetTrajectoryPoints(Vector3 posStart, Vector2 direction)
    {
        float velocity = Mathf.Sqrt((direction.x * direction.x) + (direction.y * direction.y));
        float angle = Mathf.Rad2Deg * (Mathf.Atan2(direction.y, direction.x));
        float fTime = 0;

        fTime += 0.1f;
        foreach (Transform dot in trajectoryDot)
        {
            float dx = velocity * fTime * Mathf.Cos(angle * Mathf.Deg2Rad);
            float dy = velocity * fTime * Mathf.Sin(angle * Mathf.Deg2Rad);
            Vector3 pos = new Vector3(posStart.x + dx, posStart.y + dy, 0);
            dot.position = pos;
            dot.gameObject.SetActive(true);
           // dot.eulerAngles = new Vector3(0, 0, Mathf.Atan2(direction.y, direction.x));
            fTime += 0.1f;
            //arrowPlane.transform.position = new Vector3(transform.position.x, transform.position.y, -0.2f);
            //shootCircle.transform.position = transform.position + new Vector3(0, 0, 0.05f);

            ////calculate rotation
            //Vector3 dir = helperBegin.transform.position - transform.position;
            //////print (dir);

            //float angleOfArrow;
            //angleOfArrow = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            //arrowPlane.transform.localEulerAngles = new Vector3(arrowPlane.transform.eulerAngles.x, arrowPlane.transform.eulerAngles.y, -angleOfArrow + 90);

        }

    }


    // Use this for initialization
  
	
	// Update is called once per frame
	void Update () {




    }
  
}
