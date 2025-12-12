using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public HealthBar healthBar;       // reference to visible UI bar
    public StaminaBar staminaBar;   // reference to visible Stamina bar
    public EXPBar EXPBar;             // reference to visible EXP bar
    public CharacterStats playerStats;// reference to the player's stats
    public GameObject optionsPanel;   // reference to options panel

    public KeyBindings keybindings; // reference to keybindings script

    void Start()
    {
        healthBar.Initialize(playerStats);
        staminaBar.Initialize(playerStats);
        EXPBar.Initialize(playerStats);

        //subscribe to KeyBindings event
        keybindings.OnOptionsToggled += ToggleOptions;
    }

    //get event from KeyBindings when options menu is toggled
    public void ToggleOptions()
    {
        if (optionsPanel.activeSelf)
        {
            optionsPanel.SetActive(false);
        }
        else
        {
            optionsPanel.SetActive(true);
        }
    }
}
