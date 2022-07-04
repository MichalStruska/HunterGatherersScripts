using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class TurnOffTargetIndicator : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<MovementManager>().TurnOffTargetIndicator();
        currentGameObject.GetComponent<HumanInfo>().hasTarget = false;
        Debug.Log("turn indicator off");
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
