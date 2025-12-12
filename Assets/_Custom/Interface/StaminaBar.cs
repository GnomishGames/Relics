using UnityEngine;
using UnityEngine.UI;

public class StaminaBar : MonoBehaviour
{
    public Slider slider;
    private CharacterStats characterStats;

    //subscribe to CharacterStats stamina event
    public void Initialize(CharacterStats stats)
    {
        // unsubscribe if reused
        if (characterStats != null)
            characterStats.OnStaminaChanged -= SetStamina;

        characterStats = stats;

        // subscribe to THIS character
        characterStats.OnStaminaChanged += SetStamina;

        // set initial values
        SetMaxStamina(characterStats.maxStamina);
        SetStamina(characterStats.currentStamina);
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
