using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1f;
    public float movementSpeed = 0f;
    public float damage = 1f;
    public float attackSpeed = 1f;
    public float critChance = 0.05f;
    public float critDamage = 1.5f;
    [Header("Projectiles")]
    public float projectileSize = 0f;
    public float projectileSpeed = 0f;
    public int projectileAmount = 0;

    float currentHealth = 1f;
    float currentMovementSPeed = 1f;
    float currentDamage = 1F;
    float currentAttackSpeed = 1f;
    float currentProjectileSize = 1f;
    float currentProjectileSpeed = 1f;
    int currentProjectileAmount = 1;
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
        health = currentHealth;
        damage = currentDamage;
        movementSpeed = currentMovementSPeed;
        attackSpeed = currentAttackSpeed;
        projectileSize = currentProjectileSize;
        projectileSpeed = currentProjectileSpeed;
        projectileAmount = currentProjectileAmount;
    }

    public float GetCrit(float damageValue)
    {
        float returnValue = damageValue;

        if (Random.Range(0, 101) <= critChance * 100)
            returnValue = damageValue * critDamage;

        return Mathf.FloorToInt(returnValue);
    }

    public int GetDamage(float damageValue)
    {
        return Mathf.FloorToInt(damageValue * damage);
    }

    public float GetAttackSpeed(float AttackSpeedValue)
    {
        return AttackSpeedValue / attackSpeed;
    }

}
