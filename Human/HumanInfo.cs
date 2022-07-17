using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class HumanInfo : OrganismInfoBase
{
    public bool isSelected = false;
    public bool isUnit;

    public string objectName;
    public int age;
    
    public int tuberNumber;
    public int tuberNumberMax;

    public HumanSexList sex;

    public int gait;

    public GameObject MainCanvas;
    public HumanTaskList humanTask;
    public HumanTargetTaskList humanTargetTask;
    public GameObject MinimapIndicator;

    public GameObject Player;

    public bool isInfoShown = false;
    private PanelController panelController;

    public GameObject selectionIndicator;
    public Image targetIndicator;
    public Vector3 targetIndicatorPosition;

    public bool hasTarget = false;

    public int taskTimeMultiplier = 7;

    public bool isHunting = false;

    public float progressBarValue;

    void Start()
    {
        humanTask = HumanTaskList.Idle;
        panelController = Player.GetComponent<PanelController>();
        coreTemperature = 37f;
        surfaceAbsorptivity = 0.8f;
        convectionCoefficient = 8.3f;
        //mass = 78f;
        //base.mass = mass;
    }

    void Update()
    {

        //if (health <= 0)
        //{
        //    Destroy(gameObject);
        //}

    }

    public void SelectHuman()
    {
        isSelected = true;
        MinimapIndicator.GetComponent<MinimapIndicatorManager>().SetIndicatorColorSelect();
        selectionIndicator.SetActive(true);
        if (hasTarget)
        {
            targetIndicator.enabled = true;
        }
    }
    
    public void DeselectHuman()
    {
        isSelected = false;
        MinimapIndicator.GetComponent<MinimapIndicatorManager>().SetIndicatorColorDeselect();
        isInfoShown = false;
        selectionIndicator.SetActive(false);
        targetIndicator.enabled = false;
    }

    public void SendHumanFunctions()
    {
        if (isInfoShown)
        {
            Player.GetComponent<PanelValuesManager>().SetBodyFunctions(this);
        }
    }

    private void SendHumanInfo()
    {
        if (isInfoShown)
        {
            Player.GetComponent<PanelValuesManager>().SetHumanInformation(this);
            Player.GetComponent<InventoryPanelController>().SetHumanInventory(this);
        }
    }

    public void SetSpeedSlider()
    {
        Player.GetComponent<PanelValuesManager>().SetSpeed(this);
        Player.GetComponent<PanelValuesManager>().SetSpeedText();
    }

    public void AddTuber()
    {
        if (!IsFullCapacity(tuberNumber, tuberNumberMax))
        {
            tuberNumber++;
        }
        RefreshInventory();
    }
    
    public void PutTubersToHut()
    {
        tuberNumber = 0;
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        if (isInfoShown)
        {
            Player.GetComponent<InventoryPanelController>().SetHumanInventory(this);
        }
        
    }

    public bool IsFullCapacity(int presentAmount, int maxAmount)
    {
        if (presentAmount == maxAmount)
        {
            return true;
        }
        return false;
    }

    public override void Die()
    {
        //base.Die();
        DeselectHuman();
        humanTask = HumanTaskList.Dead;
        GetComponent<MovementManager>().enabled = false;
        GetComponent<HumanThermoModel>().enabled = false;
        Destroy(gameObject);
        Player.GetComponent<InputManager>().units.RemoveAll(s => s.GetComponent<HumanInfo>().humanTask == HumanTaskList.Dead);

    }

    IEnumerator OrderDelay()
    {
        yield return new WaitForSeconds(5);
        gameObject.SetActive(false);
        //Destroy(gameObject);
    }

}
