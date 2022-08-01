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
    public GameObject Player;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        
        //agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        //hutPosition = GetComponent<MovementManager>().originalHitPoint;
        hutPosition = TargetHut.Value.GetComponent<Collider>().bounds.center;
        DeselectCurrentHumanIfActive();
        currentGameObject.transform.position = new Vector3(hutPosition.x, hutPosition.y, hutPosition.z); //+ TargetHut.Value.GetComponent<HutManager>().AddHuman();
        currentGameObject.transform.localScale = new Vector3(0, 0, 0);
        TargetHut.Value.GetComponent<HutManager>().AddHuman(currentGameObject);
        currentGameObject.GetComponent<HumanInfo>().isInHut = true;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

    public void DeselectCurrentHumanIfActive()
    {
        if (IsCurrentHumanActive())
        {
            Player.GetComponent<InputManager>().DeselectHuman(currentGameObject);
        }
    }

    private bool IsCurrentHumanActive()
    {
        return currentGameObject.GetComponent<HumanInfo>().isSelected;
    }

}
