using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FPSRigidbodyController : MonoBehaviour
{
    public Camera playerCamera;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float jumpPower = 7f;
    public float gravity = 10f;

    public float lookSpeed = 2f;
    public float lookXLimit = 45f;

    private Vector3 moveDirection = Vector3.zero;
    private float rotationX = 0;

    public bool canMove = true;

    private Rigidbody rb;

    private bool isGrounded;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; 

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMovement();
        HandleMouseLook();
    }

    private void HandleMovement()
    {
        if (canMove)
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 right = transform.TransformDirection(Vector3.right);

            //run
            bool isRunning = Input.GetKey(KeyCode.LeftShift);

            float curSpeedX = isRunning ? runSpeed * Input.GetAxis("Vertical") : walkSpeed * Input.GetAxis("Vertical");
            float curSpeedY = isRunning ? runSpeed * Input.GetAxis("Horizontal") : walkSpeed * Input.GetAxis("Horizontal");

            
            moveDirection = (forward * curSpeedX) + (right * curSpeedY);

            //movement
            rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z); // Preserve Y velocity

            // Jumping 
            if (Input.GetButton("Jump") && isGrounded)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpPower, ForceMode.Impulse);
        }
    }

    private void HandleMouseLook()
    {
        if (canMove)
        {
            // Mouse look for the camera (up/down look)
            rotationX -= Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Rotate 
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }

    void FixedUpdate()
    {
        // Check if grounded
        isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f); 
    }
}
