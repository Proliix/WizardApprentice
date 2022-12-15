using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 1f;
    public float movementSpeed = 1f;
    public float damage = 1f;
    public float attackSpeed = 1f;
    public float critChance = 0.05f;
    public float critDamage = 1.5f;
    [Header("Projectiles")]
    public float projectileSize = 0f;
    public float projectileSpeed = 0f;
    public int projectileAmount = 0;
    public float GetCrit(float damageValue)
    {
        float returnValue = damageValue;
        int randomNum = Random.Range(1, 101);

        if (randomNum <= critChance * 100)
            returnValue = damageValue * critDamage;


        return Mathf.RoundToInt(returnValue);
    }

    public int GetDamage(float damageValue)
    {
        return Mathf.RoundToInt(damageValue * damage);
    }

    public float GetAttackSpeed(float AttackSpeedValue)
    {
        return AttackSpeedValue / attackSpeed;
    }

    public void GiveStats(Reward newStats)
    {
        health += newStats.maxHealth;
        movementSpeed += newStats.movementSpeed;
        damage += newStats.damage;
        attackSpeed += newStats.attackSpeed;
        critChance += newStats.critChance;
        critDamage += newStats.critDamage;
        projectileSize += newStats.projectileSize;
        projectileSpeed += newStats.projectileSpeed;
        projectileAmount += newStats.projectileAmount;

        if (newStats.addHealh > 0)
        {
            gameObject.GetComponent<Health>()?.HealPercentageOf(newStats.addHealh);
        }
    }
}
