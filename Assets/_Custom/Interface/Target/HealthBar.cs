using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Attach to the health bar */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI targetNameText;
    
    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }

    public void SetName(string characterName)
    {
        targetNameText.text = characterName;
    }
}
