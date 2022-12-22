using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour
{
    public Canvas canvas;
    Vector2 offset;
    public Inventory inventory;
    public GameObject lastObjectAttachedTo;
    float cardHoverScale = 1.25f;
    float timeToOpenDescriptionMenu = 0.5f;
    public bool isInSwapQueue;
    public int swapPartner;

    Coroutine currentCoroutine;

    public List<int> queuedUpSwaps;

    private void Start()
    {
        queuedUpSwaps = new List<int>();
    }
    public void DragHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        Vector2 position;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerEventData.position, canvas.worldCamera, out position);

        transform.position = canvas.transform.TransformPoint(position + offset);
        for (int i = 0; i < inventory.cardHolders.Count; i++)
        {
            inventory.cardHolders[i].transform.localScale = new Vector3(1f, 1f, 1);
        }
        inventory.trashCan.transform.localScale = new Vector3(1f, 1f, 1f);
        int unitIndex = 0;
        float largestCover = -999990;
        for (int i = 0; i < inventory.cardHolders.Count; i++)
        {
            float cover = (UnitsHoveringOther(transform.position, new Vector2(transform.GetComponent<RectTransform>().rect.width * transform.localScale.x, transform.GetComponent<RectTransform>().rect.height * transform.localScale.y), inventory.cardHolders[i].transform.position, new Vector2(inventory.cardHolders[i].GetComponent<RectTransform>().rect.width * transform.localScale.x, inventory.cardHolders[i].GetComponent<RectTransform>().rect.height * transform.localScale.y)));
            if (cover > largestCover)
            {
                largestCover = cover;
                unitIndex = i;
            }
        }
        float trashCancover = (UnitsHoveringOther(transform.position, new Vector2(transform.GetComponent<RectTransform>().rect.width * transform.localScale.x, transform.GetComponent<RectTransform>().rect.height * transform.localScale.y), inventory.trashCan.transform.position, new Vector2(inventory.trashCan.GetComponent<RectTransform>().rect.width * transform.localScale.x, inventory.trashCan.GetComponent<RectTransform>().rect.height * transform.localScale.y)));
        if (trashCancover > largestCover)
        {
            inventory.trashCan.transform.localScale = new Vector3(cardHoverScale, cardHoverScale, 1);
        }
        else if (largestCover > 0)
        {
            inventory.cardHolders[unitIndex].transform.localScale = new Vector3(cardHoverScale, cardHoverScale, 1);
        }
        inventory.trashCan.gameObject.SetActive(true);
    }

    public void PointerDownHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        Vector2 mousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, pointerEventData.position, canvas.worldCamera, out mousePos);
        Vector2 objectPos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)canvas.transform, transform.position, canvas.worldCamera, out objectPos);

        offset = objectPos - mousePos;
        int unitIndex = 0;
        float largestCover = -999990;
        for (int i = 0; i < inventory.cardHolders.Count; i++)
        {
            float cover = (UnitsHoveringOther(transform.position, new Vector2(transform.GetComponent<RectTransform>().rect.width * transform.localScale.x, transform.GetComponent<RectTransform>().rect.height * transform.localScale.y), inventory.cardHolders[i].transform.position, new Vector2(inventory.cardHolders[i].GetComponent<RectTransform>().rect.width * transform.localScale.x, inventory.cardHolders[i].GetComponent<RectTransform>().rect.height * transform.localScale.y)));
            if (cover > largestCover)
            {
                largestCover = cover;
                unitIndex = i;
            }
        }
        transform.position = inventory.cardHolders[unitIndex].transform.position;
    }

    public void PointerUpHandler(BaseEventData data)
    {
        PointerEventData pointerEventData = (PointerEventData)data;
        bool hasSnappedToNew = false;
        int unitIndex = 0;
        float largestCover = -999990;
        for (int i = 0; i < inventory.cardHolders.Count; i++)
        {
            float cover = UnitsHoveringOther(transform.position, new Vector2(transform.GetComponent<RectTransform>().rect.width * transform.localScale.x, transform.GetComponent<RectTransform>().rect.height * transform.localScale.y), inventory.cardHolders[i].transform.position, new Vector2(inventory.cardHolders[i].GetComponent<RectTransform>().rect.width * transform.localScale.x, inventory.cardHolders[i].GetComponent<RectTransform>().rect.height * transform.localScale.y));
            if (cover > largestCover)
            {
                largestCover = cover;
                unitIndex = i;
            }
            inventory.cardHolders[i].transform.localScale = new Vector3(1, 1, 1);
        }
        inventory.trashCan.transform.localScale = new Vector3(1f, 1f, 1f);
        float trashCancover = (UnitsHoveringOther(transform.position, new Vector2(transform.GetComponent<RectTransform>().rect.width * transform.localScale.x, transform.GetComponent<RectTransform>().rect.height * transform.localScale.y), inventory.trashCan.transform.position, new Vector2(inventory.trashCan.GetComponent<RectTransform>().rect.width * transform.localScale.x, inventory.trashCan.GetComponent<RectTransform>().rect.height * transform.localScale.y)));
        if (trashCancover > largestCover)
        {
            for (int i = 0; i < inventory.cardHolders.Count; i++)
            {
                if (inventory.cardHolders[i].gameObject == lastObjectAttachedTo)
                {
                    if (inventory.cardHandler.cardIndex != GetMyIndex())
                    {
                        if (isInSwapQueue)
                        {
                            ResetQueuedCards();
                            inventory.cardHolders[swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                            ResetThisCard();
                        }
                        lastObjectAttachedTo.GetComponent<CardHolder>().cardObject = null;
                        Destroy(this.gameObject);
                    }
                    break;
                }
            }

        }
        else if (largestCover > 0)
        {
            //Set my position to new
            //Set other position to old
            //Set old object to other
            //Set new objcet to me
            if (unitIndex != inventory.cardHandler.cardIndex && lastObjectAttachedTo.GetComponent<CardHolder>().index != inventory.cardHandler.cardIndex)
            {
                if (inventory.cardHolders[unitIndex].cardObject != null)
                {
                    if (isInSwapQueue)
                    {
                        ResetQueuedCards();
                        inventory.cardHolders[swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                        ResetThisCard();
                    }
                    if (inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().isInSwapQueue)
                    {
                        inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetQueuedCards();
                        inventory.cardHolders[inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                        inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetThisCard();
                    }
                    transform.position = inventory.cardHolders[unitIndex].transform.position;
                    inventory.ReplaceCard(inventory.cardHolders[unitIndex].gameObject, this.gameObject);
                    inventory.cardHolders[unitIndex].cardObject.transform.position = lastObjectAttachedTo.transform.position;
                    inventory.ReplaceCard(lastObjectAttachedTo, inventory.cardHolders[unitIndex].GetComponent<CardHolder>().cardObject);
                    lastObjectAttachedTo.GetComponent<CardHolder>().cardObject = inventory.cardHolders[unitIndex].cardObject;
                    lastObjectAttachedTo.GetComponent<CardHolder>().cardObject.GetComponent<Drag>().lastObjectAttachedTo = lastObjectAttachedTo;
                    lastObjectAttachedTo = inventory.cardHolders[unitIndex].gameObject;
                    inventory.cardHolders[unitIndex].cardObject = this.gameObject;
                    hasSnappedToNew = true;
                }
                else
                {
                    if (isInSwapQueue)
                    {
                        ResetQueuedCards();
                        inventory.cardHolders[swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                        ResetThisCard();
                    }
                    if (inventory.cardHolders[unitIndex].cardObject != null && inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().isInSwapQueue)
                    {
                        inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetQueuedCards();
                        inventory.cardHolders[inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                        inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetThisCard();
                    }
                    if (lastObjectAttachedTo != null)
                    {
                        lastObjectAttachedTo.GetComponent<CardHolder>().cardObject = null;
                    }
                    inventory.cardHolders[unitIndex].cardObject = this.gameObject;
                    transform.position = inventory.cardHolders[unitIndex].transform.position;
                    lastObjectAttachedTo = inventory.cardHolders[unitIndex].gameObject;
                    hasSnappedToNew = true;
                }
            }
            else
            {
                if (isInSwapQueue)
                {
                    ResetQueuedCards();
                    inventory.cardHolders[swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                    ResetThisCard();
                }
                if (inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().isInSwapQueue)
                {
                    inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetQueuedCards();
                    inventory.cardHolders[inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().swapPartner].cardObject.GetComponent<Drag>().ResetThisCard();
                    inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().ResetThisCard();
                }
                inventory.cardHandler.cardSwapEvent += SwapQueuedUpCards;
                queuedUpSwaps.Add(unitIndex);
                isInSwapQueue = true;
                swapPartner = unitIndex;
                inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().isInSwapQueue = true;
                inventory.cardHolders[unitIndex].cardObject.GetComponent<Drag>().swapPartner = GetMyIndex();
                QueueUpCardSwap(inventory.cardHolders[unitIndex].cardObject, GetMyIndex());
                QueueUpCardSwap(lastObjectAttachedTo.GetComponent<CardHolder>().cardObject, unitIndex);
            }
        }
        if(!inventory.GetTrashcanAlwaysOnStatus())
        {
            inventory.trashCan.gameObject.SetActive(false);
        }
        if (!hasSnappedToNew)
        {
            transform.position = lastObjectAttachedTo.transform.position;
        }
    }

    private void ResetQueuedCards()
    {
        inventory.cardHandler.cardSwapEvent -= SwapQueuedUpCards;
        inventory.cardHandler.ResetQueuedCards();
    }

    public void ResetThisCard()
    {
        isInSwapQueue = false;
        swapPartner = 0;
        queuedUpSwaps.Clear();
        lastObjectAttachedTo.GetComponent<CardHolder>().cardObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
    }

    public void QueueUpCardSwap(GameObject cardToSwap, int indexToSwap)
    {
        cardToSwap.GetComponent<Image>().color = new Color32(64, 64, 64, 255);
        if (indexToSwap < 4)
        {
            inventory.AddQueuedCards(cardToSwap, indexToSwap);
        }
    }

    private void SwapQueuedUpCards()
    {
        for (int i = 0; i < queuedUpSwaps.Count; i += 2)
        {
            transform.position = inventory.cardHolders[queuedUpSwaps[i]].transform.position;
            inventory.cardHolders[queuedUpSwaps[i]].cardObject.transform.position = lastObjectAttachedTo.transform.position;
            lastObjectAttachedTo.GetComponent<CardHolder>().cardObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            inventory.cardHolders[queuedUpSwaps[i]].cardObject.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            lastObjectAttachedTo.GetComponent<CardHolder>().cardObject = inventory.cardHolders[queuedUpSwaps[i]].cardObject;
            lastObjectAttachedTo.GetComponent<CardHolder>().cardObject.GetComponent<Drag>().lastObjectAttachedTo = lastObjectAttachedTo;
            lastObjectAttachedTo = inventory.cardHolders[queuedUpSwaps[i]].gameObject;
            inventory.cardHolders[queuedUpSwaps[i]].cardObject = this.gameObject;
        }
        queuedUpSwaps.Clear();
    }

    public void PointerEnterHandler(BaseEventData data)
    {
        transform.localScale = new Vector3(cardHoverScale, cardHoverScale);
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        currentCoroutine = StartCoroutine(WaitToOpenDescription(this.gameObject));
    }

    public void PointerExitHandler(BaseEventData data)
    {
        if (currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
        }
        inventory.HideDescriptionObject();
        transform.localScale = new Vector3(1f, 1f);
    }

    public int GetMyIndex()
    {
        for (int i = 0; i < inventory.cardHolders.Count; i++)
        {
            if (lastObjectAttachedTo == inventory.cardHolders[i].gameObject)
            {
                return i;
            }
        }
        return 0;
    }
    private float UnitsHoveringOther(Vector2 myPos, Vector2 myScale, Vector2 otherPos, Vector2 otherScale)
    {
        float myLeft = myPos.x - myScale.x / 2;
        float otherRight = otherPos.x + otherScale.x / 2;
        float myRight = myPos.x + myScale.x / 2;
        float otherLeft = otherPos.x - otherScale.x / 2;
        float myTop = myPos.y + myScale.y / 2;
        float otherBottom = otherPos.y - otherScale.y / 2;
        float myBottom = myPos.y - myScale.y / 2;
        float otherTop = otherPos.y + otherScale.y / 2;

        float coveredX = Mathf.Max(Mathf.Min(myRight, otherRight) - Mathf.Max(myLeft, otherLeft), 0);
        float coveredY = Mathf.Max(Mathf.Min(myTop, otherTop) - Mathf.Max(myBottom, otherBottom), 0);

        return coveredX * coveredY;
    }

    IEnumerator WaitToOpenDescription(GameObject cardObject)
    {
        yield return new WaitForSeconds(timeToOpenDescriptionMenu);
        inventory.ShowDescriptionObject(cardObject.GetComponent<ICard>().GetTitle(), cardObject.GetComponent<ICard>().GetDescription(), (Vector2)transform.localPosition, true, new Vector2(0, lastObjectAttachedTo.GetComponent<CardHolder>().size.y / 2));
    }
}
