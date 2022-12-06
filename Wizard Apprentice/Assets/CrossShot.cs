using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossShot : MonoBehaviour, ICard
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


    PlayerStats stats;
    GameObject player;
    BulletHandler bulletHandler;
    float timer;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
        stats = player.GetComponent<PlayerStats>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(attackSound);

       
        bulletHandler.GetCircleShot(4, player, true, 45, damage, size, speed);
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
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;
        if (timer >= attackDelay)
        {
            Effect();
            timer -= attackDelay;
        }
    }


}
