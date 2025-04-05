using TMPro;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 200f;
    [HideInInspector] public float health;
    [SerializeField] private TextMeshProUGUI healthText; // Temporary health text, should be replaced with health bar
    void Start()
    {
        health = maxHealth;
    }

    
    void Update()
    {
        
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        healthText.text = health.ToString() + "HP";
        if (health <= 0f)
        {
            Die();
        }
    }

    private void Die()
    {

    }
}
