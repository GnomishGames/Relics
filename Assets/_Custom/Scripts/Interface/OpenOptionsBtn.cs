using UnityEngine;

public class OpenOptionsBtn : MonoBehaviour
{
    public GameObject optionsPanel;
    public GameObject settingsPanel;

        public void OpenOptions()
        {
            Debug.Log("OpenOptionsBtn clicked");
            //open options panel
            if (optionsPanel != null)
            {
                if (!optionsPanel.activeSelf && !settingsPanel.activeSelf)
                {
                    optionsPanel.SetActive(true);
                    Debug.Log("Options panel opened");
                }
                else if (optionsPanel.activeSelf)
                {
                    optionsPanel.SetActive(false);
                    Debug.Log("Options panel closed");
                }

                if(settingsPanel.activeSelf)
                {
                    settingsPanel.SetActive(false);
                    optionsPanel.SetActive(true);
                    Debug.Log("Settings panel closed, Options panel opened");
                }


            }


        }
}
