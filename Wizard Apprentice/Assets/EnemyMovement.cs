using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
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
    [SerializeField] bool isDashing;


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
        timer = waitTimer;
    }

   

    private void Update()
    {
        if (isDashing)
        {
            return;
        }
        //calculates speed towards the player 
        float step = moveSpeed * Time.deltaTime;
        

        timer+= Time.deltaTime;
        

        //moves the enemy towards the player when the timer is between wait timer and chase timer 
        if (target != null && timer < 0 && timer > waitTimer)
        {
            transform.position = Vector2.MoveTowards(transform.position, target.position, step);
        }
        else
        {
            StartCoroutine(DashAttack());
        }

        if (timer < waitTimer)
        {
            timer+= Time.deltaTime; 
        }

        if (timer >= 0)
        {
            timer = 0;
        }
    }

   
     IEnumerator DashAttack()
    {
        isDashing = true;
        Vector2 playerPosition = target.position;
        yield return new WaitForSeconds(0);
        float timeDashed = 0;
        while (timeDashed < 1)
        {
            float dashStep = dashMoveSpeed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, playerPosition, dashStep);
            timeDashed += Time.deltaTime;
            yield return null;
            
        }
        isDashing = false;
    }
}
