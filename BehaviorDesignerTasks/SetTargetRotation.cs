using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class SetTargetRotation : Action
{

    public SharedGameObject targetGameObjectActive;
    public SharedGameObject targetGameObjectPassive;
    private GameObject currentGameObject;
    public float rotationSpeed = 4;
    public bool targetBool;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObjectActive.Value);
    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<MovementManager>().startRotating = targetBool;
        //currentGameObject.GetComponent<MovementManager>().SetRotation();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
