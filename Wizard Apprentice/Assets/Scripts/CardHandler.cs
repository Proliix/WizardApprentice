using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] float timePerCard = 2f;
    [SerializeField] GameObject[] cardObjs = new GameObject[4];
    [Header("UI")]
    [SerializeField] Image[] cardCycle = new Image[4];

    ICard[] cards;

    float timer = 0;
    int cardIndex;


    // Start is called before the first frame update
    void Start()
    {
        cards = new ICard[cardObjs.Length];
        for (int i = 0; i < cardObjs.Length; i++)
        {
            if (cardObjs[i] != null)
                cards[i] = cardObjs[i].GetComponent<ICard>();

        }

        Debug.Log("Is here");

        for (int i = 0; i < cardCycle.Length; i++)
        {
            if (cards[i].GetSprite() != null)
                cardCycle[i].sprite = cards[i].GetSprite();
        }

    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        if (cards[cardIndex] != null)
            cards[cardIndex].UpdateCard();
        else
            Debug.LogError("CARD WITH INDEX " + cardIndex + " IS NULL");

        if (timer >= timePerCard + 0.05f)
        {
            timer = 0;

            if (cards[cardIndex] != null)
                cards[cardIndex].ResetCard();

            if (cardIndex < cardObjs.Length - 1)
                cardIndex++;
            else
                cardIndex = 0;

        }
    }
}
