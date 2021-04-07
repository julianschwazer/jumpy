using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_script : MonoBehaviour
{
    public GameObject Player; //reference to the player

    public Vector3[] waypoints; //array for the waypoints
    private Vector3 target_position; //variable for target position of platform movement
    
    private int waypoint_active = 0; //number of available waypoints for the editor and default value
    
    private float tolerance; //tolerance for the movement
    public float speed; //speed for the movement
    public float delay; //delay before platform moves to the next waypoint
    private float delay_timer; //delay timer 
    
    public bool automatic; //boolean for automatic start of the platform movement
    
    void Start()
    {
        //convert waypoint list to target positions
        if (waypoints.Length > 0)
        {
            target_position = waypoints[0];
        }

        tolerance = speed * Time.deltaTime; //set a tolerance
    }
    
    void Update()
    {
        if (transform.position != target_position)
        {
            MovePlatform();
        }
        else
        {
            UpdateTarget();
        }
    }

    //movement of the platform
    void MovePlatform()
    {
        Vector3 heading = target_position - transform.position;
        transform.position += (heading / heading.magnitude) * speed * Time.deltaTime;
        if (heading.magnitude < tolerance)
        {
            transform.position = target_position;
            delay_timer = Time.time;
        }
    }
    
    //creating a delay before the platform moves to the next waypoint
    void UpdateTarget()
    {
        if (automatic)
        {
            if (Time.time - delay_timer > delay)
            {
                NextPlatform();
            }
        }
    }
    
    //setting the next waypoint for the platform movement + reset to first waypoint
    void NextPlatform()
    {
        waypoint_active++;
        if (waypoint_active >= waypoints.Length)
        {
            waypoint_active = 0;
        }

        target_position = waypoints[waypoint_active];
    }
    
    //parent player to platform - player moves with the platform
    private void OnTriggerEnter(Collider other)
    {
       if (other.gameObject == Player)
            Player.transform.parent = transform;
    }
    
    //player separates from platform when jumping
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
            Player.transform.parent = null;
    }
    
    // Set back the variable to allow another jump
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject == Player)
        {
            Debug.Log("i am here dude");
            //transform.GetChild(1).localScale = new Vector3(5, 1, 5);
        }
    }

}