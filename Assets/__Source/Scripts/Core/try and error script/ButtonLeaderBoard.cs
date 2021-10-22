using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jiweman;

public class ButtonLeaderBoard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (GetComponent<Button>() != null)
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                Joga_LeaderBoard.Instance.ButtonEvent(gameObject.name);
                AssignFormationData.Instance.ButtonCallEvent(gameObject.name);
            });
        }
    }
}