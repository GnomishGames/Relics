using UnityEngine;

public class FootRig : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

    // list of foot armor
    public GameObject[] footArmor;
    
    [Header("Default Clothing")]
    [Tooltip("Default feet to show when nothing is equipped (e.g., bare feet)")]
    public GameObject defaultFeet;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();

        UpdateFootArmor("");
    }

    //activate the foot armor gameobject based on armorSO name
    private void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += UpdateFootArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += UpdateFootArmor;    
        }
    }

    private void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= UpdateFootArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= UpdateFootArmor;    
        }
    }

    private void UpdateFootArmor(string slotIndex)
    {
        //deactivate all foot armor
        foreach (GameObject feet in footArmor)
        {
            feet.SetActive(false);
        }

        //get equipped foot armorSO (foot armor is in armorSOs[9])
        ArmorSO equippedFeet = equipment.armorSOs[9];

        //if no foot armor equipped, activate default
        if (equippedFeet == null)
        {
            if (defaultFeet != null)
            {
                defaultFeet.SetActive(true);
            }
            return;
        }

        //activate the correct foot armor based on the equipped foot armorSO name
        foreach (GameObject feet in footArmor)
        {
            if (feet.name == equippedFeet.itemName)
            {
                feet.SetActive(true);
            }
        }
    }
}
