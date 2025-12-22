using UnityEngine;

public class TogglePanel : MonoBehaviour
{
    public GameObject targetPanel;

    public void Toggle()
    {
        if (targetPanel.activeSelf)
        {
            targetPanel.SetActive(false);
        }
        else
        {
            targetPanel.SetActive(true);
        }
    }
}