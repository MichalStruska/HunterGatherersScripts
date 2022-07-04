using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class StopThere : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public NavMeshAgent agent;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        //currentGameObject.GetComponent<MovementManager>().StopThere();
        Debug.Log("move1move1");
        agent.isStopped = true;
        agent.isStopped = false;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
