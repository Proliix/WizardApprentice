using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{

    [SerializeField] Vector2Int size;
    [SerializeField] GameObject cellHolderPrefab;
    [SerializeField] List<Cell> cells;
    Vector2Int prevSize;
    List<GameObject> cellHolders;
    List<int>[] allCellsAvailability;
    public List<GameObject> walls;
    public List<GameObject> topOfWalls;
    public List<GameObject> sideWalls;
    List<GameObject> wallsCreated;
    int iterations;
    // Start is called before the first frame update
    void Start()
    {
        cellHolders = new List<GameObject>();
        wallsCreated = new List<GameObject>();
    }


    public void GenerateCells()
    {
        allCellsAvailability = new List<int>[size.x * size.y];
        
        for (int i = 0; i < size.y; i++)
        {
            for (int j = 0; j < size.x; j++)
            {
                List<int> listOfAllCells = new List<int>();
                for (int k = 0; k < cells.Count; k++)
                {
                    listOfAllCells.Add(cells[k].cellID);
                }
                GameObject cellHolder = Instantiate(cellHolderPrefab);
                cellHolder.GetComponent<CellHolder>().SpawnCellHolder(new Vector2(j, i),i*size.x + j);
                cellHolder.GetComponent<CellHolder>().SpawnCell(new Vector2(j, i), cells[Random.Range(0, cells.Count)]);
                cellHolders.Add(cellHolder);
                allCellsAvailability[i * size.x + j] = listOfAllCells;
            }
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(prevSize.x != size.x || prevSize.y != size.y)
        {
            if (cellHolders.Count != 0)
            {
                for (int i = cellHolders.Count - 1; i >= 0; i--)
                {
                    Destroy(cellHolders[i]);
                    cellHolders.RemoveAt(i);
                }
            }
            GenerateCells();
        }

        if(Input.GetKeyDown(KeyCode.P) || Input.GetKey(KeyCode.O))
        {
            while(true)
            {
                try
                {
                    Step();
                }
                catch
                {
                    for (int j = 0; j < 1; j++)
                    {
                        ChangeCellsToMajority();
                    }
                    ColorCells();
                    break;
                }
            }
            GenerateWalls();
        }

        if (Input.GetKeyDown(KeyCode.I))
        {
            ChangeCellsToMajority();
        }

        prevSize = size;
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (Random.value < 0.5f)
            {
                size.x++;
            }
            else
            {
                size.y++;
            }
        }

    }

    private void GenerateWalls()
    {
        if(wallsCreated.Count != 0)
        {
            for (int i = wallsCreated.Count-1; i >= 0; i--)
            {
                Destroy(wallsCreated[i]);
                wallsCreated.RemoveAt(i);
            }
        }
        float scale = 5;
        for(float i = 0; i < size.x; i+= scale)
        {
            GameObject mainWall = Instantiate(walls[Random.Range(0, walls.Count)]);
            mainWall.transform.position = new Vector3((i-0.5f) + (scale / 2),(size.y-0.5f)+ 0.25f*scale);
            wallsCreated.Add(mainWall);
            GameObject topOfWall1 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)]);
            topOfWall1.transform.position = new Vector3((i-0.5f)+ 0.25f*scale, size.y+0.125f*scale + 2,-0.01f);
            wallsCreated.Add(topOfWall1);
            GameObject topOfWall2 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)]);
            topOfWall2.transform.position = new Vector3((i - 0.5f) + 0.25f * scale + scale/2, size.y+0.125f*scale + 2,-0.01f);
            wallsCreated.Add(topOfWall2);
        }
        for (float i = 0; i < size.y + scale/2; i+= scale / 2)
        {
            GameObject sideWall = Instantiate(sideWalls[Random.Range(0, sideWalls.Count)]);
            sideWall.transform.position = new Vector3(-1,i+0.75f,-0.01f);
            wallsCreated.Add(sideWall);
            GameObject sideWall2 = Instantiate(sideWalls[Random.Range(0, sideWalls.Count)]);
            sideWall2.transform.position = new Vector3(size.x, i+0.75f, -0.01f);
            wallsCreated.Add(sideWall2);
        }
        for (float i = 0; i < size.x; i += scale)
        {
            GameObject topOfWall1 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)]);
            topOfWall1.transform.position = new Vector3((i - 0.5f) + 0.25f * scale, -1f, -0.01f);
            wallsCreated.Add(topOfWall1);
            GameObject topOfWall2 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)]);
            topOfWall2.transform.position = new Vector3((i - 0.5f) + 0.25f * scale + scale / 2, -1f, -0.01f);
            wallsCreated.Add(topOfWall2);
        }
    }

    public void Step()
    {
        //iterations++;
        //if (iterations % 100 == 0)
        //{
        //    for (int i = 0; i < allCellsAvailability.Length; i++)
        //    {
        //        string cellText = "";
        //        for (int j = 0; j < allCellsAvailability[i].Count; j++)
        //        {
        //            cellText += allCellsAvailability[i][j] + ", ";
        //        }
        //        cellHolders[i].GetComponent<CellHolder>().SetCell(cellText);
        //        if (allCellsAvailability[i].Count == 1)
        //        {
        //            cellHolders[i].GetComponent<CellHolder>().SpawnCell(new Vector2(i % size.x, Mathf.FloorToInt(i / size.x)), cells[allCellsAvailability[i][0]]);
        //        }
        //    }
        //}
        //Choose cell with least entropy
        int currentLowest = 9999;
        int lowestIndex = 0;
        List<int> cellsWithLowest = new List<int>();
        for(int i = 0; i < allCellsAvailability.Length; i++)
        {
            if(allCellsAvailability[i].Count == currentLowest && allCellsAvailability[i].Count > 1)
            {
                cellsWithLowest.Add(i);
            }
            if(allCellsAvailability[i].Count < currentLowest && allCellsAvailability[i].Count > 1)
            {
                cellsWithLowest.Clear();
                cellsWithLowest.Add(i);
                currentLowest = allCellsAvailability[i].Count;
                lowestIndex = i;
            }
        }
        if(lowestIndex == 0)
        {
            lowestIndex = cellsWithLowest[Random.Range(0,cellsWithLowest.Count)];
        }

        //Collapse Cell to a single state
        int newCellID = cells[allCellsAvailability[lowestIndex][Random.Range(0, allCellsAvailability[lowestIndex].Count)]].cellID;
        List<int> oneCell = new List<int>();
        oneCell.Add(newCellID);
        allCellsAvailability[lowestIndex] = oneCell;

        //Find neighbors
        List<int> neighboringCells = new List<int>();
        if(lowestIndex - size.x >= 0)
        {
            neighboringCells.Add(lowestIndex - size.x);
        }
        if(lowestIndex % size.x != 0)
        {
            neighboringCells.Add(lowestIndex - 1);
        }
        if(lowestIndex % size.x < (lowestIndex+1) % size.x)
        {
            neighboringCells.Add(lowestIndex + 1);
        }
        if(lowestIndex + size.x < size.x*size.y)
        {
            neighboringCells.Add(lowestIndex + size.x);
        }

        //Update neighboring cells
        UpdateNeighborsToCell(lowestIndex);
        //cellHolders[lowestIndex].GetComponent<CellHolder>().SetCell(cellString);

        //Keep Updating neighboring cells if changes are made
        //Step is done
        //Repeat
    }

    public void UpdateCellBasedOnNeighbor(int cellToBeUpdated, int neighboringCell)
    {
        List<int> cellsAvaliable = new List<int>();
        for(int i = 0; i < allCellsAvailability[cellToBeUpdated].Count; i++)
        {
            bool sharesID = true;
            for(int j = 0; j < allCellsAvailability[neighboringCell].Count; j++)
            {
                bool hasSimilar = false;
                for(int k = 0; k < cells[allCellsAvailability[cellToBeUpdated][i]].validCellNeighbors.Count; k++)
                {
                    for (int l = 0; l < cells[allCellsAvailability[neighboringCell][j]].validCellNeighbors.Count; l++)
                    {
                        if (cells[allCellsAvailability[cellToBeUpdated][i]].validCellNeighbors[k] == cells[allCellsAvailability[neighboringCell][j]].validCellNeighbors[l])
                        {
                            hasSimilar = true;
                        }
                    }
                    if (!hasSimilar)
                    {
                        sharesID = false;
                    }
                }
                
                
            }
            if(sharesID)
            {
                cellsAvaliable.Add(allCellsAvailability[cellToBeUpdated][i]);
            }
        }


        if(cellsAvaliable.Count != allCellsAvailability[cellToBeUpdated].Count)
        {
            
            allCellsAvailability[cellToBeUpdated] = cellsAvaliable;
            
            //Find neighbors
            int lowestIndex = cellToBeUpdated;
            List<int> neighboringCells = new List<int>();
            if (lowestIndex - size.x >= 0)
            {
                neighboringCells.Add(lowestIndex - size.x);
            }
            if (lowestIndex % size.x != 0)
            {
                neighboringCells.Add(lowestIndex - 1);
            }
            if (lowestIndex % size.x < (lowestIndex + 1) % size.x)
            {
                neighboringCells.Add(lowestIndex + 1);
            }
            if (lowestIndex + size.x < size.x * size.y)
            {
                neighboringCells.Add(lowestIndex + size.x);
            }

            string cellString = "";
            for (int i = 0; i < allCellsAvailability[lowestIndex].Count; i++)
            {
                cellString += allCellsAvailability[lowestIndex][i] + ", ";
            }

            //Update neighboring cells
            for (int i = 0; i < neighboringCells.Count; i++)
            {
                UpdateCellBasedOnNeighbor(neighboringCells[i], lowestIndex);
            }
            cellHolders[lowestIndex].GetComponent<CellHolder>().SetCell(cellString);
        }
    }

    public void UpdateNeighborsToCell(int targetCell)
    {
        //Find neighbors with a count greater than 1
        //Find neighbors
        int lowestIndex = targetCell;
        List<int> neighboringCells = new List<int>();
        if (lowestIndex - size.x >= 0)
        {
            neighboringCells.Add(lowestIndex - size.x);
        }
        if (lowestIndex % size.x != 0)
        {
            neighboringCells.Add(lowestIndex - 1);
        }
        if (lowestIndex % size.x < (lowestIndex + 1) % size.x)
        {
            neighboringCells.Add(lowestIndex + 1);
        }
        if (lowestIndex + size.x < size.x * size.y)
        {
            neighboringCells.Add(lowestIndex + size.x);
        }

        //Compare cells and remove ones that can't work in the neighbor.
        for(int i = 0; i < neighboringCells.Count; i++)
        {
            int neighborCount = allCellsAvailability[neighboringCells[i]].Count;
            if (allCellsAvailability[neighboringCells[i]].Count > 1)
            {
                List<int> possibleCells = new List<int>();
                for(int j = 0; j < allCellsAvailability[targetCell].Count; j++)
                {
                    for (int k = 0; k < cells[allCellsAvailability[targetCell][j]].validCellNeighbors.Count; k++)
                    {
                        possibleCells.Add(cells[allCellsAvailability[targetCell][j]].validCellNeighbors[k]);
                    }
                }
                for(int j = 0; j < allCellsAvailability[neighboringCells[i]].Count; j++)
                {
                    bool sharesAny = false;
                    for (int k = 0; k < possibleCells.Count; k++)
                    {
                        if (allCellsAvailability[neighboringCells[i]][j] == possibleCells[k])
                        {
                            sharesAny = true;
                            break;
                        }
                    }
                    if (!sharesAny)
                    {
                        allCellsAvailability[neighboringCells[i]].Remove(allCellsAvailability[neighboringCells[i]][j]);
                    }
                }
                
            }
            if (allCellsAvailability[neighboringCells[i]].Count != neighborCount)
            {
                UpdateNeighborsToCell(neighboringCells[i]);
            }
        }
        //If the neighbor changes call UpdateNeighborsToCell.
    }


    public void ChangeCellsToMajority()
    {
        for(int i = 0; i < allCellsAvailability.Length; i++)
        {
            int amountTotal = 0;
            int[] amountAround = new int[cells.Count];
            if(i - size.x >= 0 && i % size.x != 0)
            {
                amountAround[cells[allCellsAvailability[i - size.x - 1][0]].cellID]++;
                amountTotal++;
            }
            if (i - size.x >= 0)
            {
                amountAround[cells[allCellsAvailability[i - size.x][0]].cellID]++;
                amountTotal++;
            }
            if (i - size.x >= 0 && i % size.x < (i + 1) % size.x)
            {
                amountAround[cells[allCellsAvailability[i - size.x + 1][0]].cellID]++;
                                amountTotal++;
            }

            if (i % size.x != 0)
            {
                amountAround[cells[allCellsAvailability[i - 1][0]].cellID]++;
                                amountTotal++;
            }
            if(i % size.x < (i + 1) % size.x)
            {
                amountAround[cells[allCellsAvailability[i + 1][0]].cellID]++;
                                amountTotal++;
            }

            if (i + size.x < allCellsAvailability.Length && i % size.x != 0)
            {
                amountAround[cells[allCellsAvailability[i + size.x - 1][0]].cellID]++;
                                amountTotal++;
            }
            if (i + size.x < allCellsAvailability.Length)
            {
                amountAround[cells[allCellsAvailability[i + size.x][0]].cellID]++;
                                amountTotal++;
            }
            if (i + size.x + 1 < allCellsAvailability.Length && i % size.x < (i + 1) % size.x)
            {
                amountAround[cells[allCellsAvailability[i + size.x + 1][0]].cellID]++;
                                amountTotal++;
            }

            int maxNum = 0;
            int maxIndex = 0;
            for(int j = 0; j < amountAround.Length; j++)
            {
                if (amountAround[j] > maxNum)
                {
                    maxNum = amountAround[j];
                    maxIndex = j;
                }
            }

            if (allCellsAvailability[i][0] != maxIndex && amountAround[allCellsAvailability[i][0]] < amountAround[maxIndex] - 1)
            {
                List<int> oneNum = new List<int>();
                oneNum.Add(maxIndex);
                allCellsAvailability[i] = oneNum;
            }
            //cellHolders[i].GetComponent<CellHolder>().SetCell(amountTotal.ToString());
            //cellHolders[i].GetComponent<CellHolder>().SpawnCell(new Vector2(i % size.x, Mathf.FloorToInt(i / size.x)), cells[allCellsAvailability[i][0]]);

        }
    }

    public void ColorCells()
    {
        for(int i = 0; i < allCellsAvailability.Length; i++)
        {
            cellHolders[i].GetComponent<CellHolder>().SpawnCell(new Vector2(i % size.x, Mathf.FloorToInt(i / size.x)), cells[allCellsAvailability[i][0]]);
        }
    }

}
