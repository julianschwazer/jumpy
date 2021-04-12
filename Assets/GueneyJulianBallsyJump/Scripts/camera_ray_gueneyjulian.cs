using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GueneyJulian {
    public enum Interaction_types
    {
        OBJECT_UP, VACUUM_CLEANER, PUSH_OBJECT
    }

    public class camera_ray_gueneyjulian : MonoBehaviour
    {
        public Interaction_types initeraction_type;
        public LayerMask ignoreMe;
        public Transform crosshair_t;
        public Transform push_object;
        public float pushing_strength = 1f;

        private GameObject[] jumpy_objects;

        // Start is called before the first frame update
        void Start()
        {
            jumpy_objects = GameObject.FindGameObjectsWithTag("jumpy");
        }

        // Update is called once per frame
        void Update()
        {
            RaycastHit hit;
            var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

            switch (initeraction_type)
            {
                case Interaction_types.OBJECT_UP:
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.rigidbody != null) {
                        hit.rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
                    }
                break;


                case Interaction_types.VACUUM_CLEANER:
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.collider.name == "Floor") {
                        
                        crosshair_t.position = new Vector3( hit.point.x , 0.51f, hit.point.z); //set crosshair position
                        //use all objects with tag "jumpy"
                        foreach (var one_jumpy in jumpy_objects)
                        {
                            Vector3 power_direction = hit.point - one_jumpy.GetComponent<Transform>().position;
                            one_jumpy.GetComponent<Rigidbody>().AddForce(power_direction, ForceMode.Force);
                        }
                    }
                break;


                case Interaction_types.PUSH_OBJECT:
                    crosshair_t.position = new Vector3( crosshair_t.position.x, 0f, crosshair_t.position.z); //below floor
                    if (!Input.GetButton("Fire1")){
                        push_object.GetComponent<Rigidbody>().isKinematic = true; //make push object not physical
                        push_object.position = new Vector3( push_object.position.x , -1f, push_object.position.z);
                    }
                    if (Physics.Raycast(ray, out hit, 500f, ~ignoreMe) && hit.collider.name == "Floor") {
                        crosshair_t.position = new Vector3( hit.point.x , 0.51f, hit.point.z); //above floor
                        if (Input.GetButton("Fire1")){
                            if (push_object.GetComponent<Rigidbody>().isKinematic){
                                push_object.position = new Vector3( hit.point.x , Mathf.Max(-1f,push_object.position.y), hit.point.z);
                                push_object.GetComponent<Rigidbody>().isKinematic = false;

                            }
                            push_object.GetComponent<Rigidbody>().AddForce(Vector3.up * 100f * Time.deltaTime, ForceMode.Impulse);
                        }
                    } 
                break;
            }
        }
    }
}
//his