using TMPro;
using UnityEngine;

public class StatusName : MonoBehaviour
{
    TextMeshProUGUI statusNameText;
    CharacterStats characterStats;

    void Start()
    {
        characterStats = GetComponentInParent<CharacterStats>();
        statusNameText = GetComponent<TextMeshProUGUI>();
        statusNameText.text = characterStats.interactableName;

        //subscribe to name changes
        characterStats.OnNameChanged += UpdateName;
    }

    void UpdateName(string newName)
    {
        statusNameText.text = newName;
    }

    private void OnDestroy()
    {
        if (characterStats != null)
            characterStats.OnNameChanged -= UpdateName;
    }
}