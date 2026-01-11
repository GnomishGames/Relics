using UnityEngine;

public class ChestRig : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

    // list of chest armor
    public GameObject[] chestArmor;
    
    [Header("Default Clothing")]
    [Tooltip("Default chest to show when nothing is equipped (e.g., underwear or bare chest)")]
    public GameObject defaultChest;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        UpdateChestArmor("");
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

        //if no chest armor equipped, activate default
        if (equippedChest == null)
        {
            if (defaultChest != null)
            {
                defaultChest.SetActive(true);
            }
            return;
        }

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