using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : Character
{
    //events
    //public event Action OnStatsChanged;

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
    public event Action<float> OnMaxManaChanged;
    
    //attribute score events
    public event Action<float> OnStrengthScoreChanged;
    public event Action<float> OnDexterityScoreChanged;
    public event Action<float> OnConstitutionScoreChanged;
    public event Action<float> OnIntelligenceScoreChanged;
    public event Action<float> OnWisdomScoreChanged;
    public event Action<float> OnCharismaScoreChanged;

    //name event for name changes
    public event Action<string> OnNameChanged;

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
    float maxHitpoints;
    float maxStamina;
    float maxMana;

    //current stats
    public float currentHitPoints;
    public float currentStamina;
    float currentMana;
    public float armorClass; //10 + armorBonus + dexMod + sizeMod

    //base stats these are the initial attribute choices during character creation
    public float strengthBase;
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
    float characterLevel;
    float experience;
    float percentage;

    //armor and size
    float sizeModifier;
    float armorBonus;
    float equipmentAc;

    //math
    Unity.Mathematics.Random rand;

    void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        equipment = GetComponent<Equipment>();

        // Initialize the random number generator with a unique seed
        rand = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks + (uint)GetInstanceID());

        CalculateAttributesAndStats();

        currentHitPoints = maxHitpoints;
        currentStamina = maxStamina;

        //subscribe to equipment changes to recalculate armor class
        equipment.OnAcChanged += SetEquipmentAc;
    }

    private void SetEquipmentAc(float ac)
    {
        equipmentAc = ac;
        CalculateAttributesAndStats();
    }

    void Start()
    {
        //manually invoke the events to get initial value after all subscribers have registered
        FireAllStatsEvents();
    }

    void OnEnable()
    {
        FireAllStatsEvents();
    }

    void OnDestroy()
    {
        equipment.OnAcChanged -= SetEquipmentAc;
    }

    public void FireAllStatsEvents()
    {
        OnLevelChanged?.Invoke(characterLevel);
        OnHealthChanged?.Invoke(currentHitPoints);
        OnMaxHealthChanged?.Invoke(maxHitpoints);
        OnStaminaChanged?.Invoke(currentStamina);
        OnMaxStaminaChanged?.Invoke(maxStamina);
        OnMaxManaChanged?.Invoke(maxMana);
        OnNameChanged?.Invoke(interactableName);
        OnEXPChanged?.Invoke(percentage);
        OnStrengthScoreChanged?.Invoke(strengthScore);
        OnDexterityScoreChanged?.Invoke(dexterityScore);
        OnConstitutionScoreChanged?.Invoke(constitutionScore);
        OnIntelligenceScoreChanged?.Invoke(intelligenceScore);
        OnWisdomScoreChanged?.Invoke(wisdomScore);
        OnCharismaScoreChanged?.Invoke(charismaScore);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0)) //0 key for testing
        {
            AddExperience(150);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9)) //9 key for testing
        {
            SubtractExperience(150);
        }
    }

    void CalculateAttributesAndStats()
    {
        //calculate level based on current experience
        characterLevel = Mathf.FloorToInt((1 + Mathf.Sqrt(experience / 125 + 1)) / 2);
        percentage = (1 + Mathf.Sqrt(experience / 125 + 1)) / 2 % 1;

        OnEXPChanged?.Invoke(percentage);
        OnLevelChanged?.Invoke(characterLevel);
        

        //calculate attributes
        strengthScore = CalculateStatScore(strengthBase, characterRace.strengthBonus, characterLevel);
        strengthModifier = CalculateStatModifier(strengthScore);
        OnStrengthScoreChanged?.Invoke(strengthScore);

        dexterityScore = CalculateStatScore(dexterityBase, characterRace.dexterityBonus, characterLevel);
        dexterityModifier = CalculateStatModifier(dexterityScore);
        OnDexterityScoreChanged?.Invoke(dexterityScore);

        constitutionScore = CalculateStatScore(constitutionBase, characterRace.constitutionBonus, characterLevel);
        constitutionModifier = CalculateStatModifier(constitutionScore);
        OnConstitutionScoreChanged?.Invoke(constitutionScore);

        intelligenceScore = CalculateStatScore(intelligenceBase, characterRace.intelligenceBonus, characterLevel);
        intelligenceModifier = CalculateStatModifier(intelligenceScore);
        OnIntelligenceScoreChanged?.Invoke(intelligenceScore);

        wisdomScore = CalculateStatScore(wisdomBase, characterRace.wisdomBonus, characterLevel);
        wisdomModifier = CalculateStatModifier(wisdomScore);
        OnWisdomScoreChanged?.Invoke(wisdomScore);

        charismaScore = CalculateStatScore(charismaBase, characterRace.charismaBonus, characterLevel);
        charismaModifier = CalculateStatModifier(charismaScore);
        OnCharismaScoreChanged?.Invoke(charismaScore);

        maxHitpoints = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        OnMaxHealthChanged?.Invoke(maxHitpoints);

        maxStamina = (characterClass.hitDie * characterLevel) + constitutionModifier; //(level * base) + con modifier
        OnMaxStaminaChanged?.Invoke(maxStamina);
        
        maxMana = (characterClass.manaDie * characterLevel) + intelligenceModifier; //(level * base) + int modifier
        OnMaxManaChanged?.Invoke(maxMana);

        sizeModifier = characterRace.sizeAcBonus;

        armorClass = 10 + equipmentAc + characterRace.naturalAcBonus + dexterityModifier + sizeModifier;
        OnArmorClassChanged?.Invoke(armorClass);

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

    public void SetName(string newName)
    {
        interactableName = newName;
        OnNameChanged?.Invoke(interactableName);
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

    public void Revive()
    {
        dead = false;
        gaveXP = false;
        animator.SetBool("Dead", false);
        AddHealth(maxHitpoints);
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