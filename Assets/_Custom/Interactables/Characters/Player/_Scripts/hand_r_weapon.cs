using UnityEngine;

public class hand_r_weapon : MonoBehaviour
{
    // an array of prefabs to choose from
    public GameObject[] weaponPrefabs;

    private GameObject currentWeapon;

    //references
    Equipment equipment;

    void Awake()
    {
        equipment = GetComponentInParent<Equipment>();
    }

    void OnEnable()
    {
        //subscribe to events or initialize as needed
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += SetWeaponByName;
        }
    }

    void OnDestroy()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= SetWeaponByName;
        }
    }

    // method to set the weapon based on an index
    public void SetWeapon(int index)
    {
        // clear any existing weapon
        ClearWeapon();

        // check if the index is valid
        if (index >= 0 && index < weaponPrefabs.Length && weaponPrefabs[index] != null)
        {
            // instantiate the selected weapon prefab as a child of this object
            currentWeapon = Instantiate(weaponPrefabs[index], transform);

            //i might want to do this later so keep it commented out for now
            //currentWeapon.transform.localPosition = Vector3.zero; // reset position
            //currentWeapon.transform.localRotation = Quaternion.identity; // reset rotation 
        }
    }

    // method to set the weapon based on the prefab name
    public void SetWeaponByName(string weaponName)
    {
        // find the weapon prefab with the matching name
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (weaponPrefabs[i] != null && weaponPrefabs[i].name == weaponName)
            {
                SetWeapon(i);
                return;
            }
        }

        // if we get here, no weapon with that name was found
        Debug.LogWarning($"Weapon '{weaponName}' not found in weaponPrefabs array.");
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