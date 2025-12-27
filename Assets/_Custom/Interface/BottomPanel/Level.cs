using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    private CharacterStats characterStats;

    void Awake()
    {
        characterStats = GetComponentInParent<CharacterStats>();

        //subscribe to level changes
        characterStats.OnLevelChanged += UpdateLevel;
    }

    void UpdateLevel(float newLevel)
    {
        levelText.text = newLevel.ToString();
    }

    private void OnDestroy()
    {
        if (characterStats != null)
            characterStats.OnLevelChanged -= UpdateLevel;
    }
}
