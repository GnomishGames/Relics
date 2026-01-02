using UnityEngine;

public class hand_r_weapon : MonoBehaviour
{
    private GameObject currentWeapon;

    //references
    Equipment equipment;
    WeaponRig weaponRig;
    CharacterStats characterStats;

    void Awake()
    {
        equipment = GetComponentInParent<Equipment>();
        weaponRig = GetComponentInParent<WeaponRig>();
        characterStats = GetComponentInParent<CharacterStats>();
    }

    // method to set the weapon based on an index
    public void SetWeapon(int index)
    {
        // clear any existing weapon
        ClearWeapon();

        // check if weaponRig exists and the index is valid
        if (weaponRig != null && index >= 0 && index < weaponRig.weaponPrefabs.Length && weaponRig.weaponPrefabs[index] != null)
        {
            // instantiate the selected weapon prefab as a child of this object
            currentWeapon = Instantiate(weaponRig.weaponPrefabs[index], transform);

            //make sure that green (y axis) points up
            Transform gripPoint = currentWeapon.transform.Find("GripPoint");
            if (gripPoint != null)
            {
                currentWeapon.transform.localPosition = -gripPoint.localPosition;
                currentWeapon.transform.localRotation = Quaternion.Inverse(gripPoint.localRotation);
            }

            // Apply race-specific offset
            if (characterStats != null && characterStats.characterRace != null)
            {
                currentWeapon.transform.localPosition += characterStats.characterRace.rightHandOffset;
            }
        }
    }

    // method to set the weapon based on the prefab name
    public void SetWeaponByName(string weaponName)
    {
        if (weaponRig == null)
        {
            Debug.LogWarning("WeaponRig reference is null. Cannot set weapon.");
            return;
        }

        // find the weapon prefab with the matching name
        for (int i = 0; i < weaponRig.weaponPrefabs.Length; i++)
        {
            if (weaponRig.weaponPrefabs[i] != null && weaponRig.weaponPrefabs[i].name == weaponName)
            {
                SetWeapon(i);
                return;
            }
        }

        // if we get here, no weapon with that name was found
        Debug.LogWarning($"Weapon '{weaponName}' not found in weaponPrefabs array.");
        ClearWeapon();
    }

    // method to clear/unequip current weapon
    public void ClearWeapon()
    {
        if (currentWeapon != null)
        {
            Destroy(currentWeapon);
            currentWeapon = null;
        }
    }

    // method to show/hide current weapon without destroying it
    public void SetWeaponActive(bool active)
    {
        if (currentWeapon != null)
        {
            currentWeapon.SetActive(active);
        }
    }

    // get the currently equipped weapon
    public GameObject GetCurrentWeapon()
    {
        return currentWeapon;
    }
}