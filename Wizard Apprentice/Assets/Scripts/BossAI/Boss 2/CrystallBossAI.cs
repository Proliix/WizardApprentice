using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CrystallBossAI : MonoBehaviour
{

  [SerializeField]  Light2D[] lightObjects;
    RoomManager roomManager;
    Boss2PiecesAI[] boss2PiecesAI;
    Health health;
    BulletHandler bulletHandler;

    [SerializeField] float patternStartUpDelay;
    [SerializeField] float randomStartUpDelay;
    [SerializeField] float lightIntensityDelaySpeed = 1;


    [Header("HP")]
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;
    [SerializeField] bool isAlive = true;

    [Header("Attack Variables")]
    [SerializeField] float damage = 10;
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

    [Header("Spawn Variables")]
    [SerializeField] GameObject spawningEnemy;
    [SerializeField] float spawnEnemyTimer;
    [SerializeField] float spawnEnemyDelay;
    [SerializeField] int spawnAmount;

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
    [SerializeField] PolygonCollider2D bossHurtbox;

    public Vector3[] spawnPoints;

    bool hasClearedRoom = false;
    

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = gameObject.GetComponent<Health>();
        roomManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoomManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();

     

        //Vectors used as spawnpoints for ads
        spawnPoints = new Vector3[4];
        spawnPoints[0] = new Vector3(-3, 0, 0);
        spawnPoints[1] = new Vector3(-3, 22, 0);
        spawnPoints[2] = new Vector3(28, 0, 0);
        spawnPoints[3] = new Vector3(28, 22, 0);


        phase1Active = true;
        isAlive = true;

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
        if (isAlive == true)
        {


            HP = health.GetHP();
            maxHP = health.GetMaxHP();

            patternTimer += Time.deltaTime;
            attackTimer += Time.deltaTime;
            randomAttackTimer += Time.deltaTime;
            spawnEnemyTimer += Time.deltaTime;


            if (spawnEnemyTimer >= spawnEnemyDelay)
            {
                StartCoroutine(SpawnEnemy());
                spawnEnemyTimer -= spawnEnemyDelay;
            }

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

            if (HP <= 0 && isAlive == true && !hasClearedRoom)
            {
                hasClearedRoom = true;
                BossDead();
            }

        }

        if (isAlive == false && lightObjects[7].intensity > 0)
        {
            ChangeLightIntensity();

        }

    }

    void BasicAttack()
    {

        for (int i = 0; i < targets.Length; i++)
        {
            bulletHandler.GetBullet(transform.position, transform.position - targets[i].transform.position, false, damage, 0.5f, bulletSpeed);
        }
    }

    void RandomAttack()
    {
        bulletHandler.GetCircleShot(randomBulletAmount, gameObject, false, Random.Range(0, 360f), damage, randomBulletSize, randomBulletSpeed);

    }

    void Phase1()
    {
       
        spawnAmount = 2;
        rotationSpeed = 40;
        StartCoroutine(AttackPattern());
        spriteRenderer.sprite = Phase1Sprite;
    }

    void Phase2()
    {
        
        spawnAmount = 3;
        rotationSpeed = 65;
        phase1Active = false;
        phase2Active = true;
        StartCoroutine(AttackPattern());
        spriteRenderer.sprite = Phase2Sprite;

    }

    void Phase3()
    {
        
        spawnAmount = 4;
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

    IEnumerator SpawnEnemy()
    {
        for (int i = 0; i < spawnAmount; i++)
        {
            GameObject enemyObject = Instantiate(spawningEnemy, spawnPoints[Random.Range(0, 4)], Quaternion.identity);
            bulletHandler.gameObject.GetComponent<RoomManager>().AddEnemy(enemyObject);
            yield return new WaitForSeconds(Random.Range(0.25f, 0.5f));
        }
        yield return new WaitForSeconds(2f);

        yield return null;

    }
    private void BossDead()
    {
        Camera.main.GetComponent<CameraMovement>().GetScreenShake(2,1);
        StopAllCoroutines();
        GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().FullHeal();
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
        bossHurtbox.enabled = false;
        isAlive = false;
        bulletHandler.ResetAll();
        roomManager.RemoveAllEnemies();
    }

    private void ChangeLightIntensity()
    {
        

            for (int i = 0; i < lightObjects.Length; i++)
            {
                lightObjects[i].intensity = Mathf.MoveTowards(lightObjects[i].intensity, 0, lightIntensityDelaySpeed * Time.deltaTime);
            
            }
        
    
    }
   
 
    
}

