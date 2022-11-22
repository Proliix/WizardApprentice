using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] GameObject player;

    bool playerIsHealed = false;

    public void Effect()
    {
        Debug.Log("Player is Healed");
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
