using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainTreeCreator : MonoBehaviour
{
    public GameObject[] treePrefabs;
    TreePrototype[] treePrototypeCollection = new TreePrototype[12];
    TreeInstance treeInstance;
    private RaycastHit hit;
    public LayerMask layerMask;
    private Terrain activeTerrain;
    private int prototypeNumber;
    private List<int> prototypeNumberList;

    void Start()
    {
        FillPrototypeNumberList();
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.H))
        {
            //AddTreePrototypesToTerrains();
            if (InputMethods.GetRayOfCursorPosition(layerMask, out hit))
            {
                CreateTreesAroundSpot();
            }            
        }
        
        if (Input.GetKeyUp(KeyCode.K))
        {
            if (InputMethods.GetRayOfCursorPosition(layerMask, out hit))
            {
                DeleteTerrainTrees();
            }   
        }
    }


    private void AddTreePrototypesToTerrains()
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            AddAllPrefabs();
            terrain.terrainData.treePrototypes = treePrototypeCollection;
        }
    }

    private void AddAllPrefabs()
    {
        for (int i = 0; i < treePrefabs.Length; i++)
        {
            GameObject treePrefab = treePrefabs[i];
            TreePrototype treePrototype = new TreePrototype();
            treePrototype.prefab = treePrefab;

            treePrototypeCollection[i] = treePrototype;

        }
    }

    private Terrain FindActiveTerrain(Vector3 worldPosition)
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            if (TerrainMethods.IsOnTerrain(worldPosition, terrain))
            {
                return terrain;
            }
        }

        return null;
    }

    private void AddTreeInstance(Vector3 worldPosition)
    {
        
        Vector3 treePositionOnTerrain = GetTreePositionOnTerrain(worldPosition);
        TreeInstance treeInstance = CreateTreeInstance(treePositionOnTerrain);

        activeTerrain.AddTreeInstance(treeInstance);
        
    }

    private Vector3 GetTreePositionOnTerrain(Vector3 worldPosition)
    {
        Vector3 treePositionTerrain = TerrainMethods.WorldToTerrainPosition(worldPosition, activeTerrain);
        int[] terrainCoord = TerrainMethods.WorldToTerrainCoordinates(worldPosition, activeTerrain);
        treePositionTerrain.y = activeTerrain.terrainData.GetHeights(terrainCoord[0], terrainCoord[1], 1, 1)[0, 0];
        return treePositionTerrain;
    }

    private TreeInstance CreateTreeInstance(Vector3 positionOnTerrain)
    {
        treeInstance = new TreeInstance();
        treeInstance.position = positionOnTerrain;
        treeInstance.prototypeIndex = prototypeNumber;
        float treeScalingCoefficient = Random.Range(.5f, 1);
        treeInstance.widthScale = treeScalingCoefficient;
        treeInstance.heightScale = treeScalingCoefficient;
        treeInstance.color = Color.white;
        treeInstance.lightmapColor = Color.white;

        return treeInstance;
    }

    private void DeleteTerrainTrees()
    {
        activeTerrain = FindActiveTerrain(hit.point);
        TreeInstance[] treeInstanceCollection = new TreeInstance[0] { };
        activeTerrain.terrainData.SetTreeInstances(treeInstanceCollection, false);
    }

    private void CreateTreesAroundSpot()
    {
        float maxPositiveDistanceFromSpot = 60f;
        float maxNegativeDistanceFromSpot = -60f;
        float DistanceFromSpotStep = 30f;
        
        for (float xPositionShift = maxNegativeDistanceFromSpot; xPositionShift < maxPositiveDistanceFromSpot; xPositionShift += DistanceFromSpotStep)
        {
            for (float zPositionShift = maxNegativeDistanceFromSpot; zPositionShift < maxPositiveDistanceFromSpot; zPositionShift += DistanceFromSpotStep)
            {
                AddTreeAtRandom(xPositionShift, zPositionShift);
                
            }     
        }
    }

    private void AddTreeAtRandom(float xPositionShift, float zPositionShift)
    {
        if (Random.value > 0.5f)
        {
            Vector3 shiftedTreePosition = new Vector3(hit.point.x + xPositionShift + Random.Range(0f, 20f), 0, hit.point.z + zPositionShift + Random.Range(0f, 20f));
            activeTerrain = FindActiveTerrain(shiftedTreePosition);
            GetRandomPrototypeNumber();
            AddTreeInstance(shiftedTreePosition);
        }
    }

    private void FillPrototypeNumberList()
    {
        prototypeNumberList = Enumerable.Range(0, treePrototypeCollection.Length).ToList();
    }

    private void GetRandomPrototypeNumber()
    {
        CheckPrototypeNumberListForEmpty();
        int randomIndex = Random.Range(0, prototypeNumberList.Count);
        prototypeNumber = prototypeNumberList[randomIndex];
        prototypeNumberList.RemoveAt(randomIndex);
    }

    private void CheckPrototypeNumberListForEmpty()
    {
        if (prototypeNumberList.Count == 0)
        {
            FillPrototypeNumberList();
        }
    }


}
