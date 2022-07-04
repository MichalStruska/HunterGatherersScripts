using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;
using System;

public class HumanThermoModel : OrganismThermoModelBase
{
    public HumanInfo humanInfo;

    protected override void Start()
    {
        base.Start();
        //mass = GetComponent<HumanInfo>().mass;

        skinBloodFlowMax = 0.043f * area;
        Debug.Log("skbf " + skinBloodFlowMax);
        conductanceSkinNoDelay = conductanceTissue + conductanceBlood;
        conductanceSkinChange = conductanceSkinNoDelay;
        humanInfo = GetComponent<HumanInfo>();
        InvokeRepeating("Model", 0f, 1f);

        skinThickness = 0.002f;
        skinDensity = 1060f;
        conductanceTissue = 9f;
        heatCapacityBlood = 3850f;
        skinBloodFlowMin = 0f;
        heatCapacityBody = 3500f;
        wettednessMin = 0.06f;
        reflectance = 0.15f;
        
        heatVapWater = 2426000f;
        fractionHair = 0.15f;
        hairEvaporEffic = 0.8f;
        conductanceHair = 10f;

        clo = 0.1f;
        timeDelay = 1200f;
}

    void Update()
    {

    }

    protected override void GetArea()
    {
        area = 0.20247f * Mathf.Pow(mass, 0.425f) * Mathf.Pow((stature/100), 0.725f);
    }

    public override void WaterLoss()
    {
        base.WaterLoss();
        SendPhysiologicalChange(GetComponent<HumanInfo>().water, -waterLoss);
        humanInfo.SendHumanFunctions();
    }

    public override void CheckBodyWater()
    {
        if (GetComponent<HumanInfo>().water < GetComponent<HumanInfo>().waterMin)
        {
            humanInfo.Die();
        }
    }

    public override void CheckEnergy()
    {
        if (GetComponent<HumanInfo>().energy < GetComponent<HumanInfo>().energyMin)
        {
            humanInfo.Die();
        }
    }
    
    public override void CheckTemperature()
    {
        if (coreTemperature > GetComponent<HumanInfo>().coreTemperatureMax)
        {
            humanInfo.Die();
        }
    }

    public override void SendTemperatureChange()
    {
        GetComponent<HumanInfo>().coreTemperature = coreTemperature;
        GetComponent<HumanInfo>().SendHumanFunctions();
    }

    public override void MetabolicHeat()
    {
        HumanTaskList activeTask = GetComponent<HumanInfo>().humanTask;
        switch (activeTask)
        {
            case HumanTaskList.Walking:
                metabolicHeat = 0.02113f * speed * mass * (127.2f * (Mathf.Pow(speed, 2)) - 363.2f * speed + 421);
                break;
            case HumanTaskList.Running:
                metabolicHeat = (0.41f * (Mathf.Pow(speed, 2)) + 1.357f * speed + 5.331f) * mass;
                break;
            default:
                metabolicHeat = 2.177f + (100.055f / mass);
                break;
        }

        humanInfo.energy -= 1.25f * metabolicHeat * 0.00024f;
        humanInfo.SendHumanFunctions();
    }

    public override void GetSpeed()
    {
        speed = GetComponent<HumanInfo>().speed;
    }

    protected override void GetInitialSkinTemperature()
    {
        skinTemperature = 14.1f + 0.159f * environmentInfo.tempAir + 0.283f * environmentInfo.pressureAir - 0.6f * 0.1f - 0.497f *
                environmentInfo.velocityAir + 5.58f * metabolicHeat / ((float)Math.Pow(10, 5) * area) - 0.03f * environmentInfo.tempAir + 0.434f * coreTemperature;
        Debug.Log("init skin " + skinTemperature);
    }

}

