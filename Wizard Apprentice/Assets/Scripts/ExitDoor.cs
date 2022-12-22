using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitDoor : MonoBehaviour
{
    RoomManager roomManager;
    [SerializeField] ExitTutorialHeal exitTutorialHeak;
    Health health;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            roomManager = GameObject.FindWithTag("GameController").GetComponent<RoomManager>();
            roomManager.PlayerWalkThroughDoor();
        }
    }

    private void FullHealPlayer()
    {
        //health = FindObjectOfType("")
    }
}
