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
        Camera.main.GetComponent<CameraMovement>().GetScreenShake(0.25f, 1);

        Destroy(gameObject, lifeTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            collision.gameObject.GetComponent<Health>().RemoveHealth(playerStats.GetDamage(damage));

        }
        
           
        
    }



    
}
