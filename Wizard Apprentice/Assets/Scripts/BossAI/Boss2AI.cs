using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AI : MonoBehaviour
{
    [SerializeField] GameObject target;

    [SerializeField] BulletHandler bulletHandler;
    [SerializeField] Health health;

    [Header("Boss Variables")]
    [SerializeField] private float bossHP;
    [SerializeField] private float bossMaxHP;

    [SerializeField] float timeUntilBossStart = 3;

    [SerializeField] float timer;
    [Header("Phases")]
    [SerializeField] bool phase1;
    [SerializeField] bool phase2;
    [SerializeField] bool phase3;

    void Start()
    {
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
    }

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.forward, 90 * Time.deltaTime);
        bulletHandler.GetBullet(target.transform.position, gameObject, false, true);
    }
}
