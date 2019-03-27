using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGenerator : MonoBehaviour
{

    private int wallWidth;
    private int mazeWidth;
    public GameObject portalPairPrefab;
    private GameObject[] portalPairArr;

    public int[] testingRows;
    public int[] testingCols;
    public int[] testingDirs;
    public int testingWides;

    void Update()
    {
        
    }
public void GeneratePortals(int[] entRows, int[] entCols, int[] entDirs, float tileWidth, float wallWidth)     // Generate the portal pairs at the specified locations around the mazes. Takes arrays of entrance rows and cols + directions
    {
        MapManager mapGenScript = GameObject.Find("MapManager").GetComponent<MapManager>();   
        mazeWidth = mapGenScript.mazeCols*(int)tileWidth;
        //float tileWidthFloat = tileWidth;       // Convert int to float

        portalPairArr = new GameObject[mapGenScript.mazeCount-1]; // set lenght of portalPairArr to the amount of Pairs needed

        for (int i = 0; i < mapGenScript.mazeCount-1; i++) // Must be mazes -1, as one pair is reserved for room exit and entrance. 
        {

            //entrance portal tranform
            GameObject mazeObject = GameObject.Find("MapManager/Maze " + i);  // Find specific maze            
            GameObject nextMazeObject = GameObject.Find("MapManager/Maze " + (i + 1));    // Find specific maze + 1
            Transform transformHelper = transform; // for calculating the proper portal position as it depends on an object's forward vector. set to transform temporally 

            Quaternion entrancePortalRotation = Quaternion.Euler(90, 90 * entDirs[i], 0);
            transformHelper.rotation = entrancePortalRotation; //give transformHelper the correct rotation
            Vector3 entrancePortalPosition = new Vector3((mazeObject.transform.position.x + (int)tileWidth / 2) + entRows[i], 0.5f, (mazeObject.transform.position.z + (int)tileWidth / 2) - entCols[i])+ transformHelper.up * ((tileWidth /2.0f)-wallWidth);
            transformHelper.position = entrancePortalPosition; //give transformHelper the correct position
            Vector3 mazeOffset = nextMazeObject.transform.position - mazeObject.transform.position; //distance between to adjadent mazes
            Debug.Log(mazeOffset);
            GameObject newPair = Instantiate(portalPairPrefab,new Vector3(0,0,0),Quaternion.Euler(0, 0, 0),transform); //instantiate new portalPair and set the parent to be the portal generator
            newPair.name = "PortalPair " +i;
            PortalPair pp = newPair.GetComponent<PortalPair>();
            pp.PortalPairConstructor(transformHelper.position, transformHelper.rotation, mazeOffset); 
            portalPairArr[i] = newPair; 
        }

        // Debug.Log(PortalPositionHelper.GetEntranceArray("topLeftCorner"));
        // Debug.Log(PortalPositionHelper.GetRandomPortalExit(0, 0, 1));

    }
    /* 
    public void GeneratePortals(int[] entRows, int[] entCols, int[] entDirs, int tileWidth)     // Generate the portal pairs at the specified locations around the mazes. Takes arrays of entrance rows and cols + directions
    {
        MapManager mapGenScript = GameObject.Find("MapManager").GetComponent<MapManager>();   
        mazeWidth = mapGenScript.mazeCols;
        float tileWidthFloat = tileWidth;       // Convert int to float

        InstantializePortalArray(mapGenScript.mazeCount);   // Instantializes the amount of portal pairs to be used


        for (int i = 0; i < mapGenScript.mazeCount - 1; i++) // Must be mazes -1, as one pair is reserved for room exit and entrance. 
        {

            //portalPairHolder = new GameObject();
            //portalPairHolder.name = "Portal Pair " + i.ToString();
            //portalPairHolder.transform.parent = transform;   // Makes the GameObject a child of the parent


            // Entrance portal:
            GameObject mazeObject = GameObject.Find("MapManager/Maze " + i);  // Find specific maze

            Vector3 portalEntSpawnPosition = new Vector3((mazeObject.transform.position.x + tileWidth / 2) + entRows[i], 0, (mazeObject.transform.position.z + tileWidth / 2) - entCols[i]);    // Find tile position vector, based on input coordinates

            GameObject emptyEntrancePortal = Instantiate(portalPairPrefabs[i, 0], portalEntSpawnPosition, Quaternion.Euler(0, 90 * entDirs[i], 0));     // Instantiate entrance portal, place it at tile coordinate and rotate according to specified direction

            emptyEntrancePortal.name = "Portal Entrance";
            emptyEntrancePortal.transform.parent = portalPairHolder.transform;   // Makes the GameObject a child of the pair holder object
            emptyEntrancePortal.transform.position = emptyEntrancePortal.transform.position + emptyEntrancePortal.transform.forward * (tileWidthFloat /2.0f);   // Uses the forward vector of the GameObject to move it to the tile edge


            // Exit portal:
            GameObject nextMazeObject = GameObject.Find("MapManager/Maze " + (i + 1));    // Find specific maze + 1

            Vector3 portalExtSpawnPosition = new Vector3((nextMazeObject.transform.position.x + tileWidth / 2) + entRows[i], 0, (nextMazeObject.transform.position.z + tileWidth / 2) - entCols[i]);
            GameObject emptyExitPortal = Instantiate(portalPairPrefabs[i, 1], portalExtSpawnPosition, Quaternion.Euler(0, ((90 * entDirs[i]) + 180) % 360, 0));     // Instantiate exit portal, place it at same tile coordinate, but of the next maze. Then rotate opposite direction of entrance portal

            emptyExitPortal.name = "Portal Exit";
            emptyExitPortal.transform.parent = portalPairHolder.transform;    // Makes the GameObject a child of the pair holder object
            emptyExitPortal.transform.position = emptyExitPortal.transform.position - emptyExitPortal.transform.forward * (tileWidthFloat / 2.0f);  // Uses the forward vector of the GameObject to move it to the tile edge

        }

        // Debug.Log(PortalPositionHelper.GetEntranceArray("topLeftCorner"));
        // Debug.Log(PortalPositionHelper.GetRandomPortalExit(0, 0, 1));

    }
    */
    /*/private void InstantializePortalArray(int mazeAmount)   // Instantializes the amount of portal pairs to be used
    {
        
        portalPairArr = new GameObject[mazeAmount-1]; // set lenght of portalPairArr to the amount of Pairs needed
        
        for (int i = 0; i < portalPairArr.Length; i++)
        {
            GameObject newPair = Instantiate(portalPairPrefab,Vector3.zero,Quaternion.Euler(0, 30, 0),transform); //instantiate new portalPair and set the parent to be the portal generator

            portalPairArr[i] = newPair;   
        }
        /* portalPairPrefabs = new GameObject[mazeAmount, 2];

        for (int i = 0; i < mazeAmount - 1; i++)
        {
            portalPairPrefabs[i, 0] = portalEntPrefab;
            portalPairPrefabs[i, 1] = portalExtPrefab;
        }
    }*/
}
