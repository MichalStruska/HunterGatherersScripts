using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class Eat : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    private float energyValue = 300f;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        
        IncreaseEnergy();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }
    public void IncreaseEnergy()
    {
        currentGameObject.GetComponent<HumanInfo>().energy += energyValue;
    }


}
