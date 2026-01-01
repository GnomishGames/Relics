using UnityEngine;

public class WeaponRig : MonoBehaviour
{
    // master array of weapon prefabs to choose from
    public GameObject[] weaponPrefabs;
    
    // references to hand weapon scripts
    private hand_r_weapon rightHand;
    private hand_l_weapon leftHand;
    private Equipment equipment;
    private Inventory inventory;
    
    void Awake()
    {
        rightHand = GetComponentInChildren<hand_r_weapon>();
        leftHand = GetComponentInChildren<hand_l_weapon>();
        equipment = GetComponentInParent<Equipment>();
        inventory = GetComponentInParent<Inventory>();
        
        Debug.Log($"WeaponRig Awake - Equipment: {(equipment != null ? "Found" : "NULL")}, Inventory: {(inventory != null ? "Found" : "NULL")}");
        Debug.Log($"WeaponRig on GameObject: {gameObject.name}, InstanceID: {GetInstanceID()}");
    }
    
    void OnEnable()
    {
        Debug.Log($"[WeaponRig OnEnable] on {gameObject.name}, InstanceID: {GetInstanceID()}");
        
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += RouteWeaponToHand;
            Debug.Log("Successfully subscribed to equipment.OnEquippedItemChanged");
        }
        else
        {
            Debug.LogWarning("Equipment reference is null in WeaponRig. Cannot subscribe to events.");
        }
        
        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += RouteWeaponToHand;
            Debug.Log($"Successfully subscribed to inventory.OnEquippedItemChanged on Inventory InstanceID: {inventory.GetInstanceID()}");
        }
        else
        {
            Debug.LogWarning("Inventory reference is null in WeaponRig. Cannot subscribe to events.");
        }
    }
    
    void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= RouteWeaponToHand;
        }
        
        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= RouteWeaponToHand;
        }
    }
    
    // Route weapon to the correct hand based on its slot type
    private void RouteWeaponToHand(string weaponName)
    {
        Debug.Log($"[WeaponRig] RouteWeaponToHand called with weaponName: '{weaponName}'");

        if (equipment == null)
        {
            Debug.LogError("Equipment is null in RouteWeaponToHand!");
            return;
        }

        // Check each weapon slot to see what's equipped
        // Slot 0 is typically Primary (right hand), Slot 1 is Secondary (left hand)
        for (int i = 0; i < equipment.weaponSOs.Length; i++)
        {
            WeaponSO weapon = equipment.weaponSOs[i];
            
            Debug.Log($"[WeaponRig] Slot {i}: {(weapon != null ? weapon.itemName : "EMPTY")}");
            
            if (weapon != null)
            {
                // There's a weapon in this slot - equip it to the appropriate hand
                if (weapon.slotType == SlotType.Primary && rightHand != null)
                {
                    Debug.Log($"Equipping '{weapon.itemName}' to right hand.");
                    rightHand.SetWeaponByName(weapon.itemName);
                }
                else if (weapon.slotType == SlotType.Secondary && leftHand != null)
                {
                    Debug.Log($"Equipping '{weapon.itemName}' to left hand.");
                    leftHand.SetWeaponByName(weapon.itemName);
                }
            }
            else
            {
                // Slot is empty - clear the appropriate hand
                if (i == 0 && rightHand != null) // Assuming slot 0 is primary/right hand
                {
                    Debug.Log("Clearing right hand.");
                    rightHand.ClearWeapon();
                }
                else if (i == 1 && leftHand != null) // Assuming slot 1 is secondary/left hand
                {
                    Debug.Log("Clearing left hand.");
                    leftHand.ClearWeapon();
                }
            }
        }
    }
}
