using UnityEngine;

public class SkillPanel : MonoBehaviour
{
    public int fromSlot;
    public string fromPanel;

    //skills that are equipped to the skill panel
    public SkillSO[] skillSOs = new SkillSO[8];
}
