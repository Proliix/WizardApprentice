using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHurtBox : MonoBehaviour
{
    [SerializeField] float damage = 10f;
    [SerializeField] float attackDelay;
    [SerializeField] float timer;

    List<Health> enemyhealth;

    private void Start()
    {
        enemyhealth = new List<Health>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (timer >= attackDelay)
        {

            if (collision.gameObject.CompareTag("Enemy"))
            {
                collision.gameObject.GetComponent<Health>().RemoveHealth(damage);
            }
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Health>() != null)
                enemyhealth.Add(collision.gameObject.GetComponent<Health>());
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (collision.gameObject.GetComponent<Health>() != null)
                enemyhealth.Remove(collision.gameObject.GetComponent<Health>());
        }
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (timer >= attackDelay)
        {
            timer = 0;

            for (int i = 0; i < enemyhealth.Count; i++)
            {
                if (enemyhealth[i] != null)
                {
                    enemyhealth[i].RemoveHealth(damage);
                }
            }

        }

    }
}
