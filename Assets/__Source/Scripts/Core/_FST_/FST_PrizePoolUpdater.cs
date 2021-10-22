using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FST_PrizePoolUpdater : MonoBehaviour
{
#pragma warning disable CS0649
    [Tooltip("0 = only on enable, else every this many seconds it will auto update")]
    [SerializeField] private float m_RefreshRate = 0f;// in seconds
    [SerializeField] private Text m_DisplayText;
    [SerializeField] private bool m_IsInGame = false;
#pragma warning restore CS0649
    private static string m_Currency = "USD";
    public static string Currency { get { if (m_Currency == "USD") return m_Currency + " $"; return m_Currency + " "; } set { m_Currency = value; } }
    public static int Amount { get; set; } = -10;

    private static Text s_Text = null;
    private static bool isLeague = false;
    private static bool isInGame = false;

    public static FST_PrizePoolUpdater Instance { get; private set; } = null;
    private void OnEnable()
    {
        Instance = this;

        if (!m_DisplayText)
        {
            Debug.LogError("FST_PrizePoolUpdater.cs > No display text has been set in inspector for prize pool updates");
            return;
        }
        isInGame = m_IsInGame;
        s_Text = m_DisplayText;
        if (isInGame)
            isLeague = FastSkillTeam.FST_SettingsManager.MatchType == 3;

        if (!isInGame || (isInGame && isLeague))
            s_Text.text = "obtaining data...";

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        yield return new WaitUntil(() => !string.IsNullOrEmpty(GameManager.CurrentLeagueID));
        if (m_RefreshRate <= 0)
            UpdateTextInternal();
        else
            InvokeRepeating("UpdateTextInternal", 0, m_RefreshRate);
    }

    private void OnDisable()
    {
        GameManager.CurrentLeagueID = "";
        StopAllCoroutines();
        CancelInvoke();
    }

    /// <summary>
    /// Will only be called if not already auto invoked
    /// </summary>
    public void UpdateText()
    {
        if (m_RefreshRate <= 0)// dont allow external scripts to update if we are auto updating
            UpdateTextInternal();
    }

    /// <summary>
    /// Call this to start the text update, it will update when it gets repsonse from server
    /// </summary>
    private void UpdateTextInternal()
    {
        if (!s_Text)
            return;

        if (isInGame && !isLeague)
            return;

        Jiweman.Joga_NetworkManager.Instance.GetTotalPrizePool(GameManager.CurrentLeagueID);
    }

    private static int count = 0;
    /// <summary>
    /// when server responds it calls this and updates the text visually with the new data
    /// </summary>
    public static void OnUpdate()
    {
        ++count;
        FST_MPDebug.Log("PrizePool: Update count this session = " + count);
        Debug.Log("PrizePool: Update count this session = " + count);
        s_Text.text = Currency + Amount.ToString("##,#"/*, new System.Globalization.CultureInfo("en-US")*/);//or "N" instead of custom "##,#"
    }
}
