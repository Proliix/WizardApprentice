using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject shooter;
    public float damage = 10f;
    public float hitNumberTime = 1.25f;
    public bool moveAwayFromShoter;
    public bool isPlayerBullet;
    public int poolIndex;
    public Color PlayerColor;
    public Color EnemyColor;

    public float bulletSpeed = 8;
    [SerializeField] float bulletLifetime = 5f;

    SpriteRenderer spriteRenderer;
    Rigidbody2D rb2d;
    float timer = 0;
    Vector3 dir;
    BulletHandler bulletHandler;
    HitNumberHandler hitNumbers;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        hitNumbers = GameObject.FindWithTag("GameController").GetComponent<HitNumberHandler>();
        rb2d = GetComponent<Rigidbody2D>();
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

    public void UpdateDirection(Vector3 newDir)
    {

        dir = newDir;

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

        rb2d.velocity = dir.normalized * bulletSpeed;

        if (timer > bulletLifetime)
        {
            bulletHandler.ResetBullet(poolIndex);
            ResetTimer();
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
            {

                if (collision.gameObject.GetComponent<Health>().GetCanBeHit() == true)
                {
                    if (hitNumbers != null)
                        hitNumbers.GetHitText(transform.position + new Vector3(Random.Range(0, 0.25f), Random.Range(0, 0.25f), 0), damage,hitNumberTime);
                    collision.gameObject.GetComponent<Health>().RemoveHealth(damage);
                    bulletHandler.ResetBullet(poolIndex);
                    ResetTimer();
                }
            }
        }

        if (collision.gameObject.CompareTag("Wall"))
        {
            bulletHandler.ResetBullet(poolIndex);
            ResetTimer();
        }
    }



}
