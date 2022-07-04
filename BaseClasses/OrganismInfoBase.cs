using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OrganismInfoBase : MonoBehaviour
{
    public float water;
    public float waterMax;
    public float waterMin;

    public float coreTemperature;
    public float coreTemperatureMax;
    public float coreTemperatureMin;

    public float energy;
    public float energyMax;
    public float energyMin;

    public float health;
    public float healthMax;

    public float mass;
    public float massInit;
    public float stature;

    public float speed;
    public float minWalkSpeed;
    public float maxWalkSpeed;
    public float minRunSpeed;
    public float maxRunSpeed;

    public float surfaceAbsorptivity;
    public float convectionCoefficient;

    void Start()
    {
        energy = energyMax;
        health = healthMax;
        waterMax = 0.1f * mass;
        water = waterMax;
        mass = massInit;
        
    }

    public virtual void Die()
    {

    }
}
