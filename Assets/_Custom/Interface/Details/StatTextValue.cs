using System;
using TMPro;
using UnityEngine;

public class StatTextValue : MonoBehaviour
{
    [SerializeField] private string statName;
    private TMP_Text text;
    private CharacterStats characterStats;

    void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();
        statName = this.gameObject.name;
        text = GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        switch (statName)
        {
            case "interactableName":
                characterStats.OnNameChanged += UpdateText;
                UpdateText(characterStats.interactableName);
                break;
            case "characterLevel":
                characterStats.OnLevelChanged += UpdateText;
                UpdateText(characterStats.characterLevel);
                break;
            case "characterClass":
                characterStats.OnClassChanged += UpdateText;
                UpdateText(characterStats.characterClass);
                break;
            case "characterRace":
                characterStats.OnRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace);
                break;
            case "maxHitpoints":
                characterStats.OnMaxHealthChanged += UpdateText;
                UpdateText(characterStats.maxHitpoints);
                break;
            case "maxStamina":
                characterStats.OnMaxStaminaChanged += UpdateText;
                UpdateText(characterStats.maxStamina);
                break;
            case "maxMana":
                characterStats.OnMaxManaChanged += UpdateText;
                UpdateText(characterStats.maxMana);
                break;
            case "armorClass":
                characterStats.OnArmorClassChanged += UpdateText;
                UpdateText(characterStats.armorClass);
                break;
            case "strengthBase":
                characterStats.OnStrengthBaseChanged += UpdateText;
                UpdateText(characterStats.strengthBase);
                break;
            case "characterRacestrengthBonus":
                characterStats.OnStrengthRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.strengthBonus);
                break;
            case "characterClassstrengthBonus":
                characterStats.OnStrengthClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.strengthBonus);
                break;
            case "strengthScore":
                characterStats.OnStrengthScoreChanged += UpdateText;
                UpdateText(characterStats.strengthScore);
                break;
            case "strengthModifier":
                characterStats.OnStrengthModifierChanged += UpdateText;
                UpdateText(characterStats.strengthModifier);
                break;
            case "dexterityBase":
                characterStats.OnDexterityBaseChanged += UpdateText;
                UpdateText(characterStats.dexterityBase);
                break;
            case "characterRacedexterityBonus":
                characterStats.OnDexterityRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.dexterityBonus);
                break;
            case "characterClassdexterityBonus":
                characterStats.OnDexterityClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.dexterityBonus);
                break;
            case "dexterityScore":
                characterStats.OnDexterityScoreChanged += UpdateText;
                UpdateText(characterStats.dexterityScore);
                break;
            case "dexterityModifier":
                characterStats.OnDexterityModifierChanged += UpdateText;
                UpdateText(characterStats.dexterityModifier);
                break;
            case "constitutionBase":
                characterStats.OnConstitutionBaseChanged += UpdateText;
                UpdateText(characterStats.constitutionBase);
                break;
            case "characterRaceconstitutionBonus":
                characterStats.OnConstitutionRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.constitutionBonus);
                break;
            case "characterClassconstitutionBonus":
                characterStats.OnConstitutionClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.constitutionBonus);
                break;
            case "constitutionScore":
                characterStats.OnConstitutionScoreChanged += UpdateText;
                UpdateText(characterStats.constitutionScore);
                break;
            case "constitutionModifier":
                characterStats.OnConstitutionModifierChanged += UpdateText;
                UpdateText(characterStats.constitutionModifier);
                break;
            case "intelligenceBase":
                characterStats.OnIntelligenceBaseChanged += UpdateText;
                UpdateText(characterStats.intelligenceBase);
                break;
            case "characterRaceintelligenceBonus":
                characterStats.OnIntelligenceRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.intelligenceBonus);
                break;
            case "characterClassintelligenceBonus":
                characterStats.OnIntelligenceClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.intelligenceBonus);
                break;
            case "intelligenceScore":
                characterStats.OnIntelligenceScoreChanged += UpdateText;
                UpdateText(characterStats.intelligenceScore);
                break;
            case "intelligenceModifier":
                characterStats.OnIntelligenceModifierChanged += UpdateText;
                UpdateText(characterStats.intelligenceModifier);
                break;
            case "wisdomBase":
                characterStats.OnWisdomBaseChanged += UpdateText;
                UpdateText(characterStats.wisdomBase);
                break;
            case "characterRacewisdomBonus":
                characterStats.OnWisdomRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.wisdomBonus);
                break;
            case "characterClasswisdomBonus":
                characterStats.OnWisdomClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.wisdomBonus);
                break;
            case "wisdomScore":
                characterStats.OnWisdomScoreChanged += UpdateText;
                UpdateText(characterStats.wisdomScore);
                break;
            case "wisdomModifier":
                characterStats.OnWisdomModifierChanged += UpdateText;
                UpdateText(characterStats.wisdomModifier);
                break;
            case "charismaBase":
                characterStats.OnCharismaBaseChanged += UpdateText;
                UpdateText(characterStats.charismaBase);
                break;
            case "characterRacecharismaBonus":
                characterStats.OnCharismaRaceChanged += UpdateText;
                UpdateText(characterStats.characterRace.charismaBonus);
                break;
            case "characterClasscharismaBonus":
                characterStats.OnCharismaClassChanged += UpdateText;
                UpdateText(characterStats.characterClass.charismaBonus);
                break;
            case "charismaScore":
                characterStats.OnCharismaScoreChanged += UpdateText;
                UpdateText(characterStats.charismaScore);
                break;
            case "charismaModifier":
                characterStats.OnCharismaModifierChanged += UpdateText;
                UpdateText(characterStats.charismaModifier);
                break;
        }
    }

    void OnDisable()
    {
        switch (statName)
        {
            case "interactableName":
                characterStats.OnNameChanged -= UpdateText;
                UpdateText(characterStats.interactableName);
                break;
            case "characterLevel":
                characterStats.OnLevelChanged -= UpdateText;
                UpdateText(characterStats.characterLevel);
                break;
            case "characterClass":
                characterStats.OnClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass);
                break;
            case "characterRace":
                characterStats.OnRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace);
                break;
            case "maxHitpoints":
                characterStats.OnMaxHealthChanged -= UpdateText;
                UpdateText(characterStats.maxHitpoints);
                break;
            case "maxStamina":
                characterStats.OnMaxStaminaChanged -= UpdateText;
                UpdateText(characterStats.maxStamina);
                break;
            case "maxMana":
                characterStats.OnMaxManaChanged -= UpdateText;
                UpdateText(characterStats.maxMana);
                break;
            case "armorClass":
                characterStats.OnArmorClassChanged -= UpdateText;
                UpdateText(characterStats.armorClass);
                break;
            case "strengthBase":
                characterStats.OnStrengthBaseChanged -= UpdateText;
                UpdateText(characterStats.strengthBase);
                break;
            case "characterRacestrengthBonus":
                characterStats.OnStrengthRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.strengthBonus);
                break;
            case "characterClassstrengthBonus":
                characterStats.OnStrengthClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.strengthBonus);
                break;
            case "strengthScore":
                characterStats.OnStrengthScoreChanged -= UpdateText;
                UpdateText(characterStats.strengthScore);
                break;
            case "strengthModifier":
                characterStats.OnStrengthModifierChanged -= UpdateText;
                UpdateText(characterStats.strengthModifier);
                break;
            case "dexterityBase":
                characterStats.OnDexterityBaseChanged -= UpdateText;
                UpdateText(characterStats.dexterityBase);
                break;
            case "characterRacedexterityBonus":
                characterStats.OnDexterityRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.dexterityBonus);
                break;
            case "characterClassdexterityBonus":
                characterStats.OnDexterityClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.dexterityBonus);
                break;
            case "dexterityScore":
                characterStats.OnDexterityScoreChanged -= UpdateText;
                UpdateText(characterStats.dexterityScore);
                break;
            case "dexterityModifier":
                characterStats.OnDexterityModifierChanged -= UpdateText;
                UpdateText(characterStats.dexterityModifier);
                break;
            case "constitutionBase":
                characterStats.OnConstitutionBaseChanged -= UpdateText;
                UpdateText(characterStats.constitutionBase);
                break;
            case "characterRaceconstitutionBonus":
                characterStats.OnConstitutionRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.constitutionBonus);
                break;
            case "characterClassconstitutionBonus":
                characterStats.OnConstitutionClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.constitutionBonus);
                break;
            case "constitutionScore":
                characterStats.OnConstitutionScoreChanged -= UpdateText;
                UpdateText(characterStats.constitutionScore);
                break;
            case "constitutionModifier":
                characterStats.OnConstitutionModifierChanged -= UpdateText;
                UpdateText(characterStats.constitutionModifier);
                break;
            case "intelligenceBase":
                characterStats.OnIntelligenceBaseChanged -= UpdateText;
                UpdateText(characterStats.intelligenceBase);
                break;
            case "characterRaceintelligenceBonus":
                characterStats.OnIntelligenceRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.intelligenceBonus);
                break;
            case "characterClassintelligenceBonus":
                characterStats.OnIntelligenceClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.intelligenceBonus);
                break;
            case "intelligenceScore":
                characterStats.OnIntelligenceScoreChanged -= UpdateText;
                UpdateText(characterStats.intelligenceScore);
                break;
            case "intelligenceModifier":
                characterStats.OnIntelligenceModifierChanged -= UpdateText;
                UpdateText(characterStats.intelligenceModifier);
                break;
            case "wisdomBase":
                characterStats.OnWisdomBaseChanged -= UpdateText;
                UpdateText(characterStats.wisdomBase);
                break;
            case "characterRacewisdomBonus":
                characterStats.OnWisdomRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.wisdomBonus);
                break;
            case "characterClasswisdomBonus":
                characterStats.OnWisdomClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.wisdomBonus);
                break;
            case "wisdomScore":
                characterStats.OnWisdomScoreChanged -= UpdateText;
                UpdateText(characterStats.wisdomScore);
                break;
            case "wisdomModifier":
                characterStats.OnWisdomModifierChanged -= UpdateText;
                UpdateText(characterStats.wisdomModifier);
                break;
            case "charismaBase":
                characterStats.OnCharismaBaseChanged -= UpdateText;
                UpdateText(characterStats.charismaBase);
                break;
            case "characterRacecharismaBonus":
                characterStats.OnCharismaRaceChanged -= UpdateText;
                UpdateText(characterStats.characterRace.charismaBonus);
                break;
            case "characterClasscharismaBonus":
                characterStats.OnCharismaClassChanged -= UpdateText;
                UpdateText(characterStats.characterClass.charismaBonus);
                break;
            case "charismaScore":
                characterStats.OnCharismaScoreChanged -= UpdateText;
                UpdateText(characterStats.charismaScore);
                break;
            case "charismaModifier":
                characterStats.OnCharismaModifierChanged -= UpdateText;
                UpdateText(characterStats.charismaModifier);
                break;
        }
    }

    void UpdateText(float newValue)
    {
        text.text = newValue.ToString();
    }

    void UpdateText(string obj)
    {
        text.text = obj;
    }

    private void UpdateText(RaceSO sO)
    {
        text.text = sO.raceName;
    }

    private void UpdateText(ClassSO sO)
    {
        text.text = sO.className;
    }
}