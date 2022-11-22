using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Vector2 movement;
    [SerializeField] private Transform target;
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float maxMoveSpeed = 7.2f;
    [SerializeField] float moveSpeedIncreasePerSecond = 0.5f;


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

   

    private void Update()
    {
      
        //calculates speed towards the player 
        float step = moveSpeed * Time.deltaTime;
        moveSpeed += moveSpeedIncreasePerSecond * Time.deltaTime;
        if (moveSpeed >= maxMoveSpeed)
        {
            moveSpeed = maxMoveSpeed;
        }
        //moves the enemy towards the player
       transform.position = Vector2.MoveTowards(transform.position, target.position, step);
       
    }

   
    
}
