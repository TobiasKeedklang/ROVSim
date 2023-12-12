using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrentZone : MonoBehaviour
{
    public float windStrength = 10f; // Adjust the strength of the wind
    public ParticleSystem windParticles; // Reference to the particle system

    private void OnTriggerStay(Collider other)
    {
        Rigidbody rb = other.GetComponent<Rigidbody>();

        if (rb != null)
        {
            // Apply force to the Rigidbody in world space
            rb.AddForce(transform.forward * windStrength, ForceMode.Force);
        }

        // Emit particles at the position of the wind zone
        if (windParticles != null)
        {
            windParticles.transform.position = transform.position;
            windParticles.Play();
        }
    }
}
