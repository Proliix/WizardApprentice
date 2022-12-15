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
    [Tooltip("It is okay to be null then it just will have no image")]
    public Sprite Image;
    [Header("Player Stats")]
    [Tooltip("{0}")] public float maxHealth = 0f;
    [Tooltip("{1}")] public float movementSpeed = 0f;
    [Tooltip("{2}")] public float damage = 0f;
    [Tooltip("{3}")] public float attackSpeed = 0f;
    [Tooltip("{4}")] public float critChance = 0;
    [Tooltip("{5}")] public float critDamage = 0;
    [Header("This will only heal,it heals with maxHP / addHealth")]
    [Tooltip("{6}")] public float addHealh;
    [Header("Projectiles")]
    [Tooltip("{7}")] public float projectileSize = 0f;
    [Tooltip("{8}")] public float projectileSpeed = 0f;
    [Tooltip("{9}")] public int projectileAmount = 0;

    public string GetDesription()
    {
        string CritChangeStr = "" + (critChance * 100) + "%";
        string CritDamageStr = "" + (critDamage * 100) + "%";
        float maxHp = GameObject.FindWithTag("Player").GetComponent<Health>().GetMaxHP();
        float newAddHealth = maxHp / addHealh;
        float startHP = GameObject.FindWithTag("Player").GetComponent<Health>().GetStartMaxHp();
        float newMaxHP = startHP * maxHealth;


        return string.Format(EffectText, newMaxHP, movementSpeed, damage, attackSpeed, CritChangeStr, CritDamageStr, newAddHealth, projectileSize, projectileSpeed, projectileAmount);
    }
}
