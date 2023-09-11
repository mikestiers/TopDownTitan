using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerV2 : MonoBehaviour
{
    [System.Serializable]
    public class Wave
    {
        public int drones;
        public int gunners;
        public int seekers;
        public int interceptors;
        public int juggernauts;
        public float delay;
    }

    public Wave[] waves;
    public Enemy dronePrefab;
    public Enemy gunnerPrefab;
    public Enemy seekerPrefab;
    public Enemy interceptorPrefab;
    public Enemy juggernautPrefab;
    private int currentWave = 0;

    private void Start()
    {
        StartCoroutine(StartNextWave());
    }

    IEnumerator StartNextWave()
    {
        if (currentWave < waves.Length)
        {
            HUD.singleton.waveText.text = ($"WAVE {currentWave + 1}");
            HUD.singleton.waveCanvas.SetActive(true);
            yield return new WaitForSeconds(waves[currentWave].delay);
            HUD.singleton.waveCanvas.SetActive(false);

            // Set the amount of enemies to spawn
            Enemy.enemyCount = waves[currentWave].drones + waves[currentWave].gunners + waves[currentWave].seekers + (waves[currentWave].interceptors * 5);

            // Spawn enemies based on current wave's instruction
            StartCoroutine(SpawnEnemy(dronePrefab, waves[currentWave].drones, 2f));
            StartCoroutine(SpawnEnemy(gunnerPrefab, waves[currentWave].gunners, 5f));
            StartCoroutine(SpawnEnemy(seekerPrefab, waves[currentWave].seekers, 7f));
            StartCoroutine(SpawnEnemy(interceptorPrefab, waves[currentWave].interceptors, 10f));

            // Wait until all enemies are defeated and spawn boss
            Debug.Log("Current count = " + Enemy.enemyCount);
            yield return new WaitUntil(() => Enemy.enemyCount == 0);
            Enemy.enemyCount = 1;
            SpawnBoss(juggernautPrefab);
            // Wait until boss dies
            yield return new WaitUntil(() => Enemy.enemyCount == 0);

            currentWave++;
            StartCoroutine(StartNextWave());
        }
    }

    IEnumerator SpawnEnemy(Enemy prefab, int count, float interval)
    {
        for (int i = 0; i < count; i++)
        {
            if (prefab.attackPattern == AttackPattern.Ring)
            {
                Vector3 center = new Vector3(0, 10, 0);
                SpawnEnemyRing(5, 5f, center, prefab);
            }
            else
            {
                SpawnSingleEnemy(prefab);
                yield return new WaitForSeconds(interval);
            }
        }
    }

    void SpawnSingleEnemy(Enemy prefab)
    {
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(Random.Range(0.1f, 0.9f), 1, Camera.main.nearClipPlane));
        spawnPosition.z = 0;
        Instantiate(prefab, spawnPosition, Quaternion.Euler(0, 0, -90));
    }

    void SpawnEnemyRing(int numberOfEnemies, float radius, Vector3 centerPosition, Enemy prefab)
    {
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
            GameObject enemy = Instantiate(prefab.gameObject, spawnPosition, Quaternion.Euler(0, 0, -90));
            enemy.transform.SetParent(pivot.transform);
        }
    }

    void SpawnBoss(Enemy prefab)
    {
        // Spawning boss at center top just slightly below (the 0.75f)
        Vector3 spawnPosition = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.75f, Camera.main.nearClipPlane));
        spawnPosition.z = 0;
        Instantiate(prefab, spawnPosition, Quaternion.Euler(0, 0, -90));
    }
}

