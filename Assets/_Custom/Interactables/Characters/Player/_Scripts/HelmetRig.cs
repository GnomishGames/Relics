using UnityEngine;

public class HelmetRig : MonoBehaviour
{
    public GameObject[] helmetPrefabs;

    private head_helmet helmetSlot;
    private Equipment equipment;
    private Inventory inventory;

    void Awake()
    {
        helmetSlot = GetComponentInChildren<head_helmet>();
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
    }

    void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += RouteHelmetToSlot; // subscribe to equipment changes
        }
        else
        {
            Debug.LogWarning("Equipment reference is null in HelmetRig. Cannot subscribe to events.");
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += RouteHelmetToSlot; // subscribe to inventory changes
        }
        else
        {
            Debug.LogWarning("Inventory reference is null in HelmetRig. Cannot subscribe to events.");
        }
    }

    void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= RouteHelmetToSlot;
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= RouteHelmetToSlot;
        }
    }

    private void RouteHelmetToSlot(string helmetName)
    {
        if (equipment == null)
        {
            Debug.LogWarning("Equipment reference is null in HelmetRig. Cannot route helmet to slot.");
            return;
        }

        ArmorSO armorSO = equipment.armorSOs[0]; // assuming helmet is in armor slot 0);

        if (armorSO != null)
        {
            foreach (GameObject prefab in helmetPrefabs)
            {
                if (prefab.name == armorSO.itemName)
                {
                    helmetSlot.SetHelmetByName(prefab.name);
                    return;
                }
            }
            Debug.LogWarning("Helmet prefab not found for equipped helmet: " + armorSO.itemName);
        }
        else
        {
            helmetSlot.ClearHelmet();
        }
    }
}
