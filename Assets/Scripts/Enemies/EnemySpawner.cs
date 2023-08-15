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

    // Update is called once per frame
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

        // Get the top position of the screen in world coordinates
        //Vector3 screenTopY = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height, Camera.main.nearClipPlane));//  new Vector3(0, Input.mousePosition.y, 0f));

        // Set the initial position of the enemy at the top of the screen relative to the player
        // TODO possible bug: get the actual top of the screen instead of calculating based on player transform
        //Vector3 spawnPosition = new Vector3(Random.Range(-5f, 5f), player.position.y + 10, player.position.z);
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1, Camera.main.nearClipPlane));
        spawnPosition = new Vector3(spawnPosition.x, spawnPosition.y, 0);

        // Spawn a random enemy
        int randomIndex = Random.Range(0, enemyPrefabs.Count);
        Enemy selectedEnemyPrefab = enemyPrefabs[randomIndex];
        Instantiate(selectedEnemyPrefab.gameObject, spawnPosition, Quaternion.Euler(0, 0, 180));
    }
}
