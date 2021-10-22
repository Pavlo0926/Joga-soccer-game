using System.Collections;
using UnityEngine;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using FastSkillTeam;

namespace Jiweman
{
    public class Joga_AuthManager : MonoBehaviour
    {
        #region Private Variables

        private string email;
        private string fullName;
        private string userName;
        private string mobile;
        private string country;
        private string password;
        private string confirmPassword;
        private string gender;
        private string dateOfBirth;
        private int countryIndex;
        private string message;

        #endregion

        #region AUTH MANAGER PROPERTIES

        public static Joga_AuthManager Instance { get; private set; }

        public Joga_UIHandler UIHandler
        {
            get { return UIController.Instance.uiHandler; }
        }

        #endregion

        private void Awake()
        {
            if (!Instance)
                Instance = this;
        }
        string firstName = "";
        string middleName = "";
        string lastName = "";
        public void Register()
        {
            firstName = UIHandler.firstNameInput.text.Trim();
            middleName = UIHandler.middleNameInput.text.Trim();
            lastName = UIHandler.lastNameInput.text.Trim();

            fullName = firstName + " " + (string.IsNullOrEmpty(middleName) ? "" : (middleName + " ")) + lastName;


            userName = UIHandler.userNameInput.text.Trim();
            email = UIHandler.emailInput.text.Trim();
            mobile = UIHandler.mobileInput.text.Trim();
            password = UIHandler.passwordInput.text.Trim();
            confirmPassword = UIHandler.confirmPasswordInput.text.Trim();
            
            dateOfBirth = UIHandler.dateOfBirthTxt.text;
            
            countryIndex = UIHandler.countryList.value;
            country = UIHandler.countryList.options[countryIndex].text;

            foreach (var item in UIHandler.OnOptionToggle.ActiveToggles())
                gender = item.name;

            if (IsRegistrationValid())
            {
                Debug.Log("Registration Validated");
                UIController.Instance.Loading_2_panel.SetActive(true);
                Joga_NetworkManager.Instance.RegistrationAuthReuqest(fullName, userName, email, password, confirmPassword, 
                    country, dateOfBirth, gender, mobile);
            }
            else
            {
                ShowMessage(message);
            }
        }

        public void LogIn()
        {
            if (FST_SettingsManager.IsGuest)
            {
                Debug.Log("NOTE: is guest, in login");
            }

            userName = UIHandler.userNameLoginInput.text.Trim();
            password = UIHandler.passwordLoginInput.text.Trim();
          //  Debug.Log("Log in username : " + userName);
          //  Debug.Log("Log in password : " + password);

            if (!string.IsNullOrEmpty(userName) && !string.IsNullOrEmpty(password))
            {
              //  Debug.Log("Authenticate this account...");

                UIController.Instance.Loading_2_panel.SetActive(true);
                Joga_NetworkManager.Instance.LoginAuthRequest(userName, password);
            }

            else
            {
                UIHandler.loginErrorTxt.text = GetLoginValidation(userName, password);
                UIHandler.loginErrorTxt.enabled = true;

                if (UIHandler.loginErrorTxt.text.Contains("Username"))
                    StartCoroutine(ImageFxWarning(UIHandler.userNameGlow, UIHandler.loginErrorTxt));

                else if (UIHandler.loginErrorTxt.text.Contains("Password"))
                    StartCoroutine(ImageFxWarning(UIHandler.passwordGlow, UIHandler.loginErrorTxt));
            }
        }

        public void AutoLogin()
        {
            if (string.IsNullOrEmpty(FST_SettingsManager.PlayerName))
                return;
            if (string.IsNullOrEmpty(FST_SettingsManager.Password))
                return;

          //  Debug.Log("Auto Authenticate this account...");

            UIController.Instance.Loading_2_panel.SetActive(true);

            userName = FST_SettingsManager.PlayerName;
            password = FST_SettingsManager.Password;
            Joga_NetworkManager.Instance.LoginAuthRequest(userName, password);
        }

        #region AUTH MANAGER VALIDATE METHOD

        private readonly char[] m_ValidNumbers = new char[] { '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };
        private bool IsMobileNumberValid(string number)
        {
            if (string.IsNullOrEmpty(number))
                return false;

            number = number.Trim();

            if (number.Length < 12)
                return false;
            if (number.Length > 16)
                return false;

            bool valid = false;

            for (int i = 0; i < number.Length; i++)
            {
                for (int x = 0; x < m_ValidNumbers.Length; x++)
                {
                    if (number[i] == m_ValidNumbers[x])
                    {
                        valid = true;
                        break;
                    }
                }
            }

            if (valid)
                return true;

            //if we make it here we are all good!
            return true;
        }

        private bool IsRegistrationValid()
        {
            if (string.IsNullOrEmpty(firstName))
                message = "Please Enter First Name";

            else if (string.IsNullOrEmpty(lastName))
                message = "Please Enter Last Name";

            else if (string.IsNullOrEmpty(userName))
                message = "Please Enter User Name";

            else if (gender == null)
                message = "Please Select Gender";

            else if (string.IsNullOrEmpty(dateOfBirth))
                message = "Please Select Date of Birth";

            else if (string.IsNullOrEmpty(password))
                message = "Please Enter Password";

            else if (string.IsNullOrEmpty(confirmPassword))
                message = "Please Enter Confirm Password";

            else if (!password.Equals(confirmPassword))
                message = "Your Password does not match!";

            else if (string.IsNullOrEmpty(email))
                message = "Please Enter Email Id";

            else if (!IsEmailValidated(email))
                message = "Please Enter a Valid Email Id";

            else if (string.IsNullOrEmpty(mobile))
                message = "Please Enter Mobile Number";

            else if (!IsMobileNumberValid(mobile))
                message = "Please Enter a Valid Mobile";

            else if (string.IsNullOrEmpty(country))
                message = "Please Select Your Country";

            else
                return true;

            return false;
        }

        public bool IsEmailValidated(string email)
        {
            string matchEmailPattern = @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
            + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
            + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
            + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

            if (email != null)
                return Regex.IsMatch(email, matchEmailPattern);
            else
                return false;
        }

        private string GetLoginValidation(string username, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                message = "Enter Username";
            }

            else if (string.IsNullOrEmpty(password))
            {
                message = "Enter Password";
            }

            else
            {
                message = "Enter Username and Password";
            }
                
            return message;
        }

        /// <summary>
        /// Verified registration details.
        /// </summary>
        public void OnRegisterVerificationFinished(bool status, string message)
        {
            UIController.Instance.Loading_2_panel.SetActive(false);

            if (status)
            {
                string accountVerificationMsg = message;
                message = "Account Registration Successful!";

                UIHandler.registrationPanel.SetActive(false);
                UIHandler.ShowMessage(accountVerificationMsg);
            }

            Debug.Log(message);
            ShowMessage(message);
        }

        /// <summary>
        ///  Verified login details.
        /// </summary>
        public void OnLoginVerificationFinished(bool status, string message)
        {
            UIController.Instance.Loading_2_panel.SetActive(false);

            if (status)
            {
                UIHandler.loginPanel.SetActive(false);

                FST_SettingsManager.Login(userName, password);

                UIController.Instance.uiHandler.SetDisplayName();

                if (FST_SettingsManager.LoginCount < 4)
                {
                    Debug.Log("Tutorial count = " + FST_SettingsManager.LoginCount);
                    GameManager.Instance.MessageStart();
                }

                Debug.Log("Connect to pun now");
                FST_MPConnection.Instance.Connect();

                Debug.Log("Login Successful!");
            }
            else
            {
                Debug.Log("Login Failed! : " + message);
                ShowMessage(message);
            }

            GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
        }

        #endregion

        private void ShowMessage(string message)
        {
            SSTools.ShowMessage(message, SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        private IEnumerator ImageFxWarning(Image img, Text errorTxt)
        {
            yield return new WaitForSeconds(0.5f);
            img.enabled = false;

            yield return new WaitForSeconds(0.3f);
            img.enabled = true;

            yield return new WaitForSeconds(0.5f);

            errorTxt.enabled = false;
            img.enabled = false;
        }
    }
}