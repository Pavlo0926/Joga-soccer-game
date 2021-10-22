/////////////////////////////////////////////////////////////////////////////////
//
//  FST_Button_CancelOpponentFind.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	A simple load balanced button, for closing the FindingOpponent
//                  screen
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

public class FST_Button_CancelOpponentFind : MonoBehaviour
{
    public static FST_Button_CancelOpponentFind Instance;
    private Button m_Button;
    private void OnEnable()
    {
        Instance = this;

        if (!m_Button)
            m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(() => GameManager.Instance.CancelOpponentFind());
    }
    private void OnDisable()
    {
        m_Button.onClick.RemoveListener(() => GameManager.Instance.CancelOpponentFind());
    }
}
