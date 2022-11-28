using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAiming : MonoBehaviour
{
    public GameObject bulletSpawn;
    [SerializeField] float amount = 0.5f;
 

    void Update()
    {
        Vector2 worldMousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector3 dir = (Vector3)worldMousePos - transform.position;

        bulletSpawn.transform.position = transform.position + (dir.normalized * amount);
        bulletSpawn.transform.up = dir;
    }
}
