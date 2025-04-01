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
        bladeCollider = blade.GetComponent<Collider>();
        bladeCollider.enabled = false;
        damage = 50f;
        knockbackMultiplier = 5f;
        knockbackDuration = 0.25f;
        inventoryScr = GameObject.FindWithTag("Player").GetComponent<Inventory>();
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
        bladeCollider.enabled = true;
        yield return new WaitForSeconds(2f);
        bladeCollider.enabled = false;
        enemiesHit.Clear();
        attacking = false;
        inventoryScr.canSwapItem = true;
    }

    
}
