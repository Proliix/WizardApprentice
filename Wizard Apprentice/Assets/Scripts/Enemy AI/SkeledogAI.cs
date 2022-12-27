using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeledogAI : MonoBehaviour, IStunnable
{

    SpriteRenderer enemyFlip;
    private Rigidbody2D rb2d;
    private Vector2 movement;
    [SerializeField] float dashMoveSpeed = 10f;
    //[SerializeField] float chargeTime;
    [SerializeField] float chargeTimeLeft;

    private Transform target;
    private EnemyManager enemyManager;
    private bool stunned = false;

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        enemyFlip = GetComponent<SpriteRenderer>();
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        target = GameObject.FindWithTag("Player").transform;

        StartCoroutine(DashAttack());

    }


    IEnumerator DashAttack()
    {
        rb2d.velocity = Vector2.zero;
        //Randomized time enemy waits inbetween dashes, to avoid them clumping 
        yield return new WaitForSeconds(Random.Range(0.3f, 1.2f));
        if (!stunned && enemyManager.enemiesActive)
        {


            //Calculates direction and lenght of dash
            Vector2 dashDirection = target.position - transform.position;


            float timeDashed = 0;

            //TimeDashed = how long a single dash is in seconds 
            while (timeDashed < 0.7f)
            {

                //Makes the enemy look in the player direction 
                if (Vector3.Distance(rb2d.transform.position, target.position) >= 0)
                {
                    Vector3 direction = (target.position - rb2d.transform.position).normalized;

                    if (direction.x >= 0)
                    {
                        enemyFlip.flipX = false;
                    }
                    else
                    {
                        enemyFlip.flipX = true;
                    }
                }

                float dashStep = dashMoveSpeed * Time.deltaTime;

                transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)dashDirection.normalized * 100, dashStep);
                timeDashed += Time.deltaTime;

                yield return null;
            }
        }
        //Loops the dashes 
        StartCoroutine(DashAttack());
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


    //private void Update()
    //{
    //    if (isDashing)
    //    {
    //        return;
    //    }
    //    //calculates speed towards the player 
    //    float step = moveSpeed * Time.deltaTime;


    //    timer += Time.deltaTime;


    //    //moves the enemy towards the player when the timer is between wait timer and chase timer 
    //    if (target != null && timer < 0 && timer > 0)
    //    {
    //        // transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    //        Debug.Log("YEP COCK");
    //    }
    //    else
    //    {
    //        StartCoroutine(DashAttack());
    //    }

    //    if (timer < waitTimer)
    //    {
    //        timer += Time.deltaTime;
    //    }

    //    if (timer >= 0)
    //    {
    //        timer = 0;
    //    }
    //}
}




