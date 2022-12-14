using Newtonsoft.Json.Bson;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    [SerializeField] GameObject cardHolderPrefab;
    public CardHandler cardHandler;
    public List<CardHolder> cardHolders = new List<CardHolder>();
    public CardHolder trashCan;
    [SerializeField] GameObject cardDescriptionObject;
    [SerializeField] TextMeshProUGUI cardTitleText;
    [SerializeField] TextMeshProUGUI cardDescriptionText;
    [SerializeField] Canvas cardCanvas;

    public delegate void CardRemovedDelegate();
    public event CardRemovedDelegate cardRemovedEvent;

    bool trashcanAlwaysOn;
    bool trashCanOff;

    void Start()
    {
        for (int i = 0; i < cardHolders.Count; i++)
        {
            cardHolders[i].index = i;
        }
    }

    public void TrashCanIsOff(bool value)
    {
        trashCanOff = value;
    }

    public bool GetTrashCanIsOff()
    {
        return trashCanOff;
    }

    public int GetInvAmount()
    {
        int returnValue = 0;

        for (int i = 0; i < cardHolders.Count; i++)
        {
            if (cardHolders[i].cardObject != null)
            {
                returnValue++;
            }
        }

        return returnValue;
    }

    public void CardDestroyed()
    {
        if (cardRemovedEvent != null)
            cardRemovedEvent.Invoke();
    }

    public void TurnOnTrashcan()
    {
        if (GetInvAmount() > 1)
        {
            trashcanAlwaysOn = true;
            trashCan.gameObject.SetActive(true);
        }
    }
    public void TurnOffTrashcan()
    {
        trashcanAlwaysOn = false;
        trashCan.gameObject.SetActive(false);
    }

    public bool GetTrashcanAlwaysOnStatus()
    {
        return trashcanAlwaysOn;
    }

    public void ResetCardsPos()
    {
        for (int i = 0; i < cardHolders.Count; i++)
        {
            if (cardHolders[i].cardObject != null)
                cardHolders[i].cardObject.transform.position = cardHolders[i].transform.position;
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
                    cardObject.GetComponent<Drag>().canvas = cardCanvas;
                    cardObject.GetComponent<Drag>().inventory = this;
                    cardHolders[i].cardObject = cardObject;
                    cardObject.GetComponent<Drag>().lastObjectAttachedTo = cardHolders[i].gameObject;
                    if (i < 4)
                    {
                        cardHandler.cardObjs[i] = cardObject;
                        cardHandler.UpdateInterface();
                    }
                    break;
                }
            }
        }
        else
        {
            Debug.Log("IS FULL");
        }
    }

    public void ShowDescriptionObject()
    {
        cardDescriptionObject.SetActive(true);
    }

    public void ShowDescriptionObject(string cardTitle, string cardDescription, Vector2 position)
    {
        cardDescriptionObject.transform.localPosition = position;
        cardTitleText.text = cardTitle;
        cardDescriptionText.text = cardDescription;
        cardDescriptionObject.SetActive(true);
    }
    public void ShowDescriptionObject(string cardTitle, string cardDescription, Vector2 position, bool adjustToLeft, Vector2 adjustAmount)
    {
        cardDescriptionObject.transform.localPosition = position + (new Vector2(0, (cardDescriptionObject.GetComponent<RectTransform>().sizeDelta.y / 2)) + adjustAmount);
        cardTitleText.text = cardTitle;
        cardDescriptionText.text = cardDescription;
        cardDescriptionObject.SetActive(true);
    }

    public void HideDescriptionObject()
    {
        cardDescriptionObject.SetActive(false);
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

    public void AddQueuedCards(GameObject cardToSwap, int indexToSwap)
    {
        cardHandler.AddQueuedCards(cardToSwap, indexToSwap);
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
