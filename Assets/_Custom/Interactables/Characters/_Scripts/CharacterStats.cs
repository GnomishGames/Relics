using System;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    public event Action<int> OnHealthChanged;
    public event Action<float> OnEXPChanged;

    //references
    //public HealthBar healthbar;
    //public EXPBar expBar;
    public InventoryStats inventoryStats;

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
        UpdateInventoryStats();

        if (Input.GetKeyDown(KeyCode.K))
        {
            TakeDamage(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Heal(1);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            GainExperience(50);
        }
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

    void UpdateInventoryStats()
    {
        //update the stats shown in the inventory
        if (inventoryStats != null)
        {
            inventoryStats.SetName(interactableName);
            inventoryStats.UpdateStats(this);
        }

    }

    public bool IsEnemy(CreatureFaction other)
    {
        return FactionManager.Instance.IsHostile(faction, other.faction);
    }

    public void ModifyHealth(int amount)
    {
        currentHitPoints += amount;
        currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitpoints);

        OnHealthChanged?.Invoke(currentHitPoints);
    }

    public void TakeDamage(int dmg)
    {
        ModifyHealth(-dmg);
    }

    public void Heal(int amount)
    {
        ModifyHealth(amount);
    }

    public void GainExperience(int amount)
    {
        ModifyExperience(amount);
    }

    public void ModifyExperience(int amount)
    {
        experience += amount;

        OnEXPChanged?.Invoke(percentage);
    }

    int CalculateLevel(int experience)
    {
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        return characterLevel;
    }
}