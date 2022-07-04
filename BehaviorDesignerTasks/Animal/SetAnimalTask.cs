using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetAnimalTask : Action
{

    public SharedGameObject targetGameObject;
    public AnimalTaskList animalTargetTask;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<AnimalInfo>().animalTask = animalTargetTask;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
