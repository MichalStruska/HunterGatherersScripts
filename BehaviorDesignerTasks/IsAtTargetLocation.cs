using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class IsAtTargetLocation : Conditional
{

    public SharedGameObject targetGameObject;
    public SharedVector3 targetPoint;
    private Vector3 subjectPosition;
    public float targetAccuracy;
    private NavMeshAgent agent;
    private GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        subjectPosition = currentGameObject.transform.position;
        //if (Vector3.Distance(subjectPosition, targetPoint.Value) > targetAccuracy)
        //{
        //    return TaskStatus.Failure;
        //}
        if (Vector2.Distance(new Vector3(subjectPosition.x, subjectPosition.z), new Vector3(targetPoint.Value.x, targetPoint.Value.z)) > targetAccuracy)
        {
            return TaskStatus.Failure;
        }
        agent.speed = 0f;
        
        return TaskStatus.Success;

    }

    public override void OnReset()
    {

    }
}
