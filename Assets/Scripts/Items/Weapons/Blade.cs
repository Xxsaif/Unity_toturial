using System.Collections;
using UnityEngine;
// Created by Herman Bergström
public class Blade : MonoBehaviour
{
    [SerializeField] private Weapon weapon;
    private Weapon weaponScr;
    private GameObject player;
    [SerializeField] private PlayerController playerMoveScr;
    
    

    void Start()
    {
        weaponScr = weapon.GetComponent<Weapon>();
        player = GameObject.Find("Player");
       
    }
 

    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && !weaponScr.enemiesHit.Contains(other.gameObject)) // 8 is the enemy layer. other.gameObject.layer returns an int and not a layermask for some reason.
        {
            EnemyBehaviour enemy = other.GetComponent<EnemyBehaviour>();
            if (enemy != null)
            {
                enemy.TakeDmg(weaponScr.Damage());
                StartCoroutine(Knockback(other.gameObject));
            }
            weaponScr.enemiesHit.Add(other.gameObject); // adds enemy to list of enemies hit to make sure that the same enemy can't be hit twice from the same attack
        }

        if (other.gameObject.TryGetComponent<EnemySpawner>(out _))
        {
            EnemySpawner enemySpawner = other.GetComponent<EnemySpawner>();
            if (enemySpawner != null)
            {
                enemySpawner.TakeDamage(weaponScr.Damage());
            }
            weaponScr.enemiesHit.Add(other.gameObject);
        }
    }

    private IEnumerator Knockback(GameObject enemy)
    {
        Rigidbody enemyRB = enemy.GetComponent<Rigidbody>();
        Vector3 forceDir = enemy.gameObject.transform.position - player.transform.position;
        forceDir = new Vector3(forceDir.x, 0f, forceDir.z);
        forceDir.Normalize();
        forceDir *= weaponScr.weaponData.knockbackMultiplier;
        enemyRB.AddForce(forceDir, ForceMode.Impulse);
        yield return new WaitForSeconds(weaponScr.weaponData.knockbackDuration);
        //enemyRB.AddForce(-0.5f * forceDir, ForceMode.Impulse);
        enemyRB.linearVelocity = Vector3.zero;


    }
}
