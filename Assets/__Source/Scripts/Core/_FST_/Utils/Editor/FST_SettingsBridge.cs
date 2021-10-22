using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
namespace FastSkillTeam {
    public class FST_SettingsBridge : EditorWindow
    {
        [MenuItem("FST Tools/ShowSettingsWindow")]
        public static void ShowSettingsWindow()
        {
            EditorWindow.GetWindow(typeof(FST_SettingsBridge));
        }

        private void OnGUI()
        {
            GUILayout.Label("League Tickets: " + FST_SettingsManager.LeagueTickets);

            if (GUILayout.Button("Reset League Tickets"))
                FST_SettingsManager.LeagueTickets = 0;

            if (GUILayout.Button("Add League Tickets"))
                FST_SettingsManager.LeagueTickets++;

            if (GUILayout.Button("Test Download 5mb"))
                FST_UpdateManager.Test();
        }
    }
}