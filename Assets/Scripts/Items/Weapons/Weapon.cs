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
    protected Inventory inventory;
    protected bool attacking;
    [SerializeField] protected PlayerLevelSystem playerLevelSystem;
    void Start()
    {
        //playerLevelSystem = GameObject.Find("Player").GetComponent<PlayerLevelSystem>();
    }
    
    void Update()
    {
        
    }

    protected virtual IEnumerator Attack()
    {
        yield return null;
    }

    protected bool CanAttack() => Input.GetKey(KeyCode.Mouse0) && !attacking && !inventory.inventoryActive;

    public float Damage() => weaponData.damage * playerLevelSystem.PlayerDamageMultiplier;
}
