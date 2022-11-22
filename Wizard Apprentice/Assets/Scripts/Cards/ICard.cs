using UnityEngine;

interface ICard
{
    public void ResetCard();

    public void Effect();

    public void UpdateCard();

    public Sprite GetSprite();
}
