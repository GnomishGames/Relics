using System;
using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Object/Skill/New Skill")]
public class SkillSO : ItemSO
{
    public string skillName;
    public float staminaCost;
    public float cooldownTime;
}
