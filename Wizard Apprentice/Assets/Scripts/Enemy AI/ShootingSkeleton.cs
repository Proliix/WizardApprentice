using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingSkeleton : MonoBehaviour, IStunnable
{


    private Rigidbody2D rb2d;

    [Header("Attack Variables")]
    [SerializeField] float attackDelay;
    [SerializeField] float patternDelay;
    [SerializeField] float damage;
    [SerializeField] float bulletSize;
    [SerializeField] float bulletSpeed;
    [SerializeField] float numberOfShotsInPattern = 4;
    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] bool isMoving;
    [SerializeField] bool canShoot;

    [Header("")]
    [SerializeField] float timer;
    [SerializeField] float moveTimer;
    [SerializeField] float moveDelay;
    [SerializeField] float moveSpeed = 1.5f;


    [SerializeField] GameObject target;

    Health health;
    Animator anim;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    bool stunned = false;

    void Start()
    {
        canShoot = false;
        isMoving = false;
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = gameObject.GetComponent<Health>();
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = gameObject.GetComponent<Animator>();
        //MoveEnemy();
        target = GameObject.FindWithTag("Player");
        float rNum= Random.Range(0, 1f);
        timer -= rNum;
        moveTimer -= rNum;
    }

    void Update()
    {
        if (!stunned && enemyManager.enemiesActive)
        {
            timer += Time.deltaTime;
            moveTimer += Time.deltaTime;

            anim.SetFloat("DirX", rb2d.velocity.normalized.x);
            anim.SetFloat("DirY", rb2d.velocity.normalized.y);

            if (timer >= patternDelay && canShoot)
            {

                timer -= patternDelay;

                StartCoroutine(AttackPattern());
            }

            if (moveTimer >= moveDelay && isMoving == false)
            {
                moveTimer -= moveDelay;
                rb2d.velocity = new Vector2(0, 0);
                StartCoroutine(MoveEnemy());
            }
        }

    }

    private void MoveEnemy1()
    {
        rb2d.velocity = new Vector2(Random.Range(-moveSpeed, moveSpeed), Random.Range(-moveSpeed, moveSpeed));
    }

    IEnumerator MoveEnemy()
    {
        isMoving = true;
        rb2d.velocity = new Vector2(Random.Range(-moveSpeed, moveSpeed), Random.Range(-moveSpeed, moveSpeed));
        yield return new WaitForSeconds(2);
        rb2d.velocity = new Vector2(0, 0);
        canShoot = true;
        yield return new WaitForSeconds(2);
        isMoving = false;
        yield return null;
    }


    private void BasicAttack()
    {
        bulletHandler.GetBullet(gameObject.transform.position, (target.transform.position - gameObject.transform.position).normalized, false, damage, bulletSize, bulletSpeed);

    }


    IEnumerator AttackPattern()
    {
        for (int i = 0; i < numberOfShotsInPattern; i++)
        {
            BasicAttack();
            yield return new WaitForSeconds(timeBetweenShots);

        }

        yield return null;
    }


    public void GetStunned(float stunDuration = 0.25F)
    {

        if (stunned)
            StopCoroutine(IsStunned(stunDuration));

        StartCoroutine(IsStunned(stunDuration));
    }

    public IEnumerator IsStunned(float stunDuration = 0.25F)
    {
        stunned = true;
        yield return new WaitForSeconds(stunDuration);
        stunned = false;
    }

}
