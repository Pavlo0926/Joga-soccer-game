/////////////////////////////////////////////////////////////////////////////////
//
//  FST_Gameplay.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	a place for globally accessible info on the game session, such
//					as whether we're in singleplayer or multiplayer mode. this can
//					be inherited to provide more info on the game: for example: custom
//					game modes
//
//					TIP: for global quick-access to the local player, see 'FST_LocalPlayer'
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine.SceneManagement;
using FastSkillTeam;
using UnityEngine;

public class FST_Gameplay
{

	public static string Version = "0.0.0";

    private static bool isMp = false;

	public static bool IsMultiplayer
    {
        get
        {
            int mt = FST_SettingsManager.MatchType;

            return isMp || mt == 2 || mt == 3 || mt == 5;
        }
        set { isMp = value; }
    }

    protected static bool m_IsMaster = true;

    private static bool m_IsPWF = false;
    /// <summary>
    /// this bool also handles the FireBasePushNotification.isPlayWithfriend bool
    /// </summary>
    public static bool IsPWF { get { return m_IsPWF; } set { m_IsPWF = value; FireBasePushNotification.IsPlayWithfriend = m_IsPWF; } }


	/// <summary>
	/// this property can be set by multiplayer scripts to assign master status
	/// to the local player. in singleplayer this is forced to true
	/// </summary>
	public static bool IsMaster
	{
		get
		{
			if (!IsMultiplayer)
				return true;
			return m_IsMaster;
		}
		set
		{
			if (!IsMultiplayer)
				return;
			m_IsMaster = value;
		}
	}


	/// <summary>
	/// pauses or unpauses the game by means of setting timescale to zero. will
	/// backup the current timescale for when the game is unpaused.
	/// NOTE: will not work in multiplayer
	/// </summary>
	public static bool IsPaused
	{
		get { return FST_TimeUtility.Paused; }
		set { FST_TimeUtility.Paused = (FST_Gameplay.IsMultiplayer ? false : value); }
	}


	// this is set by FST_VRCameraManager in OnEnable and OnDisable
	public static bool IsVR = false;

	/// <summary>
	/// returns the build index of the currently loaded level (Unity version agnostic)
	/// </summary>
	public static int CurrentLevel
	{
		get
        {
            return SceneManager.GetActiveScene().buildIndex;
        }
	}


	/// <summary>
	/// returns the name of the currently loaded level (Unity version agnostic)
	/// </summary>
	public static string CurrentLevelName
	{
		get
        {
            return SceneManager.GetActiveScene().name;
        }
	}


	/// <summary>
	/// quits the game in the appropriate way, depending on whether we're running
	/// in the editor, in a standalone build or a webplayer (these are the only
	/// platforms supported by the 'Quit' method at present)
	/// </summary>
	public static void Quit(string webplayerQuitURL = "http://FastSkillTeam.com")
	{

#if UNITY_EDITOR
		FST_GlobalEvent.Send("EditorApplicationQuit");
#elif UNITY_STANDALONE
			Application.Quit();
#elif UNITY_WEBPLAYER
			Application.OpenURL(webplayerQuitURL);
#endif
		// NOTES:
		//  1) web player is not supported by Unity 5.4+
		//	2) on iOS, an app should only be terminated by the user
		//	3) at time of writing OpenURL does not work on WebGL

	}

}