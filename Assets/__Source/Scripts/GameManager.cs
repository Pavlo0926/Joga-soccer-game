// MatchType 0 - "playwithai"
// MatchType 1 - "passnplay"
// MatchType 2 - "onenone"
// MatchType 3 - "leagueGamePlay" //.. tournament
// MatchType 4 - "offlineLeagueGamePlay" //...offlinetournament
// MatchType 5 - "playWithFriends"


using FastSkillTeam;
using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Jiweman;

public class GameManager : MonoBehaviour
{
    public static string CurrentLeagueID { get; set; } = "";

    private static GameManager m_Instance = null;
    public static GameManager Instance { get { if (m_Instance == null) m_Instance = FindObjectOfType(typeof(GameManager)) as GameManager; return m_Instance; } private set { m_Instance = value; } }

    //NOTE FIX THIS... THIS IS NOT CORRECTLY DONE!
    public static UIController UI_sharedInstance = null;
    public static UIController UIControllerInstance { get { if (UI_sharedInstance == null) UI_sharedInstance = UIController.Instance; return UI_sharedInstance; } }

    public static int SelectedStadium = 0;

    public string RemotePlayerNameString { get; set; } = "Player2";

    #region Inspector Vars
    public GameObject HelpScreen { get; set; } = null;
    public GameObject CreditScreen { get; set; } = null;
    public GameObject ShopScreen { get; set; } = null;
    public GameObject ExitScreen { get; set; } = null;
    public GameObject MenuScreen { get; set; } = null;
    public GameObject QuickPlay { get; set; } = null;
    public GameObject OfflineScreen { get; set; } = null;
    public GameObject AchievementScreen { get; set; } = null;
    public GameObject PauseScreen { get; set; } = null;
    public GameObject RateScreen { get; set; } = null;
    public GameObject SettingScreen { get; set; } = null;
    public GameObject LoadingScreen { get; set; } = null;
    public GameObject PurchasePopups { get; set; } = null;
    public GameObject LevelSelectionScreen { get; set; } = null;
    public GameObject ShopFormationScreen { get; set; } = null;
    public GameObject ChooseOpponentScreen { get; set; } = null;
    public GameObject PlayWithFriendsLeagueScreen { get; set; } = null;
    public GameObject GameOverScreen { get; set; } = null;
    public GameObject SpinWheelScreen { get; set; } = null;
    public GameObject LeagueScreen { get; set; } = null;
    public GameObject UpgradeScreen { get; set; } = null;
    public GameObject RentScreen { get; set; } = null;
    public GameObject PlayerProfileScreen { get; set; } = null;
    public GameObject BrandScreen { get; set; } = null;
    public GameObject LeaderBoardScreen { get; set; } = null;
    public GameObject OfflineLeagueScreen { get; set; } = null;
    public GameObject PlayWithFriendsScreen { get; set; } = null;
    public GameObject InviteFriendsScreen { get; set; } = null;
    public GameObject ChallengeFriendsScreen { get; set; } = null;
    public GameObject SearchUserScreen { get; set; } = null;

    public GameObject loadingPrefab;
    public GameObject PausePrefab;
    public GameObject helpPrefab;
    //    public GameObject DiceRentPanal;
    public GameObject loadingWheelScriptPrefab;
    public GameObject ClubManagementScreen { get; set; } = null;
    public GameObject StadiumSelectionScreen { get; set; } = null;
    public GameObject ClubInfoScreen { get; set; } = null;
    public GameObject OptionScreen { get; set; } = null;
    public GameObject ClubExpensesScreen { get; set; } = null;
    public GameObject JiwemanloginScreen { get; set; } = null;
    public GameObject JiwemanRegistrationScreen { get; set; } = null;
    public GameObject AccountCreatedScreen { get; set; } = null;
    public GameObject SelectCityScreen { get; set; } = null;
    public GameObject PlayerInformationScreen { get; set; } = null;
    public GameObject LoginPanal { get; set; } = null;
    public GameObject TutorialPanel { get; set; } = null;
    public GameObject BettingPanal { get; set; } = null;
    public GameObject ForgetPassword { get; set; } = null;
    public Text PlayernameText { get; set; }
    public Text OpponentNameText { get; set; }
    public Text PlayergoalsText { get; set; }
    public Text OpponentgoalsText { get; set; }
    public Text PlayerWinsText { get; set; }
    public Text PlayerMoneyText { get; set; }

   // private int timeCounter = 0;    //Actual game-time index

    //branding variables
    public int Current_brand { get; set; }
    public Sprite CurrentBg { get; set; }
    public List<Sprite> BG { get; set; } = new List<Sprite>();
    public List<int> Brand_id { get; set; } = new List<int>();
    public GameObject brandPrefab;
    public GameObject BrandParent { get; set; }
    public List<Sprite> Brand_iconImg { get; set; } = new List<Sprite>();

    public Sprite Currenttopbar { get; set; }
    public Sprite Currentdownbar { get; set; }
    public Sprite CurrenttopCornerbar { get; set; }
    public Sprite CurrentdownCornerbar { get; set; }
    public List<string> Brand_type { get; set; } = new List<string>();
    //public GameObject bgObject;
    public List<GameObject> BgObjects { get; set; } = new List<GameObject>();
    public List<GameObject> TopCornerbarObject { get; set; } = new List<GameObject>();
    public List<GameObject> DownCornerbarObject { get; set; } = new List<GameObject>();
    public GameObject TopbarObject { get; set; }
    public GameObject DownbarObject { get; set; }
    //public GameObject topCornerbarObject;
    //public GameObject downCornerbarObject;
    public List<Sprite> Brand_ads { get; set; } = new List<Sprite>();
    public List<Sprite> Topbar { get; set; } = new List<Sprite>();
    public List<Sprite> Downbar { get; set; } = new List<Sprite>();
    public List<Sprite> mtopCornerbar = new List<Sprite>();
    public List<Sprite> mdownCornerbar = new List<Sprite>();
    public List<GameObject> Levels { get; set; } = new List<GameObject>();

    #endregion


    private Text m_PlayerText;
    public Text PlayerText { get { if (!m_PlayerText) m_PlayerText = GameObject.Find("playerText").GetComponent<Text>(); return m_PlayerText; } }
    private Text m_OpponentText;
    public Text OpponentText { get { if (!m_OpponentText) m_OpponentText = GameObject.Find("opponentText").GetComponent<Text>(); return m_OpponentText; } }


    public List<Team> storeAITeamsList { get; set; }
    public List<Team> storeAISecondRoundTeamsList { get; set; }

    //NOTE: convert these to int format
    public bool isPlayerRound1Win { get; set; }
    public bool isPlayerRound2Win { get; set; }
    public bool isPlayerRound3Win { get; set; }
    public bool isPlayerLoseTournament { get; set; }

    public string TournamentLevel;
    public List<string> OnlineTournamentLevel1PlayerID { get; set; }
    public List<string> OnlineTournamentLevel2PlayerID { get; set; }
    public List<string> OnlineTournamentLevel3PlayerID { get; set; }

    //NOTE: convert these to int format
    public bool IsOnlineTournamentRound1Win { get; set; }
    public bool IsOnlineTournamentRound2Win { get; set; }
    public bool IsOnlineTournamentRound3Win { get; set; }
    public bool IsOnlineTournamentLose { get; set; }

    public int GoalsToWin { get; set; } = 3;

    public bool IsRematchOneNOne { get; set; }

    public bool IsBackKeyPressed { get; set; } = false;
    public bool IsGamePause { get; private set; } = false;

    //used privates
    public bool IsOneNOneLeaderBoard { get; private set; } = false;
    private bool exitpopupflag;//for escape button system
    private bool isMenuSpin;
    private float startTime;
    #region MONOBEHAVIOUR CALLBACKS
    public void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        }
    }

    void Start()
    {
        startTime = Time.time;
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        QualitySettings.vSyncCount = 0;

        RegisterGameStates();

        // mCurrent_brand = PlayerPrefs.GetInt ("Current_brand");
    }
    public void AnimatePressed(Transform t, float speed = 1f, Action action = null)
    {
        StartCoroutine(AnimatePress(t, speed, action));
    }
    //cached start scales in case of unfinished routines giving bad start scales on next try.
    private Dictionary<Transform, Vector3> m_StartScales = new Dictionary<Transform, Vector3>();
    private IEnumerator AnimatePress(Transform objectTransform, float speed, Action action = null)
    {
        if (!m_StartScales.TryGetValue(objectTransform, out Vector3 startingScale))
        {
            m_StartScales.Add(objectTransform, objectTransform.localScale);
            startingScale = objectTransform.localScale;
        }

        Vector3 destinationScale = startingScale * 1.1f;        //target scale

        Debug.Log("startingScale: " + startingScale);

        //Scale up
        float t = 0.0f;
        while (t <= 1.0f)
        {
            t += Time.deltaTime * speed;
            objectTransform.localScale = new Vector3(Mathf.SmoothStep(startingScale.x, destinationScale.x, t),
                                                    Mathf.SmoothStep(startingScale.y, destinationScale.y, t),
                                                    objectTransform.localScale.z);
            yield return 0;
        }

        //Scale down
        float r = 0.0f;
        if (objectTransform.localScale.x >= destinationScale.x)
        {
            while (r <= 1.0f)
            {
                r += Time.deltaTime * speed;
                objectTransform.localScale = new Vector3(Mathf.SmoothStep(destinationScale.x, startingScale.x, r),
                                                        Mathf.SmoothStep(destinationScale.y, startingScale.y, r),
                                                        objectTransform.transform.localScale.z);
                yield return 0;
            }
        }

        action?.Invoke();
    }

    private void Update()
    {
        EscapeInput();
    }
    // private void FixedUpdate()
    // {
    //  GameStates._currentState?.Invoke(); //simplify ->FST
    // }

    private void EscapeInput()
    {//NOTE: This is not all accurate, hook this up properly
        if (Input.GetKeyDown(KeyCode.Escape) && !exitpopupflag)
        {
            exitpopupflag = true;
            if (SceneManager.GetActiveScene().name == "MainMenu")
            {
                if (UIControllerInstance.Login_PopUP.activeSelf)
                {
                    UIControllerInstance.Login_PopUP.SetActive(false);
                }
                else if (UIControllerInstance.RegiSter_PopUP.activeSelf)
                {
                    UIControllerInstance.RegiSter_PopUP.SetActive(false);
                }
                else if (GameStates.currentState == GAME_STATE.MAIN_MENU)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.EXIT);
                }
                else if (GameStates.currentState == GAME_STATE.EXIT)
                {
                    if (GameStates.previousState != GAME_STATE.MAIN_MENU)
                        GameStates.SetCurrent_State_TO(GameStates.previousState);
                    else
                        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                // else if (GameStates.currentState == GAME_STATE.OFFLINE)
                // {
                //       GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                // }
                else if (GameStates.currentState == GAME_STATE.LEVELSELECTION)
                {
                    if (FireBasePushNotification.IsPlayWithfriend)
                        return;
                    if (FST_SettingsManager.MatchType == 3)//no backing out of league or realmoney
                    {
                        if (leagueProgressionPanel && leagueProgressionPanel.activeInHierarchy)
                            return;
                        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                    }
                    else GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                }
                else if (GameStates.currentState == GAME_STATE.SELECTFORMATION)
                {
                    if (FST_SettingsManager.MatchType == 3)//no backing out of league or realmoney
                        return;
                    // GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                 // else GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);
                }
                else if (GameStates.currentState == GAME_STATE.SETTINGS)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.ABOUT_US)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.SHOP)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.ACHIEVEMENT)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.OPTION);
                }
                else if (GameStates.currentState == GAME_STATE.CLUBINFOPAGE)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.CLUBEXPENSES)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.CLUBINFOPAGE);
                }
                else if (GameStates.currentState == GAME_STATE.JIWEMANLOGIN)
                {
                    // Debug.Log("Press back");
                    GameStates.SetCurrent_State_TO(GAME_STATE.EXIT);

                }
                else if (GameStates.currentState == GAME_STATE.JIWEMANREGISTRATION)
                {
                    Debug.Log("Press back");
                    GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);

                }
                else if (GameStates.currentState == GAME_STATE.PLAYERINFORMATION)
                {
                    GameStates.SetCurrent_State_TO(GameStates.previousState);
                }
                else if (GameStates.currentState == GAME_STATE.OPTION)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.SPIN)
                {
                    if (isMenuSpin)
                        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                    else
                        GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);

                }
                else if (GameStates.currentState == GAME_STATE.RENT)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);
                }
                else if (GameStates.currentState == GAME_STATE.BRAND)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.LEADERBOARD)
                {
                    if (GameStates.previousState != GAME_STATE.LEVELSELECTION)
                        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                    else
                        GameStates.SetCurrent_State_TO(GameStates.previousState);
                }
                else if (GameStates.currentState == GAME_STATE.PROFILE)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else if (GameStates.currentState == GAME_STATE.UPGRADE)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
            }
            else if (SceneManager.GetActiveScene().name == "InGame")
            {
                if (GameStates.currentState == GAME_STATE.INGAME) { }
                else if (GameStates.currentState == GAME_STATE.EXIT)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.RESUME);
                }
                else if (GameStates.currentState == GAME_STATE.PAUSEMENU)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.RESUME);
                }
            }
            IsBackKeyPressed = true;
        }
        else
        {
            IsBackKeyPressed = false;
        }
        if (Input.GetKeyUp(KeyCode.Escape) && exitpopupflag)
        {
            exitpopupflag = false;
        }
    }
    #endregion

    #region LOGIN
    private void ShowJiwemanLogin()
    {
        if (!JiwemanloginScreen)
        {
            Debug.LogError("FST Error Notification -> No JiwemanLoginScreen has been assigned, its either missing from scene, or does not contain the script that assigns instance to object");
            return;
        }

        DisableAllScreens();

        Debug.Log("login screen");

        JiwemanloginScreen.SetActive(true);
        DisableLogin_bar();
    }

    private void ShowForgetPassword()
    {
        DisableAllScreens();

        ForgetPassword.transform.localScale = Vector3.one;
        DisableLogin_bar();
    }

    private void ShowJiwemanRegistration()
    {
        DisableAllScreens();

        JiwemanRegistrationScreen.transform.localScale = Vector3.one;

        DisableLogin_bar();
    }

    private void ShowAccountCreated()
    {
        DisableAllScreens();

        AccountCreatedScreen.transform.localScale = Vector3.one;
        // Debug.Log("Welcome");
        DisableLogin_bar();
    }
    #endregion

    #region LOADING
    private void Loading()
    {
        DisableAllScreens();
        if (LoadingScreen == null)
        {
            LoadingScreen = (GameObject)Instantiate(loadingPrefab);
            Destroy(LoadingScreen);
            LoadingScreen = null;
            // Debug.Log("ShowLoginPanal-2");
            // ShowJiwemanLogin();
        }
        DisableLogin_bar();
    }

    private void LoadLevelWithLoadingScreen(string levelName)
    {
        Debug.Log("LoadLevel Called with loading coroutine");

        if (levelName == "InGame")
        {
            GameStates.SetCurrent_State_TO(GAME_STATE.LOADING);
            IsGamePause = true;
            StartCoroutine(loadingDelay(0.5f, levelName));
        }
        else
        {
            // Debug.Log("debug");
            IsGamePause = true;
            StartCoroutine(loadingDelay(0.0001f, levelName));
        }
        if (FST_Gameplay.IsMultiplayer)
            PhotonNetwork.LoadLevel(levelName);
        else
            SceneManager.LoadScene(levelName + "");
    }

    public void LoadLevel(string LevelName)
    {
        Debug.Log("LoadLevel Called");

        if (FST_Gameplay.IsMultiplayer)
            PhotonNetwork.LoadLevel(LevelName);
        else
            SceneManager.LoadScene(LevelName + "");
        IsGamePause = false;
    }

    private IEnumerator loadingDelay(float waitSec, string levelName)
    {
        // Debug.Log("Level " + levelName);

        yield return new WaitForSeconds(waitSec);
        //if (PlayerPrefs.HasKey ("firsttimehelp")) {

        //yield return new WaitForSeconds(0.2f);
        if (LoadingScreen != null)
        {
            Destroy(LoadingScreen);
            LoadingScreen = null;
        }
        if (levelName != "MainMenu")
        {
            IsGamePause = true;
            GameStates.SetCurrent_State_TO(GAME_STATE.INGAME);

            Debug.Log("loading delay complete, Gamestate > INGAME");

        }
        else GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
    }
    #endregion

    #region FORMATION functions

    private void SelectFormation()
    {
        DisableAllScreens();
        DisableLogin_bar();
        FST_FormationsManager.Instance.OpenOrCloseFormationPanel(true);
    }

    private void SelectFormation2()//likely we dont need this anymore TODO: check
    {

        DisableAllScreens();
        DisableLogin_bar();
        // Debug.Log("Player formation selection complete");

    }

    private void ShopFormation()
    {
        DisableAllScreens();
        DisableLogin_bar();
        ShopFormationScreen.transform.localScale = Vector3.one;
    }

    #endregion

    #region MATCHMAKING 
    //note these are for reference and will be moved later
    [Serializable]
    public class MatchFindData
    {
        public string playerOneUsername;
        public string playerTwoUsername;
        public string matchType;
        public string roomName;
        public bool isRematch;
        public string leagueId = "";
    }

    [Serializable]
    public class MatchEndData
    {
        public string winnerName;
        public string playerOneUserName;
        public string playerTwoUserName;
        public int playerOneGoal;
        public int playerTwoGoal;
        public string roomName;
        public bool matchType;//oneonone, leagueGamePlay, playWithFriends
        public int matchId;
        public string leagueId = ""; // this is only for LEAGUEGAMEPLAY
    }

    private void ChooseOpponent()
    {

        DisableAllScreens();
        // ChooseOpponentScreen.SetActive(true);
        ChooseOpponentScreen.transform.localScale = Vector3.one;


        if (PlayerText)
            PlayerText.text = FST_SettingsManager.PlayerName;
        else Debug.LogError("ChooseOpponent() > No player text found!");

        if (ScrollImage.Instance)
        {
            ScrollImage.Instance.gameObject.SetActive(true);
            ScrollImage.Instance.isRotateImage = false;
        }
        else Debug.LogError("ChooseOpponent() > No ScrollImage.Instance available!");

        if (OpponentText)
            OpponentText.text = "Waiting...";
        else Debug.LogError("ChooseOpponent() > No opponent text found!");

        UIControllerInstance.FindOpponentRetryBtn.SetActive(false);
        ScrollImage.Instance.isRotateImage = false;
        if (FST_SettingsManager.MatchType == 4)
        {
            Image oppontDefaultImage = GameObject.Find("oppontDefaultImage").GetComponent<Image>();
            if (!isPlayerRound1Win)
            {
                oppontDefaultImage.sprite = OfflineTournamentManager.Instance.aiTempTeamsList[0].playerImage;
                OpponentText.text = "" + OfflineTournamentManager.Instance.aiTempTeamsList[0].teamName;
            }
            else if (isPlayerRound1Win && !isPlayerRound2Win)
            {
                oppontDefaultImage.sprite = OfflineTournamentManager.Instance.aiSecondRoundTeamsList[0].playerImage;
                OpponentText.text = "" + OfflineTournamentManager.Instance.aiSecondRoundTeamsList[0].teamName;
            }
            else if (isPlayerRound1Win && isPlayerRound2Win && !isPlayerRound3Win)
            {
                oppontDefaultImage.sprite = OfflineTournamentManager.Instance.aiThirdTeam.playerImage;
                OpponentText.text = "" + OfflineTournamentManager.Instance.aiThirdTeam.teamName;
            }
        }

        //     if (FST_SettingsManager.MatchType != 3)//old for league
        StartCoroutine("AfterOpponentFind");

        DisableLogin_bar();
    }

    /// <summary>
    /// This will determine if a GameScene can be loaded
    /// </summary>
    /// <returns>true if offline, true if all rules are passed online </returns>
    private bool CanLoadGameplayScene()
    {
        if (!FST_Gameplay.IsMultiplayer)
            return true;

        if (!PhotonNetwork.IsConnectedAndReady)
            return false;

        if (PhotonNetwork.CurrentRoom == null)
        {
            // Debug.Log("not in room yet....");
            return false;
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount < (byte)2)
        {
            //   Debug.Log("not enough players yet....");
            return false;
        }

        Player[] players = PhotonNetwork.PlayerList;

        for (int i = 0; i < players.Length; i++)
        {
            if (OpponentText)// now is a good time to check out the player name and update the opponent text in the 'Finding Opponent' screen
                if (players[i] != PhotonNetwork.LocalPlayer)
                    OpponentText.text = players[i].NickName;

            if (players[i].CustomProperties.TryGetValue(FST_PlayerProps.READY, out object o))
            {
                if ((bool)o == false)
                {
                    //   Debug.Log(players[i].NickName + " is not ready");
                    return false;
                }
                //  else Debug.Log(players[i].NickName + " is ready");
            }
            else
            {
                //  Debug.LogWarning(players[i].NickName + " HAS NO CUSTOM PROPS FOR READY SET");
                return false;
            }
        }

        Debug.Log("CanLoadGameplayScene() is true, lockdown buttons..");
        PhotonNetwork.CurrentRoom.IsOpen = false;  //  PhotonNetwork.CurrentRoom.RemovedFromList = true;

        //lock player from the buttons to exit or retry
        UIControllerInstance.FindOpponentRetryBtn.SetActive(false);
        UIControllerInstance.CloseChooseOppoBtn.SetActive(false);

        return true;
    }

    public void CancelOpponentFind()
    {
        isRestartRoomSearch = false;

        //StopAllCoroutines();
        StopCoroutine("AfterOpponentFind");
        StopCoroutine("SearchingRestartCheck");


        FST_MPConnection.Instance.LeaveRoom(true, false);//fresh scene for now
                                                         // GameStates.SetCurrent_State_TO(GameStates.previousState);
        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);

    }

    public void RestartOpponentFind(Button b)
    {
        isRestartRoomSearch = true;
        if (b != null)
        {// Destroy(b.gameObject);
            b.onClick.RemoveAllListeners();
            b.gameObject.SetActive(false);
        }
    }

    private IEnumerator SearchingRestartCheck()
    {
        isRestartRoomSearch = false;
        yield return new WaitUntil(() => PhotonNetwork.InRoom);
        m_MaxRoomSearchTime = Time.time + FST_MPConnection.Instance.MaximumRoomSearchTime;
        yield return new WaitUntil(() => Time.time > m_MaxRoomSearchTime);

        if (FST_Gameplay.CurrentLevelName == "InGame")
            yield break;

        UIControllerInstance.FindOpponentRetryBtn.SetActive(true);

        Button b = UIControllerInstance.FindOpponentRetryBtn.GetComponent<Button>();
        b.onClick.AddListener(() => RestartOpponentFind(b));
    }
    public bool CanJoinRooms { get; set; } = false;//PhotonNetwork.IsConnectedAndReady was playing up, we will use our own bool
    private bool isRestartRoomSearch = false;
    private float m_MaxRoomSearchTime = 0f;

    private IEnumerator AfterOpponentFind()
    {
        // FST_Button_CancelOpponentFind.Instance.gameObject.SetActive(string.IsNullOrEmpty(CurrentLeagueID));

        bool wasMP = FST_Gameplay.IsMultiplayer;
       // FST_MPDebug.Log("Start AfterOpponentFind(), MP = " + wasMP + ", canjoinrooms = " + CanJoinRooms);
        FST_MPDebug.Log("Start AfterOpponentFind(), ClientState: " + PhotonNetwork.NetworkClientState);
      //  Debug.Log("Start AfterOpponentFind(), MP = " + wasMP + ", canjoinrooms = " + CanJoinRooms);
        Debug.Log("Start AfterOpponentFind(), ClientState: " + PhotonNetwork.NetworkClientState);

        if (FST_Gameplay.IsMultiplayer)
        {
            if (PhotonNetwork.InRoom && !FST_Gameplay.IsPWF)
                yield return new WaitUntil(() => PhotonNetwork.LeaveRoom(false));

            if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
                FST_MPConnection.Instance.Connect();

            if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToGameServer)
                FST_MPConnection.Instance.Connect();

            if (PhotonNetwork.NetworkClientState != ClientState.Leaving)
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.READY, true }, { FST_PlayerProps.LOADED_LEVEL, false } });

          /*  //just in case
            while (!PhotonNetwork.InLobby)
            {
                if (PhotonNetwork.NetworkClientState == ClientState.JoinedLobby)
                    yield break;

                if (CanJoinRooms)
                    yield break;

                if (!FST_MPConnection.Connected)
                    yield break;//fst mpconnection will be handling this already

                Debug.LogWarning("opponent join issue");
                if (PhotonNetwork.NetworkClientState == ClientState.Joining
                    || PhotonNetwork.NetworkClientState == ClientState.JoiningLobby
                    || PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer
                    || PhotonNetwork.NetworkClientState == ClientState.Authenticating
                    || PhotonNetwork.NetworkClientState == ClientState.Authenticated
                    || PhotonNetwork.NetworkClientState == ClientState.PeerCreated
                    || PhotonNetwork.NetworkClientState == ClientState.Leaving
                    || PhotonNetwork.NetworkClientState == ClientState.DisconnectingFromMasterServer
                    || !PhotonNetwork.IsConnectedAndReady)
                    yield return 0;

                if (PhotonNetwork.JoinLobby(FST_MPConnection.lobby))
                    yield break;

                yield return 0;
            }*/

            yield return new WaitUntil(() => CanJoinRooms);

            if (!FST_Gameplay.IsPWF)//if pwf or league we are already joining or in, skip this
            {
                if (PhotonNetwork.NetworkClientState != ClientState.Joining)
                    FST_MPConnection.Instance.TryJoinRoom();
            }
            else Debug.Log("IsPWF or league, will skip creating room as this is already in process");

            StartCoroutine("SearchingRestartCheck");
        }

        yield return new WaitUntil(() => CanLoadGameplayScene() || isRestartRoomSearch);

        StopCoroutine("SearchingRestartCheck");

        if (isRestartRoomSearch)// will only trigger if online
        {
            CanJoinRooms = false;
            yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.READY, false } }));//NOTE: check if this is needed (in case a player connect before we have actually left the room)
            yield return new WaitUntil(() => PhotonNetwork.LeaveRoom(false));
            yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
            yield return new WaitUntil(() => CanJoinRooms);
            Debug.Log("Opponent Find timeout, Left Room");
            StartCoroutine("AfterOpponentFind");// restart the routine
            yield break;//by now we should be in a lobby, so break out and use the freshly started routine, FST_MPConnection will relentlessy try to find a new room automatically
        }
        FST_Button_CancelOpponentFind.Instance.gameObject.SetActive(false);
        UIControllerInstance.FindOpponentRetryBtn.SetActive(false);
        //ScrollImage.Instance.gameObject.SetActive(false);
        ScrollImage.Instance.isRotateImage = true;
        yield return new WaitForSeconds(2f);

        if (!FST_Gameplay.IsMultiplayer && wasMP)
        {
            Debug.Log("no room to join, user likely cancelled search");
            yield break;// user canceled finding opponent or was disconnected
        }
        else
        {
            if (FST_Gameplay.IsMultiplayer && PhotonNetwork.CurrentRoom != null)
            {
                //temporary storing data from both players --> russel
                Joga_Data.RoomName = PhotonNetwork.CurrentRoom.Name;
                Joga_Data.MatchType = FST_SettingsManager.MatchType == 2 ? Joga_MatchType.oneonone : FST_SettingsManager.MatchType == 3 ? Joga_MatchType.leagueGamePlay : Joga_MatchType.playWithFriends;

            //    Debug.Log("Room name : " + Joga_Data.RoomName);
            //    Debug.Log("Match Type : " + Joga_Data.MatchType);

                if (FST_Gameplay.IsMaster)
                {
                    MatchFindData data = new MatchFindData { playerOneUsername = FST_SettingsManager.PlayerName, playerTwoUsername = RemotePlayerNameString, matchType = FST_SettingsManager.MatchTypeAsString, roomName = PhotonNetwork.CurrentRoom.Name, isRematch = false };
                    string newMatchData = JsonUtility.ToJson(data);
                    Joga_NetworkManager.Instance.CreateNewMatchRequest(data.playerOneUsername, data.playerTwoUsername, data.roomName, data.isRematch, FST_SettingsManager.MatchTypeAsString, CurrentLeagueID);
                }
            }

            Debug.Log("loading gameplay!");
            LoadGameplayScene();
        }
    }

    public void CallRematchFunction()
    {
        StartCoroutine("RematchOneNoneMatch");
        Debug.Log("CallRematchFunction()");
    }

    private IEnumerator RematchOneNoneMatch()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.REMATCH, 1 }, { FST_PlayerProps.LOADED_LEVEL, false } });
        Debug.Log("Try rematch");

        yield return new WaitUntil(() => RematchResult());

        if (IsRematchOneNOne)
        {
            if (FST_Gameplay.IsMultiplayer)
            {
                if (FST_Gameplay.IsMaster)
                {
                    MatchFindData data = new MatchFindData { playerOneUsername = FST_SettingsManager.PlayerName, playerTwoUsername = RemotePlayerNameString, matchType = FST_SettingsManager.MatchType.ToString(), roomName = PhotonNetwork.CurrentRoom.Name, isRematch = IsRematchOneNOne };
                    Joga_NetworkManager.Instance.CreateNewMatchRequest(data.playerOneUsername, data.playerTwoUsername, data.roomName, data.isRematch, FST_SettingsManager.MatchTypeAsString, CurrentLeagueID);
                }

                yield return new WaitForSecondsRealtime(1.5f);//give enough time to display ui changes to both players before loading the scene.

                PhotonNetwork.LoadLevel("InGame");
            }

            IsRematchOneNOne = false;
        }
        else
        {
            yield return new WaitUntil(() => PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.REMATCH, 0 } }));
            FST_MPConnection.Instance.LeaveRoom(false, false);
        }
    }

    private bool RematchResult()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
        {
            IsRematchOneNOne = false;
            Debug.Log("opponent left");
            return true;
        }
        else
        if (PhotonNetwork.IsMasterClient)
        {
            if (GlobalGameManager.Instance.RemotePlayer == null)
            {
                IsRematchOneNOne = false;
                Debug.Log("remote left");
                return true;
            }

            if (GlobalGameManager.Instance.RemotePlayer.CustomProperties.TryGetValue(FST_PlayerProps.REMATCH, out object o))
            {
                RemotePlayerNameString = GlobalGameManager.Instance.RemotePlayer.NickName;
                IsRematchOneNOne = (int)o == 1;
                Debug.Log("rematch = " + IsRematchOneNOne);
                if (IsRematchOneNOne)
                { //set diplay prompt
                    GlobalGameManager.Instance.RematchTextPlayer2.SetActive(true);
                    return true;
                }

            }
        }
        else
        {
            if (PhotonNetwork.MasterClient.CustomProperties.TryGetValue(FST_PlayerProps.REMATCH, out object o))
            {
                RemotePlayerNameString = PhotonNetwork.LocalPlayer.NickName;
                IsRematchOneNOne = (int)o == 1;
                Debug.Log("rematch = " + IsRematchOneNOne);
                if (IsRematchOneNOne)
                { //set display prompt
                    GlobalGameManager.Instance.RematchTextPlayer.SetActive(true);
                    return true;
                }

            }
        }
        return false;
    }

    #endregion

    private string UpperFirst(string text)
    {
        return char.ToUpper(text[0]) +
              ((text.Length > 1) ? text.Substring(1).ToLower() : string.Empty);
    }

    private void SetGameStateAfter(GAME_STATE state, float sec)
    {
        if (Time.time - startTime >= sec)
            GameStates.SetCurrent_State_TO(state);
    }


    public void CleanUnusedResources()
    {
        Resources.UnloadUnusedAssets();
        System.GC.Collect();
    }

    public void StartSpin()
    {
        //        Instantiate (loadingWheelScriptPrefab);
    }

    public void StopSpin()
    {
        //if(LoadingWheelScript.SharedInstance!=null)
        //Destroy (LoadingWheelScript.SharedInstance.gameObject);
    }
    private GameObject leagueProgressionPanel = null;
    public void AssignInstanceToObject(string objectname, GameObject assignedObject)
    {
        objectname = objectname.ToLower();

        if (objectname.Contains("mainmenu"))
            MenuScreen = assignedObject;
        else if (objectname.Contains("leagueprogressionpanel"))
            leagueProgressionPanel = assignedObject;
        else if (objectname.Contains("help"))
            HelpScreen = assignedObject;
        else if (objectname.Contains("exit"))
            ExitScreen = assignedObject;
        else if (objectname.Contains("offline"))
            OfflineScreen = assignedObject;
        else if (objectname.Contains("setting"))
            SettingScreen = assignedObject;
        else if (objectname.Contains("challengefriends"))
            ChallengeFriendsScreen = assignedObject;
        else if (objectname.Contains("searchuser"))
            SearchUserScreen = assignedObject;
        else if (objectname.Contains("levelselection"))
            LevelSelectionScreen = assignedObject;
        else if (objectname.Contains("chooseopponent"))
            ChooseOpponentScreen = assignedObject;
        else if (objectname.Contains("quickplay"))
            QuickPlay = assignedObject;
        else if (objectname.Contains("playerprofile"))
            PlayerProfileScreen = assignedObject;
        else if (objectname.Contains("stadiumselection"))
            StadiumSelectionScreen = assignedObject;
        else if (objectname.Contains("optionscreen"))
            OptionScreen = assignedObject;
        else if (objectname.Contains("jiwemanloginpanel"))
            JiwemanloginScreen = assignedObject;
        else if (objectname.Contains("jiwemanregistrationpanel"))
            JiwemanRegistrationScreen = assignedObject;
        else if (objectname.Contains("tutoialpanel"))
            TutorialPanel = assignedObject;
        else if (objectname.Contains("forgetpasswordpanel"))
            ForgetPassword = assignedObject;
        else if (objectname.Contains("shopformation"))
            ShopFormationScreen = assignedObject;
        else if (objectname.Contains("facebookfriendstournament"))
            PlayWithFriendsLeagueScreen = assignedObject;
        else if (objectname.Contains("achievement"))
            AchievementScreen = assignedObject;
        else if (objectname.Contains("rpshopscreen"))
            ShopScreen = assignedObject;
        else if (objectname.Contains("credit"))
            CreditScreen = assignedObject;
        else if (objectname.Contains("pause"))
            PauseScreen = assignedObject;
        else if (objectname.Contains("rateus"))
            RateScreen = assignedObject;
        else if (objectname.Contains("gameover"))
            GameOverScreen = assignedObject;
        else if (objectname.Contains("spinwhile"))
            SpinWheelScreen = assignedObject;
        else if (objectname.Contains("secondleagues"))
            OfflineLeagueScreen = assignedObject;
        else if (objectname.Contains("league"))
            LeagueScreen = assignedObject;
        else if (objectname.Contains("playwithfacebook"))
            PlayWithFriendsScreen = assignedObject;
        else if (objectname.Contains("invitefriends"))
            InviteFriendsScreen = assignedObject;
        else if (objectname.Contains("loading"))
        {
            if (LoadingScreen != null)
            {
                Destroy(LoadingScreen);
                LoadingScreen = null;
            }
            LoadingScreen = assignedObject;
        }
        else if (objectname.Contains("upgrades"))
            UpgradeScreen = assignedObject;
        else if (objectname.Contains("rentscreen"))
            RentScreen = assignedObject;
        else if (objectname.Contains("brandscreen"))
            BrandScreen = assignedObject;
        else if (objectname.Contains("leaderboardmain"))
            LeaderBoardScreen = assignedObject;
        else if (objectname.Contains("clubmanagement"))
            ClubManagementScreen = assignedObject;
        else if (objectname.Contains("clubinfopage"))
            ClubInfoScreen = assignedObject;
        else if (objectname.Contains("clubexpensescreen"))
            ClubExpensesScreen = assignedObject;
        else if (objectname.Contains("accountcreated"))
            AccountCreatedScreen = assignedObject;
        else if (objectname.Contains("selectcity"))
            SelectCityScreen = assignedObject;
        else if (objectname.Contains("informationplayer"))
            PlayerInformationScreen = assignedObject;
        else if (objectname.Contains("loginpanal"))
        {

            LoginPanal = assignedObject;
            Debug.Log("ShowLoginPanal-1");
            ShowLoginPanal();

        }
        else if (objectname.Contains("bettingcompany"))
            BettingPanal = assignedObject;
    }

    public void RegisterGameStates()
    {
        GameStates.RegisterStates(GAME_STATE.MAIN_MENU, MainMenu);
        GameStates.RegisterStates(GAME_STATE.QUICKPLAY, QuickPlay1);
        //  GameStates.RegisterStates(GAME_STATE.OFFLINE, Offline);
        GameStates.RegisterStates(GAME_STATE.SHOP, InAppState);
        GameStates.RegisterStates(GAME_STATE.HELP, Help1);
        GameStates.RegisterStates(GAME_STATE.INGAME, InGame);
        GameStates.RegisterStates(GAME_STATE.LOADING, Loading);
        GameStates.RegisterStates(GAME_STATE.LEVELSELECTION, LevelSelection);
        GameStates.RegisterStates(GAME_STATE.CHOOSEOPPONENT, ChooseOpponent);

        GameStates.RegisterStates(GAME_STATE.SELECTFORMATION, SelectFormation);

        GameStates.RegisterStates(GAME_STATE.SHOPFORMATION, ShopFormation);

        GameStates.RegisterStates(GAME_STATE.SECONDSELECTFORMATION, SelectFormation2);
        GameStates.RegisterStates(GAME_STATE.ACHIEVEMENT, Achievement);
        GameStates.RegisterStates(GAME_STATE.ABOUT_US, AboutUs);
        GameStates.RegisterStates(GAME_STATE.SETTINGS, Settings);
        GameStates.RegisterStates(GAME_STATE.SPIN, Spin);
        GameStates.RegisterStates(GAME_STATE.RENT, ShowRent);
        GameStates.RegisterStates(GAME_STATE.UPGRADE, ShowUpgrade);
        GameStates.RegisterStates(GAME_STATE.LEAGUE, League);
        GameStates.RegisterStates(GAME_STATE.PLAYWITHFRIENDS, PlayWithFriends);
        GameStates.RegisterStates(GAME_STATE.INVITEFRIENDS, ShowInviteFriendsScreen);
        GameStates.RegisterStates(GAME_STATE.CHALLENGEFRIENDS, ShowChallengeFriendsScreen);
        GameStates.RegisterStates(GAME_STATE.SEARCHUSER, ShowSearchUserScreen);
        GameStates.RegisterStates(GAME_STATE.PAUSEMENU, PauseMenu);
        GameStates.RegisterStates(GAME_STATE.RESUME, Resume);
        //    GameStates.RegisterStates(GAME_STATE.GAMEDECISION, GameDecision);
        GameStates.RegisterStates(GAME_STATE.TUTORIAL, TutorialDone);
        GameStates.RegisterStates(GAME_STATE.EXIT, ExitPopup);
        GameStates.RegisterStates(GAME_STATE.RATEUS, Rateus);
        GameStates.RegisterStates(GAME_STATE.OFFLINELEAGUE, OfflineLeague);
        GameStates.RegisterStates(GAME_STATE.BRAND, ShowBrand);
        GameStates.RegisterStates(GAME_STATE.PROFILE, ShowPlayerProfile);
        GameStates.RegisterStates(GAME_STATE.CLUBINFOPAGE, ShowClubInfoPage);
        GameStates.RegisterStates(GAME_STATE.OPTION, ShowOption);
        GameStates.RegisterStates(GAME_STATE.CLUBEXPENSES, ShowClubExpenses);
        GameStates.RegisterStates(GAME_STATE.JIWEMANLOGIN, ShowJiwemanLogin);
        GameStates.RegisterStates(GAME_STATE.JIWEMANREGISTRATION, ShowJiwemanRegistration);
        GameStates.RegisterStates(GAME_STATE.REGISTRATIONSUCCESS, ShowAccountCreated);
        GameStates.RegisterStates(GAME_STATE.SELECTCITY, ShowSelectCity);
        GameStates.RegisterStates(GAME_STATE.PLAYERINFORMATION, ShowPlayerInformation);
        GameStates.RegisterStates(GAME_STATE.BETTING_COMPANY, ShowBettingPanal);
        GameStates.RegisterStates(GAME_STATE.CLUBMANAGEMENT, ClubManagementPanel);
        GameStates.RegisterStates(GAME_STATE.LEADERBOARD, ShowLeaderBoard);
        GameStates.RegisterStates(GAME_STATE.SEASONLEADERBOARD, ShowSeasonLeaderBoard);
        GameStates.RegisterStates(GAME_STATE.STADIUMSELECTION, StadiumSelectionPanel);
        GameStates.RegisterStates(GAME_STATE.FORGETPASSWORD, ShowForgetPassword);
    }


    //NOTE: MOVE TO FST SETTINGS MANAGER
    public void UpdateCoins(int coins)
    {
        FST_SettingsManager.GameCoins += coins;
    }

    public void MainMenuEvents(string eventName)
    {
        switch (eventName)
        {
            case "welcome":
                UIControllerInstance.Welcome_msg.SetActive(false);
                TutorialON();
                break;
            case "info":
                // UIControllerInstance.Welcome_msg.SetActive(false);
                TutorialON();
                break;

            case "spin":
                isMenuSpin = true;
                GameStates.SetCurrent_State_TO(GAME_STATE.SPIN);
                break;

            case "home":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "virtualquickplay":
                SelectedStadium = 0;
                UIControllerInstance.ActiveLoading2Panel();
                FST_SettingsManager.MatchType = 2; //one on one
                GoalsToWin = 3;
                ActiveLevelsScreen("virtualquickplay");
                UIControllerInstance.DeactiveLoading2Panel(0.5f);
                GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                break;

            case "league":
                if (FST_SettingsManager.IsGuest)
                {
                    // SSTools.ShowMessage("Please Login with Jiweman user first", SSTools.Position.bottom, SSTools.Time.twoSecond);
                    UIControllerInstance.RegiSter_PopUP.SetActive(true);
                }
                else
                {
                    //Debug.Log("LEAGUE PLAY");

                    FST_SettingsManager.MatchType = 3; //league
                    ActiveLevelsScreen("leagueplay");
                    GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                    FST_League_Handler.Instance.CallLeague();
                }
                break;

            case "realmoneyquickplay":

                //Debug.Log("REAL MONEY QUICKPLAY");

                SelectedStadium = 0;
                FST_SettingsManager.MatchType = 2; //----------------------------For one o one Any Other Player------//
                ActiveLevelsScreen("realmoneyquickplay");
                // MainSocketConnection.instance.CloseConnection ();
                // MainSocketConnection.Instance.OneNOneConnect();
                // Debug.Log("ActiveLoading2Panel-10");
                //    UIControllerInstance.ActiveLoading2Panel();
                break;

            case "virtualtournament":                       //----------------------------For Online Tournament------//
                SelectedStadium = 0;
                FST_SettingsManager.MatchType = 3;
                ActiveLevelsScreen("virtualtournament");

              //  Debug.Log("VIRTUAL TOURNAMENT");
                // MainSocketConnection.instance.CloseConnection ();
                // MainSocketConnection.instance.TourConnect();
                //  UIControllerInstance.ActiveLoading2Panel();
                GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                break;

            case "realmoneytournament":
                SelectedStadium = 0;
                FST_SettingsManager.MatchType = 3;
                ActiveLevelsScreen("realmoneytournament");
              //  Debug.Log("REAL MONEY TOURNAMENT");
                // MainSocketConnection.instance.CloseConnection ();
                //MainSocketConnection.instance.TourConnect();
                UIControllerInstance.ActiveLoading2Panel();
                GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                break;

            case "playwithfriends":
                SelectedStadium = 0;
                // Debug.Log("ActiveLoading2Panel-11");
                // UIControllerInstance.ActiveLoading2Panel();
                GoalsToWin = 3;
                FST_SettingsManager.MatchType = 5; //----------------------------For one on one With facebook Friend------//  

                //only until new menu system get implement
                if (!Joga_FriendsManager.Instance.isEditMode)
                    Joga_FriendsManager.Instance.EditButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);

                Joga_FriendsManager.Instance.ParentObject.gameObject.SetActive(true);

                //ActiveLevelsScreen("playwithfriends");

                ActiveLevelsScreen("virtualquickplay");

                Debug.Log("FireBasePushNotification.isPlayWithFriend = " + FireBasePushNotification.IsPlayWithfriend);

                if (FireBasePushNotification.IsPlayWithfriend == true)
                {
                    Joga_FriendsManager.Instance.ResetFriendList();
                    UIControllerInstance.Mainscreen_Logo.SetActive(false);
                    GameStates.SetCurrent_State_TO(GAME_STATE.CHALLENGEFRIENDS);
                }
                else
                {
                    if (UIControllerInstance.CloseFormationScreenBtn)
                        UIControllerInstance.CloseFormationScreenBtn.SetActive(false);
                    if (UIControllerInstance.CloseChooseOppoBtn)
                        UIControllerInstance.CloseChooseOppoBtn.SetActive(false);
                    if (UIControllerInstance.LevelselectionCloseBtn)
                        UIControllerInstance.LevelselectionCloseBtn.SetActive(false);
                }

                UIControllerInstance.DeactiveLoading2Panel(1f);

                break;

            case "offlinebtn"://unused...
                //  UIControllerInstance.ActiveLoading2Panel();
                //  GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINE);
                break;

            case "playwithaibtn": //----------------------------For Player Vs AI screen------//
                GoalsToWin = 3;
                SelectedStadium = 0;
                QuickPlay.GetComponentInChildren<FST_Button_SelectStadium>().UpdateDisplayText(0);
                FST_SettingsManager.MatchType = 0;            //set Player Vs AI game mode to fetch later in "Game" scene  
                FST_FormationsManager.Instance.SetFormationIndicatorForAIMatchType();
                GameStates.SetCurrent_State_TO(GAME_STATE.QUICKPLAY);

                break;

            case "offlinetournament": //----------------------------For show Offline tournament levelselection screen------//
                GoalsToWin = 3;
                SelectedStadium = 0;
                FST_SettingsManager.MatchType = 4;
                ActiveLevelsScreen("offlinetournament");
                OfflineTournamentManager.Instance.AssignFirstRound();
                GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);

                break;

            case "playpass":  //----------------------------For show Player vs Player------//
                GoalsToWin = 3;
                SelectedStadium = 0;
                FST_SettingsManager.MatchType = 1;

                PlayerPrefs.DeleteKey("gametype");//TODO: do we even use this???

                FST_SettingsManager.IsTimeBased = false;

                GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                break;

            case "formation_online":
                if (FST_SettingsManager.MatchType == 3 || FST_SettingsManager.MatchType == 5)
                    GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                else
                    GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                break;

            case "formation":
                    GameStates.SetCurrent_State_TO(GAME_STATE.SECONDSELECTFORMATION);
                break;

            case "formation2":
                LoadGameplayScene();
                break;

            case "gamesoccer": //----------------------QUICK PLAY LEVELS---------------------------//
                SelectedStadium = 0;
                FST_SettingsManager.IsTimeBased = true; // Time base ON.
                GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            case "greensoccer":
                SelectedStadium = 0;
                GoalsToWin = 3;
                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                if (FST_Gameplay.IsPWF)
                    GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                else
                    GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            case "subleague":
                FST_SettingsManager.MatchType = 3;
                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                break;

            case "jogabonito":
                SelectedStadium = 0;
                FST_SettingsManager.IsTimeBased = true;//time based ON.
                GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            case "brandjogabonito":
                SelectedStadium = 0;
                GoalsToWin = 3;
                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            /*
                        case "tgamesoccer":                                                         // ----------------- TOURNAMENT LEVELS-----------------------//

                            //            goalCounter = 1;                                      
                            //            PlayerPrefs.SetInt ("Gamegoals", goalCounter);                    //save the game-time value
                            //                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                            //            TournamentLevel = "1";
                            //            MainSocketConnection.instance.RegistrationForTour (TournamentLevel);
                            //            MainSocketConnection.instance.isTournamentRealMoney = false;
                            //            GameStates.SetCurrent_State_TO (GAME_STATE.LEAGUE);

                            goalCounter = 1;
                            PlayerPrefs.SetInt("Gamegoals", goalCounter);                    //save the game-time value

                             FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                            TournamentLevel = "1";

                            WebServicesHandler.SharedInstance.GetTournamentMatchId("14", "100");

                            MainSocketConnection.instance.isTournamentRealMoney = false;
                            MainSocketConnection.instance.isTournamentBrandRealMoney = false;
                            GameStates.SetCurrent_State_TO(GAME_STATE.LEAGUE);
                            break;

                        case "tgreensoccer":

                            goalCounter = 1;
                            PlayerPrefs.SetInt("Gamegoals", goalCounter);                    //save the game-time value

                             FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                            TournamentLevel = "1";

                            GameManager.SharedInstance.PlayerRPAmountValue -= 100;

                            WebServicesHandler.SharedInstance.GetUpdateRPUserBettingCompany(PlayerPrefs.GetInt("BettingCompanyID").ToString(), GameManager.SharedInstance.PlayerRPAmountValue.ToString(), "Active");

                            WebServicesHandler.SharedInstance.GetTournamentMatchId("15", "100");

                            MainSocketConnection.instance.isTournamentRealMoney = true;
                            MainSocketConnection.instance.isTournamentBrandRealMoney = false;
                            GameStates.SetCurrent_State_TO(GAME_STATE.LEAGUE);
                            break;


                        case "brandgreensoccer":

                            goalCounter = 1;
                            PlayerPrefs.SetInt("Gamegoals", goalCounter);                    //save the game-time value

                            FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                            TournamentLevel = "1";

                            GameManager.SharedInstance.PlayerRPAmountValue -= 100;

                            WebServicesHandler.SharedInstance.GetUpdateRPUserBettingCompany(PlayerPrefs.GetInt("BettingCompanyID").ToString(), GameManager.SharedInstance.PlayerRPAmountValue.ToString(), "Active");

                            WebServicesHandler.SharedInstance.GetTournamentMatchId("16", "100");

                            MainSocketConnection.instance.isTournamentRealMoney = false;
                            MainSocketConnection.instance.isTournamentBrandRealMoney = true;

                            GameStates.SetCurrent_State_TO(GAME_STATE.LEAGUE);
                            break;
                            */
            // ----------------- PLAY WITH FRIENDS LEVELS-----------------------//
            //case "pfgamesoccer":   
            //    GameStates.SetCurrent_State_TO(GAME_STATE.PLAYWITHFRIENDS);
            //    break;

            //case "pfgreensoccer":
            //    GameStates.SetCurrent_State_TO(GAME_STATE.PLAYWITHFRIENDS);
            //    break;

            //case "pfjogabonito":
            //    GameStates.SetCurrent_State_TO(GAME_STATE.PLAYWITHFRIENDS);
            //    break;

            //case "pfsoccer":
            //    GameStates.SetCurrent_State_TO(GAME_STATE.PLAYWITHFRIENDS);
            //    break;

            case "otgamesoccer":
                // ----------------- OFFLINE TOURNAMENT LEVELS-----------------------//
                SelectedStadium = 0;
                GoalsToWin = 1;

                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                OfflineTournamentManager.Instance.AssignFirstRound();
                //            OfflineLeagueScreen.transform.Find ("Popup").transform.Find ("playleaguebtn").localScale = Vector3.one;
                GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINELEAGUE);
                break;

            case "otgreensoccer":
                SelectedStadium = 0;
                GoalsToWin = 1;

                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                OfflineTournamentManager.Instance.AssignFirstRound();
                OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.one;
                GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINELEAGUE);
                break;

            case "otjogabonito":
                SelectedStadium = 0;
                GoalsToWin = 1;

                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                OfflineTournamentManager.Instance.AssignFirstRound();
                OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.one;
                GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINELEAGUE);
                break;

            case "otsoccer":
                SelectedStadium = 0;
                GoalsToWin = 1;

                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                OfflineTournamentManager.Instance.AssignFirstRound();
                OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.one;
                GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINELEAGUE);
                break;

            case "ppgamesoccer":
                // ----------------- PLAY AND PASS LEVELS-----------------------//
                SelectedStadium = 0;
                FST_SettingsManager.IsTimeBased = true; //Time base ON.

                GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                break;

            case "ppgreensoccer":
              //  SelectedStadium = 0;
                //   GoalsToWin = 3;

                //  FST_SettingsManager.IsTimeBased = false; //Time base OFF.

                //GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
                break;

            case "profile":
                GameStates.SetCurrent_State_TO(GAME_STATE.PROFILE);
                break;

            case "formationbtn":
                GameStates.SetCurrent_State_TO(GAME_STATE.SHOPFORMATION);
                break;

            case "freegold":
                Debug.LogError("You need to integrate video in this method");
                break;

            case "rpshop":
                GameStates.SetCurrent_State_TO(GAME_STATE.SHOP);
                break;

            case "brand":
                GameStates.SetCurrent_State_TO(GAME_STATE.BRAND);
                break;

            case "rent":
                GameStates.SetCurrent_State_TO(GAME_STATE.RENT);
                break;

            case "minigame":
                isMenuSpin = false;
                GameStates.SetCurrent_State_TO(GAME_STATE.SPIN);
                break;

            case "playfacebookfriends":
                GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            case "plusfacebook":
                GameStates.SetCurrent_State_TO(GAME_STATE.CHALLENGEFRIENDS);
                break;

            case "InviteFriends":
                GameStates.SetCurrent_State_TO(GAME_STATE.INVITEFRIENDS);
                break;

            case "AddNewFriends":
                GameStates.SetCurrent_State_TO(GAME_STATE.SEARCHUSER);
                break;

            case "overmenu":
                // Debug.Log("1");
                LoadLevelWithLoadingScreen("MainMenu");
                break;

            case "mainmenu":
                FST_Gameplay.IsPWF = false;
                Debug.Log("REMOVE CASE IF THIS DEBUG DOES NOT APPEAR!!!!!!!!!");//add the funcs into the method in registered states, this is not ever used it appears
                LoadLevelWithLoadingScreen("MainMenu");
                break;

            case "pause":
                GameStates.SetCurrent_State_TO(GAME_STATE.PAUSEMENU);
                break;

            case "purchase":
                //GameStates.SetCurrent_State_TO (GAME_STATE.INAPPSUCCESSFAIL);
                break;

            case "purchaseok":
                if (PurchasePopups != null)
                {
                    Destroy(PurchasePopups);
                    PurchasePopups = null;
                }
                GameStates.SetCurrent_State_TO(GameStates.previousState);
                break;

            case "resume":
                GameStates.SetCurrent_State_TO(GAME_STATE.RESUME);
                break;

            case "help":
                GameStates.SetCurrent_State_TO(GAME_STATE.HELP);
                break;

            case "credit":
                GameStates.SetCurrent_State_TO(GAME_STATE.ABOUT_US);
                break;

            case "btnprivatepolicy":
                Application.OpenURL("http://www.aaryavarta.com/privacy-policy/");
                break;

            case "leaderboard":
                print("show leaderborad");
                GameStates.SetCurrent_State_TO(GAME_STATE.LEADERBOARD);
                break;

            case "shop":
                GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);
                break;

            case "back":
                FST_Gameplay.IsPWF = false;
                BackButtonClicked();
                break;

            case "exit":
                FST_Gameplay.IsPWF = false;
                GameStates.SetCurrent_State_TO(GAME_STATE.EXIT);
                break;

            case "yes":
                if (GameStates.currentState == GAME_STATE.EXIT)
                {
                    Application.Quit();
                }
                else if (GameStates.currentState == GAME_STATE.OPTION)
                {
                    UIControllerInstance.Loading_2_panel.SetActive(true);
                    Debug.Log("PLAYER LOGGING OUT!");
                    FST_MPConnection.Instance.Disconnect(true);
                    UIControllerInstance.LogOut();
                }

                break;

            case "no":
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                if (SceneManager.GetActiveScene().name == "InGame")
                {
                    // Debug.Log("No..........");
                    GameStates.SetCurrent_State_TO(GAME_STATE.RESUME);
                }
                break;

            case "settings":
                GameStates.SetCurrent_State_TO(GAME_STATE.SETTINGS);
                break;
            case "leaderboardbutton":
                break;
            case "music":
                break;
            case "music1":
                break;
            case "closeleaderboard":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;
            case "shopback":
                GameStates.SetCurrent_State_TO(GameStates.previousState);
                break;

            case "retry":
                LoadLevel("InGame");
                break;

            case "option":
                GameStates.SetCurrent_State_TO(GAME_STATE.OPTION);
                break;

            case "btnlater":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "btnrate":
                Application.OpenURL("https://play.google.com/store/apps/details?id=com.aaryavarta.up");
                break;
            case "facebookinvite":

                break;
            case "fbshare":

                break;
            case "btnminiexit":
                GameStates.SetCurrent_State_TO(GAME_STATE.MINIEXIT);
                break;
            case "closebtn":
                if (GameStates.currentState == GAME_STATE.CHALLENGEFRIENDS)
                {
                    //                GameStates.SetCurrent_State_TO (pre);
                }
                else if (UIControllerInstance.Tutorialscreen.activeSelf) //as per MVP Task list.
                {
                    UIControllerInstance.Tutorialscreen.SetActive(false);
                }
                break;

            case "closebrand":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "closeprofile":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;
            case "quickplaybtn":

                GoalsToWin = 3;

                FST_SettingsManager.IsTimeBased = false; //Time base OFF.
                ScrollImage.Instance.isRotateImage = true;
                GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
                break;

            case "OnlineFormationPlayButton":
                GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
                break;

            case "AddbyUserButton":

                GameStates.SetCurrent_State_TO(GAME_STATE.SEARCHUSER);
                Joga_FriendsManager.Instance.SearchFriendInput.GetComponent<InputField>().text = "";

                Debug.Log("GET ALL USERS HERE");

                break;

            case "backChallegeFriend":
                FST_Gameplay.IsPWF = false;
                Joga_FriendsManager.Instance.ParentObject.gameObject.SetActive(false);
                Joga_FriendsManager.Instance.SearchFriend_panel.SetActive(false);
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "backSearchUserPanel":
                // UIControllerInstance.ActiveLoading2Panel();
                Joga_FriendsManager.Instance.ResetFriendList();
                Joga_FriendsManager.Instance.ResetAllSearchData();
                Joga_SearchPlayerManager.Instance.ResetAllPlayerData();
                Joga_SearchPlayerManager.Instance.SearchInputField.GetComponent<InputField>().text = "";

                GameStates.SetCurrent_State_TO(GAME_STATE.CHALLENGEFRIENDS);

                UIControllerInstance.DeactiveLoading2Panel(0.5f);
                break;

            case "jiwemanlogin":
                Debug.Log("In gamemanager jiwemanlogin case");
                GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
                break;

            case "jiwemanloginPopUp":
                Debug.Log("In gamemanager jiwemanlogin case");
                //GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
                UIControllerInstance.RegiSter_PopUP.SetActive(false);
                UIControllerInstance.Login_PopUP.SetActive(true);
                break;

            case "loginback":
                Debug.Log("In gamemanager loginback case");
                GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
                break;

            case "forgetback":
                UIControllerInstance.DisableErrorForgotpass();
                GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
                break;

            case "gotologin":
                GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
                Debug.Log("go to login panel");
                break;

            case "closeoption":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "mainback":
                MenuScreen.SetActive(true);
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "buyback":
                GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);
                break;

            case "prevback":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            case "selectback":
                //GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);    // as per MVP pass and play flow

                if (FST_Gameplay.IsMultiplayer)
                {
                    // Debug.Log("disconnecting..");
                    // FST_MPConnection.Instance.LeaveRoom();
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;

            // case "settingbtn":
            //       break;
            // case "btnok":
            //       break;
            // case "ingamebtnok":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.RESUME);
            //       break;
            // case "achivement":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.ACHIEVEMENT);
            //       break;

            // case "club"://-----------------------------------------Club ------------------------------//
            //       GameStates.SetCurrent_State_TO(GAME_STATE.CLUBINFOPAGE);
            //       break;
            // case "clubexpenses":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.CLUBEXPENSES);
            //       break;
            /*
case "playleaguebtn":
ScrollImage.instance.isRotateImage = true;
GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
break;
case "backleaguebtn":

if (FST_SettingsManager.MatchType == 4)
{
isPlayerRound1Win = false;
isPlayerRound2Win = false;
isPlayerRound3Win = false;
isPlayerLoseTournament = false;
storeAITeamsList.Clear();
storeAISecondRoundTeamsList.Clear();
OfflineTournamentManager.instance.aiTempTeamsList.Clear();
OfflineTournamentManager.instance.aiSecondRoundTeamsList.Clear();
OfflineTournamentManager.instance.ResetTournament();
}
else if (FST_SettingsManager.MatchType == 3)
{
// MainSocketConnection.instance.CloseConnection ();
isOnlineTournamentLose = false;
isOnlineTournamentRound1Win = false;
isOnlineTournamentRound2Win = false;
isOnlineTournamentRound3Win = false;
OnlineTournamentLevel1PlayerID.Clear();
OnlineTournamentLevel2PlayerID.Clear();
OnlineTournamentLevel3PlayerID.Clear();
OnlineTournamentManager.instance.level1Player.Clear();
OnlineTournamentManager.instance.level2Player.Clear();
OnlineTournamentManager.instance.ResetOnlineTournament();
}
GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
break;
case "BackLeagueButton":

SceneManager.LoadScene("MainMenu");
AchievementCompletedDataManager.instance.CheckCompleteAchievement();

if (FST_SettingsManager.MatchType == 4)
{
BackButtonInLeague();
}
break;
*/
            //    case "citynext":
            // CitySlide.SharedInstance.NextButton();
            // break;

            // case "protective"://----------------------------------------------Buy Caps Screens---------------------------------------------//
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[0].SetActive(true);
            //       break;

            // case "attacking":
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[1].SetActive(true);
            //       break;

            // case "statrevival":
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[2].SetActive(true);
            //       break;

            // case "staminaprotection":
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[3].SetActive(true);
            //       break;

            // case "firstad":
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[4].SetActive(true);
            //       break;
            // case "force_Button":
            //       UIControllerInstance.aim.SetActive(false);
            //       UIControllerInstance.time.SetActive(false);
            //       UIControllerInstance.force.SetActive(true);
            //       break;

            // case "aim_Button":
            //       UIControllerInstance.aim.SetActive(true);
            //       UIControllerInstance.time.SetActive(false);
            //       UIControllerInstance.force.SetActive(false);
            //       break;

            // case "time_Button":
            //       UIControllerInstance.aim.SetActive(false);
            //       UIControllerInstance.time.SetActive(true);
            //       UIControllerInstance.force.SetActive(false);
            //       break;

            // case "closerent":
            //       UIControllerInstance.DesabledAllBuyCapsScreens();
            //       UIControllerInstance.buy_caps_pages[0].SetActive(true);
            //       GameStates.SetCurrent_State_TO(GAME_STATE.UPGRADE);
            //       break;
            // case "betting":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.BETTING_COMPANY);
            //       WebServicesHandler.SharedInstance.GetAllBettingCompany();
            //       break;
            //        case "closeclubinfo":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
            //       break;

            // case "closeclubexpense":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.CLUBINFOPAGE);
            //       break;

            // case "playerinfo":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.PLAYERINFORMATION);
            //       break;
            // case "AchievementBack":
            //       AchievementManager.instance.DeleteAllAchievement();
            //       GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
            //       break;
            // case "CloseBettingButton":
            //       BettingCompanyManager.instance.ClearBettingCompanyData();
            //       GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
            // //       break;
            // case "clubmanagement":
            //       GameStates.SetCurrent_State_TO(GAME_STATE.CLUBMANAGEMENT);
            //       break;
            //break;

            case "brandleaderboard":
                break;

            case "mainseasonleaderboard":
                GameStates.SetCurrent_State_TO(GAME_STATE.LEADERBOARD);
                break;

            case "Leagueleaderboard":
                GameStates.SetCurrent_State_TO(GAME_STATE.SEASONLEADERBOARD);
                break;

            case "playfriend":
                if (!Joga_FriendsManager.Instance.isEditMode)
                    Joga_FriendsManager.Instance.EditButton.GetComponent<Image>().color = new Color(255f, 255f, 255f, 255f);

                Joga_FriendsManager.Instance.ParentObject.gameObject.SetActive(true);

                UIControllerInstance.Mainscreen_Logo.SetActive(false);
                FST_SettingsManager.MatchType = 5;
                ActiveLevelsScreen("virtualquickplay");
                GameStates.SetCurrent_State_TO(GAME_STATE.CHALLENGEFRIENDS);
                break;

            case "SelectStadiumButton":
                GameStates.SetCurrent_State_TO(GAME_STATE.STADIUMSELECTION);
                break;
            case "backclubbutton":
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                break;
            case "backseasonleaderboard":

                Joga_LeaderBoard.Instance.ResetAllLeaderBoardData();
                HelpPopup.instance.ClosePop();

                if (IsOneNOneLeaderBoard)
                {
                    GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
                }
                else
                {
                    GameStates.SetCurrent_State_TO(GameStates.previousState);
                }

                break;
            case "backstadiumSelection":
                GameStates.SetCurrent_State_TO(GAME_STATE.CLUBMANAGEMENT);
                break;

        }
    }

    public void AssignTextInstanceToObject(string objectname, GameObject assignedObject)
    {
        objectname = objectname.ToLower();

        if (objectname.Contains("playernametext"))
            PlayernameText = assignedObject.GetComponent<Text>();

        if (objectname.Contains("opponentnametext"))
            OpponentNameText = assignedObject.GetComponent<Text>();

        if (objectname.Contains("playergoalstext"))
            PlayergoalsText = assignedObject.GetComponent<Text>();

        if (objectname.Contains("opponentgoalstext"))
            OpponentgoalsText = assignedObject.GetComponent<Text>();

        if (objectname.Contains("playerwinstext"))
            PlayerWinsText = assignedObject.GetComponent<Text>();

        if (objectname.Contains("playermoneytext"))
            PlayerMoneyText = assignedObject.GetComponent<Text>();
    }

    public void OnlineTournamentWinScreenBackButton() => StartCoroutine(BackTOWinScreenToLeagueScreen());

    private IEnumerator BackTOWinScreenToLeagueScreen()
    {
        yield return new WaitForSeconds(3.0f);
        if (FST_Gameplay.IsMultiplayer)
            FST_MPConnection.Instance.LeaveRoom(true, true);
        else
            SceneManager.LoadScene("MainMenu");

        yield return new WaitForSeconds(0.2f);
        // Debug.Log("DeactiveLoading2Panel-52");
        UIControllerInstance.DeactiveLoading2Panel(0.5f);

        /*
                if (isOnlineTournamentLose && isOnlineTournamentRound1Win && isOnlineTournamentRound2Win)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.level2Player = OnlineTournamentLevel2PlayerID;
                    OnlineTournamentManager.instance.level3Player = OnlineTournamentLevel3PlayerID;
                    OnlineTournamentManager.instance.AssignAllLevelPlayer();
                }
                else if (isOnlineTournamentLose && isOnlineTournamentRound1Win)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.level2Player = OnlineTournamentLevel2PlayerID;
                    OnlineTournamentManager.instance.Assign2RoundPlayer();
                }
                else if (isOnlineTournamentLose)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.Assign1RoundPlayer();
                }

                if (isOnlineTournamentRound1Win && !isOnlineTournamentRound2Win && !isOnlineTournamentRound3Win && !isOnlineTournamentLose)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.Assign1RoundPlayer();
                }
                else if (isOnlineTournamentRound1Win && isOnlineTournamentRound2Win && !isOnlineTournamentRound3Win && !isOnlineTournamentLose)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.level2Player = OnlineTournamentLevel2PlayerID;
                    OnlineTournamentManager.instance.Assign2RoundPlayer();
                }
                else if (isOnlineTournamentRound1Win && isOnlineTournamentRound2Win && isOnlineTournamentRound3Win && !isOnlineTournamentLose)
                {
                    OnlineTournamentManager.instance.level1Player = OnlineTournamentLevel1PlayerID;
                    OnlineTournamentManager.instance.level2Player = OnlineTournamentLevel2PlayerID;
                    OnlineTournamentManager.instance.level3Player = OnlineTournamentLevel3PlayerID;
                    OnlineTournamentManager.instance.AssignAllLevelPlayer();
                    StartCoroutine(ActiveOnlineTournamentWinScreen());
                }
                else
                {
                    yield return new WaitForSeconds(0.3f);
                    StartCoroutine(ActiveOnlineTournamentLostScreen());
                }

                GameStates.SetCurrent_State_TO(GAME_STATE.LEAGUE);

                if (isOnlineTournamentRound1Win && !isOnlineTournamentRound2Win && !isOnlineTournamentRound3Win && !isOnlineTournamentLose)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (MainSocketConnection.instance.isTournamentRealMoney)
                    {
                        TournamentLevel = "2";
                        WebServicesHandler.SharedInstance.GetSRealMoneyTournament(PlayerPrefs.GetInt("BettingCompanyID").ToString(), "100", "start", MainSocketConnection.instance.tournamentMatchId, TournamentLevel);
                    }
                    else if (MainSocketConnection.instance.isTournamentBrandRealMoney)
                    {
                        TournamentLevel = "2";
                        WebServicesHandler.SharedInstance.GetBrandRealMoneyTournamentStart(PlayerPrefs.GetInt("BettingCompanyID").ToString(), "100", "start", MainSocketConnection.instance.tournamentMatchId, TournamentLevel, "100", "1");
                    }
                    MainSocketConnection.instance.RegistrationForTour(TournamentLevel);
                }
                else if (isOnlineTournamentRound1Win && isOnlineTournamentRound2Win && !isOnlineTournamentRound3Win && !isOnlineTournamentLose)
                {
                    yield return new WaitForSeconds(0.2f);
                    if (MainSocketConnection.instance.isTournamentRealMoney)
                    {
                        TournamentLevel = "3";
                        WebServicesHandler.SharedInstance.GetSRealMoneyTournament(PlayerPrefs.GetInt("BettingCompanyID").ToString(), "100", "start", MainSocketConnection.instance.tournamentMatchId, TournamentLevel);
                    }
                    else if (MainSocketConnection.instance.isTournamentBrandRealMoney)
                    {
                        TournamentLevel = "3";
                        WebServicesHandler.SharedInstance.GetBrandRealMoneyTournamentStart(PlayerPrefs.GetInt("BettingCompanyID").ToString(), "100", "start", MainSocketConnection.instance.tournamentMatchId, TournamentLevel, "100", "1");
                    }
                    MainSocketConnection.instance.RegistrationForTour(TournamentLevel);
                }
        */
    }

    /*
        IEnumerator ActiveOnlineTournamentWinScreen()
        {
         LeagueScreen.transform.Find("backleaguebtn").localScale = Vector3.one;
         GameObject WinPanel = LeagueScreen.transform.Find("WinToPopPanel").gameObject;
        WinPanel.SetActive(true);
        GameObject LostPanel = LeagueScreen.transform.Find("LostTpPopupPanel").gameObject;
        LostPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        WinPanel.SetActive(false);
        LostPanel.SetActive(false);
         }
    */
    /*
        IEnumerator ActiveOnlineTournamentLostScreen()
        {
            LeagueScreen.transform.Find("backleaguebtn").localScale = Vector3.one;
            GameObject WinPanel = LeagueScreen.transform.Find("WinToPopPanel").gameObject;
        WinPanel.SetActive(false);
        GameObject LostPanel = LeagueScreen.transform.Find("LostTpPopupPanel").gameObject;
        LostPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        WinPanel.SetActive(false);
        LostPanel.SetActive(false);
         }
    */

    public void BackButtonInLeague() => StartCoroutine(LeagueBackButton());

    public void DisableLogin_bar()
    {
        UIControllerInstance.Mainscreen_Logo.SetActive(false);
        UIControllerInstance.topbar.SetActive(false);
        UIControllerInstance.bottom_bar.SetActive(false);
    }

    public void EnableLogin_bar()
    {
        UIControllerInstance.topbar.SetActive(true);
        UIControllerInstance.bottom_bar.SetActive(true);
      //  Debug.Log("Enabled Top bar");
    }

    private IEnumerator LeagueBackButton()
    {
        // Debug.Log("Enter the condition");

        yield return new WaitForSeconds(0.2f);
        GameStates.SetCurrent_State_TO(GAME_STATE.OFFLINELEAGUE);

        UIControllerInstance.LoadingPanel.SetActive(false);

        if (isPlayerLoseTournament && isPlayerRound1Win && isPlayerRound2Win)
        {
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.aiSecondRoundTeamsList = storeAISecondRoundTeamsList;
            OfflineTournamentManager.Instance.LostThirdRoundTournament();

        }
        else if (isPlayerLoseTournament && isPlayerRound1Win)
        {
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.aiSecondRoundTeamsList = storeAISecondRoundTeamsList;
            OfflineTournamentManager.Instance.LostSecondRoundTournament();

        }
        else if (isPlayerLoseTournament)
        {
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.LostFirstRoundTournament();
        }

        if (isPlayerRound1Win && !isPlayerRound2Win && !isPlayerLoseTournament)
        {
            OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.one;
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.AddSecondRoundList();
        }
        else if (isPlayerRound1Win && isPlayerRound2Win && !isPlayerRound3Win && !isPlayerLoseTournament)
        {
            OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.one;
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.aiSecondRoundTeamsList = storeAISecondRoundTeamsList;
            OfflineTournamentManager.Instance.AddThirdRoundList();
        }
        else if (isPlayerRound1Win && isPlayerRound2Win && isPlayerRound3Win && !isPlayerLoseTournament)
        {
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.aiSecondRoundTeamsList = storeAISecondRoundTeamsList;
            OfflineTournamentManager.Instance.ShowAllTournamentData();
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(ActiveTournamentWinScreen());
        }
        else
        {
            OfflineTournamentManager.Instance.aiTempTeamsList = storeAITeamsList;
            OfflineTournamentManager.Instance.aiSecondRoundTeamsList = storeAISecondRoundTeamsList;
            OfflineTournamentManager.Instance.ShowAllTournamentData();
            yield return new WaitForSeconds(0.3f);
            StartCoroutine(ActiveTournamentLostScreen());
        }
    }

    private IEnumerator ActiveTournamentWinScreen()
    {
        OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.zero;

        GameObject WinPanel = OfflineLeagueScreen.transform.Find("Popup").transform.Find("WinToPopPanel").gameObject;
        WinPanel.SetActive(true);
        GameObject LostPanel = OfflineLeagueScreen.transform.Find("Popup").transform.Find("LostTpPopupPanel").gameObject;
        LostPanel.SetActive(false);
        yield return new WaitForSeconds(2.0f);
        WinPanel.SetActive(false);
        LostPanel.SetActive(false);
    }

    private IEnumerator ActiveTournamentLostScreen()
    {
        OfflineLeagueScreen.transform.Find("Popup").transform.Find("playleaguebtn").localScale = Vector3.zero;

        GameObject WinPanel = OfflineLeagueScreen.transform.Find("Popup").transform.Find("WinToPopPanel").gameObject;
        WinPanel.SetActive(false);
        GameObject LostPanel = OfflineLeagueScreen.transform.Find("Popup").transform.Find("LostTpPopupPanel").gameObject;
        LostPanel.SetActive(true);
        yield return new WaitForSeconds(2.0f);
        WinPanel.SetActive(false);
        LostPanel.SetActive(false);
    }

    //Changing bg according to index
    public void Change_BG()
    {
        if (Current_brand == 0)
        {
            CurrentBg = BG[0];
            Currenttopbar = Topbar[0];
            Currentdownbar = Downbar[1];
            CurrenttopCornerbar = mtopCornerbar[0];
            CurrentdownCornerbar = mdownCornerbar[1];
        }
        else
        {
            CurrentBg = BG[1];
            Currenttopbar = Topbar[0];
            Currentdownbar = Downbar[1];
            CurrenttopCornerbar = mtopCornerbar[0];
            CurrentdownCornerbar = mdownCornerbar[1];
        }

        for (int i = 0; i < BgObjects.Count; i++)
            if (BgObjects[i] != null)
                BgObjects[i].GetComponent<Image>().sprite = CurrentBg;

        if (TopbarObject != null)
            TopbarObject.GetComponent<Image>().sprite = Currenttopbar;

        if (DownbarObject != null)
            DownbarObject.GetComponent<Image>().sprite = Currentdownbar;

        for (int i = 0; i < TopCornerbarObject.Count; i++)
            if (TopCornerbarObject[i] != null)
                TopCornerbarObject[i].GetComponent<Image>().sprite = CurrenttopCornerbar;

        for (int i = 0; i < DownCornerbarObject.Count; i++)
            if (DownCornerbarObject[i] != null)
                DownCornerbarObject[i].GetComponent<Image>().sprite = CurrentdownCornerbar;

        for (int i = 0; i < BrandManager.SharedInstance.brandads_count; i++)
            UIControllerInstance.mixbrand_ads[i].SetActive(false);
    }

    //Ads according to index
    public void Change_Ads()
    {
        CurrentBg = BG[0];
        Currenttopbar = Topbar[0];
        Currentdownbar = Downbar[0];
        CurrenttopCornerbar = mtopCornerbar[0];
        CurrentdownCornerbar = mdownCornerbar[0];

        if (BgObjects != null)
            for (int i = 0; i < BgObjects.Count; i++)
                BgObjects[i].GetComponent<Image>().sprite = CurrentBg;

        if (TopbarObject != null)
            TopbarObject.GetComponent<Image>().sprite = Currenttopbar;

        if (DownbarObject != null)
            DownbarObject.GetComponent<Image>().sprite = Currentdownbar;

        if (TopCornerbarObject != null)
            for (int i = 0; i < TopCornerbarObject.Count; i++)
                TopCornerbarObject[i].GetComponent<Image>().sprite = CurrenttopCornerbar;

        if (DownCornerbarObject != null)
            for (int i = 0; i < DownCornerbarObject.Count; i++)
                DownCornerbarObject[i].GetComponent<Image>().sprite = CurrentdownCornerbar;

        //UIControllerInstance.mixbrand_button [0].SetActive (true);
        UIControllerInstance.Mainscreen_Logo.SetActive(false);
    }

    private void BackButtonClicked() => GameStates.SetCurrent_State_TO(GameStates.previousState);

    public void OpenChooseOpponentInLeague()
    {
        ScrollImage.Instance.isRotateImage = true;
        GameStates.SetCurrent_State_TO(GAME_STATE.CHOOSEOPPONENT);
    }

    private void InAppState()
    {
        DisableAllScreens();
        ShopScreen.transform.localScale = Vector3.one;
        StartCoroutine(WaitShopScreen());
    }

    private IEnumerator WaitShopScreen()
    {
        yield return new WaitForSeconds(0.002f);

        yield break;
    }

    public void LoadGameplayScene()
    {
        if (FST_Gameplay.IsMultiplayer)
        {
           // Debug.Log("LOADING MP SETUP");
            PhotonNetwork.LoadLevel("InGame");// force all in room to play this very scene
            return;
        }

        // the below values are already set by the formation buttons... The team ones will be also

        //       FST_SettingsManager.Formation = p1FormationCounter; //save the player-1 formation index
        //       FST_SettingsManager.Team = p1TeamCounter;//save the player-1 team index
        //       FST_SettingsManager.FormationOpponent = p2FormationCounter;//save the player-2 formation index
        //       FST_SettingsManager.TeamOpponent = p2TeamCounter; //save the player-2 team index

        SceneManager.LoadScene("InGame");
    }

    private void InGame()//we likely dont need this! check it.
    {
        if (IsGamePause)
            return;

        DisableAllScreens();

        if (GlobalGameManager.IsOfflinePassAndPlayMatch || FST_SettingsManager.MatchType == 0)
            FST_FormationsManager.Instance.OpenOrCloseFormationPanel(true);
    }

    private void PauseMenu()
    {
        DisableAllScreens();
        GameStates.SetCurrent_State_TO(GAME_STATE.INGAME);
        return;
        /*
                    isGamePause = true;
                    if (PauseScreen == null)
                    {
                        GameObject PauseScreenParent = (GameObject)Instantiate(Resources.Load("PREFABS/Pause") as GameObject);
                        PauseScreen = PauseScreenParent.transform.GetChild(0).gameObject;
                    }
                    PauseScreen.transform.localScale = Vector3.one;
                    StartCoroutine("waitTimePause");

                    GameStates.SetCurrent_State_TO(GAME_STATE.INGAME);
                    DisableLogin_bar();
        */
    }
    private void Resume()
    {
        IsGamePause = false;
        DisableAllScreens();
        GameStates.SetCurrent_State_TO(GAME_STATE.INGAME);
        Debug.Log("inside resume");
    }
    public void gotoRewardedAgain()
    {
        UpdateCoins(50);
    }

    private void DisableAllScreens()
    {
        if (HelpScreen != null)
        {
            if (GameStates.currentState == GAME_STATE.PAUSEMENU)
            {
                Destroy(HelpScreen);
                HelpScreen = null;
            }
            else HelpScreen.transform.localScale = Vector3.zero;
        }
        if (leagueProgressionPanel != null)
            leagueProgressionPanel.SetActive(false);
        if (LoadingScreen != null)
            LoadingScreen.transform.localScale = Vector3.zero;
        if (ForgetPassword != null)
            ForgetPassword.transform.localScale = Vector3.zero;
        if (CreditScreen != null)
            CreditScreen.transform.localScale = Vector3.zero;
        if (ShopScreen != null)
            ShopScreen.transform.localScale = Vector3.zero;
        if (ExitScreen != null)
            ExitScreen.transform.localScale = Vector3.zero;
        if (MenuScreen != null)
            MenuScreen.SetActive(false);
        if (QuickPlay != null)
            QuickPlay.transform.localScale = Vector3.zero;
        if (OfflineScreen != null)
            OfflineScreen.transform.localScale = Vector3.zero;
        if (PauseScreen != null)
            PauseScreen.transform.localScale = Vector3.zero;
        if (RateScreen != null)
            RateScreen.transform.localScale = Vector3.zero;
        if (GameOverScreen != null)
            GameOverScreen.transform.localScale = Vector3.zero;
        if (SettingScreen != null)
            SettingScreen.transform.localScale = Vector3.zero;
        if (SpinWheelScreen != null)
            SpinWheelScreen.transform.localScale = Vector3.zero;
        if (InviteFriendsScreen != null)
            InviteFriendsScreen.transform.localScale = Vector3.zero;
        if (ChallengeFriendsScreen != null)
            ChallengeFriendsScreen.transform.localScale = Vector3.zero;
        if (SearchUserScreen != null)
            SearchUserScreen.transform.localScale = Vector3.zero;
        if (LeagueScreen != null)
            LeagueScreen.transform.localScale = Vector3.zero;
        if (PlayWithFriendsScreen != null)
            PlayWithFriendsScreen.transform.localScale = Vector3.zero;
        if (LevelSelectionScreen != null)
            LevelSelectionScreen.transform.localScale = Vector3.zero;

        FST_FormationsManager.Instance.OpenOrCloseFormationPanel(false);

        if (ShopFormationScreen != null)
            ShopFormationScreen.transform.localScale = Vector3.zero;
        if (ChooseOpponentScreen != null)
            ChooseOpponentScreen.transform.localScale = Vector3.zero;
        if (AchievementScreen != null)
            AchievementScreen.transform.localScale = Vector3.zero;
        if (PlayWithFriendsLeagueScreen != null)
            PlayWithFriendsLeagueScreen.transform.localScale = Vector3.zero;
        if (OfflineLeagueScreen != null)
            OfflineLeagueScreen.transform.localScale = Vector3.zero;
        if (UpgradeScreen != null)
            UpgradeScreen.transform.localScale = Vector3.zero;
        if (RentScreen != null)
            RentScreen.transform.localScale = Vector3.zero;
        if (BrandScreen != null)
            BrandScreen.transform.localScale = Vector3.zero;
        if (LeaderBoardScreen != null)
            LeaderBoardScreen.transform.localScale = Vector3.zero;
        if (PlayerProfileScreen != null)
            PlayerProfileScreen.transform.localScale = Vector3.zero;
        if (ClubManagementScreen != null)
            ClubManagementScreen.transform.localScale = Vector3.zero;
        if (StadiumSelectionScreen != null)
            StadiumSelectionScreen.transform.localScale = Vector3.zero;
        if (ClubInfoScreen != null)
            ClubInfoScreen.transform.localScale = Vector3.zero;
        if (OptionScreen != null)
            OptionScreen.transform.localScale = Vector3.zero;
        if (ClubExpensesScreen != null)
            ClubExpensesScreen.transform.localScale = Vector3.zero;
        if (JiwemanloginScreen != null)
        {
            // JiwemanloginScreen.transform.localScale = Vector3.zero;
            JiwemanloginScreen.SetActive(false);
        }
        if (JiwemanRegistrationScreen != null)
            JiwemanRegistrationScreen.transform.localScale = Vector3.zero;
        if (AccountCreatedScreen != null)
            AccountCreatedScreen.transform.localScale = Vector3.zero;
        if (SelectCityScreen != null)
            SelectCityScreen.transform.localScale = Vector3.zero;
        if (PlayerInformationScreen != null)
            PlayerInformationScreen.transform.localScale = Vector3.zero;
        if (LoginPanal != null)
            LoginPanal.transform.localScale = Vector3.zero;
        if (TutorialPanel != null)
            TutorialPanel.transform.localScale = Vector3.zero;
        if (BettingPanal != null)
            BettingPanal.transform.localScale = Vector3.zero;
    }

    IEnumerator ClearProps()
    {
        yield return new WaitUntil(() => PhotonNetwork.IsConnectedAndReady);
        PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { [FST_PlayerProps.REMATCH] = 0, [FST_PlayerProps.READY] = false, [FST_PlayerProps.LOADED_LEVEL] = false });
    }

    public void MainMenu()
    {
        StartCoroutine(ClearProps());
        FST_Gameplay.IsPWF = false;
        FST_Gameplay.IsMultiplayer = false;
        FST_MPDebug.Log("mm c state: " + PhotonNetwork.NetworkClientState);
        Debug.Log("mm c state: " + PhotonNetwork.NetworkClientState);
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
        {
            CancelOpponentFind();
           // FST_MPConnection.Instance.LeaveRoom(false, false);//we get a fresh scene from above line!
        }

        DisableAllScreens();

        if (!string.IsNullOrEmpty(FST_SettingsManager.LastFinishedLeagueID))
        {
            StartCoroutine(WaitForLogin());
            return;
        }

        if (MenuScreen)
            MenuScreen.SetActive(true);
        if (UIControllerInstance.Mainscreen_Logo)
            UIControllerInstance.Mainscreen_Logo.SetActive(true);

        EnableLogin_bar();

        //NOTE: if not internet should be decided AFTER connection has been tried, as the device may be able to establish a connection later... never assume they have no connection until connection has been tried
        // if no connection is found, we will simply either disable the online modes, or put a banner over them.
        if (FST_MPConnection.InternetReachability == NetworkReachability.NotReachable)
        {
            if (GameStates.previousState == GAME_STATE.TUTORIAL)
                UIControllerInstance.DeactiveLoading2Panel(0.5f);
            return;
        }

        if (GameStates.previousState == GAME_STATE.TUTORIAL)
            UIControllerInstance.DeactiveLoading2Panel(0.5f);

        if (string.IsNullOrEmpty(FST_SettingsManager.PlayerName))
        {
            UIController.Instance.uiHandler.loginPanel.SetActive(true);
        }
        else
        {
            //Auto Login player if not yet logged in.
            if (!FST_SettingsManager.IsLoggedIn)
                Joga_AuthManager.Instance.AutoLogin();
            //Player already logged in...
            else
                UIController.Instance.uiHandler.SetDisplayName();
        }
    }

    IEnumerator WaitForLogin()
    {
        Debug.Log("show league end panel");
        UIControllerInstance.Loading_2_panel.SetActive(true);
        //Auto Login player if not yet logged in.
        if (!FST_SettingsManager.IsLoggedIn)
            Joga_AuthManager.Instance.AutoLogin();

        yield return new WaitUntil(() => FST_SettingsManager.IsLoggedIn);

        MainMenuEvents("league");
    }

    public void MessageStart()
    {
        UIControllerInstance.Tutorialprocedbtn.SetActive(true);
        UIControllerInstance.Welcome_msg.SetActive(true);
    }

    public void TutorialON()
    {
        if (UIControllerInstance.Tutorialscreen.transform.localScale == Vector3.zero)
            UIControllerInstance.Tutorialscreen.transform.localScale = Vector3.one;
        UIControllerInstance.Tutorialscreen.SetActive(true);
        UIControllerInstance.SkipTutorials.SetActive(true);
    }
    private void QuickPlay1()
    {
        DisableAllScreens();
        QuickPlay.transform.localScale = Vector3.one;
        //            StartCoroutine ("waitForShowOpponent");
        DisableLogin_bar();
    }
    [Obsolete("unused, will be removed in future update")]
    private void Offline()
    {
        DisableAllScreens();

        OfflineScreen.transform.localScale = Vector3.one;
        DisableLogin_bar();
        TopCornerbarObject[0].SetActive(false);
        DownCornerbarObject[0].SetActive(false);
        UIControllerInstance.DeactiveLoading2Panel(0.5f);
    }

    private void Achievement()
    {
        DisableAllScreens();

        AchievementScreen.transform.localScale = Vector3.one;

        Debug.Log("ACHIEVEMENT GET HERE");

        // commented --@sud
        // WebServicesHandler.SharedInstance.allAchievementDetails.Clear();
        // WebServicesHandler.SharedInstance.GetAllAchievement();
        DisableLogin_bar();
    }

    private void ShowBrand()
    {
        DisableAllScreens();

        BrandScreen.transform.localScale = Vector3.one;
        DisableLogin_bar();
    }

    private void ShowClubInfoPage()
    {
        DisableAllScreens();

        ClubInfoScreen.transform.localScale = Vector3.one;
        DisableLogin_bar();
    }

    private void ShowClubExpenses()
    {
        DisableAllScreens();

        ClubExpensesScreen.transform.localScale = Vector3.one;
        DisableLogin_bar();
    }

    private void ShowOption()
    {
        DisableAllScreens();

        OptionScreen.transform.localScale = Vector3.one;
        DisableLogin_bar();
    }

    private void ShowSelectCity()
    {
        DisableAllScreens();
        DisableLogin_bar();
        UIControllerInstance.DeactiveLoading2Panel(1f);
        SelectCityScreen.transform.localScale = Vector3.one;
        // Debug.Log("DeactiveLoading2Panel-16");
    }

    private void ShowPlayerInformation()
    {
        DisableAllScreens();
        DisableLogin_bar();
        PlayerInformationScreen.transform.localScale = Vector3.one;
    }

    private void ShowLoginPanal()
    {
        Debug.LogError("OLD OBSOLOTE CODE DETECTED!");
        DisableAllScreens();
        DisableLogin_bar();
        if (FST_SettingsManager.IsGuest)
            LoginPanal.transform.localScale = Vector3.one;
        // Debug.Log("DeactiveLoading2Panel-17");
        UIControllerInstance.DeactiveLoading2Panel(2f);
    }

    private void ShowBettingPanal()
    {
        DisableAllScreens();
        DisableLogin_bar();
        BettingPanal.transform.localScale = Vector3.one;
    }

    private void ShowUpgrade()
    {
        DisableAllScreens();
        DisableLogin_bar();
        UpgradeScreen.transform.localScale = Vector3.one;
    }

    private void Help1()
    {
        DisableAllScreens();
        if (HelpScreen != null)
            HelpScreen.transform.localScale = Vector3.one;
        else Instantiate(helpPrefab);
        DisableLogin_bar();
    }

    private void LevelSelection()
    {
        DisableAllScreens();
        DisableLogin_bar();
        LevelSelectionScreen.transform.localScale = Vector3.one;
        UIControllerInstance.Mainscreen_Logo.SetActive(false);
    }

    private void ActiveLevelsScreen(string screenName)
    {
        for (int i = 0; i < Levels.Count; i++)
        {
            if (Levels[i].name == screenName)
                Levels[i].gameObject.SetActive(true);
            else Levels[i].gameObject.SetActive(false);
        }
        // Debug.Log("DeactiveLoading2Panel -111");
        // UIControllerInstance.DeactiveLoading2Panel(0.5f);
    }

    private void AboutUs()
    {
        DisableAllScreens();
        DisableLogin_bar();
        CreditScreen.transform.localScale = Vector3.one;
    }

    private void Settings()
    {
        DisableAllScreens();
        DisableLogin_bar();
        SettingScreen.transform.localScale = Vector3.one;
    }

    private void Spin()
    {
        DisableAllScreens();
        DisableLogin_bar();
        SpinWheelScreen.transform.localScale = Vector3.one;
    }

    private void ShowLeaderBoard()
    {
        IsOneNOneLeaderBoard = true;
        DisableAllScreens();
        DisableLogin_bar();

        UIControllerInstance.LeaderBoardPanel.SetActive(true);
        UIControllerInstance.Loading_2_panel.SetActive(true);
    }

    private void ShowSeasonLeaderBoard()
    {
        IsOneNOneLeaderBoard = false;
        DisableAllScreens();
        DisableLogin_bar();

        UIControllerInstance.LeaderBoardPanel.SetActive(true);
        UIControllerInstance.Loading_2_panel.SetActive(true);
    }

    private void ShowPlayerProfile()
    {
        DisableAllScreens();
        Vector3 scale = new Vector3(1, 1, 1);
        float time = 0.5f;

        Joga_NetworkManager.Instance.GetPlayerStat();

        PlayerProfileScreen.transform.localScale = Vector3.MoveTowards(transform.localScale, scale, Time.deltaTime * time);

        UIControllerInstance.Mainscreen_Logo.SetActive(false);
        UIControllerInstance.topbar.SetActive(true);
        UIControllerInstance.Loading_2_panel.SetActive(true);

        DisableLogin_bar();
    }

    private void ClubManagementPanel()
    {
        DisableAllScreens();
        DisableLogin_bar();
        ClubManagementScreen.transform.localScale = Vector3.one;
    }

    private void StadiumSelectionPanel()
    {
        DisableAllScreens();
        DisableLogin_bar();
        StadiumSelectionScreen.transform.localScale = Vector3.one;
    }

    private void ShowRent()
    {
        DisableAllScreens();
        DisableLogin_bar();
        RentScreen.transform.localScale = Vector3.one;
        //            DiceRentPanal = RentScreen.transform.Find ("Container").gameObject.transform.Find ("RentPanal").gameObject;
    }

    private void League()
    {
        DisableAllScreens();
        DisableLogin_bar();
        LeagueScreen.transform.localScale = Vector3.one;
        //            StartCoroutine ("waitForShowOpponent");
    }

    private void OfflineLeague()
    {
        DisableAllScreens();
        DisableLogin_bar();
        OfflineLeagueScreen.transform.localScale = Vector3.one;
        //            StartCoroutine ("waitForShowOpponent");
    }

    private void ShowInviteFriendsScreen()
    {
        Debug.Log("SHOW INVITE FRIENDS SCREEN");
#if UNITY_EDITOR
        string path = Application.dataPath + "/Resources/NS_InviteFriends.jpeg";
#else
        string path = Application.persistentDataPath + "/Resources/NS_InviteFriends.jpeg";
#endif
        if(!System.IO.File.Exists(path))
        {
            Debug.Log("ShowInviteFriendsScreen() File Not Found, Writing new file to : " + path);
            Texture2D image = Resources.Load<Texture2D>("NS_InviteFriends");

            byte[] by = image.EncodeToJPG();

            var file = new System.IO.FileInfo(path);
            file.Directory.Create();

            System.IO.File.WriteAllBytes(path, by);
        }

        new NativeShare()
              .SetTitle("Download Joga Bonito now and lets play amazing matches together !")
              .SetSubject("Download Joga Bonito")
              .AddFile(path)
              .SetText("Download the game and lets play amazing matches together : https://jiweman.com/uploads/build/jogabonitobuild.apk").Share();
    }

    private void ShowChallengeFriendsScreen()
    {
        DisableAllScreens();
        DisableLogin_bar();
        ChallengeFriendsScreen.transform.localScale = Vector3.one;
        Joga_FriendsManager.Instance.GetFriends();
    }

    void ShowSearchUserScreen()
    {
        DisableAllScreens();
        DisableLogin_bar();
        UIControllerInstance.Mainscreen_Logo.SetActive(false);
        SearchUserScreen.transform.localScale = Vector3.one;
    }

    private void PlayWithFriends()
    {
        Debug.Log("PLAY WITH FRIENDS");

        DisableAllScreens();
        DisableLogin_bar();
        UIControllerInstance.Mainscreen_Logo.SetActive(false);
        PlayWithFriendsScreen.transform.localScale = Vector3.one;

        UIControllerInstance.DeactiveLoading2Panel(0.3f);
    }

    private void TutorialDone()
    {
        DisableAllScreens();
        DisableLogin_bar();
        TutorialPanel.transform.localScale = Vector3.one;
    }

    private void Rateus()
    {
        DisableAllScreens();
        DisableLogin_bar();
        RateScreen.transform.localScale = Vector3.one;
    }

    private void ExitPopup()
    {
        DisableAllScreens();
        DisableLogin_bar();
        ExitScreen.transform.localScale = Vector3.one;
    }
}