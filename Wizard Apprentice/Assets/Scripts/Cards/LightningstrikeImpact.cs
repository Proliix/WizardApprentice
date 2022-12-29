using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningstrikeImpact : MonoBehaviour
{

    [SerializeField] AudioClip attackSound;

    PlayerStats playerStats;

    List<Health> enemyHealth;

    [SerializeField] float timer;
    [SerializeField] float damage = 50;
    [SerializeField] float lifeTime = 0.25f;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        enemyHealth = new List<Health>();

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().RemoveHealth(playerStats.GetDamage(damage));

           
        }
    }



    
}
