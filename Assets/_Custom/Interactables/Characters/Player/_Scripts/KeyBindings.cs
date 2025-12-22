using System;
using UnityEngine;

//an event publisher for UI screens toggled by keybindings
public class KeyBindings : MonoBehaviour
{
    //event objects
    public event Action OnOptionsToggled;

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptions();
        }
    }

    public void ToggleOptions()
    {
        OnOptionsToggled?.Invoke();
    }
}
