using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SetMemoryPositions : MonoBehaviour
{
    [SerializeField]
    public LayerMask layerMask;
    [SerializeField]    
    public string json;
    public SerializePositions serializedTree = new SerializePositions();
    public SerializePositions serializedGrass = new SerializePositions();
    public SerializePositions serializedWater = new SerializePositions();
    public GameObject[] animals;
    public GameObject[] trees;

    private Ray ray;
    private RaycastHit hit;

    void Start()
    {
        SerializeJson("C:/Users/strus/HunterGatherers_base/Data/WaterPosition.json", ref serializedWater);
        SerializeJson("C:/Users/strus/HunterGatherers_base/Data/GrassPosition.json", ref serializedGrass);
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.O))
        {
            if (GetRayOfCursorPosition())
            {
                Debug.Log("collider je " + hit.collider.gameObject.name);
            }

        }

        if (Input.GetKeyUp(KeyCode.V))
        {
            if (GetRayOfCursorPosition())
            {
                AddToMemory(hit.point, "C:/Users/strus/HunterGatherers_base/Data/WaterPosition.json", serializedWater);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.G))
        {
            if (GetRayOfCursorPosition())
            {
                AddToMemory(hit.point, "C:/Users/strus/HunterGatherers_base/Data/GrassPosition.json", serializedGrass);
            }
        }
        
        if (Input.GetKeyUp(KeyCode.T))
        {
            GetTreePositions();
        }
    }

    private bool GetRayOfCursorPosition()
    {
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            return true;
        }
        return false;
    }
    
    private void AddToMemory(Vector3 position, string jsonDirectory, SerializePositions serializedPositions)
    {
        CreateJsonFromPosition(position, serializedPositions);
        System.IO.File.WriteAllText(jsonDirectory, json);
    }

    private void CreateJsonFromPosition(Vector3 position, SerializePositions serializedPositions)
    {
        serializedPositions.positions.Add(position);
        json = JsonUtility.ToJson(serializedPositions, true);
    }

    private void SerializeJson(string jsonDirectory, ref SerializePositions serializedPositions)
    {
        string jsonString = System.IO.File.ReadAllText(jsonDirectory);
        serializedPositions = JsonUtility.FromJson<SerializePositions>(jsonString);
    }

    private void GetTreePositions()
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            TreeInstance[] treesOnTerrain = terrain.terrainData.treeInstances;
            foreach (TreeInstance tree in treesOnTerrain)
            {
                Vector3 treePosition = TerrainToWorld(tree.position, terrain);
                Debug.Log("vyrob strom");
                AddToMemory(treePosition, "C:/Users/strus/HunterGatherers_base/Data/TreePosition.json", serializedTree);
            }
        }
    }

    private Vector3 TerrainToWorld(Vector3 positionOnTerrain, Terrain terrain)
    {
        Vector3 terrainPosition = new Vector3();
        terrainPosition.x = positionOnTerrain.x * terrain.terrainData.size.x;
        terrainPosition.z = positionOnTerrain.z * terrain.terrainData.size.z;
        terrainPosition.y = terrain.terrainData.GetInterpolatedHeight(positionOnTerrain.x, positionOnTerrain.z);

        Vector3 positionOnWorld = terrainPosition + terrain.transform.position;

        return positionOnWorld;
    }
}
