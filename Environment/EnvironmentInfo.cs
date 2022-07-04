using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.WeatherMaker;

public class EnvironmentInfo : MonoBehaviour
{
    public float cloudCover;
    public float beta;
    public float tempTwoMeters;
    public float tempGround;
    public float tempAir;
    public float relativeHumidity;
    public float relativeHumidityAvg;
    private float humidityRange = 22f;
    public float tempAvg;
    public float radiationObjects;
    public float solarIrradianceDirect;
    public float solarIrradianceIndirect;
    public float radiationAthmosphere;
    public float radiationGround;
    public float pressureAir;
    public float velocityAir = 5f;
    private float temperatureRange = 17f;
    public float timeTempMax = 14f;
    public float alpha = 0.41f;
    public float sigma = 5.67f * Mathf.Pow(10, -8f);
    public float tempDiff = 5f;

    public GameObject DayNight;
    public int dayTimeSeconds;
    public float dayTimeHours;

    void Start()
    {
    }

    void Update()
    {
        
    }

    public void EnvironmentalCond()
    {
        beta = GetComponent<EnvironmentManager>().sunElevation;
        dayTimeSeconds = (int)DayNight.GetComponent<WeatherMakerDayNightCycleManagerScript>().TimeOfDay;
        dayTimeHours = DayNight.GetComponent<WeatherMakerDayNightCycleManagerScript>().TimeOfDay / 3600.0f;

        tempTwoMeters = (temperatureRange / 2) * (float)Mathf.Sin((2 * Mathf.PI * (dayTimeHours - timeTempMax + 6f)) / 24f) + tempAvg - (temperatureRange / 2f);
        tempGround = (float)(((1.4 * temperatureRange) / 2f) * Mathf.Sin((2 * Mathf.PI * (dayTimeHours - timeTempMax + 7f)) / 24f) + tempAvg + tempDiff - (1.4f * temperatureRange) / 2f);
        tempAir = tempTwoMeters + alpha * (tempGround - tempTwoMeters);
        relativeHumidity = (humidityRange / 2) * (float)Mathf.Sin((2 * Mathf.PI * (dayTimeHours - timeTempMax - 6)) / 24) + relativeHumidityAvg;

        radiationObjects = sigma * Mathf.Pow((tempGround + 273), 4);
        solarIrradianceDirect = (float)(1121 * (Mathf.Cos(((beta) / 180) * Mathf.PI) - 0.08251f));
        solarIrradianceIndirect = solarIrradianceDirect / 9;
        radiationAthmosphere = 213 + 5.5f * tempTwoMeters;
        pressureAir = (float)(0.61121f * (Mathf.Exp((18.678f - tempAir / 234.5f) * tempAir / (257.14f + tempAir))) * (relativeHumidity / 100));
    }    

}
