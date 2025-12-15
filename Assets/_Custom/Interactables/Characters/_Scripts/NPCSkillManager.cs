using UnityEngine;

public class NPCSkillManager : MonoBehaviour
{
    //reference to skillbar
    SkillBar skillBar;

    //referecne to character focus to get target
    CharacterFocus characterFocus;

    void Start()
    {
        skillBar = GetComponent<SkillBar>();
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
        var target = characterFocus.currentFocus;
        if (target == null) return;
        Debug.Log("NPC Target: " + target.name);

        //check distance to target
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget > ChooseRandomSkill().attackRange) return;
        Debug.Log("NPC Target within range: " + target.name);

        //execute skill
        SkillSO skillToUse = ChooseRandomSkill();
        int skillIndex = System.Array.IndexOf(skillBar.skillSOs, skillToUse);
        skillBar.DoSkill(skillIndex);   
        Debug.Log("NPC used skill: " + skillToUse.itemName);
    }


}
