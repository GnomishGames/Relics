using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetPanel : MonoBehaviour
{
    public TextMeshProUGUI targetNameText;
    public Slider hpSlider;
    public CharacterFocus focus;
    public CharacterStats characterStats;
    public Slider staminaSlider;

    void Start()
    {
        var player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            focus = player.GetComponent<CharacterFocus>();
        }
    }

    void Update()
    {
        if (focus == null || focus.currentFocus == null)
        {
            targetNameText.text = "No Target";
            return;
        }else{
            //update target name text
            targetNameText.text = focus.currentFocus.name;
            
            //update hp slider
            hpSlider.value = focus.currentFocus.GetComponent<CharacterStats>().currentHitPoints;
            hpSlider.maxValue = focus.currentFocus.GetComponent<CharacterStats>().maxHitpoints;

            //update stanmina slider
            staminaSlider.value = focus.currentFocus.GetComponent<CharacterStats>().currentStamina;
            staminaSlider.maxValue = focus.currentFocus.GetComponent<CharacterStats>().maxStamina;
        }
    }
}