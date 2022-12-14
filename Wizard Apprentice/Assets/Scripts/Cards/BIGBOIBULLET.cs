using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BIGBOIBULLET : MonoBehaviour, ICard
{

    [SerializeField] Sprite cardSprite;
    [SerializeField] string title;
    [SerializeField] string description;

    [Header("Card Stats")]
    [SerializeField] float damage = 65;
    [SerializeField] float shootCooldown = 2;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float bulletSize = 10;

    bool hasFired;
    float timer = 1;

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
        bulletHandler.GetBullet(spawnpoint, player, true, false, stats.GetDamage(damage), bulletSize + stats.projectileSize, bulletSpeed + stats.projectileSpeed); 
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
        timer = 1;
        hasFired = false;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if (timer >= shootCooldown && hasFired == false)
        {
            hasFired = true;
            timer -= shootCooldown;
            Effect();
        }
    }

  

    
}
