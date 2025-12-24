using TMPro;
using UnityEngine;

public class Level : MonoBehaviour
{
    public TextMeshProUGUI levelText;
    private CharacterStats characterStats;

    void Start()
    {
        characterStats = GetComponentInParent<CharacterStats>();
        levelText.text = characterStats.characterLevel.ToString();

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
