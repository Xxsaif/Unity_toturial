using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    
    
    
    [HideInInspector] public List<GameObject> enemiesHit;
    protected Collider bladeCollider;

    [HideInInspector] public float damage;
    [HideInInspector] public float knockbackMultiplier;
    [HideInInspector] public float knockbackDuration;
    protected Inventory inventoryScr;
    void Start()
    {
        
    }
    
    void Update()
    {
        
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    
}
