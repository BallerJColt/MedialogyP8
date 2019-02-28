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
    
    void Update()
    {

    }

    void InitializeMazes()
    {
        for (int i = 0; i < mazeCount; i++)
        {
            Vector3 mazeSpawnPoint = new Vector3(transform.position.x + i * (mazeRows + 1), 0, 0);
            GameObject tempMaze = Instantiate(mazeGeneratorPrefab, mazeSpawnPoint, Quaternion.identity);
            tempMaze.name = "Maze " + i.ToString();
            tempMaze.transform.parent = transform;
            MazeGenerator mazeScript = tempMaze.GetComponent<MazeGenerator>();
            mazeScript.SetDimensions(mazeRows, mazeCols);
        }
    }
}
