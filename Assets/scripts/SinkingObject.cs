using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinkingObject : MonoBehaviour
{
    private Rigidbody rb;

    public float waterLevel = 20f; // Adjust this to the water level height
    public float floatThreshold = 20f; // The object will start sinking below this threshold
    public float buoyancy = 1.0f; // Buoyancy force

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // Check if the object is below the water surface
        if (transform.position.y < waterLevel - floatThreshold)
        {
            // Calculate the buoyant force
            float depth = waterLevel - transform.position.y;
            Vector3 buoyantForce = new Vector3(0f, depth * buoyancy, 0f);

            // Apply the buoyant force to the object
            rb.AddForce(buoyantForce, ForceMode.Force);
        }
    }
}
