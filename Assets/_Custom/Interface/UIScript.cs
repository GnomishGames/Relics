using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;

    private void Update()
    {
        //PanelHotkeys();
    }

    public void PanelHotkeys()
    {
        //options panel toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsPanel != null && !settingsPanel.activeSelf && settingsPanel != null)
            {
                if (!optionsPanel.activeSelf)
                {
                    optionsPanel.SetActive(true);
                }
                else
                {
                    optionsPanel.SetActive(false);
                }
            }
        }

        //settings panel toggle
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(optionsPanel != null && !optionsPanel.activeSelf && settingsPanel != null)
            {
                if (settingsPanel.activeSelf)
                {
                    settingsPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                }
            }
        }
    }
}
