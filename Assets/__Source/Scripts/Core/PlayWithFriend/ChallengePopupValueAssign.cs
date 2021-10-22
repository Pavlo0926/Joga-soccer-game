using FastSkillTeam;
using UnityEngine;
using UnityEngine.UI;
using Jiweman;
/// <summary>
/// THIS IS THE POPUP THAT GETS SHOWN TO RECEIVER!!!!!!!!!!!!!!!!!!!!!!!!!!!
/// </summary>
public class ChallengePopupValueAssign : MonoBehaviour
{
    public Text PlayerNameText;
    public string UserName;
    public static ChallengePopupValueAssign instance;
    private static readonly FST_Timer.Handle hideTimer = new FST_Timer.Handle();
    void Awake()
    {
        instance = this;
    }
    private void Update()
    {
        if (UIController.Instance.ReceiveChallengePopup.activeInHierarchy)
        {
            if (!hideTimer.Active)
            {
                FST_Timer.In(60, delegate ()
                {
                    UIController.Instance.ReceiveChallengePopup.SetActive(false);
                }, hideTimer);
                hideTimer.CancelOnLoad = true;
            }

            int t = (int)hideTimer.DurationLeft;

            UIController.Instance.Challtime.text = t.ToString();
            UIController.Instance.acctime.text = t.ToString();
        }
    }

    private void OnDisable()
    {
        hideTimer.Cancel();
    }

    void OnEnable()
    {
        UserName = FireBasePushNotification.opponentName;
        PlayerNameText.text = UserName;
    }

    public void RejectChallenge()
    {
        FST_MPDebug.Log("RejectChallenge()");
        Debug.Log("RejectChallenge()");

        UIController.Instance.ShowReceiveChallengePopup(false);
        UIController.Instance.IsChallengeRecieved = false;

        //send the challenger a reply telling them we do not want to play!
        FST_MainChat.Instance.DeclinePWFRequest();

        Joga_NetworkManager.Instance.SendChallengeReplyTo(FireBasePushNotification.opponentId, Joga_API.ChallengeStatus.declined);
    }

    public void PlayNow()
    {
        FST_MPDebug.Log("PlayNow()");
        Debug.Log("PlayNow()");

        FST_SettingsManager.MatchType = 5;//this will also set FST_Gameplay.IsPWF and firebase pwf true
        UIController.Instance.ShowReceiveChallengePopup(false);

        //send the challenger a reply telling them we want to play!
        FST_MainChat.Instance.AcceptPWFRequest();

        Joga_NetworkManager.Instance.SendChallengeReplyTo(FireBasePushNotification.opponentId, Joga_API.ChallengeStatus.accepted);
       // Debug.Log("Opponent ID and Name" + FireBasePushNotification.opponentId + " : " + FireBasePushNotification.opponentName);	
    }
}
