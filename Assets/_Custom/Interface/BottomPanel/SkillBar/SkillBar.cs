using System;
using UnityEngine;

public class SkillBar : MonoBehaviour
{
    //skills that are equipped to the skill panel
    public SkillSO[] skillSOs = new SkillSO[8];
    public float[] skillTimer = new float[8];

    //references
    SkillBook skillBook;
    Container container;
    Interactable focus;
    public ChatPanel chatPanel;
    public CombatLog combatLog;
    Equipment equipment;
    Animator animator;

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
        equipment = GetComponent<Equipment>();
        animator = GetComponentInChildren<Animator>();
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
        targetCharacterStats = myCharacterFocus.target.GetComponent<CharacterStats>(); //get target characterstats

        //check for target null
        if (this.GetComponent<CharacterFocus>().target == null)
        {
            ChatLogMessage("No target selected.");
            autoattackOn = false;
            return;
        }

        //dead check
        if (myCharacterStats.dead)
        {
            ChatLogMessage("You are dead and cannot use skills.");
            autoattackOn = false;
            return;
        }

        //target dead check
        if (targetCharacterStats.dead)
        {
            ChatLogMessage("Target is dead.");
            autoattackOn = false;
            return;
        }
        
        //null check
        if (skillSOs[slotNumber] == null)
        {
            ChatLogMessage("No skill assigned to this slot.");
            return;
        }

        //range check
        float distanceToTarget = Vector3.Distance(this.transform.position, myCharacterFocus.target.transform.position);
        if (distanceToTarget > skillSOs[slotNumber].attackRange)
        {
            ChatLogMessage("Target is out of range for this skill.");
            return;
        }

        //cooldown check
        if (timer > 0) //skill is on cooldown but not autoattack
        {
            if (slotNumber == 0)
            {
                //ChatLogMessage("Auto Attack is on cooldown.");
                return;
            }
            else
            {
                ChatLogMessage("Skill is on cooldown.");
                return;
            }
        }

        skillTimer[slotNumber] = CoolDownTimer(skillSOs[slotNumber].cooldownTime);

        //stamina check
        if (myCharacterStats.currentStamina < skillSOs[slotNumber].staminaCost)
        {
            ChatLogMessage("Not enough stamina to use this skill.");
            return;
        }

        //use skill
        if (myCharacterFocus != null && myCharacterFocus.target != null)
        {
            
            if (targetCharacterStats != null) //doing the skill stuff
            {
                //subtract stamina cost from me
                myCharacterStats.SubtractStamina(skillSOs[slotNumber].staminaCost);

                //check if attack hits (attack roll) against target armor class
                int attackRoll = myCharacterStats.AttackRoll();

                //skill damage plus weapon damage
                if (equipment.weaponSOs[0] == null)
                {
                    ChatLogMessage("No weapon equipped.");
                    return;
                }

                //calculate total damage
                if (attackRoll >= targetCharacterStats.armorClass)
                {
                    float weaponDamage = equipment.weaponSOs[0].Damage;
                    float skillDamage = UnityEngine.Random.Range(1, skillSOs[slotNumber].targetDamage);
                    float strengthModifier = myCharacterStats.strengthModifier;
                    //total
                    float damage = strengthModifier + weaponDamage + skillDamage;
                    Debug.Log("Character: " + myCharacterStats.interactableName + " Weapon: " + weaponDamage + " Skill: " + skillDamage + " Strength: " + strengthModifier + " Total Damage: " + damage);

                    //apply health damage to target
                    targetCharacterStats.SubtractHealth(damage);

                    CombatLogMessage(true, this.GetComponent<Interactable>(), myCharacterFocus.target.GetComponent<Interactable>(), (int)damage);
                }
                else
                {
                    //missed attack
                    CombatLogMessage(false, this.GetComponent<Interactable>(), myCharacterFocus.target.GetComponent<Interactable>(), 0);
                }

                animator.SetTrigger("Attack");
            }

            //add aggressor to target hate list
            targetHateManager = myCharacterFocus.target.GetComponent<HateManager>(); //get target hate manager
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
        // Determine which combat log to use
        CombatLog logToUse = null;

        // If this is a player (has a combat log), use it
        if (combatLog != null)
        {
            logToUse = combatLog;
        }
        // If this is an NPC, send to the target's combat log
        else
        {
            var targetSkillBar = target.GetComponent<SkillBar>();
            if (targetSkillBar != null && targetSkillBar.combatLog != null) //my target has a combat log (is a player)
            {
                logToUse = targetSkillBar.combatLog; //send to target combat log of player
            }
        }

        // If no combat log available, return
        if (logToUse == null)
            return;

        if (hit)
        {
            logToUse.SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName
                + " deals " + damage + " damage to "
                + target.GetComponent<CharacterStats>().interactableName + ".",
                CombatMessage.CombatMessageType.playerAttack);
        }
        else
        {
            logToUse.SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName
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
