using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb2d;
    Animator animator;
    Health health;
    [SerializeField] GameObject[] dashIndicators;

    Vector2 movement = new Vector2();


    //Sound files
    [SerializeField] AudioClip dashSound;


    //Player Movement speed 
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float dashingSpeed;
    [SerializeField] float activeSpeed;
    public float dashingTime = 0.2f;
    [SerializeField] public bool canDash = true;
    public bool isDashing;
    [SerializeField] float dashingCooldown = 1f;


    bool CanMove = true;
    PlayerStats stats;
    Vector2 dashMovement = Vector2.right;

    void Start()
    {
        //Find our Rigidbody2D 
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        stats = GetComponent<PlayerStats>();
        activeSpeed = moveSpeed;

    }

    void Update()
    {
        if (CanMove)
        {



            if (!isDashing)
            {
                //get input from player
                float horInput = Input.GetAxisRaw("Horizontal");
                float verInput = Input.GetAxisRaw("Vertical");

                animator.SetFloat("Horizontal", horInput);
                animator.SetFloat("Vertical", verInput);
                movement.x = horInput;
                movement.y = verInput;

                if ((horInput >= 0.1 || horInput <= -0.1) || (verInput >= 0.1 || verInput <= -0.1))
                    dashMovement = movement;

                MovePlayer();
            }

            for (int i = 0; i < dashIndicators.Length; i++)
            {
                dashIndicators[i]?.SetActive(canDash);
            }

            if (Input.GetKeyDown(KeyCode.Space) && canDash)
            {
                SoundManager.Instance.PlayAudio(dashSound);
                StartCoroutine(Dash());

            }
        }
        else
        {
            animator.SetFloat("Horizontal", rb2d.velocity.normalized.x);
            animator.SetFloat("Vertical", rb2d.velocity.normalized.y);
        }

    }

    public void SetCanMove(bool value)
    {
        CanMove = value;
    }
    private IEnumerator Dash()
    {
        
        //dashIndicator.ChangeDashIndicator();
        canDash = false;
        isDashing = true;

        if (dashMovement == Vector2.zero)
            dashMovement = transform.right;

        animator.SetBool("IsDashing", isDashing);
        animator.SetFloat("DashingDuration", (1 / (dashingTime)));
        animator.SetFloat("Horizontal", dashMovement.x);
        animator.SetFloat("Vertical", dashMovement.y);


        activeSpeed = dashingSpeed;
        rb2d.velocity = dashMovement.normalized * (activeSpeed * stats.movementSpeed);
        health.SetInvicible(dashingTime + 0.2f);
        yield return new WaitForSeconds(dashingTime);


        isDashing = false;
        activeSpeed = moveSpeed;
        animator.SetBool("IsDashing", isDashing);

        yield return new WaitForSeconds(dashingCooldown);
        //dashIndicator.ChangeDashIndicator();
        canDash = true;
    }

    private void MovePlayer()
    {
        rb2d.velocity = movement.normalized * (activeSpeed + stats.movementSpeed);
    }

}
