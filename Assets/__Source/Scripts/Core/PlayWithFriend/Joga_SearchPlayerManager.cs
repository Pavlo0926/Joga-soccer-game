using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class Joga_SearchPlayerManager : MonoBehaviour
    {
        public Transform ParentObject;
        public GameObject SearchPlayerPerfab;
        public GameObject Error;
        public GameObject textPanel;
        public GameObject DataPanel;
        public GameObject ClearBtn;
        public InputField SearchInputField;
        public Text playerNameIDText;
        public bool canSwitch = false;
        public bool waitActive = false;
        public GameObject searchPanelSearchbutton;

        private float time;

        public static Joga_SearchPlayerManager Instance;

        void Awake()
        {
            if(!Instance)
                Instance = this;
        }

        void Start()
        {
            CancelSearch();
        }

        private void OnEnable()
        {
            ResetAllPlayerData();
        }

        public void ShowUserIdText()
        {
            playerNameIDText.text = "Your User Name is " + PlayerPrefs.GetString("userName");
        }

        public void AssignChallegePlayerData (){}
        
        public void AssignChallegeAllPlayer(){}

        /// <summary>
        /// Check the status of the searched player/s
        /// </summary>
        public void CheckPlayerSearchStatus(bool status, JSONNode json)
        {
            searchPanelSearchbutton.GetComponent<Button>().interactable = true;
            UIController.Instance.Loading_2_panel.SetActive(false);

            string msg = json["message"].Value;

            if (status)
            {
                if (msg.Contains("Players found"))
                {
                    Debug.Log("Player Data Found");
                    AssignPlayerData(json);
                }

                else
                    SSTools.ShowMessage("Player is already added or no player found", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }

            else
            {
                Debug.Log("Player Data Error");
                SSTools.ShowMessage("Player data not found!", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
        }

        /// <summary>
        /// Assign searched players data
        /// </summary>
        public void AssignPlayerData(JSONNode json)
        {
            int count = json["data"].Count;

            for (int i = 0; i < count; i++)
            {
                GameObject searchItem = Instantiate(SearchPlayerPerfab, Vector3.zero, Quaternion.identity) as GameObject;
                searchItem.name = "SearchPlayer" + i;
                searchItem.transform.SetParent(ParentObject);
                searchItem.transform.localScale = Vector3.one;
                searchItem.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

                Joga_SearchPlayerDataStore searchedPlayer = searchItem.GetComponent<Joga_SearchPlayerDataStore>();

                searchedPlayer.playerUserName = Joga_Data.Parse(json, "userName", i);
                searchedPlayer.playerId = Joga_Data.Parse(json, "_id", i);
                searchedPlayer.playerCountry = Joga_Data.Parse(json, "countryOfRecidence", i);
            }
        }
        
        /// <summary>
        /// On search button event
        /// </summary>
        public void SearchButton()
        {
            Search();
        }

        /// <summary>
        /// Search players by their user name
        /// </summary>
        public void Search()
        {
            if (string.IsNullOrEmpty(SearchInputField.text))
                SSTools.ShowMessage("Please Enter Player's Name", SSTools.Position.bottom, SSTools.Time.twoSecond);

            else
            {
                ResetAllPlayerData();
                searchPanelSearchbutton.GetComponent<Button>().interactable = false;
                UIController.Instance.Loading_2_panel.SetActive(true);

                string userName = SearchInputField.text;
                Joga_NetworkManager.Instance.SearchAllPlayersRequest(userName);
            }
        }

        /// <summary>
        /// Reset player data item
        /// </summary>
        public void ResetAllPlayerData()
        {
            for (int i = 0; i < ParentObject.childCount; i++)
            {
                Destroy(ParentObject.GetChild(i).gameObject);
            }
        }

        public void CancelSearch()
        {
            SearchInputField.text = "";
        }
    }
}