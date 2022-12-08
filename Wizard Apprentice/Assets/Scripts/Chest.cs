using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    [SerializeField] GameObject textCanvas;
    [SerializeField] Sprite Opened;


    RewardsHandler rewardsHandler;
    bool showingCards = false;
    bool inTrigger = false;
    SpriteRenderer sr;

    private void Start()
    {
        rewardsHandler = GameObject.FindWithTag("GameController").GetComponent<RewardsHandler>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        textCanvas.SetActive(!rewardsHandler.CanAddCards());
    }

    private void Update()
    {
        if (!showingCards)
            textCanvas.SetActive(!rewardsHandler.CanAddCards());

        if (inTrigger)
            if (rewardsHandler.CanAddCards())
                OpenChest();
    }

    void OpenChest()
    {
        showingCards = true;
        sr.sprite = Opened;
        rewardsHandler.GetRewardScreenCard();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            inTrigger = true;
            if (rewardsHandler.CanAddCards() && !showingCards)
            {
                OpenChest();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            inTrigger = false;

        }
    }


}


