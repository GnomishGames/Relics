using System;
using UnityEngine;

public class Equipment : MonoBehaviour
{
    public ArmorSO[] armorSOs = new ArmorSO[8];
    public WeaponSO[] weaponSOs = new WeaponSO[2];

    //references
    Inventory inventory;

    //events
    public event Action<float> OnAcChanged;
    public event Action<string> OnEquippedItemChanged;

    //vars
    public int ArmorAC;
    float timer;
    public GameObject prefab;
    
    private void Awake()
    {
        inventory = GetComponent<Inventory>();
    }

    private void Start()
    {
        timer = 1;
    }

    private void Update()
    {
        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            //UpdateEquipped();
            CalculateArmorClass();
            timer = 1;
        }
    }

    public void CalculateArmorClass()
    {
        ArmorAC = 0;
        for (int i = 0; i < armorSOs.Length; i++)
        {
            if (armorSOs[i] != null)
            {
                ArmorAC += armorSOs[i].ArmorBonus;
            }
        }

        for (int i = 0; i < weaponSOs.Length; i++)
        {
            if (weaponSOs[i] != null)
            {
                ArmorAC += weaponSOs[i].ArmorBonus;
            }
        }

        OnAcChanged?.Invoke(ArmorAC); //notify listeners that AC has changed
    }

    public void MoveArmor(int from, int to, SlotType slotType)
    {
        if (armorSOs[from].slotType == slotType)
        {
            var buffer = armorSOs[to];
            armorSOs[to] = armorSOs[from];
            armorSOs[from] = buffer;
        }
    }

    public void MoveWeapon(int from, int to, SlotType slotType)
    {
        if (weaponSOs[from].slotType == slotType)
        {
            var buffer = weaponSOs[to];
            weaponSOs[to] = weaponSOs[from];
            weaponSOs[from] = buffer;
        }
    }

    public void EquipArmor(int inventorySlot, int equipmentSlot, SlotType slotType)
    {
        if (inventory.inventoryItem[inventorySlot] != null && inventory.inventoryItem[inventorySlot].slotType == slotType)
        {
            var buffer = armorSOs[equipmentSlot];
            armorSOs[equipmentSlot] = (ArmorSO)inventory.inventoryItem[inventorySlot];
            inventory.inventoryItem[inventorySlot] = buffer;

            OnEquippedItemChanged?.Invoke(armorSOs[equipmentSlot].itemName);

            CalculateArmorClass();
        }
    }
    public void EquipWeapon(int inventorySlot, int equipmentSlot, SlotType slotType)
    {
        if (inventory.inventoryItem[inventorySlot] != null && inventory.inventoryItem[inventorySlot].slotType == slotType)
        {
            var buffer = weaponSOs[equipmentSlot];
            weaponSOs[equipmentSlot] = (WeaponSO)inventory.inventoryItem[inventorySlot];
            inventory.inventoryItem[inventorySlot] = buffer;

            OnEquippedItemChanged?.Invoke(weaponSOs[equipmentSlot].itemName);

            CalculateArmorClass();
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
                    child.GetChild(0).gameObject.SetActive(true);
                    break;
                }
            }
        }
    }
}