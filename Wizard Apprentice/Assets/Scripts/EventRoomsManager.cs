using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventRoomsManager : MonoBehaviour
{

    [SerializeField] public Button acceptButton;
    [SerializeField] public Button declineButton;
    [SerializeField] Reward reward;

    RoomManager roomManager;
    PlayerStats playerStats;
    
    void Start()
    {
        roomManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<RoomManager>();
        playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
    }

     public void TaskOnClickAccept()
    {
        Debug.Log("Accepted");
       // playerStats = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerStats>();
        playerStats.GiveStats(reward);
        ExitEvent();
    }

    public void TaskOnClickDecline()
    {
        Debug.Log("Declined");
        ExitEvent();
    }
    
    private void ExitEvent()
    {
        gameObject.SetActive(false);
       
        roomManager.PlayerWalkThroughDoor();
    }


}
