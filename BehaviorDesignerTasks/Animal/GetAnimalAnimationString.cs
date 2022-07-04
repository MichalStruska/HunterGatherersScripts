using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class GetAnimalAnimationString : Action
{

    public SharedGameObject targetGameObject;
    public SharedString animationString;
    private NavMeshAgent agent;
    public AnimalTaskList animalTask;
    public GameObject currentGameObject;
    public (AnimalTaskList X, string Y)[] taskAnimationPairs;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        //agent = currentGameObject.GetComponent<NavMeshAgent>();
    }

    public override TaskStatus OnUpdate()
    {
        animalTask = currentGameObject.GetComponent<AnimalInfo>().animalTask;
        FindAnimation();
        
        return TaskStatus.Success;
    }

    public void FindAnimation()
    {
        for (int i = 0; i < taskAnimationPairs.Length; i++)
        {
            if (animalTask == taskAnimationPairs[i].X)
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
