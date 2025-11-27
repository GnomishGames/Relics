using UnityEngine;
using UnityEngine.UI;

public class CloseSettingsBtn : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;

    public void ClosePanel()
    {
        //close settings panel
        if (settingsPanel != null)
        {
            settingsPanel.SetActive(false);
        }

        //open options panel when closing settings
        if (optionsPanel != null)
        {
            optionsPanel.SetActive(true);
        }
    }
}
