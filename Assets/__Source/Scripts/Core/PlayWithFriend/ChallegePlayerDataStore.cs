using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class ChallegePlayerDataStore : MonoBehaviour
    {
        //set this when clicked on
        public static ChallegePlayerDataStore Instance;

        public string playerName;
        public string userId;
        public string playerGender;
        public string playerImageUrl;

        public Text playerNameText;
        public Text userIdTxt;
        public Text playerGenderText;
        public Image playerImage;
        public Sprite SentImage;
        public GameObject Challengebtn;
        public GameObject Remove_button;
        public Text remove_text;
        public GameObject[] MainButtonArray;
      
        public bool isOnline;
        public Image OnlineIndicatorImage;

        Sprite originalImage;

        void Start()
        {
            originalImage = Challengebtn.GetComponent<Image>().sprite;

            playerNameText.text = playerName;
            userIdTxt.text = "id: " + userId;
            playerGenderText.text = playerGender;
            
            MainButtonArray[0].SetActive(true);
        }

        public void SetFriendData()
        {
            
        }

        public void Button_Challenge()
        {
            Instance = this;

            //start creating room right now!
           /// FST_MPConnection.Instance.TryPlayWithFriends(new string[] { Joga_Data.PlayerID, userId });//MAIN!!!

            Debug.Log("OnClick Button Challenge > challenged user id = " + userId);
            UIController.Instance.CantplayPopup.GetComponent<RejectChallenge>().playerName = playerName;
            UIController.Instance.ChallengeAcceptedPopup.GetComponent<AcceptChallengePopupValueAssign>().UserName = playerName;
            UIController.Instance.ChallengeAcceptedPopup.GetComponent<AcceptChallengePopupValueAssign>().challengeId = userId;
            //if (isOnline)
            //     SendChallengeOnlineTo(userId);
            //   else
            Joga_NetworkManager.Instance.SendChallengeTo(userId);
        }

        /// <summary>
        /// this will run through chat sytem and request via pun alone, it is only for use for when a challenge is to be sent to an online player
        /// </summary>
        private void SendChallengeOnlineTo(string id)
        {
            FST_MainChat.Instance.SendPWFRequestTo(id);

            ChallengeSentActive();

            Debug.Log("PWF SENDER = " + Joga_Data.PlayerID + " PWF RECIEVER = " + userId);

            UIController.Instance.Loading_2_panel.SetActive(false);
            SSTools.ShowMessage("Challenge Sent", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        public void ChallengeSent(bool status, string message)
        {
            if (status)
            {
                ChallengeSentActive();
                FST_MPDebug.Log("challenge sent!");
                Debug.Log("challenge sent!");
            }
            else Debug.LogWarning("Challenge NOT sent!");

            UIController.Instance.Loading_2_panel.SetActive(false);
            SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        public void OnDelete()
        {
            Instance = this;
            Joga_NetworkManager.Instance.DeletePlayerFriendListRequest(userId);
            UIController.Instance.Loading_2_panel.SetActive(true);
        }

        public void CheckDeletedFriendStatus(bool status, string message)
        {
            Debug.Log(message);

            if (status)
            {
                Remove_button.GetComponent<Button>().interactable = false;
                remove_text.text = "Removed";
                remove_text.color = new Color(255f, 255f, 255f, 150f);
                Destroy(gameObject);
            }

            UIController.Instance.Loading_2_panel.SetActive(false);
            SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        private void ChallengeSentActive()
        {
            UIController.Instance.IsChallengeActive = true;
            if (Challengebtn)
            {
                Challengebtn.GetComponent<Image>().sprite = SentImage;
                Challengebtn.GetComponent<Button>().interactable = false;
            }
            UIController.Instance.ChallengeHasSent.SetActive(true);

            Joga_FriendsManager.Instance.Challenge_sent.SetActive(true);
        }

        public void ChallengeSentDeActive()
        {
            UIController.Instance.IsChallengeActive = false;
            if (Challengebtn)
            {
                Challengebtn.GetComponent<Image>().sprite = originalImage;
                Challengebtn.GetComponent<Button>().interactable = true;
            }
            Joga_FriendsManager.Instance.Challenge_sent.SetActive(false);
        }
    }
}