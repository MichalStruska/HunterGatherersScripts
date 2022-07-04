using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTerrainLayers : MonoBehaviour
{
    public TerrainLayer[] layers;
    private GameObject sun;
    private Vector3 treePosition;
    public LayerMask SeeLayer;
    
    void Start()
    {
        sun = GameObject.Find("Sun");
    }
    
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            Debug.Log("pocet terenu " + Terrain.activeTerrains.Length);
            foreach (Terrain tera in Terrain.activeTerrains)
            {

                //tera.basemapDistance = 200;
                TreeInstance[] paryTrees = tera.terrainData.treeInstances;
                TreePrototype[] treePrototypes = tera.terrainData.treePrototypes;
                foreach (TreeInstance tree in paryTrees)
                {
                    Debug.Log("vyska stromu " + tree.heightScale + " " + treePrototypes[tree.prototypeIndex].prefab.transform.lossyScale.y);
                    treePosition = TerrainToWorld(tree.position, tera);
                    Vector3 groundInShadow = FindSpaceInShadow();

                    Debug.Log("ground in shadow " + groundInShadow);
                    Debug.DrawLine(groundInShadow, new Vector3(groundInShadow.x, groundInShadow.y + 50, groundInShadow.z), Color.blue, 30f);

                }
                
                Debug.Log("teren set");

                //tera.terrainData.terrainLayers = layers;
                //float[,,] map = new float[tera.terrainData.alphamapWidth, tera.terrainData.alphamapHeight, 3];
                //for (int y = 0; y < tera.terrainData.alphamapHeight; y++)
                //{
                //    for (int x = 0; x < tera.terrainData.alphamapWidth; x++)
                //    {
                //        map[x, y, 0] = 1f;
                //        map[x, y, 1] = 0f;
                //        map[x, y, 2] = 0f;
                //        //float normX = x * 1.0f / (tera.terrainData.alphamapWidth - 1);
                //        //float normY = y * 1.0f / (tera.terrainData.alphamapHeight - 1);

                //        //var angle = tera.terrainData.GetSteepness(normX, normY);

                //        //var frac = angle / 50;
                //        //map[x, y, 1] = (float)frac;
                //        //map[x, y, 0] = (float)(1 - frac);
                //        //map[x, y, 2] = 0f;

                //    }
                //}
                //tera.terrainData.SetAlphamaps(0, 0, map);
            }
        }
    }

    private Vector3 TerrainToWorld(Vector3 positionOnTerrain, Terrain terrain)
    {
        
        float mapPositionX = positionOnTerrain.x / terrain.terrainData.alphamapWidth;
        float mapPositionZ = positionOnTerrain.z / terrain.terrainData.alphamapHeight;

        Vector3 terrainPosition = new Vector3();
        terrainPosition.x = positionOnTerrain.x * terrain.terrainData.size.x;
        terrainPosition.z = positionOnTerrain.z * terrain.terrainData.size.z;
        terrainPosition.y = terrain.terrainData.GetInterpolatedHeight(positionOnTerrain.x, positionOnTerrain.z);

        Vector3 positionOnWorld = terrainPosition + terrain.transform.position;
        Debug.Log("terenni pozice " + positionOnWorld);

        return positionOnWorld;
    }

    private Vector3 FindSpaceInShadow()
    {
        RaycastHit hit;
        for (int i = 20; i < 60; i += 10)
        {
            Vector3 shiftedPosition = new Vector3(treePosition.x, treePosition.y + i, treePosition.z);
            //Debug.DrawLine(sun.transform.position, elongatedPosition, Color.red, 30f);
            
            //Debug.DrawRay(sun.transform.position, (shiftedPosition - sun.transform.position) * 20, Color.red, 30f);
            Vector3 higherTreePosition = new Vector3(treePosition.x, treePosition.y + 20, treePosition.z);
            Debug.DrawRay(higherTreePosition, (shiftedPosition - sun.transform.position) * 1, Color.red, 30f);
            if (Physics.Raycast(higherTreePosition, (shiftedPosition - sun.transform.position), out hit, SeeLayer))
            {
                Debug.Log("raycast " + hit.point);
                return hit.point;
            }
        }

        return Vector3.zero;
    }
}
