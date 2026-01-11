using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    //public event Action OnStatsChanged;

    //General
    public event Action<string> OnNameChanged;
    public event Action<ClassSO> OnClassChanged;
    public event Action<RaceSO> OnRaceChanged;

    //health events
    public event Action<float> OnHealthChanged;
    public event Action<float> OnMaxHealthChanged;

    //experience events
    public event Action<float> OnEXPChanged;
    public event Action<float> OnLevelChanged;

    //stamina events
    public event Action<float> OnStaminaChanged;
    public event Action<float> OnMaxStaminaChanged;

    //mana events
    public event Action<float> OnManaChanged;
    public event Action<float> OnMaxManaChanged;

    //strength events
    public event Action<float> OnStrengthBaseChanged;
    public event Action<float> OnStrengthRaceChanged;
    public event Action<float> OnStrengthClassChanged;
    public event Action<float> OnStrengthScoreChanged;
    public event Action<float> OnStrengthModifierChanged;

    //dexterity events
    public event Action<float> OnDexterityBaseChanged;
    public event Action<float> OnDexterityRaceChanged;
    public event Action<float> OnDexterityClassChanged;
    public event Action<float> OnDexterityScoreChanged;
    public event Action<float> OnDexterityModifierChanged;

    //constitution events
    public event Action<float> OnConstitutionBaseChanged;
    public event Action<float> OnConstitutionRaceChanged;
    public event Action<float> OnConstitutionClassChanged;
    public event Action<float> OnConstitutionScoreChanged;
    public event Action<float> OnConstitutionModifierChanged;

    //intelligence events
    public event Action<float> OnIntelligenceBaseChanged;
    public event Action<float> OnIntelligenceRaceChanged;
    public event Action<float> OnIntelligenceClassChanged;
    public event Action<float> OnIntelligenceScoreChanged;
    public event Action<float> OnIntelligenceModifierChanged;

    //wisdom events
    public event Action<float> OnWisdomBaseChanged;
    public event Action<float> OnWisdomRaceChanged;
    public event Action<float> OnWisdomClassChanged;
    public event Action<float> OnWisdomScoreChanged;
    public event Action<float> OnWisdomModifierChanged;

    //charisma events
    public event Action<float> OnCharismaBaseChanged;
    public event Action<float> OnCharismaRaceChanged;
    public event Action<float> OnCharismaClassChanged;
    public event Action<float> OnCharismaScoreChanged;
    public event Action<float> OnCharismaModifierChanged;

    //armor class event
    public event Action<float> OnArmorClassChanged;

    //references
    public InventoryStats inventoryStats;
    public Equipment equipment;

    //xp Tracking
    public List<Character> charactersWhoHitMe = new List<Character>();
    public bool gaveXP = false;

    //General
    public Sprite icon;
    public Animator animator;
    public bool isQuadruped = false;

    //Max Attributes
    public float maxHitpoints { get; private set; }
    public float maxStamina { get; private set; }
    public float maxMana { get; private set; }

    //current stats
    public float currentHitPoints { get; private set; }
    public float currentStamina { get; private set; }
    public float currentMana { get; private set; }
    public float armorClass { get; private set; } //10 + armorBonus + dexMod + sizeMod

    //base stats these are the initial attribute choices during character creation for now default to 10
    public float strengthBase { get; private set; } = 10;
    public float dexterityBase { get; private set; } = 10;
    public float constitutionBase { get; private set; } = 10;
    public float intelligenceBase { get; private set; } = 10;
    public float wisdomBase { get; private set; } = 10;
    public float charismaBase { get; private set; } = 10;

    //Attribute modifiers
    public float strengthModifier { get; private set; } //score minus 10 divided by 2
    public float dexterityModifier { get; private set; }
    public float constitutionModifier { get; private set; }
    public float intelligenceModifier { get; private set; }
    public float wisdomModifier { get; private set; }
    public float charismaModifier { get; private set; }

    //Stat Scores
    public float strengthScore { get; private set; }
    public float dexterityScore { get; private set; }
    public float constitutionScore { get; private set; }
    public float intelligenceScore { get; private set; }
    public float wisdomScore { get; private set; }
    public float charismaScore { get; private set; }

    //Progression
    public float characterLevel { get; private set; }
    float experience;
    float percentage;

    //armor and size
    public float equipmentAc { get; private set; }

    //math
    Unity.Mathematics.Random rand;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        equipment = GetComponent<Equipment>();

        // Initialize the random number generator with a unique seed
        rand = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks + (uint)GetInstanceID());

        //base stats can't ever ever be zero
        strengthBase = Mathf.Max(1, strengthBase);
        dexterityBase = Mathf.Max(1, dexterityBase);
        constitutionBase = Mathf.Max(1, constitutionBase);
        intelligenceBase = Mathf.Max(1, intelligenceBase);
        wisdomBase = Mathf.Max(1, wisdomBase);
        charismaBase = Mathf.Max(1, charismaBase);

        CalculateAttributesAndStats();

        currentHitPoints = maxHitpoints;
        currentStamina = maxStamina;
        currentMana = maxMana;
    }

    private void SetEquipmentAc(float ac)
    {
        equipmentAc = ac;
        CalculateAttributesAndStats();
    }

    void OnEnable()
    {
        //subscribe to events or initialize as needed
        equipment.OnAcChanged += SetEquipmentAc;
    }

    void OnDestroy()
    {
        equipment.OnAcChanged -= SetEquipmentAc;
    }

    void CalculateAttributesAndStats()
    {
        //invoke name changed event
        OnNameChanged?.Invoke(interactableName);
        OnClassChanged?.Invoke(characterClass);
        OnRaceChanged?.Invoke(characterRace);

        //calculate level based on current experience
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        OnEXPChanged?.Invoke(percentage);
        OnLevelChanged?.Invoke(characterLevel);

        //calculate attributes
        //Strength
        strengthScore = CalculateStatScore(strengthBase, characterRace.strengthBonus, characterClass.strengthBonus, characterLevel);
        strengthModifier = CalculateStatModifier(strengthScore, characterLevel);
        OnStrengthRaceChanged?.Invoke(characterRace.strengthBonus);
        OnStrengthClassChanged?.Invoke(characterClass.strengthBonus);
        OnStrengthScoreChanged?.Invoke(strengthScore);
        OnStrengthModifierChanged?.Invoke(strengthModifier);
        OnStrengthBaseChanged?.Invoke(strengthBase);
        //Dexterity
        dexterityScore = CalculateStatScore(dexterityBase, characterRace.dexterityBonus, characterClass.dexterityBonus, characterLevel);
        dexterityModifier = CalculateStatModifier(dexterityScore, characterLevel);
        OnDexterityRaceChanged?.Invoke(characterRace.dexterityBonus);
        OnDexterityClassChanged?.Invoke(characterClass.dexterityBonus);
        OnDexterityScoreChanged?.Invoke(dexterityScore);
        OnDexterityModifierChanged?.Invoke(dexterityModifier);
        OnDexterityBaseChanged?.Invoke(dexterityBase);
        //Constitution
        constitutionScore = CalculateStatScore(constitutionBase, characterRace.constitutionBonus, characterClass.constitutionBonus, characterLevel);
        constitutionModifier = CalculateStatModifier(constitutionScore, characterLevel);
        OnConstitutionRaceChanged?.Invoke(characterRace.constitutionBonus);
        OnConstitutionClassChanged?.Invoke(characterClass.constitutionBonus);
        OnConstitutionScoreChanged?.Invoke(constitutionScore);
        OnConstitutionModifierChanged?.Invoke(constitutionModifier);
        OnConstitutionBaseChanged?.Invoke(constitutionBase);
        //Intelligence
        intelligenceScore = CalculateStatScore(intelligenceBase, characterRace.intelligenceBonus, characterClass.intelligenceBonus, characterLevel);
        intelligenceModifier = CalculateStatModifier(intelligenceScore, characterLevel);
        OnIntelligenceRaceChanged?.Invoke(characterRace.intelligenceBonus);
        OnIntelligenceClassChanged?.Invoke(characterClass.intelligenceBonus);
        OnIntelligenceScoreChanged?.Invoke(intelligenceScore);
        OnIntelligenceModifierChanged?.Invoke(intelligenceModifier);
        OnIntelligenceBaseChanged?.Invoke(intelligenceBase);
        //Wisdom
        wisdomScore = CalculateStatScore(wisdomBase, characterRace.wisdomBonus, characterClass.wisdomBonus, characterLevel);
        wisdomModifier = CalculateStatModifier(wisdomScore, characterLevel);
        OnWisdomRaceChanged?.Invoke(characterRace.wisdomBonus);
        OnWisdomClassChanged?.Invoke(characterClass.wisdomBonus);
        OnWisdomScoreChanged?.Invoke(wisdomScore);
        OnWisdomModifierChanged?.Invoke(wisdomModifier);
        OnWisdomBaseChanged?.Invoke(wisdomBase);
        //Charisma  
        charismaScore = CalculateStatScore(charismaBase, characterRace.charismaBonus, characterClass.charismaBonus, characterLevel);
        charismaModifier = CalculateStatModifier(charismaScore, characterLevel);
        OnCharismaRaceChanged?.Invoke(characterRace.charismaBonus);
        OnCharismaClassChanged?.Invoke(characterClass.charismaBonus);
        OnCharismaScoreChanged?.Invoke(charismaScore);
        OnCharismaModifierChanged?.Invoke(charismaModifier);
        OnCharismaBaseChanged?.Invoke(charismaBase);

        maxHitpoints = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        OnHealthChanged?.Invoke(currentHitPoints);
        OnMaxHealthChanged?.Invoke(maxHitpoints);

        maxStamina = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        OnStaminaChanged?.Invoke(currentStamina);
        OnMaxStaminaChanged?.Invoke(maxStamina);

        maxMana = (characterClass.manaDie * characterLevel) + intelligenceModifier; //(level * base) + int modifier
        OnManaChanged?.Invoke(currentMana);
        OnMaxManaChanged?.Invoke(maxMana);

        armorClass = 10 + equipmentAc + characterRace.naturalAcBonus + dexterityModifier + characterRace.sizeAcBonus;
        OnArmorClassChanged?.Invoke(armorClass);
    }

    float CalculateStatScore(float statBase, float raceBonus, float classBonus, float characterLevel)
    {
        float statScore = statBase + characterLevel * raceBonus * classBonus;
        return statScore;
    }

    float CalculateStatModifier(float statScore, float characterLevel)
    {
        float statModifier = (statScore - 10) / 2f;

        if (statModifier < 1) //statModifier can't be less than 1
            statModifier = 1;

        return statModifier;
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
        OnMaxHealthChanged?.Invoke(maxHitpoints);
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

        // Recalculate level and percentage
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        // Recalculate stats based on new experience
        CalculateAttributesAndStats();
    }

    //public void SetName(string newName)
    //{
    //    interactableName = newName;
    //    OnNameChanged?.Invoke(interactableName);
    //}

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

    public void Revive()
    {
        dead = false;
        gaveXP = false;
        charactersWhoHitMe.Clear();
        animator.SetBool("Dead", false);
        AddHealth(maxHitpoints);
    }

    public void RegenerateStats()
    {
        //regenerate health
        if (!sitting && !dead)//if not sitting
        {
            AddHealth(1 + constitutionModifier);
            AddStamina(1 + constitutionModifier);
        }

        if (sitting && !dead)//extra regen if sitting
        {
            AddHealth(1 + constitutionModifier * 2);
            AddStamina(1 + constitutionModifier * 2);
        }
    }
}