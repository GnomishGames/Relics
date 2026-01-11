using UnityEngine;

public class LegsRig : MonoBehaviour
{
    private Equipment equipment;
    private Inventory inventory;

    // list of leg armor
    public GameObject[] legArmor;
    
    [Header("Default Clothing")]
    [Tooltip("Default legs to show when nothing is equipped (e.g., underwear or bare legs)")]
    public GameObject defaultLegs;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        UpdateLegArmor("");
    }

    //activate the leg armor gameobject based on armorSO name
    private void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += UpdateLegArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += UpdateLegArmor;    
        }
    }

    private void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= UpdateLegArmor;    
        }

        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= UpdateLegArmor;    
        }
    }

    private void UpdateLegArmor(string slotIndex)
    {
        //deactivate all leg armor
        foreach (GameObject legs in legArmor)
        {
            legs.SetActive(false);
        }

        //get equipped leg armorSO (leg armor is in armorSOs[4])
        ArmorSO equippedLegs = equipment.armorSOs[4];

        //if no leg armor equipped, activate default
        if (equippedLegs == null)
        {
            if (defaultLegs != null)
            {
                defaultLegs.SetActive(true);
            }
            return;
        }

        //activate the correct leg armor based on the equipped leg armorSO name
        foreach (GameObject legs in legArmor)
        {
            if (legs.name == equippedLegs.itemName)
            {
                legs.SetActive(true);
            }
        }
    }
}
