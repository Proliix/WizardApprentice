using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI2 : MonoBehaviour
{

    Boss2PiecesAI[] boss2PiecesAI;
    Health health;
    BulletHandler bulletHandler;

    [SerializeField] float patternStartUpDelay;
    [SerializeField] float randomStartUpDelay;

    [Header("HP")]
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;

    [Header("Attack Variables")]
    [SerializeField] float attackDelay = 0.04f;
    [SerializeField] float patternDelay = 3;
    [SerializeField] float pausetime = 0.5f;
    [SerializeField] float bulletSpeed = 3.5f;
    [SerializeField] float rotationSpeed = 80;

    [Header("RandomAttack Variables")]
    [SerializeField] int randomBulletAmount = 1;
    [SerializeField] float randomBulletSize = 1.5f;
    [SerializeField] float randomBulletSpeed = 4;

    [Header("Timers")]
    [SerializeField] float attackTimer;
    [SerializeField] float patternTimer;
    [SerializeField] float randomAttackTimer;

    [Header("Phases")]
    [SerializeField] bool phase1Active;
    [SerializeField] bool phase2Active;
    [SerializeField] bool phase3Active;

    [Header("Sprites")]
    [SerializeField] Sprite Phase1Sprite;
    [SerializeField] Sprite Phase2Sprite;
    [SerializeField] Sprite Phase3Sprite;

    [Header("")]
    [SerializeField] GameObject player;
    [SerializeField] GameObject[] targets;
    [SerializeField] SpriteRenderer spriteRenderer; 

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = gameObject.GetComponent<Health>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        phase1Active = true;

        attackTimer -= patternStartUpDelay;
        randomAttackTimer -= randomStartUpDelay;

        boss2PiecesAI = new Boss2PiecesAI[targets.Length];
        for (int i = 0; i < targets.Length; i++)
        {
            boss2PiecesAI[i] = targets[i].GetComponent<Boss2PiecesAI>();
        }

        for (int i = 0; i < targets.Length; i++)
        {
            boss2PiecesAI[i].RotationSpeed = rotationSpeed;
        }

    }

    void Update()
    {

        HP = health.GetHP();
        maxHP = health.GetMaxHP();

        attackTimer += Time.deltaTime;
        patternTimer += Time.deltaTime;
        randomAttackTimer += Time.deltaTime;

        if (HP < maxHP * 0.666f && HP > maxHP * 0.333f)
        {
            phase2Active = true;
        }

        if (HP < maxHP * 0.333f)
        {
            phase3Active = true;
        }

        if (attackTimer >= attackDelay)
        {
            BasicAttack();
            attackTimer -= attackDelay;
        }

        if (randomAttackTimer >= 0.78f)
        {
            RandomAttack();
            randomAttackTimer -= 0.78f;
        }

        //Attack phases (based on current boss hp)
        if (patternTimer >= patternDelay)
        {
            //Phase 1 from 100% - 66% hp
            if (phase1Active)
            {
                Phase1();
            }

            //Phase 2 from 66% - 33% hp
            if (HP < maxHP * 0.666f && phase2Active)
            {
                Phase2();
            }

            //Phase 3 from 33% - 0% hp
            if (HP < maxHP * 0.333f && phase3Active)
            {
                Phase3();
            }
            patternTimer -= patternDelay;
        }
    }

    void BasicAttack()
    {

        for (int i = 0; i < targets.Length; i++)
        {
            bulletHandler.GetBullet(transform.position, transform.position - targets[i].transform.position, false, 10, 0.5f, bulletSpeed);
        }
    }

    void RandomAttack()
    {
        bulletHandler.GetCircleShot(randomBulletAmount, gameObject, false, Random.Range(0, 360f), 10, randomBulletSize, randomBulletSpeed);

    }

    void Phase1()
    {
        rotationSpeed = 40;
        StartCoroutine(AttackPattern());
        spriteRenderer.sprite = Phase1Sprite;
    }

    void Phase2()
    {
        rotationSpeed = 65;
        phase1Active = false;
        phase2Active = true;
        StartCoroutine(AttackPattern());
        spriteRenderer.sprite = Phase2Sprite;

    }

    void Phase3()
    {
        rotationSpeed = 90;
        StartCoroutine(AttackPattern());
        phase2Active = false;
        phase3Active = true;
        spriteRenderer.sprite = Phase3Sprite;

    }

    IEnumerator AttackPattern()
    {
        patternDelay = Random.Range(pausetime + 0.1f, 5f);

        //Changes the direction 
        for (int i = 0; i < boss2PiecesAI.Length; i++)
        {
            boss2PiecesAI[i].RotationSpeed = rotationSpeed;
        }

        yield return new WaitForSeconds(pausetime);

        //Changes into the opposite direction 
        for (int i = 0; i < boss2PiecesAI.Length; i++)
        {
            boss2PiecesAI[i].RotationSpeed = -rotationSpeed;
        }

        yield return new WaitForSeconds(pausetime);

        yield return null;
    }




}

