/////////////////////////////////////////////////////////////////////////////////
//
//  FST_MainChat.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	The base class used for main chat within lobby, and "under 
//                  hood" operations throughout the game. Freinds play is largely
//                  based within this script
//
/////////////////////////////////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Jiweman;
using FastSkillTeam;

public class FST_MainChat : MonoBehaviour, IChatClientListener
{
    public static FST_MainChat Instance;

    //cache of last active PWF challenge players
    private string[] m_TempPWFlog = new string[2];

    //main chat setup reqs
    private readonly ConnectionProtocol m_ConnectProtocol = ConnectionProtocol.Udp;
    private ChatClient m_ChatClient;

    //base friends channel
    private const string k_FriendsChannel = "friends";
    private const string k_GlobalChannel = "Global";

    #region MONOBEHAVIOUR CALLBACKS
    private void OnEnable()
    {
        Instance = this;
    }

    void OnApplicationQuit()
    {
        Disconnect();
    }

    void Update()
    {
        if (m_ChatClient != null)
        {
            m_ChatClient.Service();
        }

        UpdateConnectionState();
    }
    #endregion

    public void Disconnect()
    {
        if (m_ChatClient != null) { m_ChatClient.Disconnect(); }
    }

    public float LogOnTimeOut = 30f;// if a stage in the initial connection process stalls for more than this many seconds, the connection will be restarted
    [Tooltip("after this many connection attempts, the script will abort and return to main menu, 0 = unlimited")]
    public int MaxConnectionAttempts = 0;// after this many connection attempts, the script will abort, 0 = unlimited
    protected int m_ConnectionAttempts = 0;//tracking
    public static bool StayConnected = false;
    protected ChatState m_LastChatState = ChatState.Disconnected;
    protected FST_Timer.Handle m_ConnectionTimer = new FST_Timer.Handle();
    /// <summary>
    ///	detects cases where the connection process has stalled,
    ///	disconnects and tries to connect again
    /// </summary>
    protected virtual void UpdateConnectionState()
    {
        if (FST_MPConnection.InternetReachability == NetworkReachability.NotReachable)
            return;

        if (!StayConnected)
            return;

        if (m_ChatClient.State != m_LastChatState)
        {
            string s = "Chat State-" + m_ChatClient.State.ToString();
            s = ((m_ChatClient.State == ChatState.ConnectedToFrontEnd) ? "--- " + s + " ---" : s);
            if (s == "ConnectingToFrontEnd")
                s = "Connecting to chat ...";
            Debug.Log(s);
            FST_MPDebug.Log(s);
        }

        Connected = m_ChatClient.CanChat;

        if (Connected)
        {
            if (m_ConnectionTimer.Active)
            {
                Debug.Log("Chat -Reset Connection Timer");
                m_ConnectionTimer.Cancel();
            }
        }
        else if ((m_ChatClient.State != m_LastChatState) && !m_ConnectionTimer.Active)
        {
            Reconnect();
            FST_Timer.In(LogOnTimeOut, delegate ()
            {
                m_ConnectionAttempts++;
                if (m_ConnectionAttempts < MaxConnectionAttempts || MaxConnectionAttempts == 0)
                {
                    Debug.Log("Chat -Retrying (" + m_ConnectionAttempts + ") ...");
                    FST_MPDebug.Log("Chat -Retrying (" + m_ConnectionAttempts + ") ...");
                    Reconnect();
                }
                else
                {
                    Debug.Log("Chat -Failed to connect (tried " + m_ConnectionAttempts + " times).");
                    FST_MPDebug.Log("Chat -Failed to connect (tried " + m_ConnectionAttempts + " times).");
                    Disconnect();
                }
            }, m_ConnectionTimer);
        }

        m_LastChatState = m_ChatClient.State;
    }
    public static bool Connected = false;

    /// <summary>
    /// used internally to disconnect and immediately reconnect
    /// </summary>
    protected virtual void Reconnect()
    {
        StayConnected = true;
        Debug.Log("Chat -Reconnect()");
        if (m_ChatClient.State != ChatState.Disconnected
            && m_ChatClient.State != ChatState.ConnectingToFrontEnd)
        {
            Debug.Log("Chat -Reconnect() > Disconnecting before Connect()");
            Disconnect();
        }

        Connect();
    }

    public void Connect()
    {
        StayConnected = true;
        //create our chatclient
        m_ChatClient = new ChatClient(this, m_ConnectProtocol);
        //m_ChatClient.UseBackgroundWorkerForSending = true;
        // if we like we can set our region..
        // chatClient.ChatRegion = "US";

        //use our custom authValues
        AuthenticationValues authValues = new AuthenticationValues
        {
            UserId = Joga_Data.PlayerID,//Photon.Pun.PhotonNetwork.AuthValues.UserId; 
            AuthType = CustomAuthenticationType.None
        };

        //connect
        m_ChatClient.Connect("ae8e0652-b935-4bf8-aaf2-6ea5cf914ba4", Application.version, authValues);
    }

    /// <summary>
    /// Transmits a message over the global chat
    /// </summary>
    /// <param name="message">the message to send</param>
    /// <param name="channel">the channel to send on : Default is friends channel </param>
    public void Send(string message, string channel = k_FriendsChannel)
    {
        if (Check(message, channel))
            m_ChatClient.PublishMessage(channel, message);
    }
    /// <summary>
    /// We perform actions we need upon connection here
    /// </summary>
    public void OnConnected()
    {
      //  Debug.Log("Lobby chat Connected");

        m_ChatClient.Subscribe(new string[] { k_FriendsChannel, k_GlobalChannel }); //subscribe to chat channel once connected to server, not needed for privates, for now we will have "friends chat" on hand if need be
    }

    public void OnDisconnected()
    {
      //  Debug.Log("Lobby chat Disconnected");
        channelCount = 0;
    }
    /// <summary>
    /// sends a PWF match request to a friend
    /// </summary>
    /// <param name="receiverId">the challenged friends player ID</param>
    public void SendPWFRequestTo(string receiverId)
    {
        if (CMD_SendMatchRequest(receiverId, out string r))
        {
            //log this request, we use the data for accept/decline + making rooms
            LogPWFRequest(Joga_Data.PlayerID, receiverId);

            if (!Check(r, /*k_FriendsChannel*/"*"+ receiverId))
                return;

            //TEST THIS FURTHER : USE PRIVATE CHANNELS
            m_ChatClient.SendPrivateMessage(receiverId, r);
           // m_ChatClient.PublishMessage(k_FriendsChannel, r);
        }
    }

    public void StartPWFChallenge()
    {
        m_ChatClient.SendPrivateMessage(m_TempPWFlog[1], "StartMatch");
    }

    /// <summary>
    /// Declines a PWF match request by sending the player that sent the original request a decline reply
    /// </summary>
    public void DeclinePWFRequest()
    {
        if (!Check(CMD_Send_DeclineMatchRequest(), /*k_FriendsChannel*/"*" + m_TempPWFlog[0]))
            return;

        //TEST THIS FURTHER : USE PRIVATE CHANNELS
         m_ChatClient.SendPrivateMessage(m_TempPWFlog[0], CMD_Send_DeclineMatchRequest());
      //  m_ChatClient.PublishMessage(k_FriendsChannel, CMD_Send_DeclineMatchRequest());
    }
    /// <summary>
    /// Accepts a PWF match request by sending the player that sent the original request an accept reply
    /// </summary>
    public void AcceptPWFRequest()
    {
        if(FST_SettingsManager.MatchType != 5)
        {
            Debug.Log("accepted and needed to set matchType to 5");
            FST_SettingsManager.MatchType = 5;
        }

        if (!Check(CMD_Send_AcceptMatchRequest(), /*k_FriendsChannel*/"*" + m_TempPWFlog[0]))
            return;
        //TEST THIS FURTHER
        m_ChatClient.SendPrivateMessage(m_TempPWFlog[0], CMD_Send_AcceptMatchRequest());
        //m_ChatClient.PublishMessage(k_FriendsChannel, CMD_Send_AcceptMatchRequest());
    }
    /// <summary>
    /// returns true if sendable, else will cache for later when connected and return false
    /// </summary>
    /// <param name="mssg">the message that we wish to send</param>
    /// <returns></returns>
    private bool Check(string mssg, string channel)
    {
/*        bool pvt = channel.Contains("*");

        if (pvt)//strip key for the can chat check
            channel = channel.Remove(0);*/

        if (!m_ChatClient.CanChat/*CanChatInChannel(channel)*/)
        {
/*            if (pvt)//add key back so its stored and sent properly
                channel = "*" + channel;*/

            if (!mssgCache.Contains(new string[] { mssg, channel }))
            {
                mssgCache.Add(new string[] { mssg, channel });
            }
            return false;
        }
        return true;
    }

    private void SendCachedCommands()
    {
        for (int i = 0; i < mssgCache.Count; i++)
        {
            if (mssgCache[i][1].StartsWith("*"))
            {
                string playerId = mssgCache[i][1].Remove(0);
                m_ChatClient.SendPrivateMessage(playerId, mssgCache[i][0]);
                continue;
            }
            //TEST THIS FURTHER
            // m_ChatClient.SendPrivateMessage(m_TempPWFlog[0], CMD_Send_AcceptMatchRequest());
            m_ChatClient.PublishMessage(mssgCache[i][1], mssgCache[i][0]);
        }

        mssgCache.Clear();

      //  Debug.Log("Sent and cleared message cache...");
    }

    private List<string[]> mssgCache = new List<string[]>();

    /// <summary>
    /// returns true if the reciever ID is valid
    /// </summary>
    /// <param name="recieverID">the player we want to challenge</param>
    /// <param name="result">the command required to send the request</param>
    /// <returns></returns>
    private bool CMD_SendMatchRequest (string recieverID, out string result)
    {
        result = "";
        if (string.IsNullOrEmpty(recieverID))
            return false;
        result = "pwfReq/" + recieverID;
        return true;
    }

    private string CMD_Send_AcceptMatchRequest()
    {
        return "pwfAcc/" + Joga_Data.PlayerID;
    }
    private string CMD_Send_DeclineMatchRequest()
    {
        return "pwfDec/" + Joga_Data.PlayerID;
    }
    private string CMD_Reply_AcceptPWFMatchRequest (string sender)
    {
        return "pwfAcc/" + sender;
    }
    private string CMD_Reply_DeclinedPWFMatchRequest(string sender)
    {
        return "pwfDec/" + sender;
    }
    public void LogPWFRequest(string player1, string player2)
    {
        FST_MPDebug.Log("LogPWFRequest player1: " + player1 + " -- player2:" + player2);
        Debug.Log("LogPWFRequest player1: " + player1 + " -- player2:" + player2);

        m_TempPWFlog[0] = player1;
        m_TempPWFlog[1] = player2;
    }

    private string IncomingPWFMatchRequest { get { return "pwfReq/" + Joga_Data.PlayerID; } }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {

        Debug.Log(channelName);
        int msgCount = messages.Length;
        if (channelName == k_GlobalChannel)
        {
            if (!FST_MainChatInput.Instance)
                return;

            for (int i = 0; i < msgCount; i++)
                FST_MainChatInput.Instance.AddChatMessage((string)messages[i], senders[i]== Joga_Data.PlayerID? FST_MainChatInput.MessageType.player : FST_MainChatInput.MessageType.remote, true);

            return;
        }

        for (int i = 0; i < msgCount; i++)
        { 
            //go through each received msg
            string sender = senders[i];
            string msg = (string)messages[i];
            Debug.Log(sender + " : " + msg);
        }
    }
    public static bool blockChallenge;
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        string msg = (string)message;

        Debug.Log(sender + " : " + msg);


        if (msg == IncomingPWFMatchRequest)
        {
            if (!blockChallenge)
            {//cache this request, this is a replica of what we want when player send this request, so sender is first, we use the data for accept/decline + making rooms
                LogPWFRequest(sender, Joga_Data.PlayerID);
                UIController.Instance.ShowReceiveChallengePopup(true);
                Debug.Log("GOT PUN PRIVATE FRIENDS REQUEST");
            }
            else blockChallenge = false;
        }
        if (msg =="StartMatch")
        {
            if (Joga_Data.PlayerID == sender)
            { }
            else
            {
                UIController.Instance.ChallengeHasSent.SetActive(false);
                FST_MPConnection.Instance.TryPlayWithFriends(m_TempPWFlog);
            }

            Debug.Log("GOT PUN PRIVATE FRIENDS REQUEST REPLY : MATCH START!");
        }
        //NOTE: here, we check both players. TryPlayWithFriends() makes the decision on who does what with the log info
        if (msg == CMD_Reply_AcceptPWFMatchRequest(m_TempPWFlog[0]) || msg == CMD_Reply_AcceptPWFMatchRequest(m_TempPWFlog[1]))
        {
            if (Joga_Data.PlayerID == sender)
            {
                UIController.Instance.ChallengeHasSent.SetActive(true);
            }
            else
            {
                UIController.Instance.ChallengeHasSent.SetActive(false);
                UIController.Instance.ChallengeAcceptedPopup.SetActive(true);
            }
            Debug.Log("GOT PUN PRIVATE FRIENDS REQUEST REPLY : Accepted!");
        }
        if (msg == CMD_Reply_DeclinedPWFMatchRequest(m_TempPWFlog[1]))
        {
            Debug.Log("GOT PUN PRIVATE FRIENDS REQUEST REPLY : Declined!");
            UIController.Instance.IsChallengeActive = false;
            if (ChallegePlayerDataStore.Instance)
            {
                ChallegePlayerDataStore.Instance.ChallengeSentDeActive();
                if (Joga_Data.PlayerID == m_TempPWFlog[0])
                {
                    UIController.Instance.CantplayPopup.SetActive(true);
                    SSTools.ShowMessage("Player declined the match!", SSTools.Position.bottom, SSTools.Time.threeSecond);
                }
            }
            UIController.Instance.ChallengeHasSent.SetActive(false);
            UIController.Instance.ChallengeAcceptedPopup.SetActive(false);
        }

    }
    public void DebugReturn(DebugLevel level, string message)
    {
        FST_MPDebug.Log("FST_MainChat.cs > DebugReturn:" + message);
        Debug.Log("FST_MainChat.cs > DebugReturn:" + message);
    }
    private int channelCount = 0;
    public void OnSubscribed(string[] channels, bool[] results)
    {
        for (int i = 0; i < channels.Length; i++)
        {
            channelCount++;
           // Debug.Log("Subscribed to a new channel! : " + channels[i]);
        }

      //  Debug.Log("channelCount : " + channelCount);

        if (channelCount <= 1)
            return;

        FST_MainChatInput.Instance?.Initialize();

        SendCachedCommands();
    }
    public void OnUserSubscribed(string channel, string user)
    {
       // Debug.Log("user : " + user + " Subscribed to channel : " + channel);
    }
    public void OnChatStateChange(ChatState state)
    {
        FST_MPDebug.Log("Chat state: " + state);
        Debug.Log("Chat state: " + state);

        if(state == ChatState.Disconnected)
        {
            Connect();
        }
    }
    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        // throw new System.NotImplementedException();
    }

    public void OnUnsubscribed(string[] channels)
    {
        channelCount--;
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
        // throw new System.NotImplementedException();
    }
}
