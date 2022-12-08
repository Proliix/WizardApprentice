using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    [SerializeField] Sprite openDoorImage;
    GameObject currentRoom;
    [SerializeField] GameObject roomParent;
    GameObject currentRoomParent;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] BulletHandler bulletHandler;
    [SerializeField] CardHandler cardHandler;
    [SerializeField] RewardsHandler rewardsHandler;

    [SerializeField] List<Room> possibleBossRoomsByFloor;
    [SerializeField] List<Room> possibleNormalRooms;
    public int normalRoomsPoolAmount;
    [SerializeField] List<Room> possibleMinibossRooms;
    public int miniBossRoomsPoolAmount;
    [SerializeField] List<Room> possibleTreasureRooms;
    public int treasureRoomsPoolAmount;
    [SerializeField] List<Room> possibleMysteryRooms;
    public int mysteryRoomsPoolAmount;

    private int currentRoomType;
    private bool canWalkThroughAnyDoor;
    private GameObject doorObject;

    public int currentFloor;

    // Start is called before the first frame update
    void Start()
    {
        enemyObjects = new List<GameObject>();
        LoadNewRoom(1);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P))
        {
            canWalkThroughAnyDoor = !canWalkThroughAnyDoor;
        }
    }

    public void LoadPremadeRoom(Room room)
    {
        if (currentRoomParent != null)
        {
            Destroy(currentRoomParent);
        }
        currentRoomParent = Instantiate(roomParent);
        currentRoom = Instantiate(room.roomPrefab, currentRoomParent.transform);
        currentRoom.transform.position += new Vector3((float)room.roomSize.x / 2f, (float)room.roomSize.y / 2f, 0);
        if(room.generateRandomRoom)
        {
            LoadRoomFloor(room.roomSize);
        }
        Transform[] transformsInRoom = currentRoom.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transformsInRoom.Length; i++)
        {
            if (transformsInRoom[i].CompareTag("Enemy"))
            {
                enemyObjects.Add(transformsInRoom[i].gameObject);
            }
        }
        enemyManager.enemyObjects = enemyObjects;
        if(!room.roomAlreadyHasDoor)
        {
            doorObject = Instantiate(exitDoorPrefab, new Vector3(room.roomSize.x / 2, room.roomSize.y - 0.5f, 0), Quaternion.identity, currentRoomParent.transform);
        }
        playerObject.transform.position = new Vector3(room.roomSize.x / 2, 1, 0);
    }

    public void PlayerWalkThroughDoor()
    {
        if (enemyObjects.Count <= 0 || canWalkThroughAnyDoor)
        {
            Debug.Log("Player could walk through door becuase there are " + enemyObjects.Count + " enemies left");
            RemoveAllEnemies();
            bulletHandler.ResetAll();
            cardHandler.isActive = false;
            switch(currentRoomType)
            {
                case 0:
                    roomSelectScreenGenerator.GenerateAnotherFloor();
                    rewardsHandler.GetRewardScreenCard(true);
                    break;
                case 1:
                    rewardsHandler.GetRewardScreenStats();
                    break;
                case 2:
                    rewardsHandler.GetRewardScreenCard(true);
                    break;
            }
            roomSelectScreenGenerator.roomSelectObject.SetActive(true);
            roomSelectScreenGenerator.Open();
        }
        else
        {
            Debug.Log("Player could NOT walk through door becuase there are " + enemyObjects.Count + " enemies left");
        }
    }
    public void LoadRoomFloor(Vector2Int size)
    {
        cellManager.GenerateRoom(size, currentRoomParent.transform,true);
    }

    public void LoadRandomRoom()
    {
        roomSize = new Vector2Int(Random.Range(3, 6) * 5, Random.Range(3, 6) * 3);
        cellManager.GenerateRoom(roomSize, currentRoomParent.transform);
        playerObject.transform.position = new Vector3(2, 2, 0);
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

    public void AddEnemy(GameObject enemyObject)
    {
        enemyObjects.Add(enemyObject);
        enemyManager.enemyObjects = enemyObjects;
    }
    public void RemoveAllEnemies()
    {
        for (int i = enemyObjects.Count - 1; i >= 0; i--)
        {
            Destroy(enemyObjects[i]);
            enemyObjects.RemoveAt(i);
        }
        enemyObjects.Clear();
        OpenDoor();
    }

    public void RemoveEnemy(GameObject enemyObject)
    {
        Debug.Log("Removing enemy " + enemyObject.name);
        enemyObjects.Remove(enemyObject);
        enemyManager.enemyObjects = enemyObjects;
        if(enemyObjects.Count <= 0)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        doorObject.GetComponentInChildren<SpriteRenderer>().sprite = openDoorImage;
    }

    public void LoadNewRoom(int roomType)
    {
        bulletHandler.ResetAll();
        cardHandler.isActive = true;
        switch (roomType)
        {
            case 0:
                currentRoomType = 0;
                LoadPremadeRoom(possibleBossRoomsByFloor[currentFloor % possibleBossRoomsByFloor.Count]);
                break;
            case 1:
                currentRoomType = 1;
                LoadPremadeRoom(possibleNormalRooms[Random.Range(0, possibleNormalRooms.Count)]);
                break;
            case 2:
                currentRoomType = 2;
                LoadPremadeRoom(possibleMinibossRooms[Random.Range(0, possibleMinibossRooms.Count)]);
                break;
            case 3:
                currentRoomType = 3;
                LoadPremadeRoom(possibleTreasureRooms[Random.Range(0, possibleTreasureRooms.Count)]);
                break;
            case 4:
                currentRoomType = 4;
                LoadPremadeRoom(possibleMysteryRooms[Random.Range(0, possibleMysteryRooms.Count)]);
                break;
            default:
                currentRoomType = 1;
                LoadPremadeRoom(possibleNormalRooms[Random.Range(0, possibleNormalRooms.Count)]);
                break;
        }
        Debug.Log("Turning off room select object");
        roomSelectScreenGenerator.roomSelectObject.SetActive(false);
    }
}
