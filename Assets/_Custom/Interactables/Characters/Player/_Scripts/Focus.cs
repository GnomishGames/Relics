using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Focus : MonoBehaviour
{
    Camera cam;

    public Interactable focus;
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
        //don't click throug the UI
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
                    //character.GetComponent<Focus>().OnFocused(transform); //tell the character it's being focused
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
        if (character != focus)
        {
            if (focus != null)
            {
                focus.onDeFocus();
                character.GetComponent<NPCFocus>().OnDeFocus(transform);
            }
            focus = character; //set new focus

            //add this player to the character's list of players targeting it
            character.GetComponent<NPCFocus>().OnFocused(transform);
        }
        character.OnFocused(transform);
    }
    
    void SetItemFocus(Item item)
    {
        if (item != focus)
        {
            if (focus != null)
            {
                focus.onDeFocus();
            }
            focus = item;
        }
        item.OnFocused(transform);

        inventory.PickupItem(item);
    }

    void SetContainerFocus(Container container)
    {
        if (container != focus) 
        {
            if (focus != null) 
            {
                focus.onDeFocus();
            }
            focus = container;
        }
        container.OnFocused(transform);
    }

    void RemoveFocus()
    {
        if (focus != null)
        {
            focus.onDeFocus();
        }
        focus = null;
    }

    void OnFocused(Transform item)
    {
        //list of players focusing on this object
        playersTargetingMe.Add(item.GetComponent<Interactable>());
    }


}