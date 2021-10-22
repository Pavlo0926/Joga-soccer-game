using System;
using UnityEngine;
using BestHTTP;
using Photon.Realtime;
using ExitGames.Client.Photon;
using Photon.Pun;
using FastSkillTeam;

namespace Jiweman
{
    public enum Joga_MatchType
    {
        oneonone,
        playWithFriends,
        leagueGamePlay
    }

    public class Joga_NetworkManager : MonoBehaviour
    {
        public static Joga_NetworkManager Instance;

        [Tooltip("In seconds, when leaderboard is open it will refresh once, then every this many seconds (0 = never, only on open)")]
        public float LeaderboardRefreshRate = 15f;

        #region PRIVATE VARIABLES
        //   private string token;
        //   private string playerId;
        private string message;

        //leaderboard cached sorting vals
        private string m_lastSort = "";
        private string m_lastLeagueId = "";
        private Joga_MatchType m_LastMatchType = Joga_MatchType.oneonone;
        #endregion

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);
            }

            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            CancelLeaderBoardRefresh();
        }

        #region HTTP METHOD Request

        #region AUTH REQUEST
        public void RegistrationAuthReuqest(string name, string userName, string email, string password, string confirmPass, string country, string birthday, string gender, string mobileNumber)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.Registration), HTTPMethods.Post, OnRegistrationAuthRequest);

            request.AddField("fullName", name);
            request.AddField("userName", userName);
            request.AddField("email", email);
            request.AddField("password", password);
            request.AddField("confirmPassword", confirmPass);
            request.AddField("countryOfRecidence", country);
            request.AddField("dateOfBirth", birthday);
            request.AddField("gender", gender);
            request.AddField("mobileNumber", mobileNumber);

            request.Send();
        }
        public void GetPartialUpdateRequest()
        {
            Debug.Log("GetPartialUpdateRequest() NOT YET DONE!!!!");
            HTTPRequest request = new HTTPRequest(new Uri("Joga_API.ApiForPartialTBA"), HTTPMethods.Get, OnGetPartialUpdateRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetActivateAcountVerifacitionRequest()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ActivateAccount), HTTPMethods.Get, OnGetActivateAccountVerificationRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void ForgotPasswordRequest(string email)
        {
         //  Debug.Log("FORGOT PASS REQUEST CALLED : " + Joga_API.ForgotPassword);
            
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ForgotPassword), HTTPMethods.Post, OnForgotPasswordRequest);
            request.AddField("email", email);

            request.Send();
        }

        public void ForgotUserNameRequest(string email)
        {
          //  Debug.Log("FORGOT USER REQUEST CALLED : " + Joga_API.ForgotUsername);

            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ForgotUsername), HTTPMethods.Post, OnForgotUsernameRequest);
            request.AddField("email", email);

            request.Send();
        }

        public void ResetPasswordRequest(string email)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ResetPassword), HTTPMethods.Post, OnResetPasswordRequest);
            request.AddField("email", email);

            request.Send();
        }

        public void LoginAuthRequest(string userName, string password)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.Login), HTTPMethods.Post, OnLoginAuthRequest);
            request.AddField("userName", userName);
            request.AddField("password", password);

            request.Send();
        }
        #endregion

        public void CheckAuthentication()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.CheckAuthentication), HTTPMethods.Get, OnCheckAuthentication);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        private void OnCheckAuthentication(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
               // Debug.LogWarning("OnCheckAuthentication(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

        //    Debug.Log("CheckAuthentication finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;
        }

        public void GetPlayersRequest()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.Players), HTTPMethods.Get, OnGetPlayerRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetFormationsData()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.Formations), HTTPMethods.Get, OnGetFormationsRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }
        public void GetLeaderBoardData(Joga_MatchType matchType, string by, string leagueID)
        {
            m_lastLeagueId = leagueID;
            m_lastSort = by;
            m_LastMatchType = matchType;

        //    Debug.Log("Last matchtype = " + m_LastMatchType + ", Last Sort = " + m_lastSort + ", Last LeagueID = " + m_lastLeagueId);

            if(LeaderboardRefreshRate <= 0)
            {
                RefreshLeaderboardData();
                return;
            }

            CancelLeaderBoardRefresh();

            InvokeRepeating("RefreshLeaderboardData", 0, LeaderboardRefreshRate);
        }
        public void CancelLeaderBoardRefresh()
        {
            if (LeaderboardRefreshRate <= 0)
                return;

            //   Debug.Log("CANCELLING LEADERBOARD REFRESH");
            CancelInvoke("RefreshLeaderboardData");
        }
        private void RefreshLeaderboardData()
        {
            if (!isSendLBRequest)
                return;

            isSendLBRequest = false;

            string leaderBoardUrl = Joga_API.Leaderboard + (string.IsNullOrEmpty(m_lastSort) ? "" : "sortBy=" + m_lastSort + "&") + "gameType=" + m_LastMatchType + (m_LastMatchType == Joga_MatchType.leagueGamePlay ? "&leagueId=" + m_lastLeagueId : "");
          //  Debug.Log("REFRESH LEADERBOARD! > lb url = " + leaderBoardUrl);
            HTTPRequest request = new HTTPRequest(new Uri(leaderBoardUrl), HTTPMethods.Get, OnGetLeaderboardRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetPlayerStat()
        {
            string leaderBoardUrl = Joga_API.Leaderboard + "sortBy=points&gameType=oneonone";
            HTTPRequest request = new HTTPRequest(new Uri(leaderBoardUrl), HTTPMethods.Get, OnGetPlayerStat);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetLeaderboardPointsInfo()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.PointsInfo), HTTPMethods.Get, OnGetLeaderboardPointsInfo);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        [System.Obsolete("This data is handled in UpdateMatchResultRequest. Please do NOT use this method")]
        public void PostLeaderBoardData(string winName, string p1Score, string p2Score, string p1Name, string p2Name, Joga_MatchType matchType,/*string matchDuration,*/ string matchDurationP1, string matchDurationP2, string leagueId = "")
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.UpdateLeaderboard), HTTPMethods.Post, OnPostLeaderBoardRequest);

            request.AddHeader("Authorization", Joga_Data.Token);

            request.AddField("winnerName", winName);
            request.AddField("playerOneGoal", p1Score);
            request.AddField("playerTwoGoal", p2Score);
            request.AddField("roomName", Joga_Data.RoomName);
            request.AddField("playerOneUserName", p1Name);
            request.AddField("playerTwoUserName", p2Name);
            request.AddField("matchType", matchType.ToString());
           // request.AddField("matchDuration", matchDuration);
            request.AddField("playerOneMatchDuration", matchDurationP1);
            request.AddField("playerTwoMatchDuration", matchDurationP2);
            request.AddField("matchId", Joga_Data.MatchId);
            request.AddField("leagueId", leagueId);

            request.Send();
        }

        public void GetLeaguesRequest()
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.GetLeague), HTTPMethods.Get, OnGetLeaguesRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetLeagueAvailabilityRequest(FST_LeagueCard leaguebutton)
        {
            UIController.Instance.Loading_2_panel.SetActive(true);
            m_LastLeagueButton = leaguebutton;

            string url = Joga_API.GetLeagueAvailability + leaguebutton.LeagueID;
           // Debug.Log("GetLeagueAvailabilityRequest > url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnGetLeagueAvailabilityRequest);
            request.AddHeader("Authorization", Joga_Data.Token);
            request.Send();
        }

        public void GetTotalPrizePool(string leagueID)
        {
            string url = Joga_API.GetTotalPrizePool + leagueID;
            // Debug.Log("GetLeagueAvailabilityRequest > url: " + url);
            HTTPRequest request = new HTTPRequest(new Uri(url), HTTPMethods.Get, OnGetTotalPrizePool);
            request.AddHeader("Authorization", Joga_Data.Token);
            request.Send();
        }


        private void OnGetTotalPrizePool(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetTotalPrizePool(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

            //  Debug.Log("GetTotalPrizePool Request finished with a response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.LogWarning("OnGetTotalPrizePool(): jsonData was empty!, Ignore if quitting app");
                return;
            }

            JSONNode jsonParse = JSON.Parse(jsonData);

            if (jsonParse.IsNull)
            {
                Debug.LogWarning("OnGetTotalPrizePool(): jsonParse was empty!, Ignore if quitting app");
                return;
            }

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }
            else
            {
                FST_PrizePoolUpdater.Currency = jsonParse["currency"];
                FST_PrizePoolUpdater.Amount = jsonParse["prizepoolAmount"].AsInt;
                FST_PrizePoolUpdater.OnUpdate();
               // Debug.Log(message);
            }
        }

        #region MATCH HTTP REQUEST
        public void CreateNewMatchRequest(string p1Name, string p2Name, string roomName, bool isRematch, string matchType, string leagueId)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.CreateNewMatch), HTTPMethods.Post, OnCreateNewMatchRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            int rematch = isRematch ? 1 : 0;
           
            request.AddField("playerOneUserName", p1Name);
            request.AddField("playerTwoUserName", p2Name);
            request.AddField("roomName", roomName);
            request.AddField("isRematch", rematch.ToString());
            request.AddField("matchType", matchType);
            request.AddField("leagueId", leagueId);

            request.Send();
        }

        public void UpdateMatchResultRequest(string winnerName,string p1Name, string p2Name, int p1Score, int p2Score, float matchDuration, float matchDurationP1, float matchDurationP2)
        {
         //   Debug.Log("Updating match result...");

            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.UpdateMatchResult), HTTPMethods.Post, OnUpdateMatchResultRequest);

            request.AddHeader("Authorization", Joga_Data.Token);

            request.AddField("roomName", Joga_Data.RoomName);
            request.AddField("matchId", Joga_Data.MatchId);
            request.AddField("leagueId", GameManager.CurrentLeagueID);
            request.AddField("winnerName", winnerName);
            request.AddField("playerOneUserName", p1Name);
            request.AddField("playerTwoUserName", p2Name);
            request.AddField("playerOneGoal", p1Score.ToString());
            request.AddField("playerTwoGoal", p2Score.ToString());
            request.AddField("matchType", Joga_Data.MatchType.ToString());

            decimal a = Math.Round((decimal)matchDuration + 0.0000m, 4);
            decimal b = Math.Round((decimal)matchDurationP1 + 0.0000m, 4);
            decimal c = Math.Round((decimal)matchDurationP2 + 0.0000m, 4);

            Debug.LogFormat("match times : {0}, {1}, {2}", a, b, c);

            // if need to format to minutes
           // string dur = string.Format("{0:00} : {01:00}", Mathf.CeilToInt(matchDuration / 60), Mathf.CeilToInt(matchDuration % 60));

            request.AddField("matchDuration", a.ToString());
            request.AddField("playerOneMatchDuration", b.ToString());
            request.AddField("playerTwoMatchDuration", c.ToString());

            request.Send();

           // PostLeaderBoardData(winnerName, p1Score.ToString(), p2Score.ToString(), p1Name, p2Name, Joga_Data.MatchType,/*a.ToString(),*/ b.ToString(), c.ToString(), GameManager.Instance.CurrentLeagueID);
        }
        #endregion

        #region PLAY WITH FRIENDS HTTP REQUEST
        public void SaveDeviceTokenRequest()
        {
            if(string.IsNullOrEmpty(Joga_Data.DeviceToken))
            {
                Debug.LogWarning("SaveDeviceTokenRequest() => Joga_Data.DeviceToken is NULL!!! (Ignore if in editor this is for firebase)");
                return;
            }
            if(Joga_Data.Token.Length < 2)
            {
                Debug.LogWarning("SaveDeviceTokenRequest() => Joga_Data.Token is INVALID!!!");
                return;
            }

            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.SaveDeviceToken), HTTPMethods.Post, OnSaveDeviceTokenRequest);
            request.AddField("deviceToken", Joga_Data.DeviceToken);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void AddPlayerFriendListRequest(string id)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.AddPlayerFriendList), HTTPMethods.Post, OnAddPlayerFriendListRequest);
            request.AddField("playerId", id);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void DeletePlayerFriendListRequest(string id)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.DeletePlayerFriendList + id), HTTPMethods.Delete, OnDeletePlayerFriendListRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void GetAllFriendsRequest()
        {
         //   Debug.Log(Joga_API.GetAllFriends);
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.GetAllFriends), HTTPMethods.Get, OnGetAllFriendsRequest);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void SearchAllPlayersRequest(string userName)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.SearchAllPlayers), HTTPMethods.Post, OnSearchAllPlayersRequest);
            request.AddField("userName", userName);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void SearchAllFriendsRequest(string userName)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.SearchAllFriends), HTTPMethods.Post, OnSearchAllFriendsRequest);
            request.AddField("userName", userName);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }
       
        public void SendChallengeTo(string id)
        {
            FST_MainChat.Instance.SendPWFRequestTo(id);
            Debug.Log("Send ChallengeFriendRequest() challenged player = " + id);

            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ChallengeFriend), HTTPMethods.Post, OnChallengeSent);
            request.AddField("playerId", id);
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }

        public void SendChallengeReplyTo(string id, Joga_API.ChallengeStatus challengeStatus)
        {
            HTTPRequest request = new HTTPRequest(new Uri(Joga_API.ChallengeFriendStatus), HTTPMethods.Post, OnChallengeReply);
            request.AddField("playerId", id);
            request.AddField("challengeStatus", challengeStatus.ToString());
            request.AddHeader("Authorization", Joga_Data.Token);

            request.Send();
        }
        #endregion

        #endregion

        #region Request Callback Handler

        /// <summary>
        /// check and handle any response that contains "already logged in"
        /// </summary>
        /// <param name="msg">the response as a raw string</param>
        /// <returns>true when player has logged in on another account if already logged in on first device</returns>
        private bool AlreadyLoggedIn(string msg)
        {
            bool b = false;

            if (msg.Contains("already logged in"))
            {
                b = true;

                Debug.LogWarning("This account is active on another device, return to login");

                SSTools.ShowMessage("This account is active on another device, return to login", SSTools.Position.bottom, SSTools.Time.threeSecond);

                UIController.Instance.Loading_2_panel.SetActive(false);
                UIController.Instance.LogOut();
            }

            return b;
        }

        private void OnRegistrationAuthRequest(HTTPRequest request, HTTPResponse response)
        {
            if(response == null)
            {
                Debug.LogWarning("SERVER IS DOWN OR HAS ERRORS");
                SSTools.ShowMessage("SERVER ERROR, PLEASE TRY AGAIN", SSTools.Position.bottom, SSTools.Time.threeSecond);
                UIController.Instance.DeactiveLoading2Panel(0.1f);
                return;
            }

            string jsonData = response.DataAsText;

          //  Debug.Log("Login request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(jsonParse["message"].Value);
                Joga_AuthManager.Instance.OnRegisterVerificationFinished(false, message);
            }

            else
            {
              //  string status = jsonParse["registrationStatus"].Value;//not yet used
                Joga_Data.PlayerID = jsonParse["_id"].Value;

                Joga_AuthManager.Instance.OnRegisterVerificationFinished(true, message);
            }
        }

        private void OnGetPartialUpdateRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetPartialUpdateRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

           // Debug.Log("Get partial update request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }
            else
            {
                FST_UpdateManager.PartialUpdate(jsonParse);
            }
        }

        private void OnGetActivateAccountVerificationRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetActivateAccountVerificationRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

            //Debug.Log("Get account verification request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;
        }

        private void OnForgotPasswordRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnForgotPasswordRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

          //  Debug.Log("Post forgot password request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }
            else
            {
                SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
                //Debug.Log("Check your email to activate new password");
            }
        }

        private void OnForgotUsernameRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnForgotUsernameRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

         //   Debug.Log("Post forgot username request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;
            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }
            else
            {
                SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
              //  Debug.Log("Check your email to get your username");
            }
        }

        private void OnResetPasswordRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnResetPasswordRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

         //   Debug.Log("Reset password request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(jsonParse["message"].Value);
            }
            else
            {
                UIController.Instance.LogOut();
                SSTools.ShowMessage(jsonParse["message"].Value, SSTools.Position.bottom, SSTools.Time.threeSecond);
              //  Debug.Log("Check your email to activate new password");
            }
        }

        private void OnLoginAuthRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnLoginAuthRequest() > NO RESPONSE, WILL OVERRIDE");
                UIController.Instance.Loading_2_panel.SetActive(false);
                return;
            }

            string jsonData = response.DataAsText;

         //   Debug.Log("Login request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(jsonParse["message"].Value);
                Joga_AuthManager.Instance.OnLoginVerificationFinished(false, message);
            }

            else
            {
                Joga_Data.Token = "b " + jsonParse["token"].Value;
                Joga_Data.PlayerID = jsonParse["data"]["_id"].Value;

                Joga_AuthManager.Instance.OnLoginVerificationFinished(true, message);

                //Save Device Token
                SaveDeviceTokenRequest();
            }
        }

        private void OnGetPlayerRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

            Debug.Log("Get player request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;
        }

        private void OnGetFormationsRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

          //  Debug.Log("Formation request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;
        }
        private bool isSendLBRequest = true;
        private void OnGetLeaderboardRequest(HTTPRequest request, HTTPResponse response)
        {
            isSendLBRequest = true;

            if (response == null)
            {
                Debug.LogWarning("OnGetLeaderboardRequest > response was null, ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

            if (string.IsNullOrEmpty(jsonData))
            {
                Debug.LogWarning("OnGetLeaderboardRequest > jsonData was empty, ignore if quitting app");
                return;
            }

            if (AlreadyLoggedIn(jsonData))
                return;

            //    Debug.Log("Get Leaderboard request finished with response of : " + jsonData);

            var jsonParse = JSON.Parse(jsonData);

            //For cleaner debug values
            //for (int i = 0; i < jsonParse["data"].Count; i++)
            //{
            //    Debug.Log(jsonParse["data"][i]);
            //}

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                if (message == "data not found")
                    Joga_LeaderBoard.Instance.AssignZeroOrNullData();
                else
                    Debug.LogWarning(message);
            }
            else
            {
               // Debug.Log(message);
                if (message == "data not found")
                    Joga_LeaderBoard.Instance.AssignZeroOrNullData();
                else
                    Joga_LeaderBoard.Instance.GetData(jsonParse);
            }
        }

        private void OnGetPlayerStat(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetPlayerStat(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

        //    Debug.Log("Get player stat request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }

            else
            {
             //   Debug.Log("Show Players Stat");
                Joga_Player.GetStat(jsonParse);
            }
        }

        private void OnGetLeaderboardPointsInfo(HTTPRequest originalRequest, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetLeaderboardPointsInfo(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

            if (AlreadyLoggedIn(jsonData))
                return;

            throw new NotImplementedException();
        }

        private void OnPostLeaderBoardRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnPostLeaderBoardRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

            Debug.Log("Post Leaderboard request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }
            else
            {
                Debug.Log(message);
            }
        }

        private void OnGetLeaguesRequest(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.LogWarning("OnGetLeaguesRequest(): Response was null!, Ignore if quitting app");
                UIController.Instance.Loading_2_panel.SetActive(false);//just in case its active..
                return;
            }

            string jsonData = response.DataAsText;

         //   Debug.Log("Get Leagues Request finished with a response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }

            else
            {
                FST_League_Handler.Instance.SetLeagueData(jsonParse);
              //  Debug.Log(message);
            }
        }
        private FST_LeagueCard m_LastLeagueButton = null;
        private void OnGetLeagueAvailabilityRequest(HTTPRequest request, HTTPResponse response)
        {
            UIController.Instance.Loading_2_panel.SetActive(false);

            if (response == null)
            {
                Debug.LogWarning("OnGetLeaguesRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

          //  Debug.Log("Get League Availability Request finished with a response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }

            else
            {
                if (m_LastLeagueButton != null)
                    m_LastLeagueButton.UpdateLeagueAvailability(jsonParse);
                else Debug.Log("no league button!");
                Debug.Log(message);
            }
        }

        private void OnSaveDeviceTokenRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

          //  Debug.Log("Save Device Token request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;
            Debug.Log(message);
        }

        private void OnAddPlayerFriendListRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

          //  Debug.Log("Add player to friend list request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                Joga_SearchPlayerDataStore.Instance.AddFriendStatus(false, message);
            }

            else
            {
                Debug.Log(message);
                Joga_SearchPlayerDataStore.Instance.AddFriendStatus(true, message);
            }
        }

        private void OnDeletePlayerFriendListRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

         //   Debug.Log("Delete player to friend list request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            Debug.Log(ChallegePlayerDataStore.Instance);

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                ChallegePlayerDataStore.Instance.CheckDeletedFriendStatus(false, message);
            }

            else
            {
                Debug.Log(message);
                ChallegePlayerDataStore.Instance.CheckDeletedFriendStatus(true, message);
            }
        }

        private void OnGetAllFriendsRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

         //   Debug.Log("Get all friends request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                Joga_FriendsManager.Instance.CheckFriendStatus(false, jsonParse);
            }

            else
            {
                Debug.Log(message);
                Joga_FriendsManager.Instance.CheckFriendStatus(true, jsonParse);
            }
        }

        private void OnSearchAllPlayersRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

        //    Debug.Log("Search all players request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                Joga_SearchPlayerManager.Instance.CheckPlayerSearchStatus(false, jsonParse);
            }

            else
            {
                Debug.Log(message);
                Joga_SearchPlayerManager.Instance.CheckPlayerSearchStatus(true, jsonParse);
            }
        }

        private void OnSearchAllFriendsRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

         //   Debug.Log("Search all friends request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                Joga_FriendsManager.Instance.CheckFriendStatus(false, jsonParse);
            }

            else
            {
                Debug.Log(message);
                Joga_FriendsManager.Instance.CheckFriendStatus(true, jsonParse);
            }
        }

        private void OnChallengeSent(HTTPRequest request, HTTPResponse response)
        {
            if (response == null)
            {
                Debug.Log("NO SERVER RESPONSE! Request status = " + request.State.ToString());
                ChallegePlayerDataStore.Instance.ChallengeSent(false, message);
                return;
            }

            string jsonData = response.DataAsText;

            FST_MPDebug.Log("OnChallengeSent response: " + jsonData);
            Debug.Log("OnChallengeSent response: " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
                ChallegePlayerDataStore.Instance.ChallengeSent(false, message);
            }

            else
            {
                FST_MPDebug.Log(message);
                Debug.Log(message);
                ChallegePlayerDataStore.Instance.ChallengeSent(true, message);
            }
        }
        /// <summary>
        /// NOT USED OR NEEDED YET!!!
        /// </summary>
        /// <param name="request"></param>
        /// <param name="response"></param>
        private void OnChallengeReply(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

            if (AlreadyLoggedIn(jsonData))
                return;

          //  FST_MPDebug.Log("OnChallengeReply response: " + jsonData);
          //  Debug.Log("OnChallengeReply response: " + jsonData);

            var jsonParse = JSON.Parse(jsonData);

            message = jsonParse["message"].Value;

            string playerID = jsonParse["data"]["playerId"].Value;//note, it should be like this!
            string challStatus = jsonParse["data"]["challengeStatus"].Value;//note, it should be like this!

            // FST_MPDebug.Log("challenge: " + challStatus);
            //  Debug.Log("challenge: " + challStatus);

            if (challStatus == "accepted")
            {
                if (playerID == Joga_Data.PlayerID)
                {

                }
                else
                {

                }
            }
            if (challStatus == "acceptedByChallenger")
            {
                if (playerID == Joga_Data.PlayerID)
                {
                  
                }
                else
                {
            
                }
            }
            else
            {
                if (playerID == Joga_Data.PlayerID)
                {
        
                }
                else
                {

                }
            }
        }

        private void OnCreateNewMatchRequest(HTTPRequest request, HTTPResponse response)
        {
            string jsonData = response.DataAsText;

         //   Debug.Log("Create new match status request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }

            else
            {
             //   Debug.Log(message);
                Joga_Data.MatchId = jsonParse["data"]["_id"].Value;
                PhotonNetwork.RaiseEvent(FST_ByteCodes.GOT_OB_ID, Joga_Data.MatchId, RaiseEventOptions.Default, SendOptions.SendReliable);
              //  Debug.Log(Joga_Data.MatchId);
            }
        }

        private void OnUpdateMatchResultRequest(HTTPRequest request, HTTPResponse response)
        {
            if(response == null)
            {
                Debug.LogWarning("OnUpdateMatchResultRequest(): Response was null!, Ignore if quitting app");
                return;
            }

            string jsonData = response.DataAsText;

        //    Debug.Log("Update match result status request finished with response of : " + jsonData);

            if (AlreadyLoggedIn(jsonData))
                return;

            var jsonParse = JSON.Parse(jsonData);
            message = jsonParse["message"].Value;

            if (!jsonParse["status"].AsBool)
            {
                Debug.LogWarning(message);
            }

            else
            {
                Debug.Log(message);

                //NOTE:Below is now sent as soon as, and inside of UpdateMatchResultRequest is sent
                //string winnerName = jsonParse["data"]["winnerName"].Value;
                //string p1Score = jsonParse["data"]["playerOneGoal"].Value;
                //string p2Score = jsonParse["data"]["playerTwoGoal"].Value;
                //string p1Name = jsonParse["data"]["playerOneUserName"].Value;
                //string p2Name = jsonParse["data"]["playerTwoUserName"].Value;
                //PostLeaderBoardData(winnerName, p1Score, p2Score, p1Name, p2Name, Joga_Data.MatchType, p1MatchDuration, p2MatchDuration);
            }
        }

        #endregion
    }
}