using UnityEngine;

public class ChestRig : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

    // list of chest armor
    public GameObject[] chestArmor;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
    }

    //activate the chest armor gameobject based on armorSO name
    private void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += UpdateChestArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += UpdateChestArmor;    
        }
    }

    private void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= UpdateChestArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= UpdateChestArmor;    
        }
    }

    private void UpdateChestArmor(string slotIndex)
    {
        //deactivate all chest armor
        foreach (GameObject chest in chestArmor)
        {
            chest.SetActive(false);
        }

        //get equipped chest armorSO (chest armor is in armorSOs[2])
        ArmorSO equippedChest = equipment.armorSOs[2];

        //if no chest armor equipped, exit
        if (equippedChest == null)
            return;

        //activate the correct chest armor based on the equipped chest armorSO name
        foreach (GameObject chest in chestArmor)
        {
            if (chest.name == equippedChest.itemName)
            {
                chest.SetActive(true);
                break;
            }
        }
    }


}