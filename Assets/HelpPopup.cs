using UnityEngine;
using UnityEngine.UI;

public class HelpPopup : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GameObject m_HelpPopup;
    [SerializeField] private GameObject[] m_ObsToDisableWhenEnabled;
    [SerializeField] private GameObject[] m_LeagueOnlyContent;
    [SerializeField] private GameObject[] m_NormalOnlyContent;
    [SerializeField] private Scrollbar m_HelpScrollBar;
    [SerializeField] private Button[] m_HelpCloseButtons;
    [SerializeField] private Text m_GameWinConditionsText;
    [SerializeField] private Text m_LeagueRulesText;
    [SerializeField] private Text m_BrandSponsorText;
#pragma warning restore CS0649
    // Bellow All are only related to top images on leaderboard
    public GameObject pImg, wImg, csImg, LImg, gfImg, gaImg, gdImg, ptsImg, hImg, ppmImg;
    public GameObject TotalMatch, WinMatch, CleanSheet, LoseMatch, GoalFor, GoalAgainst, GoalDiff, Points, Prize;
    public Transform RulesParent;
    public Text PrixeText, PointsInfoText;
    public float t = 0;

    public static HelpPopup instance;

    public void Awake()
    {
        if (!instance)
            instance = this;

        m_OriginalGameWinConditionsString = m_GameWinConditionsText.text;
        m_OriginalLeagueRulesString = m_LeagueRulesText.text;
        m_OriginalBrandSponsorString = m_BrandSponsorText.text;
    }

    private string m_OriginalGameWinConditionsString = "";
    private string m_OriginalLeagueRulesString = "";
    private string m_OriginalBrandSponsorString = "";
    public void Init(FastSkillTeam.FST_LeagueCard leaguebutton)
    {
        string s = m_OriginalGameWinConditionsString.Replace("#Legs", leaguebutton.TotalLegs.ToString());
        s = s.Replace("#GoalCount", leaguebutton.GoalsToWin.ToString());
        s = s.Replace("#Timeout", FST_MPConnection.Instance.BackgroundTimeout.ToString());
        m_GameWinConditionsText.text = s;
        s = m_OriginalLeagueRulesString.Replace("#Legs", leaguebutton.TotalLegs.ToString());
        m_LeagueRulesText.text = s;
        s = m_OriginalBrandSponsorString.Replace("#Brand", leaguebutton.BrandID);
        m_BrandSponsorText.text = s;

        PrixeText.enabled = true;
    }

    private bool isOneActive = false;
    public void PopUP(string name)
    {
        isOneActive = !isOneActive;

        bool mainHelpActive = name == "h" && !m_HelpPopup.activeSelf;


        if (!isOneActive)
        {
            ClosePop();

            m_HelpPopup.SetActive(mainHelpActive);

            if (mainHelpActive)
            {
                m_HelpScrollBar.value = 1;
                for (int i = 0; i < m_HelpCloseButtons.Length; i++)
                    m_HelpCloseButtons[i].onClick.AddListener(ClosePop);

                for (int i = 0; i < m_ObsToDisableWhenEnabled.Length; i++)
                    m_ObsToDisableWhenEnabled[i].SetActive(false);

                isOneActive = true;
            }

            return;
        }

        m_HelpPopup.SetActive(mainHelpActive);

        if (mainHelpActive)
        {
            m_HelpScrollBar.value = 1;
            for (int i = 0; i < m_HelpCloseButtons.Length; i++)
                m_HelpCloseButtons[i].onClick.AddListener(ClosePop);
        }

        for (int i = 0; i < m_ObsToDisableWhenEnabled.Length; i++)
            m_ObsToDisableWhenEnabled[i].SetActive(false);

        for (int i = 0; i < m_LeagueOnlyContent.Length; i++)
            m_LeagueOnlyContent[i].SetActive(!GameManager.Instance.IsOneNOneLeaderBoard);

        for (int i = 0; i < m_NormalOnlyContent.Length; i++)
            m_NormalOnlyContent[i].SetActive(GameManager.Instance.IsOneNOneLeaderBoard);

        pImg.SetActive(name == "p" && !pImg.activeSelf);
        wImg.SetActive(name == "w" && !wImg.activeSelf);
        csImg.SetActive(name == "cs" && !csImg.activeSelf);
        ptsImg.SetActive(name == "pts" && !ptsImg.activeSelf);
        LImg.SetActive(name == "L" && !LImg.activeSelf);
        gaImg.SetActive(name == "ga" && !gaImg.activeSelf);
        gfImg.SetActive(name == "gf" && !gfImg.activeSelf);
        gdImg.SetActive(name == "gd" && !gdImg.activeSelf);
        ppmImg.SetActive(name == "ppm" && !ppmImg.activeSelf);
    }

    public void ClosePop()
    {
        for (int i = 0; i < m_ObsToDisableWhenEnabled.Length; i++)
            m_ObsToDisableWhenEnabled[i].SetActive(true);

        pImg.SetActive(false);
        wImg.SetActive(false);
        csImg.SetActive(false);
        ptsImg.SetActive(false);
        LImg.SetActive(false);
        gaImg.SetActive(false);
        gfImg.SetActive(false);
        gdImg.SetActive(false);
        m_HelpPopup.SetActive(false);
        ppmImg.SetActive(false);

        isOneActive = false;

        for (int i = 0; i < m_HelpCloseButtons.Length; i++)
            m_HelpCloseButtons[i].onClick.RemoveListener(ClosePop);
    }
}
