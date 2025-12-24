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



    void Start()
    {
        //subscribe to character name changes
        characterStats.OnNameChanged += SetName;
    }

    private void OnDestroy() //unsubscribe from events
    {
        if (characterStats != null)
        {
            characterStats.OnNameChanged -= SetName;
        }
    }

    public void SetName(string name)
    {
        interactableName.text = name;
    }

    public void UpdateStats(CharacterStats characterStats)
    {


        maxHitpoints.text = characterStats.maxHitpoints.ToString();
        armorClass.text = characterStats.armorClass.ToString();
        maxStamina.text = characterStats.maxStamina.ToString();
        maxMana.text = characterStats.maxMana.ToString();

        strengthScore.text = characterStats.strengthScore.ToString();
        dexterityScore.text = characterStats.dexterityScore.ToString();
        constitutionScore.text = characterStats.constitutionScore.ToString();
        intelligenceScore.text = characterStats.intelligenceScore.ToString();
        wisdomScore.text = characterStats.wisdomScore.ToString();
        charismaScore.text = characterStats.charismaScore.ToString();
    }
}
