using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 movement;
    [SerializeField] private Transform target;
    [Header("Enemy AI Movespeed variables")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxWalkSpeed = 5;
    [SerializeField] float maxRunSpeed = 10;
    [SerializeField] float moveSpeedIncreasePerSecond = 1;
    [SerializeField] float timerCountsSeconds;


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
    }



    //private void Update()
    //{
    //    //calculates speed towards the player 
    //    float step = moveSpeed * Time.deltaTime;
    //    moveSpeed += moveSpeedIncreasePerSecond * Time.deltaTime;

    //    if (moveSpeed >= maxMoveSpeed)
    //    {
    //        moveSpeed = maxMoveSpeed;
    //    }
    //    //moves the enemy towards the player
    //   transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    //}

    private void Update()
    {

        timerCountsSeconds += Time.deltaTime;

        moveSpeed += moveSpeedIncreasePerSecond * Time.deltaTime;

        MoveEnemy();

        if (timerCountsSeconds <= 5)
        {
            maxWalkSpeed = 5;
        }

        if (timerCountsSeconds >= 5)
        {
            maxWalkSpeed = 10;
        }

        if (timerCountsSeconds > 10)
        {
            timerCountsSeconds = 0;
        }

        //Todo 
    }


    void MoveEnemy()
    {
        //calculates speed towards the player 
        float step = moveSpeed * Time.deltaTime;

        //moves the enemy towards the player
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    }


 



}
