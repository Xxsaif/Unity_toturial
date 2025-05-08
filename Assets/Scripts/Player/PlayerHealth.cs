using TMPro;
using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    public float baseMaxHealth = 200f;
    public float maxHealth = 200f;
    [HideInInspector] public static float health;
    [SerializeField] private TextMeshProUGUI healthText; // Temporary health text, should be replaced with health bar

    [SerializeField] private Transform spawnPoint;
    private PlayerController movementScr;
    private CharacterController characterController;
    private PlayerLevelSystem playerLevelSystem;
    

    void Start()
    {
        health = maxHealth;
        movementScr = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        playerLevelSystem = GetComponent<PlayerLevelSystem>();
    }

    
    void Update()
    {
        
    }

    public void Heal(float amount)
    {
        health += amount;
        health = Mathf.Clamp(health, 0, maxHealth);
        healthText.text = health.ToString() + "HP";
    }
    public void TakeDamage(float amount)
    {
        health -= amount;
        healthText.text = health.ToString() + "HP";
        if (health <= 0f)
        {
            Die();
        }
    }

    public void UpdateMaxHealth()
    {
        maxHealth = baseMaxHealth * playerLevelSystem.PlayerHealthMultiplier;
        Heal(maxHealth);
    }

    private void Die()
    {
        movementScr.enabled = false;
        characterController.enabled = false;

        transform.position = spawnPoint.position;

        characterController.enabled = true;
        movementScr.enabled = true;

        health = maxHealth;
        healthText.text = health.ToString() + "HP";
    }
}
