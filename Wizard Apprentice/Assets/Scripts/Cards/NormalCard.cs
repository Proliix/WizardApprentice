
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalCard : MonoBehaviour, ICard
{
    public GameObject projectile;
    public float shootCooldown = 0.25f;

    float timer;

    public void Effect()
    {
        Instantiate(projectile, Vector3.zero, projectile.transform.rotation);
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
