using FastSkillTeam;
using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    /// <summary>
    /// UI handler for Player Profile Panel
    /// </summary>
    [System.Serializable]
    public class Joga_PlayerProfile
    {
        /*PENDING
        #region Public Variables
        public string UserName;
        public string UserId;
        public string TotalMatch;
        public string MatchesWon;
        public string GoalsFor;
        public string GoalAgainst;
        public string MatchesLoss;
        public string CleanSheet;
        #endregion
        */
#pragma warning disable CS0649
        #region Serialiazed Field
        [SerializeField] Text _userNameTxt;
        [SerializeField] Text _userIdTxt;
        [SerializeField] Text _totalMatchTxt;
        [SerializeField] Text _matchesWonTxt;
        [SerializeField] Text _goalsForTxt;
        [SerializeField] Text _goalAgainstTxt;
        [SerializeField] Text _matchesLossTxt;
        [SerializeField] Text _cleanSheetTxt;
        #endregion
#pragma warning restore CS0649
        /*PENDING
        public void Set()
        {
            _userNameTxt.text = FST_SettingsManager.PlayerName;
            _userIdTxt.text = "id : " + Joga_Data.PlayerID;
            _totalMatchTxt.text = TotalMatch;
            _matchesWonTxt.text = MatchesWon;
            _goalsForTxt.text = GoalsFor;
            _goalAgainstTxt.text = GoalAgainst;
            _matchesLossTxt.text = MatchesLoss;
            _cleanSheetTxt.text = CleanSheet;
        }
        */

        /// <summary>
        /// Set player stats.
        /// </summary>
        public void Set(string totalMatch, string matchesWon, string goalsFor, string goalAgainst, string matchesLoss, string cleanSheet)
        {
            _userNameTxt.text = FST_SettingsManager.PlayerName;
            _userIdTxt.text = "id : " + Joga_Data.PlayerID;
            _totalMatchTxt.text = totalMatch;
            _matchesWonTxt.text = matchesWon;
            _goalsForTxt.text = goalsFor;
            _goalAgainstTxt.text = goalAgainst;
            _matchesLossTxt.text = matchesLoss;
            _cleanSheetTxt.text = cleanSheet;
        }

        /// <summary>
        /// If player doesn't have stat yet call this.
        /// </summary>
        public void Set()
        {
            _userNameTxt.text = FST_SettingsManager.PlayerName;
            _userIdTxt.text = "id : " + Joga_Data.PlayerID;
            _totalMatchTxt.text = "0";
            _matchesWonTxt.text = "0";
            _goalsForTxt.text = "0";
            _goalAgainstTxt.text = "0";
            _matchesLossTxt.text = "0";
            _cleanSheetTxt.text = "0";
        }
    }

    /// <summary>
    /// Handler to all UI's
    /// </summary>
    [System.Serializable]
    public class Joga_UIHandler
    {
        [Header("REGISTRATION FIELDS")]
        public InputField firstNameInput;
        public InputField middleNameInput;
        public InputField lastNameInput;
        public InputField userNameInput;
        public InputField emailInput;
        public InputField mobileInput;
        public InputField passwordInput;
        public InputField confirmPasswordInput;
        public Text dateOfBirthTxt;
        public ToggleGroup OnOptionToggle;
        public Dropdown countryList;
        public GameObject registerErrorObject;

        [Header("LOGIN FIELDS")]
        public InputField userNameLoginInput;
        public InputField passwordLoginInput;
        public Text loginErrorTxt;
        public Text userNameErrorTxt;
        public Text passwordErrorTxt;
        public Image userNameGlow;
        public Image passwordGlow;
        public Image loginErrorObject;

        [Header("UI PANELS")]
        public GameObject registrationPanel;
        public GameObject loginPanel;
        public GameObject messagePanel;

        [Header("HUD UI PANELS")]
        public Text userNameDisplayTxt;
        public Text formationUserNameDisplayTxt;

        [Header("PLAYER PROFILE PANEL")]
        public Joga_PlayerProfile playerProfile;

        public void ShowRegistrationPanel(bool state)
        {
            registrationPanel.SetActive(state);
        }

        /// <summary>
        /// Show message panel
        /// </summary>
        public void ShowMessage(string message)
        {
            messagePanel.SetActive(true);

            Text msgTxt = messagePanel.transform.Find("msg_img/Message").GetComponent<Text>();
            msgTxt.text = message;
        }

        /// <summary>
        /// Clear the registration fields input
        /// </summary>
        public void ClearRegistrationFields()
        {
            firstNameInput.text = "";
            middleNameInput.text = "";
            lastNameInput.text = "";
            userNameInput.text = "";
            emailInput.text = "";
            passwordInput.text = "";
            confirmPasswordInput.text = "";
            dateOfBirthTxt.text = "";
        }

        /// <summary>
        /// Clear the login fields input
        /// </summary>
        public void ClearLoginFields()
        {
            userNameLoginInput.text = "";
            passwordLoginInput.text = "";
        }

        public void SetDisplayName()
        {
            userNameDisplayTxt.text = Photon.Pun.PhotonNetwork.LocalPlayer.NickName = FST_SettingsManager.PlayerName;
            formationUserNameDisplayTxt.text = FST_SettingsManager.PlayerName + " Choose Your Formation";
        }
    }
}

