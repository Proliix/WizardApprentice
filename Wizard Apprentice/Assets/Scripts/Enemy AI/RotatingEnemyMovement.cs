using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingEnemyMovement : MonoBehaviour
{
    [SerializeField] GameObject target;
    void Start()
    {
        
    }

    void Update()
    {
        transform.RotateAround(target.transform.position, Vector3.back, 360 * Time.deltaTime);
    }
}
