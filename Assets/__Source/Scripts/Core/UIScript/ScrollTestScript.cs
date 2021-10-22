using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(RectTransform))]
public class ScrollTestScript : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField]
	private float m_CenterScale;
	[SerializeField]
	private float m_BorderScale;
	[SerializeField]
	private AnimationCurve m_ScalingCurve;
#pragma warning restore

    //public Text text;
    // Must be from 1 to 0 on y axis and from 0 to 1 on x axis
   // Transform pos;  

    void Start()
    {
        transform.position = GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().position;
    }
    protected void Update()
    {
        float scrollviewCenterPosition = GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().position.x;
        float distanceFromCenter = transform.position.x - scrollviewCenterPosition;
        float ratio = Mathf.Abs(distanceFromCenter / (GetComponentInParent<ScrollRect>().GetComponent<RectTransform>().rect.width * 0.5f));
        float scaleValue = m_BorderScale + (m_CenterScale - m_BorderScale) * m_ScalingCurve.Evaluate(ratio);
        (transform as RectTransform).localScale = Vector3.one * scaleValue;
        if (scaleValue < 0.90)
        {
            GetComponent<Button>().interactable = false;
            GetComponent<Image>().color = new Color(0.594f, 0.594f, 0.594f, 0.9f);                    
           // text.color = new Color(0.594f, 0.594f, 0.594f, 0.9f);            
        }
        else
        {
            GetComponent<Button>().interactable = true;
            GetComponent<Image>().color = new Color(1f, 1f, 1f, 1f);
           // text.color = new Color(1f, 1f, 1f, 1f);
        }
        

    }
}
