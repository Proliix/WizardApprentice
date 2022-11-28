using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] Sprite Bulletimage;
    [SerializeField] float effectRange = 2f;
    [SerializeField] float effectCooldown = 0.5f;
    [SerializeField] float lifeTime = 4f;



    bool hasFired = false;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    GameObject gravityBullet;
    GameObject player;
    List<GameObject> enemiesWithingRange;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        player = GameObject.FindWithTag("Player");
    }
    public void Effect()
    {
        enemiesWithingRange = enemyManager.GetEnemiesWithinRange(gravityBullet.transform.position, effectRange);
        for (int i = 0; i < enemiesWithingRange.Count; i++)
        {
            if (enemiesWithingRange[i].GetComponent<Rigidbody2D>() != null)
            {
                Vector2 dir = gravityBullet.transform.position - enemiesWithingRange[i].transform.position;
                enemiesWithingRange[i].GetComponent<Rigidbody2D>().AddForce(dir);
            }
        }
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        hasFired = false;
    }

    public void UpdateCard()
    {
        if (!hasFired)
        {
            hasFired = true;
            gravityBullet = bulletHandler.GetSpecialBullet(player.transform.position, gameObject, Bulletimage, SpecialBulletState.Static, this, true, effectCooldown, lifeTime);
        }
    }
}
