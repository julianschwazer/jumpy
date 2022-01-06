using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shooter_movement : MonoBehaviour
{
    public float delta = 1.5f; // amount of movement left and right
    public float speed = 2.0f; // movement speed
    
    private Vector3 startPos; // variable for the starting position
 
    void Start () {
        startPos = transform.position; // setting starting position
    }
    
    // Left and Right Movement for the Shooters
    void Update ()
    {
        Vector3 v = startPos;
        v.x += delta * Mathf.Sin (Time.time * speed);
        transform.position = v;
    }
}
