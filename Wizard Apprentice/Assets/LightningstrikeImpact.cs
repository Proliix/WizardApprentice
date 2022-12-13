using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningstrikeImpact : MonoBehaviour
{

    [SerializeField] AudioClip attackSound;

    PlayerStats playerStats;

    List<Health> enemyHealth;

    [SerializeField] float timer;
    [SerializeField] float damage = 3.33f;
    [SerializeField] float damageDelay = 0.1f;
    [SerializeField] float lifeTime = 2f;
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
            if (collision.gameObject.GetComponent<Health>() != null)
                enemyHealth.Add(collision.gameObject.GetComponent<Health>());
        }
    }

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= playerStats.GetAttackSpeed(damageDelay))
        {
            timer = 0;

            for (int i = 0; i < enemyHealth.Count; i++)
            {
                if (enemyHealth[i] != null)
                {
                    enemyHealth[i].RemoveHealth(playerStats.GetDamage(damage));
                }
            }

        }
    }

    
}
