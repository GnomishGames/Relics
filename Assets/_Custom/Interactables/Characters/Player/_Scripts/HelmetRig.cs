using UnityEngine;

public class HelmetRig : MonoBehaviour
{
    //private head_helmet helmetSlot;
    private Equipment equipment;
    private Inventory inventory;

    // list of helmets
    public GameObject[] helmets;

    private void Awake()
    {
        equipment = GetComponent<Equipment>();
        inventory = GetComponent<Inventory>();
    }

    //activate the helmet gameobject based on armorSO name
    private void OnEnable()
    {
        equipment.OnEquippedItemChanged += UpdateHelmet;
    }

    private void OnDisable()
    {
        equipment.OnEquippedItemChanged -= UpdateHelmet;
    }

    private void UpdateHelmet(string slotIndex)
    {
        //deactivate all helmets
        foreach (GameObject helmet in helmets)
        {
            helmet.SetActive(false);
        }

        //get equipped helmetSO (helmet is in armorSOs[0])
        ArmorSO equippedHelmet = equipment.armorSOs[0];

        //if no helmet equipped, exit
        if (equippedHelmet == null)
            return;

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