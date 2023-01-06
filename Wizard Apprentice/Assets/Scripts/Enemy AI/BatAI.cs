using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour, IStunnable
{
    [SerializeField] private Transform target;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float timer;
    [SerializeField] bool stunned = false;

    Rigidbody2D rb2d;
    EnemyManager enemyManager;

    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!stunned && enemyManager.enemiesActive)
        {

            MoveEnemy();

        }

    }

    void MoveEnemy()
    {
        //float step = moveSpeed * Time.deltaTime;
        //transform.position = Vector2.MoveTowards(transform.position, target.position, step);

        Vector3 dir = (target.transform.position - transform.position).normalized;
        rb2d.velocity = dir * moveSpeed;

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
}
