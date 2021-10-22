using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RejectChallenge : MonoBehaviour
{

    public string playerName;
    public Text Username;
    // Start is called before the first frame update
    private void OnEnable()
    {
        playerName = FireBasePushNotification.opponentName;
        Username.text = playerName;
    }

}
