using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_script : MonoBehaviour
{
    
    public float speed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        // Horizontal Movement - Left/Right
        float horizontal = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        transform.Translate(horizontal, 0, 0);

    }
}
