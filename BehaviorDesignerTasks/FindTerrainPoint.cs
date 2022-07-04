using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class FindTerrainPoint : Action
{
    public SharedVector3 originalPoint;
    public SharedVector3 newPoint;
    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {

        FindPoint();
        return TaskStatus.Success;

    }

    private void FindPoint()
    {
        newPoint.Value = GetComponent<GetTerrainPoint>().GetNewPoint(originalPoint.Value);
       
    }

}
