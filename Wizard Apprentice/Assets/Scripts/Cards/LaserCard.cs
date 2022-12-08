using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserCard : MonoBehaviour, ICard
{
    [SerializeField] Camera mainCamera;
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;

    [SerializeField] Transform player;

    [Header("Laser Variables")]
    [SerializeField] GameObject laserSprite;
    [SerializeField] GameObject activeLaser;
    [SerializeField] float laserLength = 4;
    [SerializeField] float laserWith = 0.5f;
    [SerializeField] Vector3 mousePos;

    [SerializeField] Vector3 rayOrigin;
    [SerializeField] Vector3 rayDirection;
    [SerializeField] float distanceToTarget;
    [SerializeField] float distance;


    bool hasActivated = false;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
     
    }

    public void Effect()
    {
        
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
        hasActivated = false;
        Destroy(activeLaser);
    }

    public void UpdateCard()
    {
        if (!hasActivated)
        {
        hasActivated = true;
            activeLaser = Instantiate(laserSprite);
        }
    }

    private void Update()
    {
       
        mainCamera = Camera.main;

        mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
    }


    IEnumerator Attack()
    {

        laserLength = distanceToTarget;

       
        Vector2 mouseDirection = (mousePos - player.transform.position).normalized;

        activeLaser.transform.localScale = new Vector2(laserWith, distanceToTarget);

        activeLaser.transform.position = (Vector2)player.transform.position - mouseDirection * 0.5f * laserLength * -1;
        float theta = Mathf.Atan2(mousePos.y - player.transform.position.y, player.transform.position.x - mousePos.x);
        if (theta < 0.0)
        {
            theta += Mathf.PI * 2;
        }
        activeLaser.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);

        yield return null;

    }


    public float TestRayCastCheck2D()
    {
        int layerMask = 1 << 8;

        //Cast a ray from the player towards the mouse
        RaycastHit2D hit = Physics2D.Raycast(player.position, (mousePos - player.position).normalized, 1000, layerMask);
        if (hit.collider != null)
        {
            distance = Vector2.Distance(hit.point, player.position);
            distanceToTarget = distance;
            //  rayDist = hit.transform.position - player.transform.position;

        }

        return distance;
    }

    void FixedUpdate()
    {
        if (hasActivated)
        {
        StartCoroutine(Attack());
        }

        TestRayCastCheck2D();
    }
}




