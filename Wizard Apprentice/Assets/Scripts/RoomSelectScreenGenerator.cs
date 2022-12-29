using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectScreenGenerator : MonoBehaviour
{
    public GameObject roomSelectObject;
    [SerializeField] GameObject playerPositionObject;
    [SerializeField] RoomSelectController roomSelectController;
    [SerializeField] RoomManager roomManager;
    [SerializeField] Sprite bossRoomImage;
    [SerializeField] Sprite normalRoomImage;
    [SerializeField] GameObject bossRoomIconPrefab;
    [SerializeField] List<GameObject> roomIconPrefabs;
    [SerializeField] GameObject linePrefab;
    [SerializeField] Transform roomSelectParent;
    [SerializeField] Transform lineParent;
    [SerializeField] Transform roomParent;
    [SerializeField] int numberOfLayers;
    [SerializeField] int maxRoomsPerLayers;
    [SerializeField] int minRoomsPerLayers;
    [SerializeField] int allowedChangePerLayer;
    [SerializeField] int outgoingSpacing;
    [SerializeField] int chancesForOutgoing;
    [SerializeField] float preferStayingInSetPosition;
    [SerializeField] float randomIconMovement;
    [SerializeField] Vector2 iconDistances;
    List<RoomSelectRoom> visitedRooms;
    List<List<RoomSelectRoom>> roomsByLayer;
    int currentFloor;

    int totalPool;
    int normalPool;
    int minibossPool;
    int treasurePool;
    int mysteryPool;

    public enum RoomTypes
    {
        BossRoom, NormalRoom, MinibossRoom, TreasureRoom, MysteryRoom, TutorialRoom
    }

    RoomSelectRoom playersCurrentRoom;

    List<GameObject> objectsCreated;

    // Start is called before the first frame update
    void Start()
    {
        currentFloor = 0;
        objectsCreated = new List<GameObject>();
        GenerateRoomLayout();
        for(int i = 0; i < numberOfLayers; i++)
        {
            GenerateLayer();
        }
        GenerateBottomLayer();
        GenerateLookOfScreen();
    }

    public void GenerateAnotherFloor()
    {
        currentFloor++;
        for(int i = objectsCreated.Count-1; i >= 0; i--)
        {
            Destroy(objectsCreated[i]);
        }
        objectsCreated.Clear();
        roomsByLayer.Clear();
        GeneratePoolAmounts();


        GenerateRoomLayout();
        for (int i = 0; i < numberOfLayers; i++)
        {
            GenerateLayer();
        }
        GenerateBottomLayer();
        GenerateLookOfScreen();
        roomManager.currentFloor = currentFloor;
    }

    // Update is called once per frame
    void Update()
    {
       if(Input.GetKeyDown(KeyCode.I))
       {
            GenerateLookOfScreen();
       }
    }

    public void GenerateRoomLayout()
    {
        visitedRooms = new List<RoomSelectRoom>();
        roomsByLayer = new List<List<RoomSelectRoom>>();
    }

    public void GenerateLayer()
    {
        List<RoomSelectRoom> roomsInThisLayer = new List<RoomSelectRoom>();

        //Select Number of rooms in layer
        int amountRoomsInThisLayer = 1;
        if(roomsByLayer.Count > 0)
        {
            amountRoomsInThisLayer = Mathf.Clamp(Random.Range(roomsByLayer[roomsByLayer.Count - 1].Count - allowedChangePerLayer, roomsByLayer[roomsByLayer.Count - 1].Count + allowedChangePerLayer + 1), minRoomsPerLayers, maxRoomsPerLayers);
        }

        //Initialize rooms
        for(int i = 0; i < amountRoomsInThisLayer; i++)
        {
            RoomSelectRoom room = new RoomSelectRoom();
            roomsInThisLayer.Add(room);
            room.incommingRooms = new List<RoomSelectRoom>();
            room.outgoingRooms = new List<RoomSelectRoom>();
            room.outgoingLineObjects = new List<GameObject>();
        }

        //Set first outgoing
        if (roomsByLayer.Count > 0)
        {
            int largestChosen = 0;
            for (int i = 0; i < amountRoomsInThisLayer; i++)
            {
                for(int j = 0; j < chancesForOutgoing; j++)
                {
                    int chosenRoom = Random.Range(largestChosen, Mathf.Min(roomsByLayer[roomsByLayer.Count - 1].Count, largestChosen + outgoingSpacing + 1));
                    if (!roomsInThisLayer[i].outgoingRooms.Contains(roomsByLayer[roomsByLayer.Count - 1][chosenRoom]))
                    {
                        roomsInThisLayer[i].outgoingRooms.Add(roomsByLayer[roomsByLayer.Count - 1][chosenRoom]);
                        roomsByLayer[roomsByLayer.Count - 1][chosenRoom].incommingRooms.Add(roomsInThisLayer[i]);
                        if(chosenRoom > largestChosen)
                        {
                            largestChosen = chosenRoom;
                        }
                    }
                }
            }
        }

        //Update incomming in layer above
        if (roomsByLayer.Count > 0)
        {
            for (int i = 0; i < roomsByLayer[roomsByLayer.Count - 1].Count; i++)
            {
                if (roomsByLayer[roomsByLayer.Count - 1][i].incommingRooms.Count == 0)
                {
                    int minRange = 0;
                    int maxRange = roomsInThisLayer.Count - 1;
                    if (i > 0)
                    {
                        int highestRoom = 0;
                        for (int j = 0; j < roomsByLayer[roomsByLayer.Count - 1][i - 1].incommingRooms.Count; j++)
                        {
                            for (int k = 0; k < roomsInThisLayer.Count; k++)
                            {
                                if (roomsInThisLayer[k] == roomsByLayer[roomsByLayer.Count - 1][i - 1].incommingRooms[j])
                                {
                                    if (k > highestRoom)
                                    {
                                        highestRoom = k;
                                    }
                                }
                            }
                        }
                        minRange = highestRoom;
                    }
                    if (i + 1 < roomsByLayer[roomsByLayer.Count - 1].Count)
                    {
                        int lowestRoom = roomsInThisLayer.Count;
                        for (int j = 0; j < roomsByLayer[roomsByLayer.Count - 1][i + 1].incommingRooms.Count; j++)
                        {
                            for (int k = 0; k < roomsInThisLayer.Count; k++)
                            {
                                if (roomsInThisLayer[k] == roomsByLayer[roomsByLayer.Count - 1][i + 1].incommingRooms[j])
                                {
                                    if (k < lowestRoom)
                                    {
                                        lowestRoom = k;
                                    }
                                }
                            }
                        }
                        maxRange = lowestRoom;
                    }
                    int chosenRoom = Random.Range(minRange, maxRange);
                    roomsByLayer[roomsByLayer.Count - 1][i].incommingRooms.Add(roomsInThisLayer[chosenRoom]);
                    roomsInThisLayer[chosenRoom].outgoingRooms.Add(roomsByLayer[roomsByLayer.Count - 1][i]);
                }
            }
        }

        //Update roomsByLayer
        roomsByLayer.Add(roomsInThisLayer);

    }

    public void GenerateBottomLayer()
    {
        List<RoomSelectRoom> rooms = new List<RoomSelectRoom>();
        RoomSelectRoom room = new RoomSelectRoom();
        rooms.Add(room);
        room.incommingRooms = new List<RoomSelectRoom>();
        room.outgoingRooms = new List<RoomSelectRoom>();
        room.outgoingLineObjects = new List<GameObject>();
        for(int i = 0; i < roomsByLayer[roomsByLayer.Count-1].Count; i++)
        {
            room.outgoingRooms.Add(roomsByLayer[roomsByLayer.Count - 1][i]);
            roomsByLayer[roomsByLayer.Count - 1][i].incommingRooms.Add(room);
        }
        roomsByLayer.Add(rooms);
        SetPlayerRoom(room);
    }

    public void Open()
    {
        roomSelectController.SetPosition(playersCurrentRoom.position.y);
        playerPositionObject.transform.localPosition = playersCurrentRoom.position + new Vector2(0,15);
        playersCurrentRoom.roomIconObject.SetActive(false);
        for(int i = 0; i < playersCurrentRoom.outgoingLineObjects.Count; i++)
        {
            playersCurrentRoom.outgoingLineObjects[i].GetComponent<Image>().color = Color.yellow;
        }
    }

    public void SetPlayerRoom(RoomSelectRoom playerRoom)
    {
        playersCurrentRoom = playerRoom;
    }

    public void GeneratePoolAmounts()
    {
        normalPool = roomManager.normalRoomsPoolAmount;
        minibossPool = roomManager.miniBossRoomsPoolAmount;
        treasurePool = roomManager.treasureRoomsPoolAmount;
        mysteryPool = roomManager.mysteryRoomsPoolAmount;
        totalPool = normalPool+minibossPool+treasurePool+mysteryPool;
    }

    public int ChooseRoomTypeFromPool()
    {
        if(totalPool <= 0)
        {
            GeneratePoolAmounts();
        }
        int randValue = Random.Range(0, totalPool);
        if(randValue < normalPool)
        {
            totalPool--;
            normalPool--;
            return 1;
        }
        else if (randValue < minibossPool+normalPool)
        {
            totalPool--;
            minibossPool--;
            return 2;
        }
        else if (randValue < treasurePool+minibossPool+normalPool)
        {
            totalPool--;
            treasurePool--;
            return 3;
        }
        else if (randValue < mysteryPool+ treasurePool + minibossPool + normalPool)
        {
            totalPool--;
            mysteryPool--;
            return 4;
        }
        else
        {
            return 1;
        }
    }

    public void GenerateLookOfScreen()
    {
        for(int i = 0; i < roomsByLayer.Count; i++)
        {
            for(int j = 0; j < roomsByLayer[i].Count; j++)
            {
                GameObject roomIcon;
                if (roomsByLayer[i][j].incommingRooms.Count == 0)
                {
                    int roomType = 5;
                    roomIcon = Instantiate(roomIconPrefabs[roomType - 1], roomParent);
                    roomsByLayer[i][j].roomIconObject = roomIcon;
                    roomsByLayer[i][j].roomType = roomType;
                }
                else if (roomsByLayer[i][j].outgoingRooms.Count > 0)
                {
                    int roomType = ChooseRoomTypeFromPool();
                    roomIcon = Instantiate(roomIconPrefabs[roomType-1], roomParent);
                    roomsByLayer[i][j].roomIconObject = roomIcon;
                    roomsByLayer[i][j].roomType = roomType;
                }
                else
                {
                    roomIcon = Instantiate(bossRoomIconPrefab, roomParent);
                    roomsByLayer[i][j].roomType = 0;
                }
                Vector2Int temp = new Vector2Int(i,j);
                roomIcon.GetComponent<Button>().onClick.AddListener(delegate { OnClickedRoomIcon(temp); });
                objectsCreated.Add(roomIcon);
                Vector2 averagePosOfOutgoing = new Vector2(j * iconDistances.x - ((roomsByLayer[i].Count-1) * iconDistances.x*0.5f), i * -iconDistances.y + 400) * preferStayingInSetPosition;
                if (roomsByLayer[i][j].outgoingRooms.Count > 0)
                {
                    for (int k = 0; k < roomsByLayer[i][j].outgoingRooms.Count; k++)
                    {
                        averagePosOfOutgoing += roomsByLayer[i][j].outgoingRooms[k].position;
                    }
                }
                averagePosOfOutgoing /= (roomsByLayer[i][j].outgoingRooms.Count+ preferStayingInSetPosition);
                Vector3 position = new Vector3(averagePosOfOutgoing.x, i * -iconDistances.y + 400 + Random.Range(-randomIconMovement, randomIconMovement), -0.1f);
                roomIcon.transform.localPosition = position;
                roomsByLayer[i][j].position = position;

                for (int k = 0; k < roomsByLayer[i][j].outgoingRooms.Count; k++)
                {
                    roomsByLayer[i][j].outgoingLineObjects.Add(DrawLine(position, (roomsByLayer[i][j].outgoingRooms[k].position)));
                }
            }
        }
    }

    public GameObject DrawLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject line = Instantiate(linePrefab, lineParent);
        Vector2 pos = (startPos + endPos) / 2;
        line.transform.localPosition = pos;
        //Vector2 dir = (pos - startPos).normalized;
        float theta = Mathf.Atan2(endPos.y-startPos.y, startPos.x - endPos.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        line.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
        line.GetComponent<RectTransform>().sizeDelta = new Vector2(100,(startPos-endPos).magnitude / line.transform.localScale.y);
        objectsCreated.Add(line);
        return line;
        
    }

    public void DestroyAllCreatedObjects()
    {
        for(int i = objectsCreated.Count-1; i >= 0; i--)
        {
            Destroy(objectsCreated[i]);
            objectsCreated.RemoveAt(i);
        }
    }

    public void OnClickedRoomIcon(Vector2Int id)
    {
        for(int i = 0; i < playersCurrentRoom.outgoingRooms.Count; i++)
        {
            if(roomsByLayer[id.x][id.y] == playersCurrentRoom.outgoingRooms[i])
            {
                Debug.Log("Loading room");
                visitedRooms.Add(roomsByLayer[id.x][id.y]);
                roomManager.LoadNewRoom(roomsByLayer[id.x][id.y].roomType);
                playersCurrentRoom.roomIconObject.SetActive(true);
                for(int j = 0; j < roomsByLayer[id.x].Count; j++)
                {
                    roomsByLayer[id.x][j].roomIconObject.GetComponent<Image>().color = new Color(1, 1, 1, 0.8f);
                    for(int k = 0; k < roomsByLayer[id.x][j].outgoingLineObjects.Count; k++)
                    {
                        roomsByLayer[id.x][j].outgoingLineObjects[k].GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f);
                    }
                }
                for (int j = 0; j < playersCurrentRoom.outgoingLineObjects.Count; j++)
                {
                    if(playersCurrentRoom.outgoingRooms[j] == roomsByLayer[id.x][id.y])
                    {
                        playersCurrentRoom.outgoingLineObjects[j].GetComponent<Image>().color = new Color(0.6f,0.1f,0.1f);
                    }
                    else
                    {
                        playersCurrentRoom.outgoingLineObjects[j].GetComponent<Image>().color = new Color(0.25f,0.25f,0.25f);
                    }
                }
                playersCurrentRoom = roomsByLayer[id.x][id.y];
                return;
            }
        }
        Debug.Log("You clicked on a room that was inaccessible!");
    }
}
