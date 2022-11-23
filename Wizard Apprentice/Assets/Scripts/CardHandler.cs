using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] float timePerCard = 2f;
    public GameObject[] cardObjs = new GameObject[4];
    [Header("UI")]
    public Image[] cardCycle = new Image[4];

    ICard[] cards;

    Animator[] animators;
    float timer = 0;
    public int cardIndex;


    // Start is called before the first frame update
    void Start()
    {
        cards = new ICard[cardObjs.Length];
        animators = new Animator[cardCycle.Length];

        for (int i = 0; i < cardCycle.Length; i++)
        {
            animators[i] = cardCycle[i].gameObject.GetComponent<Animator>();
        }
        animators[0].SetBool("IsActive", true);
        UpdateInterface();

    }

    public void ReplaceCard(GameObject card, int index)
    {
        if (card.GetComponent<ICard>() == null)
        {
            Debug.LogError("Replace card does not have a ICard script. Obj name: " + card.name);
        }
        else
        {
            cardObjs[index] = card;
            UpdateInterface();
        }
    }

    void UpdateInterface()
    {
        for (int i = 0; i < cardObjs.Length; i++)
        {
            if (cardObjs[i] != null)
            {
                cards[i] = cardObjs[i].GetComponent<ICard>();
                cardCycle[i] = cardObjs[i].GetComponent<Image>();
                animators[i] = cardCycle[i].gameObject.GetComponent<Animator>();
            }

        }

        for (int i = 0; i < cardCycle.Length; i++)
        {
            if (cards[i] != null)
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

            for (int i = 0; i < animators.Length; i++)
            {
                if (i == cardIndex)
                {
                    animators[cardIndex].SetBool("IsActive", true);
                }
                else
                {
                    animators[i].SetBool("IsActive", false);
                }
            }
        }
    }
}
