using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

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
    LaserHurtBox laserHurtBox;
    Light2D zoneLight;

    List<GameObject> enemies;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        stats = player.GetComponent<PlayerStats>();

        for (int i = 0; i < player.transform.childCount; i++)
        {
            if(player.transform.GetChild(i).GetComponent<LaserHurtBox>() != null)
            {
                zoneObject = player.transform.GetChild(i).gameObject;
                break;
            }
        }
        laserHurtBox = zoneObject.GetComponent<LaserHurtBox>();
        //zoneObject = laserHurtBox.gameObject;
        zoneLight = zoneObject.GetComponent<Light2D>();
        enemyManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<EnemyManager>();
        zoneObject.SetActive(false);
        laserHurtBox.attackDelay = attackDelay;
        laserHurtBox.damage = damage;
    }

    public void Effect()
    {
        //SoundManager.Instance.PlayAudio(attackSound);
        //enemies = enemyManager.GetEnemiesWithinRange(player.transform.position, attackRange);
        //for (int i = 0; i < enemies.Count; i++)
        //{
        //    enemies[i].GetComponent<Health>()?.RemoveHealth(stats.GetDamage(damage));
        //    Debug.Log(enemies[i].name);
        //}
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
        zoneObject.SetActive(false);
    }

    public void UpdateCard()
    {
        //timer += Time.deltaTime;
        //if (timer >= stats.GetAttackSpeed(attackDelay))
        //{
        //    Effect();
        //    timer = 0;
        //}

        if (hasActivated == false)
        {
            Vector3 scaleFix = Vector3.one - zoneObject.transform.parent.localScale;
            Vector3 newScale = (((Vector3.one + scaleFix) * attackRange) * 2);
            zoneObject.transform.localScale = newScale;
            hasActivated = true;
            zoneObject.SetActive(true);
            zoneLight.pointLightOuterRadius = zoneObject.transform.localScale.x / 2;
            zoneLight.pointLightInnerRadius = zoneObject.transform.localScale.x / 3;

        }

    }

    //private void OnDrawGizmos()
    //{
    //    if (hasActivated)
    //    {
    //        Gizmos.DrawWireSphere(player.transform.position, attackRange);
    //    }
    //}

}
