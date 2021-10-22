using UnityEngine;
using FastSkillTeam;
using Photon;

namespace Jiweman
{
    public class Joga_Data : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] Joga_API.Server serverBase;
#pragma warning restore CS0649

        #region CONSTANT VARIABLES
        public const string FIRSTTIME_KEY = "FirstTime";
        #endregion

        #region DATA PROPERTIES
        public static Joga_Data Instance { get; private set; }
        public static string PlayerID { get; set; }
        public static string Token { get; set; }
        public static string DeviceToken { get; set; }
        public static string MatchId { get; set; }
        public static string RoomName { get; set; }
        public static Joga_MatchType MatchType { get; set; }
        #endregion

        private void Awake()
        {
            if (!Instance)
            {
                Instance = this;
                DontDestroyOnLoad(this);//should be ddol anyway from go, check me
            }

            else
            {
                Destroy(gameObject);
            }

            Joga_API.UpdateServerBase(serverBase);
        }

        public static string Parse(JSONNode json, string objectName, int index)
        {
            bool isString = json["data"][index][objectName].IsString;
            string value = isString ? json["data"][index][objectName].Value : json["data"][index][objectName].AsDouble.ToString();

            return value;
        }
    }
}

