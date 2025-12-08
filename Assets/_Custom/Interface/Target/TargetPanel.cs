using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetPanel : MonoBehaviour
{
    public TextMeshProUGUI targetNameText;
    public Slider hpSlider;
    public Focus focus;
    public CharacterStats characterStats;
    public Slider staminaSlider;

    void Start()
    {
        focus = GameObject.FindWithTag("Player").GetComponent<Focus>();
    }

    void Update()
    {
        if (focus.playerFocus == null)
        {
            targetNameText.text = "No Target";
            return;
        }else{
            //update target name text
            targetNameText.text = focus.playerFocus.name;
            
            //update hp slider
            hpSlider.value = focus.playerFocus.GetComponent<CharacterStats>().currentHitPoints;
            hpSlider.maxValue = focus.playerFocus.GetComponent<CharacterStats>().maxHitpoints;

            //update stanmina slider
            staminaSlider.value = focus.playerFocus.GetComponent<CharacterStats>().currentStamina;
            staminaSlider.maxValue = focus.playerFocus.GetComponent<CharacterStats>().maxStamina;
        }
    }
}