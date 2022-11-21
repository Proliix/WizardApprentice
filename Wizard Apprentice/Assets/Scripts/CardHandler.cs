using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardHandler : MonoBehaviour
{
    public float timePerCard = 2f;
    public GameObject[] cardObjs = new GameObject[4];

    ICard[] cards;

    float timer = 0;
    int cardIndex;


    // Start is called before the first frame update
    void Start()
    {
        cards = new ICard[cardObjs.Length];
        for (int i = 0; i < cardObjs.Length; i++)
        {
            cards[i] = cardObjs[i].GetComponent<ICard>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log("Card index: " + cardIndex);
        timer += Time.deltaTime;

        if (cards[cardIndex] != null)
            cards[cardIndex].UpdateCard();
        else
            Debug.LogError("ONE OF THE CARDS IS NULL");

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
