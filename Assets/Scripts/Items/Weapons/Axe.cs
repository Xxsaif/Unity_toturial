using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Created by Herman Bergström
public class Axe : Weapon
{


    [SerializeField] private GameObject blade;
    [SerializeField] private Animator animator;

    void Start()
    {
        hotbar = GameObject.FindWithTag("Player").GetComponent<Hotbar>();
    }


    void Update()
    {
        if (CanAttack())
        {
            StartCoroutine(Attack());
            animator.SetTrigger("Attack");
        }

    }

    protected override IEnumerator Attack()
    {
        attacking = true;
        playerAttacking = true;
        yield return new WaitForSeconds(1.5f / animator.GetCurrentAnimatorStateInfo(0).speed);
        enemiesHit.Clear();
        playerAttacking = false;
        attacking = false;
    }


}
