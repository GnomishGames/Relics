using PurrNet;
using UnityEngine;

// Handles third-person camera follow, orbit and simple anti-clip
public class PlayerCamera : NetworkIdentity
{
    [Tooltip("Player transform to follow. If empty the script will try to find a movement component in the scene.")]
    public Transform target;

    public Camera cam;

    [Header("Camera Follow")]
    public float cameraDistance = 4f;
    public float cameraHeight = 1.6f;
    public float cameraSmoothTime = 0.12f;
    public float lookAtHeight = 1.2f;
    private Vector3 cameraVelocity = Vector3.zero;
    [Tooltip("Automatically rotate camera to follow player's facing direction")]
    private bool autoFollowPlayerRotation = true;
    public float cameraYawFollowSpeed = 8f;

    [Tooltip("Smooth target position to reduce jitter from character movement")]
    public bool interpolateTargetPosition = true;
    public float targetPositionSmoothTime = 0.05f;
    private Vector3 smoothedTargetPosition;
    private Vector3 targetVelocity = Vector3.zero;

    [Tooltip("Enable camera collision (prevents clipping through geometry)")]
    public bool enableCameraCollision = true;
    public LayerMask cameraCollisionMask = ~0;
    public float minCameraDistance = 1f;

    [Header("Orbit Controls")]
    public bool enableOrbit = true;
    public float orbitSensitivityX = 120f;
    public float orbitSensitivityY = 120f;
    public float minPitch = -10f;
    public float maxPitch = 75f;
    private float currentYaw = 0f;
    private float currentPitch = 15f;

    [Header("Zoom Controls")]
    public bool enableMouseWheelZoom = true;
    public float zoomSpeed = 2f;
    public float minZoomDistance = 1f;
    public float maxZoomDistance = 10f;

    [Header("Inventory Camera Mode")]
    public float inventoryCameraDistance = 2.5f;
    public float inventoryCameraHeight = 1.2f;
    public float inventoryCameraPitch = 10f;
    public float inventoryCameraTransitionSpeed = 5f;
    public float inventoryCameraSmoothTime = 0.05f;
    private bool isInventoryOpen = false;
    private float defaultCameraDistance;
    private float defaultCameraHeight;
    private float defaultCameraPitch;
    private float defaultCameraSmoothTime;
    private KeyBindings keyBindings;

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;

        if (!isOwner) {
            //Destroy(cam.gameObject);
            return;
        }

        if (cam == null)
            cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();

        if (target == null)
        {
            var mv = UnityEngine.Object.FindAnyObjectByType<PlayerMovement>();
            if (mv != null)
            {
                target = mv.transform;
            }
        }

        currentYaw = (target != null) ? target.eulerAngles.y : transform.eulerAngles.y;
        currentPitch = 15f;

        if (target != null)
        {
            smoothedTargetPosition = target.position;
        }

        // Store default camera settings
        defaultCameraDistance = cameraDistance;
        defaultCameraHeight = cameraHeight;
        defaultCameraPitch = currentPitch;
        defaultCameraSmoothTime = cameraSmoothTime;

        // Find and subscribe to KeyBindings
        keyBindings = UnityEngine.Object.FindAnyObjectByType<KeyBindings>();
        if (keyBindings != null)
        {
            keyBindings.OnCharacterPanelToggled += ToggleCharacterCamera;
        }
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        if (!isOwner) 
            return;

        // Unsubscribe from KeyBindings
        if (keyBindings != null)
        {
            keyBindings.OnCharacterPanelToggled -= ToggleCharacterCamera;
        }
    }

    void LateUpdate()
    {
        if (cam == null) return;
        if (target == null) return;

        // Smooth target position to reduce jitter from character movement
        Vector3 currentTargetPos = target.position;
        if (interpolateTargetPosition)
        {
            smoothedTargetPosition = Vector3.SmoothDamp(smoothedTargetPosition, currentTargetPos, ref targetVelocity, targetPositionSmoothTime);
        }
        else
        {
            smoothedTargetPosition = currentTargetPos;
        }

        // Handle mouse wheel zoom
        if (enableMouseWheelZoom)
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");
            if (scrollInput != 0f)
            {
                cameraDistance -= scrollInput * zoomSpeed;
                cameraDistance = Mathf.Clamp(cameraDistance, minZoomDistance, maxZoomDistance);
                
                // Update default distance if not in inventory mode
                if (!isInventoryOpen)
                {
                    defaultCameraDistance = cameraDistance;
                }
            }
        }

        // Handle orbit input (hold right mouse button to rotate)
        if (enableOrbit && Input.GetMouseButton(1))
        {
            currentYaw += Input.GetAxis("Mouse X") * orbitSensitivityX * Time.deltaTime;
            currentPitch -= Input.GetAxis("Mouse Y") * orbitSensitivityY * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }
        else if (!isInventoryOpen && autoFollowPlayerRotation)
        {
            // Smoothly follow the player's yaw so the camera stays behind the player (only when inventory is closed and auto-follow is enabled)
            float targetYaw = target.eulerAngles.y;
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, cameraYawFollowSpeed * Time.deltaTime);
        }

        // Smoothly transition camera settings when inventory opens/closes
        float targetDistance = isInventoryOpen ? inventoryCameraDistance : defaultCameraDistance;
        float targetHeight = isInventoryOpen ? inventoryCameraHeight : defaultCameraHeight;
        float targetPitch = isInventoryOpen ? inventoryCameraPitch : defaultCameraPitch;
        float targetSmoothTime = isInventoryOpen ? inventoryCameraSmoothTime : defaultCameraSmoothTime;

        cameraDistance = Mathf.Lerp(cameraDistance, targetDistance, inventoryCameraTransitionSpeed * Time.deltaTime);
        cameraHeight = Mathf.Lerp(cameraHeight, targetHeight, inventoryCameraTransitionSpeed * Time.deltaTime);
        // Use the target smooth time directly instead of lerping it to prevent velocity artifacts
        cameraSmoothTime = targetSmoothTime;
        
        if (isInventoryOpen)
        {
            currentPitch = Mathf.Lerp(currentPitch, targetPitch, inventoryCameraTransitionSpeed * Time.deltaTime);
            // Face the front of the player when inventory is open (add 180 degrees to show front instead of back)
            float frontYaw = target.eulerAngles.y + 180f;
            currentYaw = Mathf.LerpAngle(currentYaw, frontYaw, inventoryCameraTransitionSpeed * Time.deltaTime);
        }

        // Compute look target point at player's head height
        Vector3 lookTarget = smoothedTargetPosition + Vector3.up * lookAtHeight;

        // Compute rotation from orbit angles
        Quaternion orbitRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Desired camera offset relative to lookTarget
        Vector3 offset = orbitRotation * new Vector3(0f, 0f, -cameraDistance);
        Vector3 desiredPosition = lookTarget + offset + Vector3.up * (cameraHeight - lookAtHeight);

        // Handle collisions and minimum distance in one pass
        Vector3 dirFromLookTarget = (desiredPosition - lookTarget).normalized;
        float desiredDist = cameraDistance;
        
        if (enableCameraCollision)
        {
            // Raycast for world geometry (exclude the player to avoid animation jitter)
            RaycastHit[] hits = Physics.RaycastAll(lookTarget, dirFromLookTarget, desiredDist + 0.2f, cameraCollisionMask);
            foreach (RaycastHit hit in hits)
            {
                // Skip if we hit the player themselves
                if (hit.transform == target || hit.transform.IsChildOf(target))
                    continue;
                
                // Found valid obstacle
                desiredDist = Mathf.Max(minCameraDistance, hit.distance - 0.2f);
                break;
            }
        }
        
        // Enforce minimum distance from player (prevents clipping through character body)
        desiredDist = Mathf.Max(desiredDist, minCameraDistance);
        desiredPosition = lookTarget + dirFromLookTarget * desiredDist + Vector3.up * (cameraHeight - lookAtHeight);

        // Smoothly move camera to desired position
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref cameraVelocity, cameraSmoothTime);

        // Directly set rotation to look at target
        cam.transform.rotation = Quaternion.LookRotation(lookTarget - cam.transform.position);
    }

    private void ToggleCharacterCamera()
    {
        isInventoryOpen = !isInventoryOpen;
    }
}
