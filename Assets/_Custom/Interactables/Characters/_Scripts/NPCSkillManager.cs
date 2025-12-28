using UnityEngine;

public class NPCSkillManager : MonoBehaviour
{
    //reference to skillbar
    SkillBar skillBar;

    //referecne to character focus to get target
    HateManager hateManager;
    CharacterFocus characterFocus;

    void Start()
    {
        skillBar = GetComponent<SkillBar>();
        hateManager = GetComponent<HateManager>();
        characterFocus = GetComponent<CharacterFocus>();
    }

    //choose random skill from skillbar if the skill is not null
    public SkillSO ChooseRandomSkill()
    {
        SkillSO chosenSkill = null;
        while (chosenSkill == null)
        {
            int randomIndex = Random.Range(0, skillBar.skillSOs.Length);
            chosenSkill = skillBar.skillSOs[randomIndex];
        }
        return chosenSkill;
    }

    //execute skill from skillbar.cs based on chooseRandomSkill
    public void ExecuteRandomSkill()
    {
        //check for a target
        var target = characterFocus.target;
        if (target == null) return;

        //check distance to target
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget > ChooseRandomSkill().attackRange) return;

        //execute skill
        SkillSO skillToUse = ChooseRandomSkill();
        int skillIndex = System.Array.IndexOf(skillBar.skillSOs, skillToUse);
        skillBar.DoSkill(skillIndex, skillBar.skillTimer[skillIndex]);
    }

    private static void CombatLogMessage(bool hit, Interactable interactable, Interactable target, int damage)
    {
        if (target.GetComponent<CombatLog>())
        {
            if (hit)
            {
                target.GetComponent<CombatLog>().SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName +
                    " deals " + damage + " damage to " +
                    target.GetComponent<CharacterStats>().interactableName + ".",
                    CombatMessage.CombatMessageType.npcAttack);
            }
            else
            {
                target.GetComponent<CombatLog>().SendMessageToCombatLog(interactable.GetComponent<CharacterStats>().interactableName
                    + " attacks but misses "
                    + target.GetComponent<CharacterStats>().interactableName + ".",
                    CombatMessage.CombatMessageType.npcAttack);
            }
        }
    }
}
