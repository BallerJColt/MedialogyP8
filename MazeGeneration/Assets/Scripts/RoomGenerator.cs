using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{
    public int mazeRows;
    public int mazeColumns;
    public float tileWidth;
    public float wallWidth;
    public GameObject tilePrefab;
    public Tile[,] tileArray;
    bool matCheck = false;
    public int[,] mazeIntArray;
    // Start is called before the first frame update
    void Start()
    {
        InitializeRoom();
        GenerateRoom(0, 0, 1);
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

    public void InitializeRoom()
    {
        tileArray = new Tile[mazeRows, mazeColumns];
        //float mazeHalfWidth = mazeRows / 2f; // Add scalability with tile width!
        //float mazeHalfHeight = mazeColumns / 2f; // Add scalability with tile height!
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeColumns; j++)
            {
                Vector3 tileSpawnPosition = new Vector3(transform.position.x + j * tileWidth, 0, transform.position.z - i * tileWidth); //if we want to center it, we need to subtract mazeHalfwidth from x and add mazehalfheight to z.
                GameObject emptyTile = Instantiate(tilePrefab, tileSpawnPosition, Quaternion.identity);
                emptyTile.name = "Tile " + (mazeColumns * i + j).ToString();
                emptyTile.transform.parent = transform;
                tileArray[i, j] = emptyTile.GetComponent<Tile>();
                tileArray[i, j].SetWidth(tileWidth);
            }
        }
        //Debug.Log(name + " initialized.");
    }

    void GenerateRoom(int startRow, int startCol, int startDirection)
    {
        int sR = 0;
        int sC = 0;
        int eR = mazeRows;
        int eC = mazeColumns;
        int doorDirection = 0;
        switch (startDirection)
        {
            case 0:
                for (int i = mazeRows - 1; i > 0; i--)
                {
                    Tile.ConnectTiles(tileArray[i, startCol], tileArray[i - 1, startCol], startDirection);
                }
                if (startCol == 0)
                    doorDirection = 1;
                else
                    doorDirection = 3;
                tileArray[mazeRows / 2, startCol].OpenWall(doorDirection);
                eC--;
                break;
            case 1:
                for (int i = 0; i < mazeColumns - 1; i++)
                {
                    Tile.ConnectTiles(tileArray[startRow, i], tileArray[startRow, i + 1], startDirection);
                }
                if (startRow == 0)
                    doorDirection = 2;
                else
                    doorDirection = 0;
                tileArray[startRow, mazeColumns / 2].OpenWall(doorDirection);
                sR++;
                break;
            case 2:
                for (int i = 0; i < mazeRows - 1; i++)
                {
                    Tile.ConnectTiles(tileArray[i, startCol], tileArray[i + 1, startCol], startDirection);
                }
                if (startCol == 0)
                    doorDirection = 1;
                else
                    doorDirection = 3;
                tileArray[mazeRows / 2, startCol].OpenWall(doorDirection);
                sC++;
                break;
            case 3:
                for (int i = mazeColumns - 1; i > 0; i--)
                {
                    Tile.ConnectTiles(tileArray[startRow, i], tileArray[startRow, i - 1], startDirection);
                }
                if (startRow == 0)
                    doorDirection = 2;
                else
                    doorDirection = 0;
                tileArray[startRow, mazeColumns / 2].OpenWall(doorDirection);
                eR--;
                break;
            default:
                break;
        }
        GenerateEmptyRoom(sR, sC, eR, eC);
    }

    void GenerateEmptyRoom(int startRow, int startCol, int endRow, int endCol)
    {
        Debug.Log("starting from row " + startRow + " and col " + startCol);
        for (int i = startRow; i < endRow; i++)
        {
            for (int j = startCol; j < endCol; j++)
            {
                tileArray[i, j].SetTileID(15);
                if (i == 0)
                    tileArray[i, j].CloseWall(0);
                if (i == mazeRows - 1)
                    tileArray[i, j].CloseWall(2);
                if (j == 0)
                    tileArray[i, j].CloseWall(3);
                if (j == mazeColumns - 1)
                    tileArray[i, j].CloseWall(1);
            }
        }
    }
}
