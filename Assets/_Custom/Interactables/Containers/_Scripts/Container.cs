using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Container : Interactable
{
    public ItemSO[] containerItem = new ItemSO[8];

    //Interactable focus;
    Inventory inventory;
    public Transform player;

    public override void Interact()
    {
        base.Interact();
    }

    internal void UnLootItem(int inventorySlot, int containerSlot)
    {
        //focus = player.GetComponent<Focus>().focus;
        inventory = player.GetComponent<Inventory>();
        if (containerItem[containerSlot] == null)
        {
            var buffer = containerItem[containerSlot];
            containerItem[containerSlot] = inventory.inventoryItem[inventorySlot];
            inventory.inventoryItem[inventorySlot] = buffer;
        }
    }
}