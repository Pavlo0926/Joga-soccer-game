using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class Joga_FriendsManager : MonoBehaviour
    {
        public Transform ParentObject;
        public Transform FBParentObject;
        public Transform SearchParentObject;

        public GameObject ChallegeFriendPerfab;
        public GameObject EditButton;

        public GameObject Challenge_sent;               //Pop up which activate when challenge is sent or received
        public GameObject SearchFriendPerfab;          //public GameObject FBfriendPerfab;
        public GameObject Error;
        public GameObject ClearButton;
        public GameObject DataPanel;
        public GameObject FBFriendsChallenge_panel;
        public GameObject SearchFriend_panel;
        public GameObject TextPanel;
        public InputField SearchFriendInput;
        public GameObject SearchButton;
        public GameObject addFriendButton;
        public GameObject TiersPanel;
        public GameObject ScrollerPanelInSelectTier;
        public GameObject SendSuccessPanelInSelectTier;
        public bool isEditMode;
        private bool isSearch;
        public Text NextChallenegeTimer;

        public static Joga_FriendsManager Instance;

        private readonly FST_Timer.Handle challengeTimer = new FST_Timer.Handle();

        void Awake()
        {
            Instance = this;
        }

        void Start()
        {
            Challenge_sent.SetActive(false);
            SearchFriend_panel.SetActive(false);
            SearchFriendInput.enabled = false;
            SearchButton.SetActive(false);
            TextPanel.SetActive(false);
            SearchFriendInput.text = "";
        }

        public float ChallengeFriendtime = 0;
        public void Update()
        {
            if (SearchButton.GetComponent<Button>().interactable == false)
            {
                ChallengeFriendtime += Time.deltaTime;
                //Debug.Log ("time" + ChallengeFriendtime);
                if (ChallengeFriendtime >= 1)
                {
                    ChallengeFriendtime = 0;
                    SearchButton.GetComponent<Button>().interactable = true;
                }
            }

            if (UIController.Instance.IsChallengeActive)
            {
                if (!challengeTimer.Active)
                {
                    FST_Timer.In(60, delegate ()
                    {
                        ChallegePlayerDataStore.Instance.ChallengeSentDeActive();
                        UIController.Instance.IsChallengeActive = false;
                    }, challengeTimer);
                }
                else NextChallenegeTimer.text = ((int)challengeTimer.DurationLeft).ToString() + " sec";
            }

            if (SearchFriendInput.text != "")
            {
                ClearButton.SetActive(true);
            }

            else
            {
                ClearButton.SetActive(false);
            }
        }

        public void GetFriends()
        {
            Debug.Log("Get Friends");
            Joga_NetworkManager.Instance.GetAllFriendsRequest();
            UIController.Instance.Loading_2_panel.SetActive(true);

            isSearch = false;
        }

        public void CheckFriendStatus(bool status, JSONNode json)
        {
            if (UIController.Instance.IsChallengeActive /*|| UIController.Instance.IsChallengeRecieved*/)
                Challenge_sent.SetActive(true);
            else
                Challenge_sent.SetActive(false);

            string msg = json["message"].Value;


            if (status)
            {
                TextPanel.SetActive(false);
                addFriendButton.SetActive(true);
                SearchFriendInput.enabled = true;
                SearchButton.SetActive(true);
                DataPanel.SetActive(true);

                AssignFriendsData(json);

                SSTools.ShowMessage(msg, SSTools.Position.bottom, SSTools.Time.threeSecond);
            }

            else
            {
                if (isSearch)
                {
                    ResetFriendList();
                    SSTools.ShowMessage(msg, SSTools.Position.bottom, SSTools.Time.threeSecond);
                }

                else
                {
                    DataPanel.SetActive(false);
                    addFriendButton.SetActive(false);
                    TextPanel.SetActive(true);
                }
            }

            UIController.Instance.Loading_2_panel.SetActive(false);
        }

        public void AssignFriendsData(JSONNode json)
        {
            ResetFriendList();

            int count = json["data"].Count;
            Debug.Log("Friends Count = " + count);

            string[] pIDs = new string[count];

            for (int i = 0; i < count; i++)
            {
                GameObject friendItem = Instantiate(ChallegeFriendPerfab, Vector3.zero, Quaternion.identity) as GameObject;
                friendItem.name = "Friend" + i;
                friendItem.transform.SetParent(ParentObject);
                friendItem.transform.localScale = Vector3.one;
                friendItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                ChallegePlayerDataStore playerData = friendItem.GetComponent<ChallegePlayerDataStore>();

                playerData.playerName = Joga_Data.Parse(json, "FriendUserName", i);
                playerData.userId = Joga_Data.Parse(json, "userId", i);

                pIDs[i] = playerData.userId;

                Debug.Log("FRIEND USER ID " + i + " = " + playerData.userId);
                playerData.playerGender = Joga_Data.Parse(json, "gender", i);
            }
            Debug.Log("Trigger PUN Friends list update");// this will be picked up by FST_FriendsManager for sorting
            FST_FriendsManager.Init(pIDs);
        }

        public void OnSearchFriend()
        {
            FBFriendsChallenge_panel.SetActive(false);
            SearchFriendsData();
        }

        public void SearchFriendsData()
        {
            if (SearchFriendInput.text == "")
            {         
                SSTools.ShowMessage("Please Enter friends name", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            else
            {
                ResetAllSearchData();
                SearchButton.GetComponent<Button>().interactable = false;
                string userName = SearchFriendInput.text;
                Joga_NetworkManager.Instance.SearchAllFriendsRequest(userName);

                UIController.Instance.Loading_2_panel.SetActive(true);
                isSearch = true;
            }
        }

        public void ResetFriendList()
        {
            isEditMode = false;

            for (int i = 0; i < ParentObject.childCount; i++)
                Destroy(ParentObject.GetChild(i).gameObject);
        }

        public void ResetAllSearchData()
        {
            isEditMode = false;
            
            for (int i = 0; i < SearchParentObject.childCount; i++)
                Destroy(SearchParentObject.GetChild(i).gameObject);
        }

        private void OnDisable()
        {
            SearchFriendInput.text = "";
        }

        public void Button_Edit()
        {

            if (!isEditMode)
            {
                isEditMode = true;
                EditButton.GetComponent<Image>().color = new Color(255f, 0, 0, 255f);

                for (int i = 0; i < ParentObject.childCount; i++)
                {
                    ParentObject.GetChild(i).GetComponent<ChallegePlayerDataStore>().MainButtonArray[1].SetActive(true);
                    ParentObject.GetChild(i).GetComponent<ChallegePlayerDataStore>().MainButtonArray[0].SetActive(false);
                }
            }
            else
            {
                isEditMode = false;
                EditButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);
                GameManager.Instance.MainMenuEvents("playfriend");

                //   for (int i = 0; i < ParentObject.childCount; i++)
                //   {
                //         ParentObject.GetChild(i).GetComponent<ChallegePlayerDataStore>().MainButtonArray[1].SetActive(false);
                //         ParentObject.GetChild(i).GetComponent<ChallegePlayerDataStore>().MainButtonArray[0].SetActive(true);
                //   }

            }

        }

        public void CancleSearch()
        {
            ResetFriendList();

            SearchFriendInput.GetComponent<InputField>().text = "";
            DataPanel.SetActive(true);
            SearchFriend_panel.SetActive(false);

            GetFriends();
        }



        // public void FBbutton()
        // {
        //     DataPanel.SetActive(false);
        //     SearchFriend_panel.SetActive(false);
        //     Challenge_sent.SetActive(false);

        //     ResetAllFBData();
        //     FBholder.SharedInstance.GetFriendsPlayingThisGame();

        // }
        // public void ResetAllFBData()
        // {
        //     isEditMode = false;
        //     FBholder.SharedInstance.FBFriends.Clear();
        //     for (int i = 0; i < FriendListAssign.insatnce.FBParentObject.childCount; i++)
        //     {
        //         Destroy(FBParentObject.GetChild(i).gameObject);
        //     }
        // }
    }
}
