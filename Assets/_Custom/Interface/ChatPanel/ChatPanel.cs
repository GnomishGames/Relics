using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChatPanel : MonoBehaviour
{
    public int maxMessages = 25;

    public GameObject chatLogPanel, textObject;
    public Color playerMessage, info;
    public TMP_InputField chatBox;

    public CharacterStats characterStats;

    [SerializeField] List<ChatMessage> chatMessageList = new List<ChatMessage>();
    void Awake()
    {
        //characterStats = GetComponent<CharacterStats>();
    }

    void Update()
    {
        if (chatBox.text != "")
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SendMessageToChatLog(characterStats.interactableName + ": " + chatBox.text, ChatMessage.ChatMessageType.playerMessage);
                chatBox.text = "";
            }
        }
        else
        {
            if (!chatBox.isFocused && Input.GetKeyDown(KeyCode.Slash))
                chatBox.ActivateInputField();
        }
        if (!chatBox.isFocused)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //    SendMessageToChatLog("You pressed spacebar!", ChatMessage.ChatMessageType.info);
        }
    }

    public void SendMessageToChatLog(string text, ChatMessage.ChatMessageType chatMessageType)
    {
        if (chatMessageList.Count >= maxMessages)
        {
            Destroy(chatMessageList[0].textObject.gameObject);
            chatMessageList.Remove(chatMessageList[0]);
        }

        ChatMessage newMessage = new ChatMessage();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatLogPanel.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(chatMessageType);

        chatMessageList.Add(newMessage);
    }

    Color MessageTypeColor(ChatMessage.ChatMessageType chatMessageType)
    {
        Color color = info;

        switch (chatMessageType)
        {
            case ChatMessage.ChatMessageType.playerMessage:
                color = playerMessage;
                break;
            case ChatMessage.ChatMessageType.info:
                color = info;
                break;
            default:
                break;
        }
        return color;
    }
}


[System.Serializable]
public class ChatMessage
{
    public string text;
    public TMP_Text textObject;
    public ChatMessageType chatMessageType;

    public enum ChatMessageType
    {
        playerMessage,
        info
    }
}
