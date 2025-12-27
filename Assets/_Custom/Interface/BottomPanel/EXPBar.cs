using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public Slider slider;

    private CharacterStats characterStats;   // the specific stats this bar listens to

    private void Awake()
    {
        // Get reference to CharacterStats if not set
        characterStats = GetComponentInParent<CharacterStats>();

        // Subscribe to EXP changes for updates
        characterStats.OnEXPChanged += SetEXP;
    }

    public void SetMaxEXP(float maxEXP, float minEXP)
    {
        slider.maxValue = maxEXP;
        slider.minValue = minEXP;
    }

    //this is a percentage value between min and max EXP for the current level
    public void SetEXP(float EXP)
    {
        slider.maxValue = 1;
        slider.minValue = 0;
        slider.value = EXP;
    }
}
