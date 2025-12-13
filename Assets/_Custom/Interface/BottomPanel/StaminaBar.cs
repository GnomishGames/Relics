using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    private CharacterStats characterStats;

    void Start()
    {
        // Get reference to CharacterStats if not set
        characterStats = GetComponentInParent<CharacterStats>();

        // Initialize max stamina once
        SetMaxStamina(characterStats.maxStamina);
        
        // Subscribe to stamina changes for updates
        characterStats.OnStaminaChanged += SetStamina;
    }

    private void OnDestroy()
    {
        if (characterStats != null)
            characterStats.OnStaminaChanged -= SetStamina;
    }

    public void SetMaxStamina(int stamina)
    {
        slider.maxValue = stamina;
        slider.value = stamina;
    }

    public void SetStamina(int stamina)
    {
        slider.value = stamina;
    }
}
