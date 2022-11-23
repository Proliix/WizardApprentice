using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AurelionCard : MonoBehaviour
{
    [SerializeField] Sprite image;
    [SerializeField] float shootCooldown = 0.25f;
    [SerializeField] float timer = 10;
    public void ResetCard()
    {

    }

    public void Effect()
    {

    }

    public void UpdateCard()
    {

    }

    public Sprite GetSprite()
    {
        return image;
    }
}
