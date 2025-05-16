using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// Created by Herman Bergström
public abstract class Weapon : MonoBehaviour
{
    
    
    
    [HideInInspector] public List<GameObject> enemiesHit;

    public WeaponData weaponData;
    protected Hotbar hotbar;
    public static bool playerAttacking = false;
    protected bool attacking = false;
    [SerializeField] protected PlayerLevelSystem playerLevelSystem;
    
    void Update()
    {
        
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected bool CanAttack() => Input.GetKey(KeyCode.Mouse0) && !attacking && !InventorySystem.inventoryActive && !PauseMenu.paused;

    public float Damage() => weaponData.damage * playerLevelSystem.PlayerDamageMultiplier;
}
