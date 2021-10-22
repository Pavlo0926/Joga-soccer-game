/////////////////////////////////////////////////////////////////////////////////
//
//  FST_UIManager.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This class is the hub for all UI
//                 
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.SceneManagement;
namespace FastSkillTeam
{
    public class FST_UIManager : MonoBehaviour
    {
        public static FST_UIManager Instance;
        public FST_UIManager_MenuPart Menu { get; set; } = null;
        private void Awake()
        {
            if (Instance)
                Destroy(gameObject);
            else
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnEnable()
        {
          //  SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
          //  SceneManager.sceneLoaded -= OnSceneLoaded;
        }

      //  enum SceneID { Splash, MainMenu, InGame }
       // SceneID m_SceneID;

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(!Menu)
            {
                Menu = FindObjectOfType<FST_UIManager_MenuPart>();
                Debug.Log("had to find fst ui part");
            }

            if (!Menu)
            {
                Debug.LogError("no ui part");
            }


            switch (scene.name)
            {
                case "Splash":
                   // m_SceneID = SceneID.Splash;
                    break;
                case "MainMenu":
                  //  m_SceneID = SceneID.MainMenu;
                    //if quickplay button example
                //    Menu.QuickPlay.OnClick.AddListener(() => OnClickQuickPlay());// etc etc....
                    break;
                case "InGame":
                 //   m_SceneID = SceneID.InGame;
                    break;
            }

        }

        void OnClickQuickPlay()
        {

        }

        //void OnClickBack(int func)//func subject to change, this is example for when apply ONclick Listner
        //{
        //    switch (m_SceneID)
        //    {
        //        case SceneID.MainMenu:
        //            if (func == 0)
        //                GameStates.SetCurrent_State_TO(GAME_STATE.MAIN_MENU);
        //            break;
        //    }
        //}
    }
}

