using UnityEngine;

public class TouchCheckScript : MonoBehaviour
{
    public GameObject PlayerScript;
    public bool isFirstPlayerScript = false;
    private PlayerController m_PlayerController = null;
    private PlayerController GetPlayerController { get { if (!m_PlayerController) m_PlayerController =/* PlayerScript.GetComponent<PlayerController>()*/transform.root.GetComponent<PlayerController>(); return m_PlayerController; } }

    private Player2Controller m_Player2Controller = null;
    private Player2Controller GetPlayer2Controller { get { if (!m_Player2Controller) m_Player2Controller = /*PlayerScript.GetComponent<Player2Controller>()*/transform.root.GetComponent<Player2Controller>(); return m_Player2Controller; } }
    private void OnEnable()
    {
        if (GetPlayerController)
            isFirstPlayerScript = true;
        else isFirstPlayerScript = false;

        //for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++)
        //{

        //    BumpStaminaManager.instance.playerNumberForStaminaIncrease = i;
        //    BumpStaminaManager.instance.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer[i];

        //    BumpStaminaManager.instance.playerObjectCouterFromArray = i;
        //    break;

        //}
    }

    private bool IsSafe()
    {
        return !(UIManager.Instance.OppoReconnecting.activeInHierarchy ||
            UIManager.Instance.interNetLostPopup.activeInHierarchy ||
            UIManager.Instance.pauseMenuPanel.activeInHierarchy ||
            UIManager.Instance.gameStatusPlane.activeInHierarchy ||
FastSkillTeam.FST_FormationsManager.Instance.transform.GetChild(0).gameObject.activeInHierarchy ||
            UIManager.Instance.playerUpgradePanel.activeInHierarchy ||
            UIManager.Instance.GameStartgyPanel.activeInHierarchy ||
            UIManager.Instance.LoadingInGame.activeInHierarchy);
    }

    void OnMouseDown()
    {
        if (!IsSafe())
        {
            return;
        }

        if (isFirstPlayerScript)
        {
            if (!UIManager.Instance.doubleTapInitialized)
            {
                // init double tapping
                UIManager.Instance.doubleTapInitialized = true;
                UIManager.Instance.firstTapTime = Time.time;
                Invoke("CancelTimer", UIManager.Instance.timeBetweenTaps);
            }
            else if (Time.time - UIManager.Instance.firstTapTime < UIManager.Instance.timeBetweenTaps)
            {
                CancelInvoke("CancelTimer");
                UIManager.Instance.doubleTapInitialized = false;
                /*           for (int i = 0; i < GlobalGameManager.SharedInstance.allPlayer.Length; i++)
                           {

                               if (GlobalGameManager.SharedInstance.allPlayer[i].GetComponent<playerController>().discName == PlayerScript.GetComponent<playerController>().discName)
                               {
                                   *//*		BumpStaminaManager.instance.playerNumberForStaminaIncrease = i;
                                           BumpStaminaManager.instance.playerObjecctFind = GlobalGameManager.SharedInstance.allPlayer [i];

                                           BumpStaminaManager.instance.playerObjectCouterFromArray = i;
                                           *//*
                                   // for disable player stats eneble it when player stats enable
                                   break;
                               }
                           }*/
                                             //UIManager.instace.playerUpgradePanel.SetActive (true);  //as per MVP task list
                UIManager.Instance.playerUpgradePanel.SetActive(false);    //As per MVP tsak list
                return;
            }
            GetPlayerController.PlayerMouseDown();
        }
        else
        {
            GetPlayer2Controller.PlayerMouseDown();
        }
    }

    public void CancelTimer()
    {
        UIManager.Instance.doubleTapInitialized = false;
    }


    void OnMouseDrag()
    {
        if (!IsSafe())
        {
            return;
        }

        if (isFirstPlayerScript)
        {
            GetPlayerController.PlayerMouseDrag();
        }
        else
        {
            GetPlayer2Controller.PlayerMouseDrag();
        }
    }

    void OnMouseUp()
    {
        if (!IsSafe())
        {
            return;
        }

        if (isFirstPlayerScript)
        {
            GetPlayerController.PlayerMouseUp();
        }
        else
        {
            GetPlayer2Controller.PlayerMouseUp();
        }

    }
}

