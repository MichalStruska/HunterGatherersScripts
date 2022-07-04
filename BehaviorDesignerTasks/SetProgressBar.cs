using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class SetProgressBar : Action
{

    private GameObject currentGameObject;
    public SharedFloat progressBarValue;
    public SharedGameObject targetGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        SetProgressBarValue();
        return TaskStatus.Success;
    }

    private void SetProgressBarValue()
    {
        currentGameObject.GetComponent<HumanInfo>().progressBarValue = progressBarValue.Value;
        currentGameObject.GetComponent<HumanInfo>().RefreshInventory();
    }

    public override void OnReset()
    {
        
    }

}
