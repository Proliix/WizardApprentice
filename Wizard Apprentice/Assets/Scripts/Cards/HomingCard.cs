using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] Sprite Bulletimage;
    [SerializeField] float damage = 5f;
    [SerializeField] float size = 0.5f;
    [SerializeField] float speed = 8f;
    [SerializeField] float effectCooldown = 0.5f;
    [SerializeField] float lifeTime = 4f;


    BulletHandler bulletHandler;
    GameObject player;
    PlayerStats stats;
    Transform spawnpoint;
    float timer;

    private void Start()
    {

        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        spawnpoint = player.GetComponent<PlayerAiming>().bulletSpawn.transform;
    }

    public void Effect()
    {
        bulletHandler.GetSpecialBullet(spawnpoint, gameObject, Bulletimage, SpecialBulletState.Homing, this, true, Vector3.zero,0,lifeTime,false,damage + stats.damage,size + stats.projectileSize,speed + stats.projectileSpeed);
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        timer = 0;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if (timer >= effectCooldown)
        {
            timer = 0;
            Effect();
        }
    }


}
