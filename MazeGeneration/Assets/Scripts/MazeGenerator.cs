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

    // Start is called before the first frame update
    void Start()
    {
        InitializeMaze();
        GenerateMaze();
    }

    // Update is called once per frame
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

    void InitializeMaze()
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
    }

    void GenerateMaze()
    {
        int startRow = Random.Range(0, mazeRows);
        int startCol = Random.Range(0, mazeColumns);
        RecursiveDFS(startRow, startCol);
    }


    //Probably should be a coroutine as the generation hangs the game on play pretty bad.
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
}
