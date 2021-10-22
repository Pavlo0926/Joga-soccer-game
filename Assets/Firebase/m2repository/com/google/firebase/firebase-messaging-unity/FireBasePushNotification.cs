using System.Collections;
using UnityEngine;
using Jiweman;
using UnityEngine.SceneManagement;

public class FireBasePushNotification : MonoBehaviour
{
    public static FireBasePushNotification current;
    public static string opponentId;
    public static string opponentName;
    public static bool IsPlayWithfriend { get; set; } = false;

    public void Awake()
    {
        //   Debug.Log("started");
        if (current != null && current != this)
        {
            Destroy(gameObject);
        }
        else
        {
            // below line not needed. this script is a child of a donotdestroy anyway!!! ->FST
            //   DontDestroyOnLoad (gameObject);
            current = this;
        }
    }
    public void Start()
    {
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
    }

    public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        //   Debug.Log("Received Registration Token: " + token.Token);
        Joga_Data.DeviceToken = token.Token;
    }

    public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        StartCoroutine(WaitForID(sender, e));
    }


    IEnumerator WaitForID(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
       // FST_MainChatInput.Instance?.Initialize();//open chat now for debug purposes!

        FST_MPDebug.Log("WaitForID()");
        Debug.Log("WaitForID()");

        FST_Gameplay.IsPWF = true;

        opponentId = e.Message.Data["userId"];
        opponentName = e.Message.Data["userName"];

        // FST_MPDebug.Log("fb opp:" + opponentId + ", me:" + Joga_Data.PlayerID);
        // Debug.Log("fb opp:" + opponentId + ", me:" + Joga_Data.PlayerID);

        FST_MPDebug.Log("Received a new message from: " + opponentId);
        Debug.Log("Received a new message from: " + opponentId);

        while (string.IsNullOrEmpty(Joga_Data.PlayerID) || !FST_MainChat.Connected)
        {
            if (UIController.Instance && !UIController.Instance.Loading_2_panel.activeInHierarchy)
                UIController.Instance.Loading_2_panel.SetActive(true);

            yield return 0;
        }
        FST_MainChat.Instance.LogPWFRequest(opponentId, Joga_Data.PlayerID);
        UIController.Instance.Loading_2_panel.SetActive(false);

        if (e.Message.Data["message"].Equals("challenge"))
        {
            FST_MPDebug.Log("NOTE: is challenged!!");
            Debug.Log("NOTE: is challenged!!");
         //   FST_MainChat.blockChallenge = true;
            UIController.Instance.ReceiveChallengePopup.SetActive(true);
        }

        if (e.Message.Data["message"].Equals("accepted"))
        {
           // UIController.Instance.ChallengeAcceptedPopup.SetActive(true);
          //  UIController.Instance.IsChallengeActive = true;

            FST_MPDebug.Log("NOTE: is challenge Accepted!!");
            Debug.Log("NOTE: is challenge Accepted!!");
        }

        if (e.Message.Data["message"].Equals("acceptedByChallenger"))
        {
            FST_MPDebug.Log("NOTE: is challenge Accepted By Challenger!!");
            Debug.Log("NOTE: is challenge Accepted By Challenger!!");
        }

        if (e.Message.Data["message"].Equals("declined"))
        {
        //    UIController.Instance.CantplayPopup.GetComponent<RejectChallenge>().playerName = opponentName;
         //   UIController.Instance.CantplayPopup.SetActive(true);
            IsPlayWithfriend = false;

            FST_MPDebug.Log("NOTE: is challenge DECLINED!!");
            Debug.Log("NOTE: is challenge DECLINED!!");

            UIController.Instance.IsChallengeActive = false;

            if (GameStates.currentState != GAME_STATE.MAIN_MENU)
            {
                // Add test here
                FST_MPConnection.Instance.LeaveRoom(true, true);
            }
        }
    }
}