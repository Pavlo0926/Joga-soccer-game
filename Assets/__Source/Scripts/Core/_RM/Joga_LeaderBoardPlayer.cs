using UnityEngine;
using UnityEngine.UI;

namespace Jiweman
{
    public class Joga_LeaderBoardPlayer : MonoBehaviour
    {
        public Text Name, allmatches, matchwin, matchlose, goalfor, goalagainst, goaldiff, cleansheet, rank, points, prizeFor, avgPPMText, leagueRoundText, leagueLegText;

        public GameObject OverallLeaderBoardBanner;
        public GameObject GoalForLeaderBoardBanner;
        public GameObject GoalConsidedLeaderBoardBanner;
        public GameObject WinstreakLeaderBoardBanner;
        public GameObject CleanSheetLeaderBoardBanner;
        public GameObject AvgPPMLeaderBoardBanner;

        public Text GoalByLead;
        public Text WinByLead;
        public Text CleansheetByLead;
        public Text GoalConsidedByLead;
        public Text AvgPPMLeadText;
        public Text LeagueRoundLeadText;
        public Text LeagueLegLeadText;

        public GameObject prizeObj;
        public GameObject PrizeImage;


        #region PROPERTIES

        public string GoalForLead { get; set; }
        public string WinLead { get; set; }
        public string CleansheetLead { get; set; }
        public string AvgPPMLead { get; set; }
        public string GoalConsidedLead { get; set; }

        public static string Currency { get; set; } = "USD";

        public string PlayerName { get; set; }

        public int TotalMatch { get; set; }

        public string MatchWin { get; set; }

        public string MatchLose { get; set; }
        public string GoalBy { get; set; }

        public string GoalAgainst { get; set; }

        public string GoalDifference { get; set; }

        public string Cleansheet { get; set; }

        public string AveragePointsPerMinute { get; set; }

        public string Points { get; set; }

        public float Prize { get; set; }

        public int Rank { get; set; }

        public int Round { get; set; }

        public int Leg { get; set; }

        #endregion

        public void Init(bool mainLB, bool isLeague)
        {
            leagueRoundText.text = LeagueRoundLeadText.text = Round.ToString();
            leagueLegText.text = LeagueLegLeadText.text = Leg.ToString();

            Name.text = PlayerName;
            allmatches.text = TotalMatch.ToString();
            matchwin.text = MatchWin;
            matchlose.text = MatchLose;
            goalfor.text = GoalBy;
            goalagainst.text = GoalAgainst;
            cleansheet.text = Cleansheet;
            goaldiff.text = GoalDifference;
            points.text = Points;
            GoalByLead.text = GoalBy;
            CleansheetByLead.text = CleansheetLead;
            GoalConsidedByLead.text = GoalConsidedLead;
            WinByLead.text = WinLead;
            AvgPPMLeadText.text = AveragePointsPerMinute;
            rank.text = Rank.ToString();
            avgPPMText.text = AveragePointsPerMinute;
            prizeFor.text = Currency + Prize.ToString("##,#.##"/*, new System.Globalization.CultureInfo("en-US")*/);//or "N" instead of custom "##,#"
            allmatches.enabled = mainLB;
            matchwin.enabled = mainLB;
            matchlose.enabled = mainLB;
            goalfor.enabled = mainLB;
            goalagainst.enabled = mainLB;
            cleansheet.enabled = mainLB;
            goaldiff.enabled = mainLB;
            points.enabled = mainLB;
            avgPPMText.enabled = mainLB;
            prizeFor.enabled = mainLB && isLeague && Prize > 0;
            leagueRoundText.enabled = isLeague;
            leagueLegText.enabled = isLeague;
        }

        public void PopUP()
        {
            Debug.Log("REMINDER: We can remove the onclick in editor from this button");
        }
    }
}
