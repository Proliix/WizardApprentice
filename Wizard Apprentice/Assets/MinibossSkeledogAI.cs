using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinibossSkeledogAI : MonoBehaviour
{

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
    Health health;
    Rigidbody2D rb2d;

    Vector2 movement;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        rb2d = gameObject.GetComponent<Rigidbody2D>();

        canMoveIndic = true;
        canDash = true;
        hasDashedOnce = false;
    }

    void Update()
    {
        timer += Time.deltaTime;

        

        if (hasDashedOnce == false)
        {
            hasDashedOnce = true;
            StartCoroutine(DashAttack());
        }

    }


    IEnumerator DashAttack()
    {
        yield return new WaitForSeconds(1);
        FlipSpriteTowardsPlayer();

        


        movement = (playerTarget.transform.position - gameObject.transform.position).normalized;
        rb2d.velocity = movement * moveSpeed;
        
        yield return null;
    }

    IEnumerator AOEAttack()
    {

        aoeAngle = Random.Range(1, 33);

        for (int i = 0; i < numberOfShotsPerAOE; i++)
        {
            bulletHandler.GetCircleShot(aoeAmount, gameObject, false, aoeAngle, aoeDamage, aoeSize, aoeSpeed);
            yield return new WaitForSeconds(0.1f);
        }

        yield return null;

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Wall"));
        {

            SoundManager.Instance.PlayAudio(rockFall, audioVolume);
         //   SoundManager.Instance.PlayAudio(groundRumble, audioVolume);

            rb2d.velocity = new Vector2(0, 0);
            StartCoroutine(AOEAttack());
            StartCoroutine(DashAttack());

            //Camera.main.GetComponent<>
        }
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

}
