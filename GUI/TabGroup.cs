using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[System.Serializable]
public struct CustomList
{
    [SerializeField]
    public List<GameObject> objects;
}

[System.Serializable]
public struct ButtonInteractability
{
    public TabButton button;
    public bool isInteractable;
    public GameObject controlledObject;
}

public class TabGroup : MonoBehaviour
{
    public List<TabButton> tabButtons;
    public Sprite tabIdle;
    public Sprite tabHover;
    public Sprite tabActive;
    public Color idleColor;
    public Color hoverColor;
    public Color activeColor;
    public TabButton selectedTab;
    public TabButton defaultTab;
    public CustomList objectsToSwap;
    [SerializeField]
    public List<ButtonInteractability> buttonInteractabilityList;

    void Start()
    {
        foreach (ButtonInteractability buttonInteractability in buttonInteractabilityList)
        {
            if (buttonInteractability.isInteractable)
            {
                buttonInteractability.button.isInteractable = true;
            }
            else
            {
                buttonInteractability.button.isInteractable = false;
            }
        }

        SetDefaultPage();
    }

    public void Subscribe(TabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<TabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(TabButton button)
    {
        ResetTabs();
        if (selectedTab == null || selectedTab != button)
        {
            button.background.color = hoverColor;
        }
        
    }
    
    public void OnTabExit(TabButton button)
    {
        ResetTabs();
    }
    
    public void OnTabSelected(TabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.background.color = activeColor;
        
        for (int i = 0; i < buttonInteractabilityList.Count; i++)
        {
            if (button == buttonInteractabilityList[i].button)
            {
                buttonInteractabilityList[i].controlledObject.SetActive(true);
            }
            else
            {
                buttonInteractabilityList[i].controlledObject.SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        
        foreach (TabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab)
            {
                continue;
            }
            button.background.color = idleColor;
        }
    }

    public void SetDefaultPage()
    {
        OnTabSelected(defaultTab);
    }
}
