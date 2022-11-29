
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] float shootCooldown = 0.25f;

    [SerializeField] float damage = 10f;
    [SerializeField] float size = 0.5f;
    [SerializeField] float speed = 8f;

    BulletHandler bulletHandler;
    Transform spawnpoint;
    GameObject player;
    float timer = 10;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");

        spawnpoint = player.GetComponent<PlayerAiming>().bulletSpawn.transform;
    }

    public void Effect()
    {
        bulletHandler.GetBullet(spawnpoint, player, true, false, damage, size, speed);
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        timer = 10;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if (timer >= shootCooldown)
        {
            timer = 0;
            Effect();
        }

    }
}
