using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CombatLog : MonoBehaviour
{
    public int maxMessages = 20;

    public GameObject combatLogContent, textObject;
    public Color info, npcAttack, playerAttack;

    [SerializeField] List<CombatMessage> combatMessageList = new List<CombatMessage>();

    public void SendMessageToCombatLog(string text, CombatMessage.CombatMessageType combatMessageType)
    {
        if (combatMessageList.Count >= maxMessages)
        {
            Destroy(combatMessageList[0].textObject.gameObject);
            combatMessageList.Remove(combatMessageList[0]);
        }

        CombatMessage newMessage = new CombatMessage();

        newMessage.text = text;

        GameObject newText = Instantiate(textObject, combatLogContent.transform);

        newMessage.textObject = newText.GetComponent<TextMeshProUGUI>();

        newMessage.textObject.text = newMessage.text;
        newMessage.textObject.color = MessageTypeColor(combatMessageType);

        combatMessageList.Add(newMessage);
    }
    
    Color MessageTypeColor(CombatMessage.CombatMessageType combatMessageType)
    {
        Color color = info;

        switch (combatMessageType)
        {
            case CombatMessage.CombatMessageType.playerAttack:
                color = playerAttack;
                break;
            case CombatMessage.CombatMessageType.npcAttack:
                color = npcAttack;
                break;
            case CombatMessage.CombatMessageType.info:
                color = info;
                break;
            default:
                break;
        }
        return color;
    }
}

[System.Serializable]
public class CombatMessage
{
    public string text;
    public TMP_Text textObject;
    public CombatMessageType combatMessageType;

    public enum CombatMessageType
    {
        playerAttack,
        npcAttack,
        info
    }
}