using UnityEngine;

public class OpenOptionsBtn : MonoBehaviour
{
    public GameObject optionsPanel;

        public void OpenOptions()
        {
            //open options panel
            if (optionsPanel != null)
            {
                optionsPanel.SetActive(true);
            }
        }
}
