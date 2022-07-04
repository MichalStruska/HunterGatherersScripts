using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TerrainMethods
{
   
    public static Vector3 TerrainToWorld(Vector3 positionOnTerrain, Terrain terrain)
    {
        Vector3 terrainPosition = new Vector3();
        terrainPosition.x = positionOnTerrain.x * terrain.terrainData.size.x;
        terrainPosition.z = positionOnTerrain.z * terrain.terrainData.size.z;
        terrainPosition.y = terrain.terrainData.GetInterpolatedHeight(positionOnTerrain.x, positionOnTerrain.z);

        Vector3 positionOnWorld = terrainPosition + terrain.transform.position;

        return positionOnWorld;
    }
    
    public static int[] WorldToTerrainCoordinates(Vector3 positionOnWorld, Terrain terrain)
    {
        
        Vector3 mapPosition = WorldToTerrainPosition(positionOnWorld, terrain);

        float xCoord = mapPosition.x * terrain.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * terrain.terrainData.alphamapHeight;

        int newXCoord = (int)xCoord;
        int newZCoord = (int)zCoord;
        //Vector3 positionOnTerrain = new Vector3(newXCoord, 0, newXCoord);
        int[] positionOnTerrain = new int[] { newXCoord, newZCoord };

        return positionOnTerrain;
    }

    public static Vector3 WorldToTerrainPosition(Vector3 positionOnWorld, Terrain terrain)
    {
        Vector3 terrainPosition = positionOnWorld - terrain.transform.position;

        
        Vector3 mapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);
        return mapPosition;
    }

    public static bool IsOnTerrain(Vector3 position, Terrain terrain)
    {
        Vector3 terrainPosition = terrain.transform.position;
        if (position.x > terrainPosition.x && position.x < terrainPosition.x + terrain.terrainData.size.x &&
            position.z > terrainPosition.z && position.z < terrainPosition.z + terrain.terrainData.size.z)
        {
            return true;
        }
        else
        {
            return false;
        }

    }

}
