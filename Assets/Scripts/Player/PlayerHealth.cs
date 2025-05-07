using TMPro;
using UnityEngine;
using System.Collections;


public class PlayerHealth : MonoBehaviour
{
    private float maxHealth = 200f;
    [HideInInspector] public float health;
    [SerializeField] private TextMeshProUGUI healthText; // Temporary health text, should be replaced with health bar

    [SerializeField] private Transform spawnPoint;

    

    void Start()
    {
        health = maxHealth;
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
        if (health <= 1f)
        {
            Die();
        }
    }

    private void Die()
    {
        // Stäng av rörelse
        var movement = GetComponent<PlayerController>();
        if (movement != null)
        {
            movement.enabled = false;
        }


        // inaktivera character controller
        var cc = GetComponent<CharacterController>();
        if (cc != null)
        {
            cc.enabled = false;
        }


        // teleportera 
        transform.position = spawnPoint.position;

        // activate CharacterController
        if (cc != null)
        {
            cc.enabled = true;
        }

        // aktivera rörelsen igen 
        if (movement != null)
        {
            movement.enabled = true;
        }

        // Reseta health
        health = maxHealth;
        healthText.text = health.ToString() + "HP";

    }

  

}
