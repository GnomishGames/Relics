using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class settingsPanel : MonoBehaviour
{
    public Camera cam;

    public Slider clippingSlider;
    public TextMeshProUGUI clippingValueText;
    
    public Slider fovSlider;
    public TextMeshProUGUI fovValueText;

    void Start()
    {
        cam = Camera.main;
        clippingSlider.value = cam.farClipPlane;
        fovSlider.value = cam.fieldOfView;
    }

    void Update()
    {
        //update clipping plane
        cam.farClipPlane = clippingSlider.value;
        clippingValueText.text = cam.farClipPlane.ToString();

        //field of view
        cam.fieldOfView = fovSlider.value;
        fovValueText.text = cam.fieldOfView.ToString();
    }
}
