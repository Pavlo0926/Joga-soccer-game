namespace Jiweman
{
    public static class Joga_API
    {
        public enum Server
        {
            PROD,
            DEV
        }

        public enum ChallengeStatus
        {
            acceptedByChallenger = 0,
            accepted = 1,
            declined = 2
        }

        #region CONSTANT VARIABLES
        private const string PROD_SERVER_BASEURL = "https://jiweman.com:3000";
        private const string DEV_SERVER_BASEURL = "https://dev.jiweman.com/api";
        #endregion

        public static string WebPortal;

        public static string Registration = "/auth/registerWithJieman";
        public static string Login = "/auth/loginwithJiweman";
        public static string ActivateAccount = "/auth/activateAccount";
        public static string ForgotPassword = "/auth/forgotpassword";
        public static string ForgotUsername = "/auth/getUsername";
        public static string ResetPassword = "/auth/resetPassword";
        public static string PlayerProfile = "/auth/playerProfile";

        public static string Players = "/players";

        public static string Formations = "/formations";

        public static string SaveDeviceToken = "/PlayWithFriend/saveDeviceToken";
        public static string AddPlayerFriendList = "/PlayWithFriend/addPlayerFriendList";
        public static string DeletePlayerFriendList = "/PlayWithFriend/deletePlayerFriendList?playerId=";
        public static string GetAllFriends = "/PlayWithFriend/getAllFriends";
        public static string SearchAllPlayers = "/PlayWithFriend/searchAllPlayers";
        public static string SearchAllFriends = "/PlayWithFriend/searchAllFriends";
        public static string ChallengeFriend = "/PlayWithFriend/challengeFriend";
        public static string ChallengeFriendStatus = "/PlayWithFriend/challengeStatus";

        public static string GetLeague = "/league";

        public static string Leaderboard = "/leaderboard?";
        public static string UpdateLeaderboard = "/updatePlayerLeaderBoard";
        public static string PointsInfo = "/pointsInfo";

        public static string CreateNewMatch = "/createNewMatch";
        public static string UpdateMatchResult = "/updateMatchResult";

        public static string GetLeagueAvailability = "/getLeagueAvailability?leagueId=";

        public static string CheckAuthentication = "/auth/checkAuthentication";

        public static string GetTotalPrizePool = "/getprizepoolAmount?leagueId=";

        public static void UpdateServerBase(Server serverBase)
        {
            string url = serverBase.Equals(Server.PROD) ? PROD_SERVER_BASEURL : DEV_SERVER_BASEURL;
            WebPortal = serverBase.Equals(Server.PROD) ? "https://jiweman.com/web" : "https://dev.jiweman.com/web";
            Registration = url + Registration;
            Login = url + Login;
            ActivateAccount = url + ActivateAccount;
            ForgotPassword = url + ForgotPassword;
            ForgotUsername = url + ForgotUsername;
            ResetPassword = url + ResetPassword;
            PlayerProfile = url + PlayerProfile;
            Players = url + Players;
            Formations = url + Formations;
            SaveDeviceToken = url + SaveDeviceToken;
            AddPlayerFriendList = url + AddPlayerFriendList;
            DeletePlayerFriendList = url + DeletePlayerFriendList;
            GetAllFriends = url + GetAllFriends;
            SearchAllPlayers = url + SearchAllPlayers;
            SearchAllFriends = url + SearchAllFriends;
            ChallengeFriend = url + ChallengeFriend;
            ChallengeFriendStatus = url + ChallengeFriendStatus;
            GetLeague = url + GetLeague;
            Leaderboard = url + Leaderboard;
            UpdateLeaderboard = url + UpdateLeaderboard;
            PointsInfo = url + PointsInfo;
            CreateNewMatch = url + CreateNewMatch;
            UpdateMatchResult = url + UpdateMatchResult;
            GetLeagueAvailability = url + GetLeagueAvailability;
            CheckAuthentication = url + CheckAuthentication;
            GetTotalPrizePool = url + GetTotalPrizePool;
        }
    }
}

