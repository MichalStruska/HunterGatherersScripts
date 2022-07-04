using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;
using System.Collections.Generic;

public class SetTargetResourceToGrass : Action
{
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;    
    public SharedListMemoryPosition resourcePositions;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(currentGameObject);
        
    }

    public override TaskStatus OnUpdate()
    {
        resourcePositions.Value = currentGameObject.GetComponent<AnimalMemory>().grassPositions;
        return TaskStatus.Success;
    }


}
