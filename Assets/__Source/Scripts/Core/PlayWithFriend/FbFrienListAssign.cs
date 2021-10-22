using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FbFrienListAssign : MonoBehaviour
{

    //public string playerName;
    //public string playerNameID;
    //public string playerCountry;
    //public string playerImageUrl;
    //public string playerID;

    //public Text playerNameText;
    //public Text playerCountryText;
    //public Image playerImage;

   
    public GameObject FBfriendPerfab;
    

    public bool isEditMode;

   // private GameObject[] MainButtonArray;

    public static FbFrienListAssign insatnce;


    void Awake()
    {
        insatnce = this;
    }
    //void Start()
    //{
    //    playerNameText.text = "" + playerName;
    //    MainButtonArray[0].SetActive(true);
    //}

    //public void AssignFbFriendData()
    //{

    //    for (int i = 0; i < FBholder.SharedInstance.FBFriends.Count; i++)
    //    {
    //        GameObject FF = Instantiate(FBfriendPerfab, Vector3.zero, Quaternion.identity) as GameObject;
    //        Debug.Log("created" + FBfriendPerfab.gameObject.GetComponent<Transform>().position);
    //        FF.name = "Formation" + i;
    //        FF.transform.parent = FriendListAssign.insatnce.FBParentObject ;
    //        FF.transform.localScale = Vector3.one;
    //        FF.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;


    //        /*gm.GetComponent <ChallegePlayerDataStore> ().playerCountry = WebServicesHandler.SharedInstance.friendList [i].country;
    //        gm.GetComponent <ChallegePlayerDataStore> ().playerID = WebServicesHandler.SharedInstance.friendList [i].friendID;
    //        gm.GetComponent <ChallegePlayerDataStore> ().playerImageUrl = WebServicesHandler.SharedInstance.friendList [i].imgUrl;
    //        gm.GetComponent <ChallegePlayerDataStore> ().playerName = WebServicesHandler.SharedInstance.friendList [i].name;
    //        gm.GetComponent <ChallegePlayerDataStore> ().playerNameID = WebServicesHandler.SharedInstance.friendList [i].nameId;*/

    //        // FF.GetComponent<FbchallengePlayer>().playerCountry = "India";
    //        FF.GetComponent<FbchallengePlayer>().playerID = "PlayerId_1123";
    //        FF.GetComponent<FbchallengePlayer>().playerImageUrl = "";
    //        FF.GetComponent<FbchallengePlayer>().playerName = FBholder.SharedInstance.FBFriends[i].userName;
    //        // FF.GetComponent<FbchallengePlayer>().playerNameID = FBholder.SharedInstance.FBFriends[i].userId;

    //    }
    //}

    //public void ResetAllData()
    //{
    //    isEditMode = false;
    //    WebServicesHandler.SharedInstance.friendList.Clear();
    //    for (int i = 0; i < FriendListAssign.insatnce.FBParentObject.childCount; i++)
    //    {
    //        Destroy(FriendListAssign.insatnce.FBParentObject.GetChild(i).gameObject);
    //    }
    //}
}
