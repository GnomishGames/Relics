using PurrNet;
using TMPro;
using UnityEngine;


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
    private bool turnLeft, turnRight, forward, rearward, stepLeft, stepRight, jump;

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
    }

    void Update()
    {
        GroundCheck(groundMask, groundCheck);

        MovementLogic(runSpeed);
        ApplyGravityAndMove();

        if (!chatBox.isFocused)
            keyPresses();
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
        // Calculate movement direction
        Vector3 horizontalMove = Vector3.zero;
        float velocityX = 0f;
        float velocityY = 0f;

        // Forward/backward movement
        if (forward)
        {
            horizontalMove += transform.forward * moveSpeed;
            velocityY = moveSpeed;  // Actual speed value for blend tree thresholds
        }
        else if (rearward)
        {
            horizontalMove -= transform.forward * moveSpeed;
            velocityY = -moveSpeed;  // Negative for backward
        }

        // Strafe movement
        if (stepRight  && !rearward)
        {
            horizontalMove += transform.right * moveSpeed;
            velocityX = moveSpeed;  // Actual speed value for blend tree thresholds
        }
        else if (stepLeft && !rearward)
        {
            horizontalMove -= transform.right * moveSpeed;
            velocityX = -moveSpeed;  // Negative for left
        }

        // Update animator with actual velocity values (for blend tree thresholds)
        if (isGrounded)
        {
            animator.SetFloat("VelocityX", velocityX);
            animator.SetFloat("VelocityY", velocityY);
        }

        // Turning
        if (turnLeft)
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }
        else if (turnRight)
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }

        // Jump
        if (jump && isGrounded)
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
            characterFocus.currentFocus = null;

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
            characterStats.currentHitPoints = characterStats.maxHitpoints;
            characterStats.dead = false;
            animator.SetBool("Dead", false);
            positionReset = false;

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