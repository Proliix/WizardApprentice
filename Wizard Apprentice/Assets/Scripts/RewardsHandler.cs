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
    [SerializeField] TextMeshProUGUI[] titles = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] effectText = new TextMeshProUGUI[3];
    [Header("Card Rewards")]
    [SerializeField] GameObject cardScreenParent;
    [SerializeField] TextMeshProUGUI[] cardTitles = new TextMeshProUGUI[3];
    [SerializeField] Image[] cardImages = new Image[3];
    [SerializeField] List<GameObject> cards;
    [SerializeField] GameObject[] descriptionObj = new GameObject[3];
    [SerializeField] TextMeshProUGUI[] cardTitleObj = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] cardTextObj = new TextMeshProUGUI[3];


    private Reward[] activeRewards = new Reward[3];
    private GameObject[] activeCards = new GameObject[3];
    private Inventory inventory;

    bool statsAfterCard = false;

    PlayerStats stats;
    bool isPressed = false;

    // Start is called before the first frame update
    void Start()
    {
        rewardScreen.SetActive(false);
        inventory = gameObject.GetComponent<Inventory>();
        statScreenParent.SetActive(false);
        cardScreenParent.SetActive(false);
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();

        for (int i = 0; i < descriptionObj.Length; i++)
        {
            descriptionObj[i].SetActive(false);
        }
    }

    public bool CanAddCards()
    {
        return !inventory.IsFull();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F12))
        {
            if (!inventory.IsFull())
                GetRewardScreenCard();
        }
    }

    #region Card Rewards
    public void GetRewardScreenCard(bool withStats = false)
    {
        if (CanAddCards())
        {

            rewardScreen.SetActive(true);
            cardScreenParent.SetActive(true);
            statsAfterCard = withStats;
            for (int i = 0; i < descriptionObj.Length; i++)
            {
                descriptionObj[i].SetActive(false);
            }
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
        else if (withStats)
        {
            Debug.Log("Skipped adding cards went to stats");
            GetRewardScreenStats();
        }
    }

    public void ToggleDescriptionObj(int index)
    {
        descriptionObj[index].SetActive(!descriptionObj[index].activeSelf);
    }


    public void SelectRewardCard(int index)
    {
        if (!isPressed)
        {
            isPressed = true;
            StartCoroutine(UpdateCardsAfterTime(index));
        }
    }

    private void UpdatePlayerCards(int index)
    {
        inventory.AddCard(activeCards[index]);



        if (!statsAfterCard)
            rewardScreen.SetActive(false);
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
    public void GetRewardScreenStats()
    {
        rewardScreen.SetActive(true);
        statScreenParent.SetActive(true);
        int first = -100;
        int seccond = -100;
        for (int i = 0; i < activeRewards.Length; i++)
        {

            int newNum = Random.Range(0, rewards.Count);

            int runs = 0;
            while ((first == newNum || seccond == newNum) && runs < 20)
            {
                newNum = Random.Range(0, rewards.Count);
                runs++;
            }
            seccond = first;
            first = newNum;

            activeRewards[i] = rewards[newNum];

        }
        for (int i = 0; i < titles.Length; i++)
        {
            titles[i].text = activeRewards[i].Title;
            effectText[i].text = activeRewards[i].EffectText;
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
        isPressed = false;
    }
    void UpdatePlayerStats(int index)
    {
        rewardScreen.SetActive(false);
        statScreenParent.SetActive(false);
        stats.movementSpeed += activeRewards[index].movementSpeed;
        stats.damage += activeRewards[index].damage;
        stats.attackSpeed += activeRewards[index].attackSpeed;
        stats.projectileSize += activeRewards[index].projectileSize;
        stats.projectileSpeed += activeRewards[index].projectileSpeed;
        stats.projectileAmount += activeRewards[index].projectileAmount;
        stats.dashCharges += activeRewards[index].dashCharges;
        stats.dashCooldown += activeRewards[index].dashCooldown;
    }
    #endregion

}
