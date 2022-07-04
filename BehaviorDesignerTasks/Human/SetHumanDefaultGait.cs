using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class SetHumanDefaultGait : Action
{

    public SharedGameObject targetGameObject;
    private GameObject currentGameObject;
    public Animator HumanAnimator;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);

    }

    public override TaskStatus OnUpdate()
    {
        HumanAnimator = currentGameObject.GetComponent<Animator>();
        HumanAnimator.speed = 1;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
