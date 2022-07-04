using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;


public class GetHumanAnimationString : Action
{

    public SharedGameObject targetGameObject;
    public SharedString animationString;
    private NavMeshAgent agent;
    public HumanTaskList humanTask;
    private GameObject currentGameObject;
    private (HumanTaskList X, string Y)[] taskAnimationPairs = new (HumanTaskList X, string Y)[] { (HumanTaskList.Eating, "isEating"), (HumanTaskList.Walking, "isWalking"),
        (HumanTaskList.Resting, "isResting"), (HumanTaskList.Running, "isRunning"), (HumanTaskList.Sitting, "isSitting"), (HumanTaskList.Digging, "isDigging"),
        (HumanTaskList.Sleeping, "isLying"), (HumanTaskList.Dead, "isLying"), (HumanTaskList.Idle, "isIdle")};

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        agent = currentGameObject.GetComponent<NavMeshAgent>();
    
    }

    public override TaskStatus OnUpdate()
    {
        humanTask = currentGameObject.GetComponent<HumanInfo>().humanTask;
        FindRelevantAnimation();
        //switch (humanTask)
        //{
        //    case HumanTaskList.Eating:
        //        animationString.Value = "isEating";
        //        break;
        //    case HumanTaskList.Walking:
        //        animationString.Value = "isWalking";
        //        break;
        //    case HumanTaskList.Resting:
        //        animationString.Value = "isResting";
        //        break;
        //    case HumanTaskList.Running:
        //        animationString.Value = "isRunning";
        //        break;
        //    case HumanTaskList.Sitting:
        //        animationString.Value = "isSitting";
        //        break;
        //    case HumanTaskList.Digging:
        //        animationString.Value = "isDigging";
        //        break;
        //    case HumanTaskList.Sleeping:
        //        animationString.Value = "isLying";
        //        break;
        //    default:
        //        animationString.Value = "isIdle";
        //        break;
        //}


        return TaskStatus.Success;
    }

    private void FindRelevantAnimation()
    {
        
        foreach ((HumanTaskList X, string Y) taskAnimationPair in taskAnimationPairs)
        {
            Debug.Log("task je " + humanTask + " " + taskAnimationPair.X + " " + IsRelevantAnimation(taskAnimationPair.X));
            if (IsRelevantAnimation(taskAnimationPair.X))
            {
                SetAnimmation(taskAnimationPair.Y);
                return;
            }
        }
        SetAnimmation("isIdle");
    }

    private bool IsRelevantAnimation(HumanTaskList potentialTask)
    {
        return potentialTask == humanTask;
    }

    private void SetAnimmation(string targetAnimationString)
    {
        animationString.Value = targetAnimationString;
    }

    public override void OnReset()
    {
        
    }

}
