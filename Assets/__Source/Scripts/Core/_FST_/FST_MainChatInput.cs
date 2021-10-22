using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class FST_MainChatInput : MonoBehaviour
{
    public static FST_MainChatInput Instance = null;

    public Button OpenChatButton;
    public Transform ChatContent;
    public Transform ChatContentGame;
    public GameObject TextObject;

    [SerializeField] private int textSize = 16;
    [SerializeField] private InputField m_InputField = null;

   private List<Message> messageList = new List<Message>();
    private List<Message> messageListGame = new List<Message>();

    [Serializable]
    public class Message
    {
        public string text;
        public Text textOb;
    }

    private void OnEnable()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (m_InputField)
            m_InputField.onEndEdit.AddListener((string s) => Send(s));
    }

    private void OnDisable()
    {
        if (m_InputField)
            m_InputField.onEndEdit.RemoveListener((string s) => Send(s));
    }

    private void Send(string mssg)
    {
        if (string.IsNullOrEmpty(mssg))
            return;

        if (!ChatContentGame.gameObject.activeInHierarchy)
            FST_MainChat.Instance.Send(Photon.Pun.PhotonNetwork.NickName + ": " + mssg, "Global");
        else FST_MPChat.AddMessage(Photon.Pun.PhotonNetwork.NickName + ": " + mssg);
        m_InputField.text = "";
    }
    public Color playerMessageColour;
    public Color remoteMessageColour;
    public Color debugMessageColour = Color.red;
    private Color GetChatMessageColor(MessageType messageType)
    {
        Color c = playerMessageColour;

        switch (messageType)
        {

            case MessageType.player:
                break;
            case MessageType.remote:
                c = remoteMessageColour;
                break;
            case MessageType.debug:
                c = debugMessageColour;
                break;
            default:
                break;
        }

        return c;
    }

    public enum MessageType { player, remote, debug }
    public void AddChatMessage(string mssg, MessageType messageType, bool global)
    {
        if (global)
        {
            if (messageList.Count >= 20)
            {
                Destroy(messageList[0].textOb.gameObject);
                messageList.RemoveAt(0);
            }
        }
        else
        {
            if (messageListGame.Count >= 20)
            {
                Destroy(messageListGame[0].textOb.gameObject);
                messageListGame.RemoveAt(0);
            }
        }

        GameObject newText = Instantiate(TextObject, global ? ChatContent : ChatContentGame);

        Message m = new Message { text = mssg, textOb = newText.GetComponent<Text>() };

        m.textOb.color = GetChatMessageColor(messageType);

        m.textOb.text = m.text;
        m.textOb.fontSize = textSize;
        m.textOb.alignment = messageType == MessageType.player ? TextAnchor.UpperRight : TextAnchor.UpperLeft;

        if (global)
            messageList.Add(m);
        else messageListGame.Add(m);
    }

    public void Initialize()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
