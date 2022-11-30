using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
     
     BulletHandler bulletHandler;
    Health health;

    [SerializeField] float timer;


    [Header("Boss Variables")]
    [SerializeField] float basicsUntilSpecial = 10;
    [SerializeField] float currentBasics;
    [SerializeField] float attackSpeedBasic = 0.5f;
    [SerializeField] float basicDamage;
    [SerializeField] float specialDamage;
    [SerializeField] float basicBulletSize;
    [SerializeField] float specialBulletSize;
    [SerializeField] float bulletSpeed;

    [SerializeField] float timeUntilBossStart = 3;

    [Header("HP")]
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;

    [Header("Phases")]
    [SerializeField] bool phase1;
    [SerializeField] bool phase2;
    [SerializeField] bool phase3;

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = GetComponent<Health>();

        timer = -timeUntilBossStart;
        
        phase1 = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        HP = health.GetHP();
        maxHP = health.GetMaxHP();

        if (currentBasics >= basicsUntilSpecial && timer >= attackSpeedBasic)
        {
            BossAttackSpecial();
            currentBasics = 0;
        }
        else if (timer >= attackSpeedBasic)
        {

            BossAttackBasic();
        }

        //Start phase 2
        if (HP < maxHP * 0.666f && phase1)
        {
            phase1 = false;
            phase2 = true;
            attackSpeedBasic = 0.35f;
        }

        //Start phase 3
        if (HP < maxHP * 0.333f && phase2)
        {
            attackSpeedBasic = 0.25f;
            phase2 = false;
            phase3 = true;
        }
        
    }

   void BossAttackBasic()
    {
        
        currentBasics++;
        //bulletHandler.GetCircleShot(Random.Range(9,12), gameObject, false);
        bulletHandler.GetCircleShot(Random.Range(9, 12), gameObject, false, 1, basicDamage, basicBulletSize, bulletSpeed);
       
        timer = 0;
      
    }

  void BossAttackSpecial()
    {
        timer = 0;
        bulletHandler.GetCircleShot(30, gameObject, false, 1, specialDamage, specialBulletSize, bulletSpeed);

    }

}
