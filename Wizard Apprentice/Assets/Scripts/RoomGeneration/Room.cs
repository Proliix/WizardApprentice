using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/Room")]
public class Room : ScriptableObject
{
    public GameObject roomPrefab;
    public Vector2Int roomSize;
    public List<int> canSpawnOnFloor;
    public bool generateRandomRoom;
    public bool roomAlreadyHasDoor;
    public bool isLastRoom = false;
    public MusicType musicType = MusicType.Normal;
}
