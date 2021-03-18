using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class platform_script : MonoBehaviour
{
    public GameObject Player;
    public Vector3[] waypoints;
    private int waypoint_active = 0;
    private Vector3 target_position;
    private float tolerance;
    public float speed;
    public float delay;
    private float delay_timer;
    public bool automatic;
    
    
    // Start is called before the first frame update
    void Start()
    {
        if (waypoints.Length > 0)
        {
            target_position = waypoints[0];
        }

        tolerance = speed * Time.deltaTime;
    }

    // Update is called once per frame
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

    void NextPlatform()
    {
        waypoint_active++;
        if (waypoint_active >= waypoints.Length)
        {
            waypoint_active = 0;
        }

        target_position = waypoints[waypoint_active];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == Player)
            Player.transform.parent = transform;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == Player)
            Player.transform.parent = null;
    }
}