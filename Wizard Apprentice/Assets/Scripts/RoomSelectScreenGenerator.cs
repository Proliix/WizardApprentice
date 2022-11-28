using Microsoft.Win32.SafeHandles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectScreenGenerator : MonoBehaviour
{
    public GameObject roomSelectObject;
    [SerializeField] RoomManager roomManager;
    [SerializeField] Sprite bossRoomImage;
    [SerializeField] Sprite normalRoomImage;
    [SerializeField] GameObject bossRoomIconPrefab;
    [SerializeField] List<GameObject> roomIconPrefabs;
    [SerializeField] GameObject linePrefab;
    [SerializeField] Transform roomSelectParent;
    [SerializeField] Transform lineParent;
    [SerializeField] int numberOfLayers;
    [SerializeField] int maxRoomsPerLayers;
    [SerializeField] int minRoomsPerLayers;
    [SerializeField] int allowedChangePerLayer;
    [SerializeField] int outgoingSpacing;
    [SerializeField] int chancesForOutgoing;
    [SerializeField] float preferStayingInSetPosition;
    [SerializeField] float randomIconMovement;
    [SerializeField] Vector2 iconDistances;
    List<RoomSelectRoom> allRooms;
    List<List<RoomSelectRoom>> roomsByLayer;

    List<GameObject> objectsCreated;

    // Start is called before the first frame update
    void Start()
    {
        objectsCreated = new List<GameObject>();
        GenerateRoomLayout();
        for(int i = 0; i < numberOfLayers; i++)
        {
            GenerateLayer();
        }
        GenerateBottomLayer();
        GenerateLookOfScreen();
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void GenerateRoomLayout()
    {
        allRooms = new List<RoomSelectRoom>();
        roomsByLayer = new List<List<RoomSelectRoom>>();
        RoomSelectRoom bossRoom = new RoomSelectRoom(0,0,null,null,null,new Vector2(0,400), bossRoomImage);
        allRooms.Add(bossRoom);
        int firstLayerAmount = Random.Range(1, 4);
        List<RoomSelectRoom> firstLayerList = new List<RoomSelectRoom>();
        for(int i = 0; i < firstLayerAmount; i++)
        {
            RoomSelectRoom room = new RoomSelectRoom(allRooms.Count, 1, null, new List<RoomSelectRoom>() { bossRoom }, null, new Vector2(i * 100 + -100, 300), normalRoomImage);
            allRooms.Add(room);
            firstLayerList.Add(room);
        }
        bossRoom.incommingRooms = firstLayerList;
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
        for(int i = 0; i < roomsByLayer[roomsByLayer.Count-1].Count; i++)
        {
            room.outgoingRooms.Add(roomsByLayer[roomsByLayer.Count - 1][i]);
            roomsByLayer[roomsByLayer.Count - 1][i].incommingRooms.Add(room);
        }
        roomsByLayer.Add(rooms);
    }

    public void GenerateLookOfScreen()
    {
        for(int i = 0; i < roomsByLayer.Count; i++)
        {
            for(int j = 0; j < roomsByLayer[i].Count; j++)
            {
                GameObject roomIcon;
                if (roomsByLayer[i][j].outgoingRooms.Count > 0)
                {
                    int roomType = Random.Range(0, roomIconPrefabs.Count);
                    roomIcon = Instantiate(roomIconPrefabs[roomType], roomSelectParent);
                    roomsByLayer[i][j].roomType = roomType + 1;
                }
                else
                {
                    roomIcon = Instantiate(bossRoomIconPrefab, roomSelectParent);
                    roomsByLayer[i][j].roomType = 0;
                }
                Vector2Int temp = new Vector2Int(i,j);
                roomIcon.GetComponent<Button>().onClick.AddListener(delegate { OnClickedRoomIcon(temp); });
                objectsCreated.Add(roomIcon);
                Vector2 averagePosOfOutgoing = new Vector2(j * iconDistances.x - (roomsByLayer[i].Count * iconDistances.x*0.5f), i * -iconDistances.y + 400) * preferStayingInSetPosition;
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
                    DrawLine(position, (roomsByLayer[i][j].outgoingRooms[k].position));
                }
            }
        }
    }

    public void DrawLine(Vector2 startPos, Vector2 endPos)
    {
        GameObject line = Instantiate(linePrefab, lineParent);
        Vector2 pos = (startPos + endPos) / 2;
        line.transform.localPosition = pos;
        //Vector2 dir = (pos - startPos).normalized;
        float theta = Mathf.Atan2(endPos.y-startPos.y, startPos.x - endPos.x);
        if (theta < 0.0)
            theta += Mathf.PI * 2;
        line.transform.localRotation = Quaternion.Euler(0, 0, (Mathf.Rad2Deg * theta - 90) * -1);
        line.transform.localScale = new Vector3(0.03f,(startPos-endPos).magnitude / 100f,1f);
        objectsCreated.Add(line);
        
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
        Debug.Log("Loading room");
        roomManager.LoadNewRoom(roomsByLayer[id.x][id.y].roomType);
    }
}
