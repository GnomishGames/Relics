using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    public event Action<float> OnHealthChanged;
    public event Action<float> OnStaminaChanged;
    public event Action<float> OnEXPChanged;
    public event Action<string> OnNameChanged;
    public event Action<float> OnLevelChanged;

    //references
    public InventoryStats inventoryStats;

    //xp Tracking
    public List<Character> charactersWhoHitMe = new List<Character>();
    public bool gaveXP = false;

    //General
    public Sprite icon;
    public Animator animator;
    public bool isQuadruped = false;

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

    //math
    Unity.Mathematics.Random rand;

    void Awake()
    {
        // Initialize the random number generator with a unique seed
        rand = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks + (uint)GetInstanceID());

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

        animator = GetComponentInChildren<Animator>();
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
            maxMana = Mathf.Max(1, characterLevel + intelligenceModifier);
            sizeModifier = 0;
            armorClass = 10 + armorBonus + dexterityModifier + sizeModifier;
            return;
        }

        characterLevel = CalculateLevel(experience);

        //calculate attributes
        strengthScore = CalculateStatScore(strengthBase, characterRace.strengthBonus, characterLevel);
        strengthModifier = CalculateStatModifier(strengthScore);

        dexterityScore = CalculateStatScore(dexterityBase, characterRace.dexterityBonus, characterLevel);
        dexterityModifier = CalculateStatModifier(dexterityScore);

        constitutionScore = CalculateStatScore(constitutionBase, characterRace.constitutionBonus, characterLevel);
        constitutionModifier = CalculateStatModifier(constitutionScore);

        intelligenceScore = CalculateStatScore(intelligenceBase, characterRace.intelligenceBonus, characterLevel);
        intelligenceModifier = CalculateStatModifier(intelligenceScore);

        wisdomScore = CalculateStatScore(wisdomBase, characterRace.wisdomBonus, characterLevel);
        wisdomModifier = CalculateStatModifier(wisdomScore);

        charismaScore = CalculateStatScore(charismaBase, characterRace.charismaBonus, characterLevel);
        charismaModifier = CalculateStatModifier(charismaScore);

        maxHitpoints = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        maxStamina = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        maxMana = (characterClass.manaDie * characterLevel) + intelligenceModifier; //(level * base) + int modifier

        sizeModifier = characterRace.sizeAcBonus;

        //int armorBonus = equipment.ArmorAC + characterRace.naturalAcBonus;
        armorClass = 10 + armorBonus + dexterityModifier + sizeModifier;
    }

    float CalculateStatScore(float statBase, float raceBonus, float characterLevel)
    {
        float statScore = statBase + raceBonus + characterLevel;
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
        return hostile;
    }

    public int AttackRoll()
    {
        int attackRoll = rand.NextInt(1, 21) + (int)characterLevel + (int)strengthModifier;

        return attackRoll;
    }

    public void ModifyStamina(float amount)
    {
        currentStamina += amount;
        currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina); //keep it between 0 and max

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
        currentHitPoints = Mathf.Clamp(currentHitPoints, 0, maxHitpoints); //keep it between 0 and max

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

    public void SetName(string newName)
    {
        interactableName = newName;
        OnNameChanged?.Invoke(interactableName);
    }

    float CalculateLevel(float experience)
    {
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        LevelChanged();

        return characterLevel;
    }

    void LevelChanged()
    {
        OnLevelChanged?.Invoke(characterLevel);
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

    public void RegenerateStats()
    {
        //regenerate health
        if (!sitting)//if not sitting
        {
            AddHealth(1 + constitutionModifier);
            AddStamina(1 + constitutionModifier);
        }

        if (sitting)//extra regen if sitting
        {
            AddHealth(1 + constitutionModifier * 2);
            AddStamina(1 + constitutionModifier * 2);
        }
    }
}