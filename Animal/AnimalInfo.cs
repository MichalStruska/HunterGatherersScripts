using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalInfo : OrganismInfoBase
{

    public AnimalTaskList animalTask;
    public AnimalGaitList animalGait;
    
    public bool isAlive = true;

    void Start()
    {
        mass = 120f;
        health = healthMax;
        energy = energyMax;
        water = waterMax;
        coreTemperature = 30f;
        surfaceAbsorptivity = 0.8f;
        convectionCoefficient = 8.3f;
        massInit = mass;
    }

    void Update()
    {
        
    }

    public override void Die()
    {
        //base.Die();
        animalTask = AnimalTaskList.Dead;
    }

}
