using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashEnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 movement;
    [SerializeField] private Transform target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float dashMoveSpeed = 10f;
    [SerializeField] float timer;
    [SerializeField] float chaseTimer = 14000;
    [SerializeField] float waitTimer = 7000;
    // [SerializeField] float chargeTime;
    [SerializeField] float chargeTimeLeft;
    


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            target = other.transform;
        }
    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        StartCoroutine(DashAttack());
    }

   

    IEnumerator DashAttack()
    {
        //Randomized time enemy waits inbetween dashes, to avoid them clumping 
        yield return new WaitForSeconds(Random.Range(0.3f, 2.7f));

        //Calculates direction and lenght of dash
        Vector2 dashDirection = target.position - transform.position;
        
        
        float timeDashed = 0;
        
        //TimeDashed = how long a single dash is in seconds 
        while (timeDashed < 0.7f)
        {
            float dashStep = dashMoveSpeed * Time.deltaTime;
          
            transform.position = Vector3.MoveTowards(transform.position, transform.position + (Vector3)dashDirection.normalized * 100, dashStep);
            timeDashed += Time.deltaTime;
            
            yield return null;  
        }
        //Loops the dashes 
        StartCoroutine(DashAttack());
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




