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
        for (int i = 0; i < mapSequence.Length; i++)
        {
            Vector3 mapSpawnPoint = new Vector3(transform.position.x + i * (mazeCols * tileWidth + 1), 0, 0);
            GameObject tempMap = Instantiate(mazeGeneratorPrefab[(int)mapSequence[i].mapType], mapSpawnPoint, Quaternion.identity);
            tempMap.name = i.ToString() + " - " + mapSequence[i].mapType.ToString();
            tempMap.transform.parent = transform;
            MapGenerator mapScript = tempMap.GetComponent<MapGenerator>();
            mapScript.SetDimensions(mazeRows,mazeCols,tileWidth,wallWidth);
            mapScript.Initialize();
            mapScript.Generate();
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
                int[] possibleStartDirections = PortalPositionHelper.GetEntranceArray(startRow, startCol);
                int idx = Random.Range(0, possibleStartDirections.Length);
                int startDirection = possibleStartDirections[idx];

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
