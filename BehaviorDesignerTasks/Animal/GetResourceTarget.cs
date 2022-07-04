using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Collections.Generic;

public class GetResourceTarget : Action
{
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;
    
    public SharedVector3 resourceTarget;
    public SharedMemoryPosition resourceTargetMemoryPosition;
    public Vector3 animalPosition;

    public SharedListMemoryPosition resourcePositions;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        GetClosestResource();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
    }

    public void GetClosestResource()
    {
        resourceTarget.Value = resourcePositions.Value[0].position;
        float shortestDistance = Mathf.Infinity;
        foreach (MemoryPosition position in resourcePositions.Value)
        {
            float resourceDistance = Vector3.Distance(position.position, animalPosition);
            if (resourceDistance < shortestDistance && position.isRemembered == true)
            {
                shortestDistance = resourceDistance;
                resourceTarget.Value = position.position;
                resourceTargetMemoryPosition.Value = position;
            }
        }
    }


}
