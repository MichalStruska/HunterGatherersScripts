using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class CheckHumanTask : Conditional
{

    public SharedGameObject targetGameObject;
    public HumanTaskList humanTargetTask;
    private GameObject currentGameObject;
    public bool targetBool;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        return CheckTask() == targetBool ? TaskStatus.Success : TaskStatus.Failure;
    }

    public override void OnReset()
    {
        
    }

    private bool CheckTask()
    {
        return currentGameObject.GetComponent<HumanInfo>().humanTask == humanTargetTask ? true : false;
    }
}
