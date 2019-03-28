using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGeneratorCaller : MonoBehaviour
{

    private MapManager mapManager;
    private PortalGenerator portalGenerator;
    TileInfo[] portalInfos;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(LateStart());
        mapManager = GameObject.Find("MapGenerator").GetComponent<MapManager>(); 
        portalGenerator = GameObject.Find("PortalGenerator").GetComponent<PortalGenerator>(); 
    }

    IEnumerator LateStart()
    {
        //returning 0 will make it wait 1 frame
        //that way portalInfo in mapGenerator contain all dead ends for portals
        yield return 50;

        portalInfos = mapManager.portalInfo;
        portalGenerator.GeneratePortals(portalInfos,mapManager.tileWidth,mapManager.wallWidth);

    }
}
