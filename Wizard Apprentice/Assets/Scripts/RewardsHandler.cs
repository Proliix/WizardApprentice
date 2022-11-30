using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RewardsHandler : MonoBehaviour
{
    [SerializeField] GameObject rewardScreen;
    public List<Reward> rewards;
    [SerializeField] TextMeshProUGUI[] Titles = new TextMeshProUGUI[3];
    [SerializeField] TextMeshProUGUI[] EffectText = new TextMeshProUGUI[3];

    private Reward[] activeRewards;

    PlayerStats stats;

    // Start is called before the first frame update
    void Start()
    {
        rewardScreen.SetActive(false);
        activeRewards = new Reward[3];
        stats = GameObject.FindWithTag("Player").GetComponent<PlayerStats>();
    }

    public void GetRewardScreenStats()
    {
        rewardScreen.SetActive(true);
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
        for (int i = 0; i < Titles.Length; i++)
        {
            Titles[i].text = activeRewards[i].Title;
            EffectText[i].text = activeRewards[i].EffectText;
        }

    }

    public void UpdatePlayerStats(int index)
    {
        rewardScreen.SetActive(false);
        stats.movementSpeed += activeRewards[index].movementSpeed;
        stats.damage += activeRewards[index].damage;
        stats.attackSpeed += activeRewards[index].attackSpeed;
        stats.projectileSize += activeRewards[index].projectileSize;
        stats.projectileSpeed += activeRewards[index].projectileSpeed;
        stats.projectileAmount += activeRewards[index].projectileAmount;
        stats.dashCharges += activeRewards[index].dashCharges;
        stats.dashCooldown += activeRewards[index].dashCooldown;
    }

}
