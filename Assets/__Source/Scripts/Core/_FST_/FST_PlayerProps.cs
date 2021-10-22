/////////////////////////////////////////////////////////////////////////////////
//
//  FST_PlayerProps.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	One globally accessible central class that contains all 
//                  player prop codes for clarity.
//
/////////////////////////////////////////////////////////////////////////////////

namespace FastSkillTeam {
    public static class FST_PlayerProps
    {
        public const string PING = "_p";
        public const string READY = "_r";
        public const string LOADED_LEVEL = "_ll";
        public const string REMATCH = "_rm";
        public const string FORMATION = "_frm";

        public static void Set(string key, bool value, Photon.Realtime.Player targetPlayer = null)
        {
            if (targetPlayer != null)
                targetPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
            else Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
        }

        public static void Set(string key, int value, Photon.Realtime.Player targetPlayer = null)
        {
            if (targetPlayer != null)
                targetPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
            else Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
        }

        public static void Set(string key, float value, Photon.Realtime.Player targetPlayer = null)
        {
            if (targetPlayer != null)
                targetPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
            else Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
        }

        public static void Set(string key, string value, Photon.Realtime.Player targetPlayer = null)
        {
            if (targetPlayer != null)
                targetPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
            else Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
        }

        public static void Set(string key, object value, Photon.Realtime.Player targetPlayer = null)
        {
            if (targetPlayer != null)
                targetPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
            else Photon.Pun.PhotonNetwork.LocalPlayer.SetCustomProperties(new ExitGames.Client.Photon.Hashtable { { key, value } });
        }

        public static object Get(string key, Photon.Realtime.Player targetPlayer = null)
        {
            object o;
            if (targetPlayer != null)
            {
                if (targetPlayer.CustomProperties.TryGetValue(key, out o))
                {
                    return o;
                }
            }

            if (Photon.Pun.PhotonNetwork.LocalPlayer.CustomProperties.TryGetValue(key, out o))
            {
                return o;
            }
            UnityEngine.Debug.LogWarning("Trying to get playerprop with key :{0} , but it has not been set on player yet!");
            return null;
        }
    }
}