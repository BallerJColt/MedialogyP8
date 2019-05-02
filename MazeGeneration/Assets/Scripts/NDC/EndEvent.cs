using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndEvent : MonoBehaviour
{
    // AudioSource metalDoorSlamSound;
    Camera mainCam;
    public float doorShutdelay = 10;
    float currentTime;
    bool timerActive = false;

    void Start()
    {
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    void Update()
    {
        if (timerActive)
        {
            if (doorShutdelay <= Time.time - currentTime)
            {
                FindObjectOfType<AudioManager>().Play("MetalDoorSlam");
                mainCam.cullingMask = 0;
                timerActive = false;
            }
        }  
    }

    public void executeLastEvent()
    {
        currentTime = Time.time;
        timerActive = true;
    }
}
