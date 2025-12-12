using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Attach to the health bar */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI targetNameText;

    private CharacterStats characterStats; // the specific stats this bar listens to

    public void Initialize(CharacterStats stats)//"stats" sets who the bar is connected to (which character)
    {
        // unsubscribe if reused
        if (characterStats != null)
            characterStats.OnHealthChanged -= SetHealth;

        characterStats = stats;

        // subscribe to THIS character
        characterStats.OnHealthChanged += SetHealth;

        // set initial values
        SetMaxHealth(characterStats.maxHitpoints);
        SetHealth(characterStats.currentHitPoints);
        targetNameText.text = characterStats.interactableName;
    }

    private void OnDestroy()
    {
        if (characterStats != null)
            characterStats.OnHealthChanged -= SetHealth;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
