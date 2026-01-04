using UnityEngine;

public class HelmetRig : MonoBehaviour
{
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
            equipment.OnEquippedItemChanged += RouteHelmetToSlot;
        }
        else
        {
            Debug.LogWarning("Equipment reference is null in HelmetRig. Cannot subscribe to events.");
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += RouteHelmetToSlot;
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

        ArmorSO armorSO = equipment.armorSOs[0]; // assuming helmet is in armor slot 0

        if (armorSO != null)
        {
            helmetSlot.SetHelmet(armorSO);
        }
        else
        {
            helmetSlot.ClearHelmet();
        }
    }
}
