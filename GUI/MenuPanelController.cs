using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPanelController : MonoBehaviour
{

    public Button QuitButton;
    public Button ResumeButton;
    public Button SaveButton;
    public CanvasGroup MenuPanel;
    public Button MenuButton;

    public static bool isGamePaused;

    void Start()
    {
        QuitButton.onClick.AddListener(delegate { QuitGame(); });
        ResumeButton.onClick.AddListener(delegate { ResumeGame(); });
        SaveButton.onClick.AddListener(delegate { SaveGame(); });
        MenuButton.onClick.AddListener(delegate { OpenMenu(); });
        GetComponent<PanelController>().HidePanel(MenuPanel);
        isGamePaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    
    public void ResumeGame()
    {
        Time.timeScale = 1;
        GetComponent<PanelController>().HidePanel(MenuPanel);
        isGamePaused = false;
        GetComponent<PanelController>().EnableInteractables();
        MenuButton.interactable = true;
    }
    
    public void SaveGame()
    {

    }

    public void HidePanel()
    {

    }
    
    public void OpenMenu()
    {
        GetComponent<PanelController>().ShowPanel(MenuPanel);
        Time.timeScale = 0;
        isGamePaused = true;
        GetComponent<PanelController>().DisableInteractables();
        MenuButton.interactable = false;
    }

}
