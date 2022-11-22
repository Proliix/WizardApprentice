using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject shooter;
    public bool moveAwayFromShoter;
    public int poolIndex;

    [SerializeField] float bulletSpeed = 8;
    [SerializeField] float bulletLifetime = 5f;

    float timer = 0;
    Vector3 dir;
    BulletHandler bulletHandler;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
    }

    public void UpdateDirection()
    {
        if (moveAwayFromShoter)
        {
            dir = transform.position - shooter.transform.position;
        }
        else
        {
            dir = transform.up;
        }

    }

    public void ResetTimer()
    {
        timer = 0;
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if (shooter != null)
        {
            GetComponent<Rigidbody2D>().velocity = dir * bulletSpeed;
        }
        else
        {
            Debug.LogError("Shooter is not defined in " + this.name);
        }

        if(timer > bulletLifetime)
        {
            bulletHandler.ResetBullet(poolIndex);
            ResetTimer();
        }
    
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }
}
