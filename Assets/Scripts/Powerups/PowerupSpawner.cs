using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    public GameObject powerupPrefab;  // The powerup prefab to spawn
    public float spawnInterval = 15f; // Time between powerup spawns
    public float powerupLifetime = 2f; // Time before powerup is destroyed 

    // Define the spawn ranges for X and Y
    public float xMin = -10f;
    public float xMax = 10f;
    public float yMin = -4f;
    public float yMax = 4f;

    void Start()
    {
        StartCoroutine(SpawnPowerups());
    }

    // Coroutine to spawn powerups at 15s intervals
    private IEnumerator SpawnPowerups()
    {
        while (true)
        {
            // Wait for the spawn interval to finish before spawning a new power up
            yield return new WaitForSeconds(spawnInterval);

            // Calculate random positions within the defined ranges
            float randomX = Random.Range(xMin, xMax);
            float randomY = Random.Range(yMin, yMax);

            // Create a spawn position using the random X and Y values
            Vector3 spawnPosition = new Vector3(randomX, randomY, -1);

            // Instantiate the powerup at the random position
            GameObject powerup = Instantiate(powerupPrefab, spawnPosition, Quaternion.identity);

            // Destroy the instantiated powerup after 2s
            Destroy(powerup, powerupLifetime);
        }
    }
}
