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

    void Update()
    {
        if (Input.GetKeyDown("b"))
        {
            TurnOnLights();
        }
        if (Input.GetKeyDown("v"))
        {
            TurnOffLights();
        }
    }

    public void TurnOnLights()
    {
        foreach (Light light in lights)
        {
            light.enabled = true;
            light.GetComponentInParent<Renderer>().material.EnableKeyword("_EMISSION");
        }
    }

    public void TurnOffLights()
    {        
        foreach (Light light in lights)
        {
            light.enabled = false;
            light.GetComponentInParent<Renderer>().material.DisableKeyword("_EMISSION");
        }
    }
}
