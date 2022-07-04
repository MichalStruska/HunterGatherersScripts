using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class EatGrass : Action
{

    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        currentGameObject.GetComponent<AnimalInfo>().energy++;
        return TaskStatus.Success;
    }
}
