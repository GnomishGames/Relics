using UnityEngine;

public class SkillBook : MonoBehaviour
{
    //it's-a-me, Mario!
    public SkillSO[] skillSOs = new SkillSO[10];

    //panel references
    public SkillBarPanel skillBarPanel;
    public SkillBar skillBar;
    Container container;
    Interactable focus;

    public void MoveItem(int fromSlot, int toSlot)
    {
        SkillSO temp = skillSOs[fromSlot];
        skillSOs[fromSlot] = skillSOs[toSlot];
        skillSOs[toSlot] = temp;
    }

    public void DestroyItem(int fromSlot)
    {
        skillSOs[fromSlot] = null;
    }

    public void UnEquipSkill(int skillBookSlot, int skillPanelSlot)
    {
        if (skillSOs[skillBookSlot] == null)
        {
            var buffer = skillSOs[skillBookSlot];
            skillSOs[skillBookSlot] = skillBar.skillSOs[skillPanelSlot];
            skillBar.skillSOs[skillPanelSlot] = (SkillSO)buffer;
        }
        else if (skillSOs[skillBookSlot].slotType == skillBar.skillSOs[skillPanelSlot].slotType)
        {
            var buffer = skillSOs[skillBookSlot];
            skillSOs[skillBookSlot] = skillBar.skillSOs[skillPanelSlot];
            skillBar.skillSOs[skillPanelSlot] = (SkillSO)buffer;
        }
    }

    internal void LootItem(int skillBookSlot, int containerSlot)
    {
        var cf = GetComponent<CharacterFocus>();
        focus = cf != null ? cf.target : null;
        container = focus.GetComponent<Container>();
        if (skillSOs[skillBookSlot] == null)
        {
            var buffer = skillSOs[skillBookSlot];
            skillSOs[skillBookSlot] = (SkillSO)container.containerItem[containerSlot];
            container.containerItem[containerSlot] = buffer;
        }
    }
}
