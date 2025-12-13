using System;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    public event Action<int> OnHealthChanged;
    public event Action<int> OnStaminaChanged;
    public event Action<float> OnEXPChanged;

    //references
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
        // Basic null safety before initial calculations
        if (characterRace == null)
        {
            Debug.LogWarning($"CharacterStats: 'characterRace' is null on {name}. Attribute bonuses will default to 0.");
        }
        if (characterClass == null)
        {
            Debug.LogWarning($"CharacterStats: 'characterClass' is null on {name}. Max HP/Stamina calculations will default to base values.");
        }

        CalculateAttributesAndStats();
        currentHitPoints = maxHitpoints;
        currentStamina = maxStamina;
    }

    void Update()
    {
        // Guard against missing dependencies each frame (useful during scene setup)
        if (characterRace == null || characterClass == null)
        {
            // Avoid spamming logs every frame; only compute minimal safe values
            if (characterRace == null)
            {
                // one-time log per frame for visibility
                Debug.LogWarning($"CharacterStats.Update: Missing 'characterRace' on {name}.");
            }
            if (characterClass == null)
            {
                Debug.LogWarning($"CharacterStats.Update: Missing 'characterClass' on {name}.");
            }
        }

        CalculateAttributesAndStats();
        UpdateInventoryStats();

        if (Input.GetKeyDown(KeyCode.K))
        {
            SubtractHealth(1);
            SubtractStamina(1);
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            AddHealth(1);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            AddExperience(50);
        }
    }

    void CalculateAttributesAndStats()
    {
        // If core data is missing, compute conservative defaults and exit
        if (characterRace == null || characterClass == null)
        {
            // Defaults when data is missing
            characterLevel = CalculateLevel(experience);
            strengthScore = strengthBase;
            dexterityScore = dexterityBase;
            constitutionScore = constitutionBase;
            intelligenceScore = intelligenceBase;
            wisdomScore = wisdomBase;
            charismaScore = charismaBase;

            strengthModifier = CalculateStatModifier(strengthScore);
            dexterityModifier = CalculateStatModifier(dexterityScore);
            constitutionModifier = CalculateStatModifier(constitutionScore);
            intelligenceModifier = CalculateStatModifier(intelligenceScore);
            wisdomModifier = CalculateStatModifier(wisdomScore);
            charismaModifier = CalculateStatModifier(charismaScore);

            // Fallback max values without class/race bonuses
            maxHitpoints = Mathf.Max(1, characterLevel + constitutionModifier);
            maxStamina = Mathf.Max(1, characterLevel + constitutionModifier);
            sizeModifier = 0;
            armorClass = 10 + armorBonus + dexterityModifier + sizeModifier;
            return;
        }

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
        maxStamina = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier

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

    public bool IsEnemy(CharacterStats other)
    {
        // Safety checks
        if (FactionManager.Instance == null)
        {
            Debug.LogError($"IsEnemy: FactionManager.Instance is null for {name}");
            return false;
        }

        if (other == null)
        {
            Debug.LogWarning($"IsEnemy: 'other' CharacterStats is null for {name}");
            return false;
        }

        if (faction == null)
        {
            Debug.LogWarning($"IsEnemy: Self faction is null on {name}");
            return false;
        }

        if (other.faction == null)
        {
            Debug.LogWarning($"IsEnemy: Other faction is null on {other.name}");
            return false;
        }

        bool hostile = FactionManager.Instance.IsHostile(faction, other.faction);
        Debug.Log($"IsEnemy: {name}({faction.name}) vs {other.name}({other.faction.name}) => hostile={hostile}");
        return hostile;
    }

    public void ModifyStamina(int amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        OnStaminaChanged?.Invoke(currentStamina);
    }

    public void SubtractStamina(int amount)
    {
        ModifyStamina(-amount);
    }

    public void AddStamina(int amount)
    {
        ModifyStamina(amount);
    }

    public void ModifyHealth(int amount)
    {
        currentHitPoints += amount;
        currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitpoints);

        OnHealthChanged?.Invoke(currentHitPoints);
    }

    public void SubtractHealth(int amount)
    {
        ModifyHealth(-amount);
    }

    public void AddHealth(int amount)
    {
        ModifyHealth(amount);
    }

    public void AddExperience(int amount)
    {
        ModifyExperience(amount);
    }

    public void SubtractExperience(int amount)
    {
        ModifyExperience(-amount);
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