using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DigitalRuby.WeatherMaker;

[System.Serializable]
public struct HumanButton
{
    public GameObject human;
    public Button button;
}

public class HutPanelController : MonoBehaviour
{
    
    public GameObject HumanHutPanel;
    
    public Button EatButton;
    public Button BringResourcesButton;
    public Button SleepButton;
    public List<GameObject> selectedUnits;
    public GameObject Player;

    public int panelOnCounter;
    public bool leftClickOnPanel;

    private bool isHutPanelOn;

    public Text TuberCounterText;

    [SerializeField]
    public HumanButton[] humanButtons = new HumanButton[6];

    void Start()
    {
        HidePanel();

        EatButton.onClick.AddListener(delegate { EatInHut(); });
        BringResourcesButton.onClick.AddListener(delegate { BringResources(); });
        SleepButton.onClick.AddListener(delegate { SleepInHut(); });

        panelOnCounter = 0;
        leftClickOnPanel = false;


    }

    void Update()
    {
        if (isHutPanelOn)
        {
            panelOnCounter++;
            if (panelOnCounter > 3 && Input.GetMouseButtonDown(1))
            {
                HidePanel();
            }

            else if (!GetComponent<InputManager>().IsPointerOverUIObject() && Input.GetMouseButtonDown(0))
            {
                HidePanel();
            }
        }

    }

    public void ShowPanel()
    {

        //GetComponent<PanelController>().ShowPanel(HumanHutPanel);
        Debug.Log("show hut");
        HumanHutPanel.SetActive(true);
        isHutPanelOn = true;

    }

    public void HidePanel()
    {
        //GetComponent<PanelController>().HidePanel(HumanHutPanel);
        HumanHutPanel.SetActive(false);
        isHutPanelOn = false;
        panelOnCounter = 0;
    }

    public void GiveButtonsInteractability()
    {
        GiveEatButtonInteractability();
        GiveSleepButtonInteractability();
        
        
    }

    public void GiveEatButtonInteractability()
    {
        if (IsFoodInHut())
        {
            EatButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            EatButton.GetComponent<Button>().interactable = false;
        }
    }
    
    public void GiveSleepButtonInteractability()
    {
        if (GetComponent<HutManager>().IsFreeSpace())
        {
            SleepButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            SleepButton.GetComponent<Button>().interactable = false;
        }
    }


    public void EatInHut()
    {
        selectedUnits = GetComponent<InputManager>().selectedUnits;
        foreach (GameObject unit in selectedUnits)
        {
            unit.GetComponent<MovementManager>().GoToHut();
            unit.GetComponent<MovementManager>().GoToEat();
        }
        HidePanel();
    }

    public void BringResources()
    {
        selectedUnits = GetComponent<InputManager>().selectedUnits;
        foreach (GameObject unit in selectedUnits)
        {   
            unit.GetComponent<MovementManager>().GoBringResources();
            unit.GetComponent<MovementManager>().GoToHut();
        }
        HidePanel();
    }

    public void SleepInHut()
    {
        selectedUnits = GetComponent<InputManager>().selectedUnits;
        foreach (GameObject unit in selectedUnits)
        {
            unit.GetComponent<MovementManager>().GoToHut();
            unit.GetComponent<MovementManager>().GoToSleep();
        }
        HidePanel();
    }

    private bool IsFoodInHut()
    {
        if (GetComponent<HutManager>().tuberCounter > 0)
        {
            return true;
        }
        return false;
    }

    public void UpdateTuberCounter(int tuberCounter, int maxTuberNumber)
    {
        TuberCounterText.text = tuberCounter.ToString() + "/" + maxTuberNumber.ToString();
    }

}
