using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;


public class movement : MonoBehaviour
{
    //references
    public CharacterController controller;

//gravity
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float run = 5f;
    [SerializeField] private float jump = 5f;

    private Vector2 moveInput;
    public float moveSpeed = 5f;

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
        Vector3 movement = new Vector3(moveInput.x, 0f, moveInput.y) * moveSpeed * Time.deltaTime;
        transform.Translate(movement);
    }

    void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
    }
}
