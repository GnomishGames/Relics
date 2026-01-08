using UnityEngine;
using System;

public class InventoryPanel : MonoBehaviour
{
    public int fromSlot; //Tells us where the item came from when we drag/drop
    public string fromPanel; //tells us what panel it came from

    //event when inventory panel is opened
    public event Action OnInventoryPanelOpened;
    
    //event when inventory panel is closed
    public event Action OnInventoryPanelClosed;

    private void OnEnable()
    {
        OnInventoryPanelOpened?.Invoke();
        Debug.Log("Inventory Panel Opened");
    }

    private void OnDisable()
    {
        OnInventoryPanelClosed?.Invoke();
        Debug.Log("Inventory Panel Closed");
    }
}
