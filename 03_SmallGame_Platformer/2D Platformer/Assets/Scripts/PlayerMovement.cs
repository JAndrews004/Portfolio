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

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask WhatIsGround;
    public bool grounded;
    public Transform groundCheck;

    [Header("Jumping")]
    public float jumpHeight;
    private int jumpsLeft = 2;

    Rigidbody2D rb;
    float horizontalInput;
    float verticalInput;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.freezeRotation = true;
        moveSpeed = walkSpeed;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, 0.1f, WhatIsGround);
        GetInput();
    }

    private void FixedUpdate()
    {
        MovePlayer();
        if (grounded)
        {
            jumpsLeft = 2;
        }
    }
    private void GetInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");

        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            moveSpeed = sprintSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            moveSpeed = walkSpeed;
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (grounded || jumpsLeft > 0)
            {
                if (!grounded)
                {
                    jumpsLeft = 0;
                }
                else
                {
                    jumpsLeft -= 1;
                }
                Jump(jumpsLeft);


            }
        }
    }

    private void MovePlayer()
    {
        
        if (horizontalInput < 0)
        {
            rb.AddForce(Vector2.left*moveSpeed, ForceMode2D.Force);

            if(rb.velocity.x < 0)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
            
            
        }
        if (horizontalInput > 0)
        {
            rb.AddForce(Vector2.right * moveSpeed, ForceMode2D.Force);
            
            if (rb.velocity.x > 0)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }

        else
        {
            Vector2 velocity = rb.velocity;
            velocity.x = Mathf.Lerp(velocity.x, 0, groundDrag * Time.fixedDeltaTime);
            rb.velocity = velocity;
        }
        
        


    }

    private void Jump(int jump)
    {
        if(jump == 0)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        else
        {
            rb.AddForce(Vector2.up * jumpHeight, ForceMode2D.Impulse);
        }
        
    }
}
