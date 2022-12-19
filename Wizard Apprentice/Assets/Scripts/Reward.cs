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
    [Header("Player Stats" + "\n" + "First num on tooltip is what will be added seccond is how it will change")]
    [Tooltip("{0}{10}")] public float maxHealth = 0f;
    [Tooltip("{1}{11}")] public float movementSpeed = 0f;
    [Tooltip("{2}{12}")] public float damage = 0f;
    [Tooltip("{3}{13}")] public float attackSpeed = 0f;
    [Tooltip("{4}{14}")] public float critChance = 0;
    [Tooltip("{5}{15}")] public float critDamage = 0;
    [Header("This will only heal,it heals with maxHP / addHealth")]
    [Tooltip("{6}{16}")] public float addHealh;
    [Header("Projectiles")]
    [Tooltip("{7}")] public float projectileSize = 0f;
    [Tooltip("{8}")] public float projectileSpeed = 0f;
    [Tooltip("{9}")] public int projectileAmount = 0;

    public string GetDesription()
    {
        PlayerStats stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        string CritChanceStr = "" + Mathf.RoundToInt(critChance * 100) + "%";
        string CritDamageStr = "" + Mathf.RoundToInt(critDamage * 100) + "%";
        string movementStr = "" + Mathf.RoundToInt(movementSpeed * 100) + "%";
        string DamageStr = "" + Mathf.RoundToInt(damage * 100) + "%";
        string attackSpeedStr = "" + Mathf.RoundToInt(attackSpeed * 100) + "%";
        float maxHp = GameObject.FindWithTag("Player").GetComponent<Health>().GetMaxHP();
        float currentHP = GameObject.FindWithTag("Player").GetComponent<Health>().GetHP();
        float startHP = GameObject.FindWithTag("Player").GetComponent<Health>().GetStartMaxHp();
        float newMaxHPStr = Mathf.RoundToInt(startHP * maxHealth);

        float restoreAmount = Mathf.RoundToInt(maxHp / addHealh);
        if (currentHP + restoreAmount >= 100)
            restoreAmount -= (currentHP + restoreAmount) - 100;
        string newAddHealthstr = "" + restoreAmount;


        string newMaxHPStrChange = "" + maxHp + " -> <color=green>" + ((maxHp + Mathf.RoundToInt(startHP * maxHealth))) + "</color>";
        string movementStrChange = "" + (stats.movementSpeed * 100) + "%" + " -> <color=green>" + ((stats.movementSpeed + movementSpeed) * 100) + "%" + "</color>";
        string damageStrChange = "" + (stats.damage * 100) + "%" + " -> <color=red>" + ((stats.damage + damage) * 100) + "%" + "</color>";
        string attackSpeedStrChange = "" + (stats.attackSpeed * 100) + "%" + " -> <color=blue>" + ((stats.attackSpeed + attackSpeed) * 100) + "%" + "</color>";
        string critChanceStrChange = "" + (stats.critChance * 100) + "%" + " -> <color=purple>" + ((stats.critChance + critChance) * 100) + "%" + "</color>";
        string critDamageStrChange = "" + (stats.critDamage * 100) + "%" + " -> <color=#F334DA>" + ((stats.critDamage + critDamage) * 100) + "%" + "</color>";
        string newAddHealthStrChange = "" + currentHP + "/" + maxHp + " -> <color=green>" + (currentHP + restoreAmount) + "/" + maxHp + "</color>";


        return string.Format(EffectText, newMaxHPStr, movementStr, DamageStr, attackSpeedStr, CritChanceStr, CritDamageStr, newAddHealthstr, projectileSize, projectileSpeed, projectileAmount, newMaxHPStrChange, movementStrChange, damageStrChange, attackSpeedStrChange, critChanceStrChange, critDamageStrChange, newAddHealthStrChange);
    }
}
