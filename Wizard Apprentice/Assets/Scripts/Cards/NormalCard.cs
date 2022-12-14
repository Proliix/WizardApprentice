
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] float shootCooldown = 0.25f;

    [SerializeField] float damage = 10f;
    [SerializeField] float size = 0.5f;
    [SerializeField] float speed = 8f;
    [SerializeField] AudioClip attackSound;

    BulletHandler bulletHandler;
    Transform spawnpoint;
    GameObject player;
    PlayerStats stats;
    float timer = 10;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();

        spawnpoint = player.GetComponent<PlayerAiming>().bulletSpawn.transform;
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound);
        bulletHandler.GetBullet(spawnpoint, player, true, false, stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);
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

        if (timer >= stats.GetAttackSpeed(shootCooldown))
        {
            timer = 0;
            Effect();
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
}
