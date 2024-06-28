using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float normalSpeed = 5f;
    public float sprintSpeed = 10f;
    public float mouseSensitivity = 2f;
    public float jumpForce = 1f;
    public Transform cameraTransform;
    public LayerMask groundLayer;
    public float groundCheckDistance = 0.1f;

    private CharacterController characterController;
    private float verticalLookRotation;
    private Vector3 velocity;
    private bool isGrounded;

    // Zoom variables
    public float defaultFOV = 60f;
    public float zoomedFOV = 30f;
    public float zoomSpeed = 10f;
    private Camera playerCamera;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Get the Camera component from the cameraTransform
        playerCamera = cameraTransform.GetComponent<Camera>();

        // Set the default FOV
        if (playerCamera != null)
        {
            playerCamera.fieldOfView = defaultFOV;
        }
    }

    void Update()
    {
        LookAround();
        Move();
        Jump();
        ApplyGravity();
        HandleZoom();
    }

    void Move()
    {
        float currentSpeed = Input.GetKey(KeyCode.LeftShift) ? sprintSpeed : normalSpeed;
        float moveForward = Input.GetAxis("Vertical") * currentSpeed;
        float moveSide = Input.GetAxis("Horizontal") * currentSpeed;

        Vector3 move = transform.right * moveSide + transform.forward * moveForward;
        characterController.Move(move * Time.deltaTime);
    }

    void LookAround()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(Vector3.up * mouseX);

        verticalLookRotation -= mouseY;
        verticalLookRotation = Mathf.Clamp(verticalLookRotation, -90f, 90f);

        cameraTransform.localRotation = Quaternion.Euler(verticalLookRotation, 0f, 0f);
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpForce * -0.5f * Physics.gravity.y);
            characterController.Move((velocity * Time.deltaTime));
        }
    }

    void ApplyGravity()
    {
        isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, groundLayer);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -5f; // Ensure the character stays grounded
        }

        velocity.y += Physics.gravity.y * Time.deltaTime * 2;
        characterController.Move(velocity * Time.deltaTime);
    }

    void HandleZoom()
    {
        if (Input.GetMouseButton(1)) // Right mouse button
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, zoomedFOV, Time.deltaTime * zoomSpeed);
        }
        else
        {
            playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, defaultFOV, Time.deltaTime * zoomSpeed);
        }
    }
}
