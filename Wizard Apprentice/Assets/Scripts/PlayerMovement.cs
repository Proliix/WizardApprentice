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

    //Player Movement speed 
    [SerializeField] float moveSpeed = 10;
    [SerializeField] float dashingSpeed;
    [SerializeField] float activeSpeed;
    [SerializeField] float dashingTime = 0.2f;
    [SerializeField] bool canDash = true;
    [SerializeField] bool isDashing;
    [SerializeField] float dashingCooldown = 1f;

    void Start()
    {
        //Find our Rigidbody2D 
        rb2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        activeSpeed = moveSpeed;

    }

    void Update()
    {

        //get input from player
        float horInput = Input.GetAxisRaw("Horizontal");
        float verInput = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", horInput);
        animator.SetFloat("Vertical", verInput);

        movement.x = horInput;
        movement.y = verInput;

        //Update our movement 
        rb2d.velocity = movement.normalized * activeSpeed;

        if (Input.GetKeyDown(KeyCode.Space) && canDash)
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
        canDash = false;
        isDashing = true;
        activeSpeed = dashingSpeed;

        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        activeSpeed = moveSpeed;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true; 
    }

   

}
