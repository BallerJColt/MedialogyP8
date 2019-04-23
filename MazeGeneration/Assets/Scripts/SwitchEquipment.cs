using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR.InteractionSystem;
using Valve.VR;

public class SwitchEquipment : MonoBehaviour
{
    public NotePadScript notePadScript;
    //public string inputKey;
    GameObject equipmentList;
    public List<GameObject> equipments;
    Transform clipBoardPos;
    Transform timeDevicePos;
    public SteamVR_Action_Boolean triggerClick;
    bool triggerPressed;
    public SteamVR_Action_Boolean touchPadClick;
    bool touchPadPressed;
    public SteamVR_Input_Sources inputSource;

    public SwitchEquipment otherSwitcher;

    int currentEquipmentIndex;

    string currentEquipmentName;

    // Start is called before the first frame update
    void Start()
    {
        //hand = GetComponent<Hand>();

        //get equipment from equipment list
        equipmentList = GameObject.Find("Equipment List");

        if(gameObject.name == "LeftHand")
        {
            AddEquipment("EmptyLeft");
        }
        else if (gameObject.name == "RightHand")
        {
            AddEquipment("EmptyRight");
        }
        //GetAllEquipment();
        //GetEquipmentPositions(); // check if it is needed

        SwitchToSpecificEquipment("TimeDevice");
    }
    void Update()
    {
        if (!otherSwitcher)
        {
            string otherName;
            if (gameObject.name == "Hand1")
            {
                otherName = "Hand2";
            }
            else
            {
                otherName = "Hand1";
            }
            if (GameObject.Find(otherName) != null)
            {
                otherSwitcher = GameObject.Find(otherName).GetComponent<SwitchEquipment>();
            }

        }

        //if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Trigger) && currentEquipmentName == "NoteBook")
        if (triggerClick.GetState(inputSource) && currentEquipmentName == "ClipBoard")

        {
            FindObjectOfType<AudioManager>().Play("NoteSwitchSound");
            notePadScript.NotepadSwitchPage();
        }
        if (touchPadClick.GetState(inputSource))
        {
            SwitchToNextEquipment();
        }


        //if (hand.controller.GetPressDown(SteamVR_Controller.ButtonMask.Touchpad))
        {
            SwitchToNextEquipment();
        }
    }

    void GetAllEquipment()
    {
        equipmentList = GameObject.Find("Equipment List");
        for (int i = 0; i < equipmentList.transform.childCount; i++)
        {
            equipments.Add(equipmentList.transform.GetChild(i).gameObject);
        }
    }

    void AddEquipment(string equipmentName)
    {
        for (int i = 0; i < equipmentList.transform.childCount; i++)
        {
            if(equipmentList.transform.GetChild(i).name == equipmentName)
            {
                equipments.Add(equipmentList.transform.GetChild(i).gameObject);
            }
        }

    }

    void GetEquipmentPositions()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (child.name == "ClipBoardPos")
            {
                clipBoardPos = child;
            }
            else if (child.name == "TimeDevicePos")
            {
                timeDevicePos = child;
            }
        }
    }

    void SwitchToNextEquipment()
    {
        //set the currentequipment to be the next in the order by incrementing the index
        currentEquipmentIndex++;

        //set currentEquipment to index 0 
        if (currentEquipmentIndex > equipments.Count - 1)
        {
            currentEquipmentIndex = 0;
        }

        if (otherSwitcher.currentEquipmentName == equipments[currentEquipmentIndex].name // check if the other hand is holding the item you want to switch to, if so skip to the next item again.
        && otherSwitcher.currentEquipmentName != "EmptyHand") //allow the player to hand to empty hands
        {
            currentEquipmentIndex++;
        }

        //set currentEquipment to index 0 
        if (currentEquipmentIndex > equipments.Count - 1)
        {
            currentEquipmentIndex = 0;
        }

        ShowOnlyCurrentEquipment(currentEquipmentIndex);

    }

    void SwitchToSpecificEquipment(string name)
    {
        for (int i = 0; i < equipments.Count; i++)
        {

            if (equipments[i].name == name)
            {
                currentEquipmentIndex = i;
                break;
            }
        }
        ShowOnlyCurrentEquipment(currentEquipmentIndex);
    }

    void ShowOnlyCurrentEquipment(int currentEquipment)
    {
        for (int i = 0; i < equipments.Count; i++)
        {
            if (i == currentEquipment) //turn on the equipment that is player has in hand
            {
                equipments[i].SetActive(true);
                currentEquipmentName = equipments[i].name;
                positionEquipment(equipments[i]);
            }
            else if (i != otherSwitcher.currentEquipmentIndex) //you should not set the equipment on the other hand off
            {
                equipments[i].SetActive(false);
            }
        }
    }

    void positionEquipment(GameObject equipment)
    {
        if (equipment.name == "ClipBoard")
        {
            equipment.transform.SetPositionAndRotation(clipBoardPos.position, clipBoardPos.rotation);
        }
        else if (equipment.name == "TimeDevice")
        {
            equipment.transform.SetPositionAndRotation(timeDevicePos.position, timeDevicePos.rotation);
        }
    }


}
