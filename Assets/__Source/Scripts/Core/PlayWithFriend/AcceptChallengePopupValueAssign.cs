using FastSkillTeam;
using UnityEngine;
using UnityEngine.UI;
using Jiweman;

public class AcceptChallengePopupValueAssign : MonoBehaviour
{

    public Text PlayerNameText;

    public string Country;

    public string UserName;
    public string challengeId;


    private static readonly FST_Timer.Handle hideTimer = new FST_Timer.Handle();

    void OnEnable()
    {
        PlayerNameText.text = UserName;
    }
    private void Update()
    {
        if (UIController.Instance.ChallengeAcceptedPopup.activeInHierarchy)
        {
            if (!hideTimer.Active)
            {
                FST_Timer.In(60, delegate ()
                {
                    UIController.Instance.ChallengeAcceptedPopup.SetActive(false);
                }, hideTimer);
                hideTimer.CancelOnLoad = true;
            }

            int t = (int)hideTimer.DurationLeft;

            UIController.Instance.Challtime.text = t.ToString();
            UIController.Instance.acctime.text = t.ToString();
        }
    }

    public void RejectChallenge()
    {
        Joga_NetworkManager.Instance.SendChallengeReplyTo(challengeId, Joga_API.ChallengeStatus.declined);

        ChallegePlayerDataStore.Instance.ChallengeSentDeActive();
        UIController.Instance.ChallengeAcceptedPopup.SetActive(false);
        UIController.Instance.IsChallengeActive = false;
    }

    public void PlayNow()
    {
        FST_MPDebug.Log("NOTE: AcceptChallengePopupValueAssign > PlayNow() was called");
        Debug.Log("NOTE: AcceptChallengePopupValueAssign > PlayNow() was called");

        FST_SettingsManager.MatchType = 5;
        FireBasePushNotification.IsPlayWithfriend = true;
        UIController.Instance.ChallengeAcceptedPopup.SetActive(false);
        Joga_NetworkManager.Instance.SendChallengeReplyTo(challengeId, Joga_API.ChallengeStatus.acceptedByChallenger);
        FST_MPConnection.Instance.TryPlayWithFriends(new string[] { Joga_Data.PlayerID, challengeId });
        FST_MainChat.Instance.StartPWFChallenge();
    }
}
