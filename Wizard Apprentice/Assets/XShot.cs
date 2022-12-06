using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class XShot : MonoBehaviour, ICard
{


    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;

    public void Effect()
    {
        throw new System.NotImplementedException();
    }

    public string GetDescription()
    {
        return description;
    }

    public Sprite GetSprite()
    {
        return image;
    }

    public string GetTitle()
    {
        throw new System.NotImplementedException();
    }

    public void ResetCard()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateCard()
    {
        throw new System.NotImplementedException();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
