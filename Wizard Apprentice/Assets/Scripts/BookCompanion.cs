using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCompanion : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    float[] angles;
    float currentAngle;
    float targetAngle;
    float deg;
    float halfDeg;
    GameObject shootPos;
    SpriteRenderer sr;
    int index;
    PlayerAiming playerAiming;

    // Start is called before the first frame update
    void Start()
    {
        shootPos = GameObject.FindWithTag("Player").GetComponent<PlayerAiming>().bulletSpawn;
        playerAiming = GameObject.FindWithTag("Player").GetComponent<PlayerAiming>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        deg = 360 / sprites.Length;
        halfDeg = deg / 2;
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 dir = playerAiming.dir.normalized;
        sr.sprite = sprites[((Mathf.FloorToInt((((Mathf.Atan2(dir.y, dir.x) + Mathf.PI) / (Mathf.PI * 2)) + 0.5f / sprites.Length) * sprites.Length)) + Mathf.FloorToInt(sprites.Length / 4)) % sprites.Length];
       
        if (dir.y < 0)
            sr.sortingOrder = 11;
        else
            sr.sortingOrder = 9;

        transform.position = shootPos.transform.position;
    }
}
