using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCoordinateLogger : MonoBehaviour
{
    public int mazeCount;
    public float mazeOffset;
    public int currentMaze;
    public int currentRow;
    public int currentColumn;

    //make private
    public Vector3 startPos;
    public float tileWidth;

    public Vector3 currentPos;

    // Start is called before the first frame update
    void Start()
    {
        MapManager mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        tileWidth = mapManager.tileWidth;
        startPos = new Vector3(mapManager.transform.position.x - tileWidth / 2f, 0, mapManager.transform.position.z + tileWidth / 2f);
        mazeCount = mapManager.mapSequence.Length;
        mazeOffset = mapManager.mazeCols * tileWidth + 1f;
    }

    // Update is called once per frame
    void Update()
    {
        currentPos = new Vector3(transform.position.x - startPos.x, 0, transform.position.z - startPos.z);
        currentRow = (int)(-currentPos.z / tileWidth);
        currentMaze = (int)(currentPos.x / mazeOffset);
        currentColumn = (int)((currentPos.x - currentMaze * mazeOffset) / tileWidth);
    }
}
