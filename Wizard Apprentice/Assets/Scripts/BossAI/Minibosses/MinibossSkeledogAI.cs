using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossSkeledogAI : MonoBehaviour
{
    [SerializeField] GameObject firePrefab;
    [SerializeField] bool floor3Attacks = false;
    [SerializeField] float timer;
    [SerializeField] float moveSpeed = 7.5f;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AudioClip rockFall;
    [SerializeField] AudioClip groundRumble;
    [SerializeField] float audioVolume = 1;



    [Header("AOE Variables")]
    [SerializeField] int aoeAmount = 20;
    [SerializeField] float aoeDamage = 20;
    [SerializeField] float aoeSize = 1;
    [SerializeField] float aoeSpeed = 5;
    [SerializeField] float aoeAngle;
    [SerializeField] float numberOfShotsPerAOE = 3;


    [Header("Movement Variables")]
    [SerializeField] bool canMoveIndic = true;
    [SerializeField] bool canDash = true;
    [SerializeField] bool hasDashedOnce = false;


    GameObject playerTarget;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    Health health;
    Rigidbody2D rb2d;
    Animator anim;
    bool canHitWall;
    Vector2 movement;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        enemyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        canMoveIndic = true;
        canDash = true;
        hasDashedOnce = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        //if (timer >= 0.1f)
        //{
        //    AttackIndicator.CreateCircle(transform.position, 3, 0.5f, true);
        //    timer -= 0.1f;
        //}

        if (timer > 2 && (rb2d.velocity.x < 0.25f && rb2d.velocity.x > -0.25f) && (rb2d.velocity.y < 0.25f && rb2d.velocity.y > -0.25f))
            HasHitWall();


        if (hasDashedOnce == false && enemyManager.enemiesActive)
        {
            hasDashedOnce = true;
            StartCoroutine(DashAttack());
        }

    }


    IEnumerator DashAttack()
    {
        yield return new WaitForSeconds(0.5f);
        FlipSpriteTowardsPlayer();


        movement = (playerTarget.transform.position - gameObject.transform.position).normalized;
        DashIndicator();
        yield return new WaitForSeconds(0.5f);
        if (anim != null)
            anim.SetTrigger("StartDash");
        rb2d.velocity = movement * moveSpeed;

        canHitWall = true;

        yield return null;
    }

    IEnumerator AOEAttack()
    {
       
        if (floor3Attacks == true)
        {
            aoeSize = 2;
        }

        for (int i = 0; i < numberOfShotsPerAOE; i++)
        {
            if (floor3Attacks == true)
            {


                aoeSize -= 0.3f;

            }
            bulletHandler.GetCircleShot(aoeAmount, gameObject, false, aoeAngle, aoeDamage, aoeSize, aoeSpeed);
            yield return new WaitForSeconds(0.1f);
            
        }

        

        yield return null;

    }

    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall") && canHitWall && timer > 1.5f)
        {
            HasHitWall();
        }
    }

    private void HasHitWall()
    {
        if (anim != null)
            anim.SetTrigger("StopDash");
        canHitWall = false;
        timer = 0;
        SoundManager.Instance.PlayAudio(rockFall, audioVolume);
        Camera.main.GetComponent<CameraMovement>().GetScreenShake();
        //   SoundManager.Instance.PlayAudio(groundRumble, audioVolume);

        rb2d.velocity = new Vector2(0, 0);
        StartCoroutine(AOEAttack());
        StartCoroutine(DashAttack());

        //Camera.main.GetComponent<>
    }

    private void FlipSpriteTowardsPlayer()
    {
        Vector3 direction = (playerTarget.transform.position - rb2d.transform.position).normalized;


        if (direction.x >= 0)
        {
            spriteRenderer.flipX = false;
        }
        else
        {
            spriteRenderer.flipX = true;
        }
    }

    private void DashIndicator()
    {
        AttackIndicator.CreateSquare(gameObject.transform.position, playerTarget.transform.position, new Vector2(1.5f, 30), 1f, true);
    }

}
