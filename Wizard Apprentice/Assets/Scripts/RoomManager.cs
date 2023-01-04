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
    GameObject currentRoomObject;
    [HideInInspector] public Room currentRoom;
    [SerializeField] GameObject roomParent;
    GameObject currentRoomParent;
    [SerializeField] EnemyManager enemyManager;
    [SerializeField] BulletHandler bulletHandler;
    [SerializeField] CardHandler cardHandler;
    [SerializeField] RewardsHandler rewardsHandler;

    [SerializeField] Room tutorialRoom;
    [SerializeField] List<Room> possibleBossRoomsByFloor;
    [SerializeField] List<Room> possibleNormalRooms;
    public int normalRoomsPoolAmount;
    [SerializeField] List<Room> possibleMinibossRooms;
    public int miniBossRoomsPoolAmount;
    [SerializeField] List<Room> possibleTreasureRooms;
    public int treasureRoomsPoolAmount;
    [SerializeField] List<Room> possibleMysteryRooms;
    public int mysteryRoomsPoolAmount;

    [SerializeField] int difficultyRange;

    private int currentRoomType;
    private bool canWalkThroughAnyDoor;
    private GameObject doorObject;

    public int currentFloor;

    [Header("Map Open Sound")]
    [SerializeField] AudioClip mapOpen;
    [SerializeField] float mapOpenVolume = 1;

    [Header("Room Pause Variables")]
    [SerializeField] float pauseDuration = 1;
    public float originalTimescale;

    [Header("Room Clear Variables")]
    [SerializeField] AudioClip roomClearSound;
    [SerializeField] float audioVolume = 1;
    [SerializeField] ParticleSystem particleSystem;

    Inventory inv;
    EndScreen endScreen;
    // Start is called before the first frame update
    void Start()
    {
        inv = gameObject.GetComponent<Inventory>();
        endScreen = gameObject.GetComponent<EndScreen>();
        enemyObjects = new List<GameObject>();
        LoadNewRoom(5);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
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
        currentRoom = room;
        MusicManager.Instance.ChangeToMusicType(room.musicType);
        RoomFadeIn.NewRoom();
        GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().SetCanMove(true);
        currentRoomParent = Instantiate(roomParent);
        currentRoomObject = Instantiate(room.roomPrefab, currentRoomParent.transform);
        currentRoomObject.transform.position += new Vector3((float)room.roomSize.x / 2f, (float)room.roomSize.y / 2f, 0);
        if (room.generateRandomRoom)
        {
            LoadRoomFloor(room.roomSize);
        }
        Transform[] transformsInRoom = currentRoomObject.GetComponentsInChildren<Transform>();
        for (int i = 0; i < transformsInRoom.Length; i++)
        {
            if (transformsInRoom[i].CompareTag("Enemy"))
            {
                enemyObjects.Add(transformsInRoom[i].gameObject);
            }
        }
        
        for (int i = 0; i < enemyObjects.Count; i++)
        {
            if (currentRoomType != 0)
            {
                enemyObjects[i].GetComponent<Health>()?.AddMaxHealth(currentFloor * 0.75f, true);
            }
            enemyObjects[i].GetComponent<Health>()?.AddMaxHealth(AscensionManager.selectedLevel * 0.2f, true);
        }
        enemyManager.enemyObjects = enemyObjects;
        if (!room.roomAlreadyHasDoor)
        {
            doorObject = Instantiate(exitDoorPrefab, new Vector3(room.roomSize.x / 2, room.roomSize.y + 0.3f, 0), Quaternion.identity, currentRoomParent.transform);
        }
        playerObject.transform.position = new Vector3(room.roomSize.x / 2, 1, 0);
    }

    public void PlayerWalkThroughDoor()
    {
        if (enemyObjects.Count <= 0 || canWalkThroughAnyDoor || currentRoomType == 5)
        {

            Debug.Log("Player could walk through door becuase there are " + enemyObjects.Count + " enemies left");
            GameObject.FindWithTag("Player").GetComponent<PlayerMovement>().SetCanMove(false);
            RemoveAllEnemies();
            bulletHandler.ResetAll();
            cardHandler.isActive = false;
            if (currentRoom.isLastRoom)
            {
                endScreen.GetEndScreen();
            }
            else
            {

                MusicManager.Instance.ChangeToMusicType(MusicType.Map);
                enemyManager.ResetEnemyStatus();
                switch (currentRoomType)
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
                SoundManager.Instance.PlayAudio(mapOpen, mapOpenVolume);
            }
        }
        else
        {
            Debug.Log("Player could NOT walk through door becuase there are " + enemyObjects.Count + " enemies left");
        }
    }
    public void LoadRoomFloor(Vector2Int size)
    {
        cellManager.GenerateRoom(size, currentRoomParent.transform, true);
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
        int enemyCount = enemyObjects.Count;
        for (int i = enemyObjects.Count - 1; i >= 0; i--)
        {
            Destroy(enemyObjects[i]);
            enemyObjects.RemoveAt(i);
        }
        enemyObjects.Clear();
        if (enemyCount != 0 && currentRoomType != 5)
        {

            OpenDoor();
        }
    }

    public void RemoveEnemy(GameObject enemyObject)
    {

        Debug.Log("Removing enemy " + enemyObject.name);
        enemyObjects.Remove(enemyObject);
        enemyManager.enemyObjects = enemyObjects;

        if (enemyObjects.Count <= 0)
        {
            OpenDoor();
        }
    }

    public void OpenDoor()
    {
        SoundManager.Instance.PlayAudio(roomClearSound, audioVolume);
        ActivateParticleSystem();
        if (!currentRoom.roomAlreadyHasDoor)
            doorObject.GetComponentInChildren<Animator>().SetTrigger("OpenDoor");
    }

    public void LoadNewRoom(int roomType)
    {

        bulletHandler.ResetAll();

        if (roomType != 4)
        {
            cardHandler.isActive = true;
            enemyManager.ActivateEnemiesAfterTime();
        }

        if (roomType == 5)
        {
            inv.TrashCanIsOff(true);
        }
        else if (inv.GetTrashCanIsOff())
        {
            inv.TrashCanIsOff(false);
        }

        switch (roomType)
        {
            case 0:
                currentRoomType = 0;
                LoadPremadeRoom(possibleBossRoomsByFloor[currentFloor % possibleBossRoomsByFloor.Count]);
                break;
            case 1:
                currentRoomType = 1;
                Room room = possibleNormalRooms[Random.Range(0, possibleNormalRooms.Count)];
                int iter = 0;
                while (!room.canSpawnOnFloor.Contains(currentFloor))
                {
                    room = possibleNormalRooms[Random.Range(0, possibleNormalRooms.Count)];
                    iter++;
                    if (iter > 100)
                    {
                        break;
                    }
                }
                LoadPremadeRoom(room);
                break;
            case 2:
                currentRoomType = 2;
                Room miniBoss_room = possibleMinibossRooms[Random.Range(0, possibleMinibossRooms.Count)];
                int miniBoss_iter = 0;
                while (!miniBoss_room.canSpawnOnFloor.Contains(currentFloor))
                {
                    miniBoss_room = possibleMinibossRooms[Random.Range(0, possibleMinibossRooms.Count)];
                    miniBoss_iter++;
                    if (miniBoss_iter > 100)
                    {
                        break;
                    }
                }
                LoadPremadeRoom(miniBoss_room);
                break;
            case 3:
                currentRoomType = 3;
                LoadPremadeRoom(possibleTreasureRooms[Random.Range(0, possibleTreasureRooms.Count)]);
                break;
            case 4:
                currentRoomType = 4;
                LoadPremadeRoom(possibleMysteryRooms[Random.Range(0, possibleMysteryRooms.Count)]);
                break;
            case 5:
                currentRoomType = 5;
                LoadPremadeRoom(tutorialRoom);
                break;
            default:
                currentRoomType = 1;
                LoadPremadeRoom(possibleNormalRooms[Random.Range(0, possibleNormalRooms.Count)]);
                break;
        }

        roomSelectScreenGenerator.roomSelectObject.SetActive(false);
    }

    public void ActivateParticleSystem()
    {

        // Set the number of particles to emit for this burst
        particleSystem.Emit(10);

    }


    IEnumerator PauseEnemies()
    {
        originalTimescale = Time.timeScale;

        Time.timeScale = 0;

        yield return new WaitForSeconds(pauseDuration);

        Time.timeScale = originalTimescale;

    }



}
