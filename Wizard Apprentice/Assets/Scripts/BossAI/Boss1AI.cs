using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss1AI : MonoBehaviour
{
    [Header("Find GameObjects")] 
    [SerializeField] private Transform target;
    [SerializeField] BulletHandler bulletHandler;
    [SerializeField] Health health;
    [Header("Boss Variables")]
    [SerializeField] float timer;
    [SerializeField] bool hasTriggeredBasic = false;
    [SerializeField] float basicsUntilSpecial = 10;
    [SerializeField] float currentBasics;
    [SerializeField] float timeUntilBossStart = 3;
    [SerializeField] float attackSpeedBasic = 0.5f;
    [SerializeField] private float bossHP;
    [SerializeField] private float bossMaxHP;
    [Header("Phases")]
    [SerializeField] bool phase1;
    [SerializeField] bool phase2;
    [SerializeField] bool phase3;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = GetComponent<Health>();
        timer = -timeUntilBossStart;
        attackSpeedBasic = 0.5f;
        phase1 = true;
    }

    void Update()
    {
        timer += Time.deltaTime;
        bossHP = health.GetHP();
        bossMaxHP = health.GetMaxHP();

        if (currentBasics >= basicsUntilSpecial && timer >= attackSpeedBasic)
        {
            BossAttackSpecial();
            currentBasics = 0;
        }
        else if (timer >= attackSpeedBasic && hasTriggeredBasic == false)
        {
            hasTriggeredBasic = true;
            currentBasics++;
            BossAttackBasic();
        }


        //Start phase 2
        if (bossHP < bossMaxHP * 0.666f && phase1)
        {
            phase1 = false;
            phase2 = true;
            attackSpeedBasic = 0.35f;
        }

        //Start phase 3
        if (bossHP < bossMaxHP * 0.333f && phase2)
        {
            attackSpeedBasic = 0.25f;
            phase2 = false;
            phase3 = true;
        }
        
    }

   void BossAttackBasic()
    {
        bulletHandler.GetCircleShot(Random.Range(9,12), gameObject, false);
        timer = 0;
        hasTriggeredBasic = false;
      
    }

  void BossAttackSpecial()
    {
        timer = 0;
        bulletHandler.GetCircleShot(30, gameObject, false);

    }

}
