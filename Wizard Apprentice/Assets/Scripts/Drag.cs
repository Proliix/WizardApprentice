using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Drag : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    Vector2 offset;
    [SerializeField] Inventory inventory;
    public GameObject lastObjectAttachedTo;
    private void Start()
    {
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
        if (largestCover > 0)
        {
            inventory.cardHolders[unitIndex].transform.localScale = new Vector3(1.5f,1.5f,1);
        }
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
            if(cover > largestCover)
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

            inventory.cardHolders[i].transform.localScale = new Vector3(1,1,1);
            
        }
        if (largestCover > 0)
        {
            //Set my position to new
            //Set other position to old
            //Set old object to other
            //Set new objcet to me
            if (unitIndex != inventory.cardHandler.cardIndex && lastObjectAttachedTo.GetComponent<CardHolder>().index != inventory.cardHandler.cardIndex)
            {
                if (inventory.cardHolders[unitIndex].cardObject != null)
                {
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
                else if (unitIndex < 4)
                {
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
        }
        if(!hasSnappedToNew)
        {
            transform.position = lastObjectAttachedTo.transform.position;
        }
    }


    public void PointerEnterHandler(BaseEventData data)
    {
        transform.localScale = new Vector3(1.5f, 1.5f);
    }

    public void PointerExitHandler(BaseEventData data)
    {
        
        transform.localScale = new Vector3(1f, 1f);
    }

    private float UnitsHoveringOther(Vector2 myPos, Vector2 myScale, Vector2 otherPos, Vector2 otherScale)
    {
        float myLeft = myPos.x - myScale.x/2;
        float otherRight = otherPos.x + otherScale.x / 2;
        float myRight = myPos.x + myScale.x / 2;
        float otherLeft = otherPos.x - otherScale.x / 2;
        float myTop = myPos.y + myScale.y / 2;
        float otherBottom = otherPos.y - otherScale.y / 2;
        float myBottom = myPos.y - myScale.y / 2;
        float otherTop = otherPos.y + otherScale.y / 2;

        float coveredX = Mathf.Max(Mathf.Min(myRight, otherRight) - Mathf.Max(myLeft, otherLeft),0);
        float coveredY = Mathf.Max(Mathf.Min(myTop, otherTop) - Mathf.Max(myBottom, otherBottom),0);

        return coveredX * coveredY;
    }
}
