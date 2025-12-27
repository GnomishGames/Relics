using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TargetPanel : MonoBehaviour
{
    public TextMeshProUGUI targetNameText;
    public Slider hpSlider;
    public Slider staminaSlider;
    
    private CharacterStats currentTargetStats;

    public void SetNewTarget(CharacterStats targetStats)
    {
        // Unsubscribe from old target
        if (currentTargetStats != null)
        {
            currentTargetStats.OnHealthChanged -= SetHealth;
            currentTargetStats.OnMaxHealthChanged -= SetMaxHealth;
            currentTargetStats.OnStaminaChanged -= SetStamina;
            currentTargetStats.OnMaxStaminaChanged -= SetMaxStamina;
            currentTargetStats.OnNameChanged -= SetName;
        }

        currentTargetStats = targetStats;

        if (currentTargetStats != null)
        {
            // Subscribe to new target's events
            currentTargetStats.OnHealthChanged += SetHealth;
            currentTargetStats.OnMaxHealthChanged += SetMaxHealth;
            currentTargetStats.OnStaminaChanged += SetStamina;
            currentTargetStats.OnMaxStaminaChanged += SetMaxStamina;
            currentTargetStats.OnNameChanged += SetName;

            // Get initial values
            currentTargetStats.FireAllStatsEvents();
        }
    }

    void OnDisable()
    {
        // Unsubscribe when panel closes
        if (currentTargetStats != null)
        {
            currentTargetStats.OnHealthChanged -= SetHealth;
            currentTargetStats.OnMaxHealthChanged -= SetMaxHealth;
            currentTargetStats.OnStaminaChanged -= SetStamina;
            currentTargetStats.OnMaxStaminaChanged -= SetMaxStamina;
            currentTargetStats.OnNameChanged -= SetName;
        }
    }

    void SetName(string name)
    {
        targetNameText.text = name;
    }

    void SetHealth(float health)
    {
        hpSlider.value = health;
    }

    void SetMaxHealth(float maxHealth)
    {
        hpSlider.maxValue = maxHealth;
    }

    void SetStamina(float stamina)
    {
        staminaSlider.value = stamina;
    }

    void SetMaxStamina(float maxStamina)
    {
        staminaSlider.maxValue = maxStamina;
    }
}