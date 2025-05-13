using UnityEngine;

// Made by Louis Gericke

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public float spawnRadius = EnemyBehaviour.searchDistance * 1.5f; // Spawn radius should be longer than the search distance, that way the player isn't always immediatelly detected when the zombie spawns
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 8f;

    private float timer;
    private float currentSpawnInterval = 0f;

    void Start()
    {
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= spawnRadius)
        {
            timer += Time.deltaTime;
            if (timer >= currentSpawnInterval)
            {
                SpawnEnemy();
                Debug.Log("Spawn");
                timer = 0f;
                currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            }
        }
    }

    void SpawnEnemy()
    {
        Vector2 randomOffset = Random.insideUnitCircle * spawnRadius;
        Vector3 spawnPosition = new Vector3(randomOffset.x, 0, randomOffset.y) + transform.position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehaviour>().player = player;
    }
}