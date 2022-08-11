using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class PanelValuesManager : MonoBehaviour
{
    public Text NameDisplay;
    public Text AgeDisplay;
    public Text BodyMassDisplay;
    public Text BodyHeightDisplay;
    public Text SexDisplay;

    public Image humanThumbnailImage;

    public Slider HealthBar;
    public Text HealthDisplay;
    public Slider WaterBar;
    public Text WaterDisplay;
    public Slider EnergyBar;
    public Text EnergyDisplay;
    public Slider TemperatureBar;
    public Text TemperatureDisplay;

    public Slider SpeedSlider;
    public Text SpeedDisplay;
    public TMP_Dropdown GaitDropdown;

    private HumanInfo HumanInfo;
    private HumanGaitInfo humanGaitInfo;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SetAllHumanInformation(GameObject human)
    {
        HumanInfo humanInfo = human.GetComponent<HumanInfo>();
        SetBodyFunctions(human.GetComponent<HumanInfo>());
        SetHumanInformation(human.GetComponent<HumanInfo>());
        SetHumanThumbnail(human);

        if (humanInfo.humanTask == HumanTaskList.Walking || humanInfo.humanTask == HumanTaskList.Running)
        {
            Debug.Log("nastav rychlost");
            SetSpeed(human);
        }
    }

    public void SetBodyFunctions(HumanInfo HumanInfo)
    {
        HealthBar.maxValue = HumanInfo.health;
        HealthBar.value = HumanInfo.health;
        HealthDisplay.text = "HP: " + HumanInfo.health;
        EnergyBar.maxValue = HumanInfo.energyMax;
        EnergyBar.value = HumanInfo.energy;
        EnergyDisplay.text = "EP: " + HumanInfo.energy;
        WaterBar.maxValue = HumanInfo.waterMax;
        WaterBar.value = HumanInfo.water;
        WaterDisplay.text = "W: " + HumanInfo.water;
        TemperatureBar.maxValue = HumanInfo.coreTemperatureMax;
        TemperatureBar.minValue = HumanInfo.coreTemperatureMin;
        TemperatureBar.value = HumanInfo.coreTemperature;
        TemperatureDisplay.text = "T: " + HumanInfo.coreTemperature;

    }

    public void SetSpeed(GameObject human)
    {
        HumanGaitInfo HumanGaitInfo = human.GetComponent<HumanGaitInfo>();
        HumanInfo HumanInfo = human.GetComponent<HumanInfo>();
        switch (HumanInfo.humanTask)
        {
            case HumanTaskList.Running:
                /*SpeedSlider.maxValue = HumanInfo.maxRunSpeed;
                SpeedSlider.minValue = HumanInfo.minRunSpeed;*/
                SpeedSlider.value = HumanGaitInfo.speed;
                GaitDropdown.value = HumanInfo.gait;
                break;

            case HumanTaskList.Walking:
                /*SpeedSlider.maxValue = HumanInfo.maxWalkSpeed;
                SpeedSlider.minValue = HumanInfo.minWalkSpeed;*/
                SpeedSlider.value = HumanGaitInfo.speed;
                GaitDropdown.value = HumanInfo.gait;
                break;

            default:
                SpeedSlider.value = HumanInfo.minWalkSpeed;
                GaitDropdown.value = 0;
                break;
        }

        SpeedSlider.value = HumanGaitInfo.speed;
        GaitDropdown.value = (int)HumanGaitInfo.gait;
        SpeedDisplay.text = System.Math.Round(SpeedSlider.value, 2).ToString();
    }

    public void SetSpeedText()
    {
        SpeedDisplay.text = Math.Round(SpeedSlider.value, 2).ToString();
    }

    public void SetHumanInformation(HumanInfo HumanInfo)
    {
        NameDisplay.text = HumanInfo.objectName;

        AgeDisplay.text = HumanInfo.age.ToString();
        BodyMassDisplay.text = HumanInfo.mass.ToString();
        BodyHeightDisplay.text = HumanInfo.stature.ToString();
        SexDisplay.text = Enum.GetName(typeof(HumanSexList), HumanInfo.sex);
    }

    public void SetHumanThumbnail(GameObject human)
    {
        humanThumbnailImage.sprite = human.GetComponent<HumanGUIHolder>().HumanThumbnailGeneral.sprite;
    }
}
