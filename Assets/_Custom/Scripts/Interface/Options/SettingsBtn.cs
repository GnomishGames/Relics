using UnityEngine;

public class SettingsBtn : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;
    
    public void OpenSettings()
    {
        //open settings panel
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(true);
        }
        
        //close options panel when opening settings
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(false);
            Debug.Log("Options panel closed");
        }
    }
}
