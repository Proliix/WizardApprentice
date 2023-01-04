using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [Range(0.55f, 100)]
    public float health = 1f;
    [Range(0.55f, 100)]
    public float movementSpeed = 1f;
    [Range(0.55f, 100)]
    public float damage = 1f;
    [Range(0.55f, 100)]
    public float attackSpeed = 1f;
    public float critChance = 0.05f;
    [Range(0.55f, 100)]
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
        {
            returnValue = damageValue * critDamage;
            Mathf.RoundToInt(returnValue);
        }


        return returnValue;
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
        health = Mathf.Clamp(health + newStats.maxHealth, 0.1f, 200);
        movementSpeed = Mathf.Clamp(movementSpeed + newStats.movementSpeed, 0.1f, 200);
        damage = Mathf.Clamp(damage + newStats.damage, 0.1f, 200);
        attackSpeed = Mathf.Clamp(attackSpeed + newStats.attackSpeed, 0.1f, 200);
        critChance += newStats.critChance;
        critDamage = Mathf.Clamp(critDamage + newStats.critDamage, 0.1f, 200);
        projectileSize += newStats.projectileSize;
        projectileSpeed += newStats.projectileSpeed;
        projectileAmount += newStats.projectileAmount;


        if (newStats.addHealh > 0)
        {
            gameObject.GetComponent<Health>()?.HealPercentageOf(newStats.addHealh);
        }
        else if (newStats.addHealh < 0)
        {
            Health hp = gameObject.GetComponent<Health>();

            if ((hp.GetHP() - Mathf.Abs((hp.GetHP() * (0.1f * newStats.addHealh)))) <= 1)
            {
                hp?.RemoveHealth(Mathf.Abs(hp.GetHP() - 1));
            }
            else
            {
                hp?.RemoveHealth(Mathf.Abs(hp.GetHP() * (0.1f * newStats.addHealh)));
            }
        }
    }
}
