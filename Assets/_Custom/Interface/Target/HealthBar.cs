using UnityEngine;
using UnityEngine.UI;
using TMPro;

/* Attach to the health bar */
public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI targetNameText;

    private CharacterStats stats;   // the specific stats this bar listens to

    public void Initialize(CharacterStats target)
    {
        // unsubscribe if reused
        if (stats != null)
            stats.OnHealthChanged -= SetHealth;

        stats = target;

        // subscribe to THIS character
        stats.OnHealthChanged += SetHealth;

        // set initial values
        SetMaxHealth(stats.maxHitpoints);
        SetHealth(stats.currentHitPoints);
        targetNameText.text = stats.interactableName;
    }

    private void OnDestroy()
    {
        if (stats != null)
            stats.OnHealthChanged -= SetHealth;
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
