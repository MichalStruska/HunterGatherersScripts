using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class SetTerrainAttributesEdit : MonoBehaviour
{
    public Terrain activeTerrain;
    void Start()
    {
        Debug.Log("start");
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            activeTerrain = terrain;
            activeTerrain.basemapDistance = 300;
            activeTerrain.heightmapPixelError = 100;
            activeTerrain.treeDistance = 4000;
            activeTerrain.treeBillboardDistance = 500f;

            SetTerrainDetailSettings();
            //terrain.Flush();
            Debug.Log("teren setss");
        }
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.B))
        {

            //foreach (Terrain terrain in Terrain.activeTerrains)
            //{

            //    terrain.basemapDistance = 300;
            //    terrain.heightmapPixelError = 100;
            //    terrain.treeDistance = 5000;
            //    terrain.Flush();
            //    Debug.Log("teren st");
            //}
        }
    }

    void SetTerrainDetailSettings()
    {
        activeTerrain.detailObjectDistance = 1000;
        //activeTerrain.detailObjectDensity = 0.8f;
        //activeTerrain.terrainData.SetDetailResolution(24, 24);
    }
}
