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

    public GameObject HumanHutPeoplePanel;
    public GameObject HutInventoryPanel;

    public Color FreeSpaceColor;
    public Color TakenSpaceColor;

    public int selectedHumanNumber;

    [SerializeField]
    public HumanButton[] humanButtons = new HumanButton[6];

    void Start()
    {
        HidePanel(HumanHutPanel);
        HidePanel(HutInventoryPanel);
        HidePanel(HumanHutPeoplePanel);

        EatButton.onClick.AddListener(delegate { EatInHut(); });
        BringResourcesButton.onClick.AddListener(delegate { BringResources(); });
        SleepButton.onClick.AddListener(delegate { SleepInHut(); });

        for (int i = 0; i < humanButtons.Length; i++)
        {
            int x = i;
            humanButtons[i].button.onClick.AddListener(delegate { ClickOnHumanButton(x); });
            HutSpaceFree(humanButtons[i].button);
        }

        ResetHutPanelCounter();
        leftClickOnPanel = false;

    }

    void Update()
    {
        if (IsHutPanelOn())
        {
            panelOnCounter++;
            if (panelOnCounter > 3 && Input.GetMouseButtonDown(1))
            {
                HidePanel(HumanHutPanel);
                ResetHutPanelCounter();
            }

            else if (!GetComponent<InputManager>().IsPointerOverUIObject() && Input.GetMouseButtonDown(0))
            {
                HidePanel(HumanHutPanel);
                ResetHutPanelCounter();
            }
        }

    }

    public void ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
    }

    public void HidePanel(GameObject panel)
    {
        panel.SetActive(false);
    }

    public bool IsHutPanelOn()
    {
        return HumanHutPanel.activeSelf;
    }

    public void ResetHutPanelCounter()
    {
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
        HidePanel(HumanHutPanel);
        ResetHutPanelCounter();
    }

    public void BringResources()
    {
        selectedUnits = GetComponent<InputManager>().selectedUnits;
        foreach (GameObject unit in selectedUnits)
        {   
            unit.GetComponent<MovementManager>().GoBringResources();
            unit.GetComponent<MovementManager>().GoToHut();
        }
        HidePanel(HumanHutPanel);
        ResetHutPanelCounter();
    }

    public void SleepInHut()
    {
        selectedUnits = GetComponent<InputManager>().selectedUnits;
        foreach (GameObject unit in selectedUnits)
        {
            unit.GetComponent<MovementManager>().GoToHut();
            unit.GetComponent<MovementManager>().GoToSleep();
        }
        HidePanel(HumanHutPanel);
        ResetHutPanelCounter();
    }

    private bool IsFoodInHut()
    {
        if (GetComponent<HutManager>().tuberCounter > 0)
        {
            return true;
        }
        return false;
    }

    public void ClickOnHumanButton(int humanButtonNumber)
    {
        Debug.Log("clocked od human " + humanButtons[humanButtonNumber].human);

        if (humanButtons[humanButtonNumber].human != null)
        {
            Player.GetComponent<InputManager>().DeselectHuman();
            Player.GetComponent<InputManager>().SelectHuman(humanButtons[humanButtonNumber].human);
        }
    }

    public void ExitHut()
    {
        HutSpaceFree(humanButtons[selectedHumanNumber].button);
        humanButtons[selectedHumanNumber].human = null;
    }

    public void UpdateTuberCounter(int tuberCounter, int maxTuberNumber)
    {
        TuberCounterText.text = tuberCounter.ToString() + "/" + maxTuberNumber.ToString();
    }


    public void HutSelected(GameObject Hut)
    {
        ShowPanel(HumanHutPeoplePanel);
        ShowPanel(HutInventoryPanel);
        RefreshHumanPanel(Hut);
    }

    public void HutDeselected()
    {
        HidePanel(HumanHutPeoplePanel);
        HidePanel(HutInventoryPanel);
    }

    public void RefreshHumanPanel(GameObject Hut)
    {
        
        int positionCounter = 0;
        foreach (Positions position in Hut.GetComponent<HutManager>().positions)
        {
            if (position.availability)
            {
                HutSpaceFree(humanButtons[positionCounter].button);
            }
            else
            {
                HutSpaceTaken(humanButtons[positionCounter].button);
                humanButtons[positionCounter].human = position.human;
            }
            Debug.Log("refresh panel " + humanButtons[positionCounter].human);

            positionCounter++;
        }
    }

    public void HutSpaceTaken(Button button)
    {
        button.GetComponent<Image>().color = TakenSpaceColor;
    }

    public void HutSpaceFree(Button button)
    {
        button.GetComponent<Image>().color = FreeSpaceColor;
    }

}
