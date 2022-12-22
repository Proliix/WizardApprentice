using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] Sprite Opened;


    RewardsHandler rewardsHandler;
    SpriteRenderer sr;
    bool hasShown = false;

    private void Start()
    {
        rewardsHandler = GameObject.FindWithTag("GameController").GetComponent<RewardsHandler>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    void OpenChest()
    {
        hasShown = true;
        sr.sprite = Opened;
        rewardsHandler.GetRewardScreenCard();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(!hasShown)
                OpenChest();
        }
    }

}


