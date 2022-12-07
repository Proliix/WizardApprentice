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

    [SerializeField] Transform player;
    [SerializeField] GameObject laserSprite;
    [SerializeField] GameObject activeLaser;

    [SerializeField] Camera mainCamera;

    [SerializeField] Vector3 mousePos;


    private void Start()
    {
      
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

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
     


        throw new System.NotImplementedException();
    }

    private void Update()
    {
        mainCamera = Camera.main;

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
        Debug.Log(mousePos);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Attack());
        }
    }

    IEnumerator Attack()
    {
        yield return new WaitForSeconds(3);

        GameObject activeLaser = Instantiate(laserSprite);
        activeLaser.transform.position = player.transform.position + mousePos;
        float theta = Mathf.Atan2(mousePos.y - player.transform.position.y, player.transform.position.x - mousePos.x);
        if (theta < 0.0)
        {
            theta += Mathf.PI * 2;
        }
        activeLaser.transform.localRotation = Quaternion.Euler(0,0,(Mathf.Rad2Deg * theta - 90) *-1);

        Vector2 mouseDirection = (mousePos - player.transform.position).normalized;
        yield return null;

    }

}




