using UnityEngine;

public class UIScript : MonoBehaviour
{
    public GameObject optionsPanel;
    public bool showOptionsPanel;

    private void Awake()
    {
        showOptionsPanel = false;
    }

    private void Update()
    {
        //PanelHotkeys();
    }

    public void PanelHotkeys()
    {
        //options panel toggle
        if (Input.GetKeyDown(KeyCode.Escape))
            showOptionsPanel = !showOptionsPanel;
        if (showOptionsPanel)
            optionsPanel.SetActive(true);
        if (!showOptionsPanel)
            optionsPanel.SetActive(false);
    }
}
