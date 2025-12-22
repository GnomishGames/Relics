using PurrNet;
using UnityEngine;


public class PlayerMovement : NetworkIdentity
{
    //references
    public CharacterController controller;
    public Animator animator;

    [Header("Movement")]
    public float moveSpeed = 5f;
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

    void Update()
    {
        GroundCheck(groundMask, groundCheck);
        keyPresses();
        MovementLogic(moveSpeed);
        ApplyGravityAndMove();
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
            //animate walk forward
            if (isGrounded)
            {
                animator.SetBool("Forward", true);
            }
        }else
        {
            //stop walk forward animation
            animator.SetBool("Forward", false);
        }

        if (rearward)
        {
            horizontalMove -= transform.forward * moveSpeed;
            //animate walk backward
            if (isGrounded)
            {
                animator.SetBool("Back", true);
            }
        }else
        {
            //stop walk backward animation
            animator.SetBool("Back", false);
        }
    

        if (stepLeft)
        {
            horizontalMove -= transform.right * moveSpeed;
            if (isGrounded)
            {
                animator.SetBool("StrafeLeft", true);
            }
        }else
        {
            animator.SetBool("StrafeLeft", false);
        }

        if (stepRight)
        {
            horizontalMove += transform.right * moveSpeed;
            if (isGrounded)
            {
                animator.SetBool("StrafeRight", true);
            }
        }else
        {
            animator.SetBool("StrafeRight", false);
        }

        //turning logic: use configurable turnSpeed (degrees/second)
        if (turnLeft)
        {
            transform.Rotate(0f, -turnSpeed * Time.deltaTime, 0f);
        }
        if (turnRight)
        {
            transform.Rotate(0f, turnSpeed * Time.deltaTime, 0f);
        }

        //jump
        if (jump && isGrounded)
        {
            // v = sqrt(2 * jumpHeight * -gravity)
            velocity.y = Mathf.Sqrt(2f * jumpHeight * -gravity);
        }

        //falling
        if (!isGrounded)
        {
            // Optionally, you can add mid-air control here if desired
            animator.SetBool("InAir", true);
        }else
        {
            animator.SetBool("InAir", false);
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