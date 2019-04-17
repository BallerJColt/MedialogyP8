using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomGenerator : MapGenerator
{

    public override void Generate()
    {
        Generate(0, 0, 1);
    }
    public override void Generate(MapInfo info)
    {
        Generate(info.startSeed);
    }
    public override void Generate(TileInfo startSeed)
    {
        Generate(startSeed.row, startSeed.column, startSeed.direction);
    }
    public override void Generate(int startRow, int startCol, int startDir, int endRow, int endCol, int endDir)
    {
        Generate(startRow, startCol, startDir);
    }
    public override void Generate(int startRow, int startCol, int startDirection)
    {
        int sR = 0;
        int sC = 0;
        int eR = mazeRows;
        int eC = mazeColumns;
        int doorDirection = 0;

        switch (startDirection)
        {
            case 0:
                for (int i = mazeRows - 2; i > 1; i--)
                {
                    Tile.ConnectTiles(tileArray[i, startCol], tileArray[i - 1, startCol], startDirection);
                }
                if (startCol == 0)
                {
                    doorDirection = 1;
                    sC++;
                }
                else
                {
                    doorDirection = 3;
                    eC--;
                }
                tileArray[0, startCol].SetTileID(0);
                tileArray[mazeRows - 1, startCol].SetTileID(0);
                tileArray[mazeRows / 2, startCol].OpenWall(doorDirection);
                break;
            case 1:
                for (int i = 1; i < mazeColumns - 2; i++)
                {
                    Tile.ConnectTiles(tileArray[startRow, i], tileArray[startRow, i + 1], startDirection);
                }
                if (startRow == 0)
                {
                    doorDirection = 2;
                    sR++;
                }
                else
                {
                    doorDirection = 0;
                    eR--;
                }
                tileArray[startRow, 0].SetTileID(0);
                tileArray[startRow, mazeColumns - 1].SetTileID(0);
                tileArray[startRow, mazeColumns / 2].OpenWall(doorDirection);
                break;
            case 2:
                for (int i = 1; i < mazeRows - 2; i++)
                {
                    Tile.ConnectTiles(tileArray[i, startCol], tileArray[i + 1, startCol], startDirection);
                }
                if (startCol == 0)
                {
                    doorDirection = 1;
                    sC++;
                }
                else
                {
                    doorDirection = 3;
                    eC--;
                }
                tileArray[0, startCol].SetTileID(0);
                tileArray[mazeRows - 1, startCol].SetTileID(0);
                tileArray[mazeRows / 2, startCol].OpenWall(doorDirection);
                break;
            case 3:
                for (int i = mazeColumns - 2; i > 1; i--)
                {
                    Tile.ConnectTiles(tileArray[startRow, i], tileArray[startRow, i - 1], startDirection);
                }
                if (startRow == 0)
                {
                    doorDirection = 2;
                    sR++;
                }
                else
                {
                    doorDirection = 0;
                    eR--;
                }
                tileArray[startRow, 0].SetTileID(0);
                tileArray[startRow, mazeColumns - 1].SetTileID(0);
                tileArray[startRow, mazeColumns / 2].OpenWall(doorDirection);
                break;
            default:
                break;
        }
        GenerateEmptyRoom(sR, sC, eR, eC);
        GenerateIntArray();
    }

    void GenerateEmptyRoom(int startRow, int startCol, int endRow, int endCol)
    {
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
