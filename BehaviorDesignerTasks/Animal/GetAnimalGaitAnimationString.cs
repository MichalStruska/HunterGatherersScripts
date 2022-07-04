using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class GetAnimalGaitAnimationString : Action
{

    public SharedGameObject targetGameObject;
    public SharedString animationString;
    private NavMeshAgent agent;
    public AnimalGaitList animalGait;
    public GameObject currentGameObject;
    public (AnimalGaitList X, string Y)[] taskAnimationPairs;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        //agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        animalGait = currentGameObject.GetComponent<AnimalGaitInfo>().gait;
        FindAnimation();
        //switch (animalGait)
        //{
        //    case AnimalGaitList.Galloping:
        //        animationString.Value = "isGalloping";
        //        break;
        //    case AnimalGaitList.Walking:
        //        animationString.Value = "isWalking";
        //        break;
        //    case AnimalGaitList.Trotting:
        //        animationString.Value = "isTrotting";
        //        break;
        //    default:
        //        animationString.Value = "isIdle";
        //        break;
        //}

        return TaskStatus.Success;
    }
    
    public void FindAnimation()
    {
        for (int i = 0; i < taskAnimationPairs.Length; i++)
        {
            if (animalGait == taskAnimationPairs[i].X)
            {
                animationString.Value = taskAnimationPairs[i].Y;
                return;
            }
        }

        animationString.Value = "isIdle";
    }

    public override void OnReset()
    {
        
    }

}
