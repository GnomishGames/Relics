using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Scriptable Object/Skill/New Skill")]
public class SkillSO : ScriptableObject
{
    public string skillName;
    public Sprite sprite;
}
