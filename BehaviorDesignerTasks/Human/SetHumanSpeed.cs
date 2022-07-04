using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetHumanSpeed : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public float targetSpeed;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<HumanInfo>().speed = targetSpeed;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
