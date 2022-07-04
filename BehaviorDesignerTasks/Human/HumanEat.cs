using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class HumanEat : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    private float energyValue = 300f;
    private GameObject ActiveHut;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        ActiveHut = currentGameObject.GetComponent<MovementManager>().TargetObject;
        ActiveHut.GetComponent<HutManager>().RemoveTuber();
        IncreaseEnergy();

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }
    public void IncreaseEnergy()
    {
        currentGameObject.GetComponent<HumanInfo>().energy += energyValue;
    }


}
