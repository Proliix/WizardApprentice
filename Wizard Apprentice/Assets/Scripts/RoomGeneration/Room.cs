using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/Room")]
public class Room : ScriptableObject
{
    public GameObject roomPrefab;
    public Vector2Int roomSize;
    public int roomDifficulty;
}