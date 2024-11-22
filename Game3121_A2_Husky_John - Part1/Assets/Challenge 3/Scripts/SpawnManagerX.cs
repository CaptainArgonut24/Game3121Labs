using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] objectPrefabs;
    private float spawnDelay = 2;
    private float spawnInterval = 1.5f;

    private PlayerControllerX playerControllerScript;

    private Unity.Mathematics.Random rng;

    // Start is called before the first frame update
    void Start()
    {
        
        rng = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);

        //Q 3. changed from prawns to spawn
        InvokeRepeating("SpawnObjects", spawnDelay, spawnInterval);
        playerControllerScript = GameObject.Find("Player").GetComponent<PlayerControllerX>();
    }

    // Spawn obstacles
    void SpawnObjects ()
    {
        

        float3 spawnLocation = new float3(30, GenerateRandomFloat(5, 15), 0);
        int index = GenerateRandomInt(0, objectPrefabs.Length);


        // If game is still active, spawn new object
        if (!playerControllerScript.gameOver)
        {
            Instantiate(objectPrefabs[index], spawnLocation, objectPrefabs[index].transform.rotation);
        }

    }

    private float GenerateRandomFloat(float min, float max)
    {
        return math.lerp(min, max, rng.NextFloat());
    }

    private int GenerateRandomInt(int min, int max)
    {
        return rng.NextInt(min, max);
    }
}
