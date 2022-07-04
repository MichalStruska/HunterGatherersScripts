using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Collections.Generic;

public class GetPhysiologicalInfo : Action
{
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;
    public SharedFloat waterState;
    public SharedFloat temperatureState;
    public SharedFloat energyState;
    

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
    }

    public override TaskStatus OnUpdate()
    {
        GetWater();
        GetEnergy();
        GetTemperature();

        Debug.Log("thirst " + waterState.Value);
        Debug.Log("hunger " + energyState.Value);
        Debug.Log("temperature " + temperatureState.Value);
        
        return TaskStatus.Success;
    }

    public void GetWater()
    {
        float waterMax = currentGameObject.GetComponent<AnimalInfo>().waterMax;
        float water = currentGameObject.GetComponent<AnimalInfo>().water;
        float waterLimit = currentGameObject.GetComponent<AnimalInfo>().waterMin;
        waterState.Value = (waterMax - water) / (waterMax - waterLimit);
    }
    
    public void GetEnergy()
    {
        float energy = currentGameObject.GetComponent<AnimalInfo>().energy;
        float energyMax = currentGameObject.GetComponent<AnimalInfo>().energyMax;
        float energyMin = currentGameObject.GetComponent<AnimalInfo>().energyMin;
        energyState.Value = (energyMax - energy) / (energyMax - energyMin);
    }
    
    public void GetTemperature()
    {
        float coreTemperature = currentGameObject.GetComponent<AnimalInfo>().coreTemperature;
        float coreTemperatureMax = currentGameObject.GetComponent<AnimalInfo>().coreTemperatureMax;
        float coreTemperatureMin = currentGameObject.GetComponent<AnimalInfo>().coreTemperatureMin;
        float initialCoreTemperature = currentGameObject.GetComponent<AnimalInfo>().coreTemperatureMin;
        temperatureState.Value = (coreTemperature - initialCoreTemperature) / (coreTemperatureMax - initialCoreTemperature);
    }

    public override void OnReset()
    {
        //foundGrass.Value = false;
    }


}
