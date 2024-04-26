using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private AudioSource audioSource;
    private Animator animator;

    [Header("Movement")]
    public float currentSpeed;
    [SerializeField] private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    
    public float groundDrag;
    public float slideDrag;
    public float airDrag;

    Vector3 prevPos;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    [SerializeField] bool readyToJump;

    [Header("Crouching")]
    public float crouchSpeed;
    public float slideSpeed;
    public float crouchYScale;
    public float slideYScale;
    private float startYScale;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    [SerializeField] bool grounded;

    [Header("Laser Slow")]
    [SerializeField] public float slowDuration;
    [SerializeField] public float slowSpeed;
    bool isSlowed;

    [Header("Cheats")]
    static public bool doubleSpeed = false;
    static public bool isDoubleSpeedActive = false;
    static public bool doubleJumpHeight = false;
    static public bool isDoubleJumpHeightActive = false;

    [Header("Other")]
    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking, 
        sprinting,
        crouching,
        sliding,
        air,
        lasered
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        rb.freezeRotation = true;
        readyToJump = true;
        moveSpeed = walkSpeed;
        startYScale = transform.localScale.y;
        isSlowed = false;

        StartCoroutine(SpeedCalculation());
    }

    private void Update()
    {
        //This checks if you are grounded
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);
        
        MyInput();
        SpeedControl();
        StateHandler();
        CheatHandler();

        // I moved the drag functionality to the state handler function - this section is only for debugging
        // This will create drag if you are grounded, and remove it if you are in the air
        if (grounded)
        {
            Debug.Log("The player is grounded");
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        animator.SetFloat("MoveX", 1f);
        animator.SetFloat("MoveY", 1f);

        //This code allows you to jump
        if (InputControls.getJump() && readyToJump && grounded && state != MovementState.lasered)
        {
            readyToJump = false;
            //audioSource.Play(); Since no audio source is connected, this causes code to stop in the middle of the function
            Jump();
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    // State Handler function determines the player's current movement state every frame
    private void StateHandler()
    {
        // If slowed by lasers, can't sprint or crouch
        if (isSlowed == false)
        {
            // Sliding State
            // Slide is active when crouchKey is pressed, player is on the ground, 
            // moveSpeed is set to sprintSpeed or slideSpeed, and the player is moving
            if (InputControls.getCrouch() && grounded && (moveSpeed == sprintSpeed || moveSpeed == slideSpeed) && currentSpeed != 0) 
            {
                // If previous state was not sliding or crouched, adjust height
                if (state != MovementState.sliding)
                {
                    transform.localScale = new Vector3(transform.localScale.x, slideYScale, transform.localScale.z);
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                }
                moveSpeed = slideSpeed;
                state = MovementState.sliding;
                rb.AddForce(moveDirection * 5f, ForceMode.Impulse);
                rb.drag = slideDrag;
                animator.SetFloat("MoveX", 0.5f);
                animator.SetFloat("MoveY", 0.5f);
                
            }

            // Crouching State
            else if (InputControls.getCrouch() && grounded)
            {
                // If previous state was not sliding or crouched, adjust height
                if (state != MovementState.crouching)
                {
                    transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
                    rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
                }
                state = MovementState.crouching;
                moveSpeed = crouchSpeed;
                rb.drag = groundDrag;
                animator.SetFloat("MoveX", 1f);
                animator.SetFloat("MoveY", 0f);

            }

            // Sprinting State
            else if (InputControls.getSprint() && grounded)
            {
                // If previous state was sliding or crouched, adjust height
                if (state == MovementState.sliding || state == MovementState.crouching)
                {
                    transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                    
                }
                state = MovementState.sprinting;
                moveSpeed = sprintSpeed;
                rb.drag = groundDrag;
            }

            // Walking State
            else if (grounded)
            {
            
                // If previous state was sliding or crouched, adjust height
                if (state == MovementState.sliding || state == MovementState.crouching)
                {
                    transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                }
                state = MovementState.walking;
                moveSpeed = walkSpeed;
                rb.drag = groundDrag;

                // Idle State
                if (currentSpeed == 0)
                {
                    Debug.Log("The player is idle");
                    animator.SetFloat("MoveX", 0f);
                    animator.SetFloat("MoveY", 0f);
                }
            }

            // Air State
            else
            {
                // If previous state was sliding or crouched, adjust height
                if (state == MovementState.sliding || state == MovementState.crouching)
                {
                    transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
                }

                state = MovementState.air;
                animator.SetFloat("MoveX", 2f);
                animator.SetFloat("MoveY", 2f);
                // Will update moveSpeed to walkSpeed/sprintSpeed in air if necessary
                if (InputControls.getSprint())
                {
                    moveSpeed = sprintSpeed;
                } else {
                    moveSpeed = walkSpeed;
                }
                rb.drag = airDrag;
            }
        } else {
            // If slowed by laser
            // If previous state was sliding or crouched, adjust height
            if (state == MovementState.sliding || state == MovementState.crouching)
            {
                transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            }
            state = MovementState.lasered;
        }
    }

    //This function moves the player
    private void MovePlayer()
    {
        // If sliding, disables movement to lock player in slide. Movement is reenabled when slide is exited
        if (state == MovementState.sliding) 
        {
            verticalInput = 0;
            horizontalInput = 0;
        }

        //This code calculates the direction for movement
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;
    
        // If on the ground
        if (grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);
        }
        // If in the air
        else if (!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    //This function limits your max speed
    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);

        }
    }

    // This function calculates your current speed for testing purposes, it does not affect speed at all
    IEnumerator SpeedCalculation() 
    {
        while (true) {
            prevPos = transform.position;
            yield return new WaitForFixedUpdate();
            currentSpeed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);
        }
    }

    //This function makes you jump
    private void Jump()
    {
        //reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
        Debug.Log("The jump code is running");
    }

    //This function allows you to jump
    private void ResetJump()
    {
        readyToJump = true;
    }

    //This function slows the player upon contacting an object with the laser tag
    private void OnTriggerEnter(Collider other)
    {
        if ( other.CompareTag("Laser"))
        {
            isSlowed = true;
            Debug.Log("The Player will be slowed");
            moveSpeed = slowSpeed;
            //audioSource.Play(); Since no audio source is connected, this causes code to stop in the middle of the function
            Invoke(nameof(NotSlowed), slowDuration);
        }
    }

    private void NotSlowed()
    {
        isSlowed = false;
        Debug.Log("The Player returns to normal speed");
    }

    private void CheatHandler() {
        // If turning on doubleSpeed
        if (doubleSpeed && !isDoubleSpeedActive) {
            walkSpeed *= 2;
            sprintSpeed *= 2;
            crouchSpeed *= 2;
            slideSpeed *= 2;
            slowSpeed *= 2;
            isDoubleSpeedActive = true;
        // If turning off doubleSpeed
        } else if (!doubleSpeed && isDoubleSpeedActive) {
            walkSpeed /= 2;
            sprintSpeed /= 2;
            crouchSpeed /= 2;
            slideSpeed /= 2;
            slowSpeed /= 2;
            isDoubleSpeedActive = false;
        }

        // If turning on doubleJumpHeight
        if (doubleJumpHeight && !isDoubleJumpHeightActive) {
            jumpForce *= 2;
            isDoubleJumpHeightActive = true;
        // If turning off doubleJumpHeight
        } else if (!doubleJumpHeight && isDoubleJumpHeightActive) {
            jumpForce /= 2;
            isDoubleJumpHeightActive = false;
        }
    }
}