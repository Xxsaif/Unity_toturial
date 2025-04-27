using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : Weapon
{

    
    [SerializeField] private GameObject blade;
    [SerializeField] private Animator animator;

    void Start()
    {
        attacking = false;
        inventoryScr = GameObject.FindWithTag("Player").GetComponent<InventorySave>();
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
        inventoryScr.canSwapItem = false;
        attacking = true;
        yield return new WaitForSeconds(1.5f/animator.GetCurrentAnimatorStateInfo(0).speed);
        enemiesHit.Clear();
        attacking = false;
        inventoryScr.canSwapItem = true;
    }

    
}
