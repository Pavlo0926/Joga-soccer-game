using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using Jiweman;

public class JiwemanRegisterdataManager : MonoBehaviour
{
    public static JiwemanRegisterdataManager _instance;

    ///////////Registration fields\\\\\\\\\
    public InputField firstname_input, middlename_input, lastname_input, username_txt, emailId_txt, mobileInput;
    public InputField createpassword_txt, age, confirmpassword_txt;
    public Text DOB_txt;
    public string pass_word;
    public ToggleGroup OnOption;
    public Text registerError_txt;
    public GameObject registerErrorObject;
    public int mdropdown;
    public Dropdown Country_List;
    public Button RegisterButton;
    string username, country, createpassword, confirmpassword, email_ID, gender, DOB;
    //public ToggleGroup toggle;
    //public Toggle m1;
    //public Toggle m2;

    //////////Login Fields\\\\\\\\\\
    public InputField UsernameText;
    public InputField passwordText;
    public Text loginError_txt, usernameError, PassError;
    public GameObject usernmGlow, PassGlow;

    public GameObject loginErrorObject;
    public const string MatchEmailPattern =
        @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
        + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
        + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
        + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";

    void Awake()
    {
        //		_instance = this;

    }
    private void OnEnable()
    {
        mobileInput.onValueChanged.AddListener((string s) => MobileInput(s, false));
        mobileInput.onEndEdit.AddListener((string s) => MobileInput(s, true));
        RegisterButton.onClick.AddListener(() => TryRegister());
    }
    private void OnDisable()
    {
        mobileInput.onValueChanged.RemoveListener((string s) => MobileInput(s, false));
        mobileInput.onEndEdit.RemoveListener((string s) => MobileInput(s, true));
        RegisterButton.onClick.RemoveAllListeners();
    }

    void Start()
    {
        registerErrorObject.SetActive(false);

    }
    private void MobileInput(string s, bool end)
    {
        if (end)
        {
            if (!IsMobileNumberValid(s))
                SSTools.ShowMessage("Please Enter a Valid Mobile Number", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else
        {
            if (!IsMobileNumberValidWhileTyping(s))
                SSTools.ShowMessage("Must start with '+' and be followed by digits only", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
    }
    private readonly char[] m_ValidNumbers = new char[] { '+', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '0' };

    private bool IsMobileNumberValidWhileTyping(string number)
    {
        if (!string.IsNullOrEmpty(number) && (number.Length > 0))
        {
            if (number[0] != '+')
            {
                number = number.Insert(0, "+");
                mobileInput.text = number;
                mobileInput.caretPosition = number.Length;
            }
            bool valid = false;
            for (int x = 0; x < m_ValidNumbers.Length; x++)
            {
                if (number[number.Length - 1] == m_ValidNumbers[x])
                {
                    valid = true;
                    break;
                }
            }

            if (valid)
                return true;
            else number.Remove(number.Length - 1);

        }

        //if we make it here we are all good!
        return false;
    }

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
    private void TryRegister()
    {
        string first = firstname_input.text.Trim();
        string middle = middlename_input.text.Trim();
        string last = lastname_input.text.Trim();

        username = username_txt.text.Trim();
        mdropdown = Country_List.value;

        foreach (var item in OnOption.ActiveToggles())
        {
            gender = item.name;
        }

        //  Debug.Log("Gender" + OnOption + "--" + "gn" + gender);

        country = Country_List.options[mdropdown].text;

        DOB = DOB_txt.text.Trim();
        createpassword = createpassword_txt.text.Trim();
        confirmpassword = confirmpassword_txt.text.Trim();
        email_ID = emailId_txt.text;

        if (string.IsNullOrEmpty(first))
        {
            //  registerError_txt.text = "Please Enter First name";
            SSTools.ShowMessage("Please Enter First name", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else if (string.IsNullOrEmpty(last))
        {
            //  registerError_txt.text = "Please Enter Last name";
            SSTools.ShowMessage("Please Enter Last name", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else if (string.IsNullOrEmpty(username))
        {
            //registerError_txt.text = "Please Enter User name";
            SSTools.ShowMessage("Please Enter User name", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (gender == null)
        {
            // registerError_txt.text = "Please select gender";
            SSTools.ShowMessage("Please Select Gender", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (DOB == "")
        {
            //registerError_txt.text = "Please Enter Date Of Birth";
            SSTools.ShowMessage("Please Select Date of Birth", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (createpassword == "" || createpassword.Length < 4)
        {
            // registerError_txt.text = "Please Enter the Password";
            SSTools.ShowMessage("Please Enter Password (4 character minimum)", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (confirmpassword == "")
        {
            // registerError_txt.text = "Please Enter the Confirm Password";
            SSTools.ShowMessage("Please Enter Confirm Password", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (!createpassword.Equals(confirmpassword))
        {
            //registerError_txt.text = "Password & Confirm Password does not match";
            SSTools.ShowMessage("Password & Confirm Password does not match", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (email_ID == "")
        {
            // registerError_txt.text = "Please Enter Email Id";
            SSTools.ShowMessage("Please Enter Email Id", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (!ValidateEmail(email_ID))
        {
            // registerError_txt.text = "Please Enter a valid Email Id";
            SSTools.ShowMessage("Please Enter a Valid Email Id", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }

        else if (country == "")
        {
            // registerError_txt.text = "Please Enter the Country";
            SSTools.ShowMessage("Please Select Your Country", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else if(!IsMobileNumberValid(mobileInput.text))
        {
            // registerError_txt.text = "Please Enter the Country";
            SSTools.ShowMessage("Please Enter a Valid Mobile Number", SSTools.Position.bottom, SSTools.Time.threeSecond);
        }
        else
        {
            Joga_AuthManager.Instance.Register();
        }

    }

    public static JiwemanRegisterdataManager SharedInstance
    {
        get
        {
            // if the instance hasn't been assigned then search for it
            if (_instance == null)
                _instance = FindObjectOfType(typeof(JiwemanRegisterdataManager)) as JiwemanRegisterdataManager;

            return _instance;
        }
    }

    public void JiwemanRegister_user()
    {
        Debug.LogError("JiwemanRegister_user is old and unused!");
    }

    public void RegisterSuccess()
    {
        firstname_input.text = "";
        middlename_input.text = "";
        lastname_input.text = "";
        username_txt.text = "";
        createpassword_txt.text = "";
        confirmpassword_txt.text = "";
        emailId_txt.text = "";
        DOB_txt.text = "";

        username = "";
        gender = "";
        DOB = "";
        createpassword = "";
        confirmpassword = "";
        email_ID = "";
        country = "";
    }

    public void successLogin()
    {
        UsernameText.text = "";
        passwordText.text = "";
    }

    public void JiwemanLogin()
    {
        Debug.Log("JiwemanLogin() is used by a button...");

        string pass = passwordText.text.Trim();
        if (UsernameText.text.Trim() != "" && pass != "" && pass.Length > 3)
        {
            UIController.Instance.ActiveLoading2Panel();
          //  Debug.Log("pass" + passwordText.GetComponent<InputField>().text);
            pass_word = passwordText.GetComponent<InputField>().text.Trim();
        }

        else
        {
            if (UsernameText.text.Trim() == "")
            {
                usernameError.enabled = true;
                usernameError.text = "Enter Username";
                usernmGlow.GetComponent<Image>().enabled = true;
                StartCoroutine(CloseUsernm());
                // SSTools.ShowMessage("Please enter the User name", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            else if (passwordText.text.Trim() == "" || pass.Length < 4)
            {
                PassError.enabled = true;
                PassError.text = "Enter password (4 character min)";
                PassGlow.GetComponent<Image>().enabled = true;
                StartCoroutine(ClosePass());
                //SSTools.ShowMessage("Please enter the password", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            // loginErrorObject.SetActive(true);
        }
    }
    IEnumerator CloseUsernm()
    {
        yield return new WaitForSeconds(0.5f);
        usernmGlow.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        usernmGlow.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        usernameError.enabled = false;
        usernmGlow.GetComponent<Image>().enabled = false;
    }
    IEnumerator ClosePass()
    {
        yield return new WaitForSeconds(0.5f);
        PassGlow.GetComponent<Image>().enabled = false;
        yield return new WaitForSeconds(0.3f);
        PassGlow.GetComponent<Image>().enabled = true;
        yield return new WaitForSeconds(0.5f);
        PassError.enabled = false;
        PassGlow.GetComponent<Image>().enabled = false;
    }

    public static bool ValidateEmail(string email)
    {
        if (email != null)
            return Regex.IsMatch(email, MatchEmailPattern);
        else
            return false;
    }

    void CloseErrorOnTouch()
    {
        //  Touch touch = Input.GetTouch(0);
        loginErrorObject.SetActive(false);
    }

    public GameObject user_name, user_name_Glow;
    public GameObject Pass_word, Pass_word_Glow;

    public void JiwemanLoginPopUp()
    {
        Debug.Log("JiwemanLoginPopUp() is used by a button...");
        //  Debug.Log("Player first time"+PlayerPrefs.GetInt("FirstTimeLogin"));
        //   Debug.Log("Player first time"+ PlayerPrefs.GetInt ("FirstTime"));
        Debug.Log("User_Name In pop up" + user_name.GetComponent<InputField>().text);
        Debug.Log("Password In pop up" + Pass_word.GetComponent<InputField>().text);
        if (user_name.GetComponent<InputField>().text.Trim() != "" && Pass_word.GetComponent<InputField>().text.Trim() != "")
        {
            UIController.Instance.ActiveLoading2Panel();
           // Debug.Log("pass" + pass_word);
            pass_word = Pass_word.GetComponent<InputField>().text.Trim();
        }
        else
        {
            if (user_name.GetComponent<Text>().text.Trim() == "")
            {
                // usernameError.enabled = true;
                // usernameError.text = "Enter Username";
                user_name_Glow.GetComponent<Image>().enabled = true;
                StartCoroutine(CloseUsernm());
                // SSTools.ShowMessage("Please enter the User name", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }
            else if (Pass_word.GetComponent<Text>().text.Trim() == "")
            {
                // PassError.enabled = true;
                // PassError.text = "Enter password";
                Pass_word_Glow.GetComponent<Image>().enabled = true;
                StartCoroutine(ClosePass());
                //SSTools.ShowMessage("Please enter the password", SSTools.Position.bottom, SSTools.Time.threeSecond);
            }


            // loginErrorObject.SetActive(true);
        }

        //		if (GameManager.SharedInstance != null) {
        //			GameStates.SetCurrent_State_TO (GAME_STATE.SELECTCITY);
        //		}
    }
}