using UnityEngine;
using UnityEngine.UI;
using Jiweman;
namespace FastSkillTeam {

    public class FST_LeagueCard : MonoBehaviour
    {
        public string BrandID { get; set; }
        public string LeagueName { get; set; }
        public string LeagueID { get; set; }
        public string LeagueInfo { get; set; }
        public string LeagueStatus { get; set; } = "";
        public string LeagueType { get; set; }
        public int StadiumID { get; set; } = 0;
        public string Start_Date { get; set; }
        public string End_Date { get; set; }
        public string EntryFee { get; set; }
        public string LocalEntryFee { get; set; }
        public string Prize { get; set; }

        private string m_Currency = "USD";
        public string Currency { get { if (m_Currency == "USD") return m_Currency + " $"; return m_Currency + " "; } set { m_Currency = value; } }

        public Sprite FrontImageToSet { get; set; }
        public Sprite BackImageToSet { get; set; }

        public string LeagueImageUrl { get; set; }

        public Text leaguenameTxt, leagueinfo, prizeText, startdate, enddate, onlineuser, EntryFeeText;
        public Text backStartDate, backEndDate, onlinePlayer;
        public Button info;
        public GameObject frontsideInfo, backsideInfo, startDatefield, endDateField;

        public Text statusText;
        public GameObject StatusLeague;
        public Image Closed;
        public Image Upcoming, ClosedImage;
        public Transform rulesParent;
        public GameObject RulesPrefab;

        private GameObject Rules;

        public static FST_LeagueCard instance;

        public int GoalsToWin { get; private set; } = 3;

        private Button m_Button = null;


#pragma warning disable CS0649
        //[Header("Hyperlinks")]
        //[SerializeField] private Button[] m_Hyperlinks;

        [SerializeField] private Button m_LeaderboardButton;

        [Header("MVP Unlocked Objects")]
        [SerializeField] private GameObject[] m_ActiveWhenHasTicket;
        [SerializeField] private GameObject[] m_ForceCloseWhenHasTicket;
        [SerializeField] private Button m_MVPJoinNowButton;
        [SerializeField] private GameObject m_MVPLockedNoTickets;

        [Header("MVP Pre Game Popup")]
        [SerializeField] private GameObject m_MVPPreGamePopup;
        [SerializeField] private Text m_MVPPreGameText;
        [SerializeField] private Button m_MVPYesPaidButton;
        [SerializeField] private Button m_MVPNotPaidButton;
        [SerializeField] private Button m_MVPClosePreGameButton;

        [Header("MVP Ticket Fail Popup")]
        [SerializeField] private GameObject m_MVPTicketFailPopup;
        //[SerializeField] private Button m_MVPTopupCashButton;
        //  [SerializeField] private Button m_MVPGoToNonLeaderboardPlayButton;
        [SerializeField] private Button m_MVPCloseTicketFailButton;

        [Header("MVP Info Link Popup")]
        [SerializeField] private GameObject m_InfoLinkPopup;
        [SerializeField] private Button m_CloseInfoLinkButton;

        [Header("Terms And Agreements Popup")]
        [SerializeField] private GameObject m_TermsAndAgreementsPopup;
        [SerializeField] private Text m_TermsAndAgreementsText;
        [SerializeField] private Button m_TermsAndAgreementsAcceptButton;
        [SerializeField] private Button m_TermsAndAgreementsCloseButton;

        [Header("Pre Game Popup")]
        [SerializeField] private GameObject m_PreGamePopup;
        [SerializeField] private Button m_YesToLeaderboardPlayButton;
        [SerializeField] private Button m_NoToLeaderboardPlayButton;
        [SerializeField] private Button m_ClosePreGameButton;

        [Header("Ticket Fail Popup")]
        [SerializeField] private GameObject m_TicketFailPopup;
        [SerializeField] private Button m_TopupCashButton;
        [SerializeField] private Button m_GoToNonLeaderboardPlayButton;
        [SerializeField] private Button m_CloseTicketFailButton;

        [Header("Ticket Spend Confirm Popup")]
        [SerializeField] private GameObject m_TicketSpendConfirmPopup;
        [SerializeField] private Button m_ConfirmSpendButton;
        [SerializeField] private Button m_DeclineSpendButton;
        [SerializeField] private Button m_CloseSpendConfirmPopupButton;

        [Header("Wager Input")]
        [SerializeField] private GameObject m_WagerInputPopup;
        [SerializeField] private InputField m_WagerInputField;
        [SerializeField] private Button m_MoreSpendButton;
        [SerializeField] private Button m_LessSpendButton;
        [SerializeField] private Button m_WagerApplyButton;
        [SerializeField] private Button m_CloseWagerPopupButton;
#pragma warning restore CS0649

        //cached for extra purpose, less cost
        private JSONArray rulesSet = null;
        private bool m_HasTicket = false;
        private bool available = false;
        private int curWager = 1;

        private void ShowTermsAndAgreements(bool active)
        {
            m_TermsAndAgreementsPopup.SetActive(active);
        }
        private string m_OriginalTermsString = "";
        public void Awake()
        {
            m_OriginalTermsString = m_TermsAndAgreementsText.text;
            instance = this;
        }
        private Button[] allButtons = new Button[0];
        private void OnEnable()
        {
            FST_AppHandlerControl.Instance.AddBlocker(m_TermsAndAgreementsPopup);

            if (allButtons.Length < 1)
                allButtons = GetComponentsInChildren<Button>(true);

            //   for (int i = 0; i < allButtons.Length - 1; i++)
            //      allButtons[i].onClick.AddListener(() => GameManager.Instance.AnimatePressed(allButtons[i].transform, 1, () => Debug.Log("PRESSED")));

            if (!m_Button)
                m_Button = GetComponent<Button>();

            m_Button.onClick.AddListener(() => OnClickLeaguePlay());
            m_YesToLeaderboardPlayButton.onClick.AddListener(() => OnClickYesToLeaderBoardPlay());
            m_NoToLeaderboardPlayButton.onClick.AddListener(() => OnClickNoToLeaderBoardPlay());
            m_TopupCashButton.onClick.AddListener(() => OnClickTopupCash());
            m_GoToNonLeaderboardPlayButton.onClick.AddListener(() => OnClickNoToLeaderBoardPlay());
            m_ClosePreGameButton.onClick.AddListener(() => ShowPreGamePopup(false));
            m_CloseTicketFailButton.onClick.AddListener(() => OnClickCloseTicketFail());
            m_ConfirmSpendButton.onClick.AddListener(() => OnClickYesToSpendTicket());
            m_DeclineSpendButton.onClick.AddListener(() => OnClickNoToSpendTicket());
            m_CloseSpendConfirmPopupButton.onClick.AddListener(() => OnClickNoToSpendTicket());
            m_CloseWagerPopupButton.onClick.AddListener(() => OnClickCloseWager());
            m_MoreSpendButton.onClick.AddListener(() => OnClickIncreaseWager());
            m_LessSpendButton.onClick.AddListener(() => OnClickDecreaseWager());
            m_WagerApplyButton.onClick.AddListener(() => OnClickApplyWager());
            m_WagerInputField.onEndEdit.AddListener((string s) => UpdateWagerInputText(s));

            m_MVPYesPaidButton.onClick.AddListener(() => OnClickYesIPaid());
            m_MVPNotPaidButton.onClick.AddListener(() => OnClickNoIDidntPay());
            m_MVPClosePreGameButton.onClick.AddListener(() => ShowPreGamePopup(false));

            m_MVPCloseTicketFailButton.onClick.AddListener(() => OnClickCloseTicketFail());
            m_MVPJoinNowButton.onClick.AddListener(() => Joga_NetworkManager.Instance.GetLeagueAvailabilityRequest(this));//stupidly, this will only work on a purchased league else we could have a constant update...;

            m_TermsAndAgreementsAcceptButton.onClick.AddListener(() => OnClickAcceptTermsAndConditions());
            m_TermsAndAgreementsCloseButton.onClick.AddListener(() => ShowTermsAndAgreements(false));

            m_CloseInfoLinkButton.onClick.AddListener(() => ShowInfoLink(false));

            m_LeaderboardButton.onClick.AddListener(() => ShowLeaderboard());

        }

        private void OnDisable()
        {
            //    for (int i = 0; i < allButtons.Length; i++)
            //       allButtons[i].onClick.RemoveAllListeners();
            m_Button.onClick.RemoveAllListeners();
            m_YesToLeaderboardPlayButton.onClick.RemoveAllListeners();
            m_NoToLeaderboardPlayButton.onClick.RemoveAllListeners();
            m_TopupCashButton.onClick.RemoveAllListeners();
            m_GoToNonLeaderboardPlayButton.onClick.RemoveAllListeners();
            m_ClosePreGameButton.onClick.RemoveAllListeners();
            m_CloseTicketFailButton.onClick.RemoveAllListeners();
            m_ConfirmSpendButton.onClick.RemoveAllListeners();
            m_DeclineSpendButton.onClick.RemoveAllListeners();
            m_CloseSpendConfirmPopupButton.onClick.RemoveAllListeners();
            m_CloseWagerPopupButton.onClick.RemoveAllListeners();
            m_MoreSpendButton.onClick.RemoveAllListeners();
            m_LessSpendButton.onClick.RemoveAllListeners();
            m_WagerApplyButton.onClick.RemoveAllListeners();
            m_WagerInputField.onEndEdit.RemoveAllListeners();

            m_MVPYesPaidButton.onClick.RemoveAllListeners();
            m_MVPNotPaidButton.onClick.RemoveAllListeners();
            m_MVPClosePreGameButton.onClick.RemoveAllListeners();

            m_MVPCloseTicketFailButton.onClick.RemoveAllListeners();
            m_MVPJoinNowButton.onClick.RemoveAllListeners();

            m_TermsAndAgreementsAcceptButton.onClick.RemoveAllListeners();
            m_TermsAndAgreementsCloseButton.onClick.RemoveAllListeners();

            m_CloseInfoLinkButton.onClick.RemoveAllListeners();

            m_LeaderboardButton.onClick.RemoveAllListeners();

            m_TermsAndAgreementsText.text = m_OriginalTermsString;
        }

        public void Init(JSONArray prizes, JSONArray leagueRulesInfo, JSONArray allowedCountries, bool hasTickets, int totalLegs)
        {
            frontsideInfo.SetActive(true);
            backsideInfo.SetActive(false);

            //leaguenameTxt.text = LeagueName;
            Debug.Log(LeagueName + ": LeagueStatus = " + LeagueStatus);

            if (LeagueStatus == "active")
            {
                StatusLeague.SetActive(false);
                endDateField.SetActive(false);
                startDatefield.GetComponent<Text>().text = "Ending On :";
                startdate.text = End_Date;
                available = true;
                m_LeaderboardButton.gameObject.SetActive(true);
            }
            else if (LeagueStatus == "closed" || LeagueStatus == "ended")
            {
                startDatefield.SetActive(false);
                endDateField.GetComponent<Text>().text = "Ended On:";
                enddate.text = End_Date;
                available = false;
                //it may be over, but we should still have access to the leaderboard!
                m_LeaderboardButton.gameObject.SetActive(true);
            }
            else
            {
                startDatefield.SetActive(false);
                endDateField.GetComponent<Text>().text = "Starting On :";
                enddate.text = Start_Date;
                available = false;
                //upcoming leagues will have empty leaderboard and rules data! disable the button
                m_LeaderboardButton.gameObject.SetActive(false);
            }

            if (FrontImageToSet != null)
                frontsideInfo.GetComponent<Image>().sprite = FrontImageToSet;

            if (BackImageToSet != null)
                backsideInfo.GetComponent<Image>().sprite = BackImageToSet;

            //  onlinePlayer.text = Photon.Pun.PhotonNetwork.CountOfPlayers.ToString();
            backStartDate.text = Start_Date;
            backEndDate.text = End_Date;

            int prizeCount = prizes.Count;

            string s = "";

            //do remaining time calc here

            if (prizeCount > 0)
            {
                s = /*"First Place : " + */prizes[0];

                /*  if (prizeCount > 1)
                      s += " / Second Place : " + prizes[1];

                  if (prizeCount > 2)
                      s += " / Third Place : " + prizes[2];*/
            }

            prizeText.text = s;

            bool isLocalOnlyLeague = allowedCountries.Count < 2;

            if (!isLocalOnlyLeague)
                Currency = "USD";

            //   Debug.Log("Currency = " + Currency);

            //cache the rules data, we need it for other things like leaderboard
            rulesSet = leagueRulesInfo;

            SetRulesInfo(false);

            if (hasTickets)
                m_HasTicket = true;

            //   Debug.Log("HAS TICKETS = " + m_HasTicket);

            bool isFreeLeague = true;
            if (int.TryParse(EntryFee, out int result))
                if (result > 0)
                    isFreeLeague = false;

            if (int.TryParse(LocalEntryFee, out result))
                if (result > 0)
                    isFreeLeague = false;

            EntryFeeText.text = isFreeLeague ? "FREE" : Currency + (isLocalOnlyLeague ? LocalEntryFee : EntryFee);


            TotalLegs = totalLegs;
            m_MVPYesPaidButton.GetComponentInChildren<Text>().text = isFreeLeague ? "YES" : "YES, I PAID";
            m_MVPNotPaidButton.GetComponentInChildren<Text>().text = isFreeLeague ? "NO" : "NO, I HAVEN'T PAYED";
            m_MVPPreGameText.text = isFreeLeague ? "HAVE YOU ACTIVATED THIS FREE LEAGUE AND ITS STILL LOCKED?" : "HAVE YOU PAYED FOR THIS LEAGUE AND ITS STILL LOCKED?";

        }
        private void SetRulesInfo(bool isForLeaderboardHelp)
        {
            int rulesCount = rulesSet.Count;

            Transform p = isForLeaderboardHelp ? HelpPopup.instance.RulesParent : rulesParent;

            int childCount = p.childCount;
            //  Debug.Log("1. rules card count : " + childCount);
            if (childCount > rulesCount)
            {
                for (int i = rulesCount; i < childCount; i++)
                {
                    //   Debug.Log("destroy league card : " + i);
                    Destroy(p.GetChild(i).gameObject);
                }
            }

            childCount = p.childCount;

            //   Debug.Log("2. rules card count : " + childCount);

            for (int i = 0; i < rulesCount; i++)
            {
                GameObject rulesPrefab;

                if (i >= childCount)
                {
                    // Debug.Log("instantiate rules card : " + i);
                    rulesPrefab = Instantiate(RulesPrefab, transform.position, Quaternion.identity);
                    rulesPrefab.transform.SetParent(p);
                    rulesPrefab.transform.position = transform.parent.GetComponent<Transform>().position;
                    rulesPrefab.transform.localScale = Vector3.one;
                }
                else rulesPrefab = p.GetChild(i).gameObject;

                string info = rulesSet[i].ToString();
                //    Debug.Log("info = " + info);
                if (info.ToLower().Contains("goals to win"))
                {
                    int goals = int.Parse(info[1].ToString());
                    GoalsToWin = goals;

                    m_TermsAndAgreementsText.text = m_TermsAndAgreementsText.text.Replace("#GoalCount", goals.ToString());
                    //  Debug.Log("GOALS TO WIN = " + goals);
                }

                rulesPrefab.GetComponent<Text>().text = rulesSet[i].ToString();
            }
        }

        private void SetPurchasedStatus()
        {
            bool hasTicket = HasTicket();
            for (int i = 0; i < m_ActiveWhenHasTicket.Length; i++)
            {
                if (m_ActiveWhenHasTicket[i].activeSelf != hasTicket)
                    m_ActiveWhenHasTicket[i].SetActive(hasTicket);
            }

            if (hasTicket)
            {
                for (int i = 0; i < m_ForceCloseWhenHasTicket.Length; i++)
                {
                    if (m_ForceCloseWhenHasTicket[i].activeSelf)
                        m_ForceCloseWhenHasTicket[i].SetActive(false);
                }
            }

            bool activeNoTickets = available && !hasTicket && !m_MVPPreGamePopup.activeSelf && !m_MVPTicketFailPopup.activeSelf;
            if (m_Button.interactable != activeNoTickets)
                m_Button.interactable = activeNoTickets;
            if (m_MVPLockedNoTickets.activeSelf != activeNoTickets)
                m_MVPLockedNoTickets.SetActive(activeNoTickets);
        }

        private bool HasTicket()
        {
            return m_HasTicket && available; /*|| FST_SettingsManager.LeagueTickets > 0*/;//uncomment for debug tests with custom editor
        }

        private float nextUpdateTime = 0;
        void Update()
        {
            if (nextUpdateTime > Time.time)
                return;

            nextUpdateTime = Time.time + 1f;

            onlinePlayer.text = Photon.Pun.PhotonNetwork.CountOfPlayers.ToString();

            SetPurchasedStatus();
            //   double time = Photon.Pun.PhotonNetwork.Time;
            //   Debug.Log("pun time : " + time + " ------ remaining time : " + (double.Parse(End_Date) - time));

        }
        public int TotalLegs { get; private set; } = 20;
        public int CurrentRound { get; private set; } = 1;
        public int RemainingLegs { get; private set; } = 0;
        public int LeagueRank { get; private set; } = 0;
        public int LeagueRankCount { get; private set; } = 0;
        public void UpdateLeagueAvailability(JSONNode jsonData)
        {
            CurrentRound = jsonData["data"]["round"].AsInt;
            RemainingLegs = jsonData["data"]["remaining"].AsInt;
            TotalLegs = jsonData["data"]["totalAllowed"].AsInt;
            LeagueRank = jsonData["data"]["userRank"].AsInt;
            LeagueRankCount = jsonData["data"]["totalRankCount"];

            m_TermsAndAgreementsText.text = m_TermsAndAgreementsText.text.Replace("#Legs", TotalLegs.ToString());
            m_TermsAndAgreementsText.text = m_TermsAndAgreementsText.text.Replace("#Timeout", FST_MPConnection.Instance.BackgroundTimeout.ToString());

            Debug.Log("CurrentRound = " + CurrentRound + ", RemainingLegs = " + RemainingLegs + ", TotalLegs = " + TotalLegs);

            if (RemainingLegs <= 0)
            {
                m_HasTicket = false;
                FST_League_Handler.Instance.ShowLeagueProgressionPanel(true, this);
            }
            else //if (!FST_AppHandler.IsSendingLeaguesRequest)
                ShowTermsAndAgreements(true);

            FST_AppHandler.IsSendingLeaguesRequest = false;
        }

        public void InfoToBack()
        {
            frontsideInfo.SetActive(false);
            backsideInfo.SetActive(true);
        }
        public void CloseInfo()
        {
            frontsideInfo.SetActive(true);
            backsideInfo.SetActive(false);
        }

        private void OnClickYesIPaid()
        {
            Debug.Log("LeagueID ==" + LeagueID);
            UIController.Instance.ActiveLoading2Panel();
            FST_League_Handler.Instance.CallLeague();
            //  ShowUserTicketFail(true);
        }

        private void OnClickNoIDidntPay() => ShowInfoLink(true);

        private void ShowInfoLink(bool active) => m_InfoLinkPopup.SetActive(active);

        private void OnClickTopupCash()
        {
            Debug.Log("On Click Topup cash");
            //  FST_SettingsManager.LeagueTickets++;
        }

        private void OnClickLeaguePlay() => ShowPreGamePopup(true);

        private void OnClickYesToLeaderBoardPlay()
        {
            if (HasTicket())
                ShowTicketSpendConfirmPopup(true);
            else ShowUserTicketFail(true);
        }

        private void OnClickNoToLeaderBoardPlay()
        {
            Debug.Log("OnClickNoToLeaderBoardPlay() > NOT YET IMPLEMENTED!");
            ShowPreGamePopup(false);
            ShowUserTicketFail(false);
        }

        private void OnClickCloseTicketFail()
        {
            ShowUserTicketFail(false);
            ShowPreGamePopup(true);
        }

        private void ShowPreGamePopup(bool active)
        {
            if (active)
                OnAction();

            m_MVPPreGamePopup.SetActive(active);
            //  m_PreGamePopup.SetActive(active);

            nextUpdateTime = 0;
        }

        private void ShowTicketSpendConfirmPopup(bool active)
        {
            m_TicketSpendConfirmPopup.SetActive(active);
        }

        private void OnClickYesToSpendTicket() => ShowWagerEntry(true);

        private void ShowWagerEntry(bool active) => m_WagerInputPopup.SetActive(active);

        private void OnClickCloseWager() => ShowWagerEntry(false);

        private void OnClickApplyWager()
        {
            Debug.Log("APPLY WAGER OF : " + curWager.ToString());
            EnterGamePlayPhase();
        }

        private void OnClickNoToSpendTicket()
        {
            ShowTicketSpendConfirmPopup(false);
            ShowPreGamePopup(true);
        }

        private void ShowUserTicketFail(bool active)
        {
            if (active)
                ShowPreGamePopup(false);
            //m_TicketFailPopup.SetActive(active);
            m_MVPTicketFailPopup.SetActive(active);
        }

        private void OnClickDecreaseWager()
        {
            if (curWager > 1)
                curWager--;
            UpdateWagerInputText();
        }

        private void OnClickIncreaseWager()
        {
            curWager++;
            UpdateWagerInputText();
        }

        private void UpdateWagerInputText(string input = "")
        {
            if (input != "" && int.TryParse(input, out int result))
            {
                if (result > 0)
                    result = 1;

                curWager = result;
            }

            m_WagerInputField.text = curWager.ToString();
        }

        private void OnClickAcceptTermsAndConditions()
        {
            FST_League_Handler.Instance.ShowLeagueProgressionPanel(true, this);
            ShowTermsAndAgreements(false);
        }

        public void EnterGamePlayPhase()
        {
            Debug.Log("EnterGamePlayPhase > LeagueID==" + LeagueID);
            //  FST_MPConnection.Instance.TryJoinRoom(null, LeagueID);
            GameManager.CurrentLeagueID = LeagueID;
            GameManager.SelectedStadium = StadiumID;
            GameManager.Instance.GoalsToWin = GoalsToWin;

            GameManager.Instance.MainMenuEvents("subleague");

            if (RemainingLegs <= 1)
            {
                Debug.Log("LAST LEG OF LEAGUE!");
                FST_SettingsManager.LastFinishedLeagueID = LeagueID;
            }
        }

        public void ShowLeaderboard()
        {
            if (HelpPopup.instance)
                HelpPopup.instance.ClosePop();
            Debug.Log("ID==" + LeagueID);
            Joga_LeaderBoard.ShowLeaderBoard(this, LeagueID);
            SetRulesInfo(true);
        }

        private void OnAction()
        {
            FST_LeagueCard[] b = transform.parent.GetComponentsInChildren<FST_LeagueCard>();

            for (int i = 0; i < b.Length; i++)
                if (b[i] != this)
                    b[i].Refresh();
        }

        public void Refresh()
        {
            if (m_MVPPreGamePopup.activeSelf)
                m_MVPPreGamePopup.SetActive(false);
            if (m_MVPTicketFailPopup.activeSelf)
                m_MVPTicketFailPopup.SetActive(false);
        }
    }
}