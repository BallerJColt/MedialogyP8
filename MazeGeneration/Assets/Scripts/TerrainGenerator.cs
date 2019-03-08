using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class TerrainGenerator : MonoBehaviour
    {
        public GameObject wallSegment;
        // Start is called before the first frame update
        void Start()
        {
            GenerateTerrainEvent.RegisterListener(OnGenerateTerrain);
        }

        void OnGenerateTerrain(GenerateTerrainEvent generateTerrain)
        {
            //Debug.Log("Alerted about terrain generation on ID: " + generateTerrain.tileID);
            for (int i = 0; i < generateTerrain.wallArray.Length; i++)
            {
                if(generateTerrain.wallArray[i] == 0)
                {
                    GameObject newSegment = Instantiate (wallSegment, new Vector3(generateTerrain.go.transform.position.x, generateTerrain.go.transform.position.y, generateTerrain.go.transform.position.z) , Quaternion.identity);
                    newSegment.transform.parent = generateTerrain.go.transform;
                }
            }
        }
    }
}