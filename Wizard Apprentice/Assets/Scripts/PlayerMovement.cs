using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //Player Reference 
    Rigidbody2D rb2d;

    //Current movement
    Vector2 movement = new Vector2();
    Animator animator;

    [SerializeField] Health health;

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

        //Update our movement 
        rb2d.velocity = movement.normalized * activeSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && (canDash || (dashes > 0 && !isDashing)))
        {
            StartCoroutine(Dash());
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if ()
        //{

        //}
    }

    private IEnumerator Dash()
    {
        dashes--;
        canDash = false;
        isDashing = true;
        activeSpeed = dashingSpeed;
        health.SetInvicible(dashingTime + 0.2f);
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        activeSpeed = moveSpeed;
        yield return new WaitForSeconds(dashingCooldown - stats.dashCooldown);
        canDash = true;
        dashes++;
    }

}
