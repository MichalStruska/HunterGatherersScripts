using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class SetAlternativePath : Action
{

    public SharedGameObject targetGameObject;
    public SharedVector3 originalTargetPoint;
    public SharedVector3 newTargetPoint;
    private Vector3 animalPosition;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        animalPosition = currentGameObject.transform.position;
        //Debug.DrawRay(targetPoint.Value, new Vector3(targetPoint.Value.x + 5, targetPoint.Value.y + 5, targetPoint.Value.z + 5), Color.blue, 4f);
        GetAlternative();
        return TaskStatus.Success;
    }

    public override void OnReset()
    {

    }

    void GetAlternative()
    {
        float oldRelativeX = animalPosition.x - originalTargetPoint.Value.x;
        float newRelativeX = animalPosition.x - oldRelativeX;
        newTargetPoint.Value = new Vector3(newRelativeX, originalTargetPoint.Value.y, originalTargetPoint.Value.z);
    }

}
