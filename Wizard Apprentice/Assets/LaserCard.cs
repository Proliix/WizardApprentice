using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;

    [SerializeField] Transform target;
    [SerializeField] GameObject laserSprite;
    [SerializeField] GameObject activeLaser;

    [SerializeField] Camera mainCamera;

    [SerializeField] Vector3 mousePos;


    private void Start()
    {
      
        target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();


    }

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
        return title;
    }

    public void ResetCard()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateCard()
    {
        mousePos = Input.mousePosition; 
        throw new System.NotImplementedException();
    }

    IEnumerator Attack()
    {

        GameObject activeLaser = Instantiate(laserSprite);
        //activeLaser.transform.position = target.transform + 


        yield return null;

    }

}




