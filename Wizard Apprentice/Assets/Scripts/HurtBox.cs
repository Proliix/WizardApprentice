using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtBox : MonoBehaviour
{
    [SerializeField] float damage;

    Health playerHealth;


    private void Start()
    {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Has hit something");
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Has hit player should work");
            playerHealth.RemoveHealth();
        }
    }
}
