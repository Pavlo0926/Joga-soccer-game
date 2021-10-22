using FastSkillTeam;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckInternetConnection : MonoBehaviour
{
/*      public int failedNetworkCount;
      public int MAX_NUMBERS_OF_ATTEMPTS_FOR_INTERNET_CONNECTION;
      public static CheckInternetConnection instance;
      public Coroutine gameEnd;

      void Awake()
      {
            instance = this;

      }
      void Start()
      {
            MAX_NUMBERS_OF_ATTEMPTS_FOR_INTERNET_CONNECTION = 20;
            InvokeRepeating("checkNetworkConnection", 0.1f, 1f);
      }

      public void Update()
      {

      }

      public void CheckNetworkConnection()
      {
            if (!IfInternetIsConnected())
            {
                  if (FST_SettingsManager.MatchType == 2)
                  {
                        Debug.Log("INTERNET_CONNECTION_NOT_FOUND_==failedNetworkCount = " + failedNetworkCount);

                        ++failedNetworkCount;
                        if (failedNetworkCount < MAX_NUMBERS_OF_ATTEMPTS_FOR_INTERNET_CONNECTION)
                        {
                              if (failedNetworkCount == 1)
                              {                                   
                                    UIManager.Instance.OpenReconnectingPanel();
                              }
                              return;
                        }
                        else if (failedNetworkCount == MAX_NUMBERS_OF_ATTEMPTS_FOR_INTERNET_CONNECTION)
                        {
                             // UIManager.Instance.CloseReconnectingPanel();
                              Debug.Log("Max count ended" + MAX_NUMBERS_OF_ATTEMPTS_FOR_INTERNET_CONNECTION);
                              CancelInvoke("checkNetworkConnection");
                              if (UIManager.Instance.Reconnecting.activeSelf)
                                    UIManager.Instance.CloseReconnectingPanel();
                              GlobalGameManager.SharedInstance.PlayerSelfLeftLosePopup();

                              // SSTools.ShowMessage("The match has been ended. You Lost", SSTools.Position.bottom, SSTools.Time.twoSecond);
                        }
                  }
            }
            else
            {
                  failedNetworkCount = 0;
                  if (UIManager.Instance.Reconnecting.activeSelf)
                  {
                        UIManager.Instance.CloseReconnectingPanel();
                        if (GlobalGameManager.IsMastersTurn && playerController.Instance)
                              playerController.Instance.IsReadytoSoot = true;
                        // if (!UIManager.Instance.LoadingInGame.activeSelf)
                        //       UIManager.Instance.LoadingInGame.SetActive(true);
                  }
            }
      }

      public static bool IfInternetIsConnected()
      {
            if (Application.internetReachability != NetworkReachability.NotReachable)
            {
                  return true;
            }
            return false;
      }*/
}
