/////////////////////////////////////////////////////////////////////////////////
//
//  FST_FormationButton.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This script works in conjuction with FST_FormationsManager.cs
//                  All UI formation buttons should have this component on them.
//                  FST_FormationsManager.cs works both online and offline.
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEngine.UI;

namespace FastSkillTeam
{
    [RequireComponent(typeof(Button))]
    public class FST_FormationButton : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private int m_Index;
        [SerializeField] private bool m_IsPlayer2 = false;
#pragma warning restore CS0649
        private Button m_Button = null;
        public Button GetButton { get { if (!m_Button) m_Button = GetComponent<Button>(); return m_Button; } }
        public int GetIndex { get { return m_Index; } }//for play with ai image indicator

        private void OnEnable()
        {
            if (m_IsPlayer2)
                GetButton.onClick.AddListener(() => FST_FormationsManager.Instance.SelectFormation2(m_Index, GetButton));
            else GetButton.onClick.AddListener(() => FST_FormationsManager.Instance.SelectFormation(m_Index, GetButton));
        }
        private void OnDisable()
        {
            if (GetButton)
                GetButton.onClick.RemoveAllListeners();
        }
    }
}