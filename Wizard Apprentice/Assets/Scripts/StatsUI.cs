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

    [Header("Misc")]
    [SerializeField] GameObject PauseScreen;

    float timer = 0;
    bool activeIngame;
    GameObject holderChild;

    void Start()
    {
        timer = 10;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        activeIngame = PlayerPrefs.GetInt("StatInGame") > 0 ? true : false;
        holderChild = gameObject.transform.GetChild(0).gameObject;
        holderChild.SetActive(activeIngame);
        UpdateStats();
    }

    void Update()
    {
        if (activeIngame)
        {
            timer += Time.deltaTime;
            if (timer >= 0.05f)
            {
                timer -= 0.05f;
                CheckStats();
            }
            UpdateStats();
        }
        else
        {
            holderChild.SetActive(PauseScreen.activeSelf);
            if (PauseScreen.activeSelf)
            {
                CheckStats();
                UpdateStats();
            }
        }
    }


    private void CheckStats()
    {
        damage = playerStats.damage;
        attackSpeed = playerStats.attackSpeed;
        critChance = playerStats.critChance;
        critDmgMultiplier = playerStats.critDamage;
        moveSpeed = playerStats.movementSpeed;
        bulletSpeed = playerStats.projectileSize;
        bulletSize = playerStats.projectileSize;
    }

    private void UpdateStats()
    {
        string dmgText = "DMG : " + Mathf.RoundToInt(damage);
        damageTMP.SetText(dmgText);

        string attackSpeedText = "AS : " + Mathf.RoundToInt(attackSpeed);
        attackSpeedTMP.SetText(attackSpeedText);

        string critMultText = "Crit DMG : " + Mathf.RoundToInt(critDmgMultiplier * 100) + "%";
        critDmgMultiplierTMP.SetText(critMultText);

        string critChanceText = "Crit chance : " + Mathf.RoundToInt(critChance * 100) + "%";
        critChanceTMP.SetText(critChanceText);

        string moveSpeedText = "MS : " + Mathf.RoundToInt(moveSpeed);
        moveSpeedTMP.SetText(moveSpeedText);

        string bulletSpeedText = "Bullet Speed : " + Mathf.RoundToInt(bulletSpeed);
        bulletSpeedTMP.SetText(bulletSpeedText);

        string bulletSizeText = "Bullet Size : " + Mathf.RoundToInt(bulletSize);
        bulletSizeTMP.SetText(bulletSizeText);

    }

}
