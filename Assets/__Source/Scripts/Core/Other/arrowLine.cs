using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class arrowLine : MonoBehaviour
{

    //public Sprite dotSprite;
    public LineRenderer line;
    //public Vector3 arrowEnd;
    public static arrowLine instance;
  //  public GameObject[] dot;
  //  private GameObject helperBegin;
    //private Camera playcam;
    //float angleOfArrow;
  //  Vector3 dir;

    void Awake()
    {
        instance = this;
    }


    // Use this for initialization
    void Start()
    {
        //helperBegin = GameObject.FindGameObjectWithTag("mouseHelperBegin");
        //line = GetComponent<LineRenderer>();
        //for (int k = 0; k < 20; k++)
        //{
            //dot[k] = GameObject.Find("Dot (" + k + ")");         //All points are applied to the corresponding position in the dots array
            //if (dotSprite != null)
            //{                                //If a sprite is applied to dotSprite
            //    dot[k].GetComponent<SpriteRenderer>().sprite = dotSprite;    //All points will have that sprite applied
            //}

        //  playerController.Instance.arrowPlane.transform.position = new Vector3(transform.position.x, transform.position.y, -0.2f);
            //shootCircle.transform.position = transform.position + new Vector3(0, 0, 0.05f);

            //calculate rotation
         //   Vector3 dir = helperBegin .transform.position - transform.position;
            ////print (dir);

           
         

            //line.SetPosition(0, new Vector3(0, 0, 0));
        }
   

    // Update is called once per frame
   void Update()
    {
       
    //   RaycastHit hit;

       
    //if (Physics.Raycast(transform.position, transform.forward, out hit))
    //{
    //    if (hit.collider)
    //    {
    //        //line.SetPosition(1, new Vector3(hit.distance, 0, 0));
    //    }
    //}
    //else
    //{
    //  // line.SetPosition(1, new Vector3(-2f, 0, 0));
    //}
        

    //public void Collider(GameObject dot)
    //{
    //    for (int k = 0; k < 20; k++)
    //    {
    //        if (dot.name == "Dot (" + k + ")")
    //        {
    //            Debug.Log("" + dot.name);
    //            for (int i = k; i < 20; i++)
    //            {

    //                GameObject.Find("Dot (" + i + ")").GetComponent<SpriteRenderer>().enabled = false;
    //                GameObject.Find("dot(" + i + ")").GetComponent<BoxCollider>().enabled = false;

    //            }

    //        }

    //    }
    }

}
