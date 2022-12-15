using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossTeleportingSkeletonAI : MonoBehaviour
{


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

        yield return new WaitForSeconds(1f);
        GoInvisable();
        yield return new WaitForSeconds(1f);
        TeleportIndicator();
        yield return new WaitForSeconds(indicatorTime);

        gameObject.transform.localPosition = movePos;
        GoVisable();

        SpecialAttack();

        yield return new WaitForSeconds(1);

        StartCoroutine(AttackPattern());

        yield return null;
    }


    private void BasicAttack()
    {
        bulletHandler.GetBullet(gameObject.transform.position, (target.transform.position - gameObject.transform.position).normalized, false, damage, bulletSize, bulletSpeed);

    }
    private void SpecialAttack()
    {
        bulletHandler.GetCircleShot(specialBulletAmount, gameObject, false, specialDamage, specialBulletSize, specialBulletSpeed);
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
        yield return null;
    }

    private void TeleportIndicator()
    {
        AttackIndicator.CreateCircle(parentPos + movePos, indicatorRadius, indicatorTime, true);
    }



    private void GoInvisable()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        gameObject.GetComponent<CapsuleCollider2D>().GetComponentInChildren<CapsuleCollider2D>().enabled = false;
    }


    private void GoVisable()
    {
        gameObject.GetComponent<SpriteRenderer>().enabled = true;
        gameObject.GetComponent<CapsuleCollider2D>().GetComponentInChildren<CapsuleCollider2D>().enabled = true;

    }


}
