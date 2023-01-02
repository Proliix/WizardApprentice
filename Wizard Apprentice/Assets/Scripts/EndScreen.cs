using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
    [SerializeField] TextMeshProUGUI damageTMP;
    [SerializeField] TextMeshProUGUI attackSpeedTMP;
    [SerializeField] TextMeshProUGUI critChanceTMP;
    [SerializeField] TextMeshProUGUI critDamageTMP;
    [SerializeField] TextMeshProUGUI moveSpeedTMP;

    //ADD MAXHP, HP RESTORED

    int enemiesKilled = 0;
    int damageDealt = 0; 
    int damageTaken = 0;
    int restoresUsed = 0;
    int healthRestored = 0;

    bool endScreenActive = false;
    float time;
    PlayerStats pStats;

    private void Start()
    {
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
    }

    // Update is called once per frame
    void Update()
    {
        if (!endScreenActive)
            time += Time.deltaTime;
    }

    public void GetEndScreen()
    {
        endScreenActive = true;
        timeText.text = string.Format("Time: " + "{0:0.00}", time);
        UpdateStats();
        endScreenCanvas.SetActive(true);
        FadeOutObj.SetActive(true);
        StartCoroutine(SpawnObjects());
    }

    private void UpdateStats()
    {

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

    }


}
