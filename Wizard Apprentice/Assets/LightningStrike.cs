using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningStrike : MonoBehaviour, ICard
{
    [SerializeField] Sprite sprite;
    [SerializeField] string title;
    [SerializeField] Vector3 mousePos;

    [SerializeField] GameObject lightningStrike;

    [TextArea(2, 10)]
    [SerializeField] string description;

    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] float damage = 10f;



    GameObject activeLightningStrike;
    Transform spawnpoint;
    PlayerStats stats;

    float timer;

    void Start()
    {
        
    }

    public void Effect()
    {
        activeLightningStrike = Instantiate(lightningStrike, mousePos, Quaternion.identity);
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return sprite;
    }

    public string GetTitle()
    {
        return title; 
    }

    public void ResetCard()
    {
       
        timer = 0;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;
        if (timer >= shootCooldown)
        {
            timer -= shootCooldown;
            Effect();
        }
    }

    public void Update()
    {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
    }


}
