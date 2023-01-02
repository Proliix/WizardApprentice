using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RewardsHandler : MonoBehaviour
{
    [SerializeField] GameObject rewardScreen;
    [Header("Stat Rewards")]
    [SerializeField] GameObject statScreenParent;
    public List<Reward> rewards;
    public List<Reward> rewardsTier2;
    public List<Reward> rewardsTier3;
    public Reward healReward;
    public int floor = 0;
    [Range(0, 100)] public float[] chanceTeir1 = new float[3];
    [Range(0, 100)] public float[] chanceTeir2 = new float[3];
    [Range(0, 100)] public float[] chanceTeir3 = new float[3];
    [SerializeField] TextMeshProUGUI[] titles = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] effectText = new TextMeshProUGUI[3];
    [SerializeField] Image[] potionImage = new Image[3];
    [Header("Card Rewards")]
    [SerializeField] AudioClip invFullSound;
    [SerializeField] bool CardDescriptionTitles = false;
    [SerializeField] GameObject cardScreenParent;
    [SerializeField] TextMeshProUGUI[] cardTitles = new TextMeshProUGUI[3];
    [SerializeField] Button[] cardButtons = new Button[3];
    [SerializeField] Button skipButton;
    [SerializeField] Button invButton;
    [SerializeField] Image[] cardImages = new Image[3];
    [SerializeField] List<GameObject> cards;
    [SerializeField] GameObject[] descriptionObj = new GameObject[3];
    [SerializeField] TextMeshProUGUI[] cardTitleObj = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] cardTextObj = new TextMeshProUGUI[3];
    [SerializeField] GameObject inventoryFullScreen;
    [Header("Card Removal")]
    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject hotbar;
    [SerializeField] GameObject invHolder;
    [SerializeField] GameObject trashCan;
    [SerializeField] GameObject background;
    [SerializeField] Button backButton;
    [SerializeField] Canvas cardCanvas;
    [SerializeField] float moveSpeed = 1f;
    [SerializeField] Vector3 newHotbarPos;
    [SerializeField] Vector3 newHotbarScale = Vector3.one;
    [SerializeField] Vector3 newInvHolderPos;
    [SerializeField] Vector3 newTrashCanPos;
    [SerializeField] Vector3 newBackgroundPos;

    Vector3 oldHotbarPos;
    Vector3 oldHotbarScale;
    Vector3 oldTrashCanPos;
    Vector3 oldInvHolderPos;
    Vector3 oldBackgroundPos;
    Vector3 hotbarMoveStartPos;
    Animator fadeoutAnim;
    bool isMoving = false;
    bool resetAfterMove = false;
    Vector3 movePos;
    int checkIndex = 0;
    int startLayer;
    CardHandler cardHandler;
    RoomManager roomManager;
    Animator inventoryFullAnim;

    private Reward[] activeRewards = new Reward[3];
    private GameObject[] activeCards = new GameObject[3];
    private Inventory inventory;

    bool statsAfterCard = false;
    Health health;
    PlayerMovement pMovement;
    PlayerStats stats;
    bool isPressed = false;
    bool wasActive;
    bool debugMode;

    // Start is called before the first frame update
    void Start()
    {
        rewardScreen.SetActive(false);
        inventory = gameObject.GetComponent<Inventory>();
        statScreenParent.SetActive(false);
        cardScreenParent.SetActive(false);
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        health = GameObject.FindWithTag("Player").GetComponent<Health>();
        pMovement = GameObject.FindWithTag("Player").GetComponent<PlayerMovement>();
        roomManager = gameObject.GetComponent<RoomManager>();
        inventoryFullScreen.SetActive(false);
        inventoryFullAnim = inventoryFullScreen.GetComponent<Animator>();
        for (int i = 0; i < cardTitleObj.Length; i++)
        {
            cardTitleObj[i].gameObject.SetActive(CardDescriptionTitles);
        }

        //card removal screen
        cardHandler = GetComponent<CardHandler>();
        oldHotbarPos = hotbar.transform.localPosition;
        oldHotbarScale = hotbar.transform.localScale;
        oldTrashCanPos = trashCan.transform.localPosition;
        oldInvHolderPos = invHolder.transform.localPosition;
        oldBackgroundPos = background.transform.localPosition;
        fadeOut.SetActive(false);
        background.SetActive(false);
        backButton.gameObject.SetActive(false);
        fadeoutAnim = fadeOut.GetComponent<Animator>();
        startLayer = cardCanvas.sortingOrder;

        debugMode = PlayerPrefs.GetInt("Debug") > 0 ? true : false;
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12) && debugMode)
        {
            //if (!inventory.IsFull())
            cardHandler.isActive = true;
            GetRewardScreenCard();
        }
        else if (Input.GetKeyDown(KeyCode.F11) && debugMode)
        {
            cardHandler.isActive = true;
            GetRewardScreenStats();
        }

        if (isMoving)
        {
            hotbar.transform.localPosition = new Vector3(hotbar.transform.localPosition.x, Mathf.MoveTowards(hotbar.transform.localPosition.y, movePos.y, moveSpeed * Time.deltaTime), hotbar.transform.localPosition.z);

            if (hotbar.transform.localPosition.y <= movePos.y + 0.1f && hotbar.transform.localPosition.y >= movePos.y - 0.11f)
            {
                if (resetAfterMove)
                {
                    ResetInventoryObj();
                }

                isMoving = false;
            }
        }

    }

    #region Card Rewards

    public void RemoveCardInventoryScreen()
    {
        if (!isMoving)
        {
            inventoryFullScreen.SetActive(!CanAddCards());
            resetAfterMove = true;
            movePos = hotbarMoveStartPos;
            isMoving = true;
            fadeoutAnim.SetTrigger("FadeIn");
        }
    }

    void ResetInventoryObj()
    {
        resetAfterMove = false;
        for (int i = 0; i < cardButtons.Length; i++)
        {
            cardButtons[i].interactable = true;
        }
        inventory.TurnOffTrashcan();
        skipButton.interactable = true;
        invButton.interactable = true;
        fadeOut.SetActive(false);
        backButton.gameObject.SetActive(false);
        background.SetActive(false);
        backButton.gameObject.SetActive(false);
        hotbar.transform.localPosition = oldHotbarPos;
        background.transform.localPosition = oldBackgroundPos;
        hotbar.transform.localScale = oldHotbarScale;
        invHolder.transform.localPosition = oldInvHolderPos;
        trashCan.transform.localPosition = oldTrashCanPos;
        inventory.ResetCardsPos();
        cardCanvas.sortingOrder = startLayer;
    }
    public void GetCardInventoryScreen()
    {
        inventory.cardRemovedEvent += DisableInvFullText;
        if (!isMoving)
        {
            for (int i = 0; i < cardButtons.Length; i++)
            {
                cardButtons[i].interactable = false;
            }
            inventory.TurnOnTrashcan();
            skipButton.interactable = false;
            invButton.interactable = false;
            fadeOut.SetActive(true);
            fadeoutAnim.SetTrigger("FadeOut");
            backButton.gameObject.SetActive(true);
            background.SetActive(true);
            hotbarMoveStartPos = hotbar.transform.localPosition + Vector3.down * 800;
            hotbar.transform.localPosition = hotbarMoveStartPos;
            background.transform.localPosition = newBackgroundPos;
            hotbar.transform.localScale = newHotbarScale;
            invHolder.transform.localPosition = newInvHolderPos;
            trashCan.transform.localPosition = newTrashCanPos;
            movePos = newHotbarPos;
            inventory.ResetCardsPos();
            cardCanvas.sortingOrder = 10;
            isMoving = true;
        }
    }

    public bool CanAddCards()
    {
        return !inventory.IsFull();
    }

    public void GetRewardScreenCard(bool withStats = false)
    {
        wasActive = cardHandler.isActive;
        cardHandler.isActive = false;
        if (!CanAddCards())
        {
            inventoryFullScreen.SetActive(true);
        }
        else
        {
            inventoryFullScreen.SetActive(false);
        }

        rewardScreen.SetActive(true);
        cardScreenParent.SetActive(true);
        statsAfterCard = withStats;
        int first = -100;
        int seccond = -100;
        for (int i = 0; i < activeCards.Length; i++)
        {


            int newNum = Random.Range(0, cards.Count);

            int runs = 0;
            while ((first == newNum || seccond == newNum) && runs < 20)
            {
                newNum = Random.Range(0, cards.Count);
                runs++;
            }

            activeCards[i] = cards[newNum];

            if (activeCards[i].GetComponent<ICard>() != null)
            {
                seccond = first;
                first = newNum;

            }
            else
            {
                i--;
                Debug.LogError("<color=red>Error: </color>no icard script on " + activeCards[i].name + "in rewardhandler list");
            }

        }
        for (int i = 0; i < titles.Length; i++)
        {
            cardTitles[i].text = activeCards[i].GetComponent<ICard>().GetTitle();
            cardTitleObj[i].text = activeCards[i].GetComponent<ICard>().GetTitle();
            cardTextObj[i].text = activeCards[i].GetComponent<ICard>().GetDescription();
            cardImages[i].sprite = activeCards[i].GetComponent<ICard>().GetSprite();
        }

    }

    public void SkipCards()
    {
        cardScreenParent.SetActive(false);
        if (statsAfterCard)
            GetRewardScreenStats();
        else
        {
            cardHandler.isActive = wasActive;
            rewardScreen.SetActive(false);
        }
    }

    void DisableInvFullText()
    {
        inventoryFullScreen.SetActive(!CanAddCards());
        inventory.cardRemovedEvent -= DisableInvFullText;
    }

    public void SelectRewardCard(int index)
    {
        if (!isPressed && CanAddCards())
        {
            isPressed = true;
            StartCoroutine(UpdateCardsAfterTime(index));
        }
        else
        {
            SoundManager.Instance.PlayAudio(invFullSound);
            inventoryFullAnim.SetTrigger("Shake");
        }
    }

    private void UpdatePlayerCards(int index)
    {
        inventory.AddCard(activeCards[index]);



        if (!statsAfterCard)
        {
            cardHandler.isActive = wasActive;
            rewardScreen.SetActive(false);
        }
        else
            GetRewardScreenStats();



        statsAfterCard = false;

        cardScreenParent.SetActive(false);
    }

    IEnumerator UpdateCardsAfterTime(int index)
    {
        yield return new WaitForSeconds(0.03f);
        UpdatePlayerCards(index);
        isPressed = false;
    }

    #endregion



    #region Stat Rewards

    List<Reward> RollChanceList()
    {
        List<Reward> listToChange;
        float eval = Random.Range(0, 1f) * 100;
        floor = roomManager.currentFloor;
        if (floor >= chanceTeir1.Length)
            floor = chanceTeir1.Length - 1;
        if (eval <= chanceTeir1[floor])
        {
            listToChange = rewards;
            //Debug.Log("1 " + eval + " | " + chanceTeir1[floor]);
        }
        else if (eval <= chanceTeir2[floor] + chanceTeir1[floor])
        {
            listToChange = rewardsTier2;
            //Debug.Log("2 " + eval + " | " + (chanceTeir2[floor] + chanceTeir1[floor]));
        }
        else if (eval <= chanceTeir3[floor] + (chanceTeir2[floor] + chanceTeir1[floor]))
        {
            listToChange = rewardsTier3;
            //Debug.LogError("3 " + eval + " | " + (chanceTeir3[floor] + (chanceTeir2[floor] + chanceTeir1[floor])));
        }
        else
        {
            //Debug.Log("is wrong" + eval);
            listToChange = rewards;
        }

        if (listToChange.Count == 0)
            listToChange = rewards;
        return listToChange;
    }

    public void GetRewardScreenStats()
    {
        bool canGetCritChance = stats.critChance < 1 ? true : false;
        wasActive = cardHandler.isActive;
        cardHandler.isActive = false;
        rewardScreen.SetActive(true);
        statScreenParent.SetActive(true);
        int first = -100;
        int seccond = -100;
        bool fullHp = health.HasFullHealth();

        List<Reward> currentRewardsList = new List<Reward>();



        for (int i = 0; i < activeRewards.Length; i++)
        {
            currentRewardsList = RollChanceList();

            int newNum = Random.Range(0, currentRewardsList.Count);

            int runs = 0;

            while ((first == newNum || seccond == newNum || (currentRewardsList[newNum].critChance > 0 && !canGetCritChance)) && runs < 100)
            {
                newNum = Random.Range(0, currentRewardsList.Count);
                runs++;
            }

            //Debug.Log("First: " + first + " |Seccond: " + seccond + " |Newnum: " + newNum + "|Runs: " + runs);

            seccond = first;
            first = newNum;

            activeRewards[i] = i == 2 && !fullHp ? healReward : currentRewardsList[newNum];
        }
        for (int i = 0; i < titles.Length; i++)
        {
            potionImage[i].color = new Color(1, 1, 1, 1);
            titles[i].text = activeRewards[i].Title;
            effectText[i].text = activeRewards[i].GetDesription();

            if (activeRewards[i].Image != null)
                potionImage[i].sprite = activeRewards[i].Image;
            else
                potionImage[i].color = new Color(0, 0, 0, 0);
        }
    }

    public void SelectRewardStat(int index)
    {
        if (!isPressed)
        {
            isPressed = true;
            StartCoroutine(UpdatePlayerAfterTime(index));
        }
    }

    IEnumerator UpdatePlayerAfterTime(int index)
    {
        yield return new WaitForSeconds(0.03f);
        UpdatePlayerStats(index);
        cardHandler.isActive = wasActive;
        isPressed = false;

    }
    void UpdatePlayerStats(int index)
    {
        rewardScreen.SetActive(false);
        statScreenParent.SetActive(false);
        stats.GiveStats(activeRewards[index]);

        //stats.health += activeRewards[index].maxHealth;
        //stats.movementSpeed += activeRewards[index].movementSpeed;
        //stats.damage += activeRewards[index].damage;
        //stats.attackSpeed += activeRewards[index].attackSpeed;
        //stats.critChance += activeRewards[index].critChance;
        //stats.critDamage += activeRewards[index].critDamage;
        //stats.projectileSize += activeRewards[index].projectileSize;
        //stats.projectileSpeed += activeRewards[index].projectileSpeed;
        //stats.projectileAmount += activeRewards[index].projectileAmount;

        //if (activeRewards[index].addHealh > 0)
        //{
        //    stats.gameObject.GetComponent<Health>()?.HealPercentageOf(activeRewards[index].addHealh);
        //}
    }
    #endregion

}
