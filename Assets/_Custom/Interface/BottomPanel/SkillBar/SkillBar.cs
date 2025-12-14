using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
public class SkillBar : MonoBehaviour
{
    //skills that are equipped to the skill panel
    public SkillSO[] skillSOs = new SkillSO[8];

    //references
    public SkillBook skillBook;
    Container container;
    Interactable focus;

    private void Awake()
    {
        skillBook = GetComponent<SkillBook>();
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

    public void DoSkill(int slotNumber)
    {
        //null check
        if (skillSOs[slotNumber] == null)
        {
            Debug.LogWarning("No skill assigned to this slot.");
            return;
        }

        //use skill
        this.GetComponent<CharacterStats>().SubtractStamina(skillSOs[slotNumber].staminaCost);
        var cf = this.GetComponent<CharacterFocus>();
        if (cf != null && cf.currentFocus != null)
        {
            var targetStats = cf.currentFocus.GetComponent<CharacterStats>();
            if (targetStats != null)
            {
                targetStats.SubtractHealth(skillSOs[slotNumber].targetDamage);
            }
        }
                
    }
}
