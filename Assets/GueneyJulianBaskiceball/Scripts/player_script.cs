using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    
    public float speed = 10f; // movement speed variable for editor with default value
    public float jumpheight = 10f; // jump height of the player
    public Rigidbody rb; // initialize rigidbody 
    private bool ballIsJumpable = true; // boolean to only jump the ball is jumpable

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>(); // reference to rigidbody component
    }

    // Update is called once per frame
    void Update()
    {
        
        // Horizontal Movement - Left/Right with Arrow Keys
        //float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        //transform.Translate(horizontal, 0, 0);

        // Jump Movement - Up with Spacebar, just works when Ball is on the floor
        if (Input.GetButtonDown("Jump") && ballIsJumpable)
        {
            rb.AddForce(new Vector3(0, jumpheight,0), ForceMode.Impulse);
            ballIsJumpable = false;
        }

    }

    // Set back the variable to allow another jump
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Jumpable")
        {
            ballIsJumpable = true;
        }
    }
}
