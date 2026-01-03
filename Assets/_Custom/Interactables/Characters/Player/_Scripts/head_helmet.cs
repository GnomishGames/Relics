using UnityEngine;

public class head_helmet : MonoBehaviour
{
    //references
    Equipment equipment;
    HelmetRig helmetRig;
    CharacterStats characterStats;
    GameObject currentHelmet;

    void Awake()
    {
        equipment = GetComponentInParent<Equipment>();
        helmetRig = GetComponentInParent<HelmetRig>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    // method to set the helmet based on an index
    public void SetHelmet(int index)
    {
        // clear any existing helmet
        ClearHelmet();  

        // check if helmetRig exists and the index is valid
        if (helmetRig != null && index >= 0 && index < helmetRig.helmetPrefabs.Length && helmetRig.helmetPrefabs[index] != null)
        {
            // instantiate the selected helmet prefab as a child of this object
            currentHelmet = Instantiate(helmetRig.helmetPrefabs[index], transform);

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


    // method to set the helmet based on the prefab name
    public void SetHelmetByName(string helmetName)
    {
        if (helmetRig == null)
        {
            Debug.LogWarning("HelmetRig reference is null. Cannot set helmet.");
            return;
        }

        // find the index of the helmet prefab with the given name
        for (int i = 0; i < helmetRig.helmetPrefabs.Length; i++)
        {
            if (helmetRig.helmetPrefabs[i] != null && helmetRig.helmetPrefabs[i].name == helmetName)
            {
                SetHelmet(i);
                return;
            }
        }

        // if we reach here, no matching helmet was found
        Debug.LogWarning("Helmet prefab with name " + helmetName + " not found in HelmetRig.");
    }

    public GameObject GetCurrentHelmet()
    {
        return currentHelmet;
    }
}