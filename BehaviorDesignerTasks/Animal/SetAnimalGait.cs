using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetAnimalGait : Action
{

    public SharedGameObject targetGameObject;
    public AnimalGaitList animalGait;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<AnimalGaitInfo>().gait = animalGait;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
