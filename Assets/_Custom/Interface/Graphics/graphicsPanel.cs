using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class graphicsPanel : MonoBehaviour
{
    public Camera cam;

    //clipping plane
    public Slider clippingSlider;
    public TextMeshProUGUI clippingValueText;

    //field of view
    public Slider fovSlider;
    public TextMeshProUGUI fovValueText;

    //vsync
    public Toggle vSyncToggle;

    //antialiasing
    public TMP_Dropdown antiAliasingDropdown;

    //anisotropic filtering
    public Toggle anisotropicFilteringToggle;

    //shadow quality
    public TMP_Dropdown shadowQualityDropdown;

    //texture quality
    public TMP_Dropdown textureQualityDropdown;

    //fps counter
    public Toggle fpsToggle;
    private FPSCounter fpsCounter;

    void Start()
    {
        cam = Camera.main;
        clippingSlider.value = cam.farClipPlane;
        fovSlider.value = cam.fieldOfView;
        Application.targetFrameRate = 60; // Set target frame rate

        QualitySettings.vSyncCount = 0;  // Disable VSync
        QualitySettings.antiAliasing = 0; // Set anti-aliasing level, 0,2,4,8
        QualitySettings.anisotropicFiltering = AnisotropicFiltering.Disable; // Enable anisotropic filtering

        // shadow settings
        QualitySettings.shadowDistance = 10f; // Set shadow distance
        QualitySettings.shadowResolution = ShadowResolution.Low; // Set shadow resolution

        // texture quality
        QualitySettings.globalTextureMipmapLimit = 0; // Set texture quality (0 = Full Res, 1 = Half Res, etc.)

        // fps counter
        fpsCounter = FindFirstObjectByType<FPSCounter>();
        if (fpsToggle != null && fpsCounter != null)
        {
            fpsToggle.isOn = true;
            fpsToggle.onValueChanged.AddListener(OnFPSToggleChanged);
        }
    }

    void OnFPSToggleChanged(bool isOn)
    {
        if (fpsCounter != null)
        {
            fpsCounter.SetFPSVisibility(isOn);
        }
    }

    void Update()
    {
        //put on event listener later

        //update clipping plane
        cam.farClipPlane = clippingSlider.value;
        clippingValueText.text = cam.farClipPlane.ToString();

        //field of view
        cam.fieldOfView = fovSlider.value;
        fovValueText.text = cam.fieldOfView.ToString();

        //vsync
        QualitySettings.vSyncCount = vSyncToggle.isOn ? 1 : 0;

        //anisotropic filtering
        QualitySettings.anisotropicFiltering = anisotropicFilteringToggle.isOn ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;

        //Anti-aliasing
        switch (antiAliasingDropdown.value)
        {
            case 0:
                QualitySettings.antiAliasing = 0;
                break;
            case 1:
                QualitySettings.antiAliasing = 2;
                break;
            case 2:
                QualitySettings.antiAliasing = 4;
                break;
            case 3:
                QualitySettings.antiAliasing = 8;
                break;
        }

        //shadow quality
        switch (shadowQualityDropdown.value)
        {
            case 0:
                QualitySettings.shadowResolution = ShadowResolution.Low;
                break;
            case 1:
                QualitySettings.shadowResolution = ShadowResolution.Medium;
                break;
            case 2:
                QualitySettings.shadowResolution = ShadowResolution.High;
                break;
        }

        //texture quality
        switch (textureQualityDropdown.value)
        {
            case 0:
                QualitySettings.globalTextureMipmapLimit = 0; // Full Res       
                break;
            case 1: 
                QualitySettings.globalTextureMipmapLimit = 1; // Half Res
                break;
            case 2:
                QualitySettings.globalTextureMipmapLimit = 2; // Quarter Res
                break;
            case 3:
                QualitySettings.globalTextureMipmapLimit = 3; // Eighth Res
                break;
        }

        
    }
}
