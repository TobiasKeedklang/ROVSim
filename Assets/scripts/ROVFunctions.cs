using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ROVFunctions : MonoBehaviour
{
    public Light targetLight; // Reference to the Light component you want to toggle
    
    public string objectTag = "Seabed"; // Tag of the object to check proximity to
    public string objectTag2 = "wellhead";
    public float proximityDistance = 1.3f;     // Distance at which the particle effect should play
    public float proximityDistance2 = 3.0f;
    public ParticleSystem particleEffect;      // Reference to the Particle System
    public TextMeshProUGUI promptText;

    void Update()
    {
        CheckProximityToSeabed();
        CheckProximityToWellhead();

        if (Input.GetKeyDown(KeyCode.R))
        {
            ToggleLight();
        }
    }

    private void CheckProximityToSeabed()
    {
        GameObject[] taggedObjects = GameObject.FindGameObjectsWithTag(objectTag);

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
                PlayParticleEffect();
            }
        }
    }

    void CheckProximityToWellhead()
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
    }
    private void PlayParticleEffect()
    {
        if (particleEffect != null)
        {
            if (!particleEffect.isPlaying)
            {
                particleEffect.Play();
            }
        }
    }

    void ToggleLight()
    {
        if (targetLight != null)
        {
            targetLight.enabled = !targetLight.enabled; // Toggle the light's state
        }
        else
        {
            Debug.LogError("Light component is not assigned.");
        }
    }
}
