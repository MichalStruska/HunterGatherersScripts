using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;
using System;
using UnityEngine.AI;

public class AnimalThermoModel : OrganismThermoModelBase
{
    public AnimalInfo animalInfo;
    private NavMeshAgent agent;
    private float horseCoeffA;
    private float horseCoeffB;
    private float horseCoeffC;

    private float ponyCoeffA;
    private float ponyCoeffB;
    private float ponyCoeffC;
    [SerializeField]
    private float initialSkinTemperature;

    protected override void Start()
    {
        base.Start();
        skinThickness = 0.002f;
        skinDensity = 1060f;
        conductanceTissue = 9f;
        heatCapacityBlood = 3850f;
        skinBloodFlowMin = 0f;
        heatCapacityBody = 3500f;
        wettednessMin = 0.06f;
        reflectance = 0.15f;
        heatVapWater = 2426000f;
        fractionHair = 1f;
        hairEvaporEffic = 0.8f;
        conductanceHair = 34.5f;
        animalInfo = GetComponent<AnimalInfo>();
        InvokeRepeating("Model", 0f, 1f);
        agent = GetComponent<NavMeshAgent>();

        clo = 0.1f;
        timeDelay = 1200f;
    }

    void Update()
    {

    }

    public override void WaterLoss()
    {
        base.WaterLoss();
        SendPhysiologicalChange(GetComponent<AnimalInfo>().water, -waterLoss);
    }

    protected override void GetArea()
    {
        area = 1.09f + 0.008f * mass;
    }

    void SendPhysiologicalChange(float physiologicalVariable, float physiologicalChange)
    {
        physiologicalVariable += physiologicalChange;
    }

    public override void CheckBodyWater()
    {
        if (GetComponent<AnimalInfo>().water < GetComponent<AnimalInfo>().waterMin)
        {
            animalInfo.Die();
        }
    }

    public override void CheckEnergy()
    {
        if (GetComponent<AnimalInfo>().energy < GetComponent<AnimalInfo>().energyMin)
        {
            animalInfo.Die();
        }
    }

    public override void CheckTemperature()
    {
        if (GetComponent<AnimalInfo>().coreTemperature > GetComponent<AnimalInfo>().coreTemperatureMax)
        {
            animalInfo.Die();
        }
    }

    public override void MetabolicHeat()
    {
        if (GetComponent<AnimalGaitInfo>().gait == AnimalGaitList.Standing)
        {
            metabolicHeat = (area * 1000 * 4184) / (24 * 60 * 60);
        }
        else
        {

            SetCoefficients();

            GetSpeed();
            float mass = GetComponent<AnimalInfo>().mass;

            float massInit = GetComponent<AnimalInfo>().massInit;
            float massPony = 140f;
            float massHorse = 515f;
            float massFraction = (massInit - massPony) / (massHorse - massPony);
            float costOfTransport = (ponyCoeffA + (horseCoeffA - ponyCoeffA) * massFraction) * Mathf.Pow(speed, 2f) + (ponyCoeffB + (horseCoeffB - ponyCoeffB) * massFraction) * speed + (ponyCoeffC + (horseCoeffC - ponyCoeffC) * massFraction);
            metabolicHeat = (costOfTransport * speed * mass) * 0.000239f;
        }

        SendPhysiologicalChange(GetComponent<AnimalInfo>().energy, -metabolicHeat);
    }

    private void SetCoefficients()
    {
        switch (GetComponent<AnimalGaitInfo>().gait)
        {
            case AnimalGaitList.Trotting:
                horseCoeffA = 0.312f;
                horseCoeffB = -2.373f;
                horseCoeffC = 6.767f;

                ponyCoeffA = 0.250f;
                ponyCoeffB = -1.803f;
                ponyCoeffC = 5.417f;
                break;
            case AnimalGaitList.Galloping:
                horseCoeffA = 0.234f;
                horseCoeffB = -3.121f;
                horseCoeffC = 12.846f;

                ponyCoeffA = 0.063f;
                ponyCoeffB = -0.787f;
                ponyCoeffC = 4.805f;
                break;
            // walking default
            default:
                horseCoeffA = 2.243f;
                horseCoeffB = -6.397f;
                horseCoeffC = 6.579f;

                ponyCoeffA = 2.866f;
                ponyCoeffB = -6.855f;
                ponyCoeffC = 6.405f;
                break;
        }
    }

    public override void GetSpeed()
    {
        speed = (agent.velocity.magnitude) / 3.6f;
    }

    protected override void GetInitialSkinTemperature()
    {
        skinTemperature = initialSkinTemperature;
    }
}

