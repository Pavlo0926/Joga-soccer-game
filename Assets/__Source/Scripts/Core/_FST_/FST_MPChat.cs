/////////////////////////////////////////////////////////////////////////////////
//
//  FST_MPChat.cs
//  @ FastSkillTeam Productions. All Rights Reserved.
//  http://www.fastskillteam.com/
//  https://twitter.com/FastSkillTeam
//  https://www.facebook.com/FastSkillTeam
//
//	Description:	a simple chat system for basic multiplayer testing
//
/////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;

[RequireComponent(typeof(PhotonView))]

public class FST_MPChat : MonoBehaviourPun
{

    // adjustable settings
    public int MaxMessages = 100;
    public int MaxMessageLength = 100;
    public AudioClip ChatSound = null;
    public AudioClip ChatErrorSound = null;
    public MessageFilterBehavior FilterMode = MessageFilterBehavior.Alert;

    // layout
    protected float m_LineHeight = 16.0f;
    protected int m_Padding = 2;

    // work variables
    protected int m_LastTextLength = 0;
    protected int m_FocusControlAttempts = 0;
    protected float m_LastScreenWidth = 0.0f;
    protected float m_LastScreenHeight = 0.0f;
    protected Vector2 m_ScrollPosition = Vector2.zero;
    protected bool m_HaveSkin = false;
    protected bool m_TextInputVisible = false;
    protected List<int> m_MutedPlayers = new List<int>();

    // input rects
    protected Rect[] m_OriginalMouseCursorZones;
    protected Rect[] m_TextInputMouseCursorZones;

    // gui rects
    protected Rect m_ViewRect = new Rect();
    protected Rect m_TextFieldRect = new Rect();
    protected Rect m_SendButtonRect = new Rect();
    protected Rect m_ScrollbarRect = new Rect();

    // messages
    protected string m_InputLine = "";
    protected List<string> m_Messages = new List<string>();

    public enum MessageFilterBehavior
    {
        Alert,  // play a sound to alert the sender that this message was blocked
        Silent  // have it appear as if the message was sent, but only actually show it on the sender's machine
    }


    /// <summary>
    /// this delegate verifies that a message sender is currently allowed
    /// to post chat messages, and looks for forbidden strings in a message.
    /// can be used for player muting and swear word check, respectively
    /// </summary>
    protected System.Func<int, string, bool> MessageOK = delegate (int senderID, string s)
    {
        // you can implement muting of players by filling the 'mutedplayers'
        // list with id:s from a gui control
        if (m_Chat != null && m_Chat.m_MutedPlayers.Contains(senderID))
            return false;

        // you can prevent undesired words from displaying by returning false here, for example ...
        if (s.Contains("jar jar binks") || s.Contains("f00k"))
            return false;
        // ... although this should be done much more efficiently (use a dictionary)

        return true;
    };

    // --- properties ---

    protected bool m_Visible = true;
    public bool Visible
    {
        get
        {
            return m_Visible;
        }
        set
        {
            m_Visible = value;
            if (!m_Visible)
                HideTextInput();
        }
    }

    protected float ViewRectHeight
    {
        get
        {
            return (Screen.height * 0.33f);
        }
    }

    protected float LogPixelHeight
    {
        get
        {
            return (m_Messages.Count * m_LineHeight) + m_Padding;
        }
    }

    protected bool IsViewAtBottom
    {
        get
        {
            return Mathf.Abs(((int)m_ScrollPosition.y) - (int)(LogPixelHeight - ViewRectHeight)) < m_LineHeight;
        }
    }

    // --- expected components ---

    protected static FST_MPChat m_Chat = null;

    protected AudioSource m_AudioSource = null;
    protected AudioSource AudioSource
    {
        get
        {
            if (GetComponent<AudioSource>() != null)
                m_AudioSource = GetComponent<AudioSource>();
            else
                m_AudioSource = gameObject.AddComponent<AudioSource>();

            return m_AudioSource;
        }
    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void Start()
    {

        RefreshRects();
        SnapToBottom();

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnEnable()
    {

        FST_GlobalEvent<string, bool>.Register("ChatMessage", AddMessage);
        FST_GlobalEvent.Register("ClearChat", Clear);

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnDisable()
    {

        FST_GlobalEvent<string, bool>.Unregister("ChatMessage", AddMessage);
        FST_GlobalEvent.Unregister("ClearChat", Clear);

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnGUI()
    {

        if (!Visible)
            return;

        GUI.depth = 0;

        if (!m_HaveSkin)
            if (GUI.skin == true)
                m_HaveSkin = true;


        // the 'OnGUI' system has a lot of lore when it comes to focus and input.
        // this code tries to focus the text input box when user presses enter
/*
        if (Event.current.type == EventType.KeyDown
            && (Event.current.keyCode == KeyCode.KeypadEnter || Event.current.keyCode == KeyCode.Return))
        {
            if (Visible)
            {
                if (!m_TextInputVisible)
                {
                    ShowTextInput(true);
                    m_FocusControlAttempts = 2;
                }
                else
                    Send();
            }
        }
        else if (Event.current.type == EventType.KeyDown && (Event.current.keyCode == KeyCode.Escape))
        {
            HideTextInput();
        }

        if (m_FocusControlAttempts > 0)
        {
            GUI.FocusControl("TextInput");
            m_FocusControlAttempts--;
        }*/

        // make scrollview scrollbar invisible because it's on the wrong (right) side
        GUI.color = Color.clear;
        GUI.BeginScrollView(m_ViewRect, m_ScrollPosition, new Rect(0, 0, m_ViewRect.width - m_LineHeight, LogPixelHeight));
        for (int v = 0; v < m_Messages.Count; v++)
        {
            GUI.color = Color.white;
            GUI.Label(new Rect(0, v * m_LineHeight, Screen.width * 0.5f, m_LineHeight + 8), m_Messages[v], TextStyle);
        }
        GUI.EndScrollView();

        // draw our own scrollbar on the left side
        if ((ViewRectHeight < LogPixelHeight))
            m_ScrollPosition.y = GUI.VerticalScrollbar(m_ScrollbarRect, m_ScrollPosition.y, (Screen.height * 0.33f), 0, LogPixelHeight);

        if (
            m_LastScreenWidth != Screen.width
            || m_LastScreenHeight != Screen.height
            )
        {
            SnapToBottom();
            RefreshRects();
        }

        m_LastScreenWidth = Screen.width;
        m_LastScreenHeight = Screen.height;

        if (m_TextInputVisible)
        {
            GUI.SetNextControlName("TextInput");
            m_InputLine = GUI.TextField(m_TextFieldRect, m_InputLine, MaxMessageLength);
            if (m_LastTextLength != m_InputLine.Length && m_InputLine.Length == MaxMessageLength)
            {
                m_InputLine = m_InputLine.Remove(m_InputLine.Length - 1);
                PlaySound(m_Chat.ChatErrorSound);
            }
            m_LastTextLength = m_InputLine.Length;
        }

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void PlaySound(AudioClip sound)
    {

        if ((sound == null))
            return;

        if ((m_Chat == null))
            return;

        if ((m_Chat.AudioSource == null))
            return;

        if (m_Chat.AudioSource.isPlaying)
            return;

        m_Chat.AudioSource.clip = sound;
        m_Chat.AudioSource.Play();

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void Send()
    {

        if (!string.IsNullOrEmpty(m_InputLine))
        {
            if (MessageOK(-1, m_InputLine))
                AddMessage(m_InputLine);
            else
            {
                switch (FilterMode)
                {
                    case MessageFilterBehavior.Alert:
                        PlaySound(m_Chat.ChatErrorSound);
                        break;
                    case MessageFilterBehavior.Silent:
                        AddChatMessage(GetFormattedPlayerName(PhotonNetwork.LocalPlayer.ActorNumber) + m_InputLine, new PhotonMessageInfo(PhotonNetwork.LocalPlayer, PhotonNetwork.ServerTimestamp, null));
                        PlaySound(m_Chat.ChatSound);
                        break;
                }
            }
            SnapToBottom();
        }
        m_InputLine = "";
        GUI.FocusControl("");
        ShowTextInput(false);

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void SnapToBottom()
    {
        m_ScrollPosition.y = LogPixelHeight + 1000;
    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void HideTextInput()
    {

        GUI.FocusControl("");
        ShowTextInput(false);

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void ShowTextInput(bool isEnabled = true)
    {

        m_TextInputVisible = isEnabled;

    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual string GetFormattedPlayerName(int playerID)
    {
        for (int p = 0; p < PhotonNetwork.PlayerList.Length; p++)
        {
            if (PhotonNetwork.PlayerList[p].ActorNumber == playerID)
            {
                if (PhotonNetwork.PlayerList[p].NickName == "Player")
                    return "Player " + playerID.ToString();
                else
                    return "[" + PhotonNetwork.PlayerList[p].NickName + "] ";
            }
        }

        return "Unknown";
    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void RefreshRects()
    {

        m_ViewRect = new Rect(16, 0, Screen.width * 0.5f, Screen.height * 0.33f);
        m_ScrollbarRect = new Rect(0, 0, 16, Screen.height * 0.33f);
        m_TextFieldRect = m_ViewRect;
        m_TextFieldRect.x = 0;
        m_TextFieldRect.y = m_TextFieldRect.height + m_Padding;
        m_TextFieldRect.height = m_LineHeight * 1.5f;
        m_SendButtonRect = m_TextFieldRect;
        m_SendButtonRect.x = m_SendButtonRect.width + m_Padding;
        m_SendButtonRect.width = 50;

    }

    /// <summary>
    /// 
    /// </summary>
    public static void AddMessage(string message, bool broadcast = true)
    {

        if (m_Chat == null)
            m_Chat = (FST_MPChat)Component.FindObjectOfType(typeof(FST_MPChat));
        if (m_Chat != null)
        {
            if (broadcast && (PhotonNetwork.NetworkClientState == ClientState.Joined))
                m_Chat.photonView.RPC("AddChatMessage", RpcTarget.AllBuffered, message);
            else if(!broadcast)
                FST_MainChatInput.Instance.AddChatMessage(message, FST_MainChatInput.MessageType.debug, false);
            else FST_MainChatInput.Instance.AddChatMessage(message, FST_MainChatInput.MessageType.player, false);
            //  m_Chat.AddChatMessage(message, new PhotonMessageInfo(PhotonNetwork.LocalPlayer, PhotonNetwork.ServerTimestamp, null));
        }

    }


    /// <summary>
    /// 
    /// </summary>
    public void Clear()
    {
        m_Messages.Clear();
    }


    /// <summary>
    /// 
    /// </summary>
    [PunRPC]
    void AddChatMessage(string message, PhotonMessageInfo info)
    {
        FST_MainChatInput.Instance.AddChatMessage(message, info.Sender.UserId == PhotonNetwork.LocalPlayer.UserId ? FST_MainChatInput.MessageType.player : FST_MainChatInput.MessageType.remote, false);
        //return;
        //if (message == PhotonNetwork.LocalPlayer.UserId)
        //    Debug.Log("GOT SILENT MESSAGE : " + message);


        //bool shouldSnapToBottom = IsViewAtBottom;

        //if (ViewRectHeight > LogPixelHeight)
        //    shouldSnapToBottom = true;

        //string prefix = "";
        //if (info.Sender != null)
        //{

        //    // all player-sent messages that fail the message filter are silently ignored
        //    // TODO: we can also easily implement an ignore-by-player-ID feature here
        //    if (!MessageOK(info.Sender.ActorNumber, message))
        //        return;
        //    prefix = GetFormattedPlayerName(info.Sender.ActorNumber);
        //    if (message.Length > MaxMessageLength)
        //        message = message.Remove(MaxMessageLength);
        //    PlaySound(m_Chat.ChatSound);
        //}

        //while (message.Length > 0)
        //{
        //    string s = ((string.IsNullOrEmpty(prefix)) ? "" : prefix);
        //    if (m_HaveSkin)
        //    {
        //        // TODO: make cutoff point before / after whole words
        //        while ((message.Length > 0) && TextStyle.CalcSize(new GUIContent(s)).x < m_ViewRect.width - 16)
        //        {
        //            s += message[0];
        //            message = message.Substring(1);
        //        }
        //    }
        //    else
        //    {
        //        s = message;
        //        message = "";
        //    }
        //    m_Messages.Add(s);
        //}

        //if (m_Messages.Count > MaxMessages)
        //    m_Messages.Remove(m_Messages[0]);

        //if (shouldSnapToBottom)
        //    SnapToBottom();
    }


    /// <summary>
    /// 
    /// </summary>
    protected virtual void OnJoinedRoom()
    {
        // enabled = false;//edit as we have voice and use this for connection status for now
        FST_Timer.In(1, delegate ()
        {
            FST_MPDebug.Log("Press ENTER to CHAT");
        });

    }

    // -------- GUI styles --------

    protected GUIStyle m_TextStyle = null;  // NOTE: don't use this directly. instead, use its property below
    public GUIStyle TextStyle               // nametag runtime generated GUI style
    {
        get
        {
            if (m_TextStyle == null)
                m_TextStyle = new GUIStyle("Label");
            m_TextStyle.normal.textColor = Color.white;
            return m_TextStyle;
        }
    }


}
