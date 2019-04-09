using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FuseBoxPuzzle : MonoBehaviour
{

    GameObject[] fusePlugs;
    private int connectedPlugs = 0;
    public int correctPlugs = 0;

    // Start is called before the first frame update
    void Start()
    {
        EventCallbacks.FusePuzzlePlugIn.RegisterListener(OnPluggedIn);
        EventCallbacks.FusePuzzlePlugOut.RegisterListener(OnPluggedOut);
    }

    void OnPluggedIn(EventCallbacks.FusePuzzlePlugIn fusepuzzle)
    {
        connectedPlugs++;

        if (fusepuzzle.go.tag == fusepuzzle.sGo.tag)
        {
            correctPlugs++;
        }

        if (correctPlugs == 5)
        {
            gameObject.GetComponentInChildren<Light>().enabled = true;
        }

        Debug.Log(connectedPlugs + " Correct: " + correctPlugs);
    }

    void OnPluggedOut(EventCallbacks.FusePuzzlePlugOut fusepuzzle)
    {
        connectedPlugs--;

        if (fusepuzzle.go.tag == fusepuzzle.sGo.tag)
        {
            correctPlugs--;
        }
        
        gameObject.GetComponentInChildren<Light>().enabled = true;

        Debug.Log(connectedPlugs + " Correct: " + correctPlugs);
    }
}
