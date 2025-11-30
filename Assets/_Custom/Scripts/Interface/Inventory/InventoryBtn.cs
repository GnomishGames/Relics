using UnityEngine;

public class InventoryBtn : MonoBehaviour
{
    public GameObject inventoryPanel;

    public void ToggleInventory()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                //open inventory panel
                inventoryPanel.SetActive(true);
            }
            else
            {
                //close inventory panel
                inventoryPanel.SetActive(false);
            }
        }

    }
}
