using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using System;
using Unity.Mathematics;
using UnityEngine.UI;


public class EnemyBehaviour : MonoBehaviour
{
    private float baseMaxHealth = 200f;
    private float maxHealth;
    [HideInInspector] public float health;
    [SerializeField] private TextMeshProUGUI healthText; // Temporary health text, should be replaced with health bar

    [SerializeField] private NavMeshAgent agent;
    private float moveSpeed;
    private float tMove = 0f;
    [SerializeField] private AnimationCurve curveMove;

    [SerializeField] public GameObject player;
    private Vector3 playerPosition;
    [SerializeField] private States state;

    private float damage = 30f;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject model;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private LayerMask wallMask;
    private float detectionRadius = 25f;
    private float attackRadius = 2f;
    [SerializeField] private bool canAttack = false;
    private bool attacking = false;
    [SerializeField] private bool playerDetected = false;
    private System.Random r = new System.Random();
    private readonly float distance = 40f;
    private bool wasChasing = true;
    private int loopNum = 0;

    private Vector3 moveStartPos;
    private bool moveBack = false;
    private float t = 0f;
    private PlayerLevelSystem playerLevelSystem;
    private EnemyLevelSystem enemyLevelSystem;
    [SerializeField] private Slider healthbarSlider;
    private Camera playerCam;
    [SerializeField] private Visable visableScr;
    [SerializeField] private Collider objCollider;
    void Start()
    {
        playerPosition = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Vector3.zero);

        state = States.Searching;
        playerLevelSystem = GameObject.Find("Player").GetComponent<PlayerLevelSystem>();
        enemyLevelSystem = GetComponent<EnemyLevelSystem>();
        maxHealth = baseMaxHealth * enemyLevelSystem.EnemyHealthMultiplier;
        health = maxHealth;
        UpdateHealthUI();
        playerCam = GameObject.Find("PlayerCam").GetComponent<Camera>();
    }
    
    void Update()
    {
        playerPosition = player.transform.position;

        playerDetected = Physics.CheckSphere(transform.position, detectionRadius, playerMask);
        canAttack = Physics.CheckSphere(transform.position, attackRadius, playerMask);

        state = playerDetected ? canAttack ? States.Attacking : States.Chasing : States.Searching;
        switch (state)
        {
            case States.Attacking:
                AttackAni();
                agent.isStopped = true;
                agent.speed = 0f;

                if (!Attacking() && !attacking)
                {
                    Invoke(nameof(Attack), 1f);
                    AttackAni();
                    attacking = true;
                }
                if (model.transform.localPosition.y != 0f)
                {
                    model.transform.localPosition = new Vector3(model.transform.localPosition.x, 0f, model.transform.localPosition.z);
                }
                break;


            case States.Chasing:
                agent.SetDestination(playerPosition);
                CancelInvoke();
                if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f || animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0f)
                {
                    attacking = false;
                    model.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
                }
                wasChasing = true;
                break;

            case States.Searching:
                if ((agent.remainingDistance <= agent.stoppingDistance && Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance) || wasChasing)
                {
                    bool destinationFound = false;
                    while (!destinationFound)
                    {
                        Vector3 destination = new(r.Next(-100, 100), 0f, r.Next(-100, 100));
                        destination.Normalize();
                        destination = destination * distance + playerPosition;
                        for (float i = 1; i < 6; i++)
                        {
                            if (Physics.Raycast(new Vector3(destination.x, destination.y + i, destination.z), Vector3.down, out RaycastHit info, 10f, groundMask) && !Physics.CheckSphere(info.point, 1f, wallMask))
                            {
                                agent.SetDestination(info.point);
                                if (agent.pathStatus == NavMeshPathStatus.PathComplete)
                                {
                                    destinationFound = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                wasChasing = false;
                break;
        }

        if (state == States.Searching || state == States.Chasing)
        {
            WalkAni();
            agent.isStopped = false;
            tMove += Time.deltaTime / 4.033f;
            moveSpeed = curveMove.Evaluate(tMove) * 2.9f;
            agent.speed = moveSpeed;
            if ((int)tMove != loopNum)
            {
                moveBack = true;
                loopNum = (int)tMove;
                moveStartPos = model.transform.localPosition;
                t = 0f;
            }
        }

        if (moveBack)
        {
            t += Time.deltaTime * 2f;
            model.transform.localPosition = new Vector3(model.transform.localPosition.x, model.transform.localPosition.y, Mathf.Lerp(moveStartPos.z, 0f, t));
            if (t >= 1f)
            {
                moveBack = false;
            }
        }

        Vector3 healthbarPosition = new Vector3(transform.position.x, transform.position.y + 2f, transform.position.z);
        healthbarSlider.gameObject.SetActive(Physics.Raycast(healthbarPosition, playerCam.transform.position - healthbarPosition, Vector3.Distance(healthbarPosition, playerCam.transform.position), playerMask) && GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(playerCam), objCollider.bounds));
        if (healthbarSlider.gameObject.activeSelf)
        {
            healthbarSlider.gameObject.transform.position = playerCam.WorldToScreenPoint(healthbarPosition);
            float scale = 1.2f/Vector3.Distance(transform.position, playerPosition);
            healthbarSlider.gameObject.transform.localScale = new Vector3(scale, scale, scale);
        }

    }

    public void TakeDmg(float dmg)
    {
        health -= dmg;
        UpdateHealthUI();
        if (health <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        // temporarily turned off dying for the sake of testing
        //health = 200f;
        //healthText.text = health.ToString() + "hp";
        gameObject.SetActive(false);
        playerLevelSystem.IncreaseExperience(50f);
    }

    private void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }

    private void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage * enemyLevelSystem.EnemyDamageMultiplier);
        attacking = false;
    }
    
    private void IdleAni()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Running", false);
    }
    private void WalkAni()
    {
        animator.SetBool("Walking", true);
        animator.SetBool("Running", false);
    }

    private void RunAni()
    {
        animator.SetBool("Walking", false);
        animator.SetBool("Running", true);

    }

    private void AttackAni()
    {
        animator.SetTrigger("zombieAttack");
    }

    private void UpdateHealthUI()
    {
        healthText.text = Mathf.Round(health).ToString();
        healthbarSlider.value = health / maxHealth;
    }
    public bool Attacking() => animator.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack");
    private enum States
    {
        Attacking,
        Chasing,
        Searching
    }
   
}
