using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Focus : MonoBehaviour
{
    //references
    Camera cam;
    public Interactable playerFocus;
    Inventory inventory;
    //public ContainerPanel containerPanel;

    //lists
    public List<Interactable> playersTargetingMe = new List<Interactable>();

    void Awake()
    {
        cam = Camera.main;

        inventory = GetComponentInChildren<Inventory>();
    }

    void Update()
    {
        //don't click through the UI
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        //left mouse click sets focus
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100))
            {
                Character character = hit.collider.GetComponent<Character>();
                Item item = hit.collider.GetComponent<Item>();
                Container container = hit.collider.GetComponent<Container>();

                if (character != null)
                {
                    SetCharacterFocus(character); //if i click on character
                }
                if (item != null)
                {
                    SetItemFocus(item); //pick up item if i click on it
                }
                if (container != null)
                {
                    SetContainerFocus(container); //open container if i click on it
                }
            }
        }
    }

    void SetCharacterFocus(Character character)
    {
        if (character != playerFocus)
        {
            if (playerFocus != null)
            {
                playerFocus.onDeFocus();
                character.GetComponent<NPCFocus>().OnDeFocus(transform);
            }
            playerFocus = character; //set new focus

            //add this player to the character's list of players targeting it
            character.GetComponent<NPCFocus>().OnFocused(transform);
        }
        character.OnFocused(transform);
    }
    
    void SetItemFocus(Item item)
    {
        if (item != playerFocus)
        {
            if (playerFocus != null)
            {
                playerFocus.onDeFocus();
            }
            playerFocus = item;
        }
        item.OnFocused(transform);

        inventory.PickupItem(item);
    }

    void SetContainerFocus(Container container)
    {
        if (container != playerFocus) 
        {
            if (playerFocus != null) 
            {
                playerFocus.onDeFocus();
            }
            playerFocus = container;
        }
        container.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (playerFocus != null)
        {
            playerFocus.onDeFocus();
        }
        playerFocus = null;
    }

    void OnFocused(Transform item)
    {
        //list of players focusing on this object
        playersTargetingMe.Add(item.GetComponent<Interactable>());
    }


}