using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Jiweman;

public class FbchallengePlayer : MonoBehaviour
{
    public string playerName;
    public string playerNameID;
    public string playerCountry;
    public string playerImageUrl;
    public string playerID;

    public Text playerNameText;
    public Text playerCountryText;
    public Image playerImage;
   
    public GameObject[] MainButtonArray;

    // Use this for initialization
    void Start()
    {
        playerNameText.text = "" + playerName;
        MainButtonArray[0].SetActive(true);
       
    }

    public void Button_Challenge()
    {
      Joga_FriendsManager.Instance.Challenge_sent.SetActive(true);
        // FriendListAssign.insatnce.TiersPanel.SetActive(true);
        // FriendListAssign.insatnce.ScrollerPanelInSelectTier.SetActive(true);
        // TiersValuesAssign.instance.opponentFriendID = playerID.ToString();
    }

    public void Button_Remove()
    {
        // commented --@sud
        // WebServicesHandler.SharedInstance.GetRemoveFriend(playerID.ToString());
    }
}
