using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public List<Enemy> enemyPrefabs;
    public float timeBetweenSpawns = 2.0f;
    private float nextSpawnTime = 0f;
    private Transform player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + timeBetweenSpawns;
        }
    }

    void SpawnEnemy()
    {
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs set in the list");
            return;
        }

        // Set the initial position of the enemy at the top of the screen relative to the player
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1, Camera.main.nearClipPlane));
        spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

        // Spawn a random enemy
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        Enemy selectedEnemyPrefab = enemyPrefabs[randomIndex];

        if (selectedEnemyPrefab.attackPattern == AttackPattern.Ring)
        {
            Vector3 center = new Vector3(0, 10, 0);
            SpawnEnemyRing(5, 5f, center, selectedEnemyPrefab);
        }
        else
        {
            Instantiate(selectedEnemyPrefab.gameObject, spawnPosition, Quaternion.Euler(0, 0, -90));
        }
    }

    void SpawnEnemyRing(int numberOfEnemies, float radius, Vector3 centerPosition, Enemy selectedEnemyPrefab)
    {
        // Ensure there are enemy prefabs to spawn
        if (enemyPrefabs.Count == 0)
        {
            Debug.LogWarning("No enemy prefabs set in the list");
            return;
        }

        // Create a GameObject to rotate and add the RingRotation script to the it
        GameObject pivot = new GameObject("RingPivot");
        pivot.AddComponent<RingRotation>();
        pivot.transform.position = centerPosition; // This centers the pivot on both X and Y axis.

        // Calculate the angle between each enemy in the ring
        float angleBetweenEnemies = 360f / numberOfEnemies;

        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Calculate the position of each enemy based on the angle and radius
            float x = centerPosition.x + radius * Mathf.Cos(i * angleBetweenEnemies * Mathf.Deg2Rad);
            float y = centerPosition.y + radius * Mathf.Sin(i * angleBetweenEnemies * Mathf.Deg2Rad);
            Vector3 spawnPosition = new Vector3(x, y, 0);

            // Instantiate the enemy and make it a child of the pivot
            GameObject enemy = Instantiate(selectedEnemyPrefab.gameObject, spawnPosition, Quaternion.Euler(0, 0, -90));
            enemy.transform.SetParent(pivot.transform);
        }
    }
}