using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BIGBOIBULLET : MonoBehaviour, ICard
{
    [SerializeField] Sprite bulletSprite;
    [SerializeField] Sprite cardSprite;
    [SerializeField] string title;
    [SerializeField] string description;
    [SerializeField] AudioClip shootAudioClip;
    [SerializeField] float audioVolume = 1;

    [Header("Card Stats")]
    [SerializeField] float startingDamage = 65;
    [SerializeField] float maxDamage = 70;
    [SerializeField] float shootCooldown = 1;
    [SerializeField] float bulletSpeed = 15;
    [SerializeField] float bulletSize = 10;
    [SerializeField] float lifeTime = 20;
    [SerializeField] float scaleSpeed = 0.5f;
    [SerializeField] float maxScale = 20.0f;


    float timer = 0;
    float elapsedTime = 0.0f;

    BulletHandler bulletHandler;
    Transform spawnpoint;
    GameObject player;
    PlayerStats stats;
    GameObject activeBullet;

    Light2D bulletLight;

    SpecialProjectile bulletScript;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        bulletHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<BulletHandler>();
        spawnpoint = player.GetComponent<PlayerAiming>().bulletSpawn.transform;
        stats = player.GetComponent<PlayerStats>();
    }

    public void Effect()
    {
        //  bulletHandler.GetBullet(spawnpoint, player, true, false, stats.GetDamage(damage), bulletSize + stats.projectileSize, bulletSpeed + stats.projectileSpeed);
        activeBullet = bulletHandler.GetSpecialBullet(spawnpoint, player, bulletSprite, SpecialBulletState.WontHitWall, null, true, Vector3.zero, 0, lifeTime, false, stats.GetDamage(startingDamage), bulletSize + stats.projectileSize, bulletSpeed + stats.projectileSpeed);
        bulletLight = activeBullet.GetComponent<Light2D>();
        SoundManager.Instance.PlayAudio(shootAudioClip, audioVolume);
        bulletScript = activeBullet.GetComponent<SpecialProjectile>();
        elapsedTime = 0;
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

    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;


        if (timer >= stats.GetAttackSpeed(shootCooldown))
        {

            timer -= shootCooldown;
            Effect();
        }


    }
    private void Update()
    {
        elapsedTime += Time.deltaTime;
        float interpolationAmount = elapsedTime / scaleSpeed;
        interpolationAmount = Mathf.Clamp01(interpolationAmount);

        if (activeBullet != null)
        {
            activeBullet.gameObject.transform.localScale = Vector3.MoveTowards(activeBullet.gameObject.transform.localScale, new Vector3(maxScale, maxScale, 1), scaleSpeed * Time.deltaTime);
            if (activeBullet.activeSelf == false)
            {
                activeBullet = null;
            }

        }

        if (bulletScript != null)
        {
            bulletScript.damage = Mathf.Lerp(startingDamage, maxDamage, (activeBullet.gameObject.transform.localScale.y - bulletSize) / (maxScale - bulletSize));
            bulletLight.pointLightOuterRadius = activeBullet.transform.localScale.x * 4;
            Debug.Log(bulletScript.damage);
            Debug.Log(interpolationAmount);
        }
    }



}
