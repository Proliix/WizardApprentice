using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject cardHolderPrefab;
    [SerializeField] CardHandler cardHandler;
    public List<CardHolder> cardHolders = new List<CardHolder>();

    void Start()
    {

    }

    public void ReplaceCard(GameObject cardHolder, GameObject cardObject)
    {
        for(int i = 0; i < cardHolders.Count; i++)
        {
            if(cardHolders[i].gameObject == cardHolder)
            {
                cardHandler.ReplaceCard(cardObject,i);
                break;
            }
        }
    }

}
