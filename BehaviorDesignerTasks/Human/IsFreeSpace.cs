using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class IsFreeSpace : Conditional
{

    public SharedGameObject TargetObjectPassive;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        if (TargetObjectPassive.Value.GetComponent<HutManager>().IsFreeSpace())
        {
            return TaskStatus.Success;
        }
        return TaskStatus.Failure;
    }

    public override void OnReset()
    {
        
    }
}
