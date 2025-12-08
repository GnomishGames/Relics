using System.Collections.Generic;
using UnityEngine;

public class NPCFocus : MonoBehaviour
{
    public Interactable focus;

    //lists
    public List<Focus> playersTargetingMe = new List<Focus>();

    // cached reference to this object's Interactable (owner)
    Interactable selfInteractable;

    void Update()
    {
        // ensure we have a cached reference
        if (selfInteractable == null)
            selfInteractable = GetComponent<Interactable>();

        // Remove any entries where the player is null or their Focus.focus no longer points to this interactable.
        for (int i = playersTargetingMe.Count - 1; i >= 0; i--)
        {
            Focus player = playersTargetingMe[i];
            if (player == null)
            {
                playersTargetingMe.RemoveAt(i);
                continue;
            }

            // player's Focus.focus is the Interactable they are currently targeting
            if (player.playerFocus != selfInteractable)
            {
                playersTargetingMe.RemoveAt(i);
            }
        }
    }

    void SetCharacterFocus(Character character)
    {
        if (character != focus)
        {
            if (focus != null)
            {
                focus.onDeFocus();
            }
            focus = character; //set new focus
        }
        character.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.onDeFocus();
        }
        focus = null;
    }

    public void OnFocused(Transform item)
    {
        //list of players focusing on this object
        playersTargetingMe.Add(item.GetComponent<Focus>());
    }

    public void OnDeFocus(Transform item)
    {
        //remove player from list of players focusing on this object
        playersTargetingMe.Remove(item.GetComponent<Focus>());
    }
}
