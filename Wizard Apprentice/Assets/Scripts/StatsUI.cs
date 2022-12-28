using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class StatsUI : MonoBehaviour
{

    [Header("TMP references ")]
    public TextMeshProUGUI damageTMP;
    public TextMeshProUGUI attackSpeedTMP;
    public TextMeshProUGUI critChanceTMP;
    public TextMeshProUGUI critDmgMultiplierTMP;
    public TextMeshProUGUI moveSpeedTMP;
    public TextMeshProUGUI bulletSpeedTMP;
    public TextMeshProUGUI bulletSizeTMP;
    public TextMeshProUGUI healthTMP;

    [Header("Player Stats")]
    [SerializeField] float damage = 1;
    [SerializeField] float attackSpeed = 1;
    [SerializeField] float critChance = 10;
    [SerializeField] float critDmgMultiplier;
    [SerializeField] float moveSpeed = 1;
    [SerializeField] float bulletSpeed = 1;
    [SerializeField] float bulletSize = 1;
    [SerializeField] float currentHP = 100;
    [SerializeField] float maxHP = 100;

    [Header("Misc")]
    [SerializeField] GameObject PauseScreen;

    float timer = 0;
    bool activeIngame;
    PlayerStats playerStats;
    Health playerHealth;
    GameObject holderChild;

    void Start()
    {
        timer = 10;
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerHealth = GameObject.FindGameObjectWithTag("Player").GetComponent<Health>();
        activeIngame = PlayerPrefs.GetInt("StatsInGame") > 0 ? true : false;
        holderChild = gameObject.transform.GetChild(0).gameObject;
        holderChild.SetActive(activeIngame);
        healthTMP.gameObject.SetActive(activeIngame);
        UpdateStats();
    }

    void Update()
    {

        if (activeIngame)
        {
            if (!healthTMP.gameObject.activeSelf)
                healthTMP.gameObject.SetActive(true);

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
            healthTMP.gameObject.SetActive(PauseScreen.activeSelf);
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
        UpdateHP();
    }

    public void UpdateHP()
    {
        currentHP = playerHealth.GetHP();
        maxHP = playerHealth.GetMaxHP();
    }

    public void UpdateShowStatus()
    {
        activeIngame = PlayerPrefs.GetInt("StatsInGame") > 0 ? true : false;
    }
    private void UpdateStats()
    {

        string dmgText = "<color=red>DG : " + Mathf.RoundToInt(damage * 100) + "%</color>";
        damageTMP.SetText(dmgText);


        string attackSpeedText = "<color=blue>AS : " + Mathf.RoundToInt(attackSpeed * 100) + "%</color>";
        attackSpeedTMP.SetText(attackSpeedText);


        string critMultText = "<color=#F334DA>CD : " + Mathf.RoundToInt(critDmgMultiplier * 100) + "%</color>";
        critDmgMultiplierTMP.SetText(critMultText);

        string critChanceText = "<color=purple>C% : " + Mathf.RoundToInt(critChance * 100) + "%</color>";
        critChanceTMP.SetText(critChanceText);
        //critChanceTMP.color = Color.

        string moveSpeedText = "<color=yellow>MS : " + Mathf.RoundToInt(moveSpeed * 100) + "%</color>";
        moveSpeedTMP.SetText(moveSpeedText);
        moveSpeedTMP.color = Color.yellow;

        //string bulletSpeedText = "Bullet Speed : " + bulletSpeed.ToString("F2");
        //bulletSpeedTMP.SetText(bulletSpeedText);

        //string bulletSizeText = "Bullet Size : " + bulletSize.ToString("F2");
        //bulletSizeTMP.SetText(bulletSizeText);

        string healthText = Mathf.RoundToInt(currentHP) + "/" + Mathf.RoundToInt(maxHP);
        healthTMP.SetText(healthText);
    }

}
