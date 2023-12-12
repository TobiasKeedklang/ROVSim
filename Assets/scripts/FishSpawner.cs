using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class FishSpawner : MonoBehaviour
{
    public GameObject fishPrefab;
    public int spawnRange = 5;

    static int fishAmount = 10;
    public static GameObject[] totalFish = new GameObject[fishAmount];
    
    public static Vector3 newPos = Vector3.zero;
    
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < fishAmount; i++)
        {
            Vector3 pos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(1, 5),
                Random.Range(-spawnRange, spawnRange));
            totalFish[i] = (GameObject)Instantiate(fishPrefab, pos, quaternion.identity);

        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Random.Range(0,10000) < 50)
        {
            newPos = new Vector3(Random.Range(-spawnRange, spawnRange), Random.Range(-spawnRange, spawnRange),
                Random.Range(-spawnRange, spawnRange));
        }
    }
}
