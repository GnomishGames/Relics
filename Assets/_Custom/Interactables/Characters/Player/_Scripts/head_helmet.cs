using UnityEngine;

public class head_helmet : MonoBehaviour
{
    //references
    Equipment equipment;
    CharacterStats characterStats;
    GameObject currentHelmet;

    void Awake()
    {
        equipment = GetComponentInParent<Equipment>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    // method to set the helmet based on ArmorSO
    public void SetHelmet(ArmorSO armorData)
    {
        // clear any existing helmet
        ClearHelmet();  

        // check if armorData and itemPrefab exist
        if (armorData != null && armorData.itemPrefab != null)
        {
            // instantiate the helmet prefab as a child of this object
            currentHelmet = Instantiate(armorData.itemPrefab, transform);

            //make sure that green (y axis) points up and blue (z axis) points forward
            Transform fitPoint = currentHelmet.transform.Find("FitPoint");
            if (fitPoint != null)
            {
                currentHelmet.transform.localPosition = -fitPoint.localPosition;
                currentHelmet.transform.localRotation = Quaternion.Inverse(fitPoint.localRotation);
            }

            // Apply race-specific offset
            if (characterStats != null && characterStats.characterRace != null)
            {
                currentHelmet.transform.localPosition += characterStats.characterRace.helmetOffset;
            }
        }
    }

    // method to clear/unequip current helmet
    public void ClearHelmet()
    {
        if (currentHelmet != null)
        {
            Destroy(currentHelmet);
            currentHelmet = null;
        }
    }

    public GameObject GetCurrentHelmet()
    {
        return currentHelmet;
    }
}