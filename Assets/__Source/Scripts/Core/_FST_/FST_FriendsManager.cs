using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System.Collections.Generic;
using Jiweman;

public class FST_FriendsManager : MonoBehaviourPunCallbacks
{
#pragma warning disable CS0649
    [SerializeField] Transform m_FriendListParent;
#pragma warning restore CS0649

    private readonly float m_TickRate = 1f;
    private bool isButtonsEnabled = false;
    private List<FriendInfo> m_Friends = new List<FriendInfo>();
    private float nextFire = 0f;
    private static string[] m_InitPlayers = new string[0];
    public static void Init(string[] friendsUserIds)
    {
        m_InitPlayers = friendsUserIds;
    }

    private void Update()
    {
        if (m_InitPlayers.Length < 1 || !m_FriendListParent.gameObject.activeInHierarchy)
            return;

        if (!PhotonNetwork.InLobby)
        {
            for (int x = 0; x < m_FriendListParent.childCount; x++)
                if (m_FriendListParent.GetChild(x).GetComponentInChildren<ChallegePlayerDataStore>().Challengebtn.activeSelf)
                    m_FriendListParent.GetChild(x).GetComponentInChildren<ChallegePlayerDataStore>().Challengebtn.SetActive(false);

            isButtonsEnabled = false;
        }
        else if (!isButtonsEnabled)
        {
            isButtonsEnabled = true;

            Refresh();

            for (int x = 0; x < m_FriendListParent.childCount; x++)
                if (!m_FriendListParent.GetChild(x).GetComponentInChildren<ChallegePlayerDataStore>().Challengebtn.activeSelf)
                    m_FriendListParent.GetChild(x).GetComponentInChildren<ChallegePlayerDataStore>().Challengebtn.SetActive(true);

        }

        if (Time.time > nextFire)
        {
            nextFire = Time.time + m_TickRate;
            Refresh();
        }
    }
    /// <summary>
    /// unused but here just in case...
    /// </summary>
    /// <param name="friendsUserIds"></param>
    /// <returns></returns>
    public bool FindFriends(string[] friendsUserIds)
    {
        return PhotonNetwork.FindFriends(friendsUserIds);
    }

    public override void OnFriendListUpdate(List<FriendInfo> friendList)
    {
        //cache the list
        m_Friends = friendList;
    }

    private void Refresh()
    {
        if (!PhotonNetwork.InLobby)
            return;

        //refresh our cached list
        PhotonNetwork.FindFriends(m_InitPlayers);

        if (m_Friends.Count > 0)
        {
            m_InitPlayers = new string[m_Friends.Count];

            for (int i = 0; i < m_Friends.Count; i++)
            {
                FriendInfo friend = m_Friends[i];

                m_InitPlayers[i] = friend.UserId;

                for (int x = 0; x < m_FriendListParent.childCount; x++)
                {
                    ChallegePlayerDataStore c = m_FriendListParent.GetChild(x).GetComponentInChildren<ChallegePlayerDataStore>();
                    if (c.userId == friend.UserId)
                    {
                        c.isOnline = friend.IsInRoom || friend.IsOnline;
                        c.OnlineIndicatorImage.color = friend.IsInRoom ? Color.yellow : friend.IsOnline ? Color.green : Color.red;
                        break;
                    }
                }
            }
        }
    }
}
