using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCard : MonoBehaviour, ICard
{
    public GameObject player;

    bool playerIsHealed = false;

    public void Effect()
    {
        Debug.Log("Player is Healed");
    }

    public void ResetCard()
    {
        playerIsHealed = false;
    }

    public void UpdateCard()
    {
        Debug.Log("Is here");
        if (playerIsHealed == false)
        {
            playerIsHealed = true;
            Effect();
        }
    }

}
