using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetHumanGait : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public int targetGait;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<HumanInfo>().gait = targetGait;
       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
