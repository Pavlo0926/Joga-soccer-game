/////////////////////////////////////////////////////////////////////////////////
//
//  FST_GhostBall.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	This script should be attached to a copy of the ball, just the
//                  renderer. no collider. This handles the "ghosting" of the ball
//                  to give the illusion of contact when things get out of sync 
//                  during collisons. FST_DiskPlayerManager.cs sends the Ghost call
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

namespace FastSkillTeam
{
    public class FST_GhostBall : MonoBehaviour
    {
        [SerializeField] private float m_ActiveTime = 0.25f;
        [SerializeField] private float m_Speed = 20f;
        private float m_Timeout = 0;
      //  private Vector3 m_TargetPos = Vector3.zero;
        public void Ghost(Vector3 pos/*, Vector3 targetPos*/)
        {
            m_Timeout = Time.time + m_ActiveTime;
           // m_TargetPos = targetPos;
            transform.position = pos;
            gameObject.SetActive(true);
        }
        void Update()
        {
            transform.position = Vector3.MoveTowards(transform.position,/* m_TargetPos*/FST_BallManager.Instance.transform.position, Time.deltaTime * m_Speed);
            transform.rotation = FST_BallManager.Instance.transform.rotation;
            if (Time.time > m_Timeout)
                gameObject.SetActive(false);
        }
    }
}
