using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class AlwaysSuccess : Action
{

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {       
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

}
