using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    
    [SerializeField] float bulletSpeed = 8;
    [SerializeField] float bulletLifetime = 7.5f;
    [SerializeField] GameObject player; 

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");

        //Use our rigidbody to move our bullet 
        transform.up = transform.position - player.transform.position;

        //Destroy bullet after X seconds
        Destroy(gameObject, bulletLifetime);

    }
    private void Update()
    {
        transform.up = transform.position - player.transform.position;
        GetComponent<Rigidbody2D>().velocity = transform.up * bulletSpeed;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      
    }
}
