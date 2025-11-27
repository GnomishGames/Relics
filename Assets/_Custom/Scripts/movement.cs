using PurrNet;
using UnityEngine;


public class movement : NetworkIdentity
{
    //references
    public CharacterController controller;
    public Camera playerCamera;

    [Header("Camera Follow")]
    public float cameraDistance = 4f;
    public float cameraHeight = 1.6f;
    public float cameraSmoothTime = 0.12f;
    public float lookAtHeight = 1.2f;
    private Vector3 cameraVelocity = Vector3.zero;
    public float cameraYawFollowSpeed = 8f;

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

    [Header("Movement")]
    public float moveSpeed = 5f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    private Vector3 velocity; // Handles both gravity and jump
    [SerializeField] private float jumpHeight;
    private bool turnLeft, turnRight, forward, rearward, stepLeft, stepRight, jump;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        jumpHeight = 2f;
        playerCamera = Camera.main;
        // Initialize orbit angles from current player rotation
        currentYaw = transform.eulerAngles.y;
        currentPitch = 15f;
    }

    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;
    }

    void Update()
    {
        GroundCheck(groundMask, groundCheck);
        keyPresses();
        MovementLogic(moveSpeed);
        ApplyGravityAndMove();
        // Camera is updated in LateUpdate to follow player after movement
    }

    // LateUpdate is preferred for camera follow so it happens after all character movement
    void LateUpdate()
    {
        if (playerCamera == null) return;

        // Handle orbit input (hold right mouse button to rotate)
        if (enableOrbit && Input.GetMouseButton(1))
        {
            currentYaw += Input.GetAxis("Mouse X") * orbitSensitivityX * Time.deltaTime;
            currentPitch -= Input.GetAxis("Mouse Y") * orbitSensitivityY * Time.deltaTime;
            currentPitch = Mathf.Clamp(currentPitch, minPitch, maxPitch);
        }
        else
        {
            // When not orbiting, smoothly follow the player's yaw so the camera stays behind the player
            float targetYaw = transform.eulerAngles.y;
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, cameraYawFollowSpeed * Time.deltaTime);
        }

        // Compute target (look) point at player's head height
        Vector3 target = transform.position + Vector3.up * lookAtHeight;

        // Compute rotation from orbit angles
        Quaternion orbitRotation = Quaternion.Euler(currentPitch, currentYaw, 0f);

        // Desired camera offset relative to target
        Vector3 offset = orbitRotation * new Vector3(0f, 0f, -cameraDistance);
        Vector3 desiredPosition = target + offset + Vector3.up * (cameraHeight - lookAtHeight);

        // Camera collision: raycast from target towards desired position
        if (enableCameraCollision)
        {
            Vector3 dir = (desiredPosition - target).normalized;
            float desiredDist = Vector3.Distance(target, desiredPosition);
            RaycastHit hit;
            if (Physics.Raycast(target, dir, out hit, desiredDist + 0.01f, cameraCollisionMask))
            {
                float hitDist = Mathf.Max(minCameraDistance, hit.distance - 0.2f);
                desiredPosition = target + dir * hitDist;
            }
        }

        // Smoothly move camera to desired position
        playerCamera.transform.position = Vector3.SmoothDamp(playerCamera.transform.position, desiredPosition, ref cameraVelocity, cameraSmoothTime);

        // Always look at the target point
        Quaternion desiredRotation = Quaternion.LookRotation(target - playerCamera.transform.position);
        playerCamera.transform.rotation = Quaternion.Slerp(playerCamera.transform.rotation, desiredRotation, Time.deltaTime * 10f);
    }
    

    private void keyPresses()
    {
        turnLeft = Input.GetKey(KeyCode.A);
        turnRight = Input.GetKey(KeyCode.D);
        forward = Input.GetKey(KeyCode.W);
        rearward = Input.GetKey(KeyCode.S);
        stepLeft = Input.GetKey(KeyCode.Q);
        stepRight = Input.GetKey(KeyCode.E);
        jump = Input.GetKey(KeyCode.Space);
    }
    
    private void MovementLogic(float moveSpeed)
    {
        Vector3 horizontalMove = Vector3.zero;
        if (forward)
        {
            horizontalMove += transform.forward * moveSpeed;
        }
        if (rearward)
        {
            horizontalMove -= transform.forward * moveSpeed;
        }
        if (stepLeft)
        {
            horizontalMove -= transform.right * moveSpeed;
        }
        if (stepRight)
        {
            horizontalMove += transform.right * moveSpeed;
        }

        //turning logic
        if (turnLeft)
        {
            transform.Rotate(Vector3.up * -1);
        }
        if (turnRight)
        {
            transform.Rotate(Vector3.up * 1);
        }

        //jump
        if (jump && isGrounded)
        {
            // v = sqrt(2 * jumpHeight * -gravity)
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        // Move horizontally (vertical handled in ApplyGravityAndMove)
        controller.Move(horizontalMove * Time.deltaTime);
        
    }

    
    private bool GroundCheck(LayerMask groundMask, Transform groundCheck)
    {
        float groundDistance = .4f;
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        return isGrounded;
    }

    private void ApplyGravityAndMove()
    {
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Small downward force to keep grounded
        }
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}