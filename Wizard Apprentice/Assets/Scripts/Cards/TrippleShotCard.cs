using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippleShotCard : MonoBehaviour, ICard
{

    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] float shootPosDeviation = 0.25f;
    [SerializeField] float damage = 10f;
    [SerializeField] float size = 0.5f;
    [SerializeField] float speed = 8f;

    BulletHandler bulletHandler;
    GameObject player;
    PlayerStats stats;
    Transform spawnpoint;
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
        SoundManager.Instance.PlayAudio(attackSound, audioVolume);

        bulletHandler.GetBullet(spawnpoint, player, true, false, stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);
        bulletHandler.GetBullet(spawnpoint, player, true, true, (Vector3.right * shootPosDeviation), stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);
        bulletHandler.GetBullet(spawnpoint, player, true, true, -(Vector3.right * shootPosDeviation), stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);
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
