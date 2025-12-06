using UnityEngine;

public class Interactable : MonoBehaviour
{
    //Iteractables are objects that the player can interact with

    //Base class for all interactable objects in the game
    public string interactableName;
    bool isFocus = false;
    //Transform playerTransform;

    void Update()
    {
        if (isFocus)
        {
            //Debug.Log("Interacting with " + transform.name);
        }
    }

    public void OnFocused(Transform item)
    {
        isFocus = true;
    }

    public virtual void Interact()
    {
        Debug.Log("Base Interact method called on " + transform.name);
    }

    public void onDeFocus()
    {
        //playerTransform = null;
    }
}