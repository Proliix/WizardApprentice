using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject shooter;
    public bool moveAwayFromShoter;
    public bool isPlayerBullet;
    public int poolIndex;
    public Color PlayerColor;
    public Color EnemyColor;

    [SerializeField] float bulletSpeed = 8;
    [SerializeField] float bulletLifetime = 5f;

    SpriteRenderer spriteRenderer;
    float timer = 0;
    Vector3 dir;
    BulletHandler bulletHandler;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
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

    public void UpdateColor()
    {
        if (spriteRenderer == null)
            spriteRenderer = gameObject.GetComponent<SpriteRenderer>();


        Color newColor = isPlayerBullet ? PlayerColor : EnemyColor;
        spriteRenderer.color = newColor;
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
            GetComponent<Rigidbody2D>().velocity = dir.normalized * bulletSpeed;
        }
        else
        {
            Debug.LogError("Shooter is not defined in " + this.name);
        }

        if (timer > bulletLifetime)
        {
            bulletHandler.ResetBullet(poolIndex);
            ResetTimer();
        }

    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.GetComponent<Health>() != null)
    //    {
    //        if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
    //            collision.gameObject.GetComponent<Health>().RemoveHealth();
    //    }
    //}
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
            {
                collision.gameObject.GetComponent<Health>().RemoveHealth();
                bulletHandler.ResetBullet(poolIndex);
                ResetTimer();
            }
            else if (!collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
            {
                bulletHandler.ResetBullet(poolIndex);
                ResetTimer();
            }
        }
    }
}
