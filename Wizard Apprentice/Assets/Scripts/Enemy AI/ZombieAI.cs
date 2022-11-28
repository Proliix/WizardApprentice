using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieAI : MonoBehaviour
{
    private Rigidbody2D rb2d;
    [SerializeField] private Transform target;
    [Header("Enemy AI Movespeed variables")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float moveSpeedIncreasePerSecond = 0.5f;
    [SerializeField] float timerCountsSeconds;
    [SerializeField] float runTime = 5;
    [SerializeField] float moveSpeedOnReset = 4;

    private void OnTriggerEnter2D(Collider2D other)
    {

    }

    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
    }

    private void Update()
    {

        timerCountsSeconds += Time.deltaTime;

        moveSpeed += moveSpeedIncreasePerSecond * Time.deltaTime;

        MoveEnemy();


        if (timerCountsSeconds > runTime)
        {
            timerCountsSeconds = 0;
            moveSpeed = moveSpeedOnReset;
        }
    }

    void MoveEnemy()
    {

        Vector3 dir = (target.transform.position - transform.position).normalized;

        ////calculates speed towards the player 
        //float step = moveSpeed * Time.deltaTime;

        ////moves the enemy towards the player
        //transform.position = Vector2.MoveTowards(transform.position, target.position, step);

        rb2d.AddForce(dir.normalized * moveSpeed, ForceMode2D.Force);
        rb2d.velocity = new Vector3(Mathf.Clamp(rb2d.velocity.x, -moveSpeed, moveSpeed), Mathf.Clamp(rb2d.velocity.y, -moveSpeed, moveSpeed), 0);

    }

}



