using UnityEngine;


public class movement : MonoBehaviour
{
    //references
    public CharacterController controller;

//gravity
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private Vector3 downwardVelocity;
    [SerializeField] private Vector3 jumpVelocity;
    private Vector3 moveDirection;
    private bool turnLeft, turnRight, forward, rearward, stepLeft, stepRight;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        ApplyGravity(groundCheck, groundMask, controller);
        keyPresses();
        freeMovementLogic();
    }

    private void keyPresses()
    {
        turnLeft = Input.GetKey(KeyCode.Q);
        turnRight = Input.GetKey(KeyCode.E);
        forward = Input.GetKey(KeyCode.W);
        rearward = Input.GetKey(KeyCode.S);
        stepLeft = Input.GetKey(KeyCode.A);
        stepRight = Input.GetKey(KeyCode.D);
    }

    private void freeMovementLogic()
    {
        if (forward)
        {
            ForwardMotion(5);
        }

        if (rearward)
        {
            ForwardMotion(-5);
        }

        if (stepLeft)
        {
            StrafeMotion(-5);
        }

        if (stepRight)
        {
            StrafeMotion(5);
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

    private void ApplyGravity(Transform groundCheck, LayerMask groundMask, CharacterController controller)
    {
        if (groundCheck != null)
        {
            float gravitysAcceleration = -9.81f;
            float groundDistance = .4f;
            bool isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

            if (isGrounded)
            {
                downwardVelocity.y = -2f;
            }
            else
            {
                downwardVelocity.y += gravitysAcceleration * Time.deltaTime;
            }

            controller.Move(downwardVelocity * Time.deltaTime);
        }
    }
}
