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
    bool playerIsHealed = false;
    Health health;
    NormalCard testCard = new NormalCard();
    CardHandler cardHandler;


    private void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        cardHandler = GameObject.FindWithTag("GameController").GetComponent<CardHandler>();
    }

    public void Effect()
    {
        SoundManager.Instance.PlayAudio(healSound, audioVolume);
        health.AddHealth(healAmmount);

        if (cardHandler.CheckInCycle(testCard) != null)
        {
            Debug.Log("Is here");
            testCard = (NormalCard)cardHandler.CheckInCycle(testCard);
            testCard.Effect();
        }
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
        if (playerIsHealed == false)
        {
            playerIsHealed = true;
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
