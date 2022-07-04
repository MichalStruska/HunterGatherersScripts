using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;
using System;

public abstract class OrganismThermoModelBase : MonoBehaviour
{
    public float skinThickness;
    public float skinDensity;
    public float conductanceTissue;
    public float heatCapacityBlood;
    public float skinBloodFlowMin;
    public float skinBloodFlowMax;
    public float heatCapacityBody;
    public float wettednessMin;
    public float reflectance;
    public float heatVapWater;
    public float fractionHair;
    public float hairEvaporEffic;
    public float conductanceHair;
    public float conductanceSkin;
    public float conductanceSkinNoDelay;
    public float conductanceSkinChange;
    public float timeLag;
    public float clo;
    public float initialCoreTemperature;
    public float timeDelay;

    public float speed;
    public float velocityAir;
    public bool speedChange;

    public float mass;
    public float stature;
    public float metabolicHeat;
    public float solarFraction;
    public float area;

    public int lastChange;

    public float skinTemperature;
    public float evaporationResp;
    public float convectionResp;

    public float massSkin;
    public float massCore;
    public float coreTemperature;

    public float skinBloodFlowReq;
    public float skinBloodFlow;
    public float conductanceBlood;
    public float heatFlow;
    public float sweatLoss;
    public float heatGainDry;
    public float evaporationSkin;
    public float waterLoss;

    public GameObject Player;
    public EnvironmentInfo environmentInfo;

    protected virtual void Start()
    {
        GetMass();
        GetStature();
        GetArea();
        coreTemperature = initialCoreTemperature;
        GetInitialSkinTemperature();
    }
    
    void Model()
    {
        environmentInfo = Player.GetComponent<EnvironmentInfo>();
        environmentInfo.EnvironmentalCond();
        GetSpeed();
        
        MetabolicHeat();
        
        solarFraction = 4.8276f * (Mathf.Pow(10, -7)) * (Mathf.Pow(environmentInfo.beta, 3)) - ((8.1371f * Mathf.Pow(10, -5)) * (Mathf.Pow(environmentInfo.beta, 2))) + (9.6369f * Mathf.Pow(10, -4)) * environmentInfo.beta + 0.3036f;

        skinTemperature = 30 + 0.138f * environmentInfo.tempAir + 0.254f * environmentInfo.pressureAir - 0.571f * speed + 1.28f * (Mathf.Pow(10, -3)) * metabolicHeat - 0.553f * clo;
        evaporationResp = (0.0173f * metabolicHeat * (5.87f - environmentInfo.pressureAir));
        convectionResp = (0.0014f * metabolicHeat * (environmentInfo.tempAir - 34f));
        
        GetMassOfBodyLayers();
        SkinBloodFlow();
        Conductance();
        
        heatFlow = conductanceSkin * area * (coreTemperature - skinTemperature);
        CoreTemperatureChange();

        HeatGainDry();

        WaterLoss();

        SendTemperatureChange();

        SkinTemperatureChange();
        
        CheckBodyWater();
        CheckEnergy();
        CheckTemperature();

    }

    protected abstract void GetInitialSkinTemperature();

    protected abstract void GetArea();

    private void GetMass()
    {
        mass = GetComponent<OrganismInfoBase>().mass;
    }
    
    private void GetStature()
    {
        stature = GetComponent<OrganismInfoBase>().stature;
    }
    
    public abstract void MetabolicHeat();

    private void CoreTemperatureChange()
    {
        float coreTemperatureChange = (metabolicHeat + convectionResp - evaporationResp - heatFlow) / (heatCapacityBody * massCore);
        coreTemperature += coreTemperatureChange;
    }

    private void SkinTemperatureChange()
    {
        float skinTemperatureChange = (heatGainDry - evaporationResp - heatFlow) / (heatCapacityBody * massSkin);
        skinTemperature = skinTemperature + skinTemperatureChange;
    }

    void GetMassOfBodyLayers()
    {
        massSkin = area * skinThickness * skinDensity;
        massCore = mass - massSkin;
    }

    void SkinBloodFlow()
    {
        skinBloodFlowReq = ((metabolicHeat + convectionResp - evaporationResp) / (heatCapacityBlood * (coreTemperature - skinTemperature))) - (conductanceTissue * area) / heatCapacityBlood;
        Debug.Log("skbf req " + heatCapacityBlood + " " + coreTemperature + " " + skinTemperature);
        if (skinBloodFlowReq > skinBloodFlowMax)
        {
            skinBloodFlow = skinBloodFlowMax;
        }
        else if (skinBloodFlowReq < skinBloodFlowMin)
        {
            skinBloodFlow = 0;
        }
        else if (skinBloodFlow == skinBloodFlowReq)
        {
            // do nothing
        }
    }

    void Conductance()
    {
        conductanceBlood = (skinBloodFlow * heatCapacityBlood) / area;

        conductanceSkinNoDelay = conductanceTissue + conductanceBlood;

        timeLag = environmentInfo.dayTimeSeconds - lastChange;
        if (timeLag <= timeDelay && speedChange == true)
        {
            if (timeLag == 0)
            {
                conductanceSkinChange = conductanceSkin;
            }

            conductanceSkin = conductanceSkinChange + (conductanceSkinNoDelay - conductanceSkinChange) * (timeLag / timeDelay);
        }
        else
        {
            conductanceSkin = conductanceSkinNoDelay;
        };
    }

    void HeatGainDry()
    {
        float areaSkinSun = area * (fractionHair - 1) * solarFraction;
        float areaSkinShade = area * (fractionHair - 1) * (solarFraction - 1);
        float areaHairSun = area * fractionHair * solarFraction;
        float areaHairShade = area * fractionHair * (solarFraction - 1);
        float[] areasArray = new float[] { areaSkinSun, areaSkinShade, areaHairSun, areaHairShade };
        velocityAir = speed + environmentInfo.velocityAir;
        for (int j = 0; j < 4; j++)
        {
            heatGainDry += GetHeatGainDryPart(areasArray[j], j);
        }
    }

    private float GetHeatGainDryPart(float areaPart, int bodyPartIndex)
    {
        float skinTemperaturePart = skinTemperature;
        float tempSurface = skinTemperature;
        float reradiationPart = (float)(environmentInfo.sigma * Math.Pow(tempSurface, 4));

        float absorbedRadiationPart = GetRadiationHeatGain();

        float convectionCoefficient = GetComponent<OrganismInfoBase>().convectionCoefficient;
        float convectionSkin = (float)(convectionCoefficient * Math.Sqrt(velocityAir) * (environmentInfo.tempAir - tempSurface));
        if (bodyPartIndex == 2 || bodyPartIndex == 3)
        {
            float tempHair = ((convectionSkin - reradiationPart + absorbedRadiationPart) / conductanceHair) + skinTemperaturePart;
        }
        float heatGainDryPart = (absorbedRadiationPart - reradiationPart + convectionSkin) * areaPart;

        return heatGainDryPart;
    }

    private float GetRadiationHeatGain()
    {
        float surfaceAbsorptivity = GetComponent<OrganismInfoBase>().surfaceAbsorptivity;

        float solarIrradianceDirect = environmentInfo.solarIrradianceDirect;
        float solarIrradianceIndirect = environmentInfo.solarIrradianceIndirect;
        float radiationAthmosphere = environmentInfo.radiationAthmosphere;
        float radiationGround = environmentInfo.radiationGround;

        float absorbedRadiationPart = (surfaceAbsorptivity * solarIrradianceDirect + 0.5f * (surfaceAbsorptivity
            * solarIrradianceIndirect
            + surfaceAbsorptivity
            * reflectance
            * (solarIrradianceDirect + solarIrradianceIndirect)
            + radiationAthmosphere
            + radiationGround));
        return absorbedRadiationPart;
    }


    public virtual void WaterLoss()
    {
        float pressureSkin = (float)(0.61121f * (Math.Exp((18.678f - (skinTemperature / 234.5f)) * (skinTemperature / (257.14f + skinTemperature)))));
        float evaporationMax = (float)((pressureSkin - environmentInfo.pressureAir) * Math.Sqrt(velocityAir) * area * 124 * (fractionHair * (hairEvaporEffic - 1) + 1));
        float wettednessReq = (heatFlow + heatGainDry) / evaporationMax;

        //float wettednessReq = (metabolicHeat + convectionResp - evaporationResp + heatGainDry + mass * heatCapacityBody * (tempCore - tempCoreInit)) / evaporationMax; //(MetabolicRate - RespiratoryHeatLoss + HeatGain + BodyMass * HeatCapacity * (Tcore - RegulatedTcore)) / EvaporationMax
        float wettedness = GetWettedness();
        
        evaporationSkin = evaporationMax * wettedness;
        float sweatLoss = (evaporationSkin / (fractionHair * (hairEvaporEffic - 1) + 1)) / heatVapWater;

        waterLoss = (evaporationResp + evaporationSkin) / heatVapWater;
        GetComponent<OrganismInfoBase>().mass -= waterLoss;
    }

    public float GetWettedness()
    {
        float pressureSkin = (float)(0.61121f * (Math.Exp((18.678f - (skinTemperature / 234.5f)) * (skinTemperature / (257.14f + skinTemperature)))));
        float evaporationMax = (float)((pressureSkin - environmentInfo.pressureAir) * Math.Sqrt(velocityAir) * area * 124 * (fractionHair * (hairEvaporEffic - 1) + 1));
        float wettednessReq = (heatFlow + heatGainDry) / evaporationMax;
        float wettedness = Mathf.Min(0.94f * wettednessReq + wettednessMin, 1);

        return wettedness;
    }

    public virtual void SendTemperatureChange()
    {
        
    }

    public void SendPhysiologicalChange(float physiologicalVariable, float physiologicalChange)
    {
        physiologicalVariable += physiologicalChange;
    }

    public abstract void CheckBodyWater();
    public abstract void CheckEnergy();
    public abstract void CheckTemperature();

    public abstract void GetSpeed();
}

