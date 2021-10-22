using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class searchfriend : MonoBehaviour
{
    public string playerCountry;
    public string playerId;
    //public float playerGold;
    //public string playerImgURL;
    //public float playerLevel;
    //public float playerMatchLoss;
    //public float playerMatchPlayed;
    //public float playerMatchWin;
    //public string playerNameId;
    //public float playerRp;
    public string playerUserName;

    public Text playerNameText;
    public Text playerCountryText;
    public Image playerImage;
    public Sprite SentImage;
    public GameObject Challengebtn;

    public GameObject[] MainButtonArray;

    // Use this for initialization
    void Start()
    {
        playerNameText.text = "" + playerUserName;
        MainButtonArray[0].SetActive(true);
    }
    public void Button_Challenge()
    {
       // FriendListAssign.insatnce.TiersPanel.SetActive(true);
       // FriendListAssign.insatnce.ScrollerPanelInSelectTier.SetActive(true);
        TiersValuesAssign.instance.opponentFriendID = playerId.ToString();
        Challengebtn.GetComponent<Image>().sprite = SentImage;
    }
}
