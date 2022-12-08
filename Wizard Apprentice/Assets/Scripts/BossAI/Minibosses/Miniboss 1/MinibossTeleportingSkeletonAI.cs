using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossTeleportingSkeletonAI : MonoBehaviour
{


    private Rigidbody2D rb2d;

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
    

    [SerializeField] GameObject target;

    BulletHandler bulletHandler;


    void Start()
    {
        canMove = true;
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        MoveEnemy();
        target = GameObject.FindWithTag("Player");
      
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
        gameObject.transform.position = new Vector3(-5, 5, 0);
                
                break;

            case 2:
        gameObject.transform.position = new Vector3(5, 5, 0);

                break;

            case 3:
        gameObject.transform.position = new Vector3(5, -5, 0);
                break;
                    

            case 4:
        gameObject.transform.position = new Vector3(-5, -5, 0);
                break;



        }
        yield return new WaitForSeconds(0.25f);
        
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
        yield return null;
    }


}
