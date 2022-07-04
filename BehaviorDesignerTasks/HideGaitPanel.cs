using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class HideGaitPanel : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        Debug.Log("vidime info " + currentGameObject.GetComponent<HumanInfo>().isInfoShown);
        if (currentGameObject.GetComponent<HumanInfo>().isInfoShown)
        {
            currentGameObject.GetComponent<MovementManager>().HideGaitPanel();
        }

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
