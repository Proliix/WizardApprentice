using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgeObstacle : MonoBehaviour
{

    GameObject playerTarget;
    PlayerMovement playerMovement;



    private void Start()
    {
        playerTarget = GameObject.FindGameObjectWithTag("Player");
        playerMovement = playerTarget.GetComponent<PlayerMovement>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            if (playerMovement.isDashing == false)
            {
            StartCoroutine(DeathByLava());
            }
            
        }
    }

    IEnumerator DeathByLava()
    {
        playerTarget.transform.position = new Vector3(0, -27f, 0);
        yield return null;
    }

   
}

