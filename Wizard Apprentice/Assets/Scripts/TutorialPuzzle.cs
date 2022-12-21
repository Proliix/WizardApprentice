using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialPuzzle : MonoBehaviour
{


    [SerializeField] float timer;
    [SerializeField] bool hasEntered = false;

    NormalCard normalCard;
    TrippleShotCard trippleShotCard;
    CircleShotCard circleShotCard;
    CardHandler cardHandler;

   [SerializeField] AudioClip puzzleSuccess;
    private bool puzzleDone = false;

    void Start()
    {
        gameObject.GetComponent<CapsuleCollider2D>().enabled = true;
        normalCard = new NormalCard();
        trippleShotCard = new TrippleShotCard();
        circleShotCard = new CircleShotCard();
        
        cardHandler = GameObject.FindGameObjectWithTag("GameController").GetComponent<CardHandler>();
    }

    void Update()
    {

        if (hasEntered == true)
        {
            timer += Time.deltaTime;
        }

        if (timer >= 30 && timer < 60)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = "You can rearrange the spellcards";
            
        }

        if (timer >= 60)
        {
            gameObject.GetComponentInChildren<TextMeshPro>().text = "Use Mouse 1 to rearrange the 4 cards as shown on the sign";
            
        }



        if (cardHandler.CheckInSlot(normalCard, 0) && cardHandler.CheckInSlot(trippleShotCard, 1) && cardHandler.CheckInSlot(circleShotCard, 2) && cardHandler.CheckInSlot(normalCard, 3) && !puzzleDone)
        {
            
            gameObject.transform.GetChild(0).gameObject.SetActive(false);
            SoundManager.Instance.PlayAudio(puzzleSuccess);
            puzzleDone = true;
            
        }

    }

    private void OnTriggerEnter2D(Collider2D collision2D)
    {
        if (collision2D.CompareTag("Player"))
        {
            hasEntered = true;
        }
    }
}
