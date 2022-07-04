using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetTimeMultiplier : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public SharedInt timeMultiplier;
    public SharedGameObject TargetItemObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        timeMultiplier.Value = TargetItemObject.Value.GetComponent<TaskItemManager>().taskTimeMultiplier;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
