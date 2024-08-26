using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CharacterControllerScript : MonoBehaviour
{
    // Public variables for customization
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float jumpForce = 5f;

    // References to components
    private Animator animator;
    private Rigidbody rb;

    // Ground check variables
    public Transform groundCheck;
    public float groundDistance = 0.2f;
    public LayerMask groundMask;

    private bool isGrounded;
    private float speed;

    void Start()
    {
        // Get references to components
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        // Get input
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        // Determine speed
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
        }
        else
        {
            speed = walkSpeed;
        }

        // Move character
        Vector3 move = transform.right * moveX + transform.forward * moveZ;
        rb.MovePosition(transform.position + move * speed * Time.deltaTime);

        // Handle jumping
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        // Update Animator parameters
        animator.SetFloat("Speed", move.magnitude * speed);
        animator.SetBool("IsJumping", !isGrounded);
    }
}

