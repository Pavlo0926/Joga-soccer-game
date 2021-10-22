using Jiweman;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FST_JiwemanLinks : MonoBehaviour
{
    public void GoToWebPortal()
    {
        Debug.Log("go to web portal");
        Application.OpenURL(Joga_API.WebPortal);
    }
}
