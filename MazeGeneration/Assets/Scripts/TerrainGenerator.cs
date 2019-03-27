using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class TerrainGenerator : MonoBehaviour
    {
        GameObject newCeiling;
        GameObject newWall;
        GameObject newPillar;
        public float wallOffset = 0f;
        public float wallHeight = 1f;
        public GameObject ceiling;
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

            // Place ceiling
            newCeiling = Instantiate (ceiling, new Vector3(tileTransform.position.x, wallHeight, tileTransform.position.z), Quaternion.AngleAxis(90, Vector3.left));
            newCeiling.transform.parent = tileTransform;
            
            // Place walls for each tile
            for (int i = 0; i < generateTerrain.wallArray.Length; i++)
            {
                if(generateTerrain.wallArray[i] == 0)
                {
                    switch (i)
                    {
                        case 0:
                            newWall = Instantiate (wallSegment, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2), tileTransform.position.y, tileTransform.position.z + (generateTerrain.tileWidth / 2) - wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, wallHeight, newWall.transform.localScale.z);
                            newWall.transform.parent = tileTransform;
                            break;
                        case 1:
                            newWall = Instantiate (wallSegment, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - wallOffset, tileTransform.position.y, tileTransform.position.z + (generateTerrain.tileWidth / 2)) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, wallHeight, newWall.transform.localScale.z);
                            newWall.transform.parent = tileTransform;
                            break;
                        case 2:
                            newWall = Instantiate (wallSegment, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2), tileTransform.position.y, tileTransform.position.z - (generateTerrain.tileWidth / 2) + wallOffset) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, wallHeight, newWall.transform.localScale.z);
                            newWall.transform.parent = tileTransform;
                            break;
                        case 3:
                            newWall = Instantiate (wallSegment, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + wallOffset, tileTransform.position.y, tileTransform.position.z - (generateTerrain.tileWidth / 2)) , Quaternion.AngleAxis(i * 90, Vector3.up));
                            newWall.transform.localScale = new Vector3(newWall.transform.localScale.x, wallHeight, newWall.transform.localScale.z);
                            newWall.transform.parent = tileTransform;
                            break;
                        default:
                            break;
                    }
                }
            }

            // Place corner pillars
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.x / 2), tileTransform.position.y + (0.5f * wallHeight), tileTransform.position.z - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.localScale = new Vector3(newPillar.transform.localScale.x, wallHeight, newPillar.transform.localScale.z);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.x / 2), tileTransform.position.y + (0.5f * wallHeight), tileTransform.position.z + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.localScale = new Vector3(newPillar.transform.localScale.x, wallHeight, newPillar.transform.localScale.z);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.x / 2), tileTransform.position.y + (0.5f * wallHeight), tileTransform.position.z - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.localScale = new Vector3(newPillar.transform.localScale.x, wallHeight, newPillar.transform.localScale.z);
            newPillar.transform.parent = tileTransform;
            newPillar = Instantiate(woodPillar, new Vector3(tileTransform.position.x - (generateTerrain.tileWidth / 2) + (woodPillar.transform.localScale.x / 2), tileTransform.position.y + (0.5f * wallHeight), tileTransform.position.z + (generateTerrain.tileWidth / 2) - (woodPillar.transform.localScale.z / 2)), Quaternion.identity);
            newPillar.transform.localScale = new Vector3(newPillar.transform.localScale.x, wallHeight, newPillar.transform.localScale.z);
            newPillar.transform.parent = tileTransform;
        }
    }
}