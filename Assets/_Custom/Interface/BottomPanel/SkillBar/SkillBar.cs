using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterFocus))]
public class SkillBar : MonoBehaviour
{
    //skills that are equipped to the skill panel
    public SkillSO[] skillSOs = new SkillSO[8];
    public float[] skillTimer = new float[8];

    //references
    public SkillBook skillBook;
    Container container;
    Interactable focus;
    public ChatPanel chatPanel;
    public CombatLog combatLog;

    //vars
    public bool autoattackOn = false;
    CharacterStats myCharacterStats; //get my stats
    CharacterFocus myCharacterFocus; //get my target
    CharacterStats targetCharacterStats; //target stats
    HateManager targetHateManager; //target hate manager

    private void Awake()
    {
        skillBook = GetComponent<SkillBook>();
        myCharacterStats = GetComponent<CharacterStats>();
        myCharacterFocus = GetComponent<CharacterFocus>();
        //chatPanel = GetComponent<ChatPanel>();
    }

    void Update()
    {
        //coldown timers
        for (int i = 0; i < skillTimer.Length; i++)
        {
            if (skillTimer[i] > 0)
            {
                skillTimer[i] -= Time.deltaTime;
                if (skillTimer[i] < 0) skillTimer[i] = 0;
            }
        }
    }

    public void MoveSkill(int from, int to)
    {
        var buffer = skillSOs[to];
        skillSOs[to] = skillSOs[from];
        skillSOs[from] = buffer;
    }

    public void DestroyItem(int from)
    {
        skillSOs[from] = null;
    }

    public void UnEquipSkill(int skillBarSlot, int skillBookSlot)
    {
        if (skillSOs[skillBarSlot] == null)
        {
            var buffer = skillSOs[skillBarSlot];
            skillSOs[skillBarSlot] = skillBook.skillSOs[skillBookSlot];
            skillBook.skillSOs[skillBookSlot] = (SkillSO)buffer;
        }
        else if (skillSOs[skillBarSlot].slotType == skillBook.skillSOs[skillBookSlot].slotType)
        {
            var buffer = skillSOs[skillBarSlot];
            skillSOs[skillBarSlot] = skillBook.skillSOs[skillBookSlot];
            skillBook.skillSOs[skillBookSlot] = (SkillSO)buffer;
        }
    }

    public void DoSkill(int slotNumber, float timer)
    {
        //null check
        if (skillSOs[slotNumber] == null)
        {
            Debug.LogWarning("No skill assigned to this slot.");
            ChatLogMessage("No skill assigned to this slot.");
            return;
        }

        //check for target null
        if (this.GetComponent<CharacterFocus>().currentFocus == null)
        {
            Debug.LogWarning("No target selected.");
            ChatLogMessage("No target selected.");
            return;
        }

        //range check
        float distanceToTarget = Vector3.Distance(this.transform.position, myCharacterFocus.currentFocus.transform.position);
        if (distanceToTarget > skillSOs[slotNumber].attackRange)
        {
            Debug.LogWarning("Target is out of range for this skill.");
            ChatLogMessage("Target is out of range for this skill.");
            return;
        }

        //cooldown check
        if (timer > 0)
        {
            Debug.LogWarning("Skill is on cooldown.");
            ChatLogMessage("Skill is on cooldown.");
            return;
        }

        skillTimer[slotNumber] = CoolDownTimer(skillSOs[slotNumber].cooldownTime);

        //stamina check
        if (myCharacterStats.currentStamina < skillSOs[slotNumber].staminaCost)
        {
            Debug.LogWarning("Not enough stamina to use this skill.");
            ChatLogMessage("Not enough stamina to use this skill.");
            return;
        }

        //use skill
        if (myCharacterFocus != null && myCharacterFocus.currentFocus != null)
        {
            targetCharacterStats = myCharacterFocus.currentFocus.GetComponent<CharacterStats>(); //get target characterstats
            if (targetCharacterStats != null) //doing the skill stuff
            {
                //subtract stamina cost from me
                myCharacterStats.SubtractStamina(skillSOs[slotNumber].staminaCost);

                //apply health damage to target
                targetCharacterStats.SubtractHealth(skillSOs[slotNumber].targetDamage);

                //log combat message
                CombatLogMessage(true, this.GetComponent<Interactable>(), myCharacterFocus.currentFocus.GetComponent<Interactable>(), (int)skillSOs[slotNumber].targetDamage);
            }

            targetHateManager = myCharacterFocus.currentFocus.GetComponent<HateManager>(); //get target hate manager
            //add aggressor to target hate list
            if (targetHateManager != null)
            {
                targetHateManager.AddToHateList(this.GetComponent<Interactable>());
            }
        }
    }

    float CoolDownTimer(float skillTimer)
    {
        //float timer;
        skillTimer -= Time.deltaTime;
        if (skillTimer <= 0)
        {
            skillTimer = 0;
            return skillTimer;
        }
        return skillTimer;
    }

    //logging
    private void CombatLogMessage(bool hit, Interactable interactable, Interactable target, int damage)
    {
        //check for null combat log
        if (combatLog == null)
            return;

        if (hit)
        {
            combatLog.SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName
                + " deals " + damage + " damage to "
                + target.GetComponent<CharacterStats>().interactableName + ".",
                CombatMessage.CombatMessageType.playerAttack);
        }
        else
        {
            combatLog.SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName
                + " attacks but misses "
                + target.GetComponent<CharacterStats>().interactableName + ".",
                CombatMessage.CombatMessageType.playerAttack);
        }
    }

    private void ChatLogMessage(string message)
    {
        if (chatPanel == null)
            return;

        chatPanel.SendMessageToChatLog(message, ChatMessage.ChatMessageType.info);
    }
}
