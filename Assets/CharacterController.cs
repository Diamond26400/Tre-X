using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterControllerScript : MonoBehaviour
{
    // Public variables for customization
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;
    public float rotationSpeed = 10f;

    // References to components
    private Animator animator;
    private Rigidbody rb;
    public new Camera camera;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private Vector3 movement;
    private bool isGrounded;
    private float speed;
    private bool isMoving;

    void Start()
    {

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Get input and calculate movement direction
        movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0f, Input.GetAxisRaw("Vertical")).normalized;
        isMoving = movement.magnitude > 0f;

        // Determine speed
        speed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Move character
        rb.MovePosition(transform.position + movement * speed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Update Animator parameters
        animator.SetFloat("Speed", movement.magnitude * speed);
        animator.SetBool("IsJumping", !isGrounded);

        // Rotate player and camera smoothly
        if (isMoving)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movement, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            this.camera.transform.rotation = Quaternion.Slerp(camera.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

}
