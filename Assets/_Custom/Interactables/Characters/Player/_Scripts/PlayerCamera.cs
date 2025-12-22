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
    public float cameraYawFollowSpeed = 8f;

    [Tooltip("Smooth target position to reduce jitter from character movement")]
    public bool interpolateTargetPosition = true;
    public float targetPositionSmoothTime = 0.05f;
    private Vector3 smoothedTargetPosition;
    private Vector3 targetVelocity = Vector3.zero;

    [Tooltip("Enable camera collision (prevents clipping through geometry)")]
    public bool enableCameraCollision = true;
    public LayerMask cameraCollisionMask = ~0;
    public float minCameraDistance = 0.5f;

    [Header("Orbit Controls")]
    public bool enableOrbit = true;
    public float orbitSensitivityX = 120f;
    public float orbitSensitivityY = 120f;
    public float minPitch = -10f;
    public float maxPitch = 75f;
    private float currentYaw = 0f;
    private float currentPitch = 15f;

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
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        if (!isOwner) 
            return;
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

        // Handle orbit input (hold right mouse button to rotate)
        if (enableOrbit && Input.GetMouseButton(1))
        {
            currentYaw += Input.GetAxis("Mouse X") * orbitSensitivityX * Time.deltaTime;
            currentPitch -= Input.GetAxis("Mouse Y") * orbitSensitivityY * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }
        else
        {
            // Smoothly follow the player's yaw so the camera stays behind the player
            float targetYaw = target.eulerAngles.y;
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, cameraYawFollowSpeed * Time.deltaTime);
        }

        // Compute look target point at player's head height
        Vector3 lookTarget = smoothedTargetPosition + Vector3.up * lookAtHeight;

        // Compute rotation from orbit angles
        Quaternion orbitRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Desired camera offset relative to lookTarget
        Vector3 offset = orbitRotation * new Vector3(0f, 0f, -cameraDistance);
        Vector3 desiredPosition = lookTarget + offset + Vector3.up * (cameraHeight - lookAtHeight);

        // Camera collision: raycast from lookTarget towards desired position
        if (enableCameraCollision)
        {
            Vector3 dir = (desiredPosition - lookTarget).normalized;
            float desiredDist = Vector3.Distance(lookTarget, desiredPosition);
            if (Physics.Raycast(lookTarget, dir, out RaycastHit hit, desiredDist + 0.01f, cameraCollisionMask))
            {
                float hitDist = Mathf.Max(minCameraDistance, hit.distance - 0.2f);
                desiredPosition = lookTarget + dir * hitDist;
            }
        }

        // Smoothly move camera to desired position
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, desiredPosition, ref cameraVelocity, cameraSmoothTime);

        // Directly set rotation to look at target (no additional smoothing to prevent jitter)
        cam.transform.rotation = Quaternion.LookRotation(lookTarget - cam.transform.position);
    }
}
