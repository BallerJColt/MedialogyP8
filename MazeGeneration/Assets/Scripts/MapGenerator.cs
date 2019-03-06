﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mazeGeneratorPrefab;
    public int mazeCount;
    public int mazeRows;
    public int mazeCols;
    public int startRow;
    public int startCol;

    void Start()
    {
        InitializeMazes();
    }

    void InitializeMazes()
    {
        int[] nextEntrancePosition = new int[] { -1, -1 };
        int currentEntranceDirection = 0;
        int nextEntranceDirection = 0;

        for (int i = 0; i < mazeCount; i++)
        {
            Vector3 mazeSpawnPoint = new Vector3(transform.position.x + i * (mazeCols + 1), 0, 0);
            GameObject tempMaze = Instantiate(mazeGeneratorPrefab, mazeSpawnPoint, Quaternion.identity);
            tempMaze.name = "Maze " + i.ToString();
            tempMaze.transform.parent = transform;
            MazeGenerator mazeScript = tempMaze.GetComponent<MazeGenerator>();
            mazeScript.SetDimensions(mazeRows, mazeCols);
            mazeScript.InitializeMaze();

            if (i == 0)
            {
                int[] possibleStartDirections = PortalPositionHelper.GetEntranceArray(startRow, startCol);
                int idx = Random.Range(0, possibleStartDirections.Length);
                int startDirection = possibleStartDirections[idx];

                mazeScript.GenerateSeededMaze(startCol, startRow, startDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(startRow, startCol);
                Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);

            }
            else
            {

                mazeScript.GenerateSeededMaze(nextEntrancePosition[0], nextEntrancePosition[1], nextEntranceDirection);
                nextEntrancePosition = mazeScript.GetRandomDeadEnd(nextEntrancePosition[0], nextEntrancePosition[1]);
                Debug.Log(nextEntrancePosition[0] + " " + nextEntrancePosition[1]);
            }
            currentEntranceDirection = (int)Mathf.Log(mazeScript.mazeIntArray[nextEntrancePosition[0], nextEntrancePosition[1]], 2);
            Debug.Log("current entrancedirection: " + currentEntranceDirection);
            nextEntranceDirection = PortalPositionHelper.GetRandomPortalExit(nextEntrancePosition[0], nextEntrancePosition[1], currentEntranceDirection);
            Debug.Log("next entrancedirection: " + nextEntranceDirection);
        }
    }
}