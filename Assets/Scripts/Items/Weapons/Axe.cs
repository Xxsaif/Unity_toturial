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
        attacking = false;
        inventory = GameObject.FindWithTag("Player").GetComponent<Inventory>();
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
        hotbar.canSwapItem = false;
        attacking = true;
        yield return new WaitForSeconds(1.5f / animator.GetCurrentAnimatorStateInfo(0).speed);
        enemiesHit.Clear();
        attacking = false;
        hotbar.canSwapItem = true;
    }


}
