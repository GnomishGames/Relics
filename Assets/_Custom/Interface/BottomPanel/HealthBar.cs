using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Attach to the health bar */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI targetNameText;

    private CharacterStats characterStats;

    void Start()
    {
        // Get reference to CharacterStats if not set
        characterStats = GetComponentInParent<CharacterStats>();

        // Initialize max health once
        SetMaxHealth(characterStats.maxHitpoints);
        
        // Subscribe to health changes for updates
        characterStats.OnHealthChanged += SetHealth;
    }

    private void OnDestroy()
    {
        if (characterStats != null)
            characterStats.OnHealthChanged -= SetHealth;
    }

    public void SetMaxHealth(int maxHealth)
    {
        slider.maxValue = maxHealth;
        slider.value = characterStats.currentHitPoints;
    }

    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
