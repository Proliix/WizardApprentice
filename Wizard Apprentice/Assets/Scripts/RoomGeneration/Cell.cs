using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Create/Cell")]
public class Cell : ScriptableObject
{
    public int cellID;
    public GameObject cellPrefab;
    public List<int> validCellNeighbors;
}
