using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetTargetObjectCoordinates : Action
{

    public SharedGameObject targetGameObject;
    public SharedVector3 targetObjectCoordinates;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("koordinaty " + targetGameObject.Value.transform.position);
        targetObjectCoordinates.Value = targetGameObject.Value.transform.position;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
