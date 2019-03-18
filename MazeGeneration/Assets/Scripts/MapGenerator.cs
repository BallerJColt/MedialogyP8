using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mazeGeneratorPrefab;
    public int mazeCount;
    public int mazeRows;
    public int mazeCols;
    public float tileWidth = 1f;
    public float wallWidth = 0f;
    public int startRow;
    public int startCol;

    public PortalInfo[] portalInfo;

    void Start()
    {
        portalInfo = new PortalInfo[mazeCount - 1];
        InitializeMazes();
        for (int i = 0; i < portalInfo.Length; i++)
        {
            Debug.Log("pp for maze: " + i + " r: " + portalInfo[i].row + " c: " + portalInfo[i].column + " d: " + portalInfo[i].entranceDirection);
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
            GameObject tempMaze = Instantiate(mazeGeneratorPrefab, mazeSpawnPoint, Quaternion.identity);
            tempMaze.name = "Maze " + i.ToString();
            tempMaze.transform.parent = transform;
            MazeGenerator mazeScript = tempMaze.GetComponent<MazeGenerator>();
            mazeScript.SetDimensions(mazeRows, mazeCols, tileWidth, wallWidth);
            mazeScript.InitializeMaze();

            if (i == 0)
            {
                int[] possibleStartDirections = PortalPositionHelper.GetEntranceArray(startRow, startCol);
                int idx = Random.Range(0, possibleStartDirections.Length);
                int startDirection = possibleStartDirections[idx];

                mazeScript.GenerateSeededMaze(startCol, startRow, startDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(startRow, startCol);
                //Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);

            }
            else
            {

                mazeScript.GenerateSeededMaze(nextEntrancePosition[0], nextEntrancePosition[1], nextEntranceDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(nextEntrancePosition[0], nextEntrancePosition[1]);
                //Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);
            }
            currentEntranceDirection = (int)Mathf.Log(mazeScript.mazeIntArray[nextEntrancePosition[0], nextEntrancePosition[1]], 2);
            if (i < portalInfo.Length)
                portalInfo[i] = new PortalInfo(nextEntrancePosition[0], nextEntrancePosition[1], currentEntranceDirection);
            //Debug.Log("current entrancedirection: " + currentEntranceDirection);
            nextEntranceDirection = PortalPositionHelper.GetRandomPortalExit(nextEntrancePosition[0], nextEntrancePosition[1], currentEntranceDirection);
            //Debug.Log("next entrancedirection: " + nextEntranceDirection);
        }
    }
}
