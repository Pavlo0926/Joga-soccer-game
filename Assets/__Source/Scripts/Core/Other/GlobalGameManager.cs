///*************************************************************************///
/// GlobalGameManager.cs
/// This class controls main aspects of the game like rounds, levels, scores and ...
/// Please note that the game always happens between 2 player: (Player-1 vs Player-2) or (Player-1 vs AI)
/// Player-2 and AI are the same in some aspects like when they got their turns, but they use different controllers.
/// Player-2 uses a similar controller as Player-1, while AI uses an artificial intelligent routine to play the game.
///
/// Important! All units and ball object inside the game should be fixed at Z=-0.5f positon at all times. 
/// You can do this with RigidBody's freeze position.
///*************************************************************************///
using Photon.Pun;
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Realtime;
using FastSkillTeam;
using Jiweman;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using Random = UnityEngine.Random;

#if UNITY_EDITOR
using System;
using System.Reflection;
#endif
public class GlobalGameManager : MonoBehaviourPun
{
    public GameObject ChooseFormationButton;


    public enum GamePhase { NotStarted, RoundStarted, BallKicked, GoalIntermission, FormationSelect, RoundEnded, BetweenRounds, Finished, WaitingForRoomUpdate, FormationClosed }

    public GamePhase Phase { get; private set; } = GamePhase.NotStarted;

    private static GlobalGameManager m_Instance = null;
    public static GlobalGameManager Instance
    {
        get
        {
            // if the instance hasn't been assigned then search for it
            if (m_Instance == null)
                m_Instance = FindObjectOfType(typeof(GlobalGameManager)) as GlobalGameManager;// why not just _instance = this; ??? ->FST

            return m_Instance;
        }
    }


    #region INSPECTOR VARS

    //Main game timer (in seconds). Always fixed.
    public float savetimescale;

    //additional time (based on the selected team) for p2 or AI
    public GameObject p1TimeBar;
    public GameObject p2TimeBar;

    public GameObject[] allPlayer;
    public GameObject[] all2Player;
    public GameObject[] allOpponentPlayer;

    public Transform ParentGoalPlaneObject;
    public GameObject goalPlane;
    public GameObject foulGoalPlane;
    public GameObject GoalAnimation;
    public GameObject SadAnimation;
    public GameObject Goal_AnimParent;

    [Header("Audio References")]
    public AudioClip countdownAudio;
    public AudioClip startWistle;
    public AudioClip finishWistle;
    public AudioClip[] goalSfx;
    public AudioClip[] goalHappenedSfx;

    [Space]

    //user to show win/lose result at the end of match
    public Texture2D[] statusModes;
    //Available status textures

    public Text playerGameStats, opponentsGameStats, winScreenPlayerName, winScreenOpponentName;
    public Text winScreenPlayerName_1, player1GameStats;
    public Text winScreenOpponentName_1, player2GameStats;
    public GameObject winText1, loseText1;
    public GameObject winText2, loseText2;
    // public GameObject shareText1, shareText2;
#pragma warning disable CS0649
    [SerializeField] private FST_CountdownTimer m_CountdownTimer;
    [SerializeField] private Text[] m_RematchDeclineText;
    [SerializeField] private GameObject m_OpponentReconnectPanel;
    [SerializeField] private GameObject m_ReconnectPanel;
    [SerializeField] private Text m_OpponentReconnectText;
    [SerializeField] private Text m_ReconnectText;
#pragma warning restore CS0649
    public GameObject RematchTextPlayer, RematchTextOpponentPlayer;//should be text component WIP ->FST
    public GameObject RematchTextPlayer2, RematchTextOpponentPlayer2;//should be text component WIP ->FST
    public GameObject winBackButton, winLeagueBackButton;
    public GameObject substitute_panel_button;

    public Text infoText;
    public Text infoLabel;

    #endregion


    #region STATIC VARS 

    public static bool isPenaltyKick;
    //You are free to change these positions at any time to customize the location of each element
    public static Vector3 penaltyKickStartPosition = new Vector3(0, -1, -0.5f);
    public static Vector3 penaltyKickGKPosition = new Vector3(13, -1, -0.5f);
    public static Vector3 penaltyKickBallPosition = new Vector3(5, -1, -0.5f);

    //Used just in penalty mode
    public static Vector3 playerDestination;    //destination for player unit after each penlaty round
    public static Vector3 AIDestination;    //destination for AI unit after each penlaty round

    /// <summary>
    /// Is it player ones turn?
    /// </summary>
    public static bool IsMastersTurn { get; set; }
    public bool IsMyTurn { get { return (IsStartMaster && IsMastersTurn) || ((!FST_Gameplay.IsMultiplayer || !IsStartMaster) && !IsMastersTurn); } }
    /// <summary>
    /// returns cached value from FST_SettingManager <br></br>
    /// 0 = playwithai, 1 = passnplay, 2 = oneonone, 3 = leagueGamePlay, 4 = offlinetournament, 5 = playWithFriends
    /// </summary>
    public static int MatchType { get; set; }

    /// <summary>
    /// count of how many rounds have been played during this match
    /// </summary>
    public static int Round { get; set; }

    //maximum distance that players can drag away from selected unit to shoot the ball (is in direct relation with shoot power)
    public static float maxArrowDragDistance = 10.0f;
    public static float minArrowDragDistance = 0.3f;
    public static float minPower = 10f;

    public static string winnerName;
    public static float gameTime;

    #endregion


    #region WORKING VARS
    private bool didPause = false;
    private int MasterGoals { get; set; } = 0;
    private int RemoteGoals { get; set; } = 0;
    public string Player1Name { get; set; } = "Player 1";
    private string m_P2Name = "Player 2";
    public string Player2Name { get { if (IsOfflineAIMatch) m_P2Name = "CPU"; else if (IsOfflinePassAndPlayMatch) m_P2Name = "Player 2"; return m_P2Name; } set { m_P2Name = value; } }
    public const string CPU_NAME = "CPU";

    //Game timer vars
    private float gameTimer = 300;//in seconds
    private int seconds;
    private int minutes;

    private bool isTimeBasedTimerFinished;
    private bool timeBased;

    private float p1TimeBarInitScale;
    private float p2TimeBarInitScale;

    private Vector3 ballStartingPosition = new Vector3(0.15f, -1.30f, -0.448f);

    public string playerNatureString;
    string stminaStr;

    private int m_ShotsBeforeGoalScoredCount = 0;
    //kept for later
#pragma warning disable IDE0052 // Remove unread private members
    private string lastInfoLabelText = "";
#pragma warning restore IDE0052 // Remove unread private members
    private string lastInfoText = "";
    private GamePhase lastPhase = GamePhase.NotStarted;

    private bool wasMP = false;

    #endregion


    #region GET SET VARS

    public float RoundTime { get; set; } = 15f; //total time allowed between each turn
    public int GoalLimit { get; set; } = 3;    //To finish the game quickly, without letting the GameTime end.

    public Rigidbody CurrentPlayer { get; set; } // following refs after convert to get set reveals that this could be handled better, in one script maybe ->FST
    public Rigidbody CurrentOpponent { get; set; } // following refs after convert to get set reveals that this could be handled better, in one script maybe ->FST
    public Rigidbody Current2Player { get; set; } // following refs after convert to get set reveals that this could be handled better, in one script maybe ->FST
    public bool IsDisconnected { get; set; }
    private bool CanPlayCrowdChants { get; set; } = false;
    public bool GameplayTimer_End { get; set; } = false;//get set and reset timer if false ->FST
    public bool IsMasterLeft { get; set; } = false;//set from FST_MPConnection
    public string CurrentPlayerName { get; set; }
    public string CurrentOpponentName { get; set; }

    #endregion


    #region HELPER GETTERS
    /// <summary>
    /// returns true for modes 0 or 4
    /// </summary>
    public static bool IsOfflineAIMatch { get { return MatchType == 0 || MatchType == 4; } }//check this is actually accuarate ->FST
    public static bool IsOfflinePassAndPlayMatch { get { return MatchType == 1; } }

    public Player RemotePlayer { get { return netPlayers[1]; } }

    public bool IsGameOver { get { return Phase == GamePhase.Finished; } }// for goal keeper controller, later wont be needed

    #endregion


    #region EXPECTED COMPONENTS LAZY INIT GETTERS
    private Image m_player1TimeBar = null;
    public Image Player1TimeBar { get { if (!m_player1TimeBar) m_player1TimeBar = p1TimeBar.GetComponent<Image>(); return m_player1TimeBar; } }

    private Image m_player2TimeBar = null;
    public Image Player2TimeBar { get { if (!m_player2TimeBar) m_player2TimeBar = p2TimeBar.GetComponent<Image>(); return m_player2TimeBar; } }

    private PenaltyController m_PenaltyControl = null;
    private PenaltyController PenaltyControl { get { if (!m_PenaltyControl) m_PenaltyControl = GetComponent<PenaltyController>(); return m_PenaltyControl; } }

    //Getter ->FST
    private PlayerAI m_PlayerAI = null;
    private PlayerAI GetPlayerAI
    {
        get
        {
            if (!m_PlayerAI)
            {
                m_PlayerAI = PlayerAI.instance;

                if (!m_PlayerAI)
                {
                    GameObject g = GameObject.FindGameObjectWithTag("playerAI");
                    if (g)
                        m_PlayerAI = g.GetComponent<PlayerAI>();
                    if (!m_PlayerAI)
                        m_PlayerAI = FindObjectOfType<PlayerAI>();
                }
            }
            return m_PlayerAI;
        }
    }


    //Getter ->FST
    private OpponentAI m_OppononentAI = null;
    private OpponentAI GetOpponentAi
    {
        get
        {
            if (!m_OppononentAI)
            {
                m_OppononentAI = OpponentAI.instance;

                if (!m_OppononentAI)
                {
                    GameObject g = GameObject.FindGameObjectWithTag("opponentAI");
                    if (g)
                        m_OppononentAI = g.GetComponent<OpponentAI>();
                    if (!m_OppononentAI)
                        m_OppononentAI = FindObjectOfType<OpponentAI>();
                }
            }
            return m_OppononentAI;
        }
    }



    private PlayerController[] m_AllPlayerControllers = null;// make null as default so getter knows to populate on first try ->FST
    private PlayerController[] AllPlayerControllers //Getter ->FST
    {
        get
        {
            if (m_AllPlayerControllers == null || m_AllPlayerControllers.Length < 1)//Check null and also check length to ensure it has been set otherwise....
            {
                //...create the new array...
                m_AllPlayerControllers = new PlayerController[allPlayer.Length];

                //... and populate it with the controllers
                for (int i = 0; i < allPlayer.Length; i++)
                    m_AllPlayerControllers[i] = allPlayer[i].GetComponent<PlayerController>();
            }
            //... return the cached array!!
            return m_AllPlayerControllers;
        }
    }

    private Player2Controller[] m_AllPlayer2Controllers = null;// make null as default so getter knows to populate on first try ->FST
    private Player2Controller[] AllPlayer2Controllers //Getter ->FST
    {
        get
        {
            if (m_AllPlayer2Controllers == null || m_AllPlayer2Controllers.Length < 1)//Check null and also check length to ensure it has been set otherwise....
            {
                //...create the new array...
                m_AllPlayer2Controllers = new Player2Controller[all2Player.Length];

                //... and populate it with the controllers
                for (int i = 0; i < all2Player.Length; i++)
                    m_AllPlayer2Controllers[i] = all2Player[i].GetComponent<Player2Controller>();
            }
            //... return the cached array!!
            return m_AllPlayer2Controllers;
        }
    }

    private OpponentUnitController[] m_AllOpponentControllers = null;// make null as default so getter knows to populate on first try ->FST
    private OpponentUnitController[] AllOpponentControllers //Getter ->FST
    {
        get
        {
            if (m_AllOpponentControllers == null || m_AllOpponentControllers.Length < 1)//Check null and also check length to ensure it has been set otherwise....
            {
                //...create the new array...
                m_AllOpponentControllers = new OpponentUnitController[allOpponentPlayer.Length];

                //... and populate it with the controllers
                for (int i = 0; i < allOpponentPlayer.Length; i++)
                    m_AllOpponentControllers[i] = allOpponentPlayer[i].GetComponent<OpponentUnitController>();
            }
            //... return the cached array!!
            return m_AllOpponentControllers;
        }
    }


    //Getter ->FST
    private FST_BallManager m_BallManager = null;
    private FST_BallManager GetBallManager
    {
        get
        {
            if (!m_BallManager)
                m_BallManager = FST_BallManager.Instance;

            if (!m_BallManager)
            {
                GameObject g = GameObject.FindGameObjectWithTag("ball");
                m_BallManager = g.GetComponent<FST_BallManager>();

                if (!m_BallManager)
                    m_BallManager = FindObjectOfType<FST_BallManager>();
            }

            return m_BallManager;
        }
    }



    //cached and initialiser ->FST
    private AudioSource m_AudioSource = null;
    private AudioSource GetAudioSource { get { if (!m_AudioSource) m_AudioSource = GetComponent<AudioSource>(); return m_AudioSource; } }



    #endregion


    #region CONSTANTS

    public const string PLAYER1_FLAG = "Player 1";
    public const string PLAYER2_FLAG = "Player 2";
    public const string OPPONENT_FLAG = "Opponent";

    #endregion


    #region NET SYNCED WORKING VARS

    private float p1TimeBarCurrentScale;
    private float p2TimeBarCurrentScale;

    Player[] netPlayers;//master = 0

    private float m_GameStartTime = 0;
    private float m_GameDurationMaster = 0;
    private float m_GameDurationRemote = 0;

    public bool IsStartMaster { get; private set; } = false;
    #endregion


    #region HASHTABLES

    /// <summary>
    /// used for debugging purpose only if FST_PhotonRoomAndPlayerDebug.cs is forced to use this.
    /// </summary>
    /// <returns></returns>
    public Hashtable GetCurrentProps()
    {
        return AssembleGameState();
    }

    /// <summary>
    /// Main State, this is intially passed to all players on match start.
    /// </summary>
    /// <returns>Main State values</returns>
    private Hashtable AssembleGameState()
    {
        Hashtable ht = new Hashtable
        {
            [RP_MASTERGOALS] = MasterGoals,
            [RP_REMOTEGOALS] = RemoteGoals,
            [RP_MASTERNAME] = netPlayers[0].NickName,
            [RP_REMOTENAME] = netPlayers[1].NickName,
            [RP_TURN] = IsMastersTurn,
            [RP_ROUND] = Round,
            [RP_PHASE] = Phase,
            [RP_SHOTSBEFOREGOAL] = m_ShotsBeforeGoalScoredCount,
            [RP_TIMER] = m_CountdownTimer.TimeLeft,
        };

        return ht;
    }

    /// <summary>
    /// A useful state to send the reconnecting player.
    /// </summary>
    /// <returns></returns>
    private Hashtable AssembleGameStateDisconnected()
    {
        Hashtable ht = new Hashtable
        {
            [RP_MASTERGOALS] = MasterGoals,
            [RP_REMOTEGOALS] = RemoteGoals,
            [RP_TURN] = IsMastersTurn,
            [RP_ROUND] = Round,
            [RP_PHASE] = Phase,
            [RP_SHOTSBEFOREGOAL] = m_ShotsBeforeGoalScoredCount,
            [RP_TIMER] = m_CountdownTimer.TimeLeft,
        };

        return ht;
    }

    /// <summary>
    /// Always use BEFORE turn changes only.
    /// </summary>
    /// <returns>Main State values trimmed for general gameplay</returns>
    private Hashtable AssembleGameStatePartial()
    {
        Hashtable ht = new Hashtable
        {
            [RP_MASTERGOALS] = MasterGoals,
            [RP_REMOTEGOALS] = RemoteGoals,
            [RP_TURN] = IsMastersTurn,
            [RP_ROUND] = Round,
            [RP_PHASE] = Phase,
            [RP_SHOTSBEFOREGOAL] = m_ShotsBeforeGoalScoredCount,
        };

        return ht;
    }

    /// <summary>
    /// used when the turn is set, these are the trimmed values needed.
    /// </summary>
    /// <returns></returns>
    private Hashtable AssembleGameStateTurn()
    {
        Hashtable ht = new Hashtable
        {
            [RP_TURN] = IsMastersTurn,
            [RP_ROUND] = Round,
            [RP_SHOTSBEFOREGOAL] = m_ShotsBeforeGoalScoredCount,
        };

        return ht;
    }

    #endregion


    #region MONOBEHAVIOUR CALLBACKS

    private void Awake()
    {
        //First off clear our logs! we made it here with no issues.
#if UNITY_EDITOR
        //  Assembly assembly = Assembly.GetAssembly(typeof(SceneView));
        var logEntries = Type.GetType("UnityEditor.LogEntries, UnityEditor.dll");
        // Type logEntries = assembly.GetType("UnityEditor.LogEntries,UnityEditor.dll");
        MethodInfo clearConsoleMethod = logEntries.GetMethod("Clear", BindingFlags.Static | BindingFlags.Public);
        clearConsoleMethod.Invoke(new object(), null);
#else
        Debug.ClearDeveloperConsole();
#endif

        IsStartMaster = FST_Gameplay.IsMaster;
        timeBased = FST_SettingsManager.IsTimeBased;
        MatchType = FST_SettingsManager.MatchType;

        if (IsOfflinePassAndPlayMatch)
        {
            //as per changes suggested
            Player1Name = "Player 1";
            Player2Name = "Player 2";
        }
        else Player1Name = FST_SettingsManager.PlayerName;

        GameManager.Instance.BgObjects.Clear();
        GameManager.Instance.TopCornerbarObject.Clear();
        GameManager.Instance.DownCornerbarObject.Clear();

        if (FST_Gameplay.IsMultiplayer)
        {
            wasMP = true;
            FST_AppHandler.OnAppSoftPause += OnSoftPause;

            if (PhotonNetwork.InRoom)
            {
                currentRoomName = PhotonNetwork.CurrentRoom.Name;
           //     PhotonNetwork.CurrentRoom.IsOpen = false;//now done when GameManager.CanLoadGameplayScene()
            }
        }
    }

    private void Start()
    {
        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { FST_PlayerProps.LOADED_LEVEL, true } });

        InitGamePlay();

        StartCoroutine(TryStartMatch());

        GoalLimit = GameManager.Instance.GoalsToWin;

        if (!UIManager.Instance.LoadingInGame.activeSelf)
            UIManager.Instance.LoadingInGame.SetActive(true);

        if (MatchType != 2)
        {
            if (IsOfflinePassAndPlayMatch || MatchType == 4 || MatchType == 5)
                substitute_panel_button.SetActive(false);
        }
    }

    private void OnEnable()
    {
        Application.quitting += Application_Quit;

        PhotonNetwork.NetworkingClient.EventReceived += OnNetworkEvent;

        FST_CountdownTimer.OnCountdownTimerHasExpired += OnCountDownExpired;

        FST_MPConnection.OnPlayerLeft += OnPlayerLeft;
        FST_MPConnection.OnPlayerEntered += OnPlayerEntered;
        FST_MPConnection.OnRoomUpdate += OnRoomUpdate;
        FST_MPConnection.OnMasterSwitched += OnMasterSwitched;
    }

    private void OnDisable()
    {
        Application.quitting -= Application_Quit;

        PhotonNetwork.NetworkingClient.EventReceived -= OnNetworkEvent;

        FST_CountdownTimer.OnCountdownTimerHasExpired -= OnCountDownExpired;

        FST_MPConnection.OnPlayerLeft -= OnPlayerLeft;
        FST_MPConnection.OnPlayerEntered -= OnPlayerEntered;
        FST_MPConnection.OnRoomUpdate -= OnRoomUpdate;
        FST_MPConnection.OnMasterSwitched -= OnMasterSwitched;

        if (wasMP)
        {
            FST_AppHandler.OnAppSoftPause -= OnSoftPause;
        }
    }

    private void Update()
    {
        object o;
        //all done
        if (Phase == GamePhase.Finished)
        {
            lastPhase = GamePhase.Finished;//as we wont get to the end here, we set the lastPhase now.
                                           //rematch check, only for ui here BUT, NOTE: hook this up to drive the other system, to save the other system running with no need...(in gamemanager rematch)
            if (didDecline)
                return;

            if (FST_Gameplay.IsMultiplayer)
            {
                if (!PhotonNetwork.InRoom)
                {
                    didDecline = true;
                    return;
                }

                if (PhotonNetwork.CurrentRoom.PlayerCount < 2 || netPlayers[1] == null || netPlayers[0] == null || MatchType == 3) //no rematch is possible
                {
                    OnDeclineRematch();
                    return;
                }

                if (netPlayers[0].CustomProperties.TryGetValue(FST_PlayerProps.REMATCH, out o))
                {
                    if ((int)o == 1)//rematch is desired by the master player
                    {
                        if (UIManager.Instance.winscreen_2.activeInHierarchy)
                        {
                            RematchTextPlayer2.SetActive(true);
                            RematchTextOpponentPlayer.SetActive(true);
                        }
                        else
                        {
                            RematchTextPlayer.SetActive(true);
                            RematchTextOpponentPlayer2.SetActive(true);
                        }
                    }
                    else if ((int)o == 2)//rematch is declined by the master player
                        OnDeclineRematch();
                }

                if (RemotePlayer.CustomProperties.TryGetValue(FST_PlayerProps.REMATCH, out o))
                {
                    if ((int)o == 1)//rematch is desired by the remote player
                    {
                        if (UIManager.Instance.winscreen_2.activeInHierarchy)
                        {
                            RematchTextPlayer.SetActive(true);
                            RematchTextOpponentPlayer2.SetActive(true);
                        }
                        else
                        {
                            RematchTextPlayer2.SetActive(true);
                            RematchTextOpponentPlayer.SetActive(true);
                        }
                    }
                    else if ((int)o == 2)//rematch is declined by the remote player
                        OnDeclineRematch();
                }
            }

            return;
        }

        //reconnect ui driven by timers
        if (FST_Gameplay.IsMultiplayer)
        {
            //only do this if we are actually in room and active, else upon disconnect opponent will appear as Inactive
            if (PhotonNetwork.InRoom && FST_MPConnection.InternetReachability != NetworkReachability.NotReachable)
            {
                if (IsP1Connected)
                {
                    if (netPlayers[0].IsInactive)
                    {
                        FST_MPDebug.Log("*Caught player 1 inactive!");
                        Debug.LogWarning("*Caught player 1 inactive!");
                        IsP1Connected = false;
                    }
                }

                if (IsP2Connected)
                {
                    if (netPlayers[1].IsInactive)
                    {
                        FST_MPDebug.Log("*Caught player 2 inactive!");
                        Debug.LogWarning("*Caught player 2 inactive!");
                        IsP2Connected = false;
                    }
                }
            }

            if (!IsAllPlayersConnected)
            {
                if (IsMeConnected)
                    ReconnectingOpponent();
                else Reconnecting();
            }
            else CancelReconnecting();

            if (reconnectTimer.Active)
            {
                if (!m_ReconnectPanel.activeInHierarchy)
                {
                  //  FST_MPDebug.Log("reconnectTimer > OpenReconnectingPanel!!!");
                  //  Debug.Log("reconnectTimer > OpenReconnectingPanel!!!");
                    m_ReconnectPanel.SetActive(true);
                }
                m_ReconnectText.text = "Reconnecting ..!\nTime remaining: " + (int)reconnectTimer.DurationLeft;

                // return; we should stop here really, and request our state be updated when we come back.
            }
            else
            {
                if (m_ReconnectPanel.activeInHierarchy)
                {
                  //  FST_MPDebug.Log("reconnectTimer Cancelled or finished > CloseReconnectingPanel!!!");
                  //  Debug.Log("reconnectTimer Cancelled or finished > CloseReconnectingPanel!!!");
                    m_ReconnectPanel.SetActive(false);
                }
            }

            if (reconnectOppTimer.Active)
            {
                if (!m_OpponentReconnectPanel.activeInHierarchy && (!IsMyTurn || Phase == GamePhase.RoundEnded || m_CountdownTimer.TimeLeft < 0))
                {
                  //  FST_MPDebug.Log("reconnectOppTimer > OpenReconnectingPanel!!!");
                  //  Debug.Log("reconnectOppTimer > OpenReconnectingPanel!!!");
                    m_OpponentReconnectPanel.SetActive(true);

                    if (FST_CountdownTimer.IsTimerRunning && Phase == GamePhase.RoundStarted && !IsMyTurn)
                    {
                        if (!didPause)
                        {
                            Player p = IsP1Connected ? netPlayers[1] : netPlayers[0];
                            FST_CountdownTimer.Instance.PauseTimer(true, p.NickName, true, p.IsInactive);
                            didPause = true;
                        }
                    }
                }
                m_OpponentReconnectText.text = "Please don't leave...\nWaiting for opponent to reconnect ..!\nTime remaining: " + (int)reconnectOppTimer.DurationLeft;

                // set props here but only when state is appropriate!
            }
            else
            {
                if (m_OpponentReconnectPanel.activeInHierarchy)
                {
                  //  FST_MPDebug.Log("reconnectOppTimer Cancelled or finished > CloseOpponentReconnectingPanel!!!");
                  //  Debug.Log("reconnectOppTimer Cancelled or finished > CloseOpponentReconnectingPanel!!!");
                    m_OpponentReconnectPanel.SetActive(false);
                }
            }

            if (IsAllPlayersConnected)
            {
                isWaitingToSendGoalData = false;

                if (MatchType != 3)//in league the prize amount is updated by FST_PrizepoolUpdater
                {
                    if (lastInfoText != infoText.text)
                    {
                        lastInfoText = infoText.text;
                        TransmitTextUpdate(lastInfoText);
                    }
                }
            }

            if (didPause && PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(FST_CountdownTimer.CountdownPaused, out o))
            {
                if (IsAllPlayersConnected)
                {
                    FST_MPDebug.Log("unpause");
                    Debug.Log("unpause");
                    m_CountdownTimer.Countdown = (float)o;
                    m_CountdownTimer.StartTimer();

                    didPause = false;
                }
            }

            //NOTE: this will pause time at start of game if requested.
            /*        else if (!IsAllPlayersConnected && Phase == GamePhase.NotStarted)
                    {
                        if (!didPause)
                        {
                           // FST_MPDebug.Log("pause");
                           // Debug.Log("pause");
                            //m_CountdownTimer.PauseTimer(true, PhotonNetwork.LocalPlayer.NickName, true, true);
                            didPause = true;
                        }
                    }*/

        }

        if (Phase != lastPhase)
        {
          //  FST_MPDebug.Log("New Phase = " + Phase + " : Last Phase = " + lastPhase);
          //  Debug.Log("New Phase = " + Phase + " : Last Phase = " + lastPhase);

            if (Phase == GamePhase.RoundEnded)
            {
                if (lastPhase == GamePhase.NotStarted)
                {
                    if (GetAudioSource.isPlaying)
                        GetAudioSource.Stop();
                    m_GameDurationMaster = 0;
                    m_GameDurationRemote = 0;
                    m_GameStartTime = Time.time;
                    PlaySfx(startWistle, true);
                    lastPhase = GamePhase.RoundEnded;
                    return;
                }
            }
            else
            {
                if (FST_Gameplay.IsMultiplayer && IsMyTurn)
                {
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { [RP_PHASE] = Phase });
                }
            }

            lastPhase = Phase;
        }

        //fill time limit bars, only in normal game mode
        if (!isPenaltyKick)
            UpdateTimeBars();

        if (Phase == GamePhase.FormationSelect)
            OpenInGameFormation();
        else CloseInGameFormation();

        if (Phase == GamePhase.NotStarted)
        {
            SetBallToCenter();
            return;
        }

        if (Phase == GamePhase.BetweenRounds)
        {
            if (IsAllPlayersConnected)
            {
                RefreshRoundTimer();
                Phase = GamePhase.RoundStarted;
            }
            else
            {
                //cannot change turns yet
                //wait for answer from opponent
            }

            return;
        }

        if (Phase == GamePhase.RoundStarted)
        {
            //every now and then, play some crowd chants
            if (FST_Gameplay.IsMaster && CanPlayCrowdChants)
                StartCoroutine(PlayCrowdChants());

            return;
        }

        if (Phase == GamePhase.RoundEnded)
        {
            if (!FST_Gameplay.IsMultiplayer)
            {
                SetNewRound();
                RoundTurnManager();
            }
            else
            {
                if (IsAllPlayersConnected)
                {
                    m_CountdownTimer.Countdown = RoundTime;

                    if (IsMyTurn)
                    {
                        SetNewRound();

                        PhotonNetwork.CurrentRoom.SetCustomProperties(AssembleGameStateTurn());

                        SwapOwners();
                    }

                    Phase = GamePhase.WaitingForRoomUpdate;
                }
            }

            return;
        }


        if (Phase == GamePhase.BallKicked)
        { return; }

        if (Phase == GamePhase.GoalIntermission)
        { return; }
    }

    #endregion


    #region EXTERNAL CALLBACKS

    private void Application_Quit()
    {
        Debug.Log("Application_Quit");
        QuitMatch();
        PhotonNetwork.Disconnect();
    }

    private void OnNetworkEvent(EventData eventData)
    {
        byte byteCode = eventData.Code;

        switch (byteCode)
        {
            case FST_ByteCodes.GOT_OB_ID:
                Joga_Data.MatchId = (string)eventData.CustomData;
              //  Debug.Log("Received Match Id!!");
                break;
        }
    }

    /// <summary>
    /// Triggered by FST_MPConnection.OnMasterSwitched
    /// </summary>
    /// <param name="newMaster"></param>
    private void OnMasterSwitched(Player newMaster)
    {
      //  masterSwitched = true;
    }

    /// <summary>
    /// Triggered by FST_MPConnection.OnRoomUpdate
    /// </summary>
    /// <param name="propertiesThatChanged"></param>
    private void OnRoomUpdate(Hashtable propertiesThatChanged)
    {
        if (string.IsNullOrEmpty(currentRoomName))
            currentRoomName = PhotonNetwork.CurrentRoom.Name;


        if (propertiesThatChanged.TryGetValue(RP_ROUND, out object o))
        {
            Round = (int)o;

            if (Round < 1)// the master was setting the rooms initial props
                return;
        }

        if (propertiesThatChanged.TryGetValue(RP_FORMATIONSET, out o))
        {
            if ((int)o > 1)
            {
                Phase = GamePhase.FormationClosed;
                if (FST_Gameplay.IsMaster 
                    && Round > 0)//in case of rematch, the formation screen is open at the start, we dont want to stop the next timer (start round), when the round begins (next frame) this is stopped in the start timer method.
                    m_CountdownTimer.StopTimer();
                Debug.Log("Both players have selected formations!!! lets plays right now!");
            }

            return;
        }

        if (propertiesThatChanged.TryGetValue(RP_SHOTSBEFOREGOAL, out o))
            m_ShotsBeforeGoalScoredCount = (int)o;

        if (propertiesThatChanged.TryGetValue(RP_GOAL, out o))
            GoalStatusInternal((string)o);

        if (propertiesThatChanged.TryGetValue(RP_TURN, out o))
        {
            IsMastersTurn = (bool)o;
            RoundTurnManager();
        }

        if (propertiesThatChanged.TryGetValue(RP_MASTERGOALS, out o))
            MasterGoals = (int)o;

        if (propertiesThatChanged.TryGetValue(RP_REMOTEGOALS, out o))
            RemoteGoals = (int)o;

        //if (propertiesThatChanged.TryGetValue(RP_PHASE, out o))
          //  lastPhase = Phase = (GamePhase)o;

        //if (propertiesThatChanged.TryGetValue(RP_TIMER, out o))
          //  m_CountdownTimer.Countdown = (float)o;

    }

    /// <summary>
    /// Triggered by FST_CountdownTimer.OnCountDownExpired
    /// </summary>
    private void OnCountDownExpired()
    {
        FST_MPDebug.Log("CountdownExpired, phase: " + Phase + ", lastPhase: " + lastPhase + ", AllConnected: " + IsAllPlayersConnected);
        Debug.Log("CountdownExpired, phase: " + Phase + ", lastPhase: " + lastPhase + ", AllConnected: " + IsAllPlayersConnected);

        if (!IsAllPlayersConnected)
        {
            FST_MPDebug.Log("CountdownExpired with disconnected player/s, phase: " + Phase + ", lastPhase: " + lastPhase);
            Debug.Log("CountdownExpired with disconnected player/s, phase: " + Phase + ", lastPhase: " + lastPhase);
        }

        if (FST_Gameplay.IsMultiplayer && !PhotonNetwork.InRoom)
            return;

        PlayerController.CanShoot = Player2Controller.CanShoot = false;

        switch (Phase)
        {
            case GamePhase.NotStarted:
                Phase = GamePhase.RoundEnded;
                break;


            case GamePhase.RoundStarted:
                Phase = GamePhase.RoundEnded;
                break;


            case GamePhase.BallKicked:
                //DoKick(false);
                break;


            case GamePhase.GoalIntermission:
                break;


            case GamePhase.FormationSelect:
                Phase = GamePhase.FormationClosed;
                break;


            case GamePhase.RoundEnded:

                break;


            case GamePhase.BetweenRounds:
                break;


            case GamePhase.Finished:
                break;


            default:
                break;
        }
    }

    /// <summary>
    /// Triggered by FST_MPConnection.OnPlayerEntered
    /// </summary>
    /// <param name="newPlayer">the player that entered</param>
    private void OnPlayerEntered(Player newPlayer)
    {
        //   string debugstring = "*****OnPlayerEntered(" + newPlayer.NickName + ") \nP1 was connected = " + IsP1Connected + ", P2 was connected = " + IsP2Connected +" *****";

        if (newPlayer == netPlayers[0])
            IsP1Connected = true;

        if (newPlayer == netPlayers[1])
            IsP2Connected = true;

        //  debugstring += "\n-Player 1 is now connected: " + IsP1Connected + ", Player 2 is now connected: " + IsP2Connected;

        //  FST_MPDebug.Log(debugstring + "\n ****** OnPlayerEntered() COMPLETE ******");
        //  Debug.Log(debugstring + "\n ****** OnPlayerEntered() COMPLETE ******");

        if (newPlayer == PhotonNetwork.LocalPlayer)
            RefreshGameState();
    }

    /// <summary>
    /// Triggered by FST_MPConnection.OnLeftRoom
    /// </summary>
    /// <param name="playerWhoLeft">the player that left</param>
    public void OnPlayerLeft(Player playerWhoLeft)
    {
        if (Phase == GamePhase.Finished)
            return;

        FST_MPDebug.Log("OnPlayerLeft(" + playerWhoLeft.NickName + ") IsMyTurn = " + IsMyTurn + ", I am in room now = " + PhotonNetwork.InRoom + ", playerWhoLeft.IsInactive = " + playerWhoLeft.IsInactive);
        Debug.Log("OnPlayerLeft(" + playerWhoLeft.NickName + ") IsMyTurn = " + IsMyTurn + ", I am in room now = " + PhotonNetwork.InRoom + ", playerWhoLeft.IsInactive = " + playerWhoLeft.IsInactive);

        if (playerWhoLeft == netPlayers[0])
            IsP1Connected = false;

        if (playerWhoLeft == netPlayers[1])
            IsP2Connected = false;
    }

    #endregion


    #region DISCONNECT, PAUSE, REJOIN

    public const string RP_FORMATIONSET = "frmcount";
    private const string RP_TURN = "trn";
    private const string RP_MASTERGOALS = "mstrGls";
    private const string RP_REMOTEGOALS = "rmtGls";
    private const string RP_MASTERNAME = "mstrName";
    private const string RP_REMOTENAME = "rmtName";
    private const string RP_ROUND = "round";
    private const string RP_GOAL = "goal";
    private const string RP_PHASE = "phase";
    private const string RP_SHOTSBEFOREGOAL = "shotsBeforeGoal";
    private const string RP_TIMER = "timer";

    public bool IsAllPlayersConnected { get { return m_IsP1Connected && m_IsP2Connected; } }

    private bool m_IsP1Connected = true;
    private bool IsP1Connected { get { return m_IsP1Connected; } set { m_IsP1Connected = value; FST_MPDebug.Log("IsP1Connected: " + m_IsP1Connected); Debug.Log("IsP1Connected: " + m_IsP1Connected); } }

    private bool m_IsP2Connected = true;
    private bool IsP2Connected { get { return m_IsP2Connected; } set { m_IsP2Connected = value; FST_MPDebug.Log("IsP2Connected: " + m_IsP2Connected); Debug.Log("IsP2Connected: " + m_IsP2Connected); } }

    private bool IsTheDisconnectedPlayerMe { get { return (!IsP2Connected && !IsStartMaster) || (!IsP1Connected && IsStartMaster); } }
    private bool IsOpponentConnected { get { return (IsP2Connected && IsStartMaster) || (IsP1Connected && !IsStartMaster); } }
    private bool IsMeConnected { get { return (IsP1Connected && IsStartMaster) || (IsP2Connected && !IsStartMaster); } }

    private int disconnectCountOpponent = 0;
    private int disconnectCount = 0;

    private readonly FST_Timer.Handle reconnectOppTimer = new FST_Timer.Handle();
    private readonly FST_Timer.Handle reconnectTimer = new FST_Timer.Handle();

    //when soft pause false, we use this to fake some time so we can see for debugging, this screen closes too fast!
    private readonly FST_Timer.Handle cancelHandle = new FST_Timer.Handle();

    private string currentRoomName = "";
    //   bool masterSwitched = false;

    /// <summary>
    /// Triggered by FST_AppHandler.cs, this function handles the local player leaving/joining via soft pause, and notifys the opponent.
    /// </summary>
    /// <param name="isPaused"></param>
    private void OnSoftPause(bool isPaused)
    {
        if (!wasMP)//only in multiplayer!
            return;

        if (!FST_AppHandler.LastSoftPauseWasInGame && !isPaused)
            return;

        //  FST_MPDebug.Log("Player Soft Paused: " + isPaused + ". In room = " + PhotonNetwork.InRoom);
        //  Debug.Log("Player Soft Paused: " + isPaused + ". In room = " + PhotonNetwork.InRoom);

        if (!PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.LocalPlayer == netPlayers[0])
                IsP1Connected = false;
            else IsP2Connected = false;

            if (!isPaused)
            {
                if (!string.IsNullOrEmpty(currentRoomName) && !didGameOver)
                    StartCoroutine(TryGetBackIntoGame(currentRoomName));
                else PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P1_DisconnectTimeOut : PlayerLeftReason.P2_DisconnectTimeOut);
            }
        }
        else
        {
            if (PhotonNetwork.LocalPlayer == netPlayers[0])
                IsP1Connected = !isPaused;
            else IsP2Connected = !isPaused;

            if (isPaused)
            {
                Reconnecting();
                m_ReconnectPanel.SetActive(true);//NOTE: also in this case we need to activate this screen now, android device will not catch the next update frame.
            }
            else FST_Timer.In(1.5f, () => CancelReconnecting(), cancelHandle);//IMPORTANT NOTE: we fake some time so we can see for debugging, this screen closes too fast, we can remove this in releases!, see CancelReconnecting for further comments.

            if (PhotonNetwork.NetworkClientState != ClientState.Leaving)
            {
                TransmitPlayerIsSoftPaused(isPaused);//Tell the opponent...
                PhotonNetwork.SendAllOutgoingCommands();//...and send it right now!
            }
        }
    }

    /// <summary>
    /// Upon a full disconnect, this will attempt to get us back in the game.
    /// </summary>
    /// <param name="roomName"></param>
    /// <returns></returns>
    private IEnumerator TryGetBackIntoGame(string roomName)
    {
        //   FST_MPDebug.Log("TryGetBackIntoGame()");
        //  Debug.Log("TryGetBackIntoGame()");

        while (reconnectTimer.Active && !didGameOver)
        {
            if (!FST_MPConnection.Connected)
                yield break;//fst mpconnection will be handling this already

            if (PhotonNetwork.NetworkClientState == ClientState.Joining
                || PhotonNetwork.NetworkClientState == ClientState.JoiningLobby
                || PhotonNetwork.NetworkClientState == ClientState.ConnectingToMasterServer
                || PhotonNetwork.NetworkClientState == ClientState.Authenticating
                || !PhotonNetwork.IsConnectedAndReady)
                yield return 0;

            if (PhotonNetwork.RejoinRoom(roomName))
                yield break;

            yield return 0;
        }

        if (PhotonNetwork.InRoom)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount < (byte)2)
            {
                FST_MPDebug.Log("REJOINED BUT OTHER PLAYER IS GONE!");
                Debug.Log("REJOINED BUT OTHER PLAYER IS GONE!");
                //UIManager.Instance.OpenOppoReconnectingPanel();
                PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P1_DisconnectTimeOut : PlayerLeftReason.P2_DisconnectTimeOut);
            }
            else
            {
                FST_MPDebug.Log("REJOINED!");
                Debug.Log("REJOINED!");
                if (!reconnectTimer.Active)
                {
                    FST_MPDebug.Log("REJOINED AND TIMER IS ALREADY CANCELLED!");
                    Debug.Log("REJOINED AND TIMER IS ALREADY CANCELLED!");
                    //PlayerLeftWinPopup(!IsStartMaster ? PlayerLeftReason.P1_DisconnectTimeOut : PlayerLeftReason.P2_DisconnectTimeOut);
                }
            }
        }
    }

    private void ReconnectingOpponent()
    {
        if (!reconnectOppTimer.Active)
        {
            disconnectCountOpponent++;

            FST_MPDebug.Log("Opponent disconnect count this match = " + disconnectCountOpponent);
            Debug.Log("Opponent disconnect count this match = " + disconnectCountOpponent);

            if (disconnectCountOpponent > FST_MPConnection.Instance.MaxDisconnectsPerGame)
            {
                Debug.Log("Opponent disconnected more than the allowed limit, game will now end");
                FST_MPDebug.Log("Opponent disconnected more than the allowed limit, game will now end");
                PlayerLeftWinPopup(!IsStartMaster ? PlayerLeftReason.P1_DisconnectCountTooHigh : PlayerLeftReason.P2_DisconnectCountTooHigh);
                return;
            }

            FST_MPDebug.Log("Opponent D/C timer start! Phase: " + Phase + ", lastPhase: " + lastPhase);
            Debug.Log("Opponent D/C timer start! Phase: " + Phase + ", lastPhase: " + lastPhase);

            FST_Timer.In(FST_MPConnection.Instance.BackgroundTimeout, delegate ()
            {
                FST_MPDebug.Log("PlayerLeftWinPopup from background timeout! Opponent was D/C");
                Debug.Log("PlayerLeftWinPopup from background timeout! Opponent was D/C");
                PlayerLeftWinPopup(!IsStartMaster ? PlayerLeftReason.P1_DisconnectTimeOut : PlayerLeftReason.P2_DisconnectTimeOut);
            }, reconnectOppTimer);
        }
    }

    private void Reconnecting()
    {
        if (!reconnectTimer.Active)
        {
            disconnectCount++;

            FST_MPDebug.Log("Disconnect count this match = " + disconnectCount);
            Debug.Log("Disconnect count this match = " + disconnectCount);

            if (disconnectCount > FST_MPConnection.Instance.MaxDisconnectsPerGame)
            {
                Debug.Log("Disconnected more than the allowed limit, game will now end");
                FST_MPDebug.Log("Disconnected more than the allowed limit, game will now end");
                //  FST_MPConnection.Instance.LeaveRoom(false, false);
                PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P1_DisconnectCountTooHigh : PlayerLeftReason.P2_DisconnectCountTooHigh);
            }
            else
            {

                FST_MPDebug.Log("OpenReconnectingPanel! Phase: " + Phase + ", lastPhase: " + lastPhase);
                Debug.Log("OpenReconnectingPanel! Phase: " + Phase + ", lastPhase: " + lastPhase);

                FST_Timer.In(FST_MPConnection.Instance.BackgroundTimeout, delegate ()
                {
                    FST_MPDebug.Log("PlayerLeftWinPopup from background timeout! I was D/C");
                    Debug.Log("PlayerLeftWinPopup from background timeout! I was D/C");
                    //  FST_MPConnection.Instance.LeaveRoom(false, false);
                    PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P1_DisconnectTimeOut : PlayerLeftReason.P2_DisconnectTimeOut);
                },
                reconnectTimer);
            }
        }
    }

    private void CancelReconnecting()
    {
        //IMPORTANT NOTE: when soft pause false, fake some time so we can see for debugging, this screen closes too fast, we can remove this in releases!
        if (cancelHandle.Active)
            return;

        if (IsMeConnected)
        {
            if (reconnectTimer.Active)
            {
                // Debug.Log("Cancel Reconnect Timer!");
                reconnectTimer.Cancel();
            }
        }

        if (IsOpponentConnected)
        {
            if (reconnectOppTimer.Active)
            {
                //  Debug.Log("Cancel Opponent Reconnect Timer!");
                reconnectOppTimer.Cancel();
            }
        }
    }

    /// <summary>
    /// Used for the player that is reconnecting to refresh the gamestate. Gets the state of the game from the room properties.
    /// </summary>
    private void RefreshGameState()
    {
        Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

        string debugString = "******************RefreshGameState********************\n*************STATE RETRIEVED*************\n" + ht.ToString()
           + "\n************MY CURRENT STATE************\n" + AssembleGameState().ToString();

        int i;
        string lastGoalBy = "";
        if (ht.TryGetValue(RP_MASTERGOALS, out object o))
        {
            i = (int)o;
            if (i > MasterGoals)
            {
                MasterGoals = i;
                lastGoalBy = PLAYER1_FLAG;
                GameManager.Instance.PlayergoalsText.text = MasterGoals.ToString();
            }
        }
        if (ht.TryGetValue(RP_REMOTEGOALS, out o))
        {
            i = (int)o;
            if (i > RemoteGoals)
            {
                RemoteGoals = i;
                lastGoalBy = PLAYER2_FLAG;
                GameManager.Instance.OpponentgoalsText.text = RemoteGoals.ToString();
            }
        }

        ManageGameStatus();

        if (Phase == GamePhase.Finished)
        {
            GameOver();
            return;
        }

        //Player1Name = (string)ht[RP_MASTERNAME];
        //Player2Name = (string)ht[RP_REMOTENAME];

        if (ht.TryGetValue(RP_TURN, out o))
            IsMastersTurn = (bool)o;

        if (ht.TryGetValue(RP_ROUND, out o))
        {
            i = (int)o;
            if (i > Round)
                Round = i;
        }

        if (ht.TryGetValue(RP_SHOTSBEFOREGOAL, out o))
            m_ShotsBeforeGoalScoredCount = (int)o;

        if (ht.TryGetValue(RP_PHASE, out o))
        {
            GamePhase phase = (GamePhase)o;

            if (phase == GamePhase.BallKicked)
            {
                FST_MPDebug.Log("RefreshGameState AFTER BALL KICKED");
                Debug.Log("RefreshGameState AFTER BALL KICKED");
                DoKick(false);//no need to transmit this one as we just received it!
            }

            else if (phase == GamePhase.RoundStarted)
            {
                PlayerController.Instance.arrowPlane.transform.localScale = Vector3.zero;
                PlayerController.CanShoot = IsMastersTurn;
                Player2Controller.CanShoot = !IsMastersTurn;
            }
            else if (phase == GamePhase.GoalIntermission)
            {
                FST_MPDebug.Log("RefreshGameState WHILE GOAL INTERMISSION, lastGoalBy: " + lastGoalBy);
                Debug.Log("RefreshGameState WHILE GOAL INTERMISSION, lastGoalBy: " + lastGoalBy);
                ManagePostGoal(lastGoalBy);
            }
            else if (phase == GamePhase.FormationSelect)
            {
                FST_MPDebug.Log("RefreshGameState WHILE FORMATION SHOULD BE OPENED, lastGoalBy: " + lastGoalBy);
                Debug.Log("RefreshGameState WHILE FORMATION SHOULD BE OPENED, lastGoalBy: " + lastGoalBy);
                OpenInGameFormation();
            }

            Phase = phase;
        }


        //if (ht.TryGetValue(RP_FORMATIONSET, out o))
        //{ }
        //if (ht.TryGetValue(RP_TIMER, out o))
        //{ }

        if (UIManager.Instance.LoadingInGame.activeSelf)
            UIManager.Instance.LoadingInGame.SetActive(false);

        SwapOwners();

        debugString += "\n*******MY STATE AFTER APPLICATION*******\n" + AssembleGameState().ToString() + "\n*****************************************";

        FST_MPDebug.Log(debugString);
        Debug.Log(debugString);
    }

    /// <summary>
    /// Online usage, this method transfers the ownership to the player that is having the turn
    /// </summary>
    public void SwapOwners()
    {
        //    FST_MPDebug.Log("SwapOwners: master turn = " + IsMastersTurn);
        //    Debug.Log("SwapOwners: master turn = " + IsMastersTurn);

        FST_DiskPlayerManager.Instance.TransferOwnerShip(IsMastersTurn ? netPlayers[0] : netPlayers[1]);

        if (netPlayers[0] == null)
        {
            // Debug.Log("Master = NULL");
            //is called again upon reconnect
            return;
        }
        if (netPlayers[1] == null)
        {
            // Debug.Log("Client = NULL");
            //is called again upon reconnect
            return;
        }
    }

    private bool CheckMasterStatus()
    {
        if (!FST_Gameplay.IsMultiplayer)
            return true;

        if (!IsStartMaster)
            return false;

        if (netPlayers[0] == null)
        {
            //  Debug.LogError("MASTER CLIENT IS NULL");
            return false;
        }

        if (!netPlayers[0].IsMasterClient)
        {
            //FST_DiskPlayerManager.Instance.TransmitAllPositions();
            if (!netPlayers[0].IsInactive && PhotonNetwork.SetMasterClient(netPlayers[0]))
            {
                // Debug.Log("FIXED MASTER CLIENT");
            }
            else
            {
                //       Debug.LogError("WRONG MASTER");
                return false;
            }
        }
        return true;
    }

    #endregion


    #region RPC TRANSMIT AND RECEIVE

    private void TransmitPlayerLeftWinPopUp(PlayerLeftReason reason, Player targetPlayer)
    {
        photonView.RPC("RPC_ReceivePlayerWinPopUp", targetPlayer, reason);
        PhotonNetwork.SendAllOutgoingCommands();
    }

    [PunRPC]
    private void RPC_ReceivePlayerWinPopUp(PlayerLeftReason reason)
    {
        PlayerLeftWinPopup(reason, false);
    }

    private void TransmitPlayerIsSoftPaused(bool paused)
    {
        // FST_MPDebug.Log("TransmitPlayerIsSoftPaused(" + paused + ")");
        // Debug.Log("TransmitPlayerIsSoftPaused(" + paused + ")");
        photonView.RPC("RPC_ReceivePlayerIsSoftPaused", RpcTarget.Others, paused);
    }

    [PunRPC]
    private void RPC_ReceivePlayerIsSoftPaused(bool isPaused, PhotonMessageInfo info)
    {
        //  FST_MPDebug.Log("Receive Player Soft Paused: " + isPaused);
        // Debug.Log("Receive Player Soft Paused: " + isPaused);
        if (isPaused)
            OnPlayerLeft(info.Sender);
        else OnPlayerEntered(info.Sender);
    }

    private void TransmitGameStateFull(Player targetPlayer = null)
    {
        if (targetPlayer != null)
        {
            //FST_MPDebug.Log("TransmitGameStateFull to " + targetPlayer.NickName);
            //Debug.Log("TransmitGameStateFull to " + targetPlayer.NickName);
            photonView.RPC("RPC_ReceiveGameStateFull", targetPlayer, AssembleGameState());
        }
        else
        {
            //FST_MPDebug.Log("TransmitGameStateFull for match start");
            //Debug.Log("TransmitGameStateFull for match start");
            photonView.RPC("RPC_ReceiveGameStateFull", RpcTarget.Others, AssembleGameState());
        }
    }

    [PunRPC]
    private void RPC_ReceiveGameStateFull(Hashtable ht, PhotonMessageInfo info)
    {
        //string debugString = "GOT FULL GAME STATE! from " + info.Sender.NickName
        //    + "\n*************STATE RECEIVED*************\n" + ht.ToString()
        //    + "\n************MY CURRENT STATE************\n" + AssembleGameState().ToString();

        IsMastersTurn = (bool)ht[RP_TURN];

        int i = (int)ht[RP_SHOTSBEFOREGOAL];

        m_ShotsBeforeGoalScoredCount = i;

        i = (int)ht[RP_MASTERGOALS];
        if (i > MasterGoals)
            MasterGoals = i;

        i = (int)ht[RP_REMOTEGOALS];
        if (i > RemoteGoals)
            RemoteGoals = i;

        i = (int)ht[RP_ROUND];
        if (i > Round)
            Round = i;

        Phase = (GamePhase)ht[RP_PHASE];

        Player1Name = (string)ht[RP_MASTERNAME];
        Player2Name = (string)ht[RP_REMOTENAME];

        GameManager.Instance.PlayergoalsText.text = MasterGoals.ToString();
        GameManager.Instance.OpponentgoalsText.text = RemoteGoals.ToString();

        if (Phase == GamePhase.RoundStarted)
        {
            PlayerController.Instance.arrowPlane.transform.localScale = Vector3.zero;
            PlayerController.CanShoot = IsMastersTurn;
            Player2Controller.CanShoot = !IsMastersTurn;
        }

        if (UIManager.Instance.LoadingInGame.activeSelf)
            UIManager.Instance.LoadingInGame.SetActive(false);

        SwapOwners();

      //  debugString += "\n*******MY STATE AFTER APPLICATION*******\n" +  AssembleGameState().ToString();

      //  FST_MPDebug.Log(debugString);
      //  Debug.Log(debugString);
    }

    private void TransmitPhaseUpdate(GamePhase phase)
    {
        FST_MPDebug.Log("TransmitPhaseUpdate(" + phase + ")");
        Debug.Log("TransmitPhaseUpdate(" + phase + ")");
        photonView.RPC("RPC_ReceivePhaseUpdate", RpcTarget.Others, (int)phase);
    }

    [PunRPC]
    private void RPC_ReceivePhaseUpdate(int phase, PhotonMessageInfo info)
    {
        LogDelay("RPC_ReceivePhaseUpdate(" + (GamePhase)phase + ")", info);
        Phase = (GamePhase)phase;
        lastPhase = Phase;// prevent double sends
    }

    private void TransmitKick()
    {
        FST_MPDebug.Log("TransmitKick");
        Debug.Log("TransmitKick");
        photonView.RPC("RPC_ReceiveKick", RpcTarget.Others);
    }

    [PunRPC]
    private void RPC_ReceiveKick(PhotonMessageInfo info)
    {
        LogDelay("RPC_ReceiveKick", info);
        FST_DiskPlayerManager.Instance.DisableShootHelperAfterKick();
        DoKick(false);
    }

    //NOTE: info label is used for start time display and round time now.. but in case we need something later..
    private void TransmitTextLabelUpdate(string mssg)
    {
        photonView.RPC("RPC_ReceiveTextLabelUpdate", RpcTarget.OthersBuffered, mssg);
    }

    [PunRPC]
    private void RPC_ReceiveTextLabelUpdate(string mssg)
    {
        lastInfoLabelText = mssg; // later we may want this sent from client, this will prevent duplicate sends. currently only master send the command for this
        infoLabel.text = mssg;
    }
    private void TransmitTextUpdate(string mssg)
    {
        photonView.RPC("RPC_ReceiveTextUpdate", RpcTarget.OthersBuffered, mssg);
    }
    [PunRPC]
    private void RPC_ReceiveTextUpdate(string mssg)
    {
        lastInfoText = mssg; // later we may want this sent from client, this will prevent duplicate sends. currently only master send the command for this
        infoText.text = mssg;
    }


    private void TransmitFormationReset()
    {
        photonView.RpcSecure("RPC_ReceiveFormationReset", RpcTarget.Others, false);
    }

    [PunRPC]
    private void RPC_ReceiveFormationReset()
    {
        GetPlayerAI.SetFormation();
        SetBallToCenter();
        // Debug.Log("Got Formation reset!");
    }

    /// <summary>
    /// used for debugging the delay between messages
    /// </summary>
    /// <param name="methodName"></param>
    /// <param name="info"></param>
    private void LogDelay(string methodName, PhotonMessageInfo info)
    {
        int delay = PhotonNetwork.ServerTimestamp - info.SentServerTimestamp;
        FST_MPDebug.Log(methodName + ": DELAY = " + delay + "ms");
        Debug.Log(methodName + ": DELAY = " + delay + "ms");
    }

    #endregion


    #region MATCH START AND REMATCH FUNCTIONS

    private bool didDecline = false;
    private void OnDeclineRematch()
    {
        //turn off the buttons
        UIManager.Instance.Rematch1.SetActive(false);
        UIManager.Instance.Rematch2.SetActive(false);
        if (MatchType == 3)//league
        {
            m_RematchDeclineText[0].gameObject.SetActive(false);
            m_RematchDeclineText[1].gameObject.SetActive(false);
        }
        else
        {
            for (int i = 0; i < UIManager.Instance.leftPlayerText.Length; i++)
            {
                UIManager.Instance.leftPlayerText[i].GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
                // if (!onPurpose)//update the text to show the user that player was disconnected by internet foul. (its default value is "Player Left The Match");
                UIManager.Instance.leftPlayerText[i].GetComponent<Text>().text = "I Am Busy";

                //show the resulting text
                UIManager.Instance.leftPlayerText[i].SetActive(true);
            }
            // m_RematchDeclineText[0].gameObject.SetActive(true);
            //  m_RematchDeclineText[1].gameObject.SetActive(true);
        }
        RematchTextOpponentPlayer.SetActive(false);
        RematchTextPlayer2.SetActive(false);
        RematchTextPlayer.SetActive(false);
        RematchTextOpponentPlayer2.SetActive(false);

        didDecline = true;
    }

    public void TryStartRematch()
    {
        StartCoroutine(TryStartMatch());
    }

    private IEnumerator TryStartMatch()
    {
        Phase = GamePhase.NotStarted;

        yield return new WaitUntil(() => CanStartMatch());

        if (PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(FST_PlayerProps.REMATCH, out object o))
        {
            bool rematch = (int)o == 1;
            if (rematch)
            {
                Debug.Log("IS rematch begin..");
                PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.REMATCH, 0 } });//refresh it now before the 10 second wait.
                UIManager.Instance.gameStatusPlane.SetActive(false);
                UIManager.Instance.winscreen_2.SetActive(false);

                Phase = GamePhase.FormationSelect;

                yield return new WaitUntil(() => Phase != GamePhase.FormationSelect);

                lastPhase = Phase = GamePhase.NotStarted;

                if (FST_Gameplay.IsMultiplayer && FST_Gameplay.IsMaster)//reset the rooms working props.
                    PhotonNetwork.CurrentRoom.SetCustomProperties(AssembleGameStatePartial());

            }
            else PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.REMATCH, 0 } });// no wait and no rematch, refresh it now in case of old values 
        }
        
        StartMatch();
    }


    /// <summary>
    /// This will determine if a Game can be started
    /// </summary>
    /// <returns>true if offline, true if all rules are passed online </returns>
    private bool CanStartMatch()
    {
        if (!GameManager.Instance)
        {
            Debug.LogError("GlobalGameManager > CanStartMatch() > No GameManager Instance assigned!");
            return false;
        }

        if (!GameManager.Instance.PlayernameText)
            return false;

        GameManager.Instance.PlayernameText.text = Player1Name;

        if (!GameManager.Instance.OpponentNameText)
            return false;

        GameManager.Instance.OpponentNameText.text = Player2Name;

        if (!FST_Gameplay.IsMultiplayer)
            return true;

        if (!PhotonNetwork.IsConnectedAndReady)
            return false;

        if (PhotonNetwork.CurrentRoom == null)
            return false;

        if (PhotonNetwork.CurrentRoom.PlayerCount < 2)
            return false;

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i].CustomProperties.TryGetValue(FST_PlayerProps.LOADED_LEVEL, out object o))
            {
                if ((bool)o == false)
                    return false;
            }
            else return false;
        }

        return true;
    }

    public void AssignPlayers()
    {
        netPlayers = new Player[PhotonNetwork.PlayerList.Length];
        netPlayers[0] = PhotonNetwork.MasterClient;
        Player1Name = netPlayers[0].NickName;

        Player[] players = PhotonNetwork.PlayerList;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].IsMasterClient)
            {
                netPlayers[1] = players[i];
                Player2Name = netPlayers[1].NickName;
            }
        }

        if (FST_Gameplay.IsMultiplayer && FST_Gameplay.IsMaster)//set the room props, these will be needed when a player reconnects.
            PhotonNetwork.CurrentRoom.SetCustomProperties(AssembleGameState());  //   TransmitGameStateFull();

        //if (FST_Gameplay.IsMaster)//NOTE: skip this check just for now until the mats are synced appropriatly, for now it simply allow the clients mats to be set locally
        GetPlayerAI.SetFormation();

       // Debug.Log("AssignPlayers() : Player1 = " + Player1Name + ", Player2 = " + Player2Name);
    }

    private void StartMatch()
    {
        GetPlayerAI.SetFormation();

        PlaySfx(countdownAudio, false);
        if (FST_Gameplay.IsMultiplayer)
        {
            m_CountdownTimer.Countdown = 6;

            if (FST_Gameplay.IsMaster)
                m_CountdownTimer.StartTimer();
        }
        else StartCoroutine(CountDownOffline());
    }
    private IEnumerator CountDownOffline()
    {
        if (UIManager.Instance.LoadingInGame.activeSelf)
            UIManager.Instance.LoadingInGame.SetActive(false);

        infoLabel.text = "Game Starts in... 3";
        yield return new WaitForSecondsRealtime(1);
        infoLabel.text = "Game Starts in... 2";
        yield return new WaitForSecondsRealtime(1);
        infoLabel.text = "Game Starts in... 1";
        yield return new WaitForSecondsRealtime(1);
        infoLabel.text = "";
        OnCountDownExpired();
    }
   

    /// <summary>
    /// We have all units inside the game scene by default, but at the start of the game,
    /// we check which side in playing (should be active) and deactive the side that is
    /// not playing by deactivating all it's units.
    /// </summary>
    public void InitGamePlay()
    {
        CurrentPlayer = null;
        CurrentOpponent = null;
        Current2Player = null;
        MasterGoals = 0;
        RemoteGoals = 0;
        gameTime = 0;
        Round = 0;
        seconds = 0;
        minutes = 0;
        CanPlayCrowdChants = true;
        m_ShotsBeforeGoalScoredCount = 0;

        //Check if this is a penalty game or a normal match
        isPenaltyKick = (PlayerPrefs.GetInt("IsPenalty") == 1) ? true : false;

        //To avoid null reference errors caused by running the penalty scene without opening it from main menu,
        //we need to add the following lines. You should remove this in your live game.
        if (SceneManager.GetActiveScene().name == "Penalty-c#") //just to avoid null errors
            isPenaltyKick = true;

        SetDestinationForPenaltyMode(); //init the positions

        //Translate gameTimer index to actual seconds
        if (timeBased)
        {
            // As per MVP task , 5 min. game
            gameTimer = 300;
        }

        //fill player shoot timer to full (only in normal game mode, where these objects are available)
        if (!isPenaltyKick)
        {
            p1TimeBarInitScale = p2TimeBarInitScale = p1TimeBarCurrentScale = p2TimeBarCurrentScale = p1TimeBar.transform.localScale.x;
            p1TimeBar.transform.localScale = p2TimeBar.transform.localScale = new Vector3(1, 1, 1);
        }

        switch (MatchType)
        {
            case 0:
                for (int i = 0; i < all2Player.Length; i++)
                    all2Player[i].SetActive(false);
                break;

            case 1:
                for (int i = 0; i < allOpponentPlayer.Length; i++)
                    allOpponentPlayer[i].SetActive(false);
                GetOpponentAi.enabled = false;
                break;

            case 2:
                //find and deactive all AI Opponent units. This is Player-1 vs Player-2.
                for (int i = 0; i < allOpponentPlayer.Length; i++)
                    allOpponentPlayer[i].SetActive(false);
                //deactive opponent's AI
                GetOpponentAi.enabled = false;
                break;
            case 3:
                //find and deactive all AI Opponent units. This is Player-1 vs Player-2.
                for (int i = 0; i < allOpponentPlayer.Length; i++)
                    allOpponentPlayer[i].SetActive(false);
                //deactive opponent's AI
                GetOpponentAi.enabled = false;
                break;
            case 4:
                for (int i = 0; i < all2Player.Length; i++)
                    all2Player[i].SetActive(false);
                break;
            case 5:
                //find and deactive all AI Opponent units. This is Player-1 vs Player-2.
                for (int i = 0; i < allOpponentPlayer.Length; i++)
                    allOpponentPlayer[i].SetActive(false);
                //deactive opponent's AI
                GetOpponentAi.enabled = false;
                break;
        }

        if (FST_Gameplay.IsMultiplayer)
        {
            AssignPlayers();
        }
    }

    #endregion


    #region GAME LOGIC AND FUNCTIONS

    public void DoKick(bool transmit = true)
    {
        if (FST_Gameplay.IsMultiplayer && transmit)
        {
            TransmitKick();
            FST_DiskPlayerManager.Instance.TransmitShootHelperActive(false);
        }

        StartCoroutine(ManagePostShot());
    }

    /// <summary>
    /// determine the position of each side on the field for penalty mode
    /// </summary>
    private void SetDestinationForPenaltyMode()
    {
        if (IsMastersTurn)
        {
            playerDestination = penaltyKickGKPosition;
            AIDestination = penaltyKickStartPosition;
        }
        else
        {
            playerDestination = penaltyKickStartPosition;
            AIDestination = penaltyKickGKPosition;
        }
    }

    private void SetNewRound()
    {
        FST_MPDebug.Log("SetNewRound()");
        Debug.Log("SetNewRound()");

        IsMastersTurn = !IsMastersTurn;

        //add to round counter
        Round++;

        CurrentPlayer = Current2Player = CurrentOpponent = null;

        PlayerController.Instance.arrowPlane.transform.localScale = Vector3.zero;
        PlayerController.CanShoot = false;

        if (MatchType != 0)
        {
            Player2Controller.CanShoot = false;
        }
        else
        {//vs AI
            OpponentAI.opponentCanShoot = false;
            for (int i = 0; i < AllOpponentControllers.Length; i++)
                AllOpponentControllers[i].canShowSelectionCircle = false;
        }

        CurrentPlayerName = "";
    }

    /// <summary>
    /// This function gives turn to players in the game. EndPhase = BetweenRounds
    /// </summary>
    private void RoundTurnManager()
    {
        FST_MPDebug.Log("RoundTurnManager()");
        Debug.Log("RoundTurnManager()");

        CurrentPlayer = Current2Player = CurrentOpponent = null;

        PlayerController.Instance.arrowPlane.transform.localScale = Vector3.zero;
        PlayerController.CanShoot = IsMastersTurn;

        if (MatchType != 0)
        {
            Player2Controller.CanShoot = !IsMastersTurn;
        }
        else
        {//vs AI
            OpponentAI.opponentCanShoot = !IsMastersTurn;
            for (int i = 0; i < AllOpponentControllers.Length; i++)
                AllOpponentControllers[i].canShowSelectionCircle = !IsMastersTurn;
        }

        CurrentPlayerName = "";

        if (UIManager.Instance.LoadingInGame.activeSelf)
            UIManager.Instance.LoadingInGame.SetActive(false);

        Phase = GamePhase.BetweenRounds;
    }

    private void RefreshRoundTimer()
    {
        //fill time limit bars, only in normal game mode
        if (isPenaltyKick)
            return;

     //   Debug.Log("RefreshRoundTimer()");

        if (FST_Gameplay.IsMultiplayer)
        {
            m_CountdownTimer.Countdown = RoundTime;
            if (FST_Gameplay.IsMaster)
                m_CountdownTimer.StartTimer();
        }
        else
        {
            //  FST_Timer.In(RoundTime, CountDownExpired);
            FST_MPClock.Set(RoundTime);//base this on whos turn it is when this gets reset
        }

        p1TimeBarCurrentScale = p1TimeBarInitScale;
        Player1TimeBar.fillAmount = 1;
        p2TimeBarCurrentScale = p2TimeBarInitScale;
        Player2TimeBar.fillAmount = 1;
    }

    //NOTE: with a few more lines we wont even need IsMasterGoalNet, we could decide it by whos turn it is now and which end of field its at now. This works well for now.
    public void GoalStatus(bool isMastersGoalNet)
    {
        string s;
        if (isMastersGoalNet)
            s = IsOfflineAIMatch ? OPPONENT_FLAG : PLAYER2_FLAG;
        else s = PLAYER1_FLAG;

        if (FST_Gameplay.IsMultiplayer)
        {
            if (IsMyTurn)
            {
                if (FST_Gameplay.IsMultiplayer && PhotonNetwork.InRoom)//save these values in case players dropout, it will be restored upon reconnect.
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RP_MASTERGOALS, out object o))
                        MasterGoals = (int)o;
                    if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue(RP_REMOTEGOALS, out o))
                        RemoteGoals = (int)o;
                }


                //IMPORTANT, ITS BETTER WITH A WAIT SO BOTH PLAYERS ARE SYNCED!!!!! BUT THIS IS WHAT THEY WANT FOR NOW.... uncomment below two commented lines for this to be better!
             //   if (IsAllPlayersConnected)
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { [RP_GOAL] = s, });
              //  else StartCoroutine(WaitToDoGoal(s));
            }

            return;
        }

        GoalStatusInternal(s);
    }


    private bool isWaitingToSendGoalData = false;
    private IEnumerator WaitToDoGoal(string s)
    {
        isWaitingToSendGoalData = true;
        yield return new WaitUntil(() => IsAllPlayersConnected || disconnectCountOpponent > FST_MPConnection.Instance.MaxDisconnectsPerGame || disconnectCount > FST_MPConnection.Instance.MaxDisconnectsPerGame);
        PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { [RP_GOAL] = s, });
    }


    private void GoalStatusInternal(string s)
    {
        m_CountdownTimer.StopTimer();

        if (m_ShotsBeforeGoalScoredCount <= 1)// if we are not up to second shot yet or more, its a foul.
            StartCoroutine(ManagePostFoulGoal(s));
        else StartCoroutine(ManagePostGoal(s));
    }

    /// <summary>
    ///  Update timebars by changing their scale (x) over time.
    ///  If round time ends, the round will change.
    /// </summary>
    private void UpdateTimeBars()
    {
        if (Phase != GamePhase.RoundStarted)
        {
            if (Phase != GamePhase.BallKicked)
                Player1TimeBar.fillAmount = Player2TimeBar.fillAmount = p1TimeBarInitScale;
            return;
        }

        float scale = (FST_Gameplay.IsMultiplayer ? m_CountdownTimer.TimeLeft : FST_MPClock.TimeLeft) / RoundTime;

        if (scale <= 0)
            scale = 0;

        //calculations
        if (IsMastersTurn)
        {
            m_GameDurationMaster += Time.deltaTime;
            p1TimeBarCurrentScale = scale;
        }
        else
        {
            m_GameDurationRemote += Time.deltaTime;
            p2TimeBarCurrentScale = scale;
        }

        Player1TimeBar.fillAmount = p1TimeBarCurrentScale;
        Player2TimeBar.fillAmount = p2TimeBarCurrentScale;

        if (!FST_Gameplay.IsMultiplayer)
            if (p1TimeBarCurrentScale == 0 || p2TimeBarCurrentScale == 0)
                Phase = GamePhase.RoundEnded;
    }


    public IEnumerator ManagePostShot()
    {
      //  Debug.Log("GlobalGameManager -> ManagePostShoot() -> START");
        Phase = GamePhase.BallKicked;

        m_CountdownTimer.StopTimer();

        //add to this now to disable foul after 2 shots have been taken.
        m_ShotsBeforeGoalScoredCount++;
        //  FST_MPDebug.Log("shots of this round = " + m_RoundShotCount);
        //    Debug.Log("shots of this round = " + m_RoundShotCount);

        //let the units begin moving before we check if they are stopped, this call can occur before the next physics update.
        float t = 0;
        while (t < 3)
        {
            t += Time.deltaTime;
            if (Phase != GamePhase.BallKicked)
                yield break;
            yield return 0;
        }
        //let the units get to their positions
        yield return new WaitUntil(() => FST_DiskPlayerManager.Instance.AllMovingObjectsHaveStopped() || Phase != GamePhase.BallKicked);//NOTE: this is heavy here, we can have a one time check when rewrite

        if (Phase == GamePhase.BallKicked)// if we are still in this kicked phase then we may have scored or fouled etc, which in that case the round will have already ended.
            Phase = GamePhase.RoundEnded;


        if (isPenaltyKick)
        {
            //*** reformation of units ONLY for PENALTY mode ***//

            //OUCH!!!!!! PRONE TO FAIL!!!!!!!!!! FIX THIS NEXT LAP ... WIP ->FST
            StartCoroutine(PenaltyControl.updateResultArray(IsMastersTurn ? PLAYER1_FLAG : (IsOfflineAIMatch ? OPPONENT_FLAG : PLAYER2_FLAG), 0));

            //update positions for penalty mode
            SetDestinationForPenaltyMode();

            //Reformation for player_1
            StartCoroutine(GetPlayerAI.GoToPosition(PlayerAI.instance.playerTeam, playerDestination, 1));

            if (IsOfflinePassAndPlayMatch)
            {
                //2-players penalty is not implemented yet
                //...
            }
            else StartCoroutine(GetOpponentAi.GoToPosition(OpponentAI.myTeam, AIDestination, 1));// this is player1 vs AI match:

            FST_BallManager.Instance.PenaltyBallMove(penaltyKickBallPosition);
        }
        //  Debug.Log("12: ManagePostShoot() -> END");
    }


    /// <summary>
    /// If we had a goal in this round, this is the function that manages all aspects of it.
    /// </summary>
    /// <param name="_goalBy">who scored?</param>
    /// <returns></returns>
    private IEnumerator ManagePostGoal(string _goalBy)
    {
        //get who did the goal.
        Debug.Log("ManagePostGoal(): goalby = " + _goalBy);
        //avoid counting a goal as two or more
        if (Phase == GamePhase.GoalIntermission || Phase == GamePhase.Finished)
            yield break;

        if (!IsAllPlayersConnected)
            UIManager.Instance.OppoReconnecting.SetActive(false);

        FST_PrizePoolUpdater.Instance.UpdateText();

        //soft pause the game for reformation and other things...
        Phase = GamePhase.GoalIntermission;

        //add to goal counters
        if (_goalBy == PLAYER1_FLAG)
            MasterGoals++;
        else RemoteGoals++;

        if (FST_Gameplay.IsMultiplayer)//save these values in case players dropout, it will be restored upon reconnect.
            PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { [RP_MASTERGOALS] = MasterGoals, [RP_REMOTEGOALS] = RemoteGoals });

        GameManager.Instance.PlayergoalsText.text = MasterGoals.ToString();
        GameManager.Instance.OpponentgoalsText.text = RemoteGoals.ToString();

        ManageGameStatus();

        //update positions for penalty mode
        if (isPenaltyKick)
        {
            SetDestinationForPenaltyMode();
            StartCoroutine(PenaltyControl.updateResultArray(_goalBy, 1));
        }

        GetAudioSource.PlayOneShot(goalHappenedSfx[Random.Range(0, goalHappenedSfx.Length)], 1);

        //wait some time to show the effects, and let physics cooldown
        yield return new WaitForSecondsRealtime(0.5f);


        //activate the goal event plane
        GameObject anim = null;

        Goal_AnimParent.SetActive(true);

        if ((IsStartMaster && _goalBy == PLAYER1_FLAG) 
            || (!IsStartMaster && _goalBy == PLAYER2_FLAG))//extra check for multiplayer.
        {
            anim = Instantiate(GoalAnimation, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            anim.transform.SetParent(Goal_AnimParent.transform);
            anim.transform.localScale = Vector3.one;
            anim.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;

            float t = 0;
            float speed = 2.0f;
            while (t < 1)
            {
                t += Time.deltaTime * speed;
                anim.transform.position = new Vector3(Mathf.SmoothStep(30, 0, t), 0, -3);
                yield return 0;
            }
            Goal_animation.Instance.Dance = true;

            yield return new WaitForSecondsRealtime(3f);

            Goal_animation.Instance.Dance = false;
            yield return new WaitForSecondsRealtime(0.5f);


            float t2 = 0;
            while (t2 < 1)
            {
                t2 += Time.deltaTime * speed;
                anim.transform.position = new Vector3(Mathf.SmoothStep(0, -30, t2), 0, -3);
                yield return 0;
            }

            yield return new WaitForSecondsRealtime(0.5f);
            Destroy(anim, 1f);
            Goal_AnimParent.SetActive(false);
            // Debug.Log("animation end");
        }
        else
        {
            anim = Instantiate(SadAnimation, new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0)) as GameObject;
            anim.transform.SetParent(Goal_AnimParent.GetComponent<Transform>());//more suitable than parent = transform ->FST
            anim.transform.localScale = Vector3.one;
            anim.GetComponent<RectTransform>().anchoredPosition3D = Vector3.zero;
            yield return new WaitForSecondsRealtime(5f);//use realtime to help stay in sync ->FST
            Destroy(anim, 1f);
            Goal_AnimParent.SetActive(false);
            // Debug.Log("animation end");
        }

        GetBallManager.StopBall();

        // hook this into new penalty method in ballmanager ->FST
        if (!isPenaltyKick)
            SetBallToCenter();     //go to the center of field
        else GetBallManager.PenaltyBallMove(penaltyKickBallPosition);       //GO TO PENALTY POSITION!

        if (FST_Gameplay.IsMultiplayer)
        {
            if (Phase != GamePhase.Finished && !isTimeBasedTimerFinished)
            {
                if (isWaitingToSendGoalData)
                {
                    isWaitingToSendGoalData = false;

                    if (disconnectCountOpponent > FST_MPConnection.Instance.MaxDisconnectsPerGame)
                    {
                        PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P2_DisconnectCountTooHigh : PlayerLeftReason.P1_DisconnectCountTooHigh);
                        yield return null;
                    }
                    else if (disconnectCount > FST_MPConnection.Instance.MaxDisconnectsPerGame)
                    {
                        PlayerLeftWinPopup(!IsStartMaster ? PlayerLeftReason.P2_DisconnectCountTooHigh : PlayerLeftReason.P1_DisconnectCountTooHigh);
                        yield return null;
                    }
                }

                if (!ChooseFormationButton.activeInHierarchy)
                {
                    Phase = GamePhase.FormationSelect;

                    yield return new WaitUntil(() => Phase != GamePhase.FormationSelect);

                    Phase = GamePhase.FormationClosed;
                }

                UIManager.Instance.LoadingInGame.SetActive(true);

                if (FST_DiskPlayerManager.Instance.IsOwner)
                {
                    GetPlayerAI.SetFormation();
                    TransmitFormationReset();
                }

                yield return new WaitForSecondsRealtime(2f);
            }
            else
            {
                GameOver();
            }
        }
        else
        {
            yield return new WaitForSecondsRealtime(0.5f);


            if (Phase == GamePhase.Finished)
                GameOver();

            GetPlayerAI.SetFormation();
            if (IsOfflineAIMatch)
                GetOpponentAi.SetFormation();
        }

        if (FST_BallManager.Instance && FST_BallManager.Instance.TrailRenderBall)
            FST_BallManager.Instance.TrailRenderBall.enabled = true;

        yield return new WaitForSecondsRealtime(1f);

        //   if (FST_Gameplay.IsMaster)
        // {  //NOTE: We adjust the turn after the routine, allowing the units to settle

        //if goal by player-1 > opponent should start the next round, so we temporarily override the current turn in case opponent scored for player by accident
        //else  >  goal by opponent and player-1 should start the next round, so we temporarily override the current turn in case player scored for opponent by accident
        //we do this so when set turn is called later, it will ensure we give the losing rounds player the turn.
      //  Debug.Log("MASTER TURN BEFORE GOAL = " + IsMastersTurn);
        IsMastersTurn = _goalBy == PLAYER1_FLAG;
      //  Debug.Log("MASTER TURN AFTER GOAL = " + IsMastersTurn);

        UIManager.Instance.LoadingInGame.SetActive(false);

        if (Phase == GamePhase.FormationClosed || Phase == GamePhase.GoalIntermission)// if we get to here check to ensure we havent changed to the game finished state, or any other state that may be added later. Use the state we set for soft pause to check
            Phase = GamePhase.RoundEnded;

        m_ShotsBeforeGoalScoredCount = 0;//reset this now to reenable fouls
        PlaySfx(startWistle, true);
    }


    public IEnumerator ManagePostFoulGoal(string _goalBy)
    {
        Debug.Log("ManagePostFoulGoal(): foul By = " + _goalBy);

        //avoid counting a goal as two or more
        if (Phase == GamePhase.GoalIntermission)
            yield break;

        //soft pause the game for reformation and other things...
        Phase = GamePhase.GoalIntermission;

        //update positions for penalty mode
        if (isPenaltyKick)
        {
            SetDestinationForPenaltyMode();
            StartCoroutine(PenaltyControl.updateResultArray(_goalBy, 1));
        }

        GetAudioSource.PlayOneShot(goalHappenedSfx[Random.Range(0, goalHappenedSfx.Length)], 1);
        //wait a few seconds to show the effects , and physics cooldown
        //yield return new WaitForSeconds(1);

        //activate the goal event plane
        GameObject gp = Instantiate(foulGoalPlane, new Vector3(30, 0, -3), Quaternion.Euler(0, 0, 0)) as GameObject;
        gp.transform.SetParent(ParentGoalPlaneObject);
        float t = 0;
        float speed = 2.0f;
        while (t < 1)
        {
            t += Time.deltaTime * speed;
            gp.transform.position = new Vector3(Mathf.SmoothStep(30, 0, t), 0, -3);
            yield return 0;
        }
        yield return new WaitForSecondsRealtime(0.75f);
        float t2 = 0;
        while (t2 < 1)
        {
            t2 += Time.deltaTime * speed;
            gp.transform.position = new Vector3(Mathf.SmoothStep(0, -30, t2), 0, -3);
            yield return 0;
        }

        //Shit! no dont do this, we should just disable and recycle!!!! ->FST
        Destroy(gp, 1.5f);

        //stop the ball
        GetBallManager.StopBall();

        if (!isPenaltyKick)
            SetBallToCenter();     //set ball to middle of the field
        else GetBallManager.transform.position = penaltyKickBallPosition;  //GO TO PENALTY POSITION!

        if (FST_Gameplay.IsMultiplayer)
        {
            UIManager.Instance.LoadingInGame.SetActive(true);
            yield return new WaitForSecondsRealtime(3f);
        }
        else yield return new WaitForSecondsRealtime(1.5f);

        if (FST_BallManager.Instance && FST_BallManager.Instance.TrailRenderBall)
            FST_BallManager.Instance.TrailRenderBall.enabled = true;

        GetPlayerAI.SetFormation();
        if (IsOfflineAIMatch)
            GetOpponentAi.SetFormation();

        yield return new WaitForSecondsRealtime(1f);//use realtime to help stay in sync ->FST

        //MOVE TO METHOD IN BALL MANAGER ->FST 
        GetBallManager.GetRigidBody.isKinematic = false;

        //continue to the next round
        CurrentPlayer = null;
        CurrentOpponent = null;
        Current2Player = null;

        UIManager.Instance.LoadingInGame.SetActive(false);

        if (Phase == GamePhase.GoalIntermission)
            Phase = GamePhase.RoundEnded;

        PlaySfx(startWistle, true);
    }

    private void ManageGameStatus()
    {
        if (Phase == GamePhase.Finished)
            return;

        if (timeBased)
        {
            if (!isTimeBasedTimerFinished)
            {
                seconds = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) % 60;
                minutes = Mathf.CeilToInt(gameTimer - Time.timeSinceLevelLoad) / 60;
            }
            infoText.text = string.Format("{0 :0 0} : {01:0 0 }", minutes, seconds);
            // remainingTime.text = minutes.ToString();

            if (seconds == 0 && minutes == 0)
            {
                isTimeBasedTimerFinished = true;

                if (MasterGoals == RemoteGoals)
                    return;

                if ((MasterGoals > RemoteGoals) || (MasterGoals < RemoteGoals))
                    Phase = GamePhase.Finished; // GameOver("ManageGameStatus() > time based, time is up and the winner = " + (MasterGoals > RemoteGoals ? "Master" : "Remote"));
            }

        }
        else if ((MasterGoals >= GoalLimit) || (RemoteGoals >= GoalLimit))
            Phase = GamePhase.Finished; // GameOver("Not time based and the winner is " + (MasterGoals >= GoalLimit ? "Master" : "Remote"));

        //a little tweak
        //We do not need time in penalty mode, so
        if (isPenaltyKick)
        {
            seconds = 0;
            minutes = 90;
        }
        //GameManager.SharedInstance.gametimerText.text = remainingTime.ToString();

        if (GameManager.Instance)
        {
            if (MatchType == 0)
            {
                GameManager.Instance.PlayernameText.text = Player1Name;
                GameManager.Instance.OpponentNameText.text = CPU_NAME;
            }
            else if (IsOfflinePassAndPlayMatch)
            {
                GameManager.Instance.PlayernameText.text = Player1Name;
                GameManager.Instance.OpponentNameText.text = Player2Name;
            }
        }
    }



    private bool isFormationOpen = false;
    private void OpenInGameFormation()
    {
        if (isFormationOpen)
            return;

        if (FST_Gameplay.IsMultiplayer)
        {
            m_CountdownTimer.Countdown = 10;

            if (FST_Gameplay.IsMaster || !IsAllPlayersConnected)
                m_CountdownTimer.StartFormationTimer();
        }

        FST_FormationsManager.Instance.OpenOrCloseFormationPanel(true);

        isFormationOpen = true;
    }

    public void CloseInGameFormation()
    {
        if (!isFormationOpen)
            return;

        FST_FormationsManager.Instance.OpenOrCloseFormationPanel(false);

        isFormationOpen = false;
    }

    public void SetBallToCenter() => FST_DiskPlayerManager.Instance.ResetBall(ballStartingPosition);

    #endregion


    #region GAME OVER FUNCTIONS

    public enum PlayerLeftReason
    {
        P1_Quit,
        P1_DisconnectTimeOut,
        P1_DisconnectCountTooHigh,
        P2_Quit,
        P2_DisconnectTimeOut,
        P2_DisconnectCountTooHigh,
    }

    public void QuitMatch()
    {
        if (FST_Gameplay.IsMultiplayer)
        {
            if (Phase == GamePhase.Finished || didGameOver)//go home from win popup
            {
                //Debug.Log("QuitMatch! leaving room!");
                FST_MPConnection.Instance.LeaveRoom(true, false);
                //SceneManager.LoadScene("MainMenu");
            }
            else
            {
                //Debug.Log("QuitMatch!");
                PhotonNetwork.SetPlayerCustomProperties(new ExitGames.Client.Photon.Hashtable { { FST_PlayerProps.REMATCH, 2 } });

                GameManager.Instance.IsRematchOneNOne = false;

                PlayerLeftWinPopup(IsStartMaster ? PlayerLeftReason.P1_Quit : PlayerLeftReason.P2_Quit);
            }
        }
        else SceneManager.LoadScene("MainMenu");
    }

    public void PlayerLeftWinPopup(PlayerLeftReason reason, bool transmit = true)
    {
        if (didGameOver)
        {
            Debug.Log("PlayerLeftWinPopup: FAIL, game is over!");
            return;
        }

        if (FST_Gameplay.IsMultiplayer && isWaitingToSendGoalData)
        {
            //we may have scored when player told us they left!
            Debug.Log("PlayerLeftWinPopup: isWaitingToSendGoalData = true!");
            isWaitingToSendGoalData = false;
            //return;
        }

        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable { { FST_PlayerProps.REMATCH, 2 } });

        didGameOver = true;

        IsMasterLeft = reason == PlayerLeftReason.P1_DisconnectCountTooHigh || reason == PlayerLeftReason.P1_DisconnectTimeOut || reason == PlayerLeftReason.P1_Quit;

        if (PhotonNetwork.InRoom)
        {
            //we are likely quitting, we will tell the opponent.
            if (transmit)
                TransmitPlayerLeftWinPopUp(reason, IsStartMaster ? netPlayers[1] : netPlayers[0]);

            //IMPORTANT NOTE: If the master leaves first, then the pun call back OnMasterClientSwitched() will fire, this is not good at this very moment as the call back stalls the leave room operation! So first we ensure the remote has left. The master will leave after.
            if (FST_Gameplay.IsMaster)
            {
                PhotonNetwork.CurrentRoom.EmptyRoomTtl = 0;
            }
            else FST_MPConnection.Instance.LeaveRoom(false, false);
        }

        Phase = GamePhase.Finished;

        reconnectOppTimer.Cancel();
        reconnectTimer.Cancel();



        Debug.Log("PlayerLeftWinPopup, Reason: " + reason + ", IsMaster: " + FST_Gameplay.IsMaster + ", isStartMaster = " + IsStartMaster + ", IsMasterLeft = " + IsMasterLeft);

        if (UIManager.Instance)
        {
            if (UIManager.Instance.LoadingInGame.activeSelf)
                UIManager.Instance.LoadingInGame.SetActive(false);
        }

        GameplayTimer_End = true;

        winBackButton.SetActive(true);
        winLeagueBackButton.SetActive(false);

        // Following conditions are handled on IsMasterLeft bool            
        winScreenPlayerName_1.text = winScreenPlayerName.text = Player1Name;
        winScreenOpponentName_1.text = winScreenOpponentName.text = Player2Name;

        UIManager.Instance.Rematch1.SetActive(false); // Hide Rematch button from winscreen 1 when player left
        UIManager.Instance.Rematch2.SetActive(false); // Hide Rematch button from winscreen 2 when player left

        UIManager.Instance.winscreen_2.SetActive(IsMasterLeft);
        UIManager.Instance.gameStatusPlane.SetActive(!IsMasterLeft);

        if (IsMasterLeft)
        {
            UIManager.Instance.PlayerGoalTextWinscreen_2.text = MasterGoals.ToString();//RemoteGoals.ToString();
            UIManager.Instance.OpponentGoalTextWinscreen_2.text = RemoteGoals.ToString();//MasterGoals.ToString();
            if (IsStartMaster)
            {
                winText2.SetActive(false);
                loseText2.SetActive(true);
            }
            else
            {
                winText2.SetActive(true);
                loseText2.SetActive(false);
            }
        }
        else
        {
            //Player 2 left           
            UIManager.Instance.PlayerGoalTextWinscreen_1.text = MasterGoals.ToString();
            UIManager.Instance.OpponentGoalTextWinscreen_1.text = RemoteGoals.ToString();
            if (IsStartMaster)
            {
                winText1.SetActive(true);
                loseText1.SetActive(false);
            }
            else
            {
                winText1.SetActive(false);
                loseText1.SetActive(true);
            }
        }

        if (FST_Gameplay.IsMultiplayer
             &&  FST_Gameplay.IsMaster
          //  && ((!IsMasterLeft && IsStartMaster) || (IsMasterLeft && !IsStartMaster))
            )
        {
            FST_MPDebug.Log("Send match data");
            Debug.Log("Send match data");

            //we are the only one left in the game! send the data to server!
            float matchTime = Time.time - m_GameStartTime;

            Joga_NetworkManager.Instance.UpdateMatchResultRequest(FST_SettingsManager.PlayerName, Player1Name, Player2Name, MasterGoals, RemoteGoals, matchTime, m_GameDurationMaster, m_GameDurationRemote);

            //FST_MPConnection.Instance.LeaveRoom(false, false);
        }

        string displayString = (reason == PlayerLeftReason.P1_Quit || reason == PlayerLeftReason.P2_Quit) ? "I Am Busy" : "Disconnected";

        for (int i = 0; i < UIManager.Instance.leftPlayerText.Length; i++)
        {
            UIManager.Instance.leftPlayerText[i].GetComponent<Text>().horizontalOverflow = HorizontalWrapMode.Overflow;
            // if (!onPurpose)//update the text to show the user that player was disconnected by internet foul. (its default value is "Player Left The Match");
            UIManager.Instance.leftPlayerText[i].GetComponent<Text>().text = displayString;

            //show the resulting text
            UIManager.Instance.leftPlayerText[i].SetActive(true);
        }
    }

    bool didGameOver = false;
    /// <summary>
    /// After the game is finished, this function handles the events.
    /// </summary>
    public void GameOver()
    {
        if (didGameOver)
            return;

        didGameOver = true;

        Debug.Log("Game finished");

        Phase = GamePhase.Finished;

        reconnectOppTimer.Cancel();
        reconnectTimer.Cancel();

        float matchTime = Time.time - m_GameStartTime;
        // Debug.Log("ManageGameFinishState 1");
        //Play gameFinish whistle
        PlaySfx(finishWistle, true);

        GameplayTimer_End = true;

        UIManager.Instance.GameStartgyPanel.SetActive(false);

        //safety check for display text, just in case.
        if (MasterGoals > GoalLimit)
            MasterGoals = GoalLimit;
        if (RemoteGoals > GoalLimit)
            RemoteGoals = GoalLimit;

        if (MatchType == 4)
        {
            winLeagueBackButton.SetActive(true);
            winBackButton.SetActive(false);
        }
        else if (MatchType == 3)
        {
            winLeagueBackButton.SetActive(false);
            winBackButton.SetActive(true);
        }
        else
        {
            winBackButton.SetActive(true);
            winLeagueBackButton.SetActive(false);
        }

        if (!timeBased)
        {
            if (FST_Gameplay.IsMultiplayer)
            {
                if (UIManager.Instance.LoadingInGame.activeSelf)
                    UIManager.Instance.LoadingInGame.SetActive(false);

                if (FST_SettingsManager.MatchType == 3)//leagueplay
                {
                    UIManager.Instance.Rematch1.SetActive(false);
                    UIManager.Instance.Rematch2.SetActive(false);
                }

                //Master Win Status
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    // Debug.Log("ManageGameFinishState MasterGoals >= goalLimit || MasterGoals > RemoteGoals");

                    winnerName = Player1Name;

                    winScreenPlayerName_1.text = Player1Name;
                    winScreenOpponentName_1.text = Player2Name;
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = Player2Name;

                    UIManager.Instance.winscreen_2.SetActive(false);
                    UIManager.Instance.gameStatusPlane.SetActive(true);
                    UIManager.Instance.Winner_icon1.SetActive(true);
                    UIManager.Instance.PlayerGoalTextWinscreen_1.text = UIManager.Instance.PlayerGoalTextWinscreen_2.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoalTextWinscreen_1.text = UIManager.Instance.OpponentGoalTextWinscreen_2.text = RemoteGoals.ToString();

                    // playerGameStats.text = "Winner";
                    // opponentsGameStats.text = "Lose";

                    if (IsStartMaster)
                    {
                        winText1.SetActive(true);
                        loseText1.SetActive(false);

                        //winText2.SetActive(true);
                        //loseText2.SetActive(false);

                        // Debug.Log("ManageGameFinishState isStartMaster = true");
                        FST_SettingsManager.OnlineWins++;

                        if (MatchType == 3)
                        {
                            if (!GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound1Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound2Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && GameManager.Instance.IsOnlineTournamentRound2Win && !GameManager.Instance.IsOnlineTournamentRound3Win)
                                GameManager.Instance.IsOnlineTournamentRound3Win = true;

                        }
                    }
                    else
                    {
                        winText1.SetActive(false);
                        loseText1.SetActive(true);
                        // Debug.Log("ManageGameFinishState IsMaster = false");
                        if (MatchType == 3)
                            GameManager.Instance.IsOnlineTournamentLose = true;
                    }
                }

                //Remote Win Status
                else if (MasterGoals < RemoteGoals || RemoteGoals >= GoalLimit)
                {
                    winnerName = Player2Name;

                    // Debug.Log("ManageGameFinishState MasterGoals < RemoteGoals || RemoteGoals >= goalLimit");
                    winScreenPlayerName_1.text = Player1Name;
                    winScreenOpponentName_1.text = Player2Name;
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = Player2Name;

                    UIManager.Instance.winscreen_2.SetActive(true);
                    UIManager.Instance.gameStatusPlane.SetActive(false);
                    UIManager.Instance.Winner_Icon2.SetActive(true);

                    UIManager.Instance.PlayerGoalTextWinscreen_1.text = UIManager.Instance.PlayerGoalTextWinscreen_2.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoalTextWinscreen_1.text = UIManager.Instance.OpponentGoalTextWinscreen_2.text = RemoteGoals.ToString();

                    if (IsStartMaster)
                    {
                        //winText1.SetActive(false);
                        //loseText1.SetActive(true);

                        winText2.SetActive(false);
                        loseText2.SetActive(true);

                        if (MatchType == 3)
                            GameManager.Instance.IsOnlineTournamentLose = true;
                    }

                    else
                    {
                        winText2.SetActive(true);
                        loseText2.SetActive(false);

                        FST_SettingsManager.OnlineWins++;

                        if (MatchType == 3)
                        {
                            if (!GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound1Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound2Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && GameManager.Instance.IsOnlineTournamentRound2Win && !GameManager.Instance.IsOnlineTournamentRound3Win)
                                GameManager.Instance.IsOnlineTournamentRound3Win = true;
                        }
                    }
                }

                if (FST_Gameplay.IsMaster)
                {
                    Debug.Log("matchTime = " + matchTime);
                    Joga_NetworkManager.Instance.UpdateMatchResultRequest(winnerName, Player1Name, Player2Name, MasterGoals, RemoteGoals, matchTime, m_GameDurationMaster, m_GameDurationRemote);
                }

                return;
            }



            //for single player game, we should give the player some bonuses in case of winning the match
            if (MatchType == 0)
            {
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    //set the result texture
                    UIManager.Instance.Offline_WinScreen.SetActive(true);
                    UIManager.Instance.PlayerGoal.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoal.text = RemoteGoals.ToString();

                    UIManager.Instance.Offline_WinText.text = Player1Name + " " + "  wins this game!";

                    FST_SettingsManager.OfflineWins++;       //add to wins counter
                    FST_SettingsManager.OfflineMoney += 100; //handful of coins as the prize!

                    //if this is a tournament match, update it with win state and advance.
                    //if (PlayerPrefs.GetInt("IsTournament") == 1)
                    //{
                    //    PlayerPrefs.SetInt("TorunamentMatchResult", 1);
                    //    PlayerPrefs.SetInt("TorunamentLevel", PlayerPrefs.GetInt("TorunamentLevel", 0) + 1);
                    //}

                }

                else if (RemoteGoals >= GoalLimit || RemoteGoals > MasterGoals)
                {

                    UIManager.Instance.Offline_WinScreen.SetActive(true);
                    UIManager.Instance.PlayerGoal.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoal.text = RemoteGoals.ToString();

                    UIManager.Instance.Offline_WinText.text = Player2Name + " " + "  wins this game!";

                    //if this is a tournament match, update it with lose state.
                    //if (PlayerPrefs.GetInt("IsTournament") == 1)
                    //{
                    //    PlayerPrefs.SetInt("TorunamentMatchResult", 0);
                    //    PlayerPrefs.SetInt("TorunamentLevel", PlayerPrefs.GetInt("TorunamentLevel", 0) + 1);
                    //}
                }
            }

            else if (IsOfflinePassAndPlayMatch)
            {

                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    UIManager.Instance.Offline_WinScreen.SetActive(true);
                    UIManager.Instance.PlayerGoal.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoal.text = RemoteGoals.ToString();

                    UIManager.Instance.Offline_WinText.text = Player1Name + "  wins this game!";
                }
                else if (MasterGoals < RemoteGoals || RemoteGoals >= GoalLimit)
                {
                    UIManager.Instance.Offline_WinScreen.SetActive(true);
                    UIManager.Instance.PlayerGoal.text = MasterGoals.ToString();
                    UIManager.Instance.OpponentGoal.text = RemoteGoals.ToString();

                    UIManager.Instance.Offline_WinText.text = Player2Name + "  wins this game!";
                }
            }
            else if (MatchType == 4)
            {
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    winScreenPlayerName.text = "" + Player1Name;
                    winScreenOpponentName.text = "" + CPU_NAME;
                    playerGameStats.text = "Winner";
                    opponentsGameStats.text = "Lose";

                    //NOTE: these bools should be one simple int..
                    if (!GameManager.Instance.isPlayerRound1Win && !GameManager.Instance.isPlayerRound2Win)
                        GameManager.Instance.isPlayerRound1Win = true;
                    else if (GameManager.Instance.isPlayerRound1Win && !GameManager.Instance.isPlayerRound2Win)
                        GameManager.Instance.isPlayerRound2Win = true;
                    else if (GameManager.Instance.isPlayerRound1Win && GameManager.Instance.isPlayerRound2Win && !GameManager.Instance.isPlayerRound3Win)
                        GameManager.Instance.isPlayerRound3Win = true;
                }
                else if (MasterGoals < RemoteGoals || RemoteGoals >= GoalLimit)
                {
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = CPU_NAME;
                    playerGameStats.text = "Lose";
                    opponentsGameStats.text = "Winner";
                    GameManager.Instance.isPlayerLoseTournament = true;
                }
            }

        }
        //NOTE: WE DONT USE THIS AT ALL, LEFT FOR LATER REFERENCE
        else if (timeBased)
        {
            //for single player game, we should give the player some bonuses in case of winning the match
            if (MatchType == 0)
            {
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    //set the result texture
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = CPU_NAME;

                    FST_SettingsManager.OfflineWins++;       //add to wins counter
                    FST_SettingsManager.OfflineMoney += 100; //handful of coins as the prize!

                    //if this is a tournament match, update it with win state and advance.
                    if (PlayerPrefs.GetInt("IsTournament") == 1)
                    {
                        PlayerPrefs.SetInt("TorunamentMatchResult", 1);
                        PlayerPrefs.SetInt("TorunamentLevel", PlayerPrefs.GetInt("TorunamentLevel", 0) + 1);
                    }

                }
                else if (RemoteGoals >= GoalLimit || RemoteGoals > MasterGoals)
                {
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = CPU_NAME;

                    //if this is a tournament match, update it with lose state.
                    if (PlayerPrefs.GetInt("IsTournament") == 1)
                    {
                        PlayerPrefs.SetInt("TorunamentMatchResult", 0);
                        PlayerPrefs.SetInt("TorunamentLevel", PlayerPrefs.GetInt("TorunamentLevel", 0) + 1);
                    }
                }
            }
            else if (IsOfflinePassAndPlayMatch)
            {
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    UIManager.Instance.gameStatusPlane.SetActive(true);
                    winText1.SetActive(true);
                    winScreenPlayerName.text = "" + Player1Name;
                    winScreenOpponentName.text = "" + Player2Name;
                }
                else if (MasterGoals < RemoteGoals || RemoteGoals >= GoalLimit)
                {
                    UIManager.Instance.winscreen_2.SetActive(true);
                    loseText2.SetActive(true);
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = Player2Name;
                }
            }
            else if (FST_Gameplay.IsMultiplayer)
            {
                if (UIManager.Instance.LoadingInGame.activeSelf)
                    UIManager.Instance.LoadingInGame.SetActive(false);

                //Master Win Status
                if (MasterGoals > RemoteGoals)
                {
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = Player2Name;

                    winScreenPlayerName_1.text = Player1Name;
                    winScreenOpponentName_1.text = Player2Name;

                    if (IsStartMaster)//FST_Gameplay.IsMaster//netPlayers[0] == PhotonNetwork.LocalPlayer
                    {
                        winnerName = Player1Name;

                        UIManager.Instance.winscreen_2.SetActive(false);
                        UIManager.Instance.gameStatusPlane.SetActive(true);

                        winText1.SetActive(true);
                        loseText1.SetActive(false);

                        //	playerGameStats.text = "Congratulations !!!";
                        playerGameStats.enabled = true;   //this is Player win text "Congratulations"
                        playerGameStats.color = Color.yellow;

                        UIManager.Instance.PlayerGoalTextWinscreen_1.text = MasterGoals.ToString();
                        UIManager.Instance.OpponentGoalTextWinscreen_1.text = RemoteGoals.ToString();
                        if (MatchType == 3)
                        {
                           // playerGameStats.text = "Winner";
                           // opponentsGameStats.text = "Lose";
                            if (!GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound1Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound2Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && GameManager.Instance.IsOnlineTournamentRound2Win && !GameManager.Instance.IsOnlineTournamentRound3Win)
                                GameManager.Instance.IsOnlineTournamentRound3Win = true;
                        }

                        Joga_NetworkManager.Instance.UpdateMatchResultRequest(winnerName, Player1Name, Player2Name, MasterGoals, RemoteGoals, matchTime, m_GameDurationMaster, m_GameDurationRemote);//send from master
                    }
                    else
                    {
                        winnerName = Player2Name;
                        // Debug.Log("First player ID"	+MainsocketInstance.oneNOneFirstPlayerID);
                        // Debug.Log("user ID"	+ MainsocketInstance.userId);

                        UIManager.Instance.winscreen_2.SetActive(true);
                        UIManager.Instance.gameStatusPlane.SetActive(false);
                        // opponentsGameStats.enabled = true;
                        // opponentsGameStats.text = "Better luck next time...." + Color.red;
                        winText2.SetActive(true);
                        loseText2.SetActive(false);

                        UIManager.Instance.PlayerGoalTextWinscreen_2.text = RemoteGoals.ToString();
                        UIManager.Instance.OpponentGoalTextWinscreen_2.text = MasterGoals.ToString();
                        if (MatchType == 3)
                            GameManager.Instance.IsOnlineTournamentLose = true;
                    }
                }
                //Remote Win Status
                else if (MasterGoals < RemoteGoals)
                {
                    winScreenPlayerName_1.text = Player1Name;
                    winScreenOpponentName_1.text = Player2Name;
                    winScreenPlayerName.text = Player1Name;
                    winScreenOpponentName.text = Player2Name;

                    if (IsStartMaster)//FST_Gameplay.IsMaster//netPlayers[0] == PhotonNetwork.LocalPlayer
                    {
                        winnerName = Player2Name;

                        UIManager.Instance.winscreen_2.SetActive(true);
                        UIManager.Instance.gameStatusPlane.SetActive(false);
                        playerGameStats.enabled = true;
                        winText2.SetActive(false);
                        loseText2.SetActive(true);

                        UIManager.Instance.PlayerGoalTextWinscreen_2.text = MasterGoals.ToString();
                        UIManager.Instance.OpponentGoalTextWinscreen_2.text = RemoteGoals.ToString();
                        if (MatchType == 3)
                            GameManager.Instance.IsOnlineTournamentLose = true;

                        Joga_NetworkManager.Instance.UpdateMatchResultRequest(winnerName, Player1Name, Player2Name, MasterGoals, RemoteGoals, matchTime, m_GameDurationMaster, m_GameDurationRemote);//send from master
                    }
                    else
                    {
                        winnerName = Player1Name;

                        UIManager.Instance.winscreen_2.SetActive(false);
                        UIManager.Instance.gameStatusPlane.SetActive(true);
                        // opponentsGameStats.enabled = true;                  // player lose text "Better luck..."
                        winText1.SetActive(false);

                        loseText1.SetActive(true);

                        UIManager.Instance.PlayerGoalTextWinscreen_1.text = MasterGoals.ToString();
                        UIManager.Instance.OpponentGoalTextWinscreen_1.text = RemoteGoals.ToString();

                        if (MatchType == 3)
                        {
                            // playerGameStats.text = "Winner";
                            // opponentsGameStats.text = "Lose";
                            if (!GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound1Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && !GameManager.Instance.IsOnlineTournamentRound2Win)
                                GameManager.Instance.IsOnlineTournamentRound2Win = true;
                            else if (GameManager.Instance.IsOnlineTournamentRound1Win && GameManager.Instance.IsOnlineTournamentRound2Win && !GameManager.Instance.IsOnlineTournamentRound3Win)
                                GameManager.Instance.IsOnlineTournamentRound3Win = true;
                        }
                    }

                   // GameManager.Instance.OnlineTournamentWinScreenBackButton();
                }
            }
            else if (MatchType == 4)
            {
                if (MasterGoals >= GoalLimit || MasterGoals > RemoteGoals)
                {
                    winScreenPlayerName.text = "" + Player1Name;
                    winScreenOpponentName.text = "" + CPU_NAME;
                    playerGameStats.text = "Winner";
                    opponentsGameStats.text = "Lose";
                    if (!GameManager.Instance.isPlayerRound1Win && !GameManager.Instance.isPlayerRound2Win)
                        GameManager.Instance.isPlayerRound1Win = true;
                    else if (GameManager.Instance.isPlayerRound1Win && !GameManager.Instance.isPlayerRound2Win)
                        GameManager.Instance.isPlayerRound2Win = true;
                    else if (GameManager.Instance.isPlayerRound1Win && GameManager.Instance.isPlayerRound2Win && !GameManager.Instance.isPlayerRound3Win)
                        GameManager.Instance.isPlayerRound3Win = true;
                }
                else if (MasterGoals < RemoteGoals || RemoteGoals >= GoalLimit)
                {
                    winScreenPlayerName.text = "" + Player1Name;
                    winScreenOpponentName.text = "" + CPU_NAME;
                    playerGameStats.text = "Lose";
                    opponentsGameStats.text = "Winner";
                    GameManager.Instance.isPlayerLoseTournament = true;
                }
            }
        }
    }

    #endregion


    #region AUDIO FUNCTIONS

    private IEnumerator PlayCrowdChants()
    {
        if (!CanPlayCrowdChants)
            yield break;

        CanPlayCrowdChants = false;
        FST_AudioManager.Instance.PlayAudio(FST_AudioManager.AudioID.AMBIENCE_Crowd);
        yield return new WaitForSeconds(Random.Range(30, 50));
        CanPlayCrowdChants = true;
    }

    /// <summary>
    /// Plays the Audio Source in an orderly fashion
    /// </summary>
    /// <param name="_clip">the AudioClip to play</param>
    private void PlaySfx(AudioClip _clip, bool oneShot)//caps ->FST
    {
        if (oneShot)
        {
            GetAudioSource.PlayOneShot(_clip);
            return;
        }
        GetAudioSource.Stop();
        GetAudioSource.clip = _clip;
        GetAudioSource.Play();
    }

    #endregion
}