using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] CellManager cellManager;
    [SerializeField] GameObject playerObject;
    // Start is called before the first frame update
    void Start()
    {
        LoadRoom();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoom()
    {
        cellManager.GenerateRoom();
        playerObject.transform.position = new Vector3(2,2,0);
    }
}
