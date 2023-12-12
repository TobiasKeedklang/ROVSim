using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ParticleSystemController : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    void Start()
    {
        // Get the ParticleSystem component attached to this GameObject
        myParticleSystem = GetComponent<ParticleSystem>();

        // Check if the ParticleSystem component is found
        if (myParticleSystem == null)
        {
            Debug.LogError("ParticleSystem component not found on ParticleSystemPrefab GameObject.");
        }
        else
        {
            // Play the Particle System on Start
            PlayParticleSystem();
        }
    }

    void PlayParticleSystem()
    {
        // Check if the Particle System is not already playing
        if (!myParticleSystem.isPlaying)
        {
            // Play the Particle System
            myParticleSystem.Play();
        }

        // Invoke the CheckParticleSystemStatus method repeatedly
        InvokeRepeating("CheckParticleSystemStatus", 0f, 0.1f);
    }

    void CheckParticleSystemStatus()
    {
        // Check if the Particle System is not alive (not playing)
        if (!myParticleSystem.IsAlive())
        {
            // Cancel the CheckParticleSystemStatus method
            CancelInvoke("CheckParticleSystemStatus");

            // Particle system has stopped playing, destroy the GameObject
            Destroy(gameObject);
        }
    }
}