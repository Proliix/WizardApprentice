using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI2 : MonoBehaviour
{
    [SerializeField] GameObject target1;
    [SerializeField] GameObject target2;
    [SerializeField] GameObject target3;

    [SerializeField] Health health;
    [SerializeField] BulletHandler bulletHandler;

    [SerializeField] float timer;
    [SerializeField] private float HP;
    [SerializeField] private float maxHP;


    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        health = gameObject.GetComponent<Health>();
    }

    void Update()
    {

        HP = health.GetHP();
        maxHP = health.GetMaxHP();
        timer += Time.deltaTime;
        BasicAttack();
    }

    void BasicAttack()
    {
        //bulletHandler.GetBullet(target1.transform.position, gameObject, false, true);
        bulletHandler.GetBullet(transform.position, transform.position - target1.transform.position, false);
    }

}
