using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] CellManager cellManager;
    [SerializeField] GameObject playerObject;
    [SerializeField] List<GameObject> possibleEnemies;
    public Vector2Int roomSize;
    List<GameObject> enemyObjects;
    // Start is called before the first frame update
    void Start()
    {
        enemyObjects = new List<GameObject>();
        LoadRoom();
        SpawnEnemies();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadRoom()
    {
        roomSize = new Vector2Int(Random.Range(3, 6) * 5, Random.Range(3, 6) * 3);
        cellManager.GenerateRoom(roomSize);
        playerObject.transform.position = new Vector3(2,2,0);
    }

    public void SpawnEnemies()
    {
        int amountToSpawn = Random.Range(5, 12);
        for (int i = 0; i < amountToSpawn; i++)
        {
            GameObject enemy = Instantiate(possibleEnemies[Random.Range(0, possibleEnemies.Count)]);
            enemy.transform.position = new Vector3(Random.Range(0f, roomSize.x), Random.Range(0f, roomSize.y), 0);
            enemyObjects.Add(enemy);
        }
    }
}
