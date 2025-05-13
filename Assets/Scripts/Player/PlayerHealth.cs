using TMPro;
using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class PlayerHealth : MonoBehaviour
{
    public float baseMaxHealth = 200f;
    public float maxHealth = 200f;
    [HideInInspector] public float health;
    private float newHealth;
    private float oldHealth;
    private float t;

    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private Slider healthbarSlider;

    [SerializeField] private Transform spawnPoint;
    private PlayerController movementScr;
    private CharacterController characterController;
    private PlayerLevelSystem playerLevelSystem;
    

    void Start()
    {
        health = maxHealth;
        newHealth = health;
        movementScr = GetComponent<PlayerController>();
        characterController = GetComponent<CharacterController>();
        playerLevelSystem = GetComponent<PlayerLevelSystem>();
    }

    public void Update()
    {
        if (health != newHealth)
        {
            t += Time.deltaTime * 10f;
            health = Mathf.Clamp(Mathf.Lerp(oldHealth, newHealth, t), 0f, maxHealth);
            if (health <= 0f)
            {
                Die();
            }
            UpdateHealthUI();
        }
    }

    public void Heal(float amount)
    {
        t = 0;
        oldHealth = health;
        newHealth = health + amount;
    }
    public void TakeDamage(float amount)
    {
        t = 0;
        oldHealth = health;
        newHealth = health - amount;
        
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

        oldHealth = health;
        newHealth = maxHealth;
    }

    private void UpdateHealthUI()
    {
        healthText.text = Mathf.Round(health).ToString();
        healthbarSlider.value = health / maxHealth;
    }
}
