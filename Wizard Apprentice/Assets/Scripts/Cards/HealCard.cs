using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] GameObject player;
    [SerializeField] int healAmmount = 1;
    [SerializeField] AudioClip healSound;
    [SerializeField] float audioVolume = 0.25f;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] Sprite bulletSprite;
    bool playerIsHealed = false;
    Health health;
    BulletHandler bulletHandler;
    Transform playerAimObj;
    PlayerStats stats;

    float timer = 0;

    private void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        playerAimObj = GameObject.FindWithTag("Player").GetComponent<PlayerAiming>().bulletSpawn.transform;
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(healSound, audioVolume);
        health.AddHealth(healAmmount);
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        playerIsHealed = false;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if (timer >= stats.GetAttackSpeed(shootCooldown))
        {
            timer = 0;
            bulletHandler.GetSpecialBullet(playerAimObj, player, bulletSprite, SpecialBulletState.Onhit, this, true, Vector3.zero, 0, 10, false, healAmmount);
        }


        //if (playerIsHealed == false)
        //{
        //    playerIsHealed = true;
        //    Effect();
        //}
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
