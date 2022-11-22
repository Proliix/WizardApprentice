using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] GameObject player;
    [SerializeField] int healAmmount = 1;

    bool playerIsHealed = false;
    Health health;

    private void Start()
    {
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
    }

    public void Effect()
    {
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
        if (playerIsHealed == false)
        {
            playerIsHealed = true;
            Effect();
        }
    }

}
