using UnityEngine;

[CreateAssetMenu(fileName = "New Race", menuName = "Scriptable Object/Character/New Race")]
public class RaceSO : ScriptableObject
{
    public string raceName;

    //racial bonuses
    public int strengthBonus;
    public int constitutionBonus;
    public int dexterityBonus;
    public int intelligenceBonus;
    public int wisdomBonus;
    public int charismaBonus;

    public int unarmedDamage;

    //distnace and aggro
    public float viewRadius; 
    public float hearRadius;
    [Range(0, 360)] public float viewAngle;
    public float aggroRadius; //how far it runs from in order to attack
    public float attackDistance; //distance it stands from target when attacking
    public float tooClose; //distance it will back away if target is too close

    //attack vars
    public float attackSpeed;
    public float kickSpeed;

    //movement speeds
    public float walkSpeed;
    public float runSpeed;

    public int sizeAcBonus; //small +1, large -1
    public int naturalAcBonus; 

    //weapon grip offsets
    public Vector3 rightHandOffset;
    public Vector3 leftHandOffset;
}
