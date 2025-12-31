using UnityEngine;

public class hand_r_weapon : MonoBehaviour
{
    // an array of prefabs to choose from
    public GameObject[] weaponPrefabs;
    
    private GameObject currentWeapon;

    void Start()
    {
        SetWeapon(0); //default to first weapon
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SetWeapon(0); //equip first weapon
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            SetWeapon(1); //equip second weapon
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
            currentWeapon.transform.localPosition = Vector3.zero; // reset position
            currentWeapon.transform.localRotation = Quaternion.identity; // reset rotation 
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