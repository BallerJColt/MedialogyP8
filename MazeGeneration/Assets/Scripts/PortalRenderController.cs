using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalRenderController : MonoBehaviour
{
    public GameObject previousPortalCameraLeftEye;
    public GameObject previousPortalCameraRightEye;
    public GameObject nextPortalCameraLeftEye;
    public GameObject nextPortalCameraRightEye;

    public GameObject portalPrefab;
    public bool isStereoscopic;
    public int mazeCount;
    public int currentMaze;
    public float cameraOffset;
    public float portalWidth;
    public float pillarOffset = 0.1f;
    private float halfmazeWidth;
    private float halfmazeHeight;

    public GameObject prevProjectionQuad;
    public GameObject nextProjectionQuad;

    public GameObject[] prevProjectionQuadArray;
    public GameObject[] nextProjectionQuadArray;
    MapManager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mazeCount = mapManager.mapSequence.Length;
        cameraOffset = (float) mapManager.mazeRows * mapManager.tileWidth + 1f;
        portalWidth = mapManager.tileWidth;
        halfmazeHeight = mapManager.mazeRows * mapManager.tileWidth / 2f;
        halfmazeWidth = mapManager.mazeCols * mapManager.tileWidth / 2f;

        prevProjectionQuadArray = new GameObject[mazeCount - 1];
        nextProjectionQuadArray = new GameObject[mazeCount - 1];

        InitializePortals();

    }

    void InitializePortals()
    {
        Debug.Log("portals will go here:");
        for (int i = 0; i < mazeCount - 1; i++)
        {
            TileInfo currentPortal = mapManager.mapSequence[i].endSeed;
            currentPortal.PrintTile();
            GameObject tempPortal = Instantiate(portalPrefab, new Vector3(i * cameraOffset - halfmazeWidth + (float) currentPortal.column * portalWidth + portalWidth / 2f, 0, halfmazeHeight - (float) currentPortal.row * portalWidth - portalWidth / 2f), Quaternion.identity);
            Teleporter tempScript = tempPortal.GetComponent<Teleporter>();
            tempPortal.transform.Rotate(0f, 180 + 90f * currentPortal.direction, 0f);
            tempPortal.transform.Translate(0, 0, portalWidth / 2f - pillarOffset, Space.Self);
            tempScript.projectionQuad.Translate(cameraOffset, 0, 0, Space.World);

            tempPortal.name = "Fowrard Teleporter " + i;
            tempPortal.transform.parent = transform;
            tempScript.portalID = i;
            tempScript.isForwardTeleporter = true;
            nextProjectionQuadArray[i] = tempScript.projectionQuad.gameObject;

            tempPortal = Instantiate(portalPrefab, new Vector3((i + 1) * cameraOffset - halfmazeWidth + (float) currentPortal.column * portalWidth + portalWidth / 2f, 0, halfmazeHeight - (float) currentPortal.row * portalWidth - portalWidth / 2f), Quaternion.identity);
            tempScript = tempPortal.GetComponent<Teleporter>();
            tempPortal.transform.Rotate(0f, 90f * currentPortal.direction, 0f);
            tempPortal.transform.Translate(0, 0, portalWidth / 2f - pillarOffset, Space.Self);
            tempScript.projectionQuad.Translate(-cameraOffset, 0, 0, Space.World);

            tempPortal.name = "Back Teleporter " + i;
            tempPortal.transform.parent = transform;
            tempScript.portalID = i;
            tempScript.isForwardTeleporter = false;
            nextProjectionQuadArray[i] = tempScript.projectionQuad.gameObject;
        }

        //nextProjectionQuad = nextProjectionQuadArray[0];
        //prevProjectionQuad = prevProjectionQuadArray[prevProjectionQuadArray.Length - 1];
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp("space"))
        {
            currentMaze++;
            Debug.Log(currentMaze % prevProjectionQuadArray.Length);
            prevProjectionQuad = prevProjectionQuadArray[(currentMaze - 1) % prevProjectionQuadArray.Length];
            nextProjectionQuad = nextProjectionQuadArray[currentMaze % nextProjectionQuadArray.Length];
            Camera.main.transform.Translate(cameraOffset, 0, 0, Space.World);
        }
    }

    public void TeleportPlayer(int mazeID)
    {
        currentMaze = mazeID;
        prevProjectionQuad = prevProjectionQuadArray[(currentMaze - 1) % prevProjectionQuadArray.Length];
        nextProjectionQuad = nextProjectionQuadArray[currentMaze % nextProjectionQuadArray.Length];
    }
}
