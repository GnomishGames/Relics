using UnityEngine;
using UnityEngine.UI;

public class settingsPanel : MonoBehaviour
{
    public Camera cam;
    public Slider clippingSlider;
    public Slider fovSlider;

    void Start()
    {
        cam = Camera.main;
        clippingSlider.value = 100;
        fovSlider.value = cam.fieldOfView;
    }

    void Update()
    {
        //update clipping plane
        cam.farClipPlane = clippingSlider.value;
        //field of view
        cam.fieldOfView = fovSlider.value;
    }
}
