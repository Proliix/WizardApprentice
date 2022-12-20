using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpecialBulletState { Normal, Static, Timed, Rotating, Bouncy, Homing, Onhit, HauntedArmorBigArrow, HauntedArmorSplittingArrow, WontHitWall }

public class SpecialProjectile : MonoBehaviour
{
    public List<float> data;

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
    bool stoppedMoving = false;
    Vector3 dir;
    SpriteRenderer spriteRenderer;
    float effectTimer = 0;
    float timer = 0;
    Rigidbody2D rb2d;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    float startLifeTime = 0;

    Vector3 cursorPos;
    bool hasHitWall = false;

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

    public void UpdateDirection()
    {
        if (isMovingAway)
        {
            dir = transform.position - Shooter.transform.position;
        }
        else
        {
            dir = transform.up;
        }

    }

    public void UpdateDirection(Vector3 Direction)
    {
        dir = Direction;
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
        if (!hasShot)
        {
            hasShot = true;
            relativeDistance = transform.position - Shooter.transform.position;
            timer = 0;
        }

        float distance = Mathf.Sin(timer * effectCooldown);

        transform.position = Shooter.transform.position + relativeDistance.normalized * (effectSize * Mathf.Abs(distance) + 1.25f);

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
        dir = transform.up;

        if (!hasShot)
        {
            homingTarget = enemyManager.GetClosestEnemy(gameObject.transform.position);
            hasShot = true;
        }

        if (homingTarget != null)
        {
            Vector3 direction = homingTarget.transform.position - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * (bulletSpeed / 2));
        }
        rb2d.velocity = dir * bulletSpeed;
    }

    void StaticShot()
    {
        if (rb2d.velocity == Vector2.zero)
        {

            if (!hasShot)
            {
                hasShot = true;
                cursorPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }

            effectTimer += Time.deltaTime;

            if (effectTimer >= effectCooldown)
            {
                currentIcard.Effect();
                effectTimer = 0;
            }
        }

        if ((transform.position.x > cursorPos.x - 0.75f && transform.position.x < cursorPos.x + 0.75f &&
            transform.position.y > cursorPos.y - 0.75f && transform.position.y < cursorPos.y + 0.75f) || hasHitWall)
        {
            stoppedMoving = true;

            if (rb2d.velocity != Vector2.zero)
                rb2d.velocity = Vector2.zero;
        }
        else
        {
            if (dir == null || dir == Vector3.zero)
            {
                dir = transform.up;
            }

            rb2d.velocity = dir.normalized * bulletSpeed;
        }
    }

    void BouncyShot()
    {
        if (rb2d.velocity.x > -0.05f && rb2d.velocity.x < 0.05f && rb2d.velocity.y > -0.05f && rb2d.velocity.y < 0.05f)
        {
            NormalShot();
        }
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
        hasHitWall = false;
        stoppedMoving = false;
        if (data != null)
        {
            data.Clear();
        }
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

        if (collision.gameObject.GetComponent<Health>() != null)
        {
            if (bulletState != SpecialBulletState.Static)
            {
                if ((collision.gameObject.CompareTag("Player") && !isPlayerBullet) || (collision.gameObject.CompareTag("Enemy") && isPlayerBullet))
                {

                    switch (bulletState)
                    {
                        case SpecialBulletState.Timed:
                            effectTimer += effectCooldown;
                            break;
                        case SpecialBulletState.Rotating:

                            break;
                        case SpecialBulletState.Normal:
                            ResetBullet();
                            break;
                        case SpecialBulletState.Homing:
                            ResetBullet();
                            break;
                        case SpecialBulletState.Onhit:
                            currentIcard.Effect();
                            collision.GetComponent<Health>().RemoveHealth(damage);
                            ResetBullet();
                            break;
                        case SpecialBulletState.Bouncy:
                            Vector2 prevVelocity = dir;
                            dir *= -1f;
                            float theta = Mathf.Atan2(dir.y, dir.x);
                            if (theta < 0.0)
                                theta += Mathf.PI * 2;
                            transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * theta - 90) * -1));
                            NormalShot();
                            break;
                        case SpecialBulletState.HauntedArmorBigArrow:
                            if (data[0] <= 0)
                            {
                                ResetBullet();
                                break;
                            }
                            dir *= -1f;
                            float angle = Mathf.Atan2(dir.y, dir.x);
                            if (angle < 0.0)
                                angle += Mathf.PI * 2;
                            transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * angle - 90) * -1));
                            data[0]--;
                            NormalShot();
                            break;
                    }

                    collision.gameObject.GetComponent<Health>().RemoveHealth(damage);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Wall"))
        {
            hasHitWall = true;
            switch (bulletState)
            {
                case SpecialBulletState.WontHitWall:

                    break;

                case SpecialBulletState.Timed:

                    break;
                case SpecialBulletState.Rotating:

                    break;
                case SpecialBulletState.Normal:
                    ResetBullet();
                    break;
                case SpecialBulletState.Homing:
                    ResetBullet();
                    break;
                case SpecialBulletState.Bouncy:
                    Vector2 prevVelocity = dir;
                    BoxCollider2D collider = collision.GetComponent<BoxCollider2D>();
                    if (collision.transform.position.y + collider.offset.y + collision.GetComponent<BoxCollider2D>().size.y * -0.5f > rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.y + collider.offset.y + collider.size.y * 0.5f < rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.x + collider.offset.x + collider.size.x * -0.5f > rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    else if (collision.transform.position.x + collider.offset.x + collider.size.x * 0.5f < rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    float theta = Mathf.Atan2(dir.y, dir.x);
                    if (theta < 0.0)
                        theta += Mathf.PI * 2;
                    transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * theta - 90) * -1));
                    NormalShot();
                    break;
                case SpecialBulletState.HauntedArmorBigArrow:
                    if (data[0] <= 0)
                    {
                        ResetBullet();
                        break;
                    }
                    BoxCollider2D HauntedArmorBigArrow_collider = collision.GetComponent<BoxCollider2D>();
                    if (collision.transform.position.y + HauntedArmorBigArrow_collider.offset.y + HauntedArmorBigArrow_collider.size.y * -0.5f > rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.y + HauntedArmorBigArrow_collider.offset.y + HauntedArmorBigArrow_collider.size.y * 0.5f < rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.x + HauntedArmorBigArrow_collider.offset.x + HauntedArmorBigArrow_collider.size.x * -0.5f > rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    else if (collision.transform.position.x + HauntedArmorBigArrow_collider.offset.x + HauntedArmorBigArrow_collider.size.x * 0.5f < rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    float HauntedArmorBigArrow_angle = Mathf.Atan2(dir.y, dir.x);
                    if (HauntedArmorBigArrow_angle < 0.0)
                        HauntedArmorBigArrow_angle += Mathf.PI * 2;
                    transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * HauntedArmorBigArrow_angle - 90) * -1));
                    data[0]--;
                    NormalShot();
                    break;
                case SpecialBulletState.HauntedArmorSplittingArrow:
                    BoxCollider2D HauntedArmorSplittingArrow_collider = collision.GetComponent<BoxCollider2D>();
                    if (collision.transform.position.y + HauntedArmorSplittingArrow_collider.offset.y + HauntedArmorSplittingArrow_collider.size.y * -0.5f > rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.y + HauntedArmorSplittingArrow_collider.offset.y + HauntedArmorSplittingArrow_collider.size.y * 0.5f < rb2d.position.y)
                    {
                        dir.y *= -1f;
                    }
                    else if (collision.transform.position.x + HauntedArmorSplittingArrow_collider.offset.x + HauntedArmorSplittingArrow_collider.size.x * -0.5f > rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    else if (collision.transform.position.x + HauntedArmorSplittingArrow_collider.offset.x + HauntedArmorSplittingArrow_collider.size.x * 0.5f < rb2d.position.x)
                    {
                        dir.x *= -1f;
                    }
                    float HauntedArmorSplittingArrow_angle = Mathf.Atan2(dir.y, dir.x);
                    if (HauntedArmorSplittingArrow_angle < 0.0)
                        HauntedArmorSplittingArrow_angle += Mathf.PI * 2;
                    transform.localRotation = Quaternion.Euler(0, 0, ((Mathf.Rad2Deg * HauntedArmorSplittingArrow_angle - 90) * -1));

                    Vector2 newDir1 = new Vector2(Mathf.Cos((HauntedArmorSplittingArrow_angle + 15 * Mathf.Deg2Rad)), Mathf.Sin(HauntedArmorSplittingArrow_angle + 15 * Mathf.Deg2Rad));
                    bulletHandler.GetBullet(rb2d.position + (Vector2)dir.normalized, newDir1.normalized, false, 10, 0.5f, 8);
                    Vector2 newDir2 = new Vector2(Mathf.Cos((HauntedArmorSplittingArrow_angle - 15 * Mathf.Deg2Rad)), Mathf.Sin(HauntedArmorSplittingArrow_angle - 15 * Mathf.Deg2Rad));
                    bulletHandler.GetBullet(rb2d.position + (Vector2)dir.normalized, newDir2.normalized, false, 10, 0.5f, 8);
                    ResetBullet();
                    break;
            }
        }
    }

}
