using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsOptions : MonoBehaviour
{
    //terrain
    public Terrain targetTerrain;
    
    //camera reference
    public Camera cam;

    //referece to graphics panel texts
    public graphicsPanel graphicsPanelRef;
    public TextMeshProUGUI clippingValueText;
    public TextMeshProUGUI fovValueText;
    public TMP_Text detailDistanceValueText;
    public TMP_Text detailDensityValueText;

    void Start()
    {
        cam = Camera.main;

        // Get the terrain component if not already assigned
        if (targetTerrain == null)
        {
            targetTerrain = Terrain.activeTerrain;
        }

        //subscribe to detail distance change event
        if (graphicsPanelRef != null)
        {
            graphicsPanelRef.OnDetailDistanceChanged += AdjustDetailDistance;
            graphicsPanelRef.OnDetailDensityChanged += AdjustDetailDensity;
            graphicsPanelRef.OnTextureQualityChanged += AdjustTextureQuality;
            graphicsPanelRef.OnShadowQualityChanged += AdjustShadowQuality;
            graphicsPanelRef.OnAntialiasingChanged += AdjustAntialiasing;
            graphicsPanelRef.OnAnisotropicFilteringChanged += AdjustAnisotropicFiltering;
            graphicsPanelRef.OnVsyncChanged += AdjustVsync;
            graphicsPanelRef.OnFieldOfViewChanged += AdjustFieldOfView;
            graphicsPanelRef.OnClippingPlaneChanged += AdjustClippingPlane;

        }

        //set inital text values for fov and clipping plane, detail distance and density
        if (clippingValueText != null)
            clippingValueText.text = cam.farClipPlane.ToString();
        if (fovValueText != null)
            fovValueText.text = cam.fieldOfView.ToString();
        if (detailDistanceValueText != null && targetTerrain != null)
            detailDistanceValueText.text = targetTerrain.detailObjectDistance.ToString();
        if (detailDensityValueText != null && targetTerrain != null)
            detailDensityValueText.text = targetTerrain.detailObjectDensity.ToString();
    }

    private void AdjustDetailDistance(float value)
    {
        //terrain detail distance and density
        if (targetTerrain != null)
        {
            //terrain detail distance
            targetTerrain.detailObjectDistance = value;
            detailDistanceValueText.text = value.ToString();
        }
    }

    private void AdjustDetailDensity(float value)
    {
        //terrain detail density
        if (targetTerrain != null)
        {
            targetTerrain.detailObjectDensity = value;
            detailDensityValueText.text = value.ToString();
        }
    }

    private void AdjustTextureQuality(int value)
    {
        //texture quality
        switch (value)
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

    private void AdjustShadowQuality(int value)
    {
        //shadow quality
        switch (value)
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
    }

    private void AdjustAntialiasing(int value)
    {
        //Anti-aliasing
        switch (value)
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
    }

    private void AdjustAnisotropicFiltering(bool isOn)
    {
        //anisotropic filtering
        QualitySettings.anisotropicFiltering = isOn ? AnisotropicFiltering.Enable : AnisotropicFiltering.Disable;
    }

    private void AdjustVsync(bool isOn)
    {
        //vsync
        if (isOn)
        {
            QualitySettings.vSyncCount = 1;
        }
        else
        {
            QualitySettings.vSyncCount = 0;
        }
    }

    private void AdjustFieldOfView(float value)
    {
        //field of view
        cam.fieldOfView = value;
        fovValueText.text = cam.fieldOfView.ToString();
    }

    private void AdjustClippingPlane(float value)
    {
        //update clipping plane
        cam.farClipPlane = value;
        clippingValueText.text = cam.farClipPlane.ToString();
    }
}