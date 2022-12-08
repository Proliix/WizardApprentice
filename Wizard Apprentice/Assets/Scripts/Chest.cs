using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    RewardsHandler rewardsHandler;

    private void Start()
    {
        rewardsHandler = GameObject.FindWithTag("GameController").GetComponent<RewardsHandler>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (rewardsHandler.CanAddCards())
            {
                rewardsHandler.GetRewardScreenCard();
            }
            else
            {
                Debug.LogError("ADD REMOVE CARD SCREEN HERE");
            }
        }
    }

}
