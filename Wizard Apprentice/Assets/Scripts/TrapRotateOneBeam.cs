using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapRotateOneBeam : MonoBehaviour
{
    BulletHandler bulletHandler;
    float timer;

    [SerializeField] float damage = 10;
    [SerializeField] float attackCoolDown = 2.5f;
    [SerializeField] float bulletSpeed = 7.5f;
    [SerializeField] float bulletSize = 1f;
    [SerializeField] float shootingAmount = 1;
    [SerializeField] int bulletAmount = 10;
    [SerializeField] float angleChange = 3;
    [SerializeField] float timeBetweenShots = 0.2f;

    float aoeAngle = 0;
    bool isShooting;

    
    void Start()
    {
        isShooting = false;
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        timer = 0;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (isShooting == false)
        {
            isShooting = true;
            StartCoroutine(BasicAttack());
            timer -= attackCoolDown;
        }
    }

    IEnumerator BasicAttack()
    {

        for (int i = 0; i < shootingAmount; i++)
        {
           aoeAngle += angleChange;
            bulletHandler.GetCircleShot(bulletAmount, gameObject, false, aoeAngle, damage, bulletSize, bulletSpeed);
            yield return new WaitForSeconds(timeBetweenShots);

        }
        isShooting = false;
        yield return null;
    }
}
