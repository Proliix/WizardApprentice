using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float cooldown = 0.25f;

    Health playerHealth;
    float timer;
    bool playerIsIn = false;



    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (playerIsIn)
        {
            if (timer > cooldown)
            {
                timer = 0;
                playerHealth.RemoveHealth(damage);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsIn = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerIsIn = false;
        }
    }
}


