using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class CheckHumanTaskMultiple : Conditional
{

    public SharedGameObject targetGameObject;
    //public HumanTaskList humanTargetTask;
    public HumanTaskList[] humanTargetTasks;
    private GameObject currentGameObject;
    public bool targetBool;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("soucasny task " + currentGameObject.GetComponent<HumanInfo>().humanTask);
        return CheckTask() == targetBool ? TaskStatus.Success : TaskStatus.Failure;
    }

    public override void OnReset()
    {
        
    }

    private bool CheckTask()
    {
        foreach (HumanTaskList task in humanTargetTasks)
        {
            if (currentGameObject.GetComponent<HumanInfo>().humanTask == task)
            {
                return true;
            }
        }
        return false;
    }
}
