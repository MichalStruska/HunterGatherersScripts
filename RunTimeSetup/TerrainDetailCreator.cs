using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TerrainDetailCreator : MonoBehaviour
{
    public Texture2D[] prototypeTextures;
    private DetailPrototype[] detailPrototypeCollection;
    private Terrain activeTerrain;
    private Texture2D activePrototypeTexture;
    private DetailPrototype prototype;


    void Start()
    {
        //detailPrototypeCollection = new DetailPrototype[prototypeTextures.Length];
        //AddDetailPrototypeToTerrains();
    }
    
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.I))
        {
            GetPerpendicularCamera();
        }

    }


    private void AddDetailPrototypeToTerrains()
    {
        foreach (Terrain terrain in Terrain.activeTerrains)
        {
            activeTerrain = terrain;
            CreatePrototypeForEachTexture();
            terrain.terrainData.detailPrototypes = detailPrototypeCollection;
        }
    }

    private void CreatePrototypeForEachTexture()
    {
        for (int i = 0; i < prototypeTextures.Length; i++)
        {
            activePrototypeTexture = prototypeTextures[i];
            CreatePrototype();
            detailPrototypeCollection[i] = prototype;
        }
        
    }

    private void CreatePrototype()
    {
        prototype = new DetailPrototype();
        SetPrototypeAttributes();
        
    }

    private void SetPrototypeAttributes()
    {
        prototype.prototype = new GameObject();
        prototype.prototypeTexture = activePrototypeTexture;
        //prototype.usePrototypeMesh = true;
        prototype.renderMode = DetailRenderMode.GrassBillboard;
        prototype.minWidth = 4;
        prototype.maxWidth = 8;
        prototype.minHeight = 10;
        prototype.maxHeight = 20;
        prototype.noiseSpread = 0.4f;
        prototype.healthyColor = new Color32(166, 154, 4, 255);
        prototype.dryColor = new Color32(205, 188, 26, 255);
        prototype.bendFactor = 0.1f;
    }

    public void GetPerpendicularCamera()
    {
        Camera.main.transform.rotation = Quaternion.LookRotation(Vector3.down, Vector3.right);
        Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, 1000, Camera.main.transform.position.z);
    }


}
