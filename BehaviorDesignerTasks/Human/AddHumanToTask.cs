using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class AddHumanToTask : Action
{

    public SharedGameObject targetTaskObject;
    private GameObject currentGameObject;
    public SharedGameObject targetGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        targetTaskObject.Value.GetComponent<TaskItemManager>().AddHuman(currentGameObject);
        
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
