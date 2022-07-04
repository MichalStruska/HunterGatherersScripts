using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Turn : Action
{

    public SharedGameObject targetGameObject;
    public SharedVector3 targetPoint;
    private NavMeshAgent agent;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        agent = currentGameObject.GetComponent<NavMeshAgent>();
        
    }

    public override TaskStatus OnUpdate()
    {
        float rotateSpeed = 5f;
        Vector3 direction = targetPoint.Value * rotateSpeed;
        currentGameObject.transform.Rotate(direction * Time.deltaTime);

        return TaskStatus.Success;
    }

    public override void OnReset()
    {

    }

}
