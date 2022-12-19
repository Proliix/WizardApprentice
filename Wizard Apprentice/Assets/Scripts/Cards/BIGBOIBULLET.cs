using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIGBOIBULLET : MonoBehaviour, ICard
{
    [SerializeField] Sprite bulletSprite;
    [SerializeField] Sprite cardSprite;
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] float audioVolume = 1;

    [Header("Card Stats")]
    [SerializeField] float damage = 65;
    [SerializeField] float shootCooldown = 1;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float bulletSize = 10;
    [SerializeField] float lifeTime = 20;

    bool hasFired;
    float timer = 0;

    BulletHandler bulletHandler;
    Transform spawnpoint;
    GameObject player;
    PlayerStats stats;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        spawnpoint = player.GetComponent<PlayerAiming>().bulletSpawn.transform;
        stats = player.GetComponent<PlayerStats>();
        hasFired = false;
    }

    public void Effect()
    {
      //  bulletHandler.GetBullet(spawnpoint, player, true, false, stats.GetDamage(damage), bulletSize + stats.projectileSize, bulletSpeed + stats.projectileSpeed);
        bulletHandler.GetSpecialBullet(spawnpoint, player, bulletSprite, SpecialBulletState.WontHitWall, null, true,Vector3.zero, 0, lifeTime, false, stats.GetDamage(damage), bulletSize + stats.projectileSize, bulletSpeed + stats.projectileSpeed);
        SoundManager.Instance.PlayAudio(shootAudioClip, audioVolume);

    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return cardSprite;
    }

    public string GetTitle()
    {
        return title;
    }

    public void ResetCard()
    {
        timer = 0;
        hasFired = false;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if (timer >= stats.GetAttackSpeed(shootCooldown) && hasFired == false)
        {
            hasFired = true;
            timer -= shootCooldown;
            Effect();
        }
    }

  

    
}
