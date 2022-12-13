using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/Reward")]
public class Reward : ScriptableObject
{
    [Header("UI")]
    public string Title = "This is the title";
    [TextArea(2, 10)]
    public string EffectText = "This is the flavour text";
    [Header("Player Stats")]
    public float maxHealth = 0f;
    public float movementSpeed = 0f;
    public float damage = 0f;
    public float attackSpeed = 0f;
    public float critChance = 0;
    public float critDamage = 0;
    [Header("This will only heal, maxHealth will give maxhp and heal")]
    public float addHealh;
    [Header("Projectiles")]
    public float projectileSize = 0f;
    public float projectileSpeed = 0f;
    public int projectileAmount = 0;

    public string GetDesription()
    {
        string CritChangeStr = "" + (critChance * 100) + "%";
        string CritDamageStr = "" + (critDamage * 100) + "%";


        return string.Format(EffectText, maxHealth, movementSpeed, damage, attackSpeed, CritChangeStr, CritDamageStr, addHealh, projectileSize, projectileSpeed, projectileAmount);
    }
}
