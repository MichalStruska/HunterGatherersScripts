using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class GoToTarget : Action
{

    public SharedGameObject targetGameObject;
    public SharedVector3 targetPoint;
    public float moveSpeed = 30;
    private Vector3 animalPosition;
    private NavMeshAgent agent;
    public AnimalTaskList animalTargetTask;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        agent = currentGameObject.GetComponent<NavMeshAgent>();
        
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        moveSpeed = currentGameObject.GetComponent<AnimalGaitInfo>().speed;
        //Debug.DrawRay(targetPoint.Value, new Vector3(targetPoint.Value.x + 5, targetPoint.Value.y + 5, targetPoint.Value.z + 5), Color.blue, 4f);
        GoToClosest();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {

    }

    void GoToClosest()
    {
        currentGameObject.GetComponent<AnimalInfo>().animalTask = animalTargetTask;
        Debug.DrawLine(targetPoint.Value, new Vector3(targetPoint.Value.x, targetPoint.Value.y + 50, targetPoint.Value.z), Color.blue, 60f);
        Move(targetPoint.Value, moveSpeed);
    }

    public void Move(Vector3 goToPos, float speed)
    {
        agent.destination = goToPos; //hit.point;
        agent.speed = speed;
    }


}
