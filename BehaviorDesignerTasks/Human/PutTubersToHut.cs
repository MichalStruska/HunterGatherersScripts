using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class PutTubersToHut : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    private GameObject ActiveHut;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        int numberOfTubers = currentGameObject.GetComponent<HumanInfo>().tuberNumber;
        ActiveHut = currentGameObject.GetComponent<MovementManager>().TargetObject;

        if (IsSpaceForTubers())
        {
            ActiveHut.GetComponent<HutManager>().AddTubers(numberOfTubers);
        }
        currentGameObject.GetComponent<HumanInfo>().PutTubersToHut();

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

    public bool IsSpaceForTubers()
    {
        if (ActiveHut.GetComponent<HutManager>().tuberCounter < ActiveHut.GetComponent<HutManager>().maxTuberNumber)
        {
            return true;
        }
        return false;
    }

}
