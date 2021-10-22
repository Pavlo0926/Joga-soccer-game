/////////////////////////////////////////////////////////////////////////////////
//
//  FST_SettingsManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This script should not be attached to any object. The class
//                  is designed for very easy access to player pref vars that are 
//                  used throughout the project. This should be the home of all
//                  PlayerPref values for easy developer tracking. This also makes 
//                  obscuring the vars very easy if we choose to do so.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
namespace FastSkillTeam {
    public static class FST_SettingsManager
    {
        private const string PLAYER_NAME = "PlayerName";
        private const string PASSWORD = "Password";
        private const string MATCH_MODE = "MatchType";
        private const string TIME_BASED = "TimeBased";
        private const string FORMATION_SELECTION = "FormationSelection";
        private const string FORMATION_SELECTION_OFFLINE_OPPONENT = "FormationSelectionOfflineOpponent";
        private const string TEAM_SELECTION = "TeamSelection";
        private const string TEAM_SELECTION_OFFLINE_OPPONENT = "TeamSelectionOfflineOpponent";
        private const string VOLUME_FX = "SFXVolume";
        private const string DIFFICULTY = "Difficulty";
        private const string VOLUME_MASTER = "MasterVolume";
        private const string VOLUME_MUSIC = "MusicVolume";
        private const string BRIGHTNESS = "Brightness";
        private const string IMAGE_EFFECTS = "ImageEffects";
        private const string LASTFINISHEDLEAGUEID = "LastFinishedLeagueId";
        private const string PLAYER_WINS_OFFLINE = "OfflineWins";
        private const string PLAYER_WINS_ONLINE = "OnlineWins";
        private const string OFFLINE_MONEY = "OfflineMoney";
        private const string LEAGUE_TICKETS = "LeagueTickets";
        private const string GUEST = "Guest";
        private const string LOGIN_COUNT = "LoginCount";
        private const string GAME_COINS = "GameCoins";
        public static int LoginCount
        {
            get { return PlayerPrefs.GetInt(LOGIN_COUNT, 0); }
            private set { PlayerPrefs.SetInt(LOGIN_COUNT, value); }
        }

        public static string PlayerName
        {
            get { return PlayerPrefs.GetString(PLAYER_NAME, ""); }
            private set { PlayerPrefs.SetString(PLAYER_NAME, value); }
        }

        public static string Password
        {
            get { return PlayerPrefs.GetString(PASSWORD, ""); }
            private set { PlayerPrefs.SetString(PASSWORD, value); }
        }
        /// <summary>
        /// 0 = playwithai, 1 = passnplay, 2 = oneonone, 3 = leagueGamePlay, 4 = offlinetournament, 5 = playWithFriends
        /// </summary>
        public static int MatchType
        {
            get { return PlayerPrefs.GetInt(MATCH_MODE, 0); }
            set
            {
                int mode = value;
                if (mode == 2 || mode == 3 || mode == 5)
                    FST_Gameplay.IsMultiplayer = true;
                else FST_Gameplay.IsMultiplayer = false;

                FST_Gameplay.IsPWF = mode == 5;

                //safety check here. anytime this mode is set and not for league, clear leagueId
                if (mode != 3)
                    GameManager.CurrentLeagueID = "";

                Debug.Log("FST_Gameplay.IsMultiplayer = " + FST_Gameplay.IsMultiplayer);

                PlayerPrefs.SetInt(MATCH_MODE, mode);
            }
        }

        public static string MatchTypeAsString
        {
            get
            {
                int mt = MatchType;
                return mt == 0 ? "playwithai" : mt == 1 ? "passnplay" : mt == 2 ? "oneonone" : mt == 3 ? "leagueGamePlay" : mt == 4 ? "offlinetournament" : "playWithFriends";
            }
        }

        public static int LeagueTickets
        {
            get { return PlayerPrefs.GetInt(LEAGUE_TICKETS, 0); }
            set { PlayerPrefs.SetInt(LEAGUE_TICKETS, value); }
        }

        public static int GameCoins
        {
            get { return PlayerPrefs.GetInt(GAME_COINS, 0); }
            set { PlayerPrefs.SetInt(GAME_COINS, value); }
        }

        public static int Formation
        {
            get { return PlayerPrefs.GetInt(FORMATION_SELECTION, 0); }
            set { PlayerPrefs.SetInt(FORMATION_SELECTION, value); }
        }

        public static int FormationOpponent
        {
            get { return PlayerPrefs.GetInt(FORMATION_SELECTION_OFFLINE_OPPONENT, 0); }
            set { PlayerPrefs.SetInt(FORMATION_SELECTION_OFFLINE_OPPONENT, value); }
        }

        public static int OfflineWins
        {
            get { return PlayerPrefs.GetInt(PLAYER_WINS_OFFLINE, 0); }
            set { PlayerPrefs.SetInt(PLAYER_WINS_OFFLINE, value); }
        }

        public static int OfflineMoney
        {
            get { return PlayerPrefs.GetInt(OFFLINE_MONEY, 0); }
            set { PlayerPrefs.SetInt(OFFLINE_MONEY, value); }
        }

        public static int OnlineWins
        {
            get { return PlayerPrefs.GetInt(PLAYER_WINS_ONLINE, 0); }
            set { PlayerPrefs.SetInt(PLAYER_WINS_ONLINE, value); }
        }

        public static int Team
        {
            get { return PlayerPrefs.GetInt(TEAM_SELECTION, 0); }
            set { PlayerPrefs.SetInt(TEAM_SELECTION, value); }
        }
        public static int TeamOpponent
        {
            get { return PlayerPrefs.GetInt(TEAM_SELECTION_OFFLINE_OPPONENT, 0); }
            set { PlayerPrefs.SetInt(TEAM_SELECTION_OFFLINE_OPPONENT, value); }
        }

        public static int Difficulty
        {
            get { return PlayerPrefs.GetInt(DIFFICULTY, 0); }
            set { PlayerPrefs.SetInt(DIFFICULTY, value); }
        }

        public static float FxVolume
        {
            get { return PlayerPrefs.GetFloat(VOLUME_FX, 0.75f); }
            set { PlayerPrefs.SetFloat(VOLUME_FX, value); }
        }

        public static float MasterVolume
        {
            get { return PlayerPrefs.GetFloat(VOLUME_MASTER, 1f); }
            set { PlayerPrefs.SetFloat(VOLUME_MASTER, value); }
        }

        public static float MusicVolume
        {
            get { return PlayerPrefs.GetFloat(VOLUME_MUSIC, 0.75f); }
            set { PlayerPrefs.SetFloat(VOLUME_MUSIC, value); }
        }

        public static float Brightness
        {
            get { return PlayerPrefs.GetFloat(BRIGHTNESS, 1f); }
            set { PlayerPrefs.SetFloat(BRIGHTNESS, value); }
        }

        public static bool ImageEffects
        {
            get { return PlayerPrefs.GetInt(IMAGE_EFFECTS, 0) == 1; }
            set { PlayerPrefs.SetInt(IMAGE_EFFECTS, value == true ? 1 : 0); }
        }
        public static string LastFinishedLeagueID
        {
            get { return PlayerPrefs.GetString(LASTFINISHEDLEAGUEID, ""); }
            set { PlayerPrefs.SetString(LASTFINISHEDLEAGUEID, value); }
        }
        public static bool IsLoggedIn { get; private set; } = false;
        public static bool IsGuest
        {
            get { return PlayerPrefs.GetInt(GUEST, 0) == 1; }
            private set { PlayerPrefs.SetInt(GUEST, value == true ? 1 : 0); }
        }

        public static bool IsTimeBased
        {
            get { return PlayerPrefs.GetInt(TIME_BASED, 0) == 1; }
            set { PlayerPrefs.SetInt(TIME_BASED, value == true ? 1 : 0); }
        }

        public static bool IsMutedMic
        {
            get { return PlayerPrefs.GetInt("$isMutedMic", 0) == 1; }
            set { PlayerPrefs.SetInt("$isMutedMic", value == true ? 1 : 0); }
        }

        public static void Login(string username, string password)
        {
            LastFinishedLeagueID = "";
            PlayerName = username;
            Password = password;
            LoginCount++;
            IsGuest = false;
            IsLoggedIn = true;
        }

        public static void LogOut()
        {
            PlayerName = "";
            Password = "";
            IsMutedMic = false;
            Difficulty = 0;
            MasterVolume = 1f;
            FxVolume = 0.75f;
            MusicVolume = 0.75f;
            Brightness = 1f;
            ImageEffects = true;

            //   PlayerPrefs.DeleteKey("user_id");//NOTE: check this before removal
            // PlayerPrefs.DeleteKey("token");//NOTE: checked but double check this before removal

            IsGuest = true;
            IsLoggedIn = false;
        }

        public static void SetDefaults()
        {
            PlayerName = "Player";
            IsMutedMic = false;
            Difficulty = 0;
            MasterVolume = 1f;
            FxVolume = 0.75f;
            MusicVolume = 0.75f;
            Brightness = 1f;
            ImageEffects = true;
        }

        public static void ResetStats()
        {

        }
    }
}
