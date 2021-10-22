using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using System.Text.RegularExpressions;
// In this class facebook related process is handled.
// FbLoging , logout , download profile picture etc.
using Facebook.Unity;


public class FBholder : MonoBehaviour
{/*

      public List<FBFriendsDATA> FBFriends = new List<FBFriendsDATA>();
      public bool loginSuccess = false;
      public bool loginFailed = false;

      public Text FriendsText;

      public string get_data;
      public string fbname, fbMail, fbId, fbGender, fbpass;
      public string AccessToken;
      public Sprite spriteToUse;

      public Sprite profile;


      //public Image friends_profile;
      public string Friend_ID;
      public GameObject FBfriendPerfab;

      LitJson.JsonData fbData;


      private static FBholder _instance = null;

      public static FBholder SharedInstance
      {
            get
            {
                  // if the instance hasn't been assigned then search for it
                  if (_instance == null)
                  {
                        _instance = GameObject.FindObjectOfType(typeof(FBholder)) as FBholder;
                  }
                  return _instance;
            }
      }


      // Awake method
      private void Awake()
      {
            if (!FB.IsInitialized)
            {
                  // FB.Init(() =>
                  // {
                  //     if (FB.IsInitialized)
                  //         FB.ActivateApp();
                  //     else
                  //         Debug.LogError("Couldn't initialize");
                  // },
                  // isGameShown =>
                  // {
                  //     if (!isGameShown)
                  //         Time.timeScale = 0;
                  //     else
                  //         Time.timeScale = 1;
                  // });
            }
            // else
            // FB.ActivateApp();
      }


      // Use this for initialization
      void Start()
      {

            if (PlayerPrefs.GetInt("FirstTime") == 0)
            {


                  //	FBholder.SharedInstance.FBLoadPic();
            }

            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                  FB.Init(SetInit, OnHideUnity);
            }

      }



      private void SetInit()
      {
            if (FB.IsLoggedIn)
            {

            }
            else
            {

            }
      }


      /// <summary>
      /// This method is used for download profile pictire from facebook.
      /// </summary>
      public void FBLoadPic()
      {

            Debug.LogError("call pic download ");
            StartCoroutine(downloadUserProfile());

      }

      public IEnumerator downloadUserProfile()
      {

            string EmailId = PlayerPrefs.GetString("fbemail", "");

            string url1 = "http://graph.facebook.com/" + EmailId + "/picture?type=small";

            //		Debug.LogError("Downloading Profile With Url = "+url1);

            WWW www1 = new WWW(url1);
            yield return www1;

            if (www1.texture != null)
            {

                  Debug.LogError("Downloaded Profile ");

                  Texture2D texture = new Texture2D(www1.texture.width, www1.texture.height, TextureFormat.DXT1, false);

                  // assign the downloaded image to sprite
                  www1.LoadImageIntoTexture(texture);
                  Rect rec = new Rect(0, 0, texture.width, texture.height);
                  spriteToUse = Sprite.Create(texture, rec, new Vector2(1f, 1f), 100);
                  spriteToUse.name = "facebook_profile";
                  //						playerData.profilePicture = spriteToUse;
                  //			MainMenuController.Instance.HamburgerMenu_UserPic.GetComponent<Image> ().sprite = spriteToUse;
                  // UIController.SharedInstance.Player_Image.GetComponent<Image>().sprite = spriteToUse;
                  // UIController.SharedInstance.TopBarPlayer_Image.GetComponent<Image>().sprite = spriteToUse;

                  www1.Dispose();
                  www1 = null;


            }
            else
            {

            }

      }

      IEnumerator waitLoginScreen()
      {

            yield return new WaitForSeconds(1f);

            PlayerPrefs.SetInt("FirstTime", 1);
      }

      private void OnHideUnity(bool isGameShown)
      {
            // if (!isGameShown) {
            // 	Time.timeScale = 0;
            // } else {
            // 	Time.timeScale = 1;
            // }
      }


      /// <summary>
      /// This method is used to login with facebook.
      /// This method is called from MainMenuController Class.
      /// </summary>
      public void FBlogin()
      {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                  //			MainMenuController.Instance.IsInternetConnection (true);
                  return;
            }
            if (PlayerPrefs.GetInt("FirstTimeLogin") == 0)
            {
                  StartCoroutine(waitLoginScreen());
                  //	PlayerPrefs.SetInt ("FirstTimeLogin", 1);
            }
            string[] perms = { "user_friends", "email", "user_likes", "public_profile" };
            //string[] perms = { "email", "public_profile"};
            FB.LogInWithReadPermissions(perms, AuthCallBack);
            PlayerPrefs.SetInt("isBonusCoin", 1);
            //StartCoroutine(welcome_msg());
      }

      // IEnumerator welcome_msg ()
      // {
      // 	UIController.SharedInstance.Welcome_msg.SetActive(true);
      //     UIController.SharedInstance.welcome_text.GetComponent<Text>().text = "Welcome To Joga Bonito";
      // 	yield return new WaitForSeconds(5f);
      // 	UIController.SharedInstance.Welcome_msg.SetActive(false);
      // }


      /// <summary>
      /// In this method get permission for login with facebook.
      /// </summary>
      void AuthCallBack(ILoginResult result)
      {
            // Debug.Log ("FB is login" + FB.IsLoggedIn);
            // Debug.Log("Access Token--->" + result);

            // Debug.Log("Access Token--->" + result.AccessToken.TokenString);

            AccessToken = result.AccessToken.TokenString;
            PlayerPrefs.SetString("FbToken", AccessToken);

            if (FB.IsLoggedIn)
            {
                  FB.API("me?fields=id,name,email", HttpMethod.GET, UserCallBack);

                  FB.API("me/picture?type=square&height=128&width=128", HttpMethod.GET, GetPicture);

                  //	Debug.Log ("Login Successfull2.................");
            }
            else
            {
                  loginFailed = true;
                  loginSuccess = false;
                  //	Debug.Log ("Enter in not login");
            }
      }

      /// <summary>
      /// After login success get all data which we requested.
      /// </summary>
      void UserCallBack(IGraphResult result)
      {
            if (FB.IsLoggedIn)
            {
                  // Debug.Log ("ID    " + result.ResultDictionary ["id"].ToString ());
                  // Debug.Log ("Name    " + result.ResultDictionary ["name"].ToString ());

                  fbData = LitJson.JsonMapper.ToObject(result.RawResult);
                  fbname = result.ResultDictionary["name"].ToString();
                  fbId = result.ResultDictionary["id"].ToString();
                  fbMail = result.ResultDictionary["email"].ToString();

                  // fbGender = result.ResultDictionary["gender"].ToString();
                  // fbpass = result.ResultDictionary[""]
                  // Debug.Log ("fbname--" + fbname);
                  // Debug.Log ("fbId---" + fbId);
                  // Debug.Log ("fbMail---" + fbMail);
                  // Debug.Log("fbGender---" + fbGender);

                  PlayerPrefs.SetString("fbemail", fbMail);

                  Debug.Log("ActiveLoading2Panel-18");
                  UIController.SharedInstance.ActiveLoading2Panel();
                  WebServicesHandler.SharedInstance.LoginWithFacebook("facebook", fbname, fbMail, fbId);  //NoDE
                                                                                                          //UIController.SharedInstance.DeactiveLoadingPanel(1f);


            }
            else
            {
                  loginFailed = true;
                  loginSuccess = false;
            }
      }

      public void GetFriendsPlayingThisGame()
      {
            //Debug.Log("My faceBook Friends---------------->>>");
            string query = "/me/friends";
            //string query =  "me/?fields=friends";

            FB.API(query, HttpMethod.GET, result =>
            // FB.API(query, HttpMethod.GET);

            {
              //for (int i = 0; i < result.RawResult.Length; i++)
              //{
              // Debug.Log("Friend Name--- " + result.RawResult["data"][i]["FriendUserName"].ToString());
              FBFriendsDATA FBdata = new FBFriendsDATA();
              // FBdata.fbname = ((Dictionary<string, object>))["name"];


              // }
              Dictionary<string, object> Fbresult = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);

                  var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
                  var friendsList = (List<object>)dictionary["data"];

              // FriendsText.text = string.Empty;
              foreach (var dict in friendsList)
                  {
                    // Debug.Log(dict);
                    // Debug.Log("forEach");
                    // FriendsText.text += ((Dictionary<string, object>)dict)["name"];
                    FBdata.userName = ((Dictionary<string, object>)dict)["name"].ToString();
                        FBdata.fbid = ((Dictionary<string, object>)dict)["id"].ToString();
                    //CreateFriend(((Dictionary<string, object>)dict)["name"].ToString(),((Dictionary<string, object>)dict)["id"].ToString());
                    //FBdata.fbpicurl=((Dictionary<string, object>)dict)["picture"].ToString();
                    //FBdata.fbPic =  friends_profile;
              }
                  FBFriends.Add(FBdata);
                  AssignFbFriendData();
            });

            //	FB.API("me/friends",HttpMethod.GET,FriendsCallBack);  ----second method
      }

      // void FriendsCallBack(IGraphResult result)
      //     {
      //         Debug.Log(result.RawResult);
      //         IDictionary<string, object> data = result.ResultDictionary;
      //         List<object> friends = (List<object>)data["data"];
      //         foreach (object obj in friends)
      //         {
      //             Dictionary<string, object> dict = (Dictionary<string, object>)obj;
      //             CreateFriend(dict["name"].ToString(),dict["id"].ToString());
      //         }
      //     }

      // Create a friend
      // public void  CreateFriend ( string  name , string  id )
      // {

      //     FB.API(id + "/picture?width=8&height=8", HttpMethod.GET, delegate (IGraphResult frnpic) {
      // 		 if (frnpic.Error == null && frnpic.Texture != null)
      // 		 {
      //     		friends_profile.GetComponent<Image>().sprite = Sprite.Create(frnpic.Texture, new Rect(0, 0, 8, 8), new Vector2(0.5f, 0.5f));
      // 		 }
      //     });
      // }

      public void AssignFbFriendData()
      {

            for (int i = 0; i < FBholder.SharedInstance.FBFriends.Count; i++)
            {
                  GameObject FF = Instantiate(FBfriendPerfab, Vector3.zero, Quaternion.identity) as GameObject;
                  // Debug.Log("created" + FBfriendPerfab.gameObject.GetComponent<Transform>().position);
                  FF.name = "Formation" + i;
                  FF.transform.parent = FriendListAssign.insatnce.FBParentObject;
                  FF.transform.localScale = Vector3.one;
                  FF.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
                  // FF.GetComponent<FbchallengePlayer>().playerCountry = "India";
                  FF.GetComponent<FbchallengePlayer>().playerID = FBFriends[i].fbid;
                  Friend_ID = FBFriends[i].fbid;
                  FF.GetComponent<FbchallengePlayer>().playerImageUrl = "";
                  FF.GetComponent<FbchallengePlayer>().playerName = FBFriends[i].userName;
                  FB.API(Friend_ID + "/picture?width=8&height=8", HttpMethod.GET, delegate (IGraphResult frnpic)
                  {

                        if (frnpic.Error == null && frnpic.Texture != null)
                        {
                              //  Debug.Log("Got Image to post");
                              FF.GetComponent<FbchallengePlayer>().playerImage.GetComponent<Image>().sprite = Sprite.Create(frnpic.Texture, new Rect(0, 0, 24, 24), new Vector2());
                              //	UIController.SharedInstance.TopBarPlayer_Image.GetComponent<Image>().sprite =  Sprite.Create(frnpic.Texture, new Rect(0, 0, 24, 24), new Vector2());      	
                        }
                  });
                  // = FBFriends[i].fbPic;
                  // FF.GetComponent<FbchallengePlayer>().playerNameID = FBholder.SharedInstance.FBFriends[i].userId;

            }
      }

      /// <summary>
      /// This method is used for logout with facebook.
      /// </summary>
      public void Logout()
      {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                  if (FB.IsLoggedIn)
                  {
                        FB.LogOut();
                        PlayerPrefs.SetInt("isLoginWithFacebook", 0);
                        PlayerPrefs.Save();
                  }
            }
      }
      public void GetPicture(IGraphResult result)
      {
            if (result.Error == null && result.Texture != null)
            {
                  //http://stackoverflow.com/questions/19756453/how-to-get-users-profile-picture-with-facebooks-unity-sdk
                  /*if (result.Texture != null) {
				Image img = ProfilePicture.GetComponent<Image> ();
				img.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
			}
                  //ProfilePicture.sprite = Sprite.Create (result.Texture, new Rect (0, 0, 128, 128), new Vector2 ());
                  profile = Sprite.Create(result.Texture, new Rect(0, 0, 128, 128), new Vector2());

                  UIController.SharedInstance.Player_Image.GetComponent<Image>().sprite = profile;
                  UIController.SharedInstance.TopBarPlayer_Image.GetComponent<Image>().sprite = profile;

            }
      }
*/
}

[System.Serializable]

public class FBFriendsDATA
{
      public string userName;
      public string fbid;
      public string fbpicurl;

      public Sprite fbPic;

}


// Facebook Login and Get Friends Example
//using System.Collections.Generic;
//using UnityEngine;
//using Facebook.Unity;
//using UnityEngine.UI;

//public class FBholder : MonoBehaviour
//{

//    public Text FriendsText;

//    private void Awake()
//    {
//        if (!FB.IsInitialized)
//        {
//            FB.Init(() =>
//            {
//                if (FB.IsInitialized)
//                    FB.ActivateApp();
//                else
//                    Debug.LogError("Couldn't initialize");
//            },
//            isGameShown =>
//            {
//                if (!isGameShown)
//                    Time.timeScale = 0;
//                else
//                    Time.timeScale = 1;
//            });
//        }
//        else
//            FB.ActivateApp();
//    }

//    #region Login / Logout
//    public void FacebookLogin()
//    {
//        var permissions = new List<string>() { "public_profile", "email", "user_friends" };
//        FB.LogInWithReadPermissions(permissions);
//    }

//    public void FacebookLogout()
//    {
//        FB.LogOut();
//    }
//    #endregion

//    public void FacebookShare()
//    {
//        FB.ShareLink(new System.Uri("https://resocoder.com"), "Check it out!",
//            "Good programming tutorials lol!",
//            new System.Uri("https://resocoder.com/wp-content/uploads/2017/01/logoRound512.png"));
//    }

//    #region Inviting
//    public void FacebookGameRequest()
//    {
//        FB.AppRequest("Hey! Come and play this awesome game!", title: "Reso Coder Tutorial");
//    }

//    public void FacebookInvite()
//    {
//        FB.Mobile.AppInvite(new System.Uri("https://play.google.com/store/apps/details?id=com.tappybyte.byteaway"));
//    }
//    #endregion

//    public void GetFriendsPlayingThisGame()
//    {
//        string query = "/me/friends";
//        FB.API(query, HttpMethod.GET, result =>
//        {
//            var dictionary = (Dictionary<string, object>)Facebook.MiniJSON.Json.Deserialize(result.RawResult);
//            var friendsList = (List<object>)dictionary["data"];
//            FriendsText.text = string.Empty;
//            foreach (var dict in friendsList)
//                FriendsText.text += ((Dictionary<string, object>)dict)["name"];
//        });
//    }
//}