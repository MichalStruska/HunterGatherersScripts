using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class AddTuberItem : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<HumanInfo>().AddTuber();
        //currentGameObject.GetComponent<HumanObjectInfo>().RefreshInventory();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
