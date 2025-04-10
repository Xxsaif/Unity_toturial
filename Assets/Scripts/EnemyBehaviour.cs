using NUnit.Framework;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBehaviour : MonoBehaviour
{
    [HideInInspector] public float health;
    [SerializeField] private TextMeshPro healthText; // Temporary health text, should be replaced with health bar

    [SerializeField] private NavMeshAgent agent;
    private float moveSpeed;
    private float tMove = 0f;
    [SerializeField] private AnimationCurve curveMove;

    [SerializeField] private GameObject player;
    private Vector3 playerPosition;
    private States state;

    private float damage = 30f;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject model;

    [SerializeField] private LayerMask playerMask;
    private float detectionRadius = 25f;
    private float attackRadius = 2f;
    [SerializeField] private bool canAttack = false;
    private bool attacking = false;
    [SerializeField] private bool playerDetected = false;
    

    void Start()
    {
        health = 200f;
        playerPosition = Vector3.zero;

        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(Vector3.zero);

        state = States.Searching;
        
        
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
                break;

            case States.Searching:
                agent.SetDestination(playerPosition); // should be replaced with searching system
                break;
        }

        if (state == States.Searching || state == States.Chasing)
        {
            WalkAni();
            agent.isStopped = false;
            tMove += Time.deltaTime / 4.033f;
            moveSpeed = curveMove.Evaluate(tMove) * 2.9f;
            agent.speed = moveSpeed;
        }

    }
    public void TakeDmg(float dmg)
    {
        health -= dmg;
        healthText.text = health.ToString() + "hp";
        if (health <= 0f)
        {
            Die();
        }
    }
    
    private void Die()
    {
        //gameObject.SetActive(false);
        // temporarily turned of dying for the sake of testing
        health = 200f;
        healthText.text = health.ToString() + "hp";
    }

    private void Stop()
    {
        agent.isStopped = true;
        agent.speed = 0;
    }

    private void Attack()
    {
        player.GetComponent<PlayerHealth>().TakeDamage(damage);
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
    public bool Attacking() => animator.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack");
    private enum States
    {
        Attacking,
        Chasing,
        Searching
    }
   
}
