using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class LookForShadow : Action
{
    private GameObject sun;
    public LayerMask SeeLayer;
    public SharedGameObject targetGameObject;
    public SharedVector3 shadowTargetPoint;
    public SharedVector3 treePosition;
    public SharedBool foundShadow;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        sun = GameObject.Find("Sun");
        foundShadow.Value = false;

    }

    public override TaskStatus OnUpdate()
    {
        FindSpaceInShadow();
        if (foundShadow.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Failure;

    }

    public override void OnReset()
    {
    }

    private void FindSpaceInShadow()
    {
        RaycastHit hit;
        for (int i = 20; i < 60; i += 10)
        {
            Vector3 shiftedPosition = new Vector3(treePosition.Value.x, treePosition.Value.y + i, treePosition.Value.z);
            Vector3 higherTreePosition = new Vector3(treePosition.Value.x, treePosition.Value.y + 20, treePosition.Value.z);
            
            Debug.DrawRay(higherTreePosition, (shiftedPosition - sun.transform.position) * 1, Color.red, 60f);
            if (Physics.Raycast(higherTreePosition, (shiftedPosition - sun.transform.position), out hit, SeeLayer))
            {
                //Debug.Log("raycast " + hit.point);
                shadowTargetPoint.Value = hit.point;
                foundShadow.Value = true;
            }
        }

    }

}
