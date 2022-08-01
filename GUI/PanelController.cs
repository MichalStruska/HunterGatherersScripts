using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DigitalRuby.WeatherMaker;


public class PanelController : MonoBehaviour
{
    public List<GameObject> units;
    //public bool isPanelOn = false;
    public CanvasGroup HumanPickerPanel;
    public CanvasGroup HumanBodyFunctionsPanel;
    public CanvasGroup HumanInfoPanel;
    public CanvasGroup HumanGaitPanel;
    public CanvasGroup HumanInventoryPanel;
    public Slider speedSlider;
    public Button quitHuntButton;
    public Button NextHumanButton;
    public Button PreviousHumanButton;

    public TMP_Dropdown GaitDropdown;
    Animator HumanAnimator;
    public Camera huntCamera;
    public Camera mainCamera;

    public Vector3 originalTarget;

    public GameObject DayNight;

    public Text clockText;

    private bool isHuntingPanelOn = false;
    private int pickedHumanNumber;    

    void Start()
    {

        HidePanel(HumanBodyFunctionsPanel);
        HidePanel(HumanPickerPanel);
        HidePanel(HumanInfoPanel);
        HidePanel(HumanGaitPanel);
        HidePanel(HumanInventoryPanel);

        speedSlider.onValueChanged.AddListener(delegate { SpeedChanged(); });
        speedSlider.enabled = true;
        NextHumanButton.onClick.AddListener(delegate { NextHuman(); });
        PreviousHumanButton.onClick.AddListener(delegate { PreviousHuman(); });
        quitHuntButton.onClick.AddListener(delegate { QuitHunt(); });

        GaitDropdown.gameObject.SetActive(false);
        GaitDropdown.onValueChanged.AddListener(delegate { GaitDropdownChangeReceiver(GaitDropdown.value); });
        quitHuntButton.gameObject.SetActive(false);
        pickedHumanNumber = 0;
    }

    void Update()
    {
        units = GetComponent<InputManager>().units;
        int dayTimeSeconds = (int)DayNight.GetComponent<WeatherMakerDayNightCycleManagerScript>().TimeOfDay;
        System.TimeSpan t = System.TimeSpan.FromSeconds(dayTimeSeconds);
        clockText.text = string.Format("{0:00}:{1:00}:{2:00}", t.Hours, t.Minutes, t.Seconds);
    }

    public void ShowPanel(CanvasGroup panel)
    {
        panel.alpha = 1;
        panel.blocksRaycasts = true;
        panel.interactable = true;
    }

    public void HidePanel(CanvasGroup panel)
    {
        panel.alpha = 0;
        panel.blocksRaycasts = false;
        panel.interactable = false;
    }

    public void GaitDropdownChangeReceiver(int val)
    {
        GameObject unit = GetComponent<InputManager>().HumanWithShownInfo;
        if (IsHumanAlive(unit))
        {
            switch (val)
            {
                case 0:
                    speedSlider.enabled = true;
                    Debug.Log("nastaveni runspeed " + unit.GetComponent<HumanGaitInfo>().maxRunSpeed);
                    SetSpeedSliderLimits(unit.GetComponent<HumanGaitInfo>().minRunSpeed, unit.GetComponent<HumanGaitInfo>().maxRunSpeed);
                    unit.GetComponent<HumanInfo>().humanTask = HumanTaskList.Running;
                    unit.GetComponent<HumanGaitInfo>().gait = HumanGaitList.Running;
                    unit.GetComponent<HumanInfo>().gait = val;
                    break;
                case 1:
                    speedSlider.enabled = true;
                    Debug.Log("nastaveni walkspeed");
                    SetSpeedSliderLimits(unit.GetComponent<HumanGaitInfo>().minWalkSpeed, unit.GetComponent<HumanGaitInfo>().maxWalkSpeed);
                    unit.GetComponent<HumanInfo>().humanTask = HumanTaskList.Walking;
                    unit.GetComponent<HumanGaitInfo>().gait = HumanGaitList.Walking;
                    unit.GetComponent<HumanInfo>().gait = val;
                    break;
            }
        }

    }

    public bool IsHumanAlive(GameObject unit)
    {
        return unit != null && unit.GetComponent<HumanInfo>().humanTask != HumanTaskList.Dead;
    }

    public void SetSpeedSliderLimits(float minSpeed, float maxSpeed)
    {
        speedSlider.maxValue = maxSpeed;
        speedSlider.minValue = minSpeed;
    }

    void SpeedChanged()
    {
        
        foreach (GameObject unit in units)
        {

            if (!unit.GetComponent<HumanInfo>().isInfoShown)
            {
                // do nothing
            }
            else if (unit.GetComponent<HumanInfo>().isHunting)
            {
                unit.GetComponent<MovementManager>().actionList.Move(unit.GetComponent<MovementManager>().agent, unit.GetComponent<MovementManager>().shiftedPoint, speedSlider.value, unit.GetComponent<MovementManager>().unitNumber);
                unit.GetComponent<MovementManager>().locomotionSpeed = speedSlider.value;
                //HumanAnimator = unit.GetComponent<Animator>();
                unit.GetComponent<HumanGaitInfo>().speed = speedSlider.value;
                unit.GetComponent<HumanGaitInfo>().SetAnimationSpeed(speedSlider.maxValue);
                //HumanAnimator.speed = speedSlider.value;

            }
            else if (!unit.GetComponent<HumanInfo>().isHunting)
            {
                unit.GetComponent<MovementManager>().actionList.Move(unit.GetComponent<MovementManager>().agent, unit.GetComponent<MovementManager>().shiftedPoint, speedSlider.value, unit.GetComponent<MovementManager>().unitNumber);
                //HumanAnimator = unit.GetComponent<Animator>();
                unit.GetComponent<HumanGaitInfo>().speed = speedSlider.value;
                unit.GetComponent<HumanGaitInfo>().SetAnimationSpeed(speedSlider.maxValue);
            }
        }

        GetComponent<PanelValuesManager>().SetSpeedText();
    }

    void QuitHunt()
    {
        foreach (GameObject unit in units)
        {
            if (unit.GetComponent<HumanInfo>().isHunting)
            {
                unit.GetComponent<MovementManager>().QuitHunt();
               
            }
        }
        GetComponent<InputManager>().huntingMode = false;
        quitHuntButton.gameObject.SetActive(false);
    }

    public void NextHuman()
    {
        int originalPickedHumanNumber = pickedHumanNumber;
        if (pickedHumanNumber < GetComponent<InputManager>().selectedUnits.Count - 1)
        {
            pickedHumanNumber++;
        }
        else
        {
            pickedHumanNumber = 0;
        }
        HumanSelected();
        GetComponent<InputManager>().HideHumanInfo(GetComponent<InputManager>().selectedUnits[originalPickedHumanNumber]);
        GetComponent<InputManager>().ShowHumanInfo(GetComponent<InputManager>().selectedUnits[pickedHumanNumber]);
    }

    void PreviousHuman()
    {
        int originalPickedHumanNumber = pickedHumanNumber;
        if (pickedHumanNumber == 0)
        {
            pickedHumanNumber = GetComponent<InputManager>().selectedUnits.Count - 1;
        }
        else
        {
            pickedHumanNumber--;
        }
        HumanSelected();
        GetComponent<InputManager>().HideHumanInfo(GetComponent<InputManager>().selectedUnits[originalPickedHumanNumber]);
        GetComponent<InputManager>().ShowHumanInfo(GetComponent<InputManager>().selectedUnits[pickedHumanNumber]);
    }

    public void HumanSelected()
    {
        ShowPanel(HumanBodyFunctionsPanel);
        ShowPanel(HumanInfoPanel);
        if (GetComponent<InputManager>().selectedUnits[pickedHumanNumber].GetComponent<HumanInfo>().humanTask == HumanTaskList.Walking || GetComponent<InputManager>().selectedUnits[pickedHumanNumber].GetComponent<HumanInfo>().humanTask == HumanTaskList.Running)
        {
            ShowPanel(HumanGaitPanel);
        }
        
        ShowPanel(HumanPickerPanel);
        ShowPanel(HumanInventoryPanel);
    }
    
    public void HumanDeselected()
    {
        pickedHumanNumber = 0;
        HidePanel(HumanBodyFunctionsPanel);
        HidePanel(HumanInfoPanel);
        HidePanel(HumanGaitPanel);
        HidePanel(HumanPickerPanel);
        HidePanel(HumanInventoryPanel);
    }

    public void SetSpeedSlider()
    {
        speedSlider.value = speedSlider.minValue;
    }

    public void DisableInteractables()
    {
        HumanPickerPanel.interactable = false;
    }
    
    public void EnableInteractables()
    {
        HumanPickerPanel.interactable = true;
    }
}
