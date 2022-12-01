using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeAI : MonoBehaviour, IStunnable
{
    private Rigidbody2D rb2d;
    [SerializeField] private Transform target;

    [Header("Enemy AI variables")]
    [SerializeField] float jumpDistance;
    [SerializeField] float jumpSpeed;
    [SerializeField] float minJumpCooldown;
    [SerializeField] float maxJumpCooldown;
    [SerializeField] float jumpHeight;
    [SerializeField] AnimationCurve jumpCurve;
    private GameObject hurtBox;
    Vector2 targetPos;
    Vector2 jumpFromPos;
    float timeUntilNextJump;
    bool jumpActive;
    float timeInJump;
    float jumpTime;

    bool stunned = false;
    float stunTime = 0.25f;



    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        target = GameObject.FindWithTag("Player").transform;
        hurtBox = gameObject.GetComponentInChildren<HurtBox>().gameObject;
    }

    private void Update()
    {

        timeUntilNextJump -= Time.deltaTime;
        timeInJump += Time.deltaTime;
        MoveEnemy();
    }

    Vector2 GetPositionThisFrame()
    {
        if(timeUntilNextJump < 0)
        {
            hurtBox.SetActive(true);
            jumpActive = true;
            timeInJump = 0;
            jumpTime = jumpDistance / jumpSpeed;
            jumpFromPos = transform.position;
            targetPos = (target.position - transform.position).normalized * jumpDistance;
            targetPos += (Vector2)transform.position;
            timeUntilNextJump = Random.Range(minJumpCooldown,maxJumpCooldown);
        }
        if (timeInJump > jumpTime)
        {
            jumpActive = false;
            hurtBox.SetActive(false);
        }
        if (jumpActive)
        {
            Vector2 basePos = (targetPos - jumpFromPos) * (timeInJump / jumpTime);
            float addedYPos = jumpCurve.Evaluate(timeInJump / jumpTime) * jumpHeight;
            return basePos + new Vector2(0,addedYPos) + jumpFromPos;
        }
        return transform.position;
    }

    void MoveEnemy()
    {
        if (!stunned)
        {
            Vector3 dir = (target.transform.position - transform.position).normalized;

            ////calculates speed towards the player 
            //float step = moveSpeed * Time.deltaTime;

            ////moves the enemy towards the player
            //transform.position = Vector2.MoveTowards(transform.position, target.position, step);

            //rb2d.AddForce(dir.normalized * moveSpeed, ForceMode2D.Force);
            //rb2d.velocity = new Vector3(Mathf.Clamp(rb2d.velocity.x, -moveSpeed, moveSpeed), Mathf.Clamp(rb2d.velocity.y, -moveSpeed, moveSpeed), 0);

            //rb2d.velocity = dir * moveSpeed;

            Vector2 position = GetPositionThisFrame();
            transform.position = position;
            rb2d.MovePosition(position);
        }

    }


    public void GetStunned(float stunDuration = 0.25F)
    {
        stunTime = stunDuration;

        if (stunned)
            StopCoroutine(IsStunned());

        StartCoroutine(IsStunned());
    }

    public IEnumerator IsStunned()
    {
        stunned = true;
        yield return new WaitForSeconds(stunTime);
        stunned = false;
    }

}




