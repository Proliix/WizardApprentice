using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RoomSelectRoom
{
    public int roomId;
    public int roomLayer;
    public List<RoomSelectRoom> incommingRooms;
    public List<RoomSelectRoom> outgoingRooms;
    public List<GameObject> outgoingLineObjects;
    public GameObject roomIconObject;
    public Vector2 position;
    public Sprite image;
    public int roomType; 
    //0 = Boss room
    //1 = Normal room
    //2 = Miniboss room
    //3 = Treasure room
    //4 = Mystery room

    public RoomSelectRoom()
    {

    }

    public RoomSelectRoom(int roomId, int roomLayer, List<RoomSelectRoom> incommingRooms, List<RoomSelectRoom> outgoingRooms, Vector2 position, Sprite image)
    {
        this.roomId = roomId;
        this.roomLayer = roomLayer;
        this.incommingRooms = incommingRooms;
        this.outgoingRooms = outgoingRooms;
        this.position = position;
        this.image = image;
    }
}
