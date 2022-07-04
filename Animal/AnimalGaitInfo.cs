using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalGaitInfo : MonoBehaviour
{
    public AnimalGaitList gait;
    public float speed;

    void Start()
    {
        gait = AnimalGaitList.Walking;
        speed = 15;
    }

    void Update()
    {
        
    }
}
