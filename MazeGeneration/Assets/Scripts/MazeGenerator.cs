using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeGenerator : MonoBehaviour
{
    public int mazeRows;
    public int mazeColumns;
    public GameObject tilePrefab;
    public Tile[,] tileArray;
    bool matCheck = false;
    public int[,] mazeIntArray;

    // Staart is called before the first frame update
    /* void Awake() {
        Debug.Log(name + " awake method called");
        Debug.Log(name + " awake method ended");
    } */

    /* void Start()
    {
        Debug.Log(name + " start method called");
        InitializeMaze();
        //GenerateRandomMaze();
        GenerateSeededMaze(0,0,1); // now we seeded
        //GenerateSeededMaze(0, 0, 1, mazeRows - 1, mazeColumns - 1, 0);
        GenerateIntArray();
        //int[] alma = GetRandomDeadEnd(-1, -1);
        int[] alma = GetRandomDeadEnd(0, 0);
        Debug.Log(name + " First dead end: (" + alma[0] + ";" + alma[1] + ").");
        Debug.Log(name + " start method ended");
    } */

    // The inputs here are just for debug reasons to show the wang tiles
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            if (matCheck != true)
            {
                foreach (Tile t in tileArray)
                {
                    t.SetMaterial(t.GetTileID(), 'w');
                }
            }
            else
            {
                foreach (Tile t in tileArray)
                {
                    t.SetMaterial(t.GetTileID(), 'p');
                }
            }
            matCheck = !matCheck;
        }
    }

    public void InitializeMaze()
    {
        tileArray = new Tile[mazeRows, mazeColumns];
        //float mazeHalfWidth = mazeRows / 2f; // Add scalability with tile width!
        //float mazeHalfHeight = mazeColumns / 2f; // Add scalability with tile height!
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeColumns; j++)
            {
                Vector3 tileSpawnPosition = new Vector3(transform.position.x + j, 0, transform.position.z - i); //if we want to center it, we need to subtract mazeHalfwidth from x and add mazehalfheight to z.
                GameObject emptyTile = Instantiate(tilePrefab, tileSpawnPosition, Quaternion.identity);
                emptyTile.name = "Tile " + (mazeColumns * i + j).ToString();
                emptyTile.transform.parent = transform;
                tileArray[i, j] = emptyTile.GetComponent<Tile>();
            }
        }
        //Debug.Log(name + " initialized.");
    }

    // Random starting positions
    public void GenerateRandomMaze()
    {
        int startRow = Random.Range(0, mazeRows);
        int startCol = Random.Range(0, mazeColumns);
        RecursiveDFS(startRow, startCol);

        //Debug.Log(name + " generated random maze");
        GenerateIntArray();
    }

    //Generates an integer array with the tileIDs in it. Will be used to find dead ends for portal placement
    void GenerateIntArray()
    {
        mazeIntArray = new int[mazeRows, mazeColumns];
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeColumns; j++)
            {
                mazeIntArray[i, j] = tileArray[i, j].GetTileID();
            }
        }

        //Debug.Log(name + " generated int array");
    }

    // Maze generation using recursive DFS with a starting position and direction as seed.
    // It makes the first connection manually, then calls the RecursiveDFS() method to generate the rest of the maze.
    public void GenerateSeededMaze(int startRow, int startCol, int startDirection)
    {
        switch (startDirection)
        {
            case 0:
                Tile.ConnectTiles(tileArray[startRow, startCol], tileArray[startRow - 1, startCol], startDirection);
                RecursiveDFS(startRow - 1, startCol);
                break;
            case 1:
                Tile.ConnectTiles(tileArray[startRow, startCol], tileArray[startRow, startCol + 1], startDirection);
                RecursiveDFS(startRow, startCol + 1);
                break;
            case 2:
                Tile.ConnectTiles(tileArray[startRow, startCol], tileArray[startRow + 1, startCol], startDirection);
                RecursiveDFS(startRow + 1, startCol);
                break;
            case 3:
                Tile.ConnectTiles(tileArray[startRow, startCol], tileArray[startRow, startCol - 1], startDirection);
                RecursiveDFS(startRow, startCol - 1);
                break;
            default:
                break;
        }
        //Debug.Log(name + " generated seeded maze, startPos: (" + startRow + ";" + startCol + ";" + startDirection + ").");
        GenerateIntArray();
    }

    // Overload of the seeded maze generation method with a specified end position and direction as well.
    // It opens the end tile so it will be ignored by the generator method, then calls the GenerateSeededMaze(int,int,int) method,
    // finally it connects the end tile with its neighbour to the specified direction.
    void GenerateSeededMaze(int startRow, int startCol, int startDirection, int endRow, int endCol, int endDirection)
    {
        tileArray[endRow, endCol].OpenWall(endDirection);

        GenerateSeededMaze(startRow, startCol, startDirection);

        switch (endDirection)
        {
            case 0:
                Tile.ConnectTiles(tileArray[endRow, endCol], tileArray[endRow - 1, endCol], endDirection);
                break;
            case 1:
                Tile.ConnectTiles(tileArray[endRow, endCol], tileArray[endRow, endCol + 1], endDirection);
                break;
            case 2:
                Tile.ConnectTiles(tileArray[endRow, endCol], tileArray[endRow + 1, endCol], endDirection);
                break;
            case 3:
                Tile.ConnectTiles(tileArray[endRow, endCol], tileArray[endRow, endCol - 1], endDirection);
                break;
            default:
                break;
        }
        //Debug.Log(name + "endPos: (" + endRow + ";" + endCol + ").");
        GenerateIntArray();
    }


    //Probably should be a coroutine as the generation hangs the game on play pretty bad.
    /*
    Generates a perfect maze using recursive depth first search. First it gets an array of random directions, then
    using a switch, it goes through them. In the switch, the method does a bounds check on the neighbour in the specified direction,
    then checks if it has been visited yet. If not, then it connects the tile to the neighbour, then calls itself (RecursiveDFS(int,int))
    on the neighbour tile. The method returns when there are no more empty neighbours.
     */
    void RecursiveDFS(int row, int col)
    {
        int[] directions = GenerateRandomDirections();
        for (int i = 0; i < 4; i++)
        {
            switch (directions[i])
            {
                case 0:
                    if (row - 1 < 0)
                        continue;
                    if (tileArray[row - 1, col].GetTileID() == 0)
                    {
                        Tile.ConnectTiles(tileArray[row, col], tileArray[row - 1, col], 0);
                        RecursiveDFS(row - 1, col);
                    }
                    break;
                case 1:
                    if (col + 1 > mazeColumns - 1)
                        continue;
                    if (tileArray[row, col + 1].GetTileID() == 0)
                    {
                        Tile.ConnectTiles(tileArray[row, col], tileArray[row, col + 1], 1);
                        RecursiveDFS(row, col + 1);
                    }
                    break;
                case 2:
                    if (row + 1 > mazeRows - 1)
                        continue;
                    if (tileArray[row + 1, col].GetTileID() == 0)
                    {
                        Tile.ConnectTiles(tileArray[row, col], tileArray[row + 1, col], 2);
                        RecursiveDFS(row + 1, col);
                    }
                    break;
                case 3:
                    if (col - 1 < 0)
                        continue;
                    if (tileArray[row, col - 1].GetTileID() == 0)
                    {
                        Tile.ConnectTiles(tileArray[row, col], tileArray[row, col - 1], 3);
                        RecursiveDFS(row, col - 1);
                    }
                    break;
                default:
                    break;
            }
        }
    }

    public void SetDimensions(int rows, int cols)
    {
        mazeRows = rows;
        mazeColumns = cols;
    }


    // This method returns returns a 4 length array with random directions occuring once in each index.
    // It first creates a base array where the directions are in order, picks one randomly,
    // puts it into the first index of the resul array and sets it into a -1 "flag" to be ignored in the base array.
    private int[] GenerateRandomDirections()
    {
        int[] baseArray = new int[4] { 0, 1, 2, 3 };
        int[] result = new int[4] { 0, 0, 0, 0 };
        for (int i = 0; i < 4;)
        {
            int temp = Random.Range(0, 4);
            if (baseArray[temp] != -1)
            {
                result[i] = baseArray[temp];
                baseArray[temp] = -1;
                i++;
            }
        }
        return result;
    }

    // This method could be used later if we want to simplify the generator methods.
    // It returns if the neighbour in the specified direction is inbounds or not.
    private bool IsNeighbourInbounds(int row, int col, int direction)
    {
        switch (direction)
        {
            case 0:
                return (row - 1 >= 0);
            case 1:
                return (col + 1 <= mazeColumns - 1);
            case 2:
                return (row + 1 <= mazeRows - 1);
            case 3:
                return (col - 1 >= 0);
            default:
                return false;
        }
    }

    public List<int[]> GetDeadEndList()
    {
        List<int[]> deadEnd = new List<int[]>();
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeColumns; j++)
            {
                if (mazeIntArray[i, j] == 1 || mazeIntArray[i, j] == 2 || mazeIntArray[i, j] == 4 || mazeIntArray[i, j] == 8)
                {
                    deadEnd.Add(new int[] { i, j });
                    //Debug.Log("" + i + " " + j);
                }
            }
        }
        return deadEnd;
    }

    public int[] GetFirstDeadEnd(int entranceRow, int entranceCol)
    {
        List<int[]> deadEndList = GetDeadEndList();
        foreach (int[] deadEnd in deadEndList)
        {
            if(deadEnd[0] == entranceRow && deadEnd[1] == entranceCol)
                continue;
            else
                return deadEnd;
        }
        return new int[] {-1,-1};
    }

    public int[] GetRandomDeadEnd(int entranceRow, int entranceCol) {
        List<int[]> deadEndList = GetDeadEndList();
        System.Random rnd = new System.Random();
        int[] deadEnd = new int[] {-1,-1};
        do{
            int idx = rnd.Next(deadEndList.Count);
            deadEnd = deadEndList[idx];
        }
        while(deadEnd[0] == entranceRow && deadEnd[1] == entranceCol);
        return deadEnd;
    }
}
