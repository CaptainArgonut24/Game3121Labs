using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

public class SpawnManagerX : MonoBehaviour
{
    public GameObject enemyPrefab;
    public GameObject powerupPrefab;

    public float spawnRangeX = 10;
    public float spawnZMin = 15; // set min spawn Z
    public float spawnZMax = 25; // set max spawn Z
    public int enemyCount;
    public int waveCount = 1;

    public GameObject player;

    private Unity.Mathematics.Random random;

    void Awake()
    {
        random = new Unity.Mathematics.Random((uint)System.DateTime.Now.Ticks);
    }

    void Update()
    {
        enemyCount = GameObject.FindGameObjectsWithTag("Enemy").Length;

        if (enemyCount == 0)
        {
            SpawnEnemyWave(waveCount);
        }
    }

    // Generate random spawn position for powerups and enemy balls
    float3 GenerateSpawnPosition()
    {
        float xPos = random.NextFloat(-spawnRangeX, spawnRangeX);
        float zPos = random.NextFloat(spawnZMin, spawnZMax);
        return new float3(xPos, 0, zPos);
    }

    void SpawnEnemyWave(int enemiesToSpawn)
    {
        float3 powerupSpawnOffset = new float3(0, 0, -15); // make powerups spawn at player end

        // If no powerups remain, spawn a powerup
        if (GameObject.FindGameObjectsWithTag("Powerup").Length == 0) // check that there are zero powerups
        {
            Instantiate(powerupPrefab, GenerateSpawnPosition() + powerupSpawnOffset, powerupPrefab.transform.rotation);
        }

        // Spawn number of enemy balls based on wave number
        for (int i = 0; i < enemiesToSpawn; i++)
        {
            var o = Instantiate(enemyPrefab, GenerateSpawnPosition(), enemyPrefab.transform.rotation);
            EnemyX enemyX = o.GetComponent<EnemyX>();
            enemyX.speed = waveCount * 5;
        }

        waveCount++;
        ResetPlayerPosition(); // put player back at start
    }

    // Move player back to position in front of own goal
    void ResetPlayerPosition()
    {
        player.transform.position = new float3(0, 1, -7);
        player.GetComponent<Rigidbody>().velocity = float3.zero;
        player.GetComponent<Rigidbody>().angularVelocity = float3.zero;
    }
}
