using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.VFX;

public class ROVFunctions : MonoBehaviour
{
    public Light targetLight; // Reference to the Light component you want to toggle
    
    public string objectTag = "Seabed"; // Tag of the object to check proximity to
    public string objectTag2 = "wellhead";

    public float proximityDistance = 1.3f;     // Distance at which the particle effect should play
    public float equilibriumDistance = 3.0f;
    public float proximityDistance2 = 3.0f;
    public float upForce;
    public float maxPushForce = 20.0f;
    public float dampingFactor = 0.98f;
    public float forceDampingFactor = 0.95f; // Adjust this value to control the rate of force reduction

    public ROVMovement ROVM;
    public GameObject particleEffect;      // Reference to the Particle System
    public TextMeshProUGUI promptText;
    private Rigidbody rb;

    private bool bhasSpawnedParticleSystem = false;
    private bool bIsLightOn = true;

    private void Start()
    {

        rb = GetComponent<Rigidbody>();
        if (rb == null)
        {
            Debug.LogError("Rigidbody component not found on the player GameObject.");
        }

       
    }
    void Update()
    {
        upForce = ROVM.upForce;

        CheckProximityToSeabed();
        CheckProximityToWellhead();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleLight();
        }
    }

    void CheckProximityToSeabed()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(objectTag);
        bool isCloseToAnyObject = false;

        foreach (GameObject obj in taggedObjects)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
            {
                Debug.LogWarning("The tagged object does not have a Collider component.");
                continue;
            }

            Bounds bounds = objCollider.bounds;
            Vector3 closestPoint = bounds.ClosestPoint(transform.position);

            float distance = Vector3.Distance(transform.position, closestPoint);

            if (distance < proximityDistance)
            {
                isCloseToAnyObject = true;

                if(upForce>0)
                {
                    // Check if Particle System has already been spawned
                    if (!bhasSpawnedParticleSystem)
                    {
                        // Spawn a new Particle System under the player
                        PlayParticles();

                        // Set the flag to true to avoid spawning more Particle Systems
                        bhasSpawnedParticleSystem = true;
                    }

                    // Calculate the direction away from the seabed
                    Vector3 awayDirection = (transform.position - closestPoint).normalized;

                    // Calculate force based on distance with damping
                    float forceDamped = Mathf.Pow(1f - forceDampingFactor, distance / equilibriumDistance);
                    float pushForce = Mathf.Lerp(0f, maxPushForce, Mathf.InverseLerp(proximityDistance, equilibriumDistance, distance)) * forceDamped;

                    // Apply a force to push the player away
                    rb.AddForce(awayDirection * pushForce, ForceMode.Impulse);

                    // Apply damping to gradually reduce speed
                    rb.velocity *= dampingFactor;
                }



                break;
            }
        }

        // Reset the flag when the player is no longer close to any objects
        if (!isCloseToAnyObject)
        {
            bhasSpawnedParticleSystem = false;
        }

        // Additional logic based on proximity, e.g., displaying or hiding the text prompt
        if (promptText != null)
        {
            promptText.gameObject.SetActive(isCloseToAnyObject);
        }
    }



    void CheckProximityToWellhead()//Makes reference to a tagged object and cjecks proximity to activate a text on screen
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(objectTag2);
        bool isCloseToAnyObject = false;

        foreach (GameObject obj in taggedObjects)
        {
            Collider objCollider = obj.GetComponent<Collider>();
            if (objCollider == null)
            {
                Debug.LogWarning("The tagged object does not have a Collider component.");
                continue;
            }

            Bounds bounds = objCollider.bounds;
            Vector3 closestPoint = bounds.ClosestPoint(transform.position);

            float distance = Vector3.Distance(transform.position, closestPoint);

            if (distance < proximityDistance2)
            {
                isCloseToAnyObject = true;
                break;
            }
        }

        // Display or hide the text prompt based on proximity
        if (promptText != null)
        {
            promptText.gameObject.SetActive(isCloseToAnyObject);
        }
        if(promptText && Input.GetKeyDown(KeyCode.Tab))
        {
            SceneManager.LoadScene("Menu");
        }
    }

    void ToggleLight() // Turns the light component on or off
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled; // Toggle the light's state
            bIsLightOn = !bIsLightOn;
        }
        else
        {
            Debug.LogError("Light component is not assigned.");
        }
    }

    void PlayParticles()
    {
        Vector3 ROVPos = transform.position;
        ROVPos.y = ROVPos.y-2;
        // Instantiate the Particle System prefab at the player's position
        if (particleEffect != null)
        {
            Instantiate(particleEffect, ROVPos, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Particle System prefab is not assigned in the inspector.");
        }
    }

}
