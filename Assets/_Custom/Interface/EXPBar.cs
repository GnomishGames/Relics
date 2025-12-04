using UnityEngine;
using UnityEngine.UI;

public class EXPBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxEXP(int maxEXP, int minEXP)
    {
        slider.maxValue = maxEXP;
        slider.minValue = minEXP;
        //slider.value = maxEXP;
    }

    public void SetEXP(float EXP)
    {
        slider.maxValue = 1;
        slider.minValue = 0;
        slider.value = EXP;
    }
}
