using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject cardHolderPrefab;
    public CardHandler cardHandler;
    public List<CardHolder> cardHolders = new List<CardHolder>();

    void Start()
    {
        for(int i = 0; i < cardHolders.Count; i++)
        {
            cardHolders[i].index = i;
        }
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
