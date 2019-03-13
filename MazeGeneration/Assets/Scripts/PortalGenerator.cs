using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{

    private int wallWidth;
    private int mazeWidth;

    public GameObject portalEntPrefab;  // For testing purposes, need to change into one prefab
    public GameObject portalExtPrefab;
    private GameObject[,] portalPairPrefabs;
    private GameObject portalPairHolder;

    void Update()
    {

    }

    public void GeneratePortals(int[] entRows, int[] entCols, int[] entDirs, int tileWidth)     // Generate the portal pairs at the specified locations around the mazes. Takes arrays of entrance rows and cols + directions
    {
        MapGenerator mapGenScript = GameObject.Find("MapGenerator").GetComponent<MapGenerator>();   
        mazeWidth = mapGenScript.mazeCols;
        float tileWidthFloat = tileWidth;       // Convert int to float

        InstantializePortalArray(mapGenScript.mazeCount);   // Instantializes the amount of portal pairs to be used


        for (int i = 0; i < mapGenScript.mazeCount - 1; i++) // Must be mazes -1, as one pair is reserved for room exit and entrance. 
        {

            portalPairHolder = new GameObject();
            portalPairHolder.name = "Portal Pair " + i.ToString();
            portalPairHolder.transform.parent = transform;   // Makes the GameObject a child of the parent


            // Entrance portal:
            GameObject mazeObject = GameObject.Find("MapGenerator/Maze " + i);  // Find specific maze

            Vector3 portalEntSpawnPosition = new Vector3((mazeObject.transform.position.x + tileWidth / 2) + entRows[i], 0, (mazeObject.transform.position.z + tileWidth / 2) - entCols[i]);    // Find tile position vector, based on input coordinates

            GameObject emptyEntrancePortal = Instantiate(portalPairPrefabs[i, 0], portalEntSpawnPosition, Quaternion.Euler(0, 90 * entDirs[i], 0));     // Instantiate entrance portal, place it at tile coordinate and rotate according to specified direction

            emptyEntrancePortal.name = "Portal Entrance";
            emptyEntrancePortal.transform.parent = portalPairHolder.transform;   // Makes the GameObject a child of the pair holder object
            emptyEntrancePortal.transform.position = emptyEntrancePortal.transform.position + emptyEntrancePortal.transform.forward * (tileWidthFloat /2.0f);   // Uses the forward vector of the GameObject to move it to the tile edge


            // Exit portal:
            GameObject nextMazeObject = GameObject.Find("MapGenerator/Maze " + (i + 1));    // Find specific maze + 1

            Vector3 portalExtSpawnPosition = new Vector3((nextMazeObject.transform.position.x + tileWidth / 2) + entRows[i], 0, (nextMazeObject.transform.position.z + tileWidth / 2) - entCols[i]);
            GameObject emptyExitPortal = Instantiate(portalPairPrefabs[i, 1], portalExtSpawnPosition, Quaternion.Euler(0, ((90 * entDirs[i]) + 180) % 360, 0));     // Instantiate exit portal, place it at same tile coordinate, but of the next maze. Then rotate opposite direction of entrance portal

            emptyExitPortal.name = "Portal Exit";
            emptyExitPortal.transform.parent = portalPairHolder.transform;    // Makes the GameObject a child of the pair holder object
            emptyExitPortal.transform.position = emptyExitPortal.transform.position - emptyExitPortal.transform.forward * (tileWidthFloat / 2.0f);  // Uses the forward vector of the GameObject to move it to the tile edge

        }

        // Debug.Log(PortalPositionHelper.GetEntranceArray("topLeftCorner"));
        // Debug.Log(PortalPositionHelper.GetRandomPortalExit(0, 0, 1));

    }

    private void InstantializePortalArray(int mazeAmount)   // Instantializes the amount of portal pairs to be used
    {
        portalPairPrefabs = new GameObject[mazeAmount, 2];

        for (int i = 0; i < mazeAmount - 1; i++)
        {
            portalPairPrefabs[i, 0] = portalEntPrefab;
            portalPairPrefabs[i, 1] = portalExtPrefab;
        }
    }
}
