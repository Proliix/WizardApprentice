using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeBossAI : MonoBehaviour
{
     
    BulletHandler bulletHandler;
    Health health;
    RoomManager roomManager;
    Rigidbody2D rb2d;

    Vector2 movement; 

    [SerializeField] float timer;
    [SerializeField] GameObject target;

    [SerializeField] float moveSpeed = 1;

    [Header("Boss Basic Attack Variables")]
    [SerializeField] float basicsUntilSpecial = 10;
    [SerializeField] float currentBasics;
    [SerializeField] float attackSpeedBasic = 0.5f;
    [SerializeField] float basicDamage;
    [SerializeField] float basicBulletSize;
    [SerializeField] float basicBulletSpeed;
    [SerializeField] int basicMinAmount;
    [SerializeField] int basicMaxAmount;

    [Header("Boss Special Attack Varbiables")]
    [SerializeField] float specialDamage;
    [SerializeField] float specialBulletSize = 5;
    [SerializeField] float specialBulletSpeed = 8;

    [Header("Giga Attack Variables")]
    [SerializeField] float gigaBulletDamage;
    [SerializeField] float gigaBulletSize;
    [SerializeField] float gigaBulletSpeed;

    [SerializeField] float timeUntilBossStart = 3;

    [Header("HP")]
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] bool isAlive = true;

    [Header("Phases")]
    [SerializeField] bool phase1;
    [SerializeField] bool phase2;
    [SerializeField] bool phase3;

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        roomManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoomManager>();
        target = GameObject.FindGameObjectWithTag("Player");
        health = GetComponent<Health>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        timer = -timeUntilBossStart;

        isAlive = true;
        phase1 = true;
    }

    void Update()
    {
        if (isAlive == true)
        {


            if (Input.GetKeyDown(KeyCode.J))
            {
                MoveEnemy();
            }
            

            

        HP = health.GetHP();
        maxHP = health.GetMaxHP();
        
            if (HP <= 0)
            {
                BossDead();
            }

        timer += Time.deltaTime;

        if (currentBasics >= basicsUntilSpecial && timer >= attackSpeedBasic)
        {
            BossAttackSpecial();
            currentBasics = 0;
        StartCoroutine(GigaAttack());
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
            basicMinAmount = 7;
            basicMaxAmount = 10;
         //   attackSpeedBasic = 0.35f;

        }

        //Start phase 3
        if (HP < maxHP * 0.333f && phase2)
        {
          //  attackSpeedBasic = 0.25f;
            phase2 = false;
            phase3 = true;
            basicMinAmount = 9;
            basicMaxAmount = 12;
        }


        }
    }

    void MoveEnemy()
    {
        movement = (target.transform.position - gameObject.transform.position).normalized;
        rb2d.velocity = movement * moveSpeed;
    }

   void BossAttackBasic()
    {
        
        currentBasics++;
        bulletHandler.GetCircleShot(Random.Range(basicMinAmount, basicMaxAmount), gameObject, false, 1, basicDamage, basicBulletSize, basicBulletSpeed);
       
        timer = 0;
      
    }

  void BossAttackSpecial()
    {
        timer = 0;
        bulletHandler.GetCircleShot(50, gameObject, false, 1, specialDamage, specialBulletSize, specialBulletSpeed);
    }

  IEnumerator GigaAttack()
    {
        Debug.Log("start of gigaAttack");
        yield return new WaitForSeconds(2f);
        //Shoots a bullet from the gameObject towards the target(player)
        bulletHandler.GetBullet(gameObject.transform.position, target.transform.position - gameObject.transform.position, false, gigaBulletDamage, gigaBulletSize, gigaBulletSpeed);
        Debug.Log("end of gigaAttack");
        yield return null;
    }

    private void BossDead()
    {

        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().FullHeal();

        isAlive = false;
        bulletHandler.ResetAll();
        roomManager.RemoveAllEnemies();

    }


}