
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] float shootCooldown = 0.25f;

    BulletHandler bulletHandler;
    float timer = 10;

    private void Start()
    {
       bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
    }

    public void Effect()
    {
        bulletHandler.GetBullet(Spawnpoint.position, Spawnpoint.gameObject, true,false);
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        timer = 10;
    }

    public void UpdateCard()
    {
        timer += Time.deltaTime;

        if(timer >= shootCooldown)
        {
            timer = 0;
            Effect();
        }

    }
}
