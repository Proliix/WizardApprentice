using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingEnemyAI : MonoBehaviour
{
    //Target
    [SerializeField] GameObject target;
    [SerializeField] float timer;

    Rigidbody2D rb2d;
    Vector2 enemyPos;
    bool hasGenereted = true;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        
    }

    void Update()
    {
        timer += Time.deltaTime;
        
        if (timer <= 2)
        {
            //Calculates random direction enemy will move in
            if (hasGenereted)
            {
                enemyPos = RandomDirection();
                hasGenereted = false;

            }
            //Moves enemy
            Movement();
        }
        else if (timer >= 4)
        {
            //enemy is shooting here
            EnemyShooting();
            timer = 0;
            hasGenereted = true;

        }
    }
    public Vector2 RandomDirection()
    {
            switch (Random.Range(0, 4))
            {
                case 0:
                    return new Vector2(1f, 0);

                case 1:
                    return new Vector2(1f, 0);

                case 2:
                    return new Vector2(0, 1f);

                default:
                    return new Vector2(0, -1f);
        }
    }

    public void Movement()
    {
        rb2d.velocity = enemyPos * Time.deltaTime;
       
    }

    private void EnemyShooting()
    {

    }
}
