using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct GaitSpeedAnimationSpeed
{
    public float boundarySpeed;
    public float animationSpeed;
}

public static class HumanAnimationSpeeds
{
    public static GaitSpeedAnimationSpeed[] runningAnimationSpeed = new GaitSpeedAnimationSpeed[]
    {
        new GaitSpeedAnimationSpeed() { boundarySpeed = 0f, animationSpeed = 0.5f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 7f, animationSpeed = 0.8f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 10f, animationSpeed = 1.0f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 13f, animationSpeed = 1.2f }
    };
    
    public static GaitSpeedAnimationSpeed[] walkingAnimationSpeed = new GaitSpeedAnimationSpeed[]
    {
        new GaitSpeedAnimationSpeed() { boundarySpeed = 0f, animationSpeed = 0.5f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 5f, animationSpeed = 0.8f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 6f, animationSpeed = 1.0f },
        new GaitSpeedAnimationSpeed() { boundarySpeed = 8f, animationSpeed = 1.2f }
    };
}
