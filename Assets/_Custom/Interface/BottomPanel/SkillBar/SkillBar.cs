using UnityEngine;

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(CharacterFocus))]
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

        var myCharacterStats = this.GetComponent<CharacterStats>(); //get my stats
        var myCharacterFocus = this.GetComponent<CharacterFocus>(); //get my target
        var targetCharacterStats = myCharacterFocus.currentFocus.GetComponent<CharacterStats>(); //target stats
        var targetHateManager = myCharacterFocus.currentFocus.GetComponent<HateManager>(); //target hate manager


        if (myCharacterFocus != null && myCharacterFocus.currentFocus != null)
        {
            if (targetCharacterStats != null) //doing the skill stuff
            {
                //subtract stamina cost from me
                myCharacterStats.SubtractStamina(skillSOs[slotNumber].staminaCost);

                //apply health damage to target
                targetCharacterStats.SubtractHealth(skillSOs[slotNumber].targetDamage);

            }

            //add aggressor to target hate list
            if (targetHateManager != null)
            {
                targetHateManager.AddToHateList(this.GetComponent<Interactable>());
            }
        }


    }
}
