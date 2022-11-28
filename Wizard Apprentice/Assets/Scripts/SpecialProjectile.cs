using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpecialBulletState { Normal, Static, Timed, Rotating, Bouncy, Homing }

public class SpecialProjectile : MonoBehaviour
{
    public SpecialBulletState bulletState;
    public int poolIndex;
    public GameObject Shooter;
    public bool isPlayerBullet;
    [SerializeField] float bulletSpeed;
    public float bulletLifetime = 5f;
    public float effectCooldown = 1.5f;

    //bool is used to make the bullet change direction away form the shooter
    public bool isMovingAway;

    Vector3 dir;
    SpriteRenderer spriteRenderer;
    float effectTimer = 0;
    float timer = 0;
    Rigidbody2D rb2d;
    BulletHandler bulletHandler;
    float startLifeTime = 0;

    GameObject homingTarget;

    public ICard currentIcard;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        dir = transform.up;
        startLifeTime = bulletLifetime;
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
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectCooldown)
        {
            effectCooldown = -10;
            if (currentIcard != null)
                currentIcard.Effect();
            else
                Debug.LogError("TimedShot does not have a currentIcard in: " + gameObject.name);
        }
    }

    void HomingShot()
    {

    }

    void StaticShot()
    {
        effectTimer += Time.deltaTime;
        if (effectTimer >= effectCooldown)
        {
            currentIcard.Effect();
            effectTimer = 0;
        }
    }

    #endregion

    public void ResetBullet()
    {
        bulletHandler.ResetSpecialBullet(poolIndex);
        effectTimer = 0;
        timer = 0;
        bulletLifetime = startLifeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        //the update functions for the  bullets

        switch (bulletState)
        {
            case SpecialBulletState.Timed:
                TimedShot();
                break;
            case SpecialBulletState.Rotating:
                RotatingShot();
                break;
            case SpecialBulletState.Static:
                StaticShot();
                break;
            case SpecialBulletState.Homing:
                HomingShot();
                break;
            default:
                NormalShot();
                break;
        }

        if (timer > bulletLifetime)
        {
            ResetBullet();
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (bulletState != SpecialBulletState.Static)
        {
            if (collision.gameObject.GetComponent<Health>() != null)
            {
                if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
                {
                    if (bulletState == SpecialBulletState.Timed)
                    {
                        effectTimer += effectCooldown;
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
}
