using System;
using Gaia;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class graphicsPanel : MonoBehaviour
{
    Camera cam;

    //events
    public event Action<float> OnDetailDistanceChanged;
    public event Action<float> OnDetailDensityChanged;
    public event Action<int> OnTextureQualityChanged;
    public event Action<int> OnShadowQualityChanged;
    public event Action<int> OnAntialiasingChanged;
    public event Action<bool> OnAnisotropicFilteringChanged;
    public event Action<bool> OnVsyncChanged;
    public event Action<float> OnFieldOfViewChanged;
    public event Action<float> OnClippingPlaneChanged;

    //clipping plane
    public Slider clippingSlider;

    //field of view
    public Slider fovSlider;

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
    public FPSCounter fpsCounter;

    //terrain
    public Terrain targetTerrain;

    //terrain detail distance
    public Slider detailDistanceSlider;

    //terrain detail density
    public Slider detailDensitySlider;

    void Start()
    {
        cam = Camera.main;
        //fps counter
        fpsCounter = FindFirstObjectByType<FPSCounter>();
        if (fpsToggle != null && fpsCounter != null)
        {
            fpsToggle.isOn = true;
            fpsToggle.onValueChanged.AddListener(OnFPSToggleChanged);
        }

        // Get the terrain component if not already assigned
        if (targetTerrain == null)
        {
            targetTerrain = Terrain.activeTerrain;
        }

        detailDistanceSlider.onValueChanged.AddListener(OnDetailDistanceSliderChanged);
        detailDensitySlider.onValueChanged.AddListener(OnDetailDensitySliderChanged);
        textureQualityDropdown.onValueChanged.AddListener(OnTextureQualityDropdownChanged);
        shadowQualityDropdown.onValueChanged.AddListener(OnShadowQualityDropdownChanged);
        antiAliasingDropdown.onValueChanged.AddListener(OnAntiAliasingDropdownChanged);
        anisotropicFilteringToggle.onValueChanged.AddListener(OnAnostropicFilteringToggleChanged);
        vSyncToggle.onValueChanged.AddListener(OnVsyncToggleChanged);
        fovSlider.onValueChanged.AddListener(OnFieldOfViewSliderChanged);
        clippingSlider.onValueChanged.AddListener(OnClippingPlaneSliderChanged);

        //update the slider values to match current settings
        if (cam == null)
            return;
        fovSlider.value = cam.fieldOfView;
        clippingSlider.value = cam.farClipPlane;
        detailDensitySlider.value = targetTerrain.detailObjectDensity;
        detailDistanceSlider.value = targetTerrain.detailObjectDistance;

    }

    void OnFPSToggleChanged(bool isOn)
    {
        if (fpsCounter != null)
        {
            fpsCounter.SetFPSVisibility(isOn);
        }
    }

    private void OnDetailDistanceSliderChanged(float value)
    {
        OnDetailDistanceChanged?.Invoke(value);
    }

    private void OnDetailDensitySliderChanged(float value)
    {
        OnDetailDensityChanged?.Invoke(value);
    }

    private void OnTextureQualityDropdownChanged(int value)
    {
        OnTextureQualityChanged?.Invoke(value);
    }

    private void OnShadowQualityDropdownChanged(int value)
    {
        OnShadowQualityChanged?.Invoke(value);
    }

    private void OnAntiAliasingDropdownChanged(int value)
    {
        OnAntialiasingChanged?.Invoke(value);
    }

    private void OnAnostropicFilteringToggleChanged(bool isOn)
    {
        OnAnisotropicFilteringChanged?.Invoke(isOn);
    }

    private void OnVsyncToggleChanged(bool isOn)
    {
        OnVsyncChanged?.Invoke(isOn);
    }

    private void OnFieldOfViewSliderChanged(float value)
    {
        OnFieldOfViewChanged?.Invoke(value);
    }

    private void OnClippingPlaneSliderChanged(float value)
    {
        OnClippingPlaneChanged?.Invoke(value);
    }
}