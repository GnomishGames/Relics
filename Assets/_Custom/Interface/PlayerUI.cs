using UnityEngine;
using UnityEngine.EventSystems;

//this script is attached to the PlayerUI canvas in the scene
public class PlayerUI : MonoBehaviour
{
    //panel references
    public GameObject optionsPanel;   // reference to options panel
    public GameObject characterPanel; // reference to character panel

    //button references
    public GameObject optionsPanelButton;   // reference to options panel button
    public GameObject characterPanelButton; // reference to character panel button

    public KeyBindings keybindings; // reference to keybindings script

    void OnEnable()
    {
        //subscribe to KeyBindings event
        keybindings.OnOptionsToggled += ToggleOptions;
        keybindings.OnCharacterPanelToggled += ToggleCharacterPanel;
    }

    void OnDisable()
    {
        //unsubscribe from KeyBindings event
        keybindings.OnOptionsToggled -= ToggleOptions;
        keybindings.OnCharacterPanelToggled -= ToggleCharacterPanel;
    }

    //get event from KeyBindings when options menu is toggled
    public void ToggleOptions()
    {
        if (optionsPanel.activeSelf)
            optionsPanel.SetActive(false);
        else
            optionsPanel.SetActive(true);
    }

    public void ToggleCharacterPanel()
    {
        //toggle character panel
        if (characterPanel.activeSelf)
        {
            characterPanel.SetActive(false);
        }
        else
        {
            characterPanel.SetActive(true);
        }
    }
}
