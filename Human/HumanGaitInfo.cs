using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanGaitInfo : OrganismGaitInfoBase
{
    public HumanGaitList gait;
    public Animator Animator;

    void Start()
    {
        gait = HumanGaitList.Walking;
        speed = 15;
        Animator = GetComponent<Animator>();
    }

    void Update()
    {
        
    }

    public void SetAnimationSpeed(float speedSliderMaxValue)
    {
        GaitSpeedAnimationSpeed[] potentialAnimationSpeeds = GetPotentialAnimationSpeeds();

        foreach (GaitSpeedAnimationSpeed speedCouple in potentialAnimationSpeeds)
        {
            if (speed > speedCouple.boundarySpeed)
            {
                Animator.SetFloat("gaitMultiplier", speedCouple.animationSpeed);
                Debug.Log("animation speed set to " + speedCouple.animationSpeed + " " + speed);
            }
        }
    }

    public GaitSpeedAnimationSpeed[] GetPotentialAnimationSpeeds()
    {
        if (IsRunning())
        {
            return HumanAnimationSpeeds.runningAnimationSpeed;
        }
        else
        {
            return HumanAnimationSpeeds.walkingAnimationSpeed;
        }
    }

    private bool IsRunning()
    {
        return gait == HumanGaitList.Running;
    }

}
