using UnityEngine;
namespace FastSkillTeam
{
    public class FST_AppHandler : MonoBehaviour
    {
        /// <summary>
        /// OnAppSoftPause delegate.
        /// </summary>
        public delegate void AppSoftPause(bool paused);

        /// <summary>
        /// Called when the app state changes.
        /// </summary>
        public static event AppSoftPause OnAppSoftPause;

        public static bool IsSendingLeaguesRequest { get; set; } = false;

        public static bool IsQuitting { get; private set; } = false;

        public static bool LastSoftPauseWasInGame { get; private set; } = false;

        private void OnEnable()
        {
            IsQuitting = false;
        }

        private void OnApplicationQuit()
        {
            IsQuitting = true;
        }

#if UNITY_ANDROID && !UNITY_EDITOR
        /// <summary>
        /// App is soft closed:<br></br>
        ///   OnApplicationPause(true) is called<br></br><br></br>
        /// App is brought forward after soft closing:<br></br>
        ///   OnApplicationPause(false) is called<br></br>
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause(bool pause)
        {
            if (IsQuitting)
                return;

            OnAppSoftPause?.Invoke(pause);

            LastSoftPauseWasInGame = GlobalGameManager.Instance != null;
            IsSendingLeaguesRequest = false;
           // Debug.Log("OnApplicationPause: " + pause);

            if (!FST_AppHandlerControl.RunChecks)
                return;

            if (pause)
                return;

            if (GameStates.currentState != GAME_STATE.LEVELSELECTION || FST_SettingsManager.MatchType != 3)
            {
                if (!FST_SettingsManager.IsGuest)
                    Jiweman.Joga_NetworkManager.Instance?.CheckAuthentication();
                return;
            }

            Debug.Log("OnApplicationPause: refreshing league data");
            IsSendingLeaguesRequest = true;
            UIController.Instance.ActiveLoading2Panel();
            Jiweman.Joga_NetworkManager.Instance.GetLeaguesRequest();
        }
#else
        public bool InvokeAppPauseInEditor = true;
        /// <summary>
        /// App initially starts:<br></br>
        ///   OnApplicationFocus(true) is called<br></br><br></br>
        /// App is soft closed:<br></br>
        ///   OnApplicationFocus(false) is called<br></br><br></br>
        /// App is brought forward after soft closing:<br></br>
        ///   OnApplicationFocus(true) is called
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus(bool focus)
        {
            if (IsQuitting)
                return;

            if (InvokeAppPauseInEditor)
                OnAppSoftPause?.Invoke(!focus);

            LastSoftPauseWasInGame = GlobalGameManager.Instance != null;
            IsSendingLeaguesRequest = false;
          //  Debug.Log("OnApplicationFocus: " + focus);

            if (!FST_AppHandlerControl.RunChecks)
                return;

            if (!focus)
                return;

            if (GameStates.currentState != GAME_STATE.LEVELSELECTION || FST_SettingsManager.MatchType != 3)
            {
                if (!FST_SettingsManager.IsGuest)
                    Jiweman.Joga_NetworkManager.Instance?.CheckAuthentication();
                return;
            }

            Debug.Log("OnApplicationFocus: refreshing league data");
            IsSendingLeaguesRequest = true;
            UIController.Instance.ActiveLoading2Panel();
            Jiweman.Joga_NetworkManager.Instance.GetLeaguesRequest();
        }
#endif
    }
}
