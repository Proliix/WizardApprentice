using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHurtBox : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float attackDelay;
    [SerializeField] float timer;



    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer >= attackDelay)
        {

            if (collision.gameObject.CompareTag("Enemy"))
            {
                timer -= attackDelay;
                collision.gameObject.GetComponent<Health>().RemoveHealth(damage);
            }
        }

    }

    private void Update()
    {
        timer += Time.deltaTime;
    }
}
