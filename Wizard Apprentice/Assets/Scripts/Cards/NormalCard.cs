
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform Spawnpoint;
    [SerializeField] float shootCooldown = 0.25f;

    float timer;

    public void Effect()
    {
        Instantiate(projectile, Spawnpoint.position, projectile.transform.rotation);
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public void ResetCard()
    {
        
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
