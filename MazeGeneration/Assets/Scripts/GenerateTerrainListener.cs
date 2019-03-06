using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class GenerateTerrainListener : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            GenerateTerrainEvent.RegisterListener(OnGenerateTerrain);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        void OnGenerateTerrain(GenerateTerrainEvent generateTerrain)
        {
            Debug.Log("Alerted about terrain generation on ID: " + generateTerrain.tileID);
        }
    }
}