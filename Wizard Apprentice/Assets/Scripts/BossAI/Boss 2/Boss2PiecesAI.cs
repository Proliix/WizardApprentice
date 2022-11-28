using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2PiecesAI : MonoBehaviour
{
    [SerializeField] GameObject rotationTarget;
    [SerializeField] BulletHandler bulletHandler;

    [SerializeField] public float timer;

    [Header("Boss Variables")]

    [SerializeField] public float timeUntilBossStart = 3;
    [SerializeField] public float attackDelay;
    [SerializeField] public float attackDelayDecrease;
    [SerializeField] public float minAttackDelay = 0.02f;
    [SerializeField] public float RotationSpeed = 90;
    [SerializeField] public bool canShoot;

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();

    }

    void Update()
    {
        transform.RotateAround(rotationTarget.transform.position, Vector3.forward, RotationSpeed * Time.deltaTime);
       
        timer += Time.deltaTime;

        if (timer >= attackDelay && canShoot)
        {
            BossAttackBasic();
            timer -= attackDelay;
        }

    }

    void BossAttackBasic()
    {
        bulletHandler.GetBullet(rotationTarget.transform.position, gameObject, false, true);
    }

    void BossAttackSpecial()
    {

    }

    void Phase1()
    {

    }

    void Phase2()
    {

    }

    void Phase3()
    {

    }


}
