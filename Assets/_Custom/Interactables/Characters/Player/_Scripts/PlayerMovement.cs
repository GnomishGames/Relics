using PurrNet;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

//all of the required components for a character
//All NPC reuired Components
//movement and pathfinding
[RequireComponent(typeof(CharacterController))]

//Network
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(NetworkAnimator))]

[RequireComponent(typeof(PlayerMovement))]

[RequireComponent(typeof(PlayerCamera))]

[RequireComponent(typeof(CharacterStats))]
[RequireComponent(typeof(PlayerTimers))]
[RequireComponent(typeof(CharacterFocus))]
[RequireComponent(typeof(FieldOfView))]

//inventory
[RequireComponent(typeof(Inventory))]
[RequireComponent(typeof(Equipment))]
[RequireComponent(typeof(SkillBar))]

//options
[RequireComponent(typeof(GraphicsOptions))]
[RequireComponent(typeof(KeyBindings))]

//combat
[RequireComponent(typeof(SkillBook))]
[RequireComponent(typeof(SkillBar))]


public class PlayerMovement : NetworkIdentity
{
    //references
    CharacterController controller;
    Animator animator;
    CharacterStats characterStats;
    CharacterFocus characterFocus;

    [Header("Movement")]
    public float runSpeed = 5f;
    public float walkSpeed = 2f;
    [Header("Rotation")]
    [Tooltip("Degrees per second the player rotates when pressing turn keys")]
    public float turnSpeed = 180f;

    [Header("Gravity")]
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    public Vector3 velocity; // Handles both gravity and jump
    [SerializeField] private float jumpHeight;
    
    // New Input System
    private InputAction moveAction;
    private InputAction turnAction;
    private InputAction jumpAction;
    private Vector2 moveInput;
    private float turnInput;

    public float despawnTimer;
    public float respawnTimer;
    public bool despawned;
    Vector3 spawnPosition;
    Quaternion spawnRotation;
    bool positionReset;
    public TMP_InputField chatBox;


    protected override void OnSpawned()
    {
        base.OnSpawned();

        enabled = isOwner;

        controller = GetComponent<CharacterController>();
        jumpHeight = 2f;
    }

    protected override void OnDespawned()
    {
        base.OnDespawned();

        if (!isOwner)
            return;
    }

    void Start()
    {
        characterStats = GetComponent<CharacterStats>();
        characterFocus = GetComponent<CharacterFocus>();
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        //spawning and despawning
        despawned = false;
        spawnPosition = transform.position;
        spawnRotation = transform.rotation;
        respawnTimer = characterStats.behaviorSO.respawnTimer;
        despawnTimer = characterStats.behaviorSO.despawnTimer;

        // Set up new Input System actions
        moveAction = InputSystem.actions.FindAction("Move");
        turnAction = InputSystem.actions.FindAction("Turn");
        jumpAction = InputSystem.actions.FindAction("Jump");
        
        // Enable actions
        if (moveAction != null) moveAction.Enable();
        if (turnAction != null) turnAction.Enable();
        if (jumpAction != null) jumpAction.Enable();
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        
        // Disable actions when destroyed
        if (moveAction != null) moveAction.Disable();
        if (turnAction != null) turnAction.Disable();
        if (jumpAction != null) jumpAction.Disable();
    }

    void Update()
    {
        GroundCheck(groundMask, groundCheck);

        // Read input from new Input System
        if (!chatBox.isFocused && !characterStats.dead)
        {
            ReadInput();
        }
        else
        {
            // Clear input when chat is focused or dead
            moveInput = Vector2.zero;
            turnInput = 0f;
        }

        MovementLogic(runSpeed);
        ApplyGravityAndMove();
    }

    private void ReadInput()
    {
        // Read move input (WASD/QE for strafe)
        if (moveAction != null)
        {
            moveInput = moveAction.ReadValue<Vector2>();
        }
        
        // Read turn input (A/D for turning)
        if (turnAction != null)
        {
            turnInput = turnAction.ReadValue<float>();
        }
    }

    private void MovementLogic(float moveSpeed)
    {
        // Calculate movement direction
        Vector3 horizontalMove = Vector3.zero;
        float velocityX = 0f;
        float velocityY = 0f;

        // Forward/backward movement (moveInput.y)
        if (moveInput.y > 0.1f) // Forward
        {
            horizontalMove += transform.forward * moveSpeed;
            velocityY = moveSpeed;
        }
        else if (moveInput.y < -0.1f) // Backward
        {
            horizontalMove -= transform.forward * moveSpeed;
            velocityY = -moveSpeed;
        }

        // Strafe movement (moveInput.x for Q/E strafe)
        if (moveInput.x > 0.1f && moveInput.y > -0.1f) // Right strafe (not backward)
        {
            horizontalMove += transform.right * moveSpeed;
            velocityX = moveSpeed;
        }
        else if (moveInput.x < -0.1f && moveInput.y > -0.1f) // Left strafe (not backward)
        {
            horizontalMove -= transform.right * moveSpeed;
            velocityX = -moveSpeed;
        }

        // Update animator with actual velocity values (for blend tree thresholds)
        if (isGrounded)
        {
            animator.SetFloat("VelocityX", velocityX);
            animator.SetFloat("VelocityY", velocityY);
        }

        // Turning (A/D keys)
        if (turnInput < -0.1f) // Turn left
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }
        else if (turnInput > 0.1f) // Turn right
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }

        // Jump
        if (jumpAction != null && jumpAction.WasPressedThisFrame() && isGrounded)
        {
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        // In air state
        animator.SetBool("InAir", !isGrounded);

        // Move horizontally
        controller.Move(horizontalMove * Time.deltaTime);
    }

    public void DespawnCharacter()
    {
        if (characterStats.dead && !despawned)
        {
            despawnTimer -= Time.deltaTime;
        }

        if (despawnTimer <= 0)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            despawned = true;
            characterFocus.target = null;

            despawnTimer = characterStats.behaviorSO.despawnTimer;
        }
    }

    public void RespawnCharacter()
    {
        if (despawned && characterStats.dead)
        {
            respawnTimer -= Time.deltaTime;
        }

        if (respawnTimer <= 0)
        {
            despawned = false;
            transform.GetChild(0).gameObject.SetActive(true);
            positionReset = false;
            characterStats.Revive();

            respawnTimer = characterStats.behaviorSO.respawnTimer;
        }
    }

    public void ResetPosition()
    {
        if (despawned && !positionReset)
        {
            gameObject.transform.SetPositionAndRotation(spawnPosition, spawnRotation);
            positionReset = true;
        }
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