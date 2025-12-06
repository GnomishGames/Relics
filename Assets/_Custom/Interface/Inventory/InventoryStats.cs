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

    public void SetName(string name)
    {
        interactableName.text = name;
    }

    public void UpdateStats(Character character)
    {
        maxHitpoints.text = character.maxHitpoints.ToString();
        armorClass.text = character.armorClass.ToString();
        maxStamina.text = character.maxStamina.ToString();
        maxMana.text = character.maxMana.ToString();

        strengthScore.text = character.strengthScore.ToString();
        dexterityScore.text = character.dexterityScore.ToString();
        constitutionScore.text = character.constitutionScore.ToString();
        intelligenceScore.text = character.intelligenceScore.ToString();
        wisdomScore.text = character.wisdomScore.ToString();
        charismaScore.text = character.charismaScore.ToString();
    }
}
