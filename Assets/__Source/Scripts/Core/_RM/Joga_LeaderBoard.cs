using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class Joga_LeaderBoard : MonoBehaviour
    {
        public enum Filter
        {
            all = 0,
            score = 1,
            conceded = 2,
            winStreak = 3,
            cleanSheets = 4,
            avgPointsPerMinute = 5,
            userRounds = 6
        }

        public Text mLeaderBoardName;
        public GameObject PointLeaderBoard;
        public GameObject GoalLeaderBoard;
        public GameObject WinStreakLeaderBoard;
        public GameObject GoalConsidLeaderBoard;
        public GameObject CleansheetLeaderBoard;
        public GameObject AllStatPrefab;
        public Transform LeaderBoardPointParentObject;
        public Transform LeaderBoardGoalParentObject;
        public Transform LeaderBoardCleansheetParentObject;
        public Transform LeaderBoardGoalConcededParentObject;
        public Transform LeaderBoardWinstrekParentObject;
 
#pragma warning disable CS0649
        [Header("Disabled Objects When Not League")]
        [SerializeField] private GameObject[] m_LeagueOnlyObjects;
        [Header("League Stats For User")]
        [SerializeField] private GameObject m_UserStats;
        [SerializeField] private Button m_UserStatsBtn;
        [SerializeField] private Text m_UserStatsText;
        [SerializeField] private GameObject m_UserLeagueStatsLeaderBoard;
        [SerializeField] private Transform m_UserLeagueStatsParentObject;

        [Header("Total Prize Pool")]
        [SerializeField] private Text m_TotalPrizePoolText;

        [Header("AveragePoints Per Minute")]
        [SerializeField] private GameObject m_AveragePpmLeaderBoard;
        [SerializeField] private Transform m_AvgPPMParentObject;
        [SerializeField] private Text m_AvgPPMText;
        [SerializeField] private Button m_AvgPPMButton;

        [Header("ZeroOrNullData")]
        [SerializeField] private GameObject m_ZeroOrNullDataOb;

#pragma warning restore CS0649
        [Header("The Rest")]
        public GameObject overallText;
        public GameObject overallBtn;
        public GameObject WinstreakBtn;
        public GameObject WinstreakText;

        public GameObject seasonLeadBtn;
        public GameObject goalScoredText;
        public GameObject goalScoredBtn;
        public GameObject goalConcededText;
        public GameObject goalConcededBtn;
        public GameObject cleanSheetText;
        public GameObject cleanSheetBtn;
        GameObject GM { get { return FindObjectOfType<GameManager>().gameObject; } }// stupid fix, but this script never ever ref this!!! that why it not work! >FST

        public Image TopBarPointLeaderBoard;
        public Image[] TobBarSubBoards;
        public Sprite[] Status;//buttons
        public Sprite[] m_LeaderboardPlayerBackgrounds;
        public Sprite[] TopBarSpritesLeaderBoard;
        public Sprite[] TopBarSpritesSubLeaderboard;
        public string loadMoreFor = "points";
        public int skip = 0;

        //This will be the replacement to check the match type
        public Joga_MatchType matchType;
        public Filter filter;

        private Joga_LeaderBoardPlayer m_leaderBoardData = null;

        private Joga_LeaderBoardPlayer LeaderBoardData
        {
            get
            {
                if (!m_leaderBoardData && GM)
                    m_leaderBoardData = GM.GetComponent<Joga_LeaderBoardPlayer>();

                return m_leaderBoardData;
            }
        }

        private Joga_LeaderBoardPlayer playerLeaderbData;
        public static Joga_LeaderBoard Instance;
        private JSONNode m_JSON;

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }

        private void OnEnable()
        {
            for (int i = 0; i < LeaderBoardPointParentObject.childCount; i++)
                Destroy(LeaderBoardPointParentObject.GetChild(i).gameObject);

            for (int i = 0; i < m_UserLeagueStatsParentObject.childCount; i++)
                Destroy(m_UserLeagueStatsParentObject.GetChild(i).gameObject);
            //  Debug.Log("On Enable " + this);
            matchType = GameManager.Instance.IsOneNOneLeaderBoard ? Joga_MatchType.oneonone : Joga_MatchType.leagueGamePlay;

            m_UserStats.SetActive(!GameManager.Instance.IsOneNOneLeaderBoard);

            LoadLeaderBoard(Filter.all);

            m_UserStatsBtn.onClick.AddListener(() => 
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.userRounds);
            });

            m_AvgPPMButton.onClick.AddListener(() =>
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.avgPointsPerMinute);
            });

            overallBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.all);
            });

            goalScoredBtn.GetComponent<Button>().onClick.AddListener(() => 
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.score);
            });

            WinstreakBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.winStreak);
            });

            goalConcededBtn.GetComponent<Button>().onClick.AddListener(() => 
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.conceded);
            });

            cleanSheetBtn.GetComponent<Button>().onClick.AddListener(() =>
            {
                HelpPopup.instance.ClosePop();
                LoadLeaderBoard(Filter.cleanSheets);
            });

            HelpPopup.instance.PointsInfoText.gameObject.SetActive(matchType != Joga_MatchType.leagueGamePlay);
            /*
                        seasonLeadBtn.GetComponent<Button>().onClick.AddListener(() =>
                        {
                            HelpPopup.instance.ClosePop();
                        });
            */
        }

        private void OnDisable()
        {
            if (Joga_NetworkManager.Instance)
                Joga_NetworkManager.Instance.CancelLeaderBoardRefresh();
            m_AvgPPMButton.onClick.RemoveAllListeners();
            overallBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            WinstreakBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            goalScoredBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            goalConcededBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            cleanSheetBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            m_UserStatsBtn.onClick.RemoveAllListeners();
            //   seasonLeadBtn.GetComponent<Button>().onClick.RemoveAllListeners();
            CancelInvoke();
        }

        private void CloseBoard()
        {
            for (int i = 0; i < m_UserLeagueStatsParentObject.childCount; i++)
                Destroy(m_UserLeagueStatsParentObject.GetChild(i).gameObject);

            PointLeaderBoard.SetActive(filter == Filter.all);
            GoalLeaderBoard.SetActive(filter == Filter.score);
            GoalConsidLeaderBoard.SetActive(filter == Filter.conceded);
            CleansheetLeaderBoard.SetActive(filter == Filter.cleanSheets);
            WinStreakLeaderBoard.SetActive(filter == Filter.winStreak);
            m_AveragePpmLeaderBoard.SetActive(filter == Filter.avgPointsPerMinute);
            m_UserLeagueStatsLeaderBoard.SetActive(filter == Filter.userRounds);
        }

        private void SetBanner()
        {
            playerLeaderbData.Init(filter == Filter.all || filter == Filter.userRounds, matchType == Joga_MatchType.leagueGamePlay);
            //  ***HIGHLIGHT MVP
            if (playerLeaderbData.PlayerName.Split(' ')[0] == FastSkillTeam.FST_SettingsManager.PlayerName)
            {
                playerLeaderbData.GetComponent<Image>().sprite = m_LeaderboardPlayerBackgrounds[1];

                if(matchType == Joga_MatchType.leagueGamePlay)
                {
                    GameObject instance = Instantiate(playerLeaderbData.gameObject, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = playerLeaderbData.name;
                    instance.transform.SetParent(m_UserLeagueStatsParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
            }
            else playerLeaderbData.GetComponent<Image>().sprite = m_LeaderboardPlayerBackgrounds[0];

          //  playerLeaderbData.OverallLeaderBoardBanner.SetActive(filter == Filter.all || filter == Filter.userRounds);
            playerLeaderbData.GoalForLeaderBoardBanner.SetActive(filter == Filter.score);
            playerLeaderbData.GoalConsidedLeaderBoardBanner.SetActive(filter == Filter.conceded);
            playerLeaderbData.CleanSheetLeaderBoardBanner.SetActive(filter == Filter.cleanSheets);
            playerLeaderbData.WinstreakLeaderBoardBanner.SetActive(filter == Filter.winStreak);
            playerLeaderbData.AvgPPMLeaderBoardBanner.SetActive(filter == Filter.avgPointsPerMinute);
        }

        private Color blank = new Color(0, 0, 0, 255f);
        private void SetColorAndSprite()
        {
            overallText.GetComponent<Text>().color = filter == Filter.all ? Color.white : blank;
            goalScoredText.GetComponent<Text>().color = filter == Filter.score ? Color.white : blank;
            goalConcededText.GetComponent<Text>().color = filter == Filter.conceded ? Color.white : blank;
            cleanSheetText.GetComponent<Text>().color = filter == Filter.cleanSheets ? Color.white : blank;
            WinstreakText.GetComponent<Text>().color = filter == Filter.winStreak ? Color.white : blank;
            m_AvgPPMText.color = filter == Filter.avgPointsPerMinute ? Color.white : blank;

            m_UserStatsText.color = filter == Filter.userRounds ? Color.white : blank;

            overallBtn.GetComponent<Image>().sprite = filter == Filter.all ? Status[0] : Status[1];
            goalScoredBtn.GetComponent<Image>().sprite = filter == Filter.score ? Status[0] : Status[1];
            goalConcededBtn.GetComponent<Image>().sprite = filter == Filter.conceded ? Status[0] : Status[1];
            cleanSheetBtn.GetComponent<Image>().sprite = filter == Filter.cleanSheets ? Status[0] : Status[1];
            WinstreakBtn.GetComponent<Image>().sprite = filter == Filter.winStreak ? Status[0] : Status[1];
            m_AvgPPMButton.image.sprite = filter == Filter.avgPointsPerMinute ? Status[0] : Status[1];
            m_UserStatsBtn.image.sprite = filter == Filter.userRounds ? Status[0] : Status[1];
        }

        private string SortBy(Filter filter)
        {
            string sort = "";

            switch (filter)
            {
                case Filter.all:
                    sort = "points";
                    break;

                case Filter.userRounds:
                    sort = "points";
                    break;

                case Filter.score:
                    sort = "goalFor";
                    break;

                case Filter.conceded:
                    sort = "goalAgainst";
                    break;

                case Filter.winStreak:
                    sort = "highestWinStreak";
                    break;

                case Filter.cleanSheets:
                    sort = "cleanSheet";
                    break;

                case Filter.avgPointsPerMinute:
                    sort = "avgPointsPerMinute";
                    break;
            }

            return sort;
        }

        public string ParseData(string objectName, int index)
        {
            bool isString = m_JSON["data"][index][objectName].IsString;
            string value = isString ? m_JSON["data"][index][objectName].Value : m_JSON["data"][index][objectName].AsDouble.ToString();

            return value;
        }
        public void LoadLeaderBoard(Filter filterIndex)
        {
            filter = filterIndex;

            string sortBy = SortBy(filter);

            if ((filter == Filter.all || filter == Filter.userRounds) && matchType == Joga_MatchType.leagueGamePlay)
                sortBy = "";

         //   Debug.Log("LEADERBOARD FILTERED BY : " + (string.IsNullOrEmpty(sortBy) ? "League Overall" : sortBy));

            Joga_NetworkManager.Instance.GetLeaderBoardData(matchType, sortBy, m_LeagueIDFilter);

            UIController.Instance.Loading_2_panel.SetActive(true);
        }

        private string[] ZeroDataDisplayString = new string[] { "No games have been completed in this league yet!", "You have not competed in this league yet!" };

        public void AssignZeroOrNullData()
        {
            m_ZeroOrNullDataOb.GetComponentInChildren<Text>().text = filter == Filter.userRounds ? ZeroDataDisplayString[1] : ZeroDataDisplayString[0];
            m_ZeroOrNullDataOb.SetActive(true);
            UIController.Instance.Loading_2_panel.SetActive(false);
        }

        private static int count;
        /// <summary>
        /// Get Leaderboard Data
        /// </summary>
        public void GetData(JSONNode json)
        {
            ++count;
            FST_MPDebug.Log("Leaderboard: Update count this session = " + count);
            Debug.Log("Leaderboard: Update count this session = " + count);

            m_ZeroOrNullDataOb.SetActive(false);

            m_JSON = json;

            CloseBoard();

            switch (filter)
            {
                case Filter.all:
                    AssignLeaderBoardData();
                    break;

                case Filter.userRounds:
                    AssignLeaderBoardData();

                    if (m_UserLeagueStatsParentObject.childCount < 1)
                        AssignZeroOrNullData();
          
                    break;

                case Filter.score:
                    AssignGoalBoard();
                    break;

                case Filter.conceded:
                    AssignGoalConsidedBoard();
                    break;

                case Filter.winStreak:
                    AssignWinStreak();
                    break;

                case Filter.cleanSheets:
                    AssignCleanSheetBoard();
                    break;

                case Filter.avgPointsPerMinute:
                    AssignAvgPPMBoard();
                    break;
            }

            UIController.Instance.Loading_2_panel.SetActive(false);
        }

        private static string m_LeagueIDFilter = "";
        private static FastSkillTeam.FST_LeagueCard cachedLeagueButton = null;
        public static void ShowLeaderBoard(FastSkillTeam.FST_LeagueCard leaguebutton, string leagueID = "")
        {
            cachedLeagueButton = leaguebutton;
            m_LeagueIDFilter = leagueID;
            GameManager.Instance.MainMenuEvents("Leagueleaderboard");
            HelpPopup.instance.PointsInfoText.gameObject.SetActive(false);
        }

        public void AssignLeaderBoardData()
        {
            skip = 0;
            loadMoreFor = "points";

            PointLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;

            int childCount = LeaderBoardPointParentObject.childCount;

         //   Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                   // Debug.Log("destroy rank card : " + i);
                    Destroy(LeaderBoardPointParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeaderBoardPointParentObject.childCount;

            //   Debug.Log("2. rank card count : " + childCount);

            bool isLeague = matchType == Joga_MatchType.leagueGamePlay;

            for (int i = 0; i < m_LeagueOnlyObjects.Length; i++)
            {
                m_LeagueOnlyObjects[i].SetActive(isLeague);
            }

            if (isLeague)
            {
                for (int i = 0; i < TobBarSubBoards.Length; i++)
                    TobBarSubBoards[i].sprite = TopBarSpritesSubLeaderboard[1];

                TopBarPointLeaderBoard.sprite = TopBarSpritesLeaderBoard[1];
                HelpPopup.instance.Init(cachedLeagueButton);

                mLeaderBoardName.text = cachedLeagueButton ? cachedLeagueButton.LeagueName : "BETA 1 LEAGUE";

                GameManager.CurrentLeagueID = cachedLeagueButton.LeagueID;
                Joga_LeaderBoardPlayer.Currency = cachedLeagueButton.Currency;
            }
            else
            {
                for (int i = 0; i < TobBarSubBoards.Length; i++)
                    TobBarSubBoards[i].sprite = TopBarSpritesSubLeaderboard[0];

                TopBarPointLeaderBoard.sprite = TopBarSpritesLeaderBoard[0];
                mLeaderBoardName.text = "Online-Practice Leader Board";
                HelpPopup.instance.PrixeText.enabled = false;
                GameManager.CurrentLeagueID = "";
                m_TotalPrizePoolText.text = "N/A";
            }

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
                 //   Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(LeaderBoardPointParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = LeaderBoardPointParentObject.GetChild(i).gameObject;

                //get the Joga_LeaderBoardPlayer
                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();

                //assign the data
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.GoalBy = ParseData("goalFor", i);
                playerLeaderbData.GoalAgainst = ParseData("goalAgainst", i);
                playerLeaderbData.GoalDifference = ParseData("goalDiff", i);
                playerLeaderbData.Leg = playerLeaderbData.TotalMatch = int.TryParse(ParseData("matchesPlayed", i), out int res) ? res : 0;
                playerLeaderbData.MatchWin = ParseData("win", i);
                playerLeaderbData.MatchLose = ParseData("loss", i);
                playerLeaderbData.Cleansheet = ParseData("cleanSheet", i);
                playerLeaderbData.Points = ParseData("points", i);
                playerLeaderbData.AveragePointsPerMinute = ParseData("avgPointsPerMinute", i);
                playerLeaderbData.Rank = i + 1;


                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out res) ? res : 0;
           //     playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                playerLeaderbData.Prize = float.TryParse(ParseData("prize", i), out float result) ? result : 0;
    /*            bool prizeActive = isLeague && playerLeaderbData.Prize > 0;
              //  playerLeaderbData.prizeObj.SetActive(prizeActive);
                playerLeaderbData.prizeFor.enabled = prizeActive;
                playerLeaderbData.leagueRoundText.enabled = isLeague;
                playerLeaderbData.leagueLegText.enabled = isLeague;*/
                SetBanner();
            }
        }

        public void AssignGoalBoard()
        {
            skip = 0;
            loadMoreFor = "goalFor";

            GoalLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;

            int childCount = LeaderBoardGoalParentObject.childCount;

         //   Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                  //  Debug.Log("destroy rank card : " + i);
                    Destroy(LeaderBoardGoalParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeaderBoardGoalParentObject.childCount;

         //   Debug.Log("2. rank card count : " + childCount);

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
                //    Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(LeaderBoardGoalParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = LeaderBoardGoalParentObject.GetChild(i).gameObject;

                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.GoalBy = ParseData("goalFor", i);
                playerLeaderbData.Rank = i + 1;

                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out int res) ? res : 0;
                playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                SetBanner();
            }
        }

        public void AssignGoalConsidedBoard()
        {
            skip = 0;
            loadMoreFor = "goalAgainst";

            GoalConsidLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;
            int childCount = LeaderBoardGoalConcededParentObject.childCount;

          //  Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                  //  Debug.Log("destroy rank card : " + i);
                    Destroy(LeaderBoardGoalConcededParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeaderBoardGoalConcededParentObject.childCount;

          //  Debug.Log("2. rank card count : " + childCount);

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
                 //   Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(LeaderBoardGoalConcededParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = LeaderBoardGoalConcededParentObject.GetChild(i).gameObject;

                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.GoalConsidedLead = ParseData("goalAgainst", i);
                playerLeaderbData.Rank = i + 1;

                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out int res) ? res : 0;
                playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                SetBanner();
            }
        }
      
        public void AssignWinStreak()
        {
            skip = 0;
            loadMoreFor = "highestWinStreak";

            WinStreakLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;
            int childCount = LeaderBoardWinstrekParentObject.childCount;

          //  Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
            //        Debug.Log("destroy rank card : " + i);
                    Destroy(LeaderBoardWinstrekParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeaderBoardWinstrekParentObject.childCount;

          //  Debug.Log("2. rank card count : " + childCount);

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
               //     Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(LeaderBoardWinstrekParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = LeaderBoardWinstrekParentObject.GetChild(i).gameObject;

                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.WinLead = ParseData("highestWinStreak", i);
                playerLeaderbData.Rank = i + 1;

                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out int res) ? res : 0;
                playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                SetBanner();
            }
        }

        public void AssignCleanSheetBoard()
        {
            skip = 0;
            loadMoreFor = "cleanSheet";

            CleansheetLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;
            int childCount = LeaderBoardCleansheetParentObject.childCount;

        //    Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                 //   Debug.Log("destroy rank card : " + i);
                    Destroy(LeaderBoardCleansheetParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeaderBoardCleansheetParentObject.childCount;

          //  Debug.Log("2. rank card count : " + childCount);

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
                //    Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(LeaderBoardCleansheetParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = LeaderBoardCleansheetParentObject.GetChild(i).gameObject;

                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.CleansheetLead = ParseData("cleanSheet", i);
                playerLeaderbData.Rank = i + 1;

                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out int res) ? res : 0;
                playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                SetBanner();
            }
        }

        public void AssignAvgPPMBoard()
        {
            skip = 0;
            loadMoreFor = "avgPointsPerMinute";

            m_AveragePpmLeaderBoard.SetActive(true);

            SetColorAndSprite();

            int count = m_JSON["data"].Count;
            int childCount = m_AvgPPMParentObject.childCount;

          //  Debug.Log("1. rank card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
           //         Debug.Log("destroy rank card : " + i);
                    Destroy(m_AvgPPMParentObject.GetChild(i).gameObject);
                }
            }

            childCount = m_AvgPPMParentObject.childCount;

          //  Debug.Log("2. rank card count : " + childCount);

            for (int i = 0; i < count; i++)
            {
                GameObject instance;

                //check if we need to create a new object
                if (i >= childCount)
                {
                  //  Debug.Log("instantiate rank card : " + i);
                    instance = Instantiate(AllStatPrefab, Vector3.zero, Quaternion.identity) as GameObject;

                    instance.name = "Rank" + i;
                    instance.transform.SetParent(m_AvgPPMParentObject);
                    instance.transform.localScale = Vector3.one;

                    RectTransform rect = instance.GetComponent<RectTransform>();
                    rect.anchoredPosition3D = Vector3.zero;
                }
                else //use the object already made
                    instance = m_AvgPPMParentObject.GetChild(i).gameObject;

                playerLeaderbData = instance.GetComponent<Joga_LeaderBoardPlayer>();
                playerLeaderbData.PlayerName = ParseData("playerName", i);
                playerLeaderbData.AveragePointsPerMinute = ParseData("avgPointsPerMinute", i);
                //Set();
                playerLeaderbData.Rank = i + 1;

                playerLeaderbData.Round = int.TryParse(ParseData("leagueRound", i), out int res) ? res : 0;
                playerLeaderbData.Leg = int.TryParse(ParseData("leagueLeg", i), out res) ? res : 0;

                SetBanner();
            }
        }

        void Set()
        {
            float bestAvgPpm = 0;
            float worstAvgPpm = 0;

            for (int i = 0; i < m_AvgPPMParentObject.childCount; i++)
            {
                Joga_LeaderBoardPlayer instance = m_AvgPPMParentObject.GetChild(i).GetComponent<Joga_LeaderBoardPlayer>();

                if (!float.TryParse(instance.AveragePointsPerMinute, out float avgppm))
                    avgppm = 0;

                if (float.IsInfinity(avgppm) || float.IsNaN(avgppm))
                    avgppm = 0;

                if (avgppm > bestAvgPpm)
                {
                    instance.transform.SetAsFirstSibling();
                    bestAvgPpm = avgppm;
                }
                else if (avgppm < worstAvgPpm)
                {
                    instance.transform.SetAsLastSibling();
                    worstAvgPpm = avgppm;
                }
            }
        }

        public void LoadMore()
        {
            // skip = skip + 10;
            // WebserviceInstance.GetMVPLeaderboardList(loadMoreFore, skip, "10");
        }

        public void ResetAllLeaderBoardData()
        {
            for (int i = 0; i < LeaderBoardPointParentObject.childCount; i++)
                Destroy(LeaderBoardPointParentObject.GetChild(i).gameObject);
        }

        public void ButtonEvent(string Btnname)
        {
            switch (Btnname)
            {
                case "points":
                    HelpPopup.instance.ClosePop();
                    AssignLeaderBoardData();
                    break;

                case "Goal":
                    HelpPopup.instance.ClosePop();
                    AssignGoalBoard();
                    break;

                case "Conceded":
                    HelpPopup.instance.ClosePop();
                    AssignGoalConsidedBoard();
                    break;

                case "win":
                    HelpPopup.instance.ClosePop();
                    AssignWinStreak();
                    break;

                case "clean":
                    HelpPopup.instance.ClosePop();
                    AssignCleanSheetBoard();
                    break;

                case "Loadmore":
                    LoadMore();
                    break;

                case "SeasonLeaderboardBtn":
                    HelpPopup.instance.ClosePop();
                    AssignLeaderBoardData();
                    break;
            }
        }
    }
}