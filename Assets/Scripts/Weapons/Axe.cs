using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Axe : Weapon
{


    [SerializeField] private GameObject blade;
    [SerializeField] private Animator animator;

    void Start()
    {
        attacking = false;
        bladeCollider = blade.GetComponent<Collider>();
        bladeCollider.enabled = false;
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
