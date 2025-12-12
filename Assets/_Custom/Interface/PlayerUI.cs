using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public HealthBar healthBar;       // reference to visible UI bar
    public EXPBar EXPBar;             // reference to visible EXP bar
    public CharacterStats playerStats; // reference to the player's stats

    void Start()
    {
        healthBar.Initialize(playerStats);
        EXPBar.Initialize(playerStats);
    }
}
