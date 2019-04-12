using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PortalRenderController : MonoBehaviour
{
    public Camera previousPortalCamera;
    public Camera nextPortalCamera;

    public GameObject portalPrefab;
    public int mazeCount;
    public int currentMaze;
    public float cameraOffset;
    public float portalWidth;
    public float pillarOffset = 0.1f;
    private float halfmazeWidth;
    private float halfmazeHeight;
    private CommandBuffer _prevDepthHackBuffer;
    private CommandBuffer _nextDepthHackBuffer;

    public Renderer _prevHackQuad;
    public Renderer _nextHackQuad;
    Mesh prevPlaneMesh;
    Mesh nextPlaneMesh;
    Vector3[] prevVerts;
    Vector3[] nextVerts;
    Plane prevClip;
    Plane nextClip;

    public Renderer[] prevCullQuadArray;
    public Renderer[] nextCullQuadArray;
    MapManager mapManager;
    // Start is called before the first frame update
    void Start()
    {
        mapManager = GameObject.Find("MapManager").GetComponent<MapManager>();
        mazeCount = mapManager.mapSequence.Length;
        cameraOffset = mapManager.mazeRows * mapManager.tileWidth + 1;
        portalWidth = mapManager.tileWidth;
        halfmazeHeight = mapManager.mazeRows * mapManager.tileWidth / 2;
        halfmazeWidth = mapManager.mazeCols * mapManager.tileWidth / 2;

        prevCullQuadArray = new Renderer[mazeCount - 1];
        nextCullQuadArray = new Renderer[mazeCount - 1];

        InitializePortals();
        RenderTexture prevRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        RenderTexture nextRenderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        Shader.SetGlobalTexture("_PrevPortalTexture", prevRenderTexture);
        Shader.SetGlobalTexture("_NextPortalTexture", nextRenderTexture);
        previousPortalCamera.targetTexture = prevRenderTexture;
        nextPortalCamera.targetTexture = nextRenderTexture;

        CullShit();

    }

    void InitializePortals()
    {
        Debug.Log("portals will go here:");
        for (int i = 0; i < mazeCount - 1; i++)
        {
            TileInfo currentPortal = mapManager.mapSequence[i].endSeed;
            currentPortal.PrintTile();
            GameObject tempPortal = Instantiate(portalPrefab, new Vector3(i * cameraOffset - halfmazeWidth + currentPortal.column * portalWidth + portalWidth / 2f, 0, halfmazeHeight - currentPortal.row * portalWidth - portalWidth / 2f), Quaternion.identity);
            Teleporter tempScript = tempPortal.GetComponent<Teleporter>();
            tempPortal.transform.Rotate(0f, 180 + 90f * currentPortal.direction, 0f);
            tempPortal.transform.Translate(0, 0, portalWidth / 2f - pillarOffset, Space.Self);
            tempScript.cullingQuad.Translate(cameraOffset, 0, 0, Space.World);

            tempPortal.name = "Fowrard Teleporter " + i;
            tempPortal.transform.parent = transform;
            tempScript.portalID = i;
            tempScript.isForwardTeleporter = true;
            nextCullQuadArray[i] = tempScript.cullingQuad.GetComponent<Renderer>();
            tempScript.renderQuad.GetComponent<Renderer>().material = new Material(Shader.Find("Special/NextPortal"));

            tempPortal = Instantiate(portalPrefab, new Vector3((i + 1) * cameraOffset - halfmazeWidth + currentPortal.column * portalWidth + portalWidth / 2f, 0, halfmazeHeight - currentPortal.row * portalWidth - portalWidth / 2f), Quaternion.identity);
            tempScript = tempPortal.GetComponent<Teleporter>();
            tempPortal.transform.Rotate(0f, 90f * currentPortal.direction, 0f);
            tempPortal.transform.Translate(0, 0, portalWidth / 2f - pillarOffset, Space.Self);
            tempScript.cullingQuad.Translate(-cameraOffset, 0, 0, Space.World);

            tempPortal.name = "Back Teleporter " + i;
            tempPortal.transform.parent = transform;
            tempScript.portalID = i;
            prevCullQuadArray[i] = tempScript.cullingQuad.GetComponent<Renderer>();
            tempScript.renderQuad.GetComponent<Renderer>().material = new Material(Shader.Find("Special/PrevPortal"));
        }

        _nextHackQuad = nextCullQuadArray[0];
        _prevHackQuad = prevCullQuadArray[prevCullQuadArray.Length - 1];
    }

    void CullShit()
    {
        previousPortalCamera.RemoveAllCommandBuffers();
        nextPortalCamera.RemoveAllCommandBuffers();

        _prevDepthHackBuffer = new CommandBuffer();
        _prevDepthHackBuffer.ClearRenderTarget(true, true, Color.black, 0);
        _prevDepthHackBuffer.name = "Depth Culling for Prev Maze";
        _prevDepthHackBuffer.DrawRenderer(_prevHackQuad, new Material(Shader.Find("Hidden/DepthHack")));

        _nextDepthHackBuffer = new CommandBuffer();
        _nextDepthHackBuffer.ClearRenderTarget(true, true, Color.black, 0);
        _nextDepthHackBuffer.name = "Depth Culling for Next Maze";
        _nextDepthHackBuffer.DrawRenderer(_nextHackQuad, new Material(Shader.Find("Hidden/DepthHack")));

        previousPortalCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _prevDepthHackBuffer);
        nextPortalCamera.AddCommandBuffer(CameraEvent.BeforeForwardOpaque, _nextDepthHackBuffer);

        prevPlaneMesh = _prevHackQuad.GetComponent<MeshFilter>().mesh;
        nextPlaneMesh = _nextHackQuad.GetComponent<MeshFilter>().mesh;
    }

    // Update is called once per frame
    void Update()
    {
        nextPortalCamera.transform.position = new Vector3(Camera.main.transform.position.x + cameraOffset, Camera.main.transform.position.y, Camera.main.transform.position.z);
        nextPortalCamera.transform.rotation = Camera.main.transform.rotation;

        previousPortalCamera.transform.position = new Vector3(Camera.main.transform.position.x - cameraOffset, Camera.main.transform.position.y, Camera.main.transform.position.z);
        previousPortalCamera.transform.rotation = Camera.main.transform.rotation;

        prevVerts = prevPlaneMesh.vertices;
        nextVerts = nextPlaneMesh.vertices;

        for (int i = 0; i < 3; i++)
        {
            prevVerts[i] = _prevHackQuad.transform.TransformPoint(prevVerts[i]);
            nextVerts[i] = _nextHackQuad.transform.TransformPoint(nextVerts[i]);
        }

        prevClip = new Plane(prevVerts[2], prevVerts[1], prevVerts[0]);
        nextClip = new Plane(nextVerts[2], nextVerts[1], nextVerts[0]);

        Vector4 prevClipPlaneWorldSpace = new Vector4(prevClip.normal.x, prevClip.normal.y, prevClip.normal.z, -Vector3.Dot(prevClip.normal, _prevHackQuad.transform.position));
        Vector4 nextClipPlaneWorldSpace = new Vector4(nextClip.normal.x, nextClip.normal.y, nextClip.normal.z, -Vector3.Dot(nextClip.normal, _nextHackQuad.transform.position));

        Vector4 prevClipPlaneCameraSpace = Matrix4x4.Transpose(previousPortalCamera.cameraToWorldMatrix) * prevClipPlaneWorldSpace;
        Vector4 nextClipPlaneCameraSpace = Matrix4x4.Transpose(nextPortalCamera.cameraToWorldMatrix) * nextClipPlaneWorldSpace;

        previousPortalCamera.projectionMatrix = previousPortalCamera.CalculateObliqueMatrix(prevClipPlaneCameraSpace);
        nextPortalCamera.projectionMatrix = nextPortalCamera.CalculateObliqueMatrix(nextClipPlaneCameraSpace);

        if (Input.GetKeyUp("space"))
        {
            currentMaze++;
            Debug.Log(currentMaze % prevCullQuadArray.Length);
            _prevHackQuad = prevCullQuadArray[(currentMaze - 1) % prevCullQuadArray.Length];
            _nextHackQuad = nextCullQuadArray[currentMaze % nextCullQuadArray.Length];
            Camera.main.transform.Translate(cameraOffset, 0, 0, Space.World);
            CullShit();
        }
    }

    public void TeleportPlayer(int mazeID)
    {
        currentMaze = mazeID;
        _prevHackQuad = prevCullQuadArray[(currentMaze - 1) % prevCullQuadArray.Length];
        _nextHackQuad = nextCullQuadArray[currentMaze % nextCullQuadArray.Length];
        //Camera.main.transform.Translate(cameraOffset, 0, 0, Space.World);
        CullShit();
    }
}
