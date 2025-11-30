using UnityEngine;

public class OpenOptionsBtn : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;

        public void OpenOptions()
        {
            //open options panel
            if (optionsPanel != null)
            {
                if (!optionsPanel.activeSelf && !settingsPanel.activeSelf)
                {
                    optionsPanel.SetActive(true);
                }
                else if (optionsPanel.activeSelf)
                {
                    optionsPanel.SetActive(false);
                }

                if(settingsPanel.activeSelf)
                {
                    settingsPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                }
            }
        }
}
