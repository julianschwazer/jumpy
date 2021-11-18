using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    
    // Variables and References
    private Rigidbody rb; // initialize rigidbody 
    private bool birdIsJumpable = true; // boolean to enable/disable the jumping
    private bool birdIsFlyable = false; // boolean to enable/disable the flying
    private bool birdIsFlapable = false; // boolean if the bird kan flap again

    // PUBLIC Variables for the editor
    [Header("Player Jumping Settings")]
    public float jump_speed = 10f; // movement speed variable for editor with default value
    public float jump_height = 10f; // jump height of the player

    [Header("Player Flying Settings")] 
    public float fly_horizontalspeed = 10f; // horizontal movement speed while flying
    public float fly_height = 2f; // amount of force for flapping
    public float fly_flaptime = 2f; // time the flap is blocked after flapping
    
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // reference to rigidbody component
    }

    // Update
    void Update()
    {
        // Vertical Movement - Jumping with all "Up-Keys", SpaceBar, and MouseButton
        if (birdIsJumpable)
        {
            if (Input.GetButtonDown("Jump") && birdIsJumpable
                || Input.GetMouseButtonDown(0) && birdIsJumpable
                || Input.GetKeyDown(KeyCode.UpArrow) && birdIsJumpable
                || Input.GetKeyDown(KeyCode.W) && birdIsJumpable)
            {
                rb.AddForce(new Vector3(0, jump_height,0), ForceMode.Impulse);
                birdIsJumpable = false; // limit infinite jumping
            }
        }

        // Vertical Movement – Flying/flapping the wings with all "Up-Keys", SpaceBar, and MouseButton
        if (birdIsFlyable)
        {
            // Horizontal Movement - left and right
            float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * fly_horizontalspeed;
            rb.AddForce(new Vector3(horizontal, 0,0), ForceMode.Impulse);

            // Vertical Movement - flying through flapping
            if (Input.GetButtonDown("Jump") && birdIsFlapable
                || Input.GetMouseButtonDown(0) && birdIsFlapable
                || Input.GetKeyDown(KeyCode.UpArrow) && birdIsFlapable
                || Input.GetKeyDown(KeyCode.W) && birdIsFlapable)
            {
                rb.AddForce(new Vector3(0, fly_height,0), ForceMode.Impulse);
                StartCoroutine(DisableFlapping());
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Set back the variable to allow another jump when the ball hits a platform
        if (collision.gameObject.CompareTag("Jumpable")) 
        {
            if (birdIsFlyable == false)
            {
                birdIsJumpable = true;
            }
        }

        // Adding Force to the enemy so that they are pushing the player away
        if (collision.gameObject.CompareTag("Enemy")) 
        {
            rb.AddForce(200,200,200,ForceMode.Force);
        }

        // Adding Force to the walls so that the player is not able to glitch through them
        if (collision.gameObject.CompareTag("WallLeft")) 
        {
            rb.AddForce(200,0,0,ForceMode.Force);
        }
        if (collision.gameObject.CompareTag("WallRight")) 
        {
            rb.AddForce(-200,0,0,ForceMode.Force);
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        // Enable/Disable the flying capabilities when jumping through the right object
        if (trigger.gameObject.CompareTag("FlyingAreaEnter")) 
        {
            birdIsJumpable = false;
            birdIsFlyable = true;
            birdIsFlapable = true;
            Debug.Log("I am flying");
        }
        if (trigger.gameObject.CompareTag("FlyingAreaLeave"))
        {
            birdIsJumpable = true;
            birdIsFlyable = false;
            Debug.Log("I am leaving the fly area");
        }
    }
    
    // Coroutine for limiting the amount of consecutive flaps
    private IEnumerator DisableFlapping() {
        birdIsFlapable = false;
 
        yield return new WaitForSeconds(fly_flaptime);

        birdIsFlapable = true;
    }
    
}
