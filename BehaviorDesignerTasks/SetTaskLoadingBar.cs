using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class SetTaskLoadingBar : Action
{
    public SharedGameObject targetTaskObject;
    public GameObject selectionCanvas;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        FindTaskLoadingBar();
        return TaskStatus.Success;
    }

    public void FindTaskLoadingBar()
    {
        selectionCanvas.GetComponent<GUISelection>().SetupTaskLoadingBar(targetTaskObject.Value);
    }

    public override void OnReset()
    {
        
    }

}
