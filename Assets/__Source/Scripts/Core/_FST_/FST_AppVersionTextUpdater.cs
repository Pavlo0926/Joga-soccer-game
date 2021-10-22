/////////////////////////////////////////////////////////////////////////////////
//
//  FST_AppVersionTextUpdater.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	Attach this script to UI text object and it will update the 
//                  text with the current build version.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

public class FST_AppVersionTextUpdater : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<UnityEngine.UI.Text>().text = "V " + Application.version;
    }
}
