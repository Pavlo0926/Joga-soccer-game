using FastSkillTeam;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Photon.Pun;

public class UIManager : MonoBehaviour
{

    public GameObject ads;
    public GameObject pauseMenuPanel;

    // public GameObject waitingPanel;
    public Text WaitTime;
    public Text RemainingTime;

    public GameObject interNetLostPopup;

    public GameObject Reconnecting;

    public Text ReconnectingText;

    public GameObject OppoReconnecting;
    public Text OppoReconnectingText;

    public GameObject gameStatusPlane;

    public GameObject winscreen_2;

    //offline pass play and ai
    public GameObject Offline_WinScreen;
    public Text Offline_WinText;
    public Text PlayerGoal;
    public Text OpponentGoal;
    public Text PlayerGoalTextWinscreen_1, OpponentGoalTextWinscreen_1;
    public Text PlayerGoalTextWinscreen_2, OpponentGoalTextWinscreen_2;
    public GameObject Winner_icon1, Winner_Icon2;

    public GameObject satminaPanel;

    public GameObject playerUpgradePanel;

    public GameObject GameStartgyPanel;

    public GameObject LoadingInGame;
    public Text LoadingText;

    public GameObject[] leftPlayerText;

    public Button curveLoftBtn;

    public GameObject Rematch1;
    public GameObject Rematch2;
    public Button RematchOfflineButton;
    public Sprite[] curveLoft;

    public bool IsCurveActive { get; set; }

    public float firstTapTime = 0f;
    public float timeBetweenTaps = 0.3f;
    // time between taps to be resolved in double tap
    public bool doubleTapInitialized;

    public static UIManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        gameStatusPlane.SetActive(false);
        winscreen_2.SetActive(false);
        //  ActiveStaminaPanel();
    }

    private void OnEnable()
    {
        RematchOfflineButton.onClick.AddListener(() => RematchButton());
        curveLoftBtn.onClick.AddListener(() => OnClickCurveShot());
    }
    private void OnDisable()
    {
        RematchOfflineButton.onClick.RemoveListener(() => RematchButton());
        curveLoftBtn.onClick.RemoveListener(() => OnClickCurveShot());
    }

    /// <summary>
    /// Open Pause Menu in Ingame
    /// </summary>
    public void Open_PauseMenu()
    {
        if (Input.touchCount < 2)
            pauseMenuPanel.SetActive(true);
    }
    public void ResetPlayerStats()
    {
        /*
          Stats statsDetails = new Stats();

          statsDetails.forceData = forceData.ToString();
          statsDetails.time = time.ToString();
          statsDetails.stamina = stamina.ToString();
          statsDetails.aim = aim.ToString();
          statsDetails.diskName = diskName.ToString();
          statsDetails.diskId = diskId.ToString();
          statsDetails.status = diskStatus.ToString();

          string stat = JsonMapper.ToJson(statsDetails);

          //Debug.Log ("statsDetails--  " + statsDetails);

          Dictionary<string, string> headers = new Dictionary<string, string>();

          headers.Add("Content-Type", "application/json");

          byte[] pData = System.Text.Encoding.ASCII.GetBytes(stat.ToCharArray());
          ///POST by IIS hosting...

          WWW api = new WWW(WebSocketConstant.RESET_USER_DISK_STATS, pData, headers);

          StartCoroutine(waiUpdateDisk(api));
          */
    }
    public void Close_PauseMenu()
    {
        pauseMenuPanel.SetActive(false);
    }
    public void MenuButton()
    {
        Debug.Log("MenuButton!");
        GlobalGameManager.Instance.QuitMatch();
        // AchievementCompletedDataManager.instance.CheckCompleteAchievement();
    }
    public void ExitButton()
    {
        Debug.Log("ExitButton!");
        GlobalGameManager.Instance.QuitMatch();
        // AchievementCompletedDataManager.instance.CheckCompleteAchievement ();
    }
    /// <summary>
    /// Onclick function for Online Rematch
    /// </summary>
    public void RematchOneNOneButton()
    {
        Debug.Log("Try Rematch!");

        Rematch1.GetComponent<Button>().interactable = false;
        Rematch2.GetComponent<Button>().interactable = false;

        GameManager.Instance.CallRematchFunction();
    }

    public void RematchButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void OnClickCurveShot()
    {
        if (!IsCurveActive)
        {
            Debug.Log("CurveShotActive()");
            CurveShotActive();
        }
        else if (IsCurveActive)
        {
            Debug.Log("CurveShotDeActive()");
            CurveShotDeActive();
        }
    }

    public void CurveShotActive()
    {
        curveLoftBtn.GetComponent<Image>().sprite = curveLoft[0];
        IsCurveActive = true;
    }

    public void CurveShotDeActive()
    {
        curveLoftBtn.GetComponent<Image>().sprite = curveLoft[1];
        IsCurveActive = false;
    }

    public void CloseInternetLostPanel()
    {
        interNetLostPopup.SetActive(false);
    }

    public void MenuButtonInterLostPopup()
    {
        SceneManager.LoadScene("MainMenu");
    }

    private void ActiveStaminaPanel()
    {
        //if (GlobalGameManager.MatchType == 0 || GlobalGameManager.MatchType == 1 || GlobalGameManager.MatchType == 4 ) 
        if (GlobalGameManager.MatchType == 0 || GlobalGameManager.MatchType == 1 || GlobalGameManager.MatchType == 4 || GlobalGameManager.MatchType == 2)
            satminaPanel.SetActive(false);
        else satminaPanel.SetActive(true);
    }
    public void active_strategy_panel()
    {
        GameStartgyPanel.SetActive(true);
        StrategyPanelPlayerObject.instance.timest = true;
        StrategyPanelPlayerObject.instance.time = 5f;
    }
    public void Deactive_strategy_panel()
    {
        GameStartgyPanel.SetActive(false);
        StrategyPanelPlayerObject.instance.timest = false;
        StrategyPanelPlayerObject.instance.ImgeTime = 0;
        StrategyPanelPlayerObject.instance.time = 5f;
    }
}