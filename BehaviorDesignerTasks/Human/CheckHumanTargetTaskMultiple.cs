using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class CheckHumanTargetTaskMultiple : Conditional
{

    public SharedGameObject targetGameObject;
    //public HumanTaskList humanTargetTask;
    public HumanTargetTaskList[] humanTargetTasks;
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
        foreach (HumanTargetTaskList task in humanTargetTasks)
        {
            if (currentGameObject.GetComponent<HumanInfo>().humanTargetTask == task)
            {
                return true;
            }
        }
        return false;
    }
}
