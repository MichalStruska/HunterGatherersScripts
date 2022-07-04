using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetHumanTask : Action
{

    public SharedGameObject targetGameObject;
    public HumanTaskList humanTargetTask;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<HumanInfo>().humanTask = humanTargetTask;
        Debug.Log("lidsky task " + currentGameObject.GetComponent<HumanInfo>().humanTask);
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
