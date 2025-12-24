using System;
using TMPro;
using UnityEngine;

//an event publisher for UI screens toggled by keybindings
public class KeyBindings : MonoBehaviour
{
    //event objects
    public event Action OnOptionsToggled;
    public event Action OnInventoryToggled;

    //references
    public TMP_InputField chatBox;

    public void Update()
    {
        if (!chatBox.isFocused)
        {
            if (Input.GetKeyDown(KeyCode.O)) ToggleOptions();
            if (Input.GetKeyDown(KeyCode.I)) ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        OnInventoryToggled?.Invoke();
    }

    public void ToggleOptions()
    {
        OnOptionsToggled?.Invoke();
    }
}
