using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomManager : MonoBehaviour
{
    [SerializeField] RoomSelectScreenGenerator roomSelectScreenGenerator;
    [SerializeField] CellManager cellManager;
    [SerializeField] GameObject playerObject;
    [SerializeField] List<GameObject> possibleEnemies;
    [SerializeField] List<Room> possibleRooms;
    public Vector2Int roomSize;
    List<GameObject> enemyObjects;
    [SerializeField] GameObject exitDoorPrefab;
    GameObject currentRoom;
    [SerializeField] GameObject roomParent;
    GameObject currentRoomParent;
    [SerializeField] EnemyManager enemyManager;

    // Start is called before the first frame update
    void Start()
    {
        enemyObjects = new List<GameObject>();
        LoadPremadeRoom(possibleRooms[Random.Range(0,possibleRooms.Count)]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void LoadPremadeRoom(Room room)
    {
        if(currentRoomParent != null)
        {
            Destroy(currentRoomParent);
        }
        currentRoomParent = Instantiate(roomParent);
        currentRoom = Instantiate(room.roomPrefab, currentRoomParent.transform);
        currentRoom.transform.position += new Vector3((float)room.roomSize.x /2f, (float)room.roomSize.y / 2f, 0);
        LoadRoomFloor(room.roomSize);
        Transform[] transformsInRoom = currentRoom.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transformsInRoom.Length; i++)
        {
            if (transformsInRoom[i].CompareTag("Enemy"))
            {
                enemyObjects.Add(transformsInRoom[i].gameObject);
            }
        }
        enemyManager.enemyObjects = enemyObjects;
        Instantiate(exitDoorPrefab, new Vector3(room.roomSize.x/2, room.roomSize.y,0), Quaternion.identity, currentRoomParent.transform);
        playerObject.transform.position = new Vector3(room.roomSize.x/2, 1, 0);
    }

    public void PlayerWalkThroughDoor()
    {
        if(enemyObjects.Count <= 1000)
        {
            Debug.Log("Player could walk through door becuase there are " + enemyObjects.Count + " enemies left");
            roomSelectScreenGenerator.roomSelectObject.SetActive(true);
            roomSelectScreenGenerator.Open();
            Debug.Log("Turned on roomselect object");
            //LoadPremadeRoom(possibleRooms[Random.Range(0, possibleRooms.Count)]);
        }
        else
        {
            Debug.Log("Player could NOT walk through door becuase there are " + enemyObjects.Count + " enemies left");
        }
    }
    public void LoadRoomFloor(Vector2Int size)
    {
        cellManager.GenerateRoom(size, currentRoomParent.transform);
    }

    public void LoadRandomRoom()
    {
        roomSize = new Vector2Int(Random.Range(3, 6) * 5, Random.Range(3, 6) * 3);
        cellManager.GenerateRoom(roomSize, currentRoomParent.transform);
        playerObject.transform.position = new Vector3(2,2,0);
        SpawnEnemies();
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

    public void RemoveEnemy(GameObject enemyObject)
    {
        enemyObjects.Remove(enemyObject);
        enemyManager.enemyObjects = enemyObjects;
    }

    public void LoadNewRoom(int roomType)
    {
        LoadPremadeRoom(possibleRooms[Random.Range(0, possibleRooms.Count)]);
        Debug.Log("Turning off room select object");
        roomSelectScreenGenerator.roomSelectObject.SetActive(false);
    }
}
