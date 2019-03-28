using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public GameObject[] mazeGeneratorPrefab;
    public Transform playerHead;
    public int mazeCount;
    public int mazeRows;
    public int mazeCols;
    public float tileWidth = 1f;
    public float wallWidth = 0f;
    public int startRow;
    public int startCol;

    public Vector3 playAreaSize;

    public TileInfo[] portalInfo;
    public MapInfo[] mapSequence;

    void Start()
    {
        //playAreaSize = GetCameraRigSize();
        portalInfo = new TileInfo[mazeCount - 1];

        GetStartSeedFromPlayerPosition(out startCol, out startRow);

        if (startRow < 0 || startRow >= mazeRows || startCol < 0 || startCol >= mazeCols)
        {
            startRow = 0;
            startCol = 0;
            Debug.Log("Player was out of game area, Maze starts from (0;0).");
        }

        //InitializeMazes();
        GenerateMapSequence();
        OffsetMap();
        /*
        //This is to debug the portal infos in the console
        for (int i = 0; i < portalInfo.Length; i++)
        {
            Debug.Log("pp for maze: " + i + " r: " + portalInfo[i].row + " c: " + portalInfo[i].column + " d: " + portalInfo[i].direction);
        }
        */
        //maybe add script to find player head so we don't have to drag it in
    }

    void GenerateMapSequence()
    {
        if (mapSequence.Length > 0)
        {
            mapSequence[0].startSeed.row = startRow;
            mapSequence[0].startSeed.column = startCol;
            mapSequence[0].startSeed.direction = GenerateRandomStartDirection(startRow, startCol);
        }

        for (int i = 0; i < mapSequence.Length; i++)
        {
            Vector3 mapSpawnPoint = new Vector3(transform.position.x + i * (mazeCols * tileWidth + 1), 0, 0);
            GameObject tempMap = Instantiate(mazeGeneratorPrefab[(int)mapSequence[i].mapType], mapSpawnPoint, Quaternion.identity);
            tempMap.name = i.ToString() + " - " + mapSequence[i].mapType.ToString();
            tempMap.transform.parent = transform;

            MapGenerator mapScript = tempMap.GetComponent<MapGenerator>();
            mapScript.SetDimensions(mazeRows, mazeCols, tileWidth, wallWidth);
            mapScript.Initialize();

            //calculate start seed
            if (i > 0)
            {
                mapSequence[i].startSeed = new TileInfo(mapSequence[i - 1].endSeed);
                Debug.Log(mapSequence[i-1].endSeed.direction + " " + mapSequence[i].startSeed.direction);
                mapSequence[i].startSeed.direction = PortalPositionHelper.GetRandomPortalExit(mapSequence[i].startSeed.row, mapSequence[i].startSeed.column, mapSequence[i - 1].endSeed.direction);
                Debug.Log(mapSequence[i-1].endSeed.direction + " " + mapSequence[i].startSeed.direction);
            }
            if ((int)mapSequence[i].mapType != 1)
            {
                if (i + 1 < mapSequence.Length && (int)mapSequence[i + 1].mapType == 1) //Change this so we can use the enum
                {
                    mapSequence[i].isEndSeeded = true;
                    do
                    {
                        mapSequence[i].endSeed = GenerateRandomConrner();
                    }
                    while (mapSequence[i].endSeed == mapSequence[i].startSeed); //this will introduce errors if they are next to each other, need to fix
                }

            }
            mapScript.Generate(mapSequence[i]);
            mapSequence[i].endSeed = mapScript.GetRandomDeadEnd(mapSequence[i].startSeed);
        }
    }

    void InitializeMazes()
    {
        int[] nextEntrancePosition = new int[] { -1, -1 };
        int currentEntranceDirection = 0;
        int nextEntranceDirection = 0;

        for (int i = 0; i < mazeCount; i++)
        {
            Vector3 mazeSpawnPoint = new Vector3(transform.position.x + i * (mazeCols * tileWidth + 1), 0, 0);
            GameObject tempMaze = Instantiate(mazeGeneratorPrefab[0], mazeSpawnPoint, Quaternion.identity);
            tempMaze.name = "Maze " + i.ToString();
            tempMaze.transform.parent = transform;
            MazeGenerator mazeScript = tempMaze.GetComponent<MazeGenerator>();
            mazeScript.SetDimensions(mazeRows, mazeCols, tileWidth, wallWidth);
            mazeScript.Initialize();

            if (i == 0)
            {
                int startDirection = GenerateRandomStartDirection(startRow, startCol);

                mazeScript.Generate(startRow, startCol, startDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(startRow, startCol);
                //Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);

            }
            else
            {

                mazeScript.Generate(nextEntrancePosition[0], nextEntrancePosition[1], nextEntranceDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(nextEntrancePosition[0], nextEntrancePosition[1]);
                //Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);
            }
            currentEntranceDirection = (int)Mathf.Log(mazeScript.mazeIntArray[nextEntrancePosition[0], nextEntrancePosition[1]], 2);
            if (i < portalInfo.Length)
                portalInfo[i] = new TileInfo(nextEntrancePosition[0], nextEntrancePosition[1], currentEntranceDirection);
            //Debug.Log("current entrancedirection: " + currentEntranceDirection);
            nextEntranceDirection = PortalPositionHelper.GetRandomPortalExit(nextEntrancePosition[0], nextEntrancePosition[1], currentEntranceDirection);
            //Debug.Log("next entrancedirection: " + nextEntranceDirection);
        }
    }

    int GenerateRandomStartDirection(int row, int col)
    {
        return PortalPositionHelper.GetRandomPortalExit(row, col);
    }

    TileInfo GenerateRandomConrner()
    {
        int row = Mathf.RoundToInt(Random.value) * (mazeRows - 1);
        int col = Mathf.RoundToInt(Random.value) * (mazeCols - 1);
        int dir = GenerateRandomStartDirection(row, col);
        return new TileInfo(row, col, dir);
    }

    public Vector3 GetCameraRigSize()
    {
        var chaperone = Valve.VR.OpenVR.Chaperone;
        float x = 0, z = 0;
        if (chaperone != null)
        {
            chaperone.GetPlayAreaSize(ref x, ref z);
            Debug.Log("got here");
        }
        return new Vector3(x, 0, z);
    }

    void OffsetMap()
    {
        transform.Translate(-playAreaSize.x / 2f + tileWidth / 2f, 0, playAreaSize.z / 2f - tileWidth / 2f);
    }

    void GetStartSeedFromPlayerPosition(out int col, out int row)
    {
        col = Mathf.RoundToInt(Mathf.Abs((playerHead.position.x - (-playAreaSize.x / 2f + tileWidth / 2f)) / tileWidth));
        row = Mathf.RoundToInt(Mathf.Abs((playerHead.position.z - (playAreaSize.z / 2f - tileWidth / 2f)) / tileWidth));

        return;
    }
}
