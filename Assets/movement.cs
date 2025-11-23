using UnityEngine;


public class movement : MonoBehaviour
{
    //references
    public CharacterController controller;

//gravity
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private bool isGrounded;
    private Vector3 velocity; // Handles both gravity and jump
    [SerializeField] private float jumpHeight;
    private Vector3 moveDirection;
    private bool turnLeft, turnRight, forward, rearward, stepLeft, stepRight, jump;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        jumpHeight = 2f; // More typical jump height for CharacterController
    }

    void Update()
    {
        GroundCheck(groundMask, groundCheck);
        keyPresses();
        MovementLogic();
        ApplyGravityAndMove();
    }

    private void keyPresses()
    {
        turnLeft = Input.GetKey(KeyCode.Q);
        turnRight = Input.GetKey(KeyCode.E);
        forward = Input.GetKey(KeyCode.W);
        rearward = Input.GetKey(KeyCode.S);
        stepLeft = Input.GetKey(KeyCode.A);
        stepRight = Input.GetKey(KeyCode.D);
        jump = Input.GetKey(KeyCode.Space);
    }

    private void MovementLogic()
    {
        Vector3 horizontalMove = Vector3.zero;
        if (forward)
        {
            horizontalMove += transform.forward * 5f;
        }
        if (rearward)
        {
            horizontalMove -= transform.forward * 5f;
        }
        if (stepLeft)
        {
            horizontalMove -= transform.right * 5f;
        }
        if (stepRight)
        {
            horizontalMove += transform.right * 5f;
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


    void ForwardMotion(float maxSpeed)
    {
        moveDirection = new Vector3(gameObject.transform.forward.x, 0, gameObject.transform.forward.z);
        controller.Move(moveDirection * maxSpeed * Time.deltaTime);
    }
    
    void StrafeMotion(float maxSpeed)
    {
        moveDirection = new Vector3(gameObject.transform.right.x, 0, gameObject.transform.right.z);
        controller.Move(moveDirection * maxSpeed * Time.deltaTime);
    }

    // Remove JumpMotion, now handled in MovementLogic and ApplyGravityAndMove

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
