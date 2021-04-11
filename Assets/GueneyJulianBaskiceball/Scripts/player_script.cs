using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    // Variables and References
    [Header("Player Movement Settings")]
    public float mvmt_speed = 10f; // movement speed variable for editor with default value
    public float jump_height = 10f; // jump height of the player
    
    private Rigidbody rb; // initialize rigidbody 
    
    private bool ballIsJumpable = true; // boolean to only jump the ball is jumpable
    public bool horizontalMovement = false; // enable horizontal ball movement in the editor
    
    // Start
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // reference to rigidbody component
    }

    // Update
    void Update()
    {
        // Horizontal Movement - Left/Right with Arrow Keys
        if (horizontalMovement)
        {
            float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * mvmt_speed;
            transform.Translate(horizontal, 0, 0);
        }
        

        // Jump Movement - Up with Spacebar, just works when Ball is on the floor
        if (Input.GetButtonDown("Jump") && ballIsJumpable)
        {
            rb.AddForce(new Vector3(0, jump_height,0), ForceMode.Impulse);
            ballIsJumpable = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Set back the variable to allow another jump when the ball hits a platform
        if (collision.gameObject.tag == "Jumpable") 
        {
            ballIsJumpable = true;
        }
        
        // Adding Force to the enemy so that they are pushing the player away
        if (collision.gameObject.tag == "Enemy") 
        {
            rb.AddForce(20,20,20,ForceMode.Acceleration);
        }
        
        // Adding Force to the walls so that the player is not able to glitch through them
        if (collision.gameObject.tag == "WallLeft") 
        {
            rb.AddForce(300,0,0,ForceMode.Force);
        }
        if (collision.gameObject.tag == "WallRight") 
        {
            rb.AddForce(-300,0,0,ForceMode.Force);
        }
    }
}
