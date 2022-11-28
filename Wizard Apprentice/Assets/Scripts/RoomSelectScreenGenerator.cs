using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectScreenGenerator : MonoBehaviour
{
    [SerializeField] Sprite bossRoomImage;
    [SerializeField] Sprite normalRoomImage;
    [SerializeField] GameObject roomIconPrefab;
    [SerializeField] Transform roomSelectParent;
    [SerializeField] int maxRoomsPerLayers;
    [SerializeField] int minRoomsPerLayers;
    [SerializeField] int allowedChangePerLayer;
    List<RoomSelectRoom> allRooms;
    List<List<RoomSelectRoom>> roomsByLayer;

    // Start is called before the first frame update
    void Start()
    {
        GenerateRoomLayout();
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

        for(int i = 0; i < allRooms.Count; i++)
        {
            GameObject roomIcon = Instantiate(roomIconPrefab, roomSelectParent);
            roomIcon.transform.localPosition = new Vector3(allRooms[i].position.x, allRooms[i].position.y, 0);
            roomIcon.GetComponent<Image>().sprite = allRooms[i].image;
        }
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
        }

        //Set first outgoing
        if (roomsByLayer.Count > 0)
        {
            for (int i = 0; i < amountRoomsInThisLayer; i++)
            {
                int iterations = 0;
                while(iterations < 100)
                {
                    iterations++;
                }
                //roomsInThisLayer[i].outgoingRooms = 
            }
        }
    }
}
