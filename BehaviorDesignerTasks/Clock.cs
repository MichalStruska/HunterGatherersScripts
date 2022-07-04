using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;
using UnityEngine.AI;

public class Clock : Action
{
    public SharedFloat timer;

    public override void OnAwake()
    {
    }

    public override TaskStatus OnUpdate()
    {
        timer.Value += Time.deltaTime;
        return TaskStatus.Success;
    }
}
