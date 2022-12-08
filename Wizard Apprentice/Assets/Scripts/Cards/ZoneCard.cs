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
    [SerializeField] GameObject playerTarget;
    [SerializeField] GameObject zoneObject;

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
        player = GameObject.FindGameObjectWithTag("Player");
        zoneObject = GameObject.Find("ZoneCardActiveZone");
        enemyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
    }

    public void Effect()
    {
        //SoundManager.Instance.PlayAudio(attackSound);
        enemies = enemyManager.GetEnemiesWithinRange(player.transform.position, attackRange);
        for (int i = 0; i < enemies.Count; i++)
        {
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
            Vector3 scaleFix = Vector3.one - zoneObject.transform.parent.localScale;
            Vector3 newScale = (((Vector3.one + scaleFix) * attackRange) * 2);
            zoneObject.transform.localScale = newScale;
            hasActivated = true;
            StartCoroutine(Attack());

        }

    }
    IEnumerator Attack()
    {

        zoneObject.GetComponent<SpriteRenderer>().enabled = true;

        yield return new WaitForSeconds(2);

        zoneObject.GetComponent<SpriteRenderer>().enabled = false;
        yield return null;
    }

    private void OnDrawGizmos()
    {
        if (hasActivated)
        {
            Gizmos.DrawWireSphere(player.transform.position, attackRange);
        }
    }

}
