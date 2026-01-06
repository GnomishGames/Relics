using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject targetPanel;
    public GameObject[] panelsToDisable;

    public void Toggle()
    {
        if (targetPanel.activeSelf)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            targetPanel.SetActive(true);
            DisableOtherPanels();
        }
    }

    private void DisableOtherPanels()
    {
        foreach (GameObject panel in panelsToDisable)
        {
            if (panel != targetPanel)
            {
                panel.SetActive(false);
            }
        }
    }
    private void OnEnable()
    {
        DisableOtherPanels();
    }
}