using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserHurtBox : MonoBehaviour
{
    public float damage = 10f;
    public float attackDelay;
    [SerializeField] float timer;

    PlayerStats stats;

    List<Health> enemyhealth;

    private void Start()
    {

        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        enemyhealth = new List<Health>();
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
        if (timer >= stats.GetAttackSpeed(attackDelay))
        {
            timer = 0;

            for (int i = 0; i < enemyhealth.Count; i++)
            {
                if (enemyhealth[i] != null)
                {
                    enemyhealth[i].RemoveHealth(stats.GetDamage(damage));
                }
            }

        }

    }
}
