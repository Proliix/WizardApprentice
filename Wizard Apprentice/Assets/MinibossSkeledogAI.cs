using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossSkeledogAI : MonoBehaviour
{

    [SerializeField] float timer;


    [Header("AOE Variables")]
    [SerializeField] int aoeAmount = 20;
    [SerializeField] float aoeDamage = 20;
    [SerializeField] float aoeSize= 1;
    [SerializeField] float aoeSpeed= 5;
    [SerializeField] float aoeAngle;
    [SerializeField] float numberOfShotsPerAOE = 3;


    [Header("Movement Variables")]
    [SerializeField] bool canMoveIndic;
    


    GameObject playerTarget;
    BulletHandler bulletHandler;
    Health health;
    Rigidbody2D rb2d;

    Vector2 movement;

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        canMoveIndic = true;
    }

    void Update()
    {
        timer += Time.deltaTime;


        if (Input.GetKeyDown(KeyCode.J))
        {
            StartCoroutine(AOEAttack());
        }

        if (canMoveIndic == true)
        {
            rb2d.velocity = movement.normalized;
        }

    }

    

    IEnumerator DashAttack()
    {
        canMoveIndic = false;


        canMoveIndic = true;
        yield return null;
    }

    IEnumerator AOEAttack()
    {

        aoeAngle = Random.Range(1, 33);
          

        for (int i = 0; i < numberOfShotsPerAOE; i++)
        {
        bulletHandler.GetCircleShot(aoeAmount, gameObject, false, aoeAngle, aoeDamage, aoeSize, aoeSpeed);
            yield return new WaitForSeconds(0.2f);
        }

        yield return null;

    }



}
