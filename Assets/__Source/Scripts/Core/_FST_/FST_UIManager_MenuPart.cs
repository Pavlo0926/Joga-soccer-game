/////////////////////////////////////////////////////////////////////////////////
//
//  FST_UIManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class is a part for the hub of UI
//                 
/////////////////////////////////////////////////////////////////////////////////

using Jiweman;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace FastSkillTeam
{
    public class FST_UIManager_MenuPart : MonoBehaviour
    {
        public static FST_UIManager_MenuPart Instance;

        //from gamemanager
        public Transform myTransform;
        public float xPos = 0, yPos = 0, yPosCredits = 400;
        public float xpos1, ypos1;
        public float musicSliderValue, musicThumbValue;
        public float soundSliderValue, soundThumbValue;

        private ScrollRect ScrollRectRenameToCorrectNAME;// which scroll rect? define it
        public GameObject HelpScreen;// main help or will be more? rename if more, go by panel if it panel
        public GameObject CreditScreen;// credits? as in game coin creds or dev creds?, go by panel if it panel
        public GameObject ShopScreen;//shop, go by panel if it panel
        public GameObject ExitScreen;//etc etc which exit screen?
        public GameObject MenuScreen;//mainmenu yeah
        public GameObject QuickPlay;// button? if so make it button here too, no getcomponent need then
        public GameObject OfflineScreen;
        public GameObject AchievementScreen;
        public GameObject PauseScreen;
        public GameObject RateScreen;
        public GameObject SettingScreen;
        public GameObject LoadingScreen;
        public GameObject PurchasePopups;
        public GameObject LevelSelectionScreen;
        public GameObject SelectFormationScreen;
        public GameObject FormationSelectionOnline;
        public GameObject ShopFormationScreen;
        public GameObject ChooseOpponentScreen;
        public GameObject PlayWithFriendsLeagueScreen;
        public GameObject GameOverScreen;
        public GameObject SpinWheelScreen;
        public GameObject LeagueScreen;
        public GameObject UpgradeScreen;
        public GameObject RentScreen;
        public GameObject PlayerProfileScreen;
        public GameObject BrandScreen;
        public GameObject LeaderBoardScreen;
        public GameObject OfflineLeagueScreen;
        public GameObject SelectFormationScreen2;
        public GameObject PlayWithFriendsScreen;
        public GameObject InviteFriendsScreen;
        public GameObject ChallengeFriendsScreen;
        public GameObject SearchUserScreen;

        public GameObject loadingPrefab;
        public GameObject PausePrefab;
        public GameObject helpPrefab;
        //    public GameObject DiceRentPanal;
        public GameObject purchasePopPrefab;
        public GameObject loadingWheelScriptPrefab;
        public GameObject btnRetry, btnPause;

        public GameObject TeamManagentScreen;
        public GameObject StadiumStratgyScreen;
        public GameObject ClubManagementScreen;
        public GameObject SeasonLeaderboardScreen;
        public GameObject StadiumSelectionScreen;
        public GameObject ClubInfoScreen;
        public GameObject OptionScreen;
        public GameObject ClubExpensesScreen;
        public GameObject JiwemanloginScreen;
        public GameObject JiwemanRegistrationScreen;
        public GameObject accountCreatedScreen;
        public GameObject SelectCityScreen;
        public GameObject PlayerInformationScreen;
        public GameObject LoginPanal;
        public GameObject TutorialPanel;
        public GameObject BettingPanal;
        public GameObject ForgetPassword;


        //from uicontroller
        public List<GameObject> allLevelsScreen = new List<GameObject>();
        public InputField emailfield;
        public GameObject[] mScroller_mixbrand;
        public GameObject[] mixbrand_button;
        public GameObject[] mixbrand_ads;
        public GameObject[] mainbutton;

        [Header("Login and Registration UI Fields")]//NOTE THESE ARE USED, CHECK THEM
        public Joga_UIHandler loginAndRegisterUI;


        public GameObject ReconnctButton;
        public GameObject FindOpponentBtn;
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
        string mUser_id;

        public GameObject player1ForamtionPanel;
        public GameObject player2ForamtionPanel;


        public GameObject ReceiveChallengePopup, ChallengeAcceptedPopup, CantplayPopup, ChallengeHasSent;


        public List<GameObject> buy_caps_pages = new List<GameObject>();


        public GameObject force;
        public GameObject aim;
        public GameObject time;

        public string user_name;
        public string Pass_word;
        public string E_mail;
        public string user_ID;


        public bool isLoginActive;

        public Text PlayerRPText;

        public Image Player_Image;

        public Image TopBarPlayer_Image;

        public bool isChallengeActive;
        public bool isChallengeRecieve;

        public Text Challtime;
        public Text acctime;

        //If guest player try to play league show bello pop up
        public GameObject RegiSter_PopUP;
        public GameObject Login_PopUP;

        private void OnEnable()
        {
            FST_UIManager.Instance.Menu = this;
        }
    }
}

