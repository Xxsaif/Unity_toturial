using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Weapon : MonoBehaviour
{
    
    
    
    [HideInInspector] public List<GameObject> enemiesHit;

    public WeaponData weaponData;
    protected Inventory inventoryScr;
    protected bool attacking;
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

    protected bool CanAttack() => Input.GetKey(KeyCode.Mouse0) && !attacking && !inventoryScr.inventoryActive;
}
