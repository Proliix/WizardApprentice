using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AurelionCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] Sprite bulletImage;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] float distance = 1.25f;
    [SerializeField] float lifetime = 2.25f;
    [SerializeField] float damage = 10f;
    [SerializeField] float size = 1.25f;
    [SerializeField] float speed = 10f;
    [Range(1, 100)]
    [SerializeField] int amount = 4;
    [Tooltip("Shoot pos from player pos")]
    [SerializeField] Vector2 shootPos;

    float timer = 10;
    BulletHandler bulletHandler;
    GameObject player;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
    }

    public void Effect()
    {
        for (float deg = 0; deg < 360; deg += 360f / amount)
        {
            float vertical = Mathf.Sin(Mathf.Deg2Rad * (deg + 90));
            float horizontal = Mathf.Cos(Mathf.Deg2Rad * (deg + 90));

            Vector3 spawnDir = new Vector3(horizontal, vertical, 0);

            Vector3 spawnPos = player.transform.position + spawnDir;

            bulletHandler.GetSpecialBullet(player.transform.position + spawnDir, player, bulletImage, SpecialBulletState.Rotating, this, true, 0, lifetime, false, damage, size, speed, distance);
        }

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

    public void ResetCard()
    {
        timer = 10;
    }
    public Sprite GetSprite()
    {
        return image;
    }
}
