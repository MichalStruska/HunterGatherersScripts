using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class MemoryPosition
{
    //[SerializeField] public memoryPosition targetPos;
    public Vector3 position;
    public bool isRemembered;
    public int rememberCounter;
}

public class AnimalMemory : MonoBehaviour
{
    public Vector3[] humanPosition;
    public List<GameObject> humans;
    public GameObject[] trees;
    public int humanPositionMemorySize = 10;
    public int humanPositionCounter;
    //public (Vector3 X, bool Y)[] waterPositions;
    [SerializeField] public List<MemoryPosition> waterPositions;
    [SerializeField] public List<MemoryPosition> grassPositions;
    [SerializeField] public List<MemoryPosition> treePositions;
    public string jsonString;
    public GameObject Player;

    void Start()
    {
        Player = GameObject.Find("Player");
        humanPositionCounter = 0;
        humanPosition = new Vector3[10];
        GetWaterPositions();
        GetGrassPositions();
        GetTreePositions();
        //trees = GameObject.FindGameObjectsWithTag("Tree");
    }

    void Update()
    {
        humans = Player.GetComponent<InputManager>().units;
        foreach (Vector3 hum in humanPosition)
        {
        }
    }

    public void AddHumanPosition(Vector3 newLocation)
    {
        humanPosition[humanPositionCounter] = newLocation;
        if (humanPositionCounter == humanPositionMemorySize - 1)
        {
            humanPositionCounter = 0;
        }
        else
        {
            humanPositionCounter++;
        }
    }

    public void GetWaterPositions()
    {
        jsonString = System.IO.File.ReadAllText("C:/Users/strus/HunterGatherers_base/Data/WaterPosition.json");
        SerializePositions LoadedMemory = JsonUtility.FromJson<SerializePositions>(jsonString);
        FillMemory(waterPositions, LoadedMemory.positions);
    }
    
    public void GetGrassPositions()
    {
        jsonString = System.IO.File.ReadAllText("C:/Users/strus/HunterGatherers_base/Data/GrassPosition.json");
        SerializePositions LoadedMemory = JsonUtility.FromJson<SerializePositions>(jsonString);
        FillMemory(grassPositions, LoadedMemory.positions);
    }
    
    public void GetTreePositions()
    {
        jsonString = System.IO.File.ReadAllText("C:/Users/strus/HunterGatherers_base/Data/TreePosition.json");
        SerializePositions LoadedMemory = JsonUtility.FromJson<SerializePositions>(jsonString);
        FillMemory(treePositions, LoadedMemory.positions);
    }

    public void FillMemory(List<MemoryPosition> partialMemory, List<Vector3> MemoryPositionsSource)
    {
        foreach (Vector3 posi in MemoryPositionsSource)
        {
            //partialMemory.Add(new MemoryPositionWrapper() { targetPos = new memoryPosition() { position = posi, isRemembered = true, rememberCounter = 0 } });
            partialMemory.Add(new MemoryPosition() { position = posi, isRemembered = true, rememberCounter = 0  });
        }
    }


}
