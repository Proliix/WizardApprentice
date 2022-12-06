using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCompanion : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;

    int playerSorting;
    GameObject shootPos;
    SpriteRenderer sr;
    PlayerAiming playerAiming;

    // Start is called before the first frame update
    void Start()
    {
        playerAiming = GameObject.FindWithTag("Player").GetComponent<PlayerAiming>();
        shootPos = playerAiming.bulletSpawn;
        playerSorting = playerAiming.gameObject.GetComponent<SpriteRenderer>().sortingOrder;
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

        Vector2 dir = playerAiming.dir.normalized;
        sr.sprite = sprites[((Mathf.FloorToInt((((Mathf.Atan2(dir.y, dir.x) + Mathf.PI) / (Mathf.PI * 2)) + 0.5f / sprites.Length) * sprites.Length)) + Mathf.FloorToInt(sprites.Length / 4)) % sprites.Length];

        if (dir.y < 0)
            sr.sortingOrder = playerSorting + 1;
        else
            sr.sortingOrder = playerSorting - 1;

        transform.position = shootPos.transform.position;
    }
}
