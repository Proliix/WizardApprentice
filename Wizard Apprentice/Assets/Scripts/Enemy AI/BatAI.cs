using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour, IStunnable
{
    [SerializeField] private Transform target;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float timer;
    [SerializeField] bool stunned = false;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        if (!stunned)
        {

            MoveEnemy();

        }

    }

    void MoveEnemy()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
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
