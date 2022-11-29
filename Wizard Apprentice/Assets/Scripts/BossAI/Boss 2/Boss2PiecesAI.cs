using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2PiecesAI : MonoBehaviour
{
    [SerializeField] GameObject rotationTarget;

    [Header("Boss Variables")]
    [SerializeField] public float RotationSpeed = 90;
    [SerializeField] public bool canShoot;

    void Start()
    {

    }

    void Update()
    {
        transform.RotateAround(rotationTarget.transform.position, Vector3.forward, RotationSpeed * Time.deltaTime);
       
    }

}
