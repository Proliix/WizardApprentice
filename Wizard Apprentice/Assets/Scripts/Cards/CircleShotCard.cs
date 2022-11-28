using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleShotCard : MonoBehaviour, ICard
{

    [SerializeField] Sprite image;
    [SerializeField] int projectileAmmount = 6;
    [SerializeField] float shootCooldown = 0.5f;

    BulletHandler bulletHandler;
    GameObject player;
    float timer = 10;

    private void Start()
    {
        bulletHandler = GameObject.FindWithTag("GameController").GetComponent<BulletHandler>();
        player = GameObject.FindWithTag("Player");
    }
    public void Effect()
    {
        bulletHandler.GetCircleShot(projectileAmmount, player, true);
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

        if (timer >= shootCooldown)
        {
            timer = 0;
            Effect();
        }

    }
}
