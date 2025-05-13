using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float groundDrag;
    public float sprintSpeed;
    public float walkSpeed;
    public float slideSpeed;

    private float desiredMoveSpeed;
    private float lastDesiredMoveSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump =true;

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Key Binds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    bool grounded;

    [Header("Slope handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;

    public Transform orientation;
    float horizontalInput;
    float verticalInput;

    public bool isRunning;
    public bool isCrouching;
    public bool inLight;

    Vector3 moveDirection;
    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        sliding,
        crouching,
        air
    }

    public bool sliding;

    private void StateHandler()
    {
        if(sliding)
        {
            state = MovementState.sliding;
            desiredMoveSpeed = slideSpeed;
        }
        else if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            desiredMoveSpeed = crouchSpeed;
            isCrouching = true;
            isRunning = false;
        }
        else if(grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            desiredMoveSpeed = sprintSpeed;
            isCrouching = false;
            isRunning = true;
        }
        else if(grounded)
        {
            state = MovementState.walking;
            desiredMoveSpeed = walkSpeed;
            isCrouching = false;
            isRunning = false;
        }
        else
        {
            state = MovementState.air;
        }

        
        moveSpeed = desiredMoveSpeed;
     

        lastDesiredMoveSpeed = desiredMoveSpeed;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

     

        startYScale = transform.localScale.y;
    }

    private void Update()
    {
        grounded = Physics.Raycast(transform.position, Vector3.down,playerHeight *0.5f +0.2f, WhatIsGround);
        MyInput();
        SpeedControl();
        StateHandler();
        CheckIfInLight();

        if (grounded)
        {
            rb.drag = groundDrag;
            
        }
        else
        {
            rb.drag = 0;
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

        if(Input.GetKey(jumpKey) && readyToJump && grounded)
        {
            readyToJump = false;

            Jump();

            Invoke(nameof(ResetJump), jumpCooldown);

        }

        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down *5f,ForceMode.Impulse);
        }

        if(Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void MovePlayer()
    {
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection(moveDirection) * moveSpeed * 20f, ForceMode.Force);

            if(rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
            }
        }

        else if (grounded)
        {
           
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        }
        else if(!grounded)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    
    private void SpeedControl()
    {
        if (OnSlope()&&!exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        
    }

    private void ResetJump()
    {
        readyToJump = true;
        exitingSlope = false;
    }

    public bool OnSlope()
    {
        if(Physics.Raycast(transform.position, Vector3.down,out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle !=0;  
        }

        return false;
    }

    public Vector3 GetSlopeMoveDirection(Vector3 direction)
    {
        return Vector3.ProjectOnPlane(direction,slopeHit.normal).normalized;
    }

    public void CheckIfInLight()
    {
        inLight = false; // Assume not in light

        RaycastHit hit;

        // Iterate through all spotlights in the scene
        foreach (Light spotlight in FindObjectsOfType<Light>())
        {
            if (spotlight.type == LightType.Spot)
            {
                Vector3 lightPosition = spotlight.transform.position;
                Vector3 directionToPlayer = transform.position - lightPosition; // Vector from light to player

                // Calculate the angle between the light's forward vector and the direction to the player
                float angleToPlayer = Vector3.Angle(spotlight.transform.forward, directionToPlayer);

                // Debug the angle to player
                //Debug.Log("Angle to Player: " + angleToPlayer);

                // If the player is within the spotlight's angle cone
                if (angleToPlayer <= spotlight.spotAngle / 2f)
                {
                    // Check distance to ensure the player is within the spotlight's range
                    float distanceToPlayer = directionToPlayer.magnitude;
                    if (distanceToPlayer <= spotlight.range)
                    {
                        // Debug the distance to player
                        //Debug.Log("Player within light cone! Distance: " + distanceToPlayer);

                        // Perform the raycast to check if the player is in the light
                        if (Physics.Raycast(lightPosition, directionToPlayer.normalized, out hit, spotlight.range))
                        {
                            if (hit.collider.CompareTag("Player"))
                            {
                                inLight = true;
                                //Debug.Log("Player is in light!");
                                Debug.DrawRay(lightPosition, directionToPlayer.normalized * hit.distance, Color.yellow, 0.5f);
                                return; // Stop checking after detecting the player
                            }
                        }
                    }
                }
            }
        }
    }





    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        foreach (Light spotlight in FindObjectsOfType<Light>())
        {
            if (spotlight.type == LightType.Spot)
            {
                Vector3 lightPosition = spotlight.transform.position;
                float coneRadius = spotlight.range * Mathf.Tan(spotlight.spotAngle * 0.5f * Mathf.Deg2Rad);

                // Draw cone (using a sphere for simplicity)
                Gizmos.DrawWireSphere(lightPosition + spotlight.transform.forward * spotlight.range, coneRadius);
            }
        }
    }




}


