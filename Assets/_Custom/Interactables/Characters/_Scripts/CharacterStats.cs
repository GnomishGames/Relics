using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    public event Action<float> OnHealthChanged;
    public event Action<float> OnStaminaChanged;
    public event Action<float> OnEXPChanged;

    //references
    public InventoryStats inventoryStats;

    public List<Character> charactersWhoHitMe = new List<Character>();
    public bool gaveXP = false;

    //General
    public Sprite icon;
    public Animator animator;

    //Base Attributes
    public float currentHitPoints;
    public float maxHitpoints;
    public float armorClass; //10 + armorBonus + dexMod + sizeMod
    public float currentStamina;
    public float maxStamina;
    public float currentMana;
    public float maxMana;

    public float strengthBase; //initial attribute choices
    public float dexterityBase;
    public float constitutionBase;
    public float intelligenceBase;
    public float wisdomBase;
    public float charismaBase;

    //Attribute modifiers
    public float strengthModifier; //score minus 10 divided by 2
    public float dexterityModifier;
    public float constitutionModifier;
    public float intelligenceModifier;
    public float wisdomModifier;
    public float charismaModifier;

    //Stat Scores
    public float strengthScore; //base plus bonuses
    public float dexterityScore;
    public float constitutionScore;
    public float intelligenceScore;
    public float wisdomScore;
    public float charismaScore;

    //Progression
    public float characterLevel;
    public float experience;
    private float percentage;

    //armor and size
    public float sizeModifier;
    public float armorBonus;

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

    float CalculateStatScore(float statBase, float raceBonus)
    {
        float statScore = statBase + raceBonus;
        return statScore;
    }

    float CalculateStatModifier(float statScore)
    {
        float statModifier = (statScore - 10) / 2f;
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
        bool hostile = FactionManager.Instance.IsHostile(faction, other.faction);
        Debug.Log($"IsEnemy: {name}({faction.name}) vs {other.name}({other.faction.name}) => hostile={hostile}");
        return hostile;
    }

    public void ModifyStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);

        OnStaminaChanged?.Invoke(currentStamina);
    }

    public void SubtractStamina(float amount)
    {
        ModifyStamina(-amount);
    }

    public void AddStamina(float amount)
    {
        ModifyStamina(amount);
    }

    public void ModifyHealth(float amount)
    {
        currentHitPoints += amount;
        currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitpoints);

        OnHealthChanged?.Invoke(currentHitPoints);
    }

    public void SubtractHealth(float amount)
    {
        ModifyHealth(-amount);
    }

    public void AddHealth(float amount)
    {
        ModifyHealth(amount);
    }

    public void AddExperience(float amount)
    {
        ModifyExperience(amount);
    }

    public void SubtractExperience(float amount)
    {
        ModifyExperience(-amount);
    }

    public void ModifyExperience(float amount)
    {
        experience += amount;

        OnEXPChanged?.Invoke(percentage);
    }

    float CalculateLevel(float experience)
    {
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        return characterLevel;
    }

    public void DeathCheck()
    {
        if (currentHitPoints <= 0)
        {
            currentHitPoints = 0;

            animator.SetBool("Dead", true);
            dead = true;

            if (!gaveXP)
            {
                foreach (var character in charactersWhoHitMe)
                {
                    character.GetComponent<CharacterStats>().experience += xpToGive;
                }
                gaveXP = true;
            }
        }
    }
}