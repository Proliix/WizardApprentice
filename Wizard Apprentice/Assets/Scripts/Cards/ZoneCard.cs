using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneCard : MonoBehaviour, ICard
{

    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] GameObject target;

    [SerializeField] float attackRange = 7;
    [SerializeField] float timer;


    [Header("Attack Variables")]
    [SerializeField] float damage;
    [SerializeField] float attackDelay = 0.1f;
    [SerializeField] bool hasActivated = false;

    CardHandler cardHandler;
    PlayerStats stats;
    GameObject player;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;

    List<GameObject> enemies;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player");
        enemyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
    }

    public void Effect()
    {
        //SoundManager.Instance.PlayAudio(attackSound);
        enemies = enemyManager.GetEnemiesWithinRange(target.transform.position, attackRange);   
        Debug.Log("Enemies: " + enemies.Count);
        for (int i = 0; i < enemies.Count; i++)
        {
            Debug.Log("is here");
            enemies[i].GetComponent<Health>()?.RemoveHealth(damage);
            Debug.Log(enemies[i].name);
        }
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public string GetTitle()
    {
        return title;
    }

    public void ResetCard()
    {
        hasActivated = false;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;
        if (timer >= attackDelay)
        {
            Effect();
            timer -= attackDelay;
        }

        if (hasActivated == false)
        {
            hasActivated = true;
            StartCoroutine(Attack());


        }



    }
    IEnumerator Attack()
    {
        target.GetComponentInChildren<CircleCollider2D>().enabled = true;
        target.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().enabled = true;
        yield return new WaitForSeconds(2);
        target.GetComponentInChildren<CircleCollider2D>().enabled = false;
        target.GetComponentInChildren<CircleCollider2D>().gameObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return null;
    }

}
