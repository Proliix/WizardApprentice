using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XShot : MonoBehaviour, ICard
{


    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;

    [Header("Card stats")]
    [SerializeField] float damage = 10f;
    [SerializeField] float size = 0.5f;
    [SerializeField] float speed = 8f;

    [Header("Attack Variables")]
    float attackDelay = 0.5f;

    CrossShot crossShot;

    bool triedToFind = false;
    CardHandler cardHandler;
    PlayerStats stats;
    GameObject player;
    BulletHandler bulletHandler;
    float timer;

    private void Start()
    {
        cardHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<CardHandler>();
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound);
        bulletHandler.GetCircleShot(4, player, true, 45, stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);

        if (crossShot != null)
        {

            crossShot.Effect();
        }
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public string GetTitle()
    {
        return title;
    }

    public void ResetCard()
    {
        timer = 0;
        triedToFind = false;
        crossShot = null;
    }

    public void UpdateCard()
    {
        if (!triedToFind)
        {
            triedToFind = true;
            CrossShot temp = new CrossShot();

            crossShot = (CrossShot)cardHandler.CheckInCycle(temp);
        }

        timer += Time.deltaTime;
        if (timer >= stats.GetAttackSpeed(attackDelay))
        {
            Effect();
            timer -= attackDelay;
        }
    }



}
