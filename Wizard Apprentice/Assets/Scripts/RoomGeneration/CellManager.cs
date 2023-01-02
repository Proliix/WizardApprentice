using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellManager : MonoBehaviour
{

    [SerializeField] Vector2Int size;
    [SerializeField] GameObject cellHolderPrefab;
    [SerializeField] List<Cell> cells;
    [SerializeField] Sprite slicedTileSprite;
    [SerializeField] Sprite spriteMaskSprite;
    [SerializeField] GameObject tileObject;
    [SerializeField] GameObject colliderObject;
    [SerializeField] GameObject topWall, rightWall, leftWall, botWall, cornerWall;
    Vector2Int prevSize;
    List<GameObject> cellHolders;
    List<int>[] allCellsAvailability;
    public List<GameObject> walls;
    public List<GameObject> topOfWalls;
    public List<GameObject> sideWalls;
    List<GameObject> wallsCreated;
    Transform roomParent;
    int iterations;
    // Start is called before the first frame update
    void Start()
    {
        cellHolders = new List<GameObject>();
        wallsCreated = new List<GameObject>();
    }

    public void GenerateRoom(Vector2Int size, Transform cellParent)
    {
        roomParent = cellParent;
        if (cellHolders == null)
        {
            cellHolders = new List<GameObject>();
        }
        else
        {
            cellHolders.Clear();
        }
        if (wallsCreated == null)
        {
            wallsCreated = new List<GameObject>();
        }
        else
        {
            wallsCreated.Clear();
        }
        this.size = size;
        GenerateCells();

        while (true)
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

    public void GenerateRoom(Vector2Int size, Transform cellParent, bool isNewTiles)
    {
        roomParent = cellParent;
        this.size = size;
        Generate9SliceFloor(size, cellParent);
        //GenerateColliderWalls(size, cellParent);
        GenerateWallSprites(size, cellParent);
    }

    public void GenerateWallSprites(Vector2Int size, Transform cellParent)
    {
        bool yDivideable = false;
        bool xDivideable = false;

        if (size.y % 2 == 0)
            yDivideable = true;

        if (size.x % 2 == 0)
            xDivideable = true;

        float addNumY = yDivideable ? 0.5f : 0;
        GameObject newTopWall = Instantiate(topWall, new Vector3(size.x / 2, size.y + (1.5f + addNumY), 0), topWall.transform.rotation, cellParent);
        newTopWall.GetComponent<SpriteRenderer>().size = new Vector2(size.x, newTopWall.GetComponent<SpriteRenderer>().size.y);
        newTopWall.GetComponent<BoxCollider2D>().size = newTopWall.GetComponent<SpriteRenderer>().size;

        GameObject topWallFix = Instantiate(botWall, new Vector3(size.x / 2, size.y + (3 + addNumY), 0), botWall.transform.rotation, cellParent);
        topWallFix.GetComponent<SpriteRenderer>().size = new Vector2(size.x, topWallFix.GetComponent<SpriteRenderer>().size.y);
        topWallFix.GetComponent<BoxCollider2D>().size = topWallFix.GetComponent<SpriteRenderer>().size;

        float addNumXLeft = xDivideable ? 0 : -0.5f;
        float addNumXRight = xDivideable ? 0.5f : 0;
        GameObject newLeftCornerTop = Instantiate(cornerWall, new Vector3(-0.5f + addNumXLeft, size.y + (3f + addNumY), 0), cornerWall.transform.rotation, cellParent);
        newLeftCornerTop.GetComponent<BoxCollider2D>().size = newLeftCornerTop.GetComponent<SpriteRenderer>().size;
        newLeftCornerTop.GetComponent<SpriteRenderer>().flipY = true;

        GameObject newRightCornerTop = Instantiate(cornerWall, new Vector3(size.x + addNumXRight, size.y + (3f + addNumY), 0), cornerWall.transform.rotation, cellParent);
        newRightCornerTop.GetComponent<SpriteRenderer>().flipX = true;
        newRightCornerTop.GetComponent<SpriteRenderer>().flipY = true;
        newRightCornerTop.GetComponent<BoxCollider2D>().size = newRightCornerTop.GetComponent<SpriteRenderer>().size;

        addNumY = yDivideable ? 1.5f : 1f;
        GameObject newLeftWall = Instantiate(leftWall, new Vector3(-0.5f + addNumXLeft, (size.y / 2f) + addNumY, 0), leftWall.transform.rotation, cellParent);
        newLeftWall.GetComponent<SpriteRenderer>().size = new Vector2(newLeftWall.GetComponent<SpriteRenderer>().size.x, size.y + 3f);
        newLeftWall.GetComponent<BoxCollider2D>().size = newLeftWall.GetComponent<SpriteRenderer>().size;

        GameObject newRightWall = Instantiate(rightWall, new Vector3(size.x + addNumXRight, (size.y / 2f) + addNumY, 0), rightWall.transform.rotation, cellParent);
        newRightWall.GetComponent<SpriteRenderer>().size = new Vector2(newRightWall.GetComponent<SpriteRenderer>().size.x, size.y + 3f);
        newRightWall.GetComponent<BoxCollider2D>().size = newRightWall.GetComponent<SpriteRenderer>().size;

        addNumY = yDivideable ? 0.5f : 1;
        GameObject newLeftCornerBot = Instantiate(cornerWall, new Vector3(-0.5f + addNumXLeft, -addNumY, 0), cornerWall.transform.rotation, cellParent);
        newLeftCornerBot.GetComponent<BoxCollider2D>().size = newLeftCornerBot.GetComponent<SpriteRenderer>().size;

        GameObject newRightCornerBot = Instantiate(cornerWall, new Vector3(size.x + addNumXRight, -addNumY, 0), cornerWall.transform.rotation, cellParent);
        newRightCornerBot.GetComponent<SpriteRenderer>().flipX = true;
        newRightCornerBot.GetComponent<BoxCollider2D>().size = newRightCornerBot.GetComponent<SpriteRenderer>().size;

        GameObject newBotWall = Instantiate(botWall, new Vector3(size.x / 2, -addNumY, 0), botWall.transform.rotation, cellParent);
        newBotWall.GetComponent<SpriteRenderer>().size = new Vector2(size.x, newBotWall.GetComponent<SpriteRenderer>().size.y);
        newBotWall.GetComponent<BoxCollider2D>().size = newBotWall.GetComponent<SpriteRenderer>().size;

    }

    public void Generate9SliceFloor(Vector2Int size, Transform cellParent)
    {
        GameObject sliced = Instantiate(tileObject, cellParent);
        sliced.GetComponent<SpriteRenderer>().sprite = slicedTileSprite;
        sliced.transform.position = Vector2.zero + size / 2;
        sliced.GetComponent<SpriteRenderer>().size = size;
        GameObject mask = new GameObject();
        mask.AddComponent<SpriteMask>();
        mask.GetComponent<SpriteMask>().sprite = spriteMaskSprite;
        mask.transform.parent = sliced.transform;
        mask.name = "Sprite mask";
        mask.transform.localPosition = Vector3.zero;
        mask.transform.localScale = new Vector3(size.x, size.y, 0);
    }

    public void GenerateColliderWalls(Vector2Int size, Transform cellParent)
    {
        GameObject objcet1 = Instantiate(colliderObject, cellParent);
        objcet1.transform.position = new Vector3(0, 0, 0);
        objcet1.GetComponent<BoxCollider2D>().size = size * 2;
        objcet1.GetComponent<BoxCollider2D>().offset = new Vector2(size.x * -1f + 0.25f, 0);

        GameObject objcet2 = Instantiate(colliderObject, cellParent);
        objcet2.transform.position = new Vector3(0, 0, 0);
        objcet2.GetComponent<BoxCollider2D>().size = size * 2;
        objcet2.GetComponent<BoxCollider2D>().offset = new Vector2(size.x * 2f - 1.25f, 0);

        GameObject objcet3 = Instantiate(colliderObject, cellParent);
        objcet3.transform.position = new Vector3(0, 0, 0);
        objcet3.GetComponent<BoxCollider2D>().size = size * 2;
        objcet3.GetComponent<BoxCollider2D>().offset = new Vector2(0, size.y * -1f + 0.25f);

        GameObject objcet4 = Instantiate(colliderObject, cellParent);
        objcet4.transform.position = new Vector3(0, 0, 0);
        objcet4.GetComponent<BoxCollider2D>().size = size * 2;
        objcet4.GetComponent<BoxCollider2D>().offset = new Vector2(0, size.y * 2f - 1.25f);
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
                GameObject cellHolder = Instantiate(cellHolderPrefab, roomParent);
                cellHolder.GetComponent<CellHolder>().SpawnCellHolder(new Vector2(j, i), i * size.x + j);
                cellHolder.GetComponent<CellHolder>().SpawnCell(new Vector2(j, i), cells[Random.Range(0, cells.Count)]);
                cellHolders.Add(cellHolder);
                allCellsAvailability[i * size.x + j] = listOfAllCells;
            }
        }
    }
    // Update is called once per frame
    //void Update()
    //{
    //    if(prevSize.x != size.x || prevSize.y != size.y)
    //    {
    //        if (cellHolders.Count != 0)
    //        {
    //            for (int i = cellHolders.Count - 1; i >= 0; i--)
    //            {
    //                Destroy(cellHolders[i]);
    //                cellHolders.RemoveAt(i);
    //            }
    //        }
    //        GenerateCells();
    //    }

    //    if(Input.GetKeyDown(KeyCode.P) || Input.GetKey(KeyCode.O))
    //    {
    //        while(true)
    //        {
    //            try
    //            {
    //                Step();
    //            }
    //            catch
    //            {
    //                for (int j = 0; j < 1; j++)
    //                {
    //                    ChangeCellsToMajority();
    //                }
    //                ColorCells();
    //                break;
    //            }
    //        }
    //        GenerateWalls();
    //    }

    //    if (Input.GetKeyDown(KeyCode.I))
    //    {
    //        ChangeCellsToMajority();
    //    }

    //    prevSize = size;
    //    if (Input.GetKeyDown(KeyCode.J))
    //    {
    //        if (Random.value < 0.5f)
    //        {
    //            size.x++;
    //        }
    //        else
    //        {
    //            size.y++;
    //        }
    //    }

    //}

    private void GenerateWalls()
    {
        if (wallsCreated.Count != 0)
        {
            for (int i = wallsCreated.Count - 1; i >= 0; i--)
            {
                Destroy(wallsCreated[i]);
                wallsCreated.RemoveAt(i);
            }
        }
        float scale = 5;
        for (float i = 0; i < size.x; i += scale)
        {
            GameObject mainWall = Instantiate(walls[Random.Range(0, walls.Count)], roomParent);
            mainWall.transform.position = new Vector3((i - 0.5f) + (scale / 2), (size.y - 0.5f) + 0.25f * scale);
            wallsCreated.Add(mainWall);
            GameObject topOfWall1 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)], roomParent);
            topOfWall1.transform.position = new Vector3((i - 0.5f) + 0.25f * scale, size.y + 0.125f * scale + 2, -0.01f);
            wallsCreated.Add(topOfWall1);
            GameObject topOfWall2 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)], roomParent);
            topOfWall2.transform.position = new Vector3((i - 0.5f) + 0.25f * scale + scale / 2, size.y + 0.125f * scale + 2, -0.01f);
            wallsCreated.Add(topOfWall2);
        }
        for (float i = 0; i < size.y + scale / 2; i += scale / 2)
        {
            GameObject sideWall = Instantiate(sideWalls[Random.Range(0, sideWalls.Count)], roomParent);
            sideWall.transform.position = new Vector3(-1, i + 0.75f, -0.01f);
            wallsCreated.Add(sideWall);
            GameObject sideWall2 = Instantiate(sideWalls[Random.Range(0, sideWalls.Count)], roomParent);
            sideWall2.transform.position = new Vector3(size.x, i + 0.75f, -0.01f);
            wallsCreated.Add(sideWall2);
        }
        for (float i = 0; i < size.x; i += scale)
        {
            GameObject topOfWall1 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)], roomParent);
            topOfWall1.transform.position = new Vector3((i - 0.5f) + 0.25f * scale, -1f, -0.01f);
            wallsCreated.Add(topOfWall1);
            GameObject topOfWall2 = Instantiate(topOfWalls[Random.Range(0, topOfWalls.Count)], roomParent);
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
        for (int i = 0; i < allCellsAvailability.Length; i++)
        {
            if (allCellsAvailability[i].Count == currentLowest && allCellsAvailability[i].Count > 1)
            {
                cellsWithLowest.Add(i);
            }
            if (allCellsAvailability[i].Count < currentLowest && allCellsAvailability[i].Count > 1)
            {
                cellsWithLowest.Clear();
                cellsWithLowest.Add(i);
                currentLowest = allCellsAvailability[i].Count;
                lowestIndex = i;
            }
        }
        if (lowestIndex == 0)
        {
            lowestIndex = cellsWithLowest[Random.Range(0, cellsWithLowest.Count)];
        }

        //Collapse Cell to a single state
        int newCellID = cells[allCellsAvailability[lowestIndex][Random.Range(0, allCellsAvailability[lowestIndex].Count)]].cellID;
        List<int> oneCell = new List<int>();
        oneCell.Add(newCellID);
        allCellsAvailability[lowestIndex] = oneCell;

        //Find neighbors
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
        for (int i = 0; i < allCellsAvailability[cellToBeUpdated].Count; i++)
        {
            bool sharesID = true;
            for (int j = 0; j < allCellsAvailability[neighboringCell].Count; j++)
            {
                bool hasSimilar = false;
                for (int k = 0; k < cells[allCellsAvailability[cellToBeUpdated][i]].validCellNeighbors.Count; k++)
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
            if (sharesID)
            {
                cellsAvaliable.Add(allCellsAvailability[cellToBeUpdated][i]);
            }
        }


        if (cellsAvaliable.Count != allCellsAvailability[cellToBeUpdated].Count)
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
        for (int i = 0; i < neighboringCells.Count; i++)
        {
            int neighborCount = allCellsAvailability[neighboringCells[i]].Count;
            if (allCellsAvailability[neighboringCells[i]].Count > 1)
            {
                List<int> possibleCells = new List<int>();
                for (int j = 0; j < allCellsAvailability[targetCell].Count; j++)
                {
                    for (int k = 0; k < cells[allCellsAvailability[targetCell][j]].validCellNeighbors.Count; k++)
                    {
                        possibleCells.Add(cells[allCellsAvailability[targetCell][j]].validCellNeighbors[k]);
                    }
                }
                for (int j = 0; j < allCellsAvailability[neighboringCells[i]].Count; j++)
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
        for (int i = 0; i < allCellsAvailability.Length; i++)
        {
            int amountTotal = 0;
            int[] amountAround = new int[cells.Count];
            if (i - size.x >= 0 && i % size.x != 0)
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
            if (i % size.x < (i + 1) % size.x)
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
            for (int j = 0; j < amountAround.Length; j++)
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
        for (int i = 0; i < allCellsAvailability.Length; i++)
        {
            cellHolders[i].GetComponent<CellHolder>().SpawnCell(new Vector2(i % size.x, Mathf.FloorToInt(i / size.x)), cells[allCellsAvailability[i][0]]);
        }
    }

}
