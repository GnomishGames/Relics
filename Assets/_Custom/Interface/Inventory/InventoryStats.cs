using TMPro;
using UnityEngine;

//attach to the stats section of the inventory panel
public class InventoryStats : MonoBehaviour
{
    //references to text fields
    public TextMeshProUGUI interactableName;
    public TextMeshProUGUI maxHitpoints;
    public TextMeshProUGUI armorClass;
    public TextMeshProUGUI maxStamina;
    public TextMeshProUGUI maxMana;

    //Stat Scores
    public TextMeshProUGUI strengthScore;
    public TextMeshProUGUI dexterityScore;
    public TextMeshProUGUI constitutionScore;
    public TextMeshProUGUI intelligenceScore;
    public TextMeshProUGUI wisdomScore;
    public TextMeshProUGUI charismaScore;

    //subscribe to character stats and update display
    private CharacterStats characterStats;

    void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();
    }

    void OnEnable()
    {
        //subscribe to character stats changes
        characterStats.OnNameChanged += SetName;
        SetName(characterStats.interactableName);
        
        characterStats.OnMaxHealthChanged += SetMaxHealth;
        characterStats.OnMaxStaminaChanged += SetMaxStamina;
        characterStats.OnMaxManaChanged += SetMaxMana;
        characterStats.OnStrengthScoreChanged += SetStrengthScore;
        characterStats.OnDexterityScoreChanged += SetDexterityScore;
        characterStats.OnConstitutionScoreChanged += SetConstitutionScore;
        characterStats.OnIntelligenceScoreChanged += SetIntelligenceScore;
        characterStats.OnWisdomScoreChanged += SetWisdomScore;
        characterStats.OnCharismaScoreChanged += SetCharismaScore;
        characterStats.OnArmorClassChanged += SetArmorClass;
    }

    private void OnDestroy() //unsubscribe from events
    {
        if (characterStats != null)
        {
            characterStats.OnNameChanged -= SetName;
            characterStats.OnMaxHealthChanged -= SetMaxHealth;
            characterStats.OnMaxStaminaChanged -= SetMaxStamina;
            characterStats.OnMaxManaChanged -= SetMaxMana;
            characterStats.OnStrengthScoreChanged -= SetStrengthScore;
            characterStats.OnDexterityScoreChanged -= SetDexterityScore;
            characterStats.OnConstitutionScoreChanged -= SetConstitutionScore;
            characterStats.OnIntelligenceScoreChanged -= SetIntelligenceScore;
            characterStats.OnWisdomScoreChanged -= SetWisdomScore;
            characterStats.OnCharismaScoreChanged -= SetCharismaScore;
            characterStats.OnArmorClassChanged -= SetArmorClass;

        }
    }

    public void SetName(string name)
    {
        interactableName.text = name;
    }

    void SetMaxHealth(float maxHealth)
    {
        maxHitpoints.text = maxHealth.ToString();
    }

    void SetArmorClass(float ac)
    {
        armorClass.text = ac.ToString();
    }

    void SetMaxStamina(float stamina)
    {
        maxStamina.text = stamina.ToString();
    }

    void SetMaxMana(float mana)
    {
        maxMana.text = mana.ToString();
    }

    void SetStrengthScore(float score)
    {
        strengthScore.text = score.ToString();
    }

    void SetDexterityScore(float score)
    {
        dexterityScore.text = score.ToString();
    }

    void SetConstitutionScore(float score)
    {
        constitutionScore.text = score.ToString();
    }

    void SetIntelligenceScore(float score)
    {
        intelligenceScore.text = score.ToString();
    }

    void SetWisdomScore(float score)
    {
        wisdomScore.text = score.ToString();
    }

    void SetCharismaScore(float score)
    {
        charismaScore.text = score.ToString();
    }
}
