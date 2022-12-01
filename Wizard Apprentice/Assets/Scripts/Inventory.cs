using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject cardHolderPrefab;
    public CardHandler cardHandler;
    public List<CardHolder> cardHolders = new List<CardHolder>();
    [SerializeField] CardHolder trashCan;

    void Start()
    {
        for (int i = 0; i < cardHolders.Count; i++)
        {
            cardHolders[i].index = i;
        }
    }

    public void AddCard(GameObject cardPrefab)
    {
        if (!IsFull())
        {
            for (int i = 0; i < cardHolders.Count; i++)
            {
                if (cardHolders[i].cardObject == null)
                {
                    GameObject cardObject = Instantiate(cardPrefab, cardHolders[i].transform.parent);
                    cardObject.transform.localPosition = cardHolders[i].transform.localPosition;
                    cardObject.GetComponent<Drag>().canvas = cardHolders[i].transform.parent.gameObject.GetComponent<Canvas>();
                    cardObject.GetComponent<Drag>().inventory = this;
                    cardHolders[i].cardObject = cardObject;
                    cardObject.GetComponent<Drag>().lastObjectAttachedTo = cardHolders[i].gameObject;
                    break;
                }
            }
        }
        else
        {
            Debug.Log("IS FULL");
        }
    }

    public bool IsFull()
    {
        bool isfull = true;
        for (int i = 0; i < cardHolders.Count; i++)
        {
            if (cardHolders[i].cardObject == null)
            {
                isfull = false;
                break;
            }
        }
        return isfull;
    }

    public void ReplaceCard(GameObject cardHolder, GameObject cardObject)
    {
        for (int i = 0; i < Mathf.Min(cardHolders.Count, 4); i++)
        {
            if (cardHolders[i].gameObject == cardHolder)
            {
                cardHandler.ReplaceCard(cardObject, i);
                break;
            }
        }
    }

}
