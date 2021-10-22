using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class Joga_SearchPlayerDataStore : MonoBehaviour
    {
        public string playerCountry;
        public string playerId;
        public string playerUserName;
        public GameObject AddBtn;
        public GameObject addedtxt;
        public Text playerNameText;
        public Text playerIdTxt;
        public Text playerCountryTxt;

        public static Joga_SearchPlayerDataStore Instance;

        void Start()
        {
            Assign();
        }

        public void Assign()
        {
            playerNameText.text = playerUserName;
            playerIdTxt.text = playerId;
            playerCountryTxt.text = playerCountry;
        }

        public void OnAddFriend()
        {
            Instance = this;
            Joga_NetworkManager.Instance.AddPlayerFriendListRequest(playerId);
            UIController.Instance.Loading_2_panel.SetActive(true);
        }

        public void AddFriendStatus(bool status, string message)
        {
            UIController.Instance.Loading_2_panel.SetActive(false);

            string msg = message;

            if (status)
            {
                msg = "Player successfully added!";
                AddFriend(msg);
            }

            else
            {
                SSTools.ShowMessage(msg, SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
        }

        public void AddFriend(string message)
        {
            SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);

            AddBtn.GetComponent<Button>().interactable = false;
            addedtxt.GetComponent<Text>().text = "Added";
            addedtxt.GetComponent<Text>().color = new Color(0.7843137f, 0.7843137f, 0.7843137f, 0.5019608f);
        }
    }
}