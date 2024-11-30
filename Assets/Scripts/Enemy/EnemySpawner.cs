using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform enemyPrefab1; // Prefab for enemy type 1
    public Transform enemyPrefab2; // Prefab for enemy type 2
    public BezierCurve bezierCurve; // Reference to the Bézier curve
    public Transform enemiesParent; // Parent object for organizing spawned enemies

    public float initialSpawnInterval = 3f; // Initial spawn interval
    public float minSpawnInterval = 0.5f;   // Minimum spawn interval
    public float spawnRateIncrease = 0.1f;  // Rate at which spawn interval decreases

    private float currentSpawnInterval;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
        StartCoroutine(SpawnEnemies());
    }

    private IEnumerator SpawnEnemies()
    {
        while (true)
        {
            // Randomly select which enemy to spawn
            if (Random.value < 0.5f)
            {
                SpawnEnemy(enemyPrefab1); // Type 1 enemy with speed 2 and health 50
            }
            else
            {
                SpawnEnemy(enemyPrefab2); // Type 2 enemy with speed 2 and health 100
            }

            // Decrease the spawn interval, ensuring it doesn't go below the minimum
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnRateIncrease);

            // Wait before spawning the next enemy
            yield return new WaitForSeconds(currentSpawnInterval);
        }
    }

    private void SpawnEnemy(Transform prefab)
    {
        // Instantiate the enemy at the start of the Bézier curve
        Transform enemyTransform = Instantiate(prefab, bezierCurve.GetPoint(0), Quaternion.identity);

        // Make the enemy a child of the specified parent
        if (enemiesParent != null)
        {
            enemyTransform.SetParent(enemiesParent);
        }

        // Configure the enemy's properties
        Enemy enemyScript = enemyTransform.GetComponent<Enemy>();
        enemyScript.bezierCurve = bezierCurve;
    }
}
