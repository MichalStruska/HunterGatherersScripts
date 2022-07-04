using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class GoToGrass : Action
{

    public SharedGameObject targetGameObject;
    public SharedVector3 targetPoint;
    private float walkSpeed = 40f;
    private Vector3 animalPosition;
    public float closestGrass = 3000f;
    private NavMeshAgent agent;
    private float targetAccuracy = 5f;

    public override void OnAwake()
    {
        agent = targetGameObject.Value.GetComponent<NavMeshAgent>();
        Debug.Log("jsme v goto01 " + targetPoint.Value);
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = targetGameObject.Value.transform.position;
        Debug.Log("jsme v goto " + targetPoint.Value);

        Debug.DrawRay(targetPoint.Value, new Vector3(targetPoint.Value.x + 5, targetPoint.Value.y + 5, targetPoint.Value.z + 5), Color.blue, 4f);
        if (Vector3.Distance(animalPosition, targetPoint.Value) > targetAccuracy)
        {
            CheckTexture();
            return TaskStatus.Running;
        }
        targetGameObject.Value.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.Eating;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {

    }

    void CheckTexture()
    {
       
        closestGrass = Vector3.Distance(targetPoint.Value, animalPosition);
        Debug.Log("nejblizsi trava " + closestGrass + " " + targetPoint.Value);

        
        // sees grass and the path does not go through human area and the distance to the grass is lower than to already found grass
        // (or there is not already found grass)
        
        Debug.Log("bez pro travu");
        targetGameObject.Value.GetComponent<AnimalInfo>().animalTask = AnimalTaskList.GoingToGrass;
        Move(targetPoint.Value, walkSpeed);
        closestGrass = Vector3.Distance(targetPoint.Value, animalPosition);

    }

    public void Move(Vector3 goToPos, float speed)
    {
        agent.destination = goToPos; //hit.point;
        agent.speed = speed;
    }


}
