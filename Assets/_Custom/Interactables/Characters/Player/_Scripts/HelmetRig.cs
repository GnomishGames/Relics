using UnityEngine;

public class HelmetRig : MonoBehaviour
{
    //private head_helmet helmetSlot;
    private Equipment equipment;
    private Inventory inventory;

    // list of helmets
    public GameObject[] helmets;
    
    [Header("Default Clothing")]
    [Tooltip("Default helmet to show when nothing is equipped (e.g., hair or bare head)")]
    public GameObject defaultHelmet;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
        UpdateHelmet("");
    }

    //activate the helmet gameobject based on armorSO name
    private void OnEnable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged += UpdateHelmet;
        }
        
        if (inventory != null)
        {
            inventory.OnEquippedItemChanged += UpdateHelmet;
        }
    }

    private void OnDisable()
    {
        if (equipment != null)
        {
            equipment.OnEquippedItemChanged -= UpdateHelmet;
        }
        
        if (inventory != null)
        {
            inventory.OnEquippedItemChanged -= UpdateHelmet;
        }
    }

    private void UpdateHelmet(string slotIndex)
    {
        //deactivate all helmets
        foreach (GameObject helmet in helmets)
        {
            helmet.SetActive(false);
            defaultHelmet.SetActive(false);
        }

        //get equipped helmetSO (helmet is in armorSOs[0])
        ArmorSO equippedHelmet = equipment.armorSOs[0];

        //if no helmet equipped, activate default
        if (equippedHelmet == null)
        {
            if (defaultHelmet != null)
            {
                defaultHelmet.SetActive(true);
            }
            return;
        }

        //activate the correct helmet based on the equipped helmetSO name
        foreach (GameObject helmet in helmets)
        {
            if (helmet.name == equippedHelmet.name)
            {
                helmet.SetActive(true);
                break;
            }
        }
    }
}