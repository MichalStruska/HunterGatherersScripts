using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DigitalRuby.WeatherMaker;


public class InventoryPanelController : MonoBehaviour
{
    
    public GameObject InventoryPanel;
    public Text TuberCounterText;
    public int tuberCounterInt;
    public Slider ItemSlider;

    void Start()
    {
        tuberCounterInt = 0;
    }

    void Update()
    {

    }


    public void AddTuber()
    {

    }

    public void SetHumanInventory(HumanInfo HumanInfo)
    {
        TuberCounterText.text = HumanInfo.tuberNumber + "/" + HumanInfo.tuberNumberMax;
        ItemSlider.value = HumanInfo.progressBarValue;
    }

}
