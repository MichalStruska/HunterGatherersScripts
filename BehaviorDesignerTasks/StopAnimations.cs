using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

public class StopAnimations : Action
{

    public SharedGameObject targetGameObject;
    public Animator animator;
    private GameObject currentGameObject;
    public SharedString animationString;

    public override void OnAwake()
    {
        currentGameObject = GetDefaultGameObject(targetGameObject.Value);
        animator = currentGameObject.GetComponent<Animator>();
    }

    public override TaskStatus OnUpdate()
    {
        SetAnimationsFalse();
        
        return TaskStatus.Success;
    }

    public void SetAnimationsFalse()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animationString.Value && parameter.name != "runMultiplier")
            {
                animator.SetBool(parameter.name, false);
            }

        }

    }
}
