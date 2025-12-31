using UnityEngine;

public class WeaponRig : MonoBehaviour
{
    // master array of weapon prefabs to choose from
    public GameObject[] weaponPrefabs;
    
    // references to hand weapon scripts
    private hand_r_weapon rightHand;
    private hand_l_weapon leftHand;
    private Equipment equipment;
    
    void Awake()
    {
        rightHand = GetComponentInChildren<hand_r_weapon>();
        leftHand = GetComponentInChildren<hand_l_weapon>();
        equipment = GetComponentInParent<Equipment>();
    }
    
    void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += RouteWeaponToHand;
        }
        else
        {
            Debug.LogWarning("Equipment reference is null in WeaponRig. Cannot subscribe to events.");
        }
    }
    
    void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= RouteWeaponToHand;
        }
        else
        {
            Debug.LogWarning("Equipment reference is null in WeaponRig. Cannot unsubscribe from events.");
        }
    }
    
    // Route weapon to the correct hand based on its slot type
    private void RouteWeaponToHand(string weaponName)
    { Debug.Log($"Routing weapon: {weaponName}");

        // Find the weapon prefab to check its slot type
        GameObject weaponPrefab = null;
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (weaponPrefabs[i] != null && weaponPrefabs[i].name == weaponName)
            {
                weaponPrefab = weaponPrefabs[i];
                break;
            }
        }
        
        if (weaponPrefab == null)
        {
            Debug.LogWarning($"Weapon '{weaponName}' not found in weaponPrefabs array.");
            return;
        }
        
        SlotType slotType = SlotType.Primary; // default
        
        // Check weapon slots in equipment for this weapon
        for (int i = 0; i < equipment.weaponSOs.Length; i++)
        {
            if (equipment.weaponSOs[i] != null && equipment.weaponSOs[i].name == weaponName)
            {
                slotType = equipment.weaponSOs[i].slotType;
                break;
            }
        }
        
        // Route to the appropriate hand
        if (slotType == SlotType.Primary)
        {
            if (rightHand != null)
            {
                Debug.Log($"Setting weapon '{weaponName}' to right hand.");
                rightHand.SetWeaponByName(weaponName);
            }
        }
        else if (slotType == SlotType.Secondary)
        {
            if (leftHand != null)
            {
                Debug.Log($"Setting weapon '{weaponName}' to left hand.");
                leftHand.SetWeaponByName(weaponName);
            }
        }
    }
}
