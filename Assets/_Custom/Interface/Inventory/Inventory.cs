using System;
using UnityEngine;

//This goes on the character and holds their inventory items
public class Inventory : MonoBehaviour
{
    //it's-a-me!
    public ItemSO[] inventoryItem = new ItemSO[8];

    //references
    Equipment equipment;
    Container container;
    Interactable focus;

    public event Action<string> OnEquippedItemChanged;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
    }

    public void MoveItem(int from, int to)
    {
        var buffer = inventoryItem[to];
        inventoryItem[to] = inventoryItem[from];
        inventoryItem[from] = buffer;
    }

    public void DestroyItem(int from)
    {
        inventoryItem[from] = null;
    }

    public void UnEquipArmor(int inventorySlot, int equipmentSlot)
    {
        if (inventoryItem[inventorySlot] == null)
        {
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName1);
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName2);
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName3);

            var buffer = inventoryItem[inventorySlot];
            inventoryItem[inventorySlot] = equipment.armorSOs[equipmentSlot];
            equipment.armorSOs[equipmentSlot] = (ArmorSO)buffer;
        }
        else if (inventoryItem[inventorySlot].slotType == equipment.armorSOs[equipmentSlot].slotType)
        {
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName1);
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName2);
            UpdateVisuals(equipment.armorSOs[equipmentSlot].VisualsName3);

            var buffer = inventoryItem[inventorySlot];
            inventoryItem[inventorySlot] = equipment.armorSOs[equipmentSlot];
            equipment.armorSOs[equipmentSlot] = (ArmorSO)buffer;
        }

        equipment.CalculateArmorClass();
    }

    public void UnEquipWeapon(int inventorySlot, int equipmentSlot)
    {
        if (inventoryItem[inventorySlot] == null)
        {
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName1);
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName2);
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName3);

            var buffer = inventoryItem[inventorySlot];
            inventoryItem[inventorySlot] = equipment.weaponSOs[equipmentSlot];
            equipment.weaponSOs[equipmentSlot] = (WeaponSO)buffer;
            
            OnEquippedItemChanged?.Invoke(inventoryItem[inventorySlot]?.itemName ?? "");
            Debug.Log("Unequip weapon event invoked: " + inventoryItem[inventorySlot]?.itemName);
        }
        else if (inventoryItem[inventorySlot].slotType == equipment.weaponSOs[equipmentSlot].slotType)
        {
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName1);
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName2);
            UpdateVisuals(equipment.weaponSOs[equipmentSlot].VisualsName3);

            var buffer = inventoryItem[inventorySlot];
            inventoryItem[inventorySlot] = equipment.weaponSOs[equipmentSlot];
            equipment.weaponSOs[equipmentSlot] = (WeaponSO)buffer;
            
            OnEquippedItemChanged?.Invoke(equipment.weaponSOs[equipmentSlot]?.itemName ?? "");
            Debug.Log("Unequip weapon event invoked: " + equipment.weaponSOs[equipmentSlot]?.itemName);
        }

        equipment.CalculateArmorClass();
    }

    internal void LootItem(int inventorySlot, int containerSlot)
    {
        var cf = GetComponent<CharacterFocus>();
        focus = cf != null ? cf.target : null;
        container = (Container)focus.GetComponent<Container>();
        if (inventoryItem[inventorySlot] == null)
        {
            var buffer = inventoryItem[inventorySlot];
            inventoryItem[inventorySlot] = container.containerItem[containerSlot];
            container.containerItem[containerSlot] = buffer;
        }
    }

    void UpdateVisuals(string name)
    {
        if (name != "" && name != null)
        {
            var children = GetComponentsInChildren<Transform>();
            foreach (var child in children)
            {
                if (child.name == name)
                {
                    child.GetChild(0).gameObject.SetActive(false);
                    break;
                }
            }
        }
    }

    public void PickupItem(Item item)
    {
        for (int i = 0; i < inventoryItem.Length; i++)
        {
            if (inventoryItem[i] == null)
            {
                inventoryItem[i] = item.GetComponent<Item>().item;
                Destroy(item.gameObject);
                break;
            }
        }
    }
}