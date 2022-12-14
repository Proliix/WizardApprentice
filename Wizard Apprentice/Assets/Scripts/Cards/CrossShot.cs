using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : MonoBehaviour, ICard
{

    [SerializeField] Sprite image;
    [SerializeField] Sprite comboImage;
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

    XShot xShot;

    PlayerStats stats;
    GameObject player;
    CardHandler cardHandler;
    BulletHandler bulletHandler;
    bool triedToFind = false;
    bool combined;
    float timer;

    private void Start()
    {


        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
        cardHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<CardHandler>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound);
        bulletHandler.GetCircleShot(4, player, true, stats.GetDamage(damage), size + stats.projectileSize, speed + stats.projectileSpeed);

        if (xShot != null)
        {
            xShot.Effect();
        }

    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        Sprite returnValue = image;

        if (combined)
        {
            returnValue = comboImage;
        }

        return returnValue;
    }

    public string GetTitle()
    {
        return title;
    }

    public void ResetCard()
    {
        timer = 0;
        triedToFind = false;
        xShot = null;
    }

    public void UpdateCard()
    {
        if (!triedToFind)
        {
            triedToFind = true;
            xShot = cardHandler.CheckInCycle<XShot>();
        }
        timer += Time.deltaTime;
        if (timer >= stats.GetAttackSpeed(attackDelay))
        {
            Effect();
            timer -= attackDelay;
        }
    }

}
