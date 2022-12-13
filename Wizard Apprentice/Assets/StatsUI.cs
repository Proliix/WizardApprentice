using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StatsUI : MonoBehaviour
{

    PlayerStats playerStats;
    [Header("TMP references ")]
    public TextMeshProUGUI damageTMP;
    public TextMeshProUGUI attackSpeedTMP;
    public TextMeshProUGUI critChanceTMP;
    public TextMeshProUGUI critDmgMultiplierTMP;
    public TextMeshProUGUI moveSpeedTMP;
    public TextMeshProUGUI bulletSpeedTMP;
    public TextMeshProUGUI bulletSizeTMP;

    [Header("Player Stats")]
    [SerializeField] float damage = 1;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float critChance = 10;
    [SerializeField] float critDmgMultiplier;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float bulletSpeed = 1;
    [SerializeField] float bulletSize = 1;

    [SerializeField] float timer = 0;


    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= 0.05f)
        {
            timer -= 0.05f;
            CheckStats();
        }

        UpdateStats();
    }

    private void CheckStats()
    {
        damage = playerStats.damage;
        attackSpeed = playerStats.attackSpeed;
        //crit = playerStats.crit;
        moveSpeed = playerStats.movementSpeed;
        bulletSpeed = playerStats.projectileSize;
        bulletSize = playerStats.projectileSize;
    }

    private void UpdateStats()
    {
        string dmgText = "DMG : " + damage;
        damageTMP.SetText(dmgText);

        string attackSpeedText = "AS : " + attackSpeed;
        attackSpeedTMP.SetText(attackSpeedText);

        string critMultText = "Crit DMG % : ";
        critDmgMultiplierTMP.SetText(critMultText);

        string critChanceText = "Crit chance : ";
        critChanceTMP.SetText(critChanceText);

        string moveSpeedText = "MS : " + moveSpeed;
        moveSpeedTMP.SetText(moveSpeedText);

        string bulletSpeedText = "Bullet Speed : " + bulletSpeed;
        bulletSpeedTMP.SetText(bulletSpeedText);

        string bulletSizeText = "Bullet Size : " + bulletSize;
        bulletSizeTMP.SetText(bulletSizeText);

    }

}
