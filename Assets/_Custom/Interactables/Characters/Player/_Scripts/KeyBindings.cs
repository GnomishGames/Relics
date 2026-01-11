using System;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//an event publisher for UI screens toggled by keybindings
public class KeyBindings : MonoBehaviour
{
    //event objects
    public event Action OnOptionsToggled;
    public event Action OnCharacterPanelToggled;

    //references
    public TMP_InputField chatBox;

    // new input system
    private InputAction optionsAction;
    private InputAction characterPanelAction;

    void OnEnable()
    {
        // Find and enable Input Actions
        optionsAction = InputSystem.actions.FindAction("Options");
        characterPanelAction = InputSystem.actions.FindAction("Inventory");
        
        if (optionsAction != null)
        {
            optionsAction.Enable();
            optionsAction.performed += OnOptionsPerformed;
        }
        
        if (characterPanelAction != null)
        {
            characterPanelAction.Enable();
            characterPanelAction.performed += OnCharacterPanelPerformed;
        }
    }

    void OnDisable()
    {
        // Unsubscribe and disable actions
        if (optionsAction != null)
        {
            optionsAction.performed -= OnOptionsPerformed;
            optionsAction.Disable();
        }
        
        if (characterPanelAction != null)
        {
            characterPanelAction.performed -= OnCharacterPanelPerformed;
            characterPanelAction.Disable();
        }
    }

    private void OnOptionsPerformed(InputAction.CallbackContext context)
    {
        // Only trigger if chat is not focused
        if (!chatBox.isFocused)
        {
            ToggleOptions();
        }
    }

    private void OnCharacterPanelPerformed(InputAction.CallbackContext context)
    {
        // Only trigger if chat is not focused
        if (!chatBox.isFocused)
        {
            ToggleCharacterPanel();
        }
    }

    public void ToggleOptions()
    {
        OnOptionsToggled?.Invoke();
    }

    private void ToggleCharacterPanel()
    {
        OnCharacterPanelToggled?.Invoke();
    }
}
