using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;


public class IsAnimalPathCrossingHumans : Conditional
{

    public SharedGameObject targetGameObject;
    public GameObject currentGameObject;
    public SharedVector3 shadowTargetPoint;
    public GameObject Player;
    public SharedBool targetBool;
    public SharedBool crossingHumans;
    public float humanDangerZone;
    private Ray ray;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
    }

    public override TaskStatus OnUpdate()
    {
        
        IsPathCrossingHumans();
        if (!IsPathCrossingHumans())
        {
            crossingHumans = false;
            return TaskStatus.Success;
        }
        crossingHumans = true;
        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        
    }

    public bool IsPathCrossingHumans()
    {
        
        Vector3 animalPosition = currentGameObject.transform.position;
        Vector3 direction = (shadowTargetPoint.Value - currentGameObject.transform.position);

        ray = new Ray(animalPosition, direction);

        //Debug.DrawRay(animalPosition, new Vector3(direction.x, direction.y, direction.z), Color.yellow, 0.2f);

        foreach (GameObject human in Player.GetComponent<InputManager>().units)
        {
            if (IsThereDanger(human.transform.position))
            {
                return true;
            }
        }
        
        foreach (Vector3 memoryPosition in currentGameObject.GetComponent<AnimalMemory>().humanPosition)
        {
            if (IsThereDanger(memoryPosition))
            {
                return true;
            }
        }

        return false; 
    }

    public bool IsThereDanger(Vector3 position)
    {
       
        float distance = Vector3.Cross(ray.direction, position - ray.origin).magnitude;
        if (distance < humanDangerZone)
        {
            return true;
        }

        return false;
    }
}
