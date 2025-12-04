using UnityEngine;

public class Stats : Character
{
    //references
    public HealthBar healthbar;
    EXPBar expBar;
    Equipment equipment;
    
    //General
    public string characterName;
    public Sprite icon;
    
    //Base Attributes
    public int currentHitPoints;
    public int maxHitpoints;
    public int armorClass; //10 + armorBonus + dexMod + sizeMod
    public int currentStamina;
    public int maxStamina;
    public int currentMana;
    public int maxMana;
    
    public int strengthBase; //initial attribute choices
    public int dexterityBase;
    public int constitutionBase;
    public int intelligenceBase;
    public int wisdomBase;
    public int charismaBase;
    
    //Attribute modifiers
    public int strengthModifier; //score minus 10 divided by 2
    public int dexterityModifier;
    public int constitutionModifier;
    public int intelligenceModifier;
    public int wisdomModifier;
    public int charismaModifier;
    
    //Stat Scores
    public int strengthScore; //base plus bonuses
    public int dexterityScore;
    public int constitutionScore;
    public int intelligenceScore;
    public int wisdomScore;
    public int charismaScore;
    
    //Progression
    public int characterLevel;
    public int experience;
    private float percentage;
    
    public int sizeModifier;
    public int armorBonus;
    
    int CalculateLevel(int experience)
    {
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = ((1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1);

        return characterLevel;
    }
    
    public void CalculateAttributesAndStats()
    {
        characterLevel = CalculateLevel(experience);

        //calculate attributes
        strengthScore = CalculateStatScore(strengthBase, characterRace.strengthBonus);
        strengthModifier = CalculateStatModifier(strengthScore);

        dexterityScore = CalculateStatScore(dexterityBase, characterRace.dexterityBonus);
        dexterityModifier = CalculateStatModifier(dexterityScore);

        constitutionScore = CalculateStatScore(constitutionBase, characterRace.constitutionBonus);
        constitutionModifier = CalculateStatModifier(constitutionScore);

        intelligenceScore = CalculateStatScore(intelligenceBase, characterRace.intelligenceBonus);
        intelligenceModifier = CalculateStatModifier(intelligenceScore);

        wisdomScore = CalculateStatScore(wisdomBase, characterRace.wisdomBonus);
        wisdomModifier = CalculateStatModifier(wisdomScore);

        charismaScore = CalculateStatScore(charismaBase, characterRace.charismaBonus);
        charismaModifier = CalculateStatModifier(charismaScore);

        maxHitpoints = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier

        if (healthbar != null)
        {
            healthbar.SetMaxHealth(maxHitpoints);
            healthbar.SetHealth(currentHitPoints);
        }

        if (expBar != null)
        {
            expBar.SetEXP(percentage);
        }

        sizeModifier = characterRace.sizeAcBonus;

        int armorBonus = equipment.ArmorAC + characterRace.naturalAcBonus;
        armorClass = 10 + armorBonus + dexterityModifier + sizeModifier;

    }
    
    int CalculateStatScore(int statBase, int raceBonus)
    {
        int statScore = statBase + raceBonus;
        return statScore;
    }

    int CalculateStatModifier(int statScore)
    {
        int statModifier = (statScore - 10) / 2;
        return statModifier;
    }
}
