using UnityEngine;

public class hand_l_weapon : MonoBehaviour
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

    // Note: WeaponRig now handles routing weapons to the correct hand
    // based on slot type, so we don't subscribe directly to equipment events here

    // method to set the weapon based on the prefab name
    public void SetWeapon(WeaponSO weaponData)
    {
        // clear any existing weapon
        ClearWeapon();

        //check if weaponData and itemPrefab exist
        if (weaponData != null && weaponData.itemPrefab != null)
        {
            //instantiate the weapon prefab as a child of this object
            currentWeapon = Instantiate(weaponData.itemPrefab, transform);

            //make sure that green (y axis) points up and blue (z axis) points forward
            Transform gripPoint = currentWeapon.transform.Find("GripPoint");
            if (gripPoint != null)
            {
                currentWeapon.transform.localPosition = -gripPoint.localPosition;
                currentWeapon.transform.localRotation = Quaternion.Inverse(gripPoint.localRotation);
            }

            // Apply race specific offset
            if (characterStats != null && characterStats.characterRace != null)
            {
                currentWeapon.transform.localPosition += characterStats.characterRace.leftHandOffset;
            }
        }
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
