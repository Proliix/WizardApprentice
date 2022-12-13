using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    public float health = 0f;
    public float movementSpeed = 0f;
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
