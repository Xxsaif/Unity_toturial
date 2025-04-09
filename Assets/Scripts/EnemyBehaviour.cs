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
    private float moveSpeed = 3f;
    private float tMove = 0f;
    [SerializeField] private AnimationCurve curveMove;

    [SerializeField] private GameObject player;
    private Vector3 playerPosition;
    private EnemyStates state;

    private bool attacking;
    private float damage = 30f;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject model;
    private List<Vector3> pastPlayerPos;
    private List<Vector3> reachablePos;
    private Vector3 closestPos = new Vector3(0f, 0f, 0f);
    private float timer = 0f;
    

    void Start()
    {
        pastPlayerPos = new List<Vector3>();
        reachablePos = new List<Vector3>();
        health = 200f;
        playerPosition = Vector3.zero;

        agent = GetComponent<NavMeshAgent>();
        agent.isStopped = true;
        agent.speed = moveSpeed;
        agent.SetDestination(Vector3.zero);

        state = EnemyStates.Idle;
        attacking = false;

        
    }
    
    void Update()
    {
        /*
        Problem: when enemy can't find path to the target (player) it stops moving.

        Proposed solutions: they go idle, they despawn unless a path is found with short time period or system for finding the closest position to the player that the enemy can find a path to.
        */

        playerPosition = player.transform.position;
        agent.SetDestination(playerPosition);

        //timer += Time.deltaTime;
        //if (timer > 5f && agent.pathStatus == NavMeshPathStatus.PathComplete)
        //{
        //    pastPlayerPos.Add(playerPosition);
        //    if (pastPlayerPos.Count > 10)
        //    {
        //        pastPlayerPos.RemoveAt(0);
        //    }
        //    timer = 0f;
        //}
        //if (agent.pathStatus != NavMeshPathStatus.PathComplete)
        //{
        //    for (int i = 0; i < pastPlayerPos.Count; i++)
        //    {
        //        agent.SetDestination(pastPlayerPos[i]);
        //        if (agent.pathStatus == NavMeshPathStatus.PathComplete)
        //        {
        //            reachablePos.Add(pastPlayerPos[i]);
        //        }
        //    }

        //    for (int i = 0; i < reachablePos.Count; i++)
        //    {
        //        closestPos = Mathf.Min(Vector3.Distance(playerPosition, closestPos), Vector3.Distance(playerPosition, reachablePos[i])) == Vector3.Distance(playerPosition, reachablePos[i]) ? reachablePos[i] : closestPos;
        //    }

        //    agent.SetDestination(closestPos);
        //}

        

        if (Vector3.Distance(transform.position, playerPosition) <= agent.stoppingDistance)
        {
            Stop();
            state = EnemyStates.Attacking;
        }
        else
        {
            CancelInvoke();
            if (agent.isStopped && (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f || animator.GetCurrentAnimatorStateInfo(0).normalizedTime == 0f))
            {
                Move();
                state = EnemyStates.Walking;
                WalkAni();
                attacking = false;
                model.transform.rotation = new Quaternion(0f, 0f, 0f, 0f);
            }
        }
        
        if (!agent.isStopped && state == EnemyStates.Walking)
        {
            tMove += Time.deltaTime / 4.033f;
            moveSpeed = curveMove.Evaluate(tMove) * 2.9f;
            agent.speed = moveSpeed;
        }
        if (state == EnemyStates.Attacking && agent.isStopped)
        {
            if (!Attacking() && !attacking)
            {
                Invoke(nameof(Attack), 1f);
                AttackAni();
                attacking = true;
            }
            if (model.transform.position.y != 0f)
            {
                model.transform.localPosition = new Vector3(model.transform.localPosition.x, 0f, model.transform.localPosition.z);
            }
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

    private void Move()
    {
        agent.isStopped = false;
        agent.speed = moveSpeed;
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
        //IdleAni();
        //animator.SetBool("Walking", false);
        //animator.SetBool("Running", false);
        animator.SetTrigger("zombieAttack");

    }
    public bool Attacking() => animator.GetCurrentAnimatorStateInfo(0).IsName("zombie_attack");
    private enum EnemyStates
    {
        Idle,
        Walking,
        Running,
        Attacking,
        Dying,
        Dead
    }
   
}
