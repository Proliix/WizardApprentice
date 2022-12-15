using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLock : MonoBehaviour
{
    NormalCard normalCard;
    TrippleShotCard trippleShotCard;
    CircleShotCard circleShotCard;
    CardHandler cardHandler;

    AudioClip puzzleSuccess;

    void Start()
    {
        normalCard = new NormalCard();
        trippleShotCard = new TrippleShotCard();
        circleShotCard = new CircleShotCard();

        cardHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<CardHandler>();
    }

    void Update()
    {
        if (cardHandler.CheckInSlot(normalCard, 0) && cardHandler.CheckInSlot(trippleShotCard, 1) && cardHandler.CheckInSlot(circleShotCard, 2) && cardHandler.CheckInSlot(normalCard, 3))
        {
            
            SoundManager.Instance.PlayAudio(puzzleSuccess);
        }
       
    }
}
