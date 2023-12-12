using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishAI : MonoBehaviour
{
    public float moveSpeed = 0.01f;
    private float rotSpeed = 4.0f;
    private Vector3 averageHeading;
    private Vector3 averagePosition;
    private float fishDistance = 2.0f;
    private float xBoundary = 30.0f;
    private float yBoundary = 10.0f;
    private float zBoundary = 30.0f;
    
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = Random.Range(1f, 2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,5) < 1)
        {
            EngageFish();
        }
        
        MoveFish();
        
    }

    void MoveFish()
    {
        transform.Translate(0, 0, Time.deltaTime  * moveSpeed);

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -xBoundary, xBoundary),
            Mathf.Clamp(transform.position.y, -yBoundary, yBoundary),
            Mathf.Clamp(transform.position.z, -zBoundary, zBoundary));
    }

    // ReSharper disable Unity.PerformanceAnalysis
    void EngageFish()
    {
        GameObject[] objects;
        objects = FishSpawner.totalFish;

        Vector3 centre = Vector3.zero;
        Vector3 avoid = Vector3.zero;

        float groupSpeed = 0.1f;

        Vector3 newPos = FishSpawner.newPos;

        float dist;

        int groupSize = 0;
        foreach (GameObject fish in objects)
        {
            if (fish != this.gameObject)
            {
                dist = Vector3.Distance(fish.transform.position, this.transform.position);
                if (dist <= fishDistance)
                {
                    centre += fish.transform.position;
                    groupSize++;
                    if (dist < 1.0f)
                    {
                        avoid = avoid + (this.transform.position - fish.transform.position);
                    }

                    FishAI anotherFlock = fish.GetComponent<FishAI>();
                    groupSpeed = groupSpeed + anotherFlock.moveSpeed;
                }
            }
        }

        if (groupSize > 0)
        {
            centre = centre / groupSize + (newPos - this.transform.position);
            moveSpeed = groupSpeed / groupSize;

            Vector3 direction = (centre + avoid) - transform.position;
            if (direction != Vector3.zero)
            {
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotSpeed * Time.deltaTime);
            }
        }
        
    }
}
