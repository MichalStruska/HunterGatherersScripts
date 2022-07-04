using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetTargetObject : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public SharedGameObject targetObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        targetObject.Value = currentGameObject.GetComponent<MovementManager>().TargetObject;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
