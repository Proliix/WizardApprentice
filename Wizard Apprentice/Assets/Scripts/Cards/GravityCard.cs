using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [SerializeField] GameObject particleEffect;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] float damage = 5;
    [SerializeField] Sprite Bulletimage;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] float size = 1.25f;
    [SerializeField] float effectRange = 2f;
    [SerializeField] float effectCooldown = 0.25f;
    [SerializeField] float lifeTime = 4f;
    [SerializeField] float force = 2f;
    [SerializeField] float stunDuration = 0.15f;
    [SerializeField] int dealDamagerPerEffect = 3;

    bool hasParticles = false;
    GameObject particleInstance;
    bool hasFired = false;
    BulletHandler bulletHandler;
    EnemyManager enemyManager;
    GameObject gravityBullet;
    GameObject player;
    PlayerAiming pAim;
    PlayerStats stats;
    List<GameObject> enemiesWithingRange;
    int effectNum = 0;

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
        effectNum++;
        if (particleInstance != null && !particleInstance.activeSelf)
            particleInstance.SetActive(true);

        enemiesWithingRange = enemyManager.GetEnemiesWithinRange(gravityBullet.transform.position, effectRange + stats.projectileSize);
        for (int i = 0; i < enemiesWithingRange.Count; i++)
        {
            if (effectNum % dealDamagerPerEffect == 0)
                enemiesWithingRange[i].GetComponent<Health>()?.RemoveHealth(damage);

            if (enemiesWithingRange[i].GetComponent<IStunnable>() != null)
            {
                if (enemiesWithingRange[i].GetComponent<Rigidbody2D>() != null)
                {
                    Vector2 dir = gravityBullet.transform.position - enemiesWithingRange[i].transform.position;
                    enemiesWithingRange[i].GetComponent<IStunnable>().GetStunned(stunDuration);
                    enemiesWithingRange[i].GetComponent<Rigidbody2D>().velocity = dir;
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
        hasParticles = false;
    }

    public void UpdateCard()
    {
        if (!hasFired)
        {
            SoundManager.Instance.PlayAudio(attackSound, audioVolume);
            hasFired = true;
            gravityBullet = bulletHandler.GetSpecialBullet(pAim.bulletSpawn.transform, player, Bulletimage, SpecialBulletState.Static, this, true, "Blackhole", Vector3.zero, effectCooldown, lifeTime, false, 0, size + stats.projectileSize);
            if (!hasParticles)
            {
                hasParticles = true;
                effectNum = 0;
                particleInstance = Instantiate(particleEffect, gravityBullet.gameObject.transform);
                Vector3 newScale = ((Vector3.one * effectRange) / 2) - (Vector3.one * 0.1f);
                particleInstance.transform.localScale = newScale;
                particleInstance.SetActive(false);
                Destroy(particleInstance, lifeTime);
            }
        }
    }

    public string GetTitle()
    {
        return title;
    }

    public string GetDescription()
    {
        return description;
    }

    private void OnDrawGizmos()
    {
        if (particleInstance != null)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(gravityBullet.transform.position, effectRange);
        }
    }
}
