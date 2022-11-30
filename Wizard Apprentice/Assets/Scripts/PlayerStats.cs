using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 0f;
    public float movementSpeed = 0f;
    public float damage = 0f;
    public float attackSpeed = 0f;
    [Header("Projectiles")]
    public float projectileSize = 0f;
    public float projectileSpeed = 0f;
    public int projectileAmount = 0;
    [Header("Dashes")]
    public int dashCharges = 0;
    public float dashCooldown = 0f;

    float currentHealth = 0f;
    float currentMovementSPeed = 0f;
    float currentDamage = 0F;
    float currentAttackSpeed = 0f;
    float currentProjectileSize = 0f;
    float currentProjectileSpeed = 0f;
    int currentProjectileAmount = 0;
    void Start()
    {
        UpdateCurrentStats();
    }

    public void UpdateCurrentStats()
    {
        currentHealth = health;
        currentDamage = damage;
        currentMovementSPeed = movementSpeed;
        currentAttackSpeed = attackSpeed;
        currentProjectileSize = projectileSize;
        currentProjectileSpeed = projectileSpeed;
        currentProjectileAmount = projectileAmount;
    }

    public void ResetToCurrentStats()
    {
        health = currentHealth ;
        damage = currentDamage = damage;
        movementSpeed = currentMovementSPeed;
        attackSpeed = currentAttackSpeed;
        projectileSize = currentProjectileSize;
        projectileSpeed = currentProjectileSpeed;
        projectileAmount = currentProjectileAmount;
    }

}
