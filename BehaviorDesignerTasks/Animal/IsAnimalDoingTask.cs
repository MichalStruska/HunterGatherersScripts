using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class IsAnimalDoingTask : Conditional
{

    public SharedGameObject targetGameObject;
    public AnimalTaskList animalTargetTask;
    public bool targetBool;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        if (TestBool() == targetBool)
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        
    }

    public bool TestBool()
    {
        return currentGameObject.GetComponent<AnimalInfo>().animalTask == animalTargetTask;
    }
}
