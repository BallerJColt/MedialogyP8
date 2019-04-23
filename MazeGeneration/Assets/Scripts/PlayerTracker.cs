using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    private int mazeCount;
    private float mazeOffset;
    Vector3 startPos;
    float tileWidth;

    Vector3 currentPos;
    bool isLoggerRunning;
    public bool logPosition;
    public float timeBetweenPings;
    public int currentMaze;
    public int currentRow;
    public int currentColumn;

    // Start is called before the first frame update
    void Start()
    {
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        tileWidth = mapManager.tileWidth;
        startPos = new Vector3(mapManager.transform.position.x - tileWidth / 2f, 0, mapManager.transform.position.z + tileWidth / 2f);
        mazeCount = mapManager.mapSequence.Length;
        mazeOffset = mapManager.mazeCols * tileWidth + 1f;

        if(!isLoggerRunning && logPosition)
            StartCoroutine("PositionToConsole");
    }
    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector3(transform.position.x - startPos.x, 0, transform.position.z - startPos.z);
        currentRow = (int)(-currentPos.z / tileWidth);
        currentMaze = (int)(currentPos.x / mazeOffset);
        currentColumn = (int)((currentPos.x - currentMaze * mazeOffset) / tileWidth);
    }

    private IEnumerator PositionToConsole()
    {
        isLoggerRunning = true;
        while (true)
        {
            Debug.Log("Time: " + Time.time + ", Maze: " + currentMaze + ", Row: " + currentRow + ", Column: " + currentColumn + ".");
            yield return new WaitForSeconds(timeBetweenPings);
        }
    }
}
