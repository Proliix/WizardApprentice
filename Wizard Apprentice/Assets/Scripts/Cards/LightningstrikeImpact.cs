using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningstrikeImpact : MonoBehaviour
{

    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume;
    PlayerStats playerStats;


    [SerializeField] float timer;
    [SerializeField] float damage = 50;
    [SerializeField] float lifeTime = 0.25f;
    void Start()
    {
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        Camera.main.GetComponent<CameraMovement>().GetScreenShake(0.25f, 0.33f, true);
        SoundManager.Instance.PlayAudio(attackSound, audioVolume);
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
