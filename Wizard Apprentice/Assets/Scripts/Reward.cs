using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/Reward")]
public class Reward : ScriptableObject
{
    [Header("UI")]
    public string Title = "This is the title";
    [TextArea(2,10)]
    public string EffectText = "This is the flavour text";
    [Header("Player Stats")]
    public float health = 0f;
    public float movementSpeed = 0f;
    public float damage = 0f;
    public float attackSpeed = 0f;
    [Header("Projectiles")]
    public float projectileSize = 0f;
    public float projectileSpeed = 0f;
    public int projectileAmount = 0;

}
