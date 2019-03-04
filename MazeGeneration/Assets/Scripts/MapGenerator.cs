using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    public GameObject mazeGeneratorPrefab;
    public int mazeCount;
    public int mazeRows;
    public int mazeCols;

    void Start()
    {
        InitializeMazes();
    }

    void InitializeMazes()
    {
        for (int i = 0; i < mazeCount; i++)
        {
            Vector3 mazeSpawnPoint = new Vector3(transform.position.x + i * (mazeCols + 1), 0, 0);
            GameObject tempMaze = Instantiate(mazeGeneratorPrefab, mazeSpawnPoint, Quaternion.identity);
            tempMaze.name = "Maze " + i.ToString();
            tempMaze.transform.parent = transform;
            MazeGenerator mazeScript = tempMaze.GetComponent<MazeGenerator>();
            mazeScript.SetDimensions(mazeRows, mazeCols);
            mazeScript.InitializeMaze();
            mazeScript.GenerateRandomMaze();

        }
    }

    private int[] FindFirstDeadEnd(MazeGenerator maze, int rowFlag, int colFlag)
    {
        for (int i = 0; i < mazeRows; i++)
        {
            for (int j = 0; j < mazeCols; j++)
            {
                if (i == rowFlag && j == colFlag) // skip previous entrance
                    continue;
                if (maze.mazeIntArray[i, j] == 1 || maze.mazeIntArray[i, j] == 2 || maze.mazeIntArray[i, j] == 4 || maze.mazeIntArray[i, j] == 8)
                    return new int[] { i, j };
            }
        }
        return new int[] { 0, 0 };
    }

    private int[] FindFirstDeadEnd(MazeGenerator maze)
    {
        return FindFirstDeadEnd(maze, -1, -1);
    }
}
