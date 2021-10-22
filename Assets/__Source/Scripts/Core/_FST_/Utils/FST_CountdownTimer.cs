using UnityEngine;
using UnityEngine.UI;
using ExitGames.Client.Photon;
using Photon.Pun;

namespace FastSkillTeam
{
    /// <summary>
    /// This is a basic CountdownTimer. In order to start the timer, the MasterClient can add a certain entry to the Custom Room Properties,
    /// which contains the property's name 'StartTime' and the actual start time describing the moment, the timer has been started.
    /// To have a synchronized timer, the best practice is to use PhotonNetwork.Time.
    /// In order to subscribe to the CountdownTimerHasExpired event you can call CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
    /// from Unity's OnEnable function for example. For unsubscribing simply call CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;.
    /// You can do this from Unity's OnDisable function for example.
    /// </summary>
    public class FST_CountdownTimer : MonoBehaviour
    {
        public static FST_CountdownTimer Instance;

        public const string CountdownStartTime = "StartTime";
        public const string CountdownPaused = "CountdownPaused";
        public const string FormationStartTime = "FormationStart";
        /// <summary>
        /// OnCountdownTimerHasExpired delegate.
        /// </summary>
        public delegate void CountdownTimerHasExpired();

        /// <summary>
        /// Called when the timer has expired.
        /// </summary>
        public static event CountdownTimerHasExpired OnCountdownTimerHasExpired;

        public static bool IsTimerRunning { get; private set; }
        public static bool IsFormationTimerRunning { get; private set; }

        private float startTime;

        [Header("Reference to a Text component for visualizing the countdown")]
        public Text Text;
        public Text TextForFormation;

        //[Header("Countdown time in seconds")]
        public float Countdown { get { return countdown; }set { countdown = value; FST_MPDebug.Log("countdown set to: " + countdown); Debug.Log("countdown set to: " + countdown); } }
        private float countdown = 5.0f;
        public float TimeLeft { get; private set; }

        private void Awake()
        {
            Instance = this;
            FST_MPConnection.OnRoomUpdate += OnRoomUpdate;
        }

        private void OnDisable()
        {
            FST_MPConnection.OnRoomUpdate -= OnRoomUpdate;
        }

        public void Start()
        {
            if (Text == null)
            {
                Debug.LogError("Reference to 'Text' is not set. Please set a valid reference.", this);
                return;
            }
        }

        public void Update()
        {
            if (!IsTimerRunning)
            {
                if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.BetweenRounds
                    || GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.RoundEnded
                    || GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.WaitingForRoomUpdate)
                    Text.text = "TURN CHANGE!";

                else if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.BallKicked)
                    Text.text = "KICKED!";

                else if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.GoalIntermission)
                    Text.text = "GOAL INTERMISSION!";

                else if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.FormationClosed)
                    Text.text = "ASSIGN FORMATIONS!";

                else if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.Finished)
                    Text.text = "GAME OVER!";

                return;
            }

            if (!PhotonNetwork.InRoom || PhotonNetwork.NetworkClientState == Photon.Realtime.ClientState.Leaving)
            {
                IsTimerRunning = false;
                return;
            }

            float timer = (float)PhotonNetwork.Time - startTime;
            TimeLeft = Countdown - timer;

            if (TextForFormation.gameObject.activeInHierarchy)
            {
                Text.text = "SELECT FORMATION!";
                TextForFormation.text = string.Format("Time Remaining: {0} seconds", TimeLeft.ToString("n0"));
            }
            else Text.text = string.Format((GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.NotStarted ? "Game starts in" : "Round ends in") + " {0} seconds", TimeLeft.ToString("n0"));

            if (TimeLeft > 0.0f)
                return;

            if (!GlobalGameManager.Instance.IsAllPlayersConnected && !IsFormationTimerRunning)
            {
                Text.text = "PAUSED";
                return;
            }

            IsFormationTimerRunning = false;
            IsTimerRunning = false;

            Text.text = "TIME UP"; 

            OnCountdownTimerHasExpired?.Invoke();
        }

        private void OnRoomUpdate(Hashtable propertiesThatChanged)
        {
            if (GlobalGameManager.Instance.Phase == GlobalGameManager.GamePhase.Finished)
            {
                //                UIManager.Instance.CloseReconnectingPanel();
                //                UIManager.Instance.CloseOppoReconnectingPanel();
                return;
            }

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out object o))
            {
                if (o == null)
                    return;
                if (GlobalGameManager.Instance.IsAllPlayersConnected)
                {
                    Debug.Log("Timer Start!");

                    if (UIManager.Instance.LoadingInGame.activeSelf)
                        UIManager.Instance.LoadingInGame.SetActive(false);

                    IsTimerRunning = true;
                    startTime = (float)o;
                }
                else Debug.Log("Timer Start Abandoned!");
            }
            else if (propertiesThatChanged.TryGetValue(FormationStartTime, out o))
            {
                if (o == null)
                    return;

                Debug.Log("Formation Timer Start!");

                if (UIManager.Instance.LoadingInGame.activeSelf)
                    UIManager.Instance.LoadingInGame.SetActive(false);

                IsFormationTimerRunning = true;
                IsTimerRunning = true;
                startTime = (float)o;

            }
            else if (propertiesThatChanged.TryGetValue(CountdownPaused, out o))
            {
                if (o == null)
                    return;
                Debug.Log("Timer Paused!");
                Countdown = (float)o;

                IsTimerRunning = false;
            }
        }

        public void StartTimer()
        {
            if (FST_Gameplay.IsMultiplayer && PhotonNetwork.InRoom)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { CountdownStartTime, (float)PhotonNetwork.Time } });
        }

        public void StartFormationTimer()
        {
            if (FST_Gameplay.IsMultiplayer && PhotonNetwork.InRoom)
                PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FormationStartTime, (float)PhotonNetwork.Time } });
        }

        public void StopTimer()
        {
            if (FST_Gameplay.IsMultiplayer && PhotonNetwork.InRoom)
            {
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CountdownStartTime))
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { CountdownStartTime, null } });
                if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(FormationStartTime))
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { FormationStartTime, null } });
            }

            IsTimerRunning = false;
        }
       // float cachedCountdown = 0;
        public void PauseTimer(bool isPaused, string whoPaused, bool setProps, bool isDisconnected = false)
        {
            if (!isPaused)
            {
                if (setProps)
                {
                    if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey(CountdownPaused))
                        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { CountdownPaused, null }, { "pdcn", null } });
                    FST_MPDebug.Log("Timer resumed by: " + whoPaused);
                    Debug.Log("Timer resumed by: " + whoPaused);
                    StartTimer();
                }
            }
            else
            {
                IsTimerRunning = false;
                Text.text = "Paused";
                if (FST_Gameplay.IsMultiplayer && PhotonNetwork.InRoom)
                {
                    if (setProps)
                    {
                        FST_MPDebug.Log("Timer paused by: " + whoPaused);
                        Debug.Log("Timer paused by: " + whoPaused);
                        Countdown = TimeLeft;
                        PhotonNetwork.CurrentRoom.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { CountdownPaused, Countdown }, { "pdcn", whoPaused } });
                    }
                }
            }
        }
    }
}