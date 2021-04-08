using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_movement : MonoBehaviour
{
    public GameObject Player; // reference to the player
    
    private int waypoint_active = 0; // number of available waypoints for the editor and default value
    
    private float tolerance; // tolerance for the movement
    public float speed; // speed for the movement
    public float delay; // delay before platform moves to the next waypoint
    private float delay_timer; // delay timer 
    public bool automatic; // boolean for automatic start of the platform movement
    
    public Vector3[] waypoints; // array for the waypoints
    private Vector3 target_position; // variable for target position of platform movement

    [Header("Platform Scaling")]
    public bool scaleOnEnter = false; // enable scale on enter
    public bool scaleOnLeave = false; // enable scale on leave
    
    public float scaleX; // variable for scaling
    public float scaleY; // variable for scaling
    public float scaleZ; // variable for scaling

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
            MovePlatform(); // include movement function
        }
        else
        {
            UpdateTarget(); // include update target function
        }
    }

    // movement of the platform
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

    // automatic start of platform and delay
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

    // setting the next waypoint for the platform movement + reset to first waypoint
    void NextPlatform()
    {
        waypoint_active++;
        if (waypoint_active >= waypoints.Length)
        {
            waypoint_active = 0;
        }

        target_position = waypoints[waypoint_active];
    }

    // parent player to platform - player moves with the platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
            if (scaleOnEnter)
            {
                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
        
            Player.transform.parent = transform;
    }

    // player separates from platform when jumping
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
        {
            Player.transform.parent = null;

            if (scaleOnLeave)
            {
                transform.localScale = new Vector3(scaleX, scaleY, scaleZ);
            }
        }
    }
}