using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetAnimalSpeed : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public float targetSpeed;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<AnimalGaitInfo>().speed = targetSpeed;
        SetGait();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

    public void SetGait()
    {
        if (targetSpeed > 30)
        {
            currentGameObject.GetComponent<AnimalGaitInfo>().gait = AnimalGaitList.Galloping;
        }
        else
        {
            currentGameObject.GetComponent<AnimalGaitInfo>().gait = AnimalGaitList.Walking;
        }
    }

}
