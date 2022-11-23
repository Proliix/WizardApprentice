using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrippleShotCard : MonoBehaviour,ICard
{

    [SerializeField] Sprite image;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] float shootPosDeviation = 0.25f;

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
        bulletHandler.GetBullet(Spawnpoint.position, player, true, false);
        bulletHandler.GetBullet(Spawnpoint.position + (Vector3.right * shootPosDeviation), player, true, true);
        bulletHandler.GetBullet(Spawnpoint.position - (Vector3.right * shootPosDeviation), player, true, true);
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
