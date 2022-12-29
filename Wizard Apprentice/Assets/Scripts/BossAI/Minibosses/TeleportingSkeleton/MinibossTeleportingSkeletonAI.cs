using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossTeleportingSkeletonAI : MonoBehaviour
{
   [SerializeField] ParticleSystem teleportParticleEffect;

    BulletHandler bulletHandler;
    Rigidbody2D rb2d;
    public Vector3 movePos;
    [SerializeField] float indicatorRadius = 50;
    float indicatorTime = 0.5f;


    [Header("Attack Variables")]
    [SerializeField] float bulletSize;
    [SerializeField] float damage;
    [SerializeField] float bulletSpeed;
    [SerializeField] float numberOfShotsInPattern = 4;

    [Header("")]
    [SerializeField] float specialBulletSize;
    [SerializeField] float specialBulletSpeed;
    [SerializeField] float specialDamage;
    [SerializeField] int specialBulletAmount;

    [Header("Teleport Variables")]

    [SerializeField] float timeBetweenShots = 0.2f;
    [SerializeField] bool canMove;
    [SerializeField] float timer;
    [SerializeField] float moveDelay;

    private int lastNumber;
    Vector3 parentPos;


    [SerializeField] GameObject target;



    void Start()
    {
        canMove = true;
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        MoveEnemy();
        target = GameObject.FindWithTag("Player");
        parentPos = gameObject.transform.parent.position;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= moveDelay && canMove == true)
        {
            timer -= moveDelay;

            StartCoroutine(MoveEnemy());
        }

    }

    IEnumerator MoveEnemy()
    {


        canMove = false;
        int currentNumber = Random.Range(1, 5);

        while (currentNumber == lastNumber)
        {
            currentNumber = Random.Range(1, 5);

        }

        lastNumber = currentNumber;

        switch (currentNumber)
        {
            case 1:

                movePos = new Vector3(-5, 5, 0);

                break;

            case 2:
                movePos = new Vector3(5, 5, 0);

                break;

            case 3:
                movePos = new Vector3(5, -5, 0);

                break;

            case 4:
                movePos = new Vector3(-5, -5, 0);

                break;

        }

        TeleportEffect();
        yield return new WaitForSeconds(1f);

        //Boss isn't visable
        GoInvisable();
        yield return new WaitForSeconds(1f);

        TeleportIndicator();
        yield return new WaitForSeconds(indicatorTime);

        //Bos is visable
        gameObject.transform.localPosition = movePos;

        StartCoroutine(SpecialAttack()); 
        yield return new WaitForSeconds(1);

        StartCoroutine(AttackPattern());
        
    }


    private void BasicAttack()
    {
        bulletHandler.GetBullet(gameObject.transform.position, (target.transform.position - gameObject.transform.position).normalized, false, damage, bulletSize, bulletSpeed);

    }
     IEnumerator SpecialAttack()
    {
        for (int i = 0; i < 4; i++)
        {
            yield return new WaitForSeconds(0.15f);
        bulletHandler.GetCircleShot(specialBulletAmount, gameObject, false, i*25 ,specialDamage, specialBulletSize, specialBulletSpeed);
        }
    }


    IEnumerator AttackPattern()
    {
        for (int i = 0; i < numberOfShotsInPattern; i++)
        {
            BasicAttack();
            yield return new WaitForSeconds(timeBetweenShots);

        }

        canMove = true;
        yield return new WaitForSeconds(2f);
       
    }

    private void TeleportIndicator()
    {
        AttackIndicator.CreateCircle(parentPos + movePos, indicatorRadius, indicatorTime, true);
    }



    private void GoInvisable()
    {
        gameObject.transform.position = new Vector3(100, 100, 0);
       
    }

    private void TeleportEffect()
    {
        teleportParticleEffect.Play();
    }

}
