using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class TeleportToHut : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public NavMeshAgent agent;
    public Vector3 hutPosition;
    public SharedGameObject TargetHut;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        
        //agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        hutPosition = GetComponent<MovementManager>().originalHitPoint;
        currentGameObject.transform.position = new Vector3(hutPosition.x, hutPosition.y, hutPosition.z) + TargetHut.Value.GetComponent<HutManager>().AddHuman();
        
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
