using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineLights : MonoBehaviour
{
    private Light[] lights;

    void Start()
    {
        lights = gameObject.GetComponentsInChildren<Light>();
    }

    public void TurnOnLights()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
        }
    }

    public void TurnOffLights()
    {        
        foreach (Light light in lights)
        {
            light.enabled = false;
        }
    }
}
