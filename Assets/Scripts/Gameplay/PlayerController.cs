using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float speed = 4.0f;
    [SerializeField] float jumpForce = 7.5f;
    [SerializeField] float rollForce = 6.0f;
    [SerializeField] bool noBlood = false;


    private PlayerSensor groundSensor;
    private PlayerSensor wallSensorR1;
    private PlayerSensor wallSensorR2;
    private PlayerSensor wallSensorL1;
    private PlayerSensor wallSensorL2;
    private Animator animator;
    private Rigidbody2D rb;

    private bool grounded = false;
    private int facingDirection = 1;
    private float currentAttack;
    private float timeSinceAttack = 0.0f;
    private float delayToIdle = 0.0f;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        groundSensor = transform.Find("GroundSensor").GetComponent<PlayerSensor>();
        wallSensorR1 = transform.Find("WallSensor_R1").GetComponent<PlayerSensor>();
        wallSensorL1 = transform.Find("WallSensor_L1").GetComponent<PlayerSensor>();
        wallSensorL2 = transform.Find("WallSensor_L2").GetComponent<PlayerSensor>();
    }

    // Update is called once per frame
    void Update()
    {
        // Increase timer that controls attack combo
        timeSinceAttack += Time.deltaTime;

        // Check if character just landed on the ground
        if (!grounded && groundSensor.State())
        {
            grounded = true;
            animator.SetBool("Grounded", grounded);
        }

        // Check if character just started failling
        if (grounded && !groundSensor.State())
        {
            grounded = false;
            animator.SetBool("Grounded", grounded);
        }

        // handle input and movement
        float inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
        {
            GetComponent<SpriteRenderer>().flipX = false;
            facingDirection = 1;
        }
        else if (inputX < 0)
        {
            GetComponent<SpriteRenderer>().flipX = true;
            facingDirection = -1;
        }

        // Move
        rb.velocity = new Vector2(inputX * speed, rb.velocity.y);

        // Set AirSpeed in animator
        animator.SetFloat("AirSpeedY", rb.velocity.y);

        // Attack
        if (Input.GetMouseButtonDown(0) && timeSinceAttack > 0.25f)
        {
            currentAttack++;

            // Loop back to one after third attack
            if (currentAttack > 3)
            {
                currentAttack = 1;
            }

            // Reset attack combo if time since last attack is too large
            if (timeSinceAttack > 1.0f)
            {
                currentAttack = 1;
            }

            // Call on of three animations "Attack1", "Attack2", "Attack3"
            animator.SetTrigger("Attack" + currentAttack);

            // Reset timer
            timeSinceAttack = 0.0f;
        }
        // Block
        else if (Input.GetMouseButtonDown(1))
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IdleBlock", false);

        }
        // Jump
        else if (Input.GetKeyDown("space") && grounded)
        {
            animator.SetTrigger("Jump");
            grounded = false;
            animator.SetBool("Grounded", grounded);
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            groundSensor.Disable(0.2f);
        }

        // Run
        else if (Mathf.Abs(inputX) > Mathf.Epsilon)
        {
            // Reset timer
            delayToIdle = 0.05f;
            animator.SetInteger("AnimState", 1);
        }
        // Idle
        else
        {
            // prevents flickering transitions to idle
            delayToIdle -= Time.deltaTime;
            if (delayToIdle < 0)
            {
                animator.SetInteger("AnimState", 0);
            }

        }

    }
}
