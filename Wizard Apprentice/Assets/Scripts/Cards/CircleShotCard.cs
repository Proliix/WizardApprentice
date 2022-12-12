using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShotCard : MonoBehaviour, ICard
{

    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] int projectileAmmount = 6;
    [SerializeField] float shootCooldown = 0.5f;
    [SerializeField] float damage = 10f;
    [SerializeField] float bulletSize = 0.5f;
    [SerializeField] float speed = 8;

    BulletHandler bulletHandler;
    GameObject player;
    PlayerStats stats;
    float timer = 10;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }
    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound, audioVolume);

        bulletHandler.GetCircleShot(projectileAmmount + stats.projectileAmount, player, true, stats.GetDamage(damage), bulletSize + stats.projectileSize, speed + stats.projectileSpeed);
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
