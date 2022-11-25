using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpecialBulletState { Normal, Static, Timed, Rotating, bouncy }

public class SpecialProjectile : MonoBehaviour
{
    public SpecialBulletState bulletState;
    public int poolIndex;
    public Sprite Image;
    public GameObject Shooter;
    public bool isPlayerBullet;
    [SerializeField] float bulletSpeed;
    [SerializeField] float bulletLifetime = 5f;
    [Header("Timed Shot")]
    [SerializeField] float timeToExplode = 1.5f;

    //bool is used to make the bullet change direction away form the shooter
    public bool isMovingAway;

    Vector3 dir;
    SpriteRenderer spriteRenderer;
    float timerToExplode = 0;
    float timer = 0;
    Rigidbody2D rb2d;
    BulletHandler bulletHandler;

    public ICard currentIcard;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        dir = transform.up;
    }

    #region Updates for projectiles

    void NormalShot()
    {
        rb2d.velocity = dir.normalized * bulletSpeed;
    }

    void RotatingShot()
    {
        transform.RotateAround(Shooter.transform.position, transform.up, bulletSpeed * Time.deltaTime);
    }

    void TimedShot()
    {
        NormalShot();
        timerToExplode += Time.deltaTime;
        if (timerToExplode >= timeToExplode)
        {
            timeToExplode = -10;
            if (currentIcard != null)
                currentIcard.Effect();
            else
                Debug.LogError("TimedShot does not have a currentIcard in: " + gameObject.name);
        }
    }

    #endregion

    public void ResetBullet()
    {
        bulletHandler.ResetBullet(poolIndex);
        timerToExplode = 0;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //the update functions for the  bullets
        if (bulletState != SpecialBulletState.Static)
        {
            switch (bulletState)
            {
                case SpecialBulletState.Timed:
                    TimedShot();
                    break;
                case SpecialBulletState.Rotating:
                    RotatingShot();
                    break;
                default:
                    NormalShot();
                    break;
            }
        }

        if (timer > bulletLifetime)
        {
            ResetBullet();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
            {
                if (bulletState == SpecialBulletState.Timed)
                {
                    timerToExplode += timeToExplode;
                }

                collision.gameObject.GetComponent<Health>().RemoveHealth();
                bulletHandler.ResetBullet(poolIndex);
                ResetBullet();
            }
        }
        else if (!collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
        {
            bulletHandler.ResetBullet(poolIndex);
            ResetBullet();
        }
    }
}
