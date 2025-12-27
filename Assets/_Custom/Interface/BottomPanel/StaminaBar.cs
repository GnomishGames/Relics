using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    private CharacterStats characterStats;

    void Awake()
    {
        // Get reference to CharacterStats if not set
        characterStats = GetComponentInParent<CharacterStats>();

        // Subscribe to stamina changes for updates
        characterStats.OnStaminaChanged += SetStamina;
        characterStats.OnMaxStaminaChanged += SetMaxStamina;
    }

    private void OnDestroy()
    {
        if (characterStats != null)
        {
            characterStats.OnStaminaChanged -= SetStamina;
            characterStats.OnMaxStaminaChanged -= SetMaxStamina;
        }
    }

    public void SetMaxStamina(float stamina)
    {
        slider.maxValue = stamina;
    }

    public void SetStamina(float stamina)
    {
        slider.value = stamina;
    }
}
