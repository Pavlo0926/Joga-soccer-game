using UnityEngine;
using Jiweman;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using System;
namespace FastSkillTeam
{
    public class FST_League_Handler : MonoBehaviour
    {
        public static FST_League_Handler Instance { get; private set; }

        public GameObject ArrowIndicator;
        public Transform LeagueParentObject;
        public GameObject LeagueCard;//prefab
#pragma warning disable CS0649
        [SerializeField] private Sprite m_CardDownloadingPlaceHolderImage;
        [SerializeField] private Sprite[] m_LeagueImages = new Sprite[4];
        [Header("League Progression Panel")]
        [SerializeField] private GameObject m_LeagueProgressionPanel;
        [SerializeField] private GameObject m_StartPanel;//not yet used, TODO: hook up when hard coding buttons for final ui here
        [SerializeField] private GameObject m_IntermissionPanel;//not yet used, TODO: hook up when hard coding buttons for final ui here
        [SerializeField] private GameObject m_LeagueResultPanel;
        [SerializeField] private Button m_PlayButton;
        [SerializeField] private Text m_WelcomeText;
        [SerializeField] private Text m_LeagueRoundsRemainingText;
        [SerializeField] private Text m_LeagueRankText;
        [SerializeField] private Text m_LeagueGoalsToWinText;
        [SerializeField] private Text m_LeagueResultText;
        [SerializeField] private Text m_AnotherGoText;
        [SerializeField] private Text m_AnotherGoButtonText;
#pragma warning restore CS0649
        JSONNode jSON = null;
        private string originalLeagueResultText = "";
        private string originalAnotherGoText = "";
        private string m_OriginalWelcomeText = "";
        #region MonoBehaviour Callbacks
        void Awake()
        {
            originalLeagueResultText = m_LeagueResultText.text;
            originalAnotherGoText = m_AnotherGoText.text;
            m_OriginalWelcomeText = m_WelcomeText.text;
            for (int i = 0; i < LeagueParentObject.childCount; i++)
                Destroy(LeagueParentObject.GetChild(i).gameObject);

            Instance = this;
            CheckTickets();
        }

        #endregion

        #region Progression Panel

        public void OnClickHome_FromFinalResult()
        {
            m_LeagueResultPanel.SetActive(false);
            m_LeagueProgressionPanel.SetActive(false);
        }

        public void OnClickBack(bool startPanel)
        {
            if (startPanel)
            {
                m_LeagueProgressionPanel.SetActive(false);
            }
            else
            {
                m_IntermissionPanel.SetActive(false);
                m_StartPanel.SetActive(true);
            }
        }

        public void OnClickNext_FromStartPanel()
        {
            m_StartPanel.SetActive(false);
            m_IntermissionPanel.SetActive(true);
        }

        public void ShowLeagueProgressionPanel(bool active, FST_LeagueCard leaguebutton = null)
        {
            if (leaguebutton)
            {
                if (leaguebutton.RemainingLegs < 1)
                {
                    SetLeagueResultText(leaguebutton);
                    return;
                }

                m_PlayButton.onClick.RemoveAllListeners();
                m_PlayButton.onClick.AddListener(() => leaguebutton.EnterGamePlayPhase());

                m_WelcomeText.text = m_OriginalWelcomeText.Replace("#Player", FastSkillTeam.FST_SettingsManager.PlayerName);
                m_WelcomeText.text = m_WelcomeText.text.Replace("#LeagueName", leaguebutton.LeagueName);

                m_LeagueRoundsRemainingText.text = m_LeagueRoundsRemainingText.text.Replace("#1", leaguebutton.CurrentRound.ToString());
                m_LeagueRoundsRemainingText.text = m_LeagueRoundsRemainingText.text.Replace("#2", (leaguebutton.TotalLegs - leaguebutton.RemainingLegs + 1).ToString());
                m_LeagueRoundsRemainingText.text = m_LeagueRoundsRemainingText.text.Replace("#3", leaguebutton.TotalLegs.ToString());

                m_LeagueRankText.text = m_LeagueRankText.text.Replace("#1", leaguebutton.LeagueRank.ToString());
                m_LeagueRankText.text = m_LeagueRankText.text.Replace("#2", leaguebutton.LeagueRankCount.ToString());

                m_LeagueGoalsToWinText.text = m_LeagueGoalsToWinText.text.Replace("#1", leaguebutton.GoalsToWin.ToString());
            }
            m_LeagueProgressionPanel.SetActive(active);
        }

        private void SetLeagueResultText(FST_LeagueCard leaguebutton)
        {
            string s = originalLeagueResultText.Replace("#1", leaguebutton.TotalLegs.ToString());
            s = s.Replace("#2", leaguebutton.CurrentRound.ToString());
            s = s.Replace("#3", leaguebutton.LeagueRank.ToString());
            s = s.Replace("#4", leaguebutton.LeagueRankCount.ToString());

            m_LeagueResultText.text = s;

            s = originalAnotherGoText.Replace("#1", (leaguebutton.CurrentRound + 1).ToString());
            s = s.Replace("#2", leaguebutton.CurrentRound.ToString());

            m_AnotherGoText.text = s;

            bool isFreeLeague = true;
            if (int.TryParse(leaguebutton.EntryFee, out int result))
                if (result > 0)
                    isFreeLeague = false;

            if (int.TryParse(leaguebutton.LocalEntryFee, out result))
                if (result > 0)
                    isFreeLeague = false;

            s = isFreeLeague ? "ACTIVATE FREE TICKET TO START NEW ROUND" : "BUY ANOTHER TICKET TO START NEW ROUND";

            m_AnotherGoButtonText.text = s;

            m_LeagueResultPanel.SetActive(true);
            m_LeagueProgressionPanel.SetActive(true);

            FastSkillTeam.FST_SettingsManager.LastFinishedLeagueID = "";
        }
        #endregion

        public void CheckTickets()
        {
            ArrowIndicator.SetActive(true);
        }

        public void CallLeague()
        {
            if (LeagueParentObject.childCount < 1)
                UIController.Instance.ActiveLoading2Panel();
            Joga_NetworkManager.Instance.GetLeaguesRequest();
        }

        public void SetLeagueData(JSONNode jsonData)
        {
            jSON = jsonData;

            int count = jSON["data"].Count;

            //For cleaner debug values
            //for (int i = 0; i < count; i++)
            //{
            //    Debug.Log(jSON["data"][i]);
            //}

            int childCount = LeagueParentObject.childCount;
            //  Debug.Log("1. league card count : " + childCount);
            if (childCount > count)
            {
                for (int i = count; i < childCount; i++)
                {
                    //   Debug.Log("destroy league card : " + i);
                    Destroy(LeagueParentObject.GetChild(i).gameObject);
                }
            }

            childCount = LeagueParentObject.childCount;

            //   Debug.Log("2. league card count : " + childCount);

            string checkForLastFinishedLeagueID = FastSkillTeam.FST_SettingsManager.LastFinishedLeagueID;

            for (int i = 0; i < count; i++)
            {
                GameObject league;

                if (i >= childCount)
                {
                    // Debug.Log("instantiate league card : " + i);
                    league = Instantiate(LeagueCard, transform.position, Quaternion.identity);
                    league.transform.SetParent(LeagueParentObject);
                    league.transform.localScale = Vector3.one;
                }
                else league = LeagueParentObject.GetChild(i).gameObject;

                FST_LeagueCard b = league.GetComponent<FST_LeagueCard>();

                b.LeagueName = ParseData("leagueName", i);
                b.LeagueType = ParseData("leagueType", i);

                string leagueStatus = ParseData("leagueStatus", i);

                b.LeagueStatus = leagueStatus;

                if (leagueStatus == "closed" || leagueStatus == "ended")
                {
                    // b.StatusLeague.SetActive(false);
                    b.Closed.gameObject.SetActive(true);
                    b.ClosedImage.gameObject.SetActive(false);
                    //b.statusText.text = "Closed";
                }
                else if (leagueStatus == "upcoming")
                {
                    // b.StatusLeague.SetActive(true);
                    b.Upcoming.gameObject.SetActive(true);
                    b.ClosedImage.gameObject.SetActive(true);
                    //b.statusText.text = "Upcoming";
                }

                b.Start_Date = ParseData("startDate", i).Substring(0, 10);
                b.End_Date = ParseData("endDate", i).Substring(0, 10);
                b.BrandID = ParseData("brandId", i);
                b.LeagueID = ParseData("_id", i);


                bool hasTicket = jSON["data"][i]["leagueEnable"].AsBool;

                if (b.LeagueID == checkForLastFinishedLeagueID)
                {
                    Joga_NetworkManager.Instance.GetLeagueAvailabilityRequest(b);
                    if (hasTicket)
                    {
                        Debug.LogWarning("GETLEAGUESREQUEST API SAYS WE HAVE A TICKET, BUT WE DONT!!!, WE COMPLETED ALL LEGS AND THIS IS STORED LOCALLY! WE SHOULD NOT NEED TO CALL GetLeagueAvailabilityRequest TO UPDATE THIS VALUE WHEN WE OPEN THIS SCREEN!!!!!!!");
                        //  hasTicket = false;
                    }
                }

                b.Currency = ParseData("localCurrency", i);
                b.EntryFee = ParseData("entryFee", i);
                b.LocalEntryFee = ParseData("localEntryFee", i);

                b.LeagueImageUrl = ParseData("leagueCardImageUrl", i);

                string[] filennamefilter = b.LeagueImageUrl.Split('/');
                string fileNameRaw = filennamefilter[filennamefilter.Length - 1];//including type (eg. "image.png")
                string fileName = fileNameRaw.Split('.')[0];//just the name without the type (eg. "image")

                if (!LoadFromDisk(fileNameRaw, texture => b.FrontImageToSet = texture.ToSprite()))
                {
                    Sprite s = Resources.Load<Texture2D>("LeagueCards/" + fileName)?.ToSprite();
                    b.FrontImageToSet = s;
                }

                if (!b.FrontImageToSet)
                {
                    Debug.Log("Partial Update Required! download is requested for: " + fileNameRaw);

                    if (m_CardDownloadingPlaceHolderImage)
                        b.FrontImageToSet = m_CardDownloadingPlaceHolderImage;
                    FST_UpdateManager.PartialUpdate(b.LeagueImageUrl, "Resources/LeagueCards/" + fileNameRaw);
                    FST_MPConnection.Instance.StartCoroutine(DownloadImage(b, fileName, fileNameRaw));
                }

                //use the int from card name for stadium id for now...
                if (fileNameRaw.ToLower().Contains("card"))
                {
                    if (int.TryParse(fileNameRaw[fileNameRaw.Length - 5].ToString(), out int stadiumId))
                    {
                        b.StadiumID = stadiumId;
                        //   Debug.Log("STADIUM ID FROM URL IS CORRECT FOR : " + b.LeagueImageUrl);
                    }
                }
                else b.StadiumID = 4;  //default if cardname is bad...

                if (!b.FrontImageToSet)
                { //set a backed up default
                    Debug.LogWarning("SETTING DEFAULT STADIUM ID FOR: " + b.LeagueName + ", ID:" + (b.StadiumID - 1));
                    b.FrontImageToSet = m_LeagueImages[b.StadiumID - 1];
                }

                b.Init(jSON["data"][i]["prize"].AsArray, jSON["data"][i]["leagueInfo"].AsArray, jSON["data"][i]["allowedCountries"].AsArray, hasTicket, jsonData["data"][i]["gameCount"].AsInt);
            }
            UIController.Instance.DeactiveLoading2Panel(0.1f);
        }

        private IEnumerator DownloadImage(FST_LeagueCard b, string fileName, string fileNameRaw)
        {
            yield return new WaitUntil(() => FST_UpdateManager.IsDone);

            if (!b)//users may have exited by now or clicked on another league card
                yield break;

            if (!LoadFromDisk(fileNameRaw, texture => b.frontsideInfo.GetComponent<Image>().sprite = texture.ToSprite()))
            {
                Debug.Log("Reverting to PROJECT RESOURCES!");
                Sprite s = Resources.Load<Texture2D>("LeagueCards/" + fileName)?.ToSprite();
                if (s)
                    b.frontsideInfo.GetComponent<Image>().sprite = s;
            }
        }

        public string ParseData(string objectName, int index)
        {
            bool isString = jSON["data"][index][objectName].IsString;
            string value = isString ? jSON["data"][index][objectName].Value : jSON["data"][index][objectName].AsDouble.ToString();

            return value;
        }

        private bool LoadFromDisk(string filename, Action<Texture2D> result = null)
        {
#if UNITY_EDITOR
            string directory = Application.dataPath + "/Resources/LeagueCards/" + filename;
#else
        string directory = Application.persistentDataPath + "/Resources/LeagueCards/" + filename;
#endif
            if (!File.Exists(directory))
            {
                Debug.Log("File not found: " + directory);
                return false;
            }

            var retVal = new Texture2D(0, 0, TextureFormat.RGBA32, false);
            retVal.LoadImage(File.ReadAllBytes(directory));
            result?.Invoke(retVal);

            Debug.Log("File found and set : " + directory);
            return true;
        }
    }
}