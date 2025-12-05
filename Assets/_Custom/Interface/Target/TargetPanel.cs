using UnityEngine;
using TMPro;

public class TargetPanel : MonoBehaviour
{
    public TextMeshProUGUI targetNameText;
    [Tooltip("Reference to the Focus script which stores the currently clicked target (playerTarget GameObject). If left empty the script will try to find one in the scene.")]
    public Focus focus;

    void Start()
    {
        focus = GameObject.FindWithTag("Player").GetComponent<Focus>();
    }

    void Update()
    {
        if (focus.focus == null)
        {
            targetNameText.text = "No Target";
            return;
        }else{
            //update target name text
            targetNameText.text = focus.focus.name;
        }
    }
}