using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class GetRealSpeed : Action
{
    public SharedGameObject targetGameObject;
    private NavMeshAgent agent;
    public GameObject currentGameObject;
    public SharedFloat speed;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        speed.Value = currentGameObject.GetComponent<MovementInfo>().GetSpeed();
        return TaskStatus.Success;

    }

    

}
