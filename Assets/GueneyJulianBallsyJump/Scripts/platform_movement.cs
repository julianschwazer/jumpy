using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
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

    private float initialX; // initial X scale
    private float initialY; // initial Y scale
    private float initialZ; // initial Z scale
    
    private float currentScale = 0; // start for percentage calculation
    private float targetScale = 100; // end for percentage calculation
    private const int FramesCount = 200; // frame count = scaling speed
    private float ds; // delta scale
    private const float AnimationTimeSeconds = 2; // scaling time in s
    private float deltaTime = AnimationTimeSeconds/FramesCount; // scaling
   
    private float tileX = 5; // target X variable of tile scale
    private float tileY = 5; // target Y variable of tile scale
    private float initTileX = 1; // initial X variable of tile scale
    private float initTileY = 1; // initial Y variable of tile scale

    void Start()
    {
        // setting default values for the scaling platforms
        ds = (targetScale - currentScale)/FramesCount; 
        initialX = transform.localScale.x;
        initialY = transform.localScale.y;
        initialZ = transform.localScale.z;
            
        // convert waypoint list to target positions
        if (waypoints.Length > 0)
        {
            target_position = waypoints[0];
        }

        tolerance = speed * Time.deltaTime; // set a tolerance
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
                StartCoroutine(Enlarge());
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
                StartCoroutine(Enlarge());
            }
        }
    }
    
    // platform scaling animation
    private IEnumerator Enlarge() 
    { 
        bool upScale = true;
        while (upScale) 
        {
            currentScale += ds; // current scale increased by delta scale
            if (currentScale > targetScale) // if current scale reaches target scale stop enlarging
            {
                upScale = false;
                currentScale = targetScale;
            }

            float currentXscale = (scaleX - initialX) / 100 * currentScale + initialX; // X scale percentage 
            float currentYscale = (scaleY - initialY) / 100 * currentScale + initialY; // Y scale percentage
            float currentZscale = (scaleZ - initialZ) / 100 * currentScale + initialZ; // Z scale percentage
           
            float currentTileX = (tileX - initTileX) / 100 * currentScale + initTileX; // tile X scale percentage 
            float currentTileY = (tileY - initTileY) / 100 * currentScale + initTileY; // tile Y scale percentage
            
            GetComponent<Renderer>().material.mainTextureScale = new Vector2(currentTileX, currentTileY); // update the tile scale

            var prevParent = Player.transform.parent; // remember the previous parent
            
            Player.transform.parent = null; // so that player doesn't also scale with platform
            transform.localScale = new Vector3(currentXscale,currentYscale,currentZscale); // update the scale
            Player.transform.parent = prevParent; // reattach the player to the previous parrent
            
            yield return new WaitForSeconds(deltaTime);
        }
    }
}