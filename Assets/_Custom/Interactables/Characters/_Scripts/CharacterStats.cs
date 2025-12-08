using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Character
{
    //references
    public HealthBar healthbar;
    public EXPBar expBar;
    public InventoryStats inventoryStats;
    //public FactionSO factionSO;
    //public BehaviorSO behaviorSO;

    //lists
    
    
    //General
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

        void Awake()
    {
        //expBar = GetComponentInChildren<EXPBar>();
        
        CalculateAttributesAndStats();
        currentHitPoints = maxHitpoints;
    }

    void Update()
    {
        CalculateAttributesAndStats();
        UpdateHealthBar();
        UpdateExpBar();
        UpdateInventoryStats();
    }
    
    int CalculateLevel(int experience)
    {
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = ((1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1);

        return characterLevel;
    }
    
    void CalculateAttributesAndStats()
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

        sizeModifier = characterRace.sizeAcBonus;

        //int armorBonus = equipment.ArmorAC + characterRace.naturalAcBonus;
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

    void UpdateHealthBar()
    {
        if (healthbar != null)
        {
            healthbar.SetMaxHealth(maxHitpoints);
            healthbar.SetHealth(currentHitPoints);
            healthbar.SetName(interactableName);
        }
    }

    void UpdateExpBar()
    {
        if (expBar != null)
        {
            expBar.SetEXP(percentage);
        }
    }

    void UpdateInventoryStats()
    {
        //update the stats shown in the inventory
        if (inventoryStats != null)
        {
            inventoryStats.SetName(interactableName);
            inventoryStats.UpdateStats(this);
        }

    }


}