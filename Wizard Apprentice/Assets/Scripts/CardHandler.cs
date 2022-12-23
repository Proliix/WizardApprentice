using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardHandler : MonoBehaviour
{
    [SerializeField] float timePerCard = 2f;
    public GameObject[] cardObjs = new GameObject[4];
    public bool isActive = true;
    [SerializeField] float audioVolume;
    [Header("UI")]
    public Image[] cardCycle = new Image[4];

    ICard[] cards;

    Animator[] animators;
    private Inventory inv;
    float timer = 0;
    public int cardIndex;
    bool hasbeenReset = false;

    private List<GameObject> rememberedSwapObject;
    private List<int> rememberedSwapIndex;

    public delegate void CardHasSwappedDelegate();
    public event CardHasSwappedDelegate cardSwapEvent;

    // Start is called before the first frame update
    void Start()
    {
        cards = new ICard[cardObjs.Length];
        animators = new Animator[cardCycle.Length];
        inv = GetComponent<Inventory>();

        rememberedSwapIndex = new List<int>();
        rememberedSwapObject = new List<GameObject>();

        for (int i = 0; i < cardCycle.Length; i++)
        {
            animators[i] = cardCycle[i].gameObject.GetComponent<Animator>();
        }
        animators[0].SetBool("IsActive", true);
        UpdateInterface();

    }

    public T CheckInCycle<T>() where T : ICard
    {
        for (int i = 0; i < cards.Length; i++)
        {
            if (cards[i] is T)
            {
                return (T)cards[i];
            }
        }
        return default(T);
    }

    public bool CheckInSlot(ICard card, int index)
    {
        bool returnValue = false;

        if (cards[index].GetType() == card.GetType())
            returnValue = true;

        return returnValue;
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

    public void AddQueuedCards(GameObject cardToSwap, int indexToSwap)
    {
        rememberedSwapObject.Add(cardToSwap);
        rememberedSwapIndex.Add(indexToSwap);
    }

    public void ResetQueuedCards()
    {
        rememberedSwapObject.Clear();
        rememberedSwapIndex.Clear();
    }

    public void SwapQueuedCards()
    {
        if (rememberedSwapObject.Count <= 0)
        {
            return;
        }
        Debug.Log("swapping");
        for (int i = 0; i < rememberedSwapObject.Count; i++)
        {
            Debug.Log("replaceed cards");
            ReplaceCard(rememberedSwapObject[i], rememberedSwapIndex[i]);
        }
        rememberedSwapIndex.Clear();
        rememberedSwapObject.Clear();
        if (cardSwapEvent != null)
        {
            cardSwapEvent.Invoke();
        }
    }

    public void UpdateInterface()
    {
        for (int i = 0; i < cardObjs.Length; i++)
        {
            if (cardObjs[i] != null)
            {
                cards[i] = cardObjs[i].GetComponent<ICard>();
                cardCycle[i] = cardObjs[i].GetComponent<Image>();
                animators[i] = cardObjs[i].GetComponent<Animator>();
            }

        }

        for (int i = 0; i < cardCycle.Length; i++)
        {
            if (cards[i] != null)
                if (cards[i].GetSprite() != null)
                    cardCycle[i].sprite = cards[i].GetSprite();
        }


    }

    public void TrunOffAnims(int exeption = -100)
    {
        Animator CheckAnim = null;
        if (exeption >= 0 && exeption < animators.Length)
            CheckAnim = animators[exeption];

        Animator[] anims = new Animator[inv.cardHolders.Count];
        for (int i = 0; i < inv.cardHolders.Count; i++)
        {
            if (inv.cardHolders[i].cardObject != null && inv.cardHolders[i].cardObject.GetComponent<Animator>() != null)
                anims[i] = inv.cardHolders[i].cardObject.GetComponent<Animator>();
            else
                anims[i] = null;
        }

        for (int i = 0; i < anims.Length; i++)
        {
            if (anims[i] != null && anims[i] != CheckAnim)
            {
                anims[i].SetBool("IsActive", false);
            }
            else if (anims[i] == CheckAnim)
            {
                anims[i].SetBool("IsActive", true);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isActive)
        {
            if (hasbeenReset)
            {
                cardIndex = 0;
                if (cards[cardIndex] != null && (cardObjs[cardIndex] != null))
                    animators[cardIndex].SetBool("IsActive", true);
                hasbeenReset = false;
            }
            timer += Time.deltaTime;

            if (cards[cardIndex] != null && (cardObjs[cardIndex] != null))
                cards[cardIndex].UpdateCard();
            //else
            //    Debug.LogError("CARD WITH INDEX " + cardIndex + " IS NULL");


            if (timer >= timePerCard + 0.05f)
            {
                timer = 0;

                if (cards[cardIndex] != null && (cardObjs[cardIndex] != null))
                    cards[cardIndex].ResetCard();


                SwapQueuedCards();

                if (cardIndex < cardObjs.Length - 1)
                    cardIndex++;
                else
                {
                    cardIndex = 0;
                }


                TrunOffAnims(cardIndex);

            }
        }
        else if (!hasbeenReset)
        {
            hasbeenReset = true;
            if ((cardObjs[cardIndex] != null) && cards[cardIndex] != null)
            {
                cards[cardIndex].ResetCard();
            }
            TrunOffAnims();
            cardIndex = -1;
            timer = 0;

        }

    }
}
