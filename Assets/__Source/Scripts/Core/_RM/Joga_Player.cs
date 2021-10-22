using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Jiweman
{
    public class Joga_Player
    {
        public static void GetStat(JSONNode json)
        {
            UIController.Instance.Loading_2_panel.SetActive(false);
            string playerData = json["LoggedInPlayerData"].Value;
            Debug.Log(playerData);

            Joga_PlayerProfile profile = UIController.Instance.uiHandler.playerProfile;

            if (!string.IsNullOrEmpty(playerData))
            {
                profile.Set();
                return;
            }

            string totalMatch = json["LoggedInPlayerData"]["matchesPlayed"].Value;
            string totalWon = json["LoggedInPlayerData"]["win"].Value;
            string totalGoalFor = json["LoggedInPlayerData"]["goalFor"].Value;
            string totalGoalAgainst = json["LoggedInPlayerData"]["goalAgainst"].Value;
            string totalLoss = json["LoggedInPlayerData"]["loss"].Value;
            string totalCleanSheet = json["LoggedInPlayerData"]["cleanSheet"].Value;

            profile.Set(totalMatch, totalWon, totalGoalFor, totalGoalAgainst, totalLoss, totalCleanSheet);

            Debug.Log("Player profile loaded");
        }
    }
}
