/////////////////////////////////////////////////////////////////////////////////
//
//  FST_MPConnection.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:    initiates and manages the connection to Photon Cloud, regulates
//					room creation, max player count per room and logon timeout.
//					also keeps the 'IsMultiplayer' and 'IsMaster' flags up-to-date.				
//
/////////////////////////////////////////////////////////////////////////////////


// for Anti-Cheat Toolkit support
using CodeStage.AntiCheat.ObscuredTypes;

using UnityEngine;
using UnityEngine.SceneManagement;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using FastSkillTeam;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using Jiweman;
using System.Collections;

public class FST_MPConnection : MonoBehaviourPunCallbacks
{
    public ObscuredFloat LogOnTimeOut = 20f;// if a stage in the initial connection process stalls for more than this many seconds, the connection will be restarted
    [Tooltip("after this many connection attempts, the script will abort and return to main menu, 0 = unlimited")]
    public ObscuredInt MaxConnectionAttempts = 10;// after this many connection attempts, the script will abort and return to main menu, 0 = unlimited
    protected ObscuredInt m_ConnectionAttempts = 0;//tracking
    [Tooltip("Player can restart search for opponent after this amount of time (in seconds)")]
    public ObscuredFloat MaximumRoomSearchTime = 45f;
    public ObscuredInt SendRate = 20;//default 20f
    public ObscuredInt SerializationRate = 10;//default 10f
    public ObscuredByte MaximumPlayers = 2;
    public string SceneToLoadOnDisconnect = "MainMenu";     // this scene will be loaded when the 'Disconnect' method is executed
    public static bool StayConnected = false;       // as long as this is true, this component will relentlessly try to reconnect to the photon cloud
    [Tooltip("If in a background state, connection will be cancelled after this amount of time (in seconds)")]
    public ObscuredFloat BackgroundTimeout = 60f;//default is 60f
    [Tooltip("After a player disconnects, they can retry joining the same match for this amount of time (in ms)")]
    public ObscuredInt ActorTimeToLiveOnDc = 60000;// 1 minute in ms
    public ObscuredInt MaxDisconnectsPerGame = 3;
    // ping
    public float PingReportInterval = 10.0f;
    protected int m_LastPing = 0;
    protected float m_NextAllowedPingTime = 0.0f;

    //working vars
    public static string MapToLoad { get; set; } = "Nairobi";// for room creation, keeps maps seperate and saves people loading unwanted maps // set via UI_MP_Menu
    protected ClientState m_LastClientState = ClientState.Disconnected;
    protected FST_Timer.Handle m_ConnectionTimer = new FST_Timer.Handle();

    // instance
    public new bool DontDestroyOnLoad = true;
    public static FST_MPConnection Instance = null;


    private void Awake()
    {
        if (!Instance)
            Instance = this;
        else Destroy(gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start()
    {

        if (StayConnected)
            Connect();

        if (DontDestroyOnLoad)
            Object.DontDestroyOnLoad(transform.root.gameObject);

    }
    bool simOn = false;//uncomment to test disconnect
    float dcTime = 0;//uncomment to test disconnect
    /// <summary>
    /// 
    /// </summary>
    protected virtual void Update()
    {
#if UNITY_EDITOR
        PhotonNetwork.SendRate = SendRate;
        PhotonNetwork.SerializationRate = SerializationRate;
        PhotonNetwork.KeepAliveInBackground = BackgroundTimeout;
#endif


        UpdateConnectionState();

        UpdatePing();

#if UNITY_EDITOR
        // SNIPPET: uncomment to test disconnect
        if (GlobalGameManager.Instance)
        {
            if (Input.GetKeyUp(KeyCode.K))
            {
                simOn = true;
                dcTime = 0;
                PhotonNetwork.NetworkingClient.SimulateConnectionLoss(true);
                Debug.Log("**************************** SIMULATE CONNECTION LOSS START ***********************************");
            }
            if (Input.GetKeyUp(KeyCode.I))
            {
                simOn = false;
                PhotonNetwork.NetworkingClient.SimulateConnectionLoss(false);
                Debug.Log("**************************** SIMULATE CONNECTION LOSS STOP ***********************************");
                dcTime = 0;

                if (GlobalGameManager.Instance && !PhotonNetwork.InRoom)
                    PhotonNetwork.ReconnectAndRejoin();
            }

            if (Input.GetKeyUp(KeyCode.M))
            {
                if (GlobalGameManager.Instance.IsStartMaster && FST_Gameplay.IsMaster)
                    PhotonNetwork.SetMasterClient(GlobalGameManager.Instance.RemotePlayer);
                else PhotonNetwork.SetMasterClient(PhotonNetwork.LocalPlayer);
                Debug.Log("**************************** FORCE SWITCH MASTER ***********************************");
            }

            if (simOn)
            {
                dcTime += Time.deltaTime;
                Debug.Log("DISCONNECTED FOR : " + dcTime);
            }
        }
#endif

    }

    public static NetworkReachability InternetReachability { get; private set; } = NetworkReachability.NotReachable;
    /// <summary>
    ///	detects cases where the connection process has stalled,
    ///	disconnects and tries to connect again
    /// </summary>
    protected virtual void UpdateConnectionState()
    {
        InternetReachability = Application.internetReachability;

        if (InternetReachability == NetworkReachability.NotReachable)
            return;

        if (!StayConnected)
            return;

        if (PhotonNetwork.NetworkClientState != m_LastClientState)
        {
            string s = PhotonNetwork.NetworkClientState.ToString();
            s = ((PhotonNetwork.NetworkClientState == ClientState.Joined) ? "--- " + s + " ---" : s);
            if (s == "PeerCreated")
                s = "Connecting to the best region's cloud ...";
            Debug.Log(s);
            FST_MPDebug.Log(s);
        }

        Connected = PhotonNetwork.IsConnected;

        if (Connected)
        {
            if (m_ConnectionTimer.Active)
            {
                Debug.Log("Reset Connection Timer");
                m_ConnectionTimer.Cancel();
                m_ConnectionAttempts = 0;
            }
        }
        else if ((PhotonNetwork.NetworkClientState != m_LastClientState) && !m_ConnectionTimer.Active)
        {
            Reconnect();
            FST_Timer.In(LogOnTimeOut, delegate ()
            {
                m_ConnectionAttempts++;
                if (m_ConnectionAttempts < MaxConnectionAttempts || MaxConnectionAttempts == 0)
                {
                    Debug.Log("Retrying (" + m_ConnectionAttempts + ") ...");
                    FST_MPDebug.Log("Retrying (" + m_ConnectionAttempts + ") ...");
                    Reconnect();
                }
                else
                {
                    Debug.Log("Failed to connect (tried " + m_ConnectionAttempts + " times).");
                    FST_MPDebug.Log("Failed to connect (tried " + m_ConnectionAttempts + " times).");
                    Disconnect(false, false);
                }
            }, m_ConnectionTimer);
        }

        m_LastClientState = PhotonNetwork.NetworkClientState;
    }
    public static bool Connected = false;

    /// <summary>
    /// reports ping every 10 (default) seconds by storing it as a custom player
    /// prefs value in the Photon Cloud. 'Ping' is defined as the roundtrip time to
    /// the Photon server and it is only reported if it has changed
    /// </summary>
    public virtual void UpdatePing()
    {
        if (!PhotonNetwork.IsConnectedAndReady)
            return;

        // only report ping every 10 (default) seconds
        if (Time.time < m_NextAllowedPingTime)
            return;
        m_NextAllowedPingTime = Time.time + PingReportInterval;

        // get the roundtrip time to the photon server
        int ping = PhotonNetwork.GetPing();

        // only report ping if it changed since last time
        if (ping == m_LastPing)
            return;
        m_LastPing = ping;

        // send the ping as a custom player property (the first time it will be
        // created, from then on it will be updated)
        Hashtable playerCustomProps = new Hashtable
        {
            { FST_PlayerProps.PING , ping }
        };
        PhotonNetwork.LocalPlayer.SetCustomProperties(playerCustomProps);

    }


    /// <summary>
    /// this method smooths over a harmless error case where 'TryCreateRoom' fails
    /// because someone was creating the same room name at the exact same time as us,
    /// and 'TryCreateRoom' failed to sort it out. instead of pausing the editor and
    /// showing a scary crash dialog, we should keep calm, carry on and reconnect
    /// </summary>
    public override void OnCreateRoomFailed(short sh, string s)
    {
        // unpause editor (if paused)
#if UNITY_EDITOR
        if (UnityEditor.EditorApplication.isPaused)
            UnityEditor.EditorApplication.isPaused = false;
#endif
    }

    private string[] m_ExpectedPlayerIDs;
    public void TryPlayWithFriends(string[] expectedPlayerIDs)
    {
        FST_Gameplay.IsPWF = true;
        if (!FST_Gameplay.IsMultiplayer)
        {
            Debug.Log("MP WAS FALSE!");
            FST_Gameplay.IsMultiplayer = true;
        }

       // GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
    
        if (expectedPlayerIDs[0] == Joga_Data.PlayerID)
        {
            // Debug.Log("I SENT THE NOTIFICATION");
            GameStates.SetCurrent_State_TO(GAME_STATE.LEVELSELECTION);
            TryJoinRoom(expectedPlayerIDs);
        }
        else
        {
            GameStates.SetCurrent_State_TO(GAME_STATE.SELECTFORMATION);
            StartCoroutine(WaitForRoomCreation(expectedPlayerIDs));
        }
    }
    /// <summary>
    /// For PWF when the client accepts we need to wait a period of time before joining that room, this ensures player will in fact have a room to join
    /// </summary>
    /// <param name="expectedPlayerIDs"></param>
    /// <returns></returns>
    private IEnumerator WaitForRoomCreation(string[] expectedPlayerIDs)
    {
        float maxTime = Time.time + 10f;
        //TODO: here we will change this, the player will get a message if the room is ready.
        while (Time.time < maxTime && RoomInfos == null)
        {
            yield return 0;
        }

        TryJoinRoom(expectedPlayerIDs);
    }

    /// <summary>
    /// used internally when user trys to join a room and is not yet "connected and ready"
    /// </summary>
    /// <param name="expectedPlayerIDs"></param>
    /// <returns></returns>
    private IEnumerator RetryRoomInternal(string[] expectedPlayerIDs = null)
    {
        if (!GameManager.Instance.CanJoinRooms)
        {
            FST_MPDebug.Log("status of connection: " + PhotonNetwork.NetworkClientState);
            Debug.Log("status of connection: " + PhotonNetwork.NetworkClientState);

            if (PhotonNetwork.NetworkClientState == ClientState.ConnectedToGameServer || PhotonNetwork.NetworkClientState == ClientState.ConnectingToGameServer)
                Reconnect();
        }


        yield return new WaitUntil(() => GameManager.Instance.CanJoinRooms);

        Debug.Log("RERUN");
        TryJoinRoom(expectedPlayerIDs);
    }

    /// <summary>
    /// creates a new room named and numbered 'MapToLoad + current room count + 1', or joins
    /// that room if someone else has just created it
    /// 
    ///  MapToLoad is the "Room" replacement and namer
    /// </summary>
    public virtual void TryJoinRoom(string[] expectedPlayerIDs = null)
    {
        if (PhotonNetwork.InRoom)
        {
            //in pwf we join early with one player!
            if (expectedPlayerIDs != null && expectedPlayerIDs != PhotonNetwork.CurrentRoom.ExpectedUsers)
            {
                FST_MPDebug.Log("TRYING TO JOIN ROOM BUT ALREADY IN ONE!");
                Debug.Log("TRYING TO JOIN ROOM BUT ALREADY IN ONE!");
                LeaveRoom(false, false);
            }
            else
            {
                FST_MPDebug.Log("TRYING TO JOIN ROOM BUT ALREADY IN CORRECT PWF ROOM!");
                Debug.Log("TRYING TO JOIN ROOM BUT ALREADY IN CORRECT PWF ROOM!");
                return;
            }
        }

        Joga_NetworkManager.Instance.CheckAuthentication();

        if (!GameManager.Instance.CanJoinRooms)
        {
            Debug.Log("USER TRYING TO JOIN ROOM BEFORE CONNECTION IS READY, WILL RERUN");
            StartCoroutine(RetryRoomInternal(expectedPlayerIDs));
            return;
        }

        Debug.Log("room count: " + PhotonNetwork.CountOfRooms + ". room infos count: " + (RoomInfos != null ? RoomInfos.Count.ToString() : "0"));

        string MatchTypeString = string.IsNullOrEmpty(GameManager.CurrentLeagueID) ? FST_SettingsManager.MatchTypeAsString : GameManager.CurrentLeagueID;

        ExitGames.Client.Photon.Hashtable ht = new ExitGames.Client.Photon.Hashtable
        {
            { "MatchType" , MatchTypeString }
        };

        if (RoomInfos != null)
        {
            Debug.Log("trying to find matching room to join");
            for (int i = 0; i < RoomInfos.Count; i++)
            {
                if (RoomInfos[i].PlayerCount >= 2)
                    continue;

                //  Debug.Log(RoomInfos[i].CustomProperties);
                if (RoomInfos[i].CustomProperties.TryGetValue("MatchType", out object matchType) && (string)matchType == MatchTypeString)
                {
                    Debug.Log("found matching a room so will join it");
                    PhotonNetwork.JoinRoom(RoomInfos[i].Name);
                    return;
                }
            }
        }

        RoomOptions roomOptions = new RoomOptions
        {// SuppressRoomEvents = true,
            CleanupCacheOnLeave = false,
            MaxPlayers = MaximumPlayers,
            CustomRoomPropertiesForLobby = new string[] { "MatchType" },//NOTE: not totally needed unless we use a lobby system really. but left here for ease to hook up later
            CustomRoomProperties = ht,
            PlayerTtl = -1/*ActorTimeToLiveOnDc*/,
        };


        string roomName = /*MapToLoad + " " +*/ MatchTypeString + " #" + (PhotonNetwork.CountOfRooms + 1).ToString();

        if (expectedPlayerIDs != null)
        {
            roomName = "PWF" + expectedPlayerIDs[0];
            if (Joga_Data.PlayerID == expectedPlayerIDs[1])
            {
                if (!PhotonNetwork.JoinRoom(roomName, expectedPlayerIDs))
                {
                    FST_MPDebug.Log("ROOM NOT FOUND");
                    Debug.LogError("ROOM NOT FOUND");
                }
                return;
            }
        } 

        // join the wanted room, if none available create it!
        if (PhotonNetwork.JoinOrCreateRoom(roomName, roomOptions, lobby, expectedPlayerIDs))
        {
            FST_MPDebug.Log("no matching room found so creating one " + roomOptions.CustomRoomProperties);
            //  Debug.Log("create or join room success \n" + roomName);
        }
    }
    public static TypedLobby lobby = TypedLobby.Default;//new TypedLobby("MainLobby", LobbyType.SqlLobby);
    /// <summary>
    /// Initial connection is attempted here
    /// </summary>
    public virtual void Connect()
    {
        //keep connection alive on dropouts
        StayConnected = true;

        if (InternetReachability == NetworkReachability.NotReachable)
        {
            Debug.Log("No Internet, Abort Connect()");
            return;
        }

        Debug.Log("Connect()");

        PhotonNetwork.LocalPlayer.NickName = FST_SettingsManager.PlayerName;

        // Debug.Log("SET PUN PLAYER ID TO: " + Joga_Data.PlayerID);

        PhotonNetwork.GameVersion = Application.version; // auto set by Unity build version

        PhotonNetwork.AuthValues = new AuthenticationValues
        {
            AuthType = CustomAuthenticationType.Custom,
            // PhotonNetwork.AuthValues.AddAuthParameter("version", version); // HTTP GET
            UserId = Joga_Data.PlayerID
        };

        PhotonNetwork.SendRate = SendRate;
        PhotonNetwork.SerializationRate = SerializationRate;
        PhotonNetwork.KeepAliveInBackground = BackgroundTimeout;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = Application.version;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "0c5aa1fb-7657-4e9f-a34c-95ff430ca33b";
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat = "ae8e0652-b935-4bf8-aaf2-6ea5cf914ba4";

        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "";//if user prefs we can set here.. or...
        //... use... with some setup also..
        // PhotonNetwork.ConnectToRegion("za"); 

        PhotonNetwork.ConnectUsingSettings();
        FST_MainChat.Instance.Connect();
    }

    protected void ConnectToChina()
    {
        // you could also set these values directly in the PhotonServerSettings from Unity Editor
        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = "cn";
        PhotonNetwork.PhotonServerSettings.AppSettings.UseNameServer = true;
        PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "ChinaPUNAppId"; // TODO: replace with your own AppId
        PhotonNetwork.PhotonServerSettings.AppSettings.AppVersion = "ChinaAppVersion"; // optional
        PhotonNetwork.PhotonServerSettings.AppSettings.Server = "ns.photonengine.cn";
        PhotonNetwork.ConnectUsingSettings();
    }

    /// <summary>
    /// used internally to disconnect and immediately reconnect
    /// </summary>
    protected virtual void Reconnect()
    {
        Debug.Log("Reconnect()");

        if (FST_Gameplay.IsMultiplayer && /*SceneManagerHelper.ActiveSceneName == "InGame"*/GlobalGameManager.Instance && GlobalGameManager.Instance.Phase != GlobalGameManager.GamePhase.Finished)
        {
            StayConnected = true;
            Debug.Log("D/C > Attempt to rejoin room");
            if (InternetReachability != NetworkReachability.NotReachable)
                PhotonNetwork.ReconnectAndRejoin();
            //   OnDisconnect?.Invoke();
            return;
        }
        else if (PhotonNetwork.NetworkClientState != ClientState.Disconnected
            && PhotonNetwork.NetworkClientState != ClientState.PeerCreated)
        {
            Debug.Log("Reconnect() > Disconnecting before Connect()");
            PhotonNetwork.Disconnect();
        }

        Connect();
    }

    public virtual void LeaveRoom(bool freshScene, bool onlyBecomeInactive)
    {
        if (Instance)
            Instance.StopAllCoroutines();

        if (freshScene)
            FST_Gameplay.IsMultiplayer = false;
        if (PhotonNetwork.InRoom)
            PhotonNetwork.LeaveRoom(onlyBecomeInactive);

        m_ConnectionAttempts = 0;

        // load a blank scene (if provided) to destroy the currently played level.
        // NOTE: by default some master gameplay objects (such
        // as this component) will survive
        if (freshScene && !string.IsNullOrEmpty(SceneToLoadOnDisconnect))
            SceneManager.LoadScene(SceneToLoadOnDisconnect);

        FST_MPDebug.Log("--- Left Room: " + PhotonNetwork.InRoom);
        Debug.Log("--- Left Room: " + PhotonNetwork.InRoom);
    }

    public delegate void DisconnectForced();

    public static event DisconnectForced OnDisconnectForced;
    /// <summary>
    /// disconnects the player from an ongoing game, loads a blank level
    /// (if provided) and sends a globalevent informing external objects
    /// of the disconnect. TIP: call this method from anywhere using:
    /// FST_MPConnection.Instance.Disconnect();
    /// </summary>
    public virtual void Disconnect(bool disableAutoConnection, bool freshScene = true)
    {
        FST_Gameplay.IsMultiplayer = false;

        if (PhotonNetwork.NetworkClientState == ClientState.Disconnected)
            return;

        OnDisconnectForced?.Invoke();
        // disable MP auto-reconnection and disconnect from Photon
        StayConnected = false;
        PhotonNetwork.Disconnect();
        m_ConnectionAttempts = 0;

        // load a blank scene (if provided) to destroy the currently played level.
        // NOTE: by default some master gameplay objects (such
        // as this component) will survive
        if (freshScene && !string.IsNullOrEmpty(SceneToLoadOnDisconnect)/* && SceneManager.GetActiveScene().name != SceneToLoadOnDisconnect*/)
            SceneManager.LoadScene(SceneToLoadOnDisconnect);

        // send a message to inform external objects that we have disconnected
        FST_GlobalEvent.Send("Disconnected");

        Debug.Log("--- Disconnected ---");

        //Notify user by chat function
        FST_MPDebug.Log("--- Disconnected ---");
    }
    //called once only???
    public override void OnConnectedToMaster()//Server
    {
        //  PhotonNetwork.LocalPlayer.NickName = FST_SettingsManager.PlayerName;
        PhotonNetwork.JoinLobby(lobby);
        Debug.Log("OnConnectedToMaster()");
    }

    /// <summary>
    /// 
    /// </summary>
    public override void OnJoinedLobby()
    {
        RoomInfos.Clear();
        // update name of this player in the cloud
        PhotonNetwork.LocalPlayer.NickName = FST_SettingsManager.PlayerName;
        GameManager.Instance.CanJoinRooms = true;
        Debug.Log("OnJoinedLobby()");
    }
    /// <summary>
    /// A cached list of rooms recieved when roomlist updates on pun. This allows us to access this list at any time.
    /// </summary>
    public List<RoomInfo> RoomInfos { get; private set; } = new List<RoomInfo>();
    /// <summary>
    /// Here we simply cache the list we recieve, which allows use for later or in between updates
    /// </summary>
    public override void OnRoomListUpdate(List<RoomInfo> roomInfos)
    {
        RoomInfos = roomInfos;

        FST_MPDebug.Log("Total players using app: " + PhotonNetwork.CountOfPlayers);
        Debug.Log("OnRoomListUpdate() " + roomInfos);
    }


    /// <summary>
    /// updates the 'IsMaster' flag which gets read by Base classes.
    /// also, master will set the max players for the room
    /// </summary>
    public override void OnJoinedRoom()
    {
        StayConnected = true;
        if (!FST_Gameplay.IsMultiplayer)
        {
            Debug.Log("OnJoinedRoom() > MP has been cancelled, disconnecting!");
            LeaveRoom(false, false);
            return;
        }
        Debug.Log("OnJoinedRoom()");

        if (PhotonNetwork.IsMasterClient)
            PhotonNetwork.CurrentRoom.MaxPlayers = (byte)MaximumPlayers;

        if (!GlobalGameManager.Instance)// we only do this if not in game scene, else it can mess up the final win screen
            FST_Gameplay.IsMaster = PhotonNetwork.IsMasterClient;

        OnPlayerEntered?.Invoke(PhotonNetwork.LocalPlayer);
    }

    public delegate void PlayerEntered(Player newPlayer);

    public static event PlayerEntered OnPlayerEntered;

    /// <summary>
    /// updates the cached RemotePlayerNameString which gets read by Base classes.
    /// also, announces players joining in the chat (if any)
    /// </summary>
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + " joined the game");
        FST_MPDebug.Log(newPlayer.NickName + " joined the game");

        OnPlayerEntered?.Invoke(newPlayer);

        if (!GlobalGameManager.Instance)
        {// we only do this if not in game scene, else it can mess up the data
            if (!newPlayer.IsMasterClient)
                GameManager.Instance.RemotePlayerNameString = newPlayer.NickName;
        }
    }

    public delegate void PlayerLeft(Player player);

    public static event PlayerLeft OnPlayerLeft;

    /// <summary>
    ///  Announces players leaving in the chat (if any), and invokes any registered event. <br></br>
    ///  Subscribe to the event with FST_MPConnection.OnPlayerLeft to save double handling room events!
    /// </summary>
    public override void OnPlayerLeftRoom(Player player)
    {
        OnPlayerLeft?.Invoke(player);

        Debug.Log(player.NickName + " left the game");
        FST_MPDebug.Log(player.NickName + " left the game");
    }

    public delegate void MasterSwitched(Player newMaster);
    public static event MasterSwitched OnMasterSwitched;
    /// <summary>
    ///  updates the 'IsMaster' flag which gets read by Base classes.
    /// </summary>
    /// <param name="newMasterClient"></param>
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        FST_Gameplay.IsMaster = PhotonNetwork.IsMasterClient;
        Debug.Log("OnMasterClientSwitched, new master: " + newMasterClient);
        FST_MPDebug.Log("OnMasterClientSwitched, new master: " + newMasterClient);
        OnMasterSwitched?.Invoke(newMasterClient);

        if(GlobalGameManager.Instance && GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.Finished)
        {
            Debug.Log("MASTER SWITCH ON FINISH!!! REPAIRING....");

            Reconnect();
        }
    }


    /// <summary>
    /// A cached list of all enabled PUN regions
    /// </summary>
    public List<Region> EnabledRegions { get; private set; } = null;

    /// <summary>
    /// cache the region recieved, we may use these for player choice
    /// </summary>
    /// <param name="regionHandler"></param>
    public override void OnRegionListReceived(RegionHandler regionHandler)
    {
        EnabledRegions = regionHandler.EnabledRegions;
        Debug.Log("OnRegionListReceived() > Best region: " + regionHandler.BestRegion + " , EnabledRegions: " + EnabledRegions.ToStringFull());
    }
    public delegate void RoomUpdate(Hashtable propertiesThatChanged);

    public static event RoomUpdate OnRoomUpdate;
    public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
    {
       // Debug.Log("OnRoomPropertiesUpdate() > " + propertiesThatChanged.ToString());
        OnRoomUpdate?.Invoke(propertiesThatChanged);
    }

    // public delegate void Disconnected();

    //  public static event Disconnected OnDisconnect;
    public override void OnDisconnected(DisconnectCause disconnectCause)
    {
        Debug.Log("OnDisconnected >" + disconnectCause);

        if (FST_AppHandler.IsQuitting)
            return;

        GameManager.Instance.CanJoinRooms = false;

        if (disconnectCause != DisconnectCause.DisconnectByClientLogic)
        {
            if (GlobalGameManager.Instance)
            {
                FST_MPDebug.Log("D/C OPEN RECONNECT!!");
                Debug.Log("D/C OPEN RECONNECT!!");
                GlobalGameManager.Instance.OnPlayerLeft(PhotonNetwork.LocalPlayer);
            }
        }

        switch (disconnectCause)
        {
            case DisconnectCause.None:
                break;
            case DisconnectCause.ExceptionOnConnect:
                break;
            case DisconnectCause.Exception:
                break;
            case DisconnectCause.ServerTimeout:
                break;
            case DisconnectCause.ClientTimeout:
                break;
            case DisconnectCause.DisconnectByServerLogic:
                break;
            case DisconnectCause.DisconnectByServerReasonUnknown:
                break;
            case DisconnectCause.InvalidAuthentication:
                break;
            case DisconnectCause.CustomAuthenticationFailed:
                break;
            case DisconnectCause.AuthenticationTicketExpired:
                break;
            case DisconnectCause.MaxCcuReached:
                break;
            case DisconnectCause.InvalidRegion:
                break;
            case DisconnectCause.OperationNotAllowedInCurrentState:
                break;
            case DisconnectCause.DisconnectByClientLogic:
                break;
            default:
                break;
        }

    }


    //public override void OnConnected()
    //{
    //    Debug.Log("OnConnected()");
    //    base.OnConnected();
    //}

    //public override void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
    //{
    //    base.OnLobbyStatisticsUpdate(lobbyStatistics);
    //    Debug.Log("lobby stats update");
    //}

    //public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    //{
    //    base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);
    //    // Debug.Log("OnPlayerPropertiesUpdate()");
    //}

    //public override void OnFriendListUpdate(List<FriendInfo> friendList)
    //{
    //    base.OnFriendListUpdate(friendList);
    //    Debug.Log("OnFriendListUpdate()");
    //}

    //public override void OnCreatedRoom()
    //{
    //    base.OnCreatedRoom();
    //    Debug.Log("OnCreatedRoom()");
    //}

    //public override void OnJoinRoomFailed(short returnCode, string message)
    //{
    //    base.OnJoinRoomFailed(returnCode, message);
    //    Debug.Log("OnJoinRoomFailed()");
    //}
    public override void OnLeftRoom()
    {
        Debug.Log("OnLeftRoom()");
        OnPlayerLeft?.Invoke(PhotonNetwork.LocalPlayer);
    }

    //public override void OnCustomAuthenticationResponse(Dictionary<string, object> data)
    //{
    //    base.OnCustomAuthenticationResponse(data);
    //    Debug.Log("OnCustomAuthenticationResponse()");
    //}

    //public override void OnCustomAuthenticationFailed(string debugMessage)
    //{
    //    base.OnCustomAuthenticationFailed(debugMessage);
    //    Debug.Log("OnCustomAuthenticationFailed(): " + debugMessage);
    //}

    //public override void OnJoinRandomFailed(short sh, string s)
    //{
    //    base.OnJoinRandomFailed(sh, s);
    //    Debug.Log("join random failed");
    //    //PhotonNetwork.CreateRoom(null);
    //}

    public override void OnLeftLobby()
    {
        Debug.Log("left lobby");
    }
}

