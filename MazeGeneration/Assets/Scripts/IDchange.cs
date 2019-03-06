using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EventCallbacks
{
    public class IDchange : MonoBehaviour
    {
        int tileID;
        int[] wallArray = { 0, 0, 0, 0 };
        // Start is called before the first frame update
        void Start()
        {
            tileID = Random.Range(0, 10);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.D))
            {
                GenerateTile();
            }
        }

        void GenerateTile()
        {
            GenerateTerrainEvent gtei = new GenerateTerrainEvent();
            gtei.Description = "Two tiles are connected. Tile ID is changed and terrain needs to be generated";
            gtei.wallArray = wallArray;
            gtei.tileID = tileID;
            //ID Changing when creating new tile
            gtei.FireEvent();

            tileID = Random.Range(0, 10);
        }
    }
} 