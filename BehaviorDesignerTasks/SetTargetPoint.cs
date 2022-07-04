using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetTargetPoint : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public SharedVector3 targetPoint;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        targetPoint.Value = currentGameObject.GetComponent<MovementManager>().shiftedPoint;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
