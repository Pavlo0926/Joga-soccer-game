/////////////////////////////////////////////////////////////////////////////////
//
//  FST_FormationsManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This script is the bridge between UI formation selections and
//                  in game formations. Formation buttons should have a 
//                  FST_FormationButton.cs component attached in order to make 
//                  these selections. Works both online and offline.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using System.Collections;

namespace FastSkillTeam
{
    public class FST_FormationsManager : MonoBehaviour
    {
        public static FST_FormationsManager Instance;

        #region INSPECTOR VARIABLES
#pragma warning disable CS0649
        [SerializeField] private GameObject m_MainPanel;//child of this object
        [SerializeField] private Text m_TitleBarText;
        [SerializeField] private Button m_AttackingButton;
        [SerializeField] private Button m_DefensiveButton;
        [SerializeField] private Button m_BalanceButton;
        [SerializeField] private Button m_ApplyButton;
        [SerializeField] private Button m_BackButton;
        [SerializeField] private GameObject m_Player1_Container;
        [SerializeField] private GameObject m_Player2_Container;
        [SerializeField] private GameObject m_ScrollRectPlayer1_Attacking;
        [SerializeField] private GameObject m_ScrollRectPlayer1_Balance;
        [SerializeField] private GameObject m_ScrollRectPlayer1_Defensive;
        [SerializeField] private GameObject m_ScrollRectPlayer2_Attacking;
        [SerializeField] private GameObject m_ScrollRectPlayer2_Balance;
        [SerializeField] private GameObject m_ScrollRectPlayer2_Defensive;
        [SerializeField] private Transform m_PlayWithAIFormationIndicatorParent;
#pragma warning restore CS0649
        #endregion
        private bool didSendApply = false;
        private bool isOpenInGame = false;
        //NOTE: play with ai formation select button is hooked in by inspector
        public void OpenOrCloseFormationPanel(bool open)
        {
            bool inGame = SceneManager.GetActiveScene().name == "InGame";

            if (FST_Gameplay.IsMultiplayer)
            {
                if (open && inGame)
                {
                    if (isOpenInGame)
                    {
                        Debug.Log("Formations already open!");
                        return;
                    }
                    didSendApply = false;
                    isOpenInGame = true;
                }

                if (!open && inGame)
                {
                    if (!isOpenInGame)
                    {
                        Debug.Log("Formations already closed!");
                        return;
                    }

                    isOpenInGame = false;
                }
            }

            if (inGame)
                UIManager.Instance.RemainingTime.enabled = true;

            if (FST_Gameplay.IsMultiplayer)
            {
                if (inGame && open && (FST_Gameplay.IsMaster || !GlobalGameManager.Instance.IsAllPlayersConnected) && PhotonNetwork.InRoom)
                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { GlobalGameManager.RP_FORMATIONSET, 0 } });

                m_TitleBarText.text = FST_SettingsManager.PlayerName + " Choose your formation!";
            }
            else m_TitleBarText.text = "Player 1 Choose your formation!";

            if (!open)
            {
                transform.localScale = Vector3.zero;
                m_MainPanel.SetActive(false);
                return;
            }

            m_Player1_Container.SetActive(true);
            m_Player2_Container.SetActive(false);

            transform.localScale = Vector3.one;
            m_BackButton.gameObject.SetActive(!inGame);
            m_MainPanel.SetActive(true);
        }

        private void OnEnable()
        {
            Instance = this;

            m_ApplyButton.interactable = false;

            m_BackButton.onClick.AddListener(() => OnClickBack());

            m_BalanceButton.onClick.AddListener(() => ShowBalance());
            m_AttackingButton.onClick.AddListener(() => ShowAttacking());
            m_DefensiveButton.onClick.AddListener(() => ShowDefensive());
        }

        private void ShowBalance()
        {
            m_AttackingButton.transform.GetChild(0).gameObject.SetActive(false);
            m_DefensiveButton.transform.GetChild(0).gameObject.SetActive(false);
            m_BalanceButton.transform.GetChild(0).gameObject.SetActive(true);

            if (m_Player1_Container.activeInHierarchy)
            {
                m_ScrollRectPlayer1_Attacking.SetActive(false);
                m_ScrollRectPlayer1_Defensive.SetActive(false);
                m_ScrollRectPlayer1_Balance.SetActive(true);
            }
            else
            {
                m_ScrollRectPlayer2_Attacking.SetActive(false);
                m_ScrollRectPlayer2_Defensive.SetActive(false);
                m_ScrollRectPlayer2_Balance.SetActive(true);
            }
        }

        private void ShowAttacking()
        {
            m_AttackingButton.transform.GetChild(0).gameObject.SetActive(true);
            m_DefensiveButton.transform.GetChild(0).gameObject.SetActive(false);
            m_BalanceButton.transform.GetChild(0).gameObject.SetActive(false);

            if (m_Player1_Container.activeInHierarchy)
            {
                m_ScrollRectPlayer1_Attacking.SetActive(true);
                m_ScrollRectPlayer1_Defensive.SetActive(false);
                m_ScrollRectPlayer1_Balance.SetActive(false);
            }
            else
            {
                m_ScrollRectPlayer2_Attacking.SetActive(true);
                m_ScrollRectPlayer2_Defensive.SetActive(false);
                m_ScrollRectPlayer2_Balance.SetActive(false);
            }
        }

        private void ShowDefensive()
        {
            m_AttackingButton.transform.GetChild(0).gameObject.SetActive(false);
            m_DefensiveButton.transform.GetChild(0).gameObject.SetActive(true);
            m_BalanceButton.transform.GetChild(0).gameObject.SetActive(false);

            if (m_Player1_Container.activeInHierarchy)
            {
                m_ScrollRectPlayer1_Attacking.SetActive(false);
                m_ScrollRectPlayer1_Defensive.SetActive(true);
                m_ScrollRectPlayer1_Balance.SetActive(false);
            }
            else
            {
                m_ScrollRectPlayer2_Attacking.SetActive(false);
                m_ScrollRectPlayer2_Defensive.SetActive(true);
                m_ScrollRectPlayer2_Balance.SetActive(false);
            }
        }

        private void OnDisable()
        {
            m_BackButton.onClick.RemoveListener(() => OnClickBack());
            m_BalanceButton.onClick.RemoveListener(() => ShowBalance());
            m_AttackingButton.onClick.RemoveListener(() => ShowAttacking());
            m_DefensiveButton.onClick.RemoveListener(() => ShowDefensive());
        }
        private void OnClickBack()
        {
            m_ApplyButton.interactable = false;
            m_ApplyButton.onClick.RemoveAllListeners();
        }
        private void Apply(bool formation1)
        {
            m_ApplyButton.onClick.RemoveAllListeners();
            m_ApplyButton.interactable = false;
            if (formation1)
            {
                if (SceneManager.GetActiveScene().name == "InGame")
                {
                    if (FST_Gameplay.IsMultiplayer)
                    {
                        if (GlobalGameManager.Instance.ChooseFormationButton.activeInHierarchy)
                        {
                            OpenOrCloseFormationPanel(false);
                            return;
                        }

                        // When time is up, the panel will close... but lets speed it up if two players have applied!
                        if (PhotonNetwork.InRoom)
                        {
                            Hashtable ht = PhotonNetwork.CurrentRoom.CustomProperties;

                            if (!didSendApply)
                            {
                                if (ht.TryGetValue(GlobalGameManager.RP_FORMATIONSET, out object o))
                                    PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { GlobalGameManager.RP_FORMATIONSET, (int)o + 1 } });
                                else PhotonNetwork.CurrentRoom.SetCustomProperties(new Hashtable { { GlobalGameManager.RP_FORMATIONSET, 1 } });
                            }
                            didSendApply = true;
                        }
                    }
                    else
                    {
                        if (FST_SettingsManager.MatchType == 0)
                        {
                            OpenOrCloseFormationPanel(false);
                            return;
                        }

                        StartCoroutine(OnAnimate());

                        m_TitleBarText.text = "Player 2 Choose your formation!";
                        m_Player1_Container.SetActive(false);
                        m_Player2_Container.SetActive(true);
                    }
                }
                else
                {
                    if (FST_Gameplay.IsMultiplayer)
                    {
                        GameManager.Instance.MainMenuEvents("formation_online");
                    }
                    else
                    {
                        if (FST_SettingsManager.MatchType == 0)
                        {
                            SetFormationIndicatorForAIMatchType();
                            OpenOrCloseFormationPanel(false);
                            return;
                        }


                        StartCoroutine(OnAnimate());
                        m_TitleBarText.text = "Player 2 Choose your formation!";
                        m_Player1_Container.SetActive(false);
                        m_Player2_Container.SetActive(true);
                        ShowAttacking();

                        StartCoroutine(OnAnimate());
                    }
                }
            }
            else
            {
                if (SceneManager.GetActiveScene().name == "MainMenu")
                {
                  //  Debug.Log("ActiveLoading2Panel-19");
                    UIController.Instance.ActiveLoading2Panel();
                    GameManager.Instance.MainMenuEvents("formation2");

                }
                else
                {
                    UIManager.Instance.LoadingInGame.SetActive(true);
                    GameManager.Instance.LoadLevel("InGame");
                }
            }
        }

        public void SetFormationIndicatorForAIMatchType()
        {
            if (m_PlayWithAIFormationIndicatorParent)
            {
                for (int i = 0; i < m_PlayWithAIFormationIndicatorParent.childCount; i++)
                {
                    if (m_PlayWithAIFormationIndicatorParent.GetChild(i).GetComponent<FST_FormationButton>().GetIndex == FST_SettingsManager.Formation)
                    {
                        m_PlayWithAIFormationIndicatorParent.GetChild(i).gameObject.SetActive(true);
                        continue;
                    }
                    m_PlayWithAIFormationIndicatorParent.GetChild(i).gameObject.SetActive(false);
                }
            }
        }

        private RectTransform tr;
        private IEnumerator OnAnimate()
        {
            if (tr == null)
                tr = m_Player2_Container.GetComponent<RectTransform>();

            float x = 0;

            while (x < 360)
            {
                x += Time.deltaTime * 350;
                tr.localRotation = Quaternion.Euler(x, 0, 0);
                yield return null;
            }

           tr.localRotation = Quaternion.identity;
        }
        private FST_FormationButton[] formationButtons = null;
        public void SelectFormation(int index_number, Button button)
        {
            if(formationButtons == null || formationButtons.Length < 1)
                formationButtons = GetComponentsInChildren<FST_FormationButton>(true);

            for (int i = 0; i < formationButtons.Length; i++)
                formationButtons[i].GetButton.interactable = formationButtons[i].GetButton != button;

            if (FST_Gameplay.IsMultiplayer)
            {
                bool ready = FST_SettingsManager.MatchType == 3 || FST_Gameplay.IsPWF;
                Hashtable ht = new Hashtable { { FST_PlayerProps.FORMATION, index_number }, { FST_PlayerProps.READY, ready } };
               // Debug.LogFormat("Set PlayerProps > Formation: {0} ... Ready {1}", index_number, ready);
                PhotonNetwork.LocalPlayer.SetCustomProperties(ht);
            }
            FST_SettingsManager.Formation = index_number;

            m_ApplyButton.onClick.AddListener(() => Apply(true));

            m_ApplyButton.interactable = true;
        }

        public void SelectFormation2(int id, Button button)
        {
            if (formationButtons == null || formationButtons.Length < 1)
                formationButtons = GetComponentsInChildren<FST_FormationButton>(true);

            for (int i = 0; i < formationButtons.Length; i++)
                formationButtons[i].GetButton.interactable = formationButtons[i].GetButton != button;

            FST_SettingsManager.FormationOpponent = id;

            m_ApplyButton.onClick.AddListener(() => Apply(false));

            m_ApplyButton.interactable = true;
        }
    }
}


