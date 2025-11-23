using UnityEngine;


public class movement : MonoBehaviour
{
    //references
    public CharacterController controller;

//gravity
    public float gravity = -9.81f;
    public LayerMask groundMask;
    public Transform groundCheck;
    public Vector3 downwardVelocity;
    Vector3 jumpVelocity;
    Vector3 moveDirection;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ApplyGravity(groundCheck, groundMask, controller);
        MovementLogic();
    }

    void MovementLogic()
    {
        //input
        bool forward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool left = Input.GetKey(KeyCode.A);
        bool right = Input.GetKey(KeyCode.D);
        bool walk = Input.GetKey(KeyCode.LeftShift);
        bool jump = Input.GetKey(KeyCode.Space);
        bool sit = Input.GetKeyDown(KeyCode.C);
        bool mouseLock = Input.GetKeyDown(KeyCode.L);

        if (forward && !left && !right && !backward)
            {
                moveDirection.z = 2f;
            }
            
        controller.Move(moveDirection);
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
