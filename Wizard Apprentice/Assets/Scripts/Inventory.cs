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
        for(int i = 0; i < cardHolders.Count; i++)
        {
            cardHolders[i].index = i;
        }
    }

    public void AddCard(GameObject cardObject)
    {
        if(!IsFull())
        {
            for(int i = 0; i < cardHolders.Count; i++)
            {
                if (cardHolders[i].cardObject == null)
                {
                    cardHolders[i].cardObject = cardObject;
                    cardObject.GetComponent<Drag>().lastObjectAttachedTo = cardHolders[i].gameObject;
                    break;
                }
            }
        }
    }

    public bool IsFull()
    {
        for(int i = 0; i < cardHolders.Count; i++)
        {
            if (cardHolders[i].cardObject == null)
            {
                return true;
            }
        }
        return false;
    }

    public void ReplaceCard(GameObject cardHolder, GameObject cardObject)
    {
        for(int i = 0; i < Mathf.Min(cardHolders.Count,4); i++)
        {
            if(cardHolders[i].gameObject == cardHolder)
            {
                cardHandler.ReplaceCard(cardObject, i);
                break;
            }
        }
    }

}
