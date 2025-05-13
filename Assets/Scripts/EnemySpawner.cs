using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Made by Louis Gericke

public class EnemySpawner : MonoBehaviour
{
    public GameObject player;
    public GameObject enemyPrefab;
    public float zombieSpawnRadius = 10f;
    private float playerDetectRadius;
    public float minSpawnInterval = 3f;
    public float maxSpawnInterval = 8f;

    private float timer;
    private float currentSpawnInterval = 0f;

    private readonly float maxHealth = 500f;
    private float health;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthbarSlider;

    void Start()
    {
        health = maxHealth;
        playerDetectRadius = EnemyBehaviour.searchDistance * 1.5f;
        UpdateHealthUI();
    }

    void Update()
    {
        if (Vector3.Distance(player.transform.position, transform.position) <= playerDetectRadius)
        {
            timer += Time.deltaTime;
            if (timer >= currentSpawnInterval)
            {
                SpawnEnemy();
                timer = 0f;
                currentSpawnInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            }
        }
        healthbarSlider.gameObject.SetActive(Vector3.Distance(player.transform.position, transform.position) <= zombieSpawnRadius);
        
    }

    void SpawnEnemy()
    {
        Vector2 randomOffset = Random.insideUnitCircle * zombieSpawnRadius;
        Vector3 spawnPosition = new Vector3(randomOffset.x, 0, randomOffset.y) + transform.position;

        GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity, transform);
        enemy.GetComponent<EnemyBehaviour>().player = player;
    }

    public void TakeDamage(float amount)
    {
        health -= amount;
        UpdateHealthUI();
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {
        gameObject.SetActive(false);
    }
    private void UpdateHealthUI()
    {
        healthText.text = Mathf.Round(health).ToString();
        healthbarSlider.value = health / maxHealth;
    }
}