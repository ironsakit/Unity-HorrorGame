using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 5f;
    public Transform cameraTransform;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private CharacterController characterController;
    private float verticalLookRotation;
    private Vector3 velocity;
    private bool isGrounded;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        LookAround();
        Move();
        Jump();
        ApplyGravity();
    }

    void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;
        float moveForward = Input.GetAxis("Vertical") * currentSpeed;
        float moveSide = Input.GetAxis("Horizontal") * currentSpeed;

        Vector3 move = transform.right * moveSide + transform.forward * moveForward;
        characterController.Move(move * Time.deltaTime); // This is the Move method of CharacterController
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        // Rotate character around the Y axis (horizontal rotation)
        transform.Rotate(Vector3.up * mouseX);

        // Rotate camera around the X axis (vertical rotation)
        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -2f * Physics.gravity.y);
        }
    }

    void ApplyGravity()
    {
        isGrounded = Physics.CheckSphere(transform.position, groundCheckDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // A small negative value to ensure the character stays grounded
        }

        velocity.y += Physics.gravity.y * Time.deltaTime;
        characterController.Move(velocity * Time.deltaTime); // Apply gravity effect
    }
}
