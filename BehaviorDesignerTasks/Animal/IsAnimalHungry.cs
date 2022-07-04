using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class IsAnimalHungry : Conditional
{

    public SharedGameObject targetGameObject;
    private float energyRemaining;
    public SharedFloat energyLimit;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        energyRemaining = currentGameObject.GetComponent<AnimalInfo>().energy;
        if (energyRemaining < energyLimit.Value)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        
    }
}
