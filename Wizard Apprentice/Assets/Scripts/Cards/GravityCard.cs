using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] Sprite Bulletimage;
    [SerializeField] float size = 1.25f;
    [SerializeField] float effectRange = 2f;
    [SerializeField] float effectCooldown = 0.5f;
    [SerializeField] float lifeTime = 4f;
    [SerializeField] float force = 2f;
    [SerializeField] float stunDuration = 0.15f;



    bool hasFired = false;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    GameObject gravityBullet;
    GameObject player;
    PlayerAiming pAim;
    PlayerStats stats;
    List<GameObject> enemiesWithingRange;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        enemyManager = GameObject.FindWithTag("GameController").GetComponent<EnemyManager>();
        player = GameObject.FindWithTag("Player");
        pAim = player.GetComponent<PlayerAiming>();
        stats = player.GetComponent<PlayerStats>();
    }
    public void Effect()
    {
        enemiesWithingRange = enemyManager.GetEnemiesWithinRange(gravityBullet.transform.position, effectRange + stats.projectileSize);
        for (int i = 0; i < enemiesWithingRange.Count; i++)
        {
            if (enemiesWithingRange[i].GetComponent<IStunnable>() != null)
            {
                if (enemiesWithingRange[i].GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 dir = gravityBullet.transform.position - enemiesWithingRange[i].transform.position;
                    enemiesWithingRange[i].GetComponent<IStunnable>().GetStunned(stunDuration);
                    //enemiesWithingRange[i].GetComponent<Rigidbody2D>().velocity += dir.normalized * force;
                    enemiesWithingRange[i].GetComponent<Rigidbody2D>().AddForce(dir * force, ForceMode2D.Impulse);
                    Debug.Log("Gravity card effect Triggered");
                }
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
            gravityBullet = bulletHandler.GetSpecialBullet(pAim.bulletSpawn.transform.position, player, Bulletimage, SpecialBulletState.Static, this, true, "Blackhole", effectCooldown, lifeTime, false, 0, size + stats.projectileSize);
        }
    }
}
