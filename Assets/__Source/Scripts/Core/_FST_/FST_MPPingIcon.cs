/////////////////////////////////////////////////////////////////////////////////
//
//  FST_MPPingIcon.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	A simple class that handles the icon display for ping. 
//                  NOTE: the ping properties are set locally by FST_MPConnection
//
/////////////////////////////////////////////////////////////////////////////////

using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

namespace FastSkillTeam
{
    [RequireComponent(typeof(Image))]
    public class FST_MPPingIcon : MonoBehaviour
    {
#pragma warning disable CS0649
        [SerializeField] private Text m_Text;
#pragma warning restore CS0649
        private float updateTimer = 0;
        private const float k_UpdateTickRate = 5f;
        private int m_Ping = 100;

        private Image m_Icon;
        private Image Icon { get { if (!m_Icon) m_Icon = GetComponent<Image>(); return m_Icon; } }
        bool isRemote = false;
        void Start()
        {
            if (!FST_Gameplay.IsMultiplayer)
            {
                gameObject.SetActive(false);
                return;
            }

            if (name.Contains("2"))
                isRemote = true;

            CheckPing();

        }

        void Update()
        {
            updateTimer += Time.deltaTime;

            if (updateTimer < k_UpdateTickRate)
                return;

            updateTimer = 0;

            CheckPing();

        }

        void CheckPing()
        {
            if (isRemote)
            {
                if (GlobalGameManager.Instance.RemotePlayer == null)
                    return;

                if (GlobalGameManager.Instance.RemotePlayer.CustomProperties.TryGetValue(FST_PlayerProps.PING, out object o))
                    m_Ping = (int)o;
            }
            else if (PhotonNetwork.MasterClient != null && PhotonNetwork.MasterClient.CustomProperties.TryGetValue(FST_PlayerProps.PING, out object o))
                m_Ping = (int)o;

            SetConnectionIndicator(m_Ping);

         //   FST_MPDebug.Log(isRemote ? "Remote Ping = " + m_Ping : "Master Ping = " + m_Ping);
        }

        void SetConnectionIndicator(float _ping)
        {
            if (!Icon) return;

            if (!Icon.enabled)
                Icon.enabled = true;

            if (_ping < 250)
                Icon.color = Color.green;
            else if (_ping < 400)
                Icon.color = Color.white;
            else Icon.color = Color.red;

            if (m_Text)
                m_Text.text = _ping.ToString() + "ms";
        }
    }
}