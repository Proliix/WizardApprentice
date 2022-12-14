using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AurelionCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] Sprite bulletImage;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] float distance = 1.25f;
    [SerializeField] float lifetime = 2.25f;
    [SerializeField] float damage = 10f;
    [SerializeField] float size = 1.25f;
    [SerializeField] float speed = 10f;
    [SerializeField] float sinSpeed = 5f;
    [Range(1, 100)]
    [SerializeField] int amount = 4;
    [Tooltip("Shoot pos from player pos")]
    [SerializeField] Vector2 shootPos;

    bool hasFired = false;
    BulletHandler bulletHandler;
    GameObject player;
    PlayerStats stats;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound, audioVolume);
        for (float deg = 0; deg < 360; deg += 360f / (amount + stats.projectileAmount))
        {
            float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90));
            float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90));

            Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

            Vector3 spawnPos = player.transform.position + spawnDir;

            bulletHandler.GetSpecialBullet(spawnPos, player, bulletImage, SpecialBulletState.Rotating, this, true, sinSpeed,lifetime, false, stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed, distance + stats.projectileSize);
        }
    }

    public void UpdateCard()
    {
        if (!hasFired)
        {
            hasFired = true;
            Effect();
        }
    }

    public void ResetCard()
    {
        hasFired = false;
    }
    public Sprite GetSprite()
    {
        return image;
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
