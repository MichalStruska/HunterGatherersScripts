using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class WorkOnTask : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public SharedGameObject targetTaskObject;
    public float time;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        targetTaskObject.Value.GetComponent<TaskItemManager>().WorkOnTask();
        SetProgressBarValue();
        if (targetTaskObject.Value.GetComponent<TaskItemManager>().taskCompleted == true)
        {
            return TaskStatus.Failure;
        }
        return TaskStatus.Success;
    }

    private void SetProgressBarValue()
    {
        currentGameObject.GetComponent<HumanInfo>().progressBarValue = (targetTaskObject.Value.GetComponent<TaskItemManager>().timer / targetTaskObject.Value.GetComponent<TaskItemManager>().timeLimit) * 100;
        currentGameObject.GetComponent<HumanInfo>().RefreshInventory();
    }

    public override void OnReset()
    {
        
    }

}
