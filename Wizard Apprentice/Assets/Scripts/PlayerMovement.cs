using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
   
    Rigidbody2D rb2d;
    Animator animator;
    Health health;
    [SerializeField] DashIndicator dashIndicator;
   
    Vector2 movement = new Vector2();


    //Sound files
    [SerializeField] AudioClip dashSound;


    //Player Movement speed 
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float dashingSpeed;
    [SerializeField] float activeSpeed;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] bool canDash = true;
    [SerializeField] int dashes = 1;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingCooldown = 1f;

    int maxDashes = 1;
    PlayerStats stats;

    void Start()
    {
        //Find our Rigidbody2D 
        maxDashes = dashes;
        dashIndicator = GameObject.Find("DashIndicator").GetComponent<DashIndicator>();
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        stats = GetComponent<PlayerStats>();
        activeSpeed = moveSpeed;

    }

    void Update()
    {

        if (maxDashes < stats.dashCharges + 1)
        {
            maxDashes++;
            dashes++;
        }

        //get input from player
        float horInput = Input.GetAxisRaw("Horizontal");
        float verInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", horInput);
        animator.SetFloat("Vertical", verInput);

        movement.x = horInput;
        movement.y = verInput;

       
            MovePlayer();
        


        if (Input.GetKeyDown(KeyCode.Space) && (canDash || (dashes > 0 && !isDashing)))
        {
            SoundManager.Instance.PlayAudio(dashSound);
           StartCoroutine(Dash());
        }
       
    }


    private IEnumerator Dash()
    {

        dashIndicator.ChangeDashIndicator();
        dashes--;
        canDash = false;
        isDashing = true;
        activeSpeed = dashingSpeed;
        health.SetInvicible(dashingTime + 0.2f);

        yield return new WaitForSeconds(dashingTime);


        isDashing = false;
        activeSpeed = moveSpeed;

        yield return new WaitForSeconds(dashingCooldown - stats.dashCooldown);
        dashIndicator.ChangeDashIndicator();
        canDash = true;
        dashes++;
    }

    private void MovePlayer()
    {
       
        rb2d.velocity = movement.normalized * (activeSpeed + stats.movementSpeed);
    }

}
