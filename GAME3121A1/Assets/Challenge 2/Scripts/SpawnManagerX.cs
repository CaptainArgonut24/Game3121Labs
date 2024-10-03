using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject[] ballPrefabs;


    private float spawnLimitXLeft = -19;
    private float spawnLimitXRight = 9;
    private float spawnPosY = 30;

    private float startDelay = 1.0f;
    private float minSpawnInterval = 3.0f;
    private float maxSpawnInterval = 5.0f;

    void Start()
    {
        InvokeRepeating("SpawnRandomBall", startDelay, Random.Range(minSpawnInterval, maxSpawnInterval));
    }

    // Spawn random ball at random 
    void SpawnRandomBall()
    {
      
        int index = Random.Range(0, ballPrefabs.Length);
        Vector3 spawnPos = new Vector3(Random.Range(spawnLimitXLeft, spawnLimitXRight), spawnPosY, 0);

        Instantiate(ballPrefabs[index], spawnPos, ballPrefabs[index].transform.rotation);
    }

}
