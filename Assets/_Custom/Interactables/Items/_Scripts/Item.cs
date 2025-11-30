using UnityEngine;

public class Item : Interactable
{
    //Items are things that can be picked up and used by the player

    // Item properties
    public Sprite itemIcon;
    public float itemWeight;
    public int itemHealth;

    public override void Interact()
    {
        base.Interact();
    }
}
