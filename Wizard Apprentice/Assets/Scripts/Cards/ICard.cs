using UnityEngine;

public interface ICard
{
    //gets called when a card returns to its normal state
    public void ResetCard();
    //the cards effect
    public void Effect();
    //use this as the update function, it gets called when a card is active
    public void UpdateCard();
    //Return the sprite of the card
    public Sprite GetSprite();

    public string GetTitle();

    public string GetDescription();
}
