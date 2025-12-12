using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public Slider slider;

    private CharacterStats stats;   // the specific stats this bar listens to

    public void Initialize(CharacterStats target)
    {
        // unsubscribe if reused
        if (stats != null)
            stats.OnEXPChanged -= SetEXP;

        stats = target;

        // subscribe to THIS character
        stats.OnEXPChanged += SetEXP;

        // set initial values
        SetEXP(stats.experience);
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
