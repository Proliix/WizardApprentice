using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EndScreen : MonoBehaviour
{
    [SerializeField] float timePerSpawn = 0.15f;

    [SerializeField] GameObject endScreenCanvas;
    [SerializeField] GameObject FadeOutObj;
    [SerializeField] GameObject winText;
    [SerializeField] TextMeshProUGUI timeText;
    [SerializeField] TextMeshProUGUI enemiesText;
    [SerializeField] TextMeshProUGUI damageDealtText;
    [SerializeField] TextMeshProUGUI damageTakenText;
    [SerializeField] TextMeshProUGUI restoresText;
    [SerializeField] TextMeshProUGUI amountRestoredText;
    [SerializeField] TextMeshProUGUI timesHitText;
    [SerializeField] TextMeshProUGUI damageTMP;
    [SerializeField] TextMeshProUGUI attackSpeedTMP;
    [SerializeField] TextMeshProUGUI critChanceTMP;
    [SerializeField] TextMeshProUGUI critDamageTMP;
    [SerializeField] TextMeshProUGUI moveSpeedTMP;
    [SerializeField] TextMeshProUGUI maxHpTMP;
    [SerializeField] TextMeshProUGUI cardText;
    [SerializeField] GameObject[] HotbarCards;
    [SerializeField] GameObject[] invCards;
    [SerializeField] GameObject restartButton;
    [SerializeField] GameObject menuButton;

    int enemiesKilled = 0;
    int damageDealt = 0;
    int damageTaken = 0;
    int restoresUsed = 0;
    int healthRestored = 0;
    int cardAmount = 0;
    int timesHit = 0;

    bool endScreenActive = false;
    float time;
    PlayerStats pStats;
    Inventory inv;

    private void Start()
    {
        inv = GetComponent<Inventory>();
        for (int i = 0; i < endScreenCanvas.transform.childCount; i++)
        {
            endScreenCanvas.transform.GetChild(i).gameObject.SetActive(false);
        }

        endScreenCanvas.SetActive(false);
        pStats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
        //Invoke("GetEndScreen", 3);
    }

    private IEnumerator SpawnObjects()
    {
        yield return new WaitForSeconds(0.5f);
        winText.SetActive(true);
        damageTMP.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        attackSpeedTMP.gameObject.SetActive(true);
        enemiesText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        critDamageTMP.gameObject.SetActive(true);
        damageDealtText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        critChanceTMP.gameObject.SetActive(true);
        damageTakenText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        moveSpeedTMP.gameObject.SetActive(true);
        restoresText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        maxHpTMP.gameObject.SetActive(true);
        amountRestoredText.gameObject.SetActive(true);
        yield return new WaitForSeconds(timePerSpawn);
        cardText.gameObject.SetActive(true);
        for (int i = 0; i < HotbarCards.Length; i++)
        {
            HotbarCards[i].gameObject.SetActive(true);
            yield return new WaitForSeconds(timePerSpawn / HotbarCards.Length);
        }
        yield return new WaitForSeconds(timePerSpawn);
        timesHitText.gameObject.SetActive(true);
        for (int i = 0; i < invCards.Length; i++)
        {
            invCards[i].SetActive(true);
            yield return new WaitForSeconds(timePerSpawn / HotbarCards.Length);
        }
        yield return new WaitForSeconds(timePerSpawn / HotbarCards.Length);
        menuButton.gameObject.SetActive(true);
        yield return new WaitForSeconds(0.05f);
        restartButton.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetKeyDown(KeyCode.M))
        //    GetEndScreen();

        if (!endScreenActive)
            time += Time.deltaTime;
    }

    public void AddEnemies()
    {
        enemiesKilled++;
    }

    public void AddDamage(int damage)
    {
        damageDealt += damage;
    }

    public void AddDamageTaken(int damage)
    {
        damageTaken += damage;
    }

    public void AddRestores()
    {
        restoresUsed++;
    }

    public void AddHealthRestored(int restored)
    {
        healthRestored += restored;
    }

    public void AddCard()
    {
        cardAmount++;
    }

    public void AddHit()
    {
        timesHit++;
    }
    public void GetEndScreen()
    {
        PlayerPrefs.SetInt("Completions", PlayerPrefs.GetInt("Completions",0)+1);
        if(AscensionManager.selectedLevel > 0)
        {
            PlayerPrefs.SetInt("ascensionRank",Mathf.Max(PlayerPrefs.GetInt("ascensionRank",0),AscensionManager.selectedLevel));
        }

        MusicManager.Instance.ChangeToMusicType(MusicType.End);
        endScreenActive = true;
        UpdateStats();
        endScreenCanvas.SetActive(true);
        FadeOutObj.SetActive(true);
        StartCoroutine(SpawnObjects());
    }

    private void UpdateStats()
    {

        System.TimeSpan timeSpan = System.TimeSpan.FromSeconds(time);
        string timeTextFormated = timeSpan.Hours > 0 ? string.Format("Time: {0:D2}:{1:D2}:{2:D2}", timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds) : string.Format("Time: {0:D2}:{1:D2}", timeSpan.Minutes, timeSpan.Seconds);
        timeText.text = timeTextFormated;

        for (int i = 0; i < HotbarCards.Length; i++)
        {
            if (inv.cardHolders[i].cardObject != null)
                HotbarCards[i].transform.GetComponentInChildren<Image>().sprite = inv.cardHolders[i].cardObject.GetComponent<ICard>().GetSprite();
        }
        for (int i = 0; i < invCards.Length; i++)
        {

            if (inv.cardHolders[HotbarCards.Length + i].cardObject != null)
                invCards[i].transform.GetComponentInChildren<Image>().sprite = inv.cardHolders[HotbarCards.Length + i].cardObject.GetComponent<ICard>().GetSprite();
        }

        enemiesText.text = "Enemies Killed: " + enemiesKilled;
        damageDealtText.text = "Damage Dealt: " + damageDealt;
        damageTakenText.text = "Damage Taken: " + damageTaken;
        restoresText.text = "Restores Used: " + restoresUsed;
        amountRestoredText.text = "Health Restored: " + healthRestored;
        cardText.text = "Cards Aquired: " + cardAmount;
        timesHitText.text = "Times Hit: " + timesHit;


        string dmgText = "<color=red>DG : " + Mathf.RoundToInt(pStats.damage * 100) + "%</color>";
        damageTMP.SetText(dmgText);


        string attackSpeedText = "<color=blue>AS : " + Mathf.RoundToInt(pStats.attackSpeed * 100) + "%</color>";
        attackSpeedTMP.SetText(attackSpeedText);


        string critMultText = "<color=#F334DA>CD : " + Mathf.RoundToInt(pStats.critDamage * 100) + "%</color>";
        critDamageTMP.SetText(critMultText);

        string critChanceText = "<color=purple>C% : " + Mathf.RoundToInt(pStats.critChance * 100) + "%</color>";
        critChanceTMP.SetText(critChanceText);

        string moveSpeedText = "<color=yellow>MS : " + Mathf.RoundToInt(pStats.movementSpeed * 100) + "%</color>";
        moveSpeedTMP.SetText(moveSpeedText);

        string maxHealthText = "<color=green>HP: " + Mathf.RoundToInt(pStats.health * 100) + "</color>";
        maxHpTMP.SetText(maxHealthText);

    }


}
