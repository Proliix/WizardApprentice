using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatAI : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] float moveSpeed = 2;
    [SerializeField] float timer;
    
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        float step = moveSpeed * Time.deltaTime;
        transform.position = Vector2.MoveTowards(transform.position, target.position, step);
    }
}
