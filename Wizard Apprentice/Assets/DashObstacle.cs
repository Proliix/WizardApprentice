using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashObstacle : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] PlayerMovement playerMovement;
    [SerializeField] bool boolFlip;
    BoxCollider2D boxCollider2d;

    void Start()
    {
        boolFlip = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();

        boxCollider2d = gameObject.GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (playerMovement.isDashing == true)
        {
            StartCoroutine(Dashable());
        }

        IEnumerator Dashable()
        {

            boxCollider2d.enabled = false;
               
            yield return new WaitForSeconds(playerMovement.dashingTime);

            boxCollider2d.enabled = true;


            yield return null;
        }

    }
}
