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
    public List<RoomSelectRoom> sameLayer;
    public Vector2 position;
    public Sprite image;

    public RoomSelectRoom(int roomId, int roomLayer, List<RoomSelectRoom> incommingRooms, List<RoomSelectRoom> outgoingRooms, List<RoomSelectRoom> sameLayer, Vector2 position, Sprite image)
    {
        this.roomId = roomId;
        this.roomLayer = roomLayer;
        this.incommingRooms = incommingRooms;
        this.outgoingRooms = outgoingRooms;
        this.sameLayer = sameLayer;
        this.position = position;
        this.image = image;
    }
}
