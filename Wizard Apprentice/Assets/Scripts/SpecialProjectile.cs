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
    public float damage = 10f;
    public float bulletSpeed = 8;
    public float bulletLifetime = 5f;
    public float effectCooldown = 1.5f;
    public float effectSize = 1;

    //bool is used to make the bullet change direction away form the shooter
    public bool isMovingAway;

    bool hasShot = false;
    Vector3 dir;
    SpriteRenderer spriteRenderer;
    float effectTimer = 0;
    float timer = 0;
    Rigidbody2D rb2d;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    float startLifeTime = 0;

    Transform parent;
    GameObject homingTarget;

    Vector3 relativeDistance = Vector3.zero;
    Animator anim;

    public ICard currentIcard;

    private void Awake()
    {
        parent = transform.parent;
        anim = gameObject.GetComponent<Animator>();
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        startLifeTime = bulletLifetime;
    }

    #region Updates for projectiles

    void NormalShot()
    {
        if (dir == null || dir == Vector3.zero)
        {
            dir = transform.up;
        }

        rb2d.velocity = dir.normalized * bulletSpeed;
    }

    void RotatingShot()
    {
        if (relativeDistance == Vector3.zero)
            relativeDistance = transform.position - Shooter.transform.position;

        transform.position = Shooter.transform.position + relativeDistance;

        transform.position = Shooter.transform.position + (transform.position - Shooter.transform.position).normalized * effectSize;

        transform.RotateAround(Shooter.transform.position, Vector3.forward, (bulletSpeed * 10) * Time.deltaTime);

        relativeDistance = transform.position - Shooter.transform.position;
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
        if (!hasShot)
        {

            homingTarget = enemyManager.GetClosestEnemy(gameObject.transform.position);
            hasShot = true;
        }

        if (homingTarget != null)
        {
            dir = homingTarget.transform.position - transform.position;
        }

        if (dir == Vector3.zero || dir == null)
        {
            dir = transform.up;
        }

        rb2d.velocity = dir.normalized * bulletSpeed;
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

    void BouncyShot()
    {
        if (rb2d.velocity.x > -0.05f && rb2d.velocity.x < 0.05f && rb2d.velocity.y > -0.05f && rb2d.velocity.y < 0.05f)
        {
            NormalShot();
        }
        Debug.Log("Is here" + rb2d.velocity);
    }
    #endregion

    public void ResetBullet()
    {
        bulletHandler.ResetSpecialBullet(poolIndex);
        ResetAnimations();
        effectTimer = 0;
        hasShot = false;
        rb2d.velocity = Vector2.zero;
        dir = Vector3.zero;
        homingTarget = null;
        timer = 0;
        bulletLifetime = startLifeTime;
        transform.parent = parent;
        relativeDistance = Vector3.zero;
    }

    public void SetAnimationBool(string name, bool value = true)
    {
        anim.SetBool(name, value);
    }

    public void ResetAnimations()
    {
        anim.SetBool("Blackhole", false);
        anim.SetTrigger("Reset");
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
            case SpecialBulletState.Bouncy:
                BouncyShot();
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

                    collision.gameObject.GetComponent<Health>().RemoveHealth(damage);
                    if (bulletState != SpecialBulletState.Rotating && bulletState != SpecialBulletState.Bouncy)
                    {
                        bulletHandler.ResetBullet(poolIndex);
                        ResetBullet();
                    }
                    else if (bulletState == SpecialBulletState.Bouncy)
                    {
                        Vector2 hitInfo = -transform.position.normalized;

                        Vector2 reflection = Vector2.Reflect(rb2d.velocity.normalized, hitInfo);
                        transform.rotation = Quaternion.FromToRotation(transform.up, reflection) * transform.rotation;
                        dir = Vector3.zero;
                        NormalShot();
                    }
                }
            }
            else if (!collision.gameObject.CompareTag("Projectile") && !collision.gameObject.CompareTag("Player") && !collision.gameObject.CompareTag("Enemy"))
            {
                if (bulletState != SpecialBulletState.Rotating && bulletState != SpecialBulletState.Bouncy)
                {
                    bulletHandler.ResetBullet(poolIndex);
                    ResetBullet();
                }
                else if (bulletState == SpecialBulletState.Bouncy)
                {
                    Vector2 hitInfo = -transform.position.normalized;

                    Vector2 reflection = Vector2.Reflect(rb2d.velocity.normalized, hitInfo);
                    transform.rotation = Quaternion.FromToRotation(transform.up, reflection) * transform.rotation;
                    dir = Vector3.zero;
                    NormalShot();
                }
            }
        }
    }
}
