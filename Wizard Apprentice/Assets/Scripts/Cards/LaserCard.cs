using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LaserCard : MonoBehaviour, ICard
{
    [SerializeField] Sprite image;
    [SerializeField] string title;
    [TextArea(2, 10)]
    [SerializeField] string description;
    [SerializeField] AudioClip attackSound;
    [SerializeField] float audioVolume = 1;


    [Header("Laser Variables")]
    [SerializeField] GameObject laserSpritePrefab;
    [SerializeField] float laserLength = 4;
    [SerializeField] float laserWith = 1f;
    [SerializeField] Vector3 mousePos;

    [SerializeField] Vector3 rayOrigin;
    [SerializeField] Vector3 rayDirection;
    [SerializeField] float distanceToTarget;
    [SerializeField] float distance;

    GameObject activeLaser;
    SpriteRenderer laserSpriteRenderer;
    BoxCollider2D laserCol;
    Transform player;
    Light2D laserLight;
    Vector3[] lightPos = new Vector3[4];



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
        Debug.Log("Reseting laser");
        hasActivated = false;
        Destroy(activeLaser);
    }

    public void UpdateCard()
    {
        if (!hasActivated)
        {
            Camera.main.GetComponent<CameraMovement>().GetScreenShake(2, 0.10f, false);
            hasActivated = true;
            activeLaser = Instantiate(laserSpritePrefab, Vector3.one * 100, laserSpritePrefab.transform.rotation);
            laserSpriteRenderer = activeLaser.GetComponent<SpriteRenderer>();
            laserCol = activeLaser.GetComponent<BoxCollider2D>();
            laserLight = activeLaser.GetComponent<Light2D>();
        }
    }

    private void Update()
    {

        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0;
    }


    IEnumerator Attack()
    {

        laserLength = distanceToTarget;


        Vector2 mouseDirection = (mousePos - player.transform.position).normalized;

        //activeLaser.transform.localScale = new Vector2(laserWith, distanceToTarget);
        laserSpriteRenderer.size = new Vector2(laserWith, distanceToTarget);
        laserCol.size = new Vector2(laserWith, distanceToTarget);
        Bounds sizeBound = laserCol.bounds;
        lightPos[0] = new Vector3( laserWith / 2, distanceToTarget / 2, 0);
        lightPos[1] = new Vector3(-laserWith / 2, distanceToTarget / 2, 0);
        lightPos[2] = new Vector3 (0, -distanceToTarget / 2, 0);
        lightPos[3] = new Vector3(0, -distanceToTarget / 2, 0);



        laserLight.SetShapePath(lightPos);

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




