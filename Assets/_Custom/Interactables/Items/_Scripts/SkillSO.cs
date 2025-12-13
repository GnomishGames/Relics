using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Object/Skill/New Skill")]
public class SkillSO : ItemSO
{
    //public string skillName; // Inherited from ItemSO as itemName
    public float staminaCost;
    public float cooldownTime;
    public float targetDamage;
    public float selfDamage;
}
