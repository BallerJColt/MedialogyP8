using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalPair : MonoBehaviour
{
    public Vector3 tilePosition;
    public Quaternion tileRotation;
    public Vector3 offset;
    public GameObject entrancePortal;
    public  GameObject exitPortal;
    public GameObject portalCamera;
    public GameObject playerCamera;
    public Material CameraMat;

    public TeleportTrigger entranceTrigger;
    public TeleportTrigger exitTrigger;
    public Transform cameraRig;
    public Vector3 cameraRigToEntrancePortal;

    public void PortalPairConstructor(Vector3 tilePosition, Quaternion tileRotation, Vector3 offset)
    {
        this.tilePosition = tilePosition;
        this.tileRotation = tileRotation;
        this.offset = offset;
        SetVariables();
    }
    void SetVariables()
    {
        //set entrancePortal, exitPortal and portalCamera
        SearchThroughChildObjects();
        
        FindPlayerCamera();

        entranceTrigger = entrancePortal.GetComponent<TeleportTrigger>();
        exitTrigger = exitPortal.GetComponent<TeleportTrigger>();

        cameraRig = GameObject.Find("[CameraRig]").transform;
        //cameraRigToEntrancePortal = cameraRig.position - entrancePortal.transform.position;
    }
    void FindPlayerCamera()
    {

        //playerCamera = GameObject.Find("VRCamera");
        if(!playerCamera)
        {
            playerCamera = GameObject.Find("FallbackObjects");
        }
    }
    void SearchThroughChildObjects()
    {
        for (int i = 0; i < transform.GetChildCount(); i++)
        {
            if(transform.GetChild(i).name == "ent")
            {
                entrancePortal = transform.GetChild(i).gameObject;
            }   
            else if(transform.GetChild(i).name == "exit")
            {
                exitPortal = transform.GetChild(i).gameObject;
            }
            else if(transform.GetChild(i).name == "cam")
            {
                portalCamera = transform.GetChild(i).gameObject;
            }    
        }
    }
    void Start()
    {
        PositionPortals();
        cameraRigToEntrancePortal = cameraRig.position - entrancePortal.transform.position;
    }

    void Update()
    {
        UpdatePortalCameraTransform();
        CheckForTeleport();
    }
    public void UpdatePortalCameraTransform()
    {
        //
        Vector3 playerOffsetFromPortal = entrancePortal.transform.position - playerCamera.transform.position;
        portalCamera.transform.position = exitPortal.transform.position - playerOffsetFromPortal;
        //
        portalCamera.transform.forward = playerCamera.transform.forward;
    }
    public void SetUpMaterialForPortals()
    {
        //get the targettextures from the portalCamera and set it on the entrance and exit portals.
    }
    public void PositionPortals()
    {
        
        Debug.Log("Positioning Portals");
        //entrance
        entrancePortal.transform.position = tilePosition;
        entrancePortal.transform.rotation = tileRotation;

        //exit
        exitPortal.transform.position = tilePosition+offset;
        exitPortal.transform.rotation = Quaternion.Inverse(tileRotation);
    }
    public void CheckForTeleport()
    {
        //if(entranceTrigger.shouldTeleport || exitTrigger.shouldTeleport)        
        if(entrancePortal.GetComponent<TeleportTrigger>().shouldTeleport || exitPortal.GetComponent<TeleportTrigger>().shouldTeleport)
        {
            TeleportPlayer();
            entrancePortal.GetComponent<TeleportTrigger>().shouldTeleport = false;
            exitPortal.GetComponent<TeleportTrigger>().shouldTeleport= false;
        }
    }
    public void TeleportPlayer()
    {
        //switch which is the entrance and exit portal
        
        //make temp position and rotation
        Vector3 tempPos = entrancePortal.transform.position;
        Quaternion tempRot = entrancePortal.transform.rotation;
       //switch them around
        entrancePortal.transform.position = exitPortal.transform.position;
        entrancePortal.transform.rotation = exitPortal.transform.rotation;
        exitPortal.transform.position = tempPos;
        exitPortal.transform.rotation = tempRot;



        //switch the position of the camera rig 
        cameraRig.position = entrancePortal.transform.position + cameraRigToEntrancePortal;
        playerCamera.transform.position = portalCamera.transform.position;
        //portalCamera.transform.position = temp.transform.position;

        //portalCamera.transform.position = temp.position;

    }
}
