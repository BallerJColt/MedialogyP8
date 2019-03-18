using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class TerrainGenerator : MonoBehaviour
    {
        GameObject newSegment;
        GameObject newPillar;
        public float wallOffset = 0f;
        public GameObject wallSegment;
        public GameObject woodPillar;
        // Start is called before the first frame update
        void Start()
        {
            GenerateTerrainEvent.RegisterListener(OnGenerateTerrain);
        }

        void OnGenerateTerrain(GenerateTerrainEvent generateTerrain)
        {
            Transform tileTransform = generateTerrain.go.transform;
            
            for (int i = 0; i < generateTerrain.wallArray.Length; i++)
            {
                if(generateTerrain.wallArray[i] == 0)
                {
                    switch (i)
                    {
                        case 0:
                            newSegment = Instantiate (wallSegment, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + wallOffset, tileTransform.position.y, tileTransform.position.z + (generateTerrain.tileWidth / 2) - wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newSegment.transform.parent = tileTransform;
                            break;
                        case 1:
                            newSegment = Instantiate (wallSegment, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - wallOffset, tileTransform.position.y, tileTransform.position.z + (generateTerrain.tileWidth / 2) - wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newSegment.transform.parent = tileTransform;
                            break;
                        case 2:
                            newSegment = Instantiate (wallSegment, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - wallOffset, tileTransform.position.y, tileTransform.position.z - (generateTerrain.tileWidth / 2) + wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newSegment.transform.parent = tileTransform;
                            break;
                        case 3:
                            newSegment = Instantiate (wallSegment, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + wallOffset, tileTransform.position.y, tileTransform.position.z - (generateTerrain.tileWidth / 2) + wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newSegment.transform.parent = tileTransform;
                            break;
                        default:
                            break;
                    }
                }
            }
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.x / 2), tileTransform.position.y + 0.5f, tileTransform.position.z - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.x / 2), tileTransform.position.y + 0.5f, tileTransform.position.z + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.x / 2), tileTransform.position.y + 0.5f, tileTransform.position.z - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.x / 2), tileTransform.position.y + 0.5f, tileTransform.position.z + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.parent = tileTransform;
        }
    }
}