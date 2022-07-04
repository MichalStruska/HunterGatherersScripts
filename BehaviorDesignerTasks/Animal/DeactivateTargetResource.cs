using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Collections.Generic;

public class DeactivateTargetResource : Action
{
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;    
    public SharedMemoryPosition resourceTargetMemoryPosition;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
    }

    public override TaskStatus OnUpdate()
    {
        if (!CheckForNullObject())
        {
            resourceTargetMemoryPosition.Value.isRemembered = false;
        }
        return TaskStatus.Success;
    }

    public bool CheckForNullObject()
    {
        if (targetGameObject.Value == null)
        {
            return true;
        }
        return false;
    }

}
