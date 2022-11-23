using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellHolder : MonoBehaviour
{
    public Vector2 position;
    public GameObject cellObject;
    [SerializeField] GameObject cellHolderTextPrefab;
    List<int> availableCells;
    Text textObject;

    public void SpawnCellHolder(Vector2 pos, int index)
    {
        availableCells = new List<int>();
        this.transform.position = pos;
    }

    public void SetCell(string textToWrite)
    {
        availableCells = new List<int>();
        textObject.text = textToWrite;
    }

    public void SpawnCell(Vector2 pos, Cell cell)
    {
        position = pos;
        if(cellObject != null)
        {
            Destroy(cellObject);
        }
        cellObject = Instantiate(cell.cellPrefab,this.transform);
        cellObject.transform.position = pos;
        int times = Random.Range(0,4);
        cellObject.transform.rotation = Quaternion.Euler(0,0,0);
    }
}
