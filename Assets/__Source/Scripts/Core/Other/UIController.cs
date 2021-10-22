using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Jiweman;
using FastSkillTeam;

public class UIController : MonoBehaviour
{
#pragma warning disable CS0649
    [SerializeField] private GameObject[] m_OnlineOnlyObjects;
#pragma warning restore CS0649

    public Button ForgetPassSubmitButton;
    public Button ForgetUserSubmitButton;

    public List<GameObject> allLevelsScreen = new List<GameObject>();
    public InputField emailfield;
    public GameObject[] mScroller_mixbrand;
    public GameObject[] mixbrand_button;
    public GameObject[] mixbrand_ads;
    public GameObject[] mainbutton;

    [Header("JOGA NEW UI HANDLER - NOT COMPLETED")]
    public Joga_UIHandler uiHandler;

    [Header("LeaderBoard Panel - Temporary and I can easily see this panel")]
    public GameObject LeaderBoardPanel;


    public GameObject ReconnctButton;
    public GameObject FindOpponentRetryBtn;
    public GameObject CloseChooseOppoBtn;
    public GameObject CloseFormationScreenBtn;
    public GameObject LevelselectionCloseBtn;

    public GameObject brandparent;

    public GameObject Mainscreen_Logo;

    public GameObject net_connection;

    public GameObject LoadingPanel;

    public GameObject Loading_2_panel;
    public GameObject loginPanel;

    public GameObject errorScreen;
    public GameObject Tutorialscreen;
    public GameObject SkipTutorials;

    public GameObject topbar;
    public GameObject bottom_bar;

    public GameObject Downcorner;
    public GameObject TopCorner;

    public GameObject Welcome_msg;
    public GameObject Tutorialprocedbtn;
    public GameObject Tutorialclosebtn;

    public Text welcome_text;
    public GameObject txtUser;

    public GameObject User_text;
    public GameObject txtUser_id;
    public GameObject imgProfile;
    public GameObject forgotPassOkPanel;
    public Text forgotPassMessege;
    public Text ForgotPassErroeText;
    public Image ForgotPassErrorImage;
    public bool forgetresponce;

    public GameObject player1ForamtionPanel;
    public GameObject player2ForamtionPanel;


    public GameObject ReceiveChallengePopup, ChallengeAcceptedPopup, CantplayPopup, ChallengeHasSent;


    public List<GameObject> buy_caps_pages = new List<GameObject>();

    public GameObject force;
    public GameObject aim;
    public GameObject time;

    public bool isLoginActive;

    public Text PlayerRPText;

    public Image Player_Image;

    public Image TopBarPlayer_Image;

    public bool IsChallengeActive { get; set; }
    public bool IsChallengeRecieved { get; set; }

    public Text Challtime;
    public Text acctime;

    //If guest player try to play league show bello pop up
    public GameObject RegiSter_PopUP;
    public GameObject Login_PopUP;

    private static UIController _instance = null;

    public static UIController Instance
    {
        get
        {
            // if the instance hasn't been assigned then search for it
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(UIController)) as UIController;
            }
            return _instance;
        }
    }

    #region call Initalize Api and Store the response

    void Start()
    {
        forgetresponce = false;

        IsChallengeActive = false;
        IsChallengeRecieved = false;

        GameManager.Instance.Levels = allLevelsScreen;
        Mainscreen_Logo.SetActive(true);

        GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
    }
    public void ShowRegistrationPanel(bool state)
    {
        uiHandler.ShowRegistrationPanel(state);
    }
    //private float nextTick = 0;
    private void Update()
    {
        // if (Time.time < nextTick)
        //    return;

        //  nextTick = Time.time + 1;

        if (FST_MPConnection.InternetReachability == NetworkReachability.NotReachable)
        {
            if (GameStates.currentState != GAME_STATE.MAIN_MENU && FST_Gameplay.IsMultiplayer)
                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);

            if (m_OnlineOnlyObjects[0].activeSelf)
                for (int i = 0; i < m_OnlineOnlyObjects.Length; i++)
                    m_OnlineOnlyObjects[i].SetActive(false);
            net_connection.SetActive(GameStates.currentState == GAME_STATE.MAIN_MENU);

        }
        else
        {
            if (!m_OnlineOnlyObjects[0].activeSelf)
                for (int i = 0; i < m_OnlineOnlyObjects.Length; i++)
                    m_OnlineOnlyObjects[i].SetActive(true);

            net_connection.SetActive(false);
        }
    }
    #endregion

    #region Active And Deactive Loading Panel

    public void ActiveLoading2Panel()
    {
        Loading_2_panel.SetActive(true);
    }

    //Deactive loading Panel After some time
    public void DeactiveLoading2Panel(float waitTime)
    {
        //  Debug.Log("Called");
        StartCoroutine(CloseLoading2Panel(0.2f));
    }

    IEnumerator CloseLoading2Panel(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Loading_2_panel.SetActive(false);
    }
    #endregion


    //Instantiate the prefab and assign reference to branding
    //As per MVP task list
    public void BGprefab_brand()
    {
        for (int i = 0; i < BrandManager.SharedInstance.brandlogo_count + 1; i++)
        {
            GameObject gm = Instantiate(GameManager.Instance.brandPrefab, Vector3.zero, Quaternion.identity) as GameObject;
            gm.GetComponent<Image>().sprite = GameManager.Instance.Brand_iconImg[i];
            gm.name = gm.name.Replace("1(Clone)", i + "");
            gm.transform.parent = brandparent.transform;
            gm.transform.localScale = Vector3.one;
            gm.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    public void Ads_assigning()
    {
        for (int i = 0; i < BrandManager.SharedInstance.brandads_count; i++)
        {
            mixbrand_ads[i].GetComponent<Image>().sprite = GameManager.Instance.Brand_ads[i];
            Instance.mixbrand_ads[i].SetActive(true);
        }
    }

    public void DesabledAllBuyCapsScreens()
    {
        for (int i = 0; i < buy_caps_pages.Count; i++)
        {
            buy_caps_pages[i].SetActive(false);
        }
    }


    #region Call Register With Play As Guest

    public void Guest_user()
    {
        // Debug.Log("ActiveLoading2Panel-2");
        if (PlayerPrefs.GetInt("FirstTime") != 0)
            ActiveLoading2Panel();
        // commented --@sud
        //   WebServicesHandler.SharedInstance.GetGuestLogin("guest");

        if (PlayerPrefs.HasKey("FirstTime") || PlayerPrefs.HasKey(" LoginType"))
        {
            GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
        }
        //WebSocketConstant.GET_GUESREGISTRATION;
    }

    #endregion


    #region Select GamePlay

    /// <summary>
    /// Which one you select type of gameplay active the room 
    /// </summary>
    /// <param name="button">Button.</param>
    public void clickLevels(GameObject button)
    {
        GameManager.Instance.MainMenuEvents(button.name);
    }

    public void clickTournamentLevels(GameObject button)
    {
        GameManager.Instance.MainMenuEvents(button.name);
    }

    #endregion


    #region Active And Deactive Challenge Popup


    public void ShowReceiveChallengePopup(bool show)
    {
        if (show)
        {
            IsChallengeRecieved = true;
            ReceiveChallengePopup.SetActive(true);
        }
        else ReceiveChallengePopup.SetActive(false);
    }



    //public void ShowPlaynowPopup()
    //{
    //    playnowPopup.SetActive(true);
    //}
    //public void ClosePlaynowPopup()
    //{
    //    playnowPopup.SetActive(false);
    //}

    #endregion

    public void ForgetPassword()
    {
        ForgetPassSubmitButton.gameObject.SetActive(true);
        ForgetUserSubmitButton.gameObject.SetActive(false);
        GameStates.SetCurrent_State_TO(GAME_STATE.FORGETPASSWORD);
        emailfield.text = "";
        if (JiwemanRegisterdataManager.SharedInstance)
            JiwemanRegisterdataManager.SharedInstance.successLogin();
    }

    public void OnClick_ForgetUsername()
    {
        ForgetPassSubmitButton.gameObject.SetActive(false);
        ForgetUserSubmitButton.gameObject.SetActive(true);
        GameStates.SetCurrent_State_TO(GAME_STATE.FORGETPASSWORD);
        emailfield.text = "";
        if (JiwemanRegisterdataManager.SharedInstance)
            JiwemanRegisterdataManager.SharedInstance.successLogin();
    }

    public void submit()
    {
        string emailtosend = emailfield.text;
        Debug.Log("FORGOT PASS : " + emailtosend);
        if (!JiwemanRegisterdataManager.ValidateEmail(emailtosend))
        {
            SSTools.ShowMessage("Please Enter Valid Email", SSTools.Position.bottom, SSTools.Time.threeSecond);
            return;
        }

        Jiweman.Joga_NetworkManager.Instance.ForgotPasswordRequest(emailtosend);

        emailfield.text = "";
        DisableErrorForgotpass();
    }

    public void OnClick_SubmitForgetUsername()
    {
        string emailtosend = emailfield.text;
        Debug.Log("FORGOT USERNAME : " + emailtosend);
        if (!JiwemanRegisterdataManager.ValidateEmail(emailtosend))
        {
            SSTools.ShowMessage("Please Enter Valid Email", SSTools.Position.bottom, SSTools.Time.threeSecond);
            return;
        }
        Jiweman.Joga_NetworkManager.Instance.ForgotUserNameRequest(emailtosend);

        emailfield.text = "";
        DisableErrorForgotpass();
    }

    public void DisableErrorForgotpass()
    {
        Instance.ForgotPassErrorImage.enabled = false;
        Instance.ForgotPassErroeText.enabled = false;
    }

    public void playwithfriendPopupclose()
    {
        ReceiveChallengePopup.SetActive(false);
        ChallengeAcceptedPopup.SetActive(false);
        CantplayPopup.SetActive(false);
    }

    public void ok()
    {
        if (forgetresponce)
        {
            GameStates.SetCurrent_State_TO(GAME_STATE.JIWEMANLOGIN);
            forgotPassOkPanel.SetActive(false);
        }
        else
            forgotPassOkPanel.SetActive(false);
    }

    public void LogOut()
    {
        FST_SettingsManager.LogOut();
        SceneManager.LoadScene("MainMenu");
    }
}