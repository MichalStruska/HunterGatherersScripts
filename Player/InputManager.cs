using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public GameObject selectedObject;

    private HumanInfo selectedInfo;
    GUISelection GuiSelection;

    public List<GameObject> units;
    public List<GameObject> selectedUnits;
    public GameObject SelectionCanvas;

    public bool huntingMode = false;

    public Vector3 followedHunterPos;
    public Transform followedPreyTransform;
    public GameObject followedPrey;

    public Texture2D cursorTextureDefault;
    public Texture2D cursorTextureMove;
    public CursorMode cursorMode = CursorMode.Auto;
    public bool clickedOnUiFirst;

    public static bool isSomeoneSelected;

    private Terrain observedTerrain;
    private Terrain currentTerrain;
    public int posX;
    public int posZ;

    public LayerMask layerMask;

    public GameObject HumanWithShownInfo;

    public GameObject HumansPanel;

    void Awake()
    {
        GameObject[] unitsArray = GameObject.FindGameObjectsWithTag("Selectable");
        units.AddRange(unitsArray);

        HumansPanel.GetComponent<HumanThumbnailsManager>().SetThumbnails();
    }

    void Start()
    {

        selectedUnits = new List<GameObject>();
        Cursor.SetCursor(cursorTextureDefault, Vector2.zero, cursorMode);

        GuiSelection = SelectionCanvas.GetComponent<GUISelection>();
        isSomeoneSelected = false;
    }

    void Update()
    {
        observedTerrain = Terrain.activeTerrain;
        

        if (MenuPanelController.isGamePaused)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GetComponent<MenuPanelController>().ResumeGame();
            }
            //do nothing
        }

        else
        {
            if (!huntingMode && Input.GetMouseButtonDown(0))
            {
                if (!IsPointerOverUIObject())
                {
                    LeftClick();
                    clickedOnUiFirst = false;
                }
                else
                {
                    clickedOnUiFirst = true;
                }
            }

            else if(huntingMode)
            {
                followedPreyTransform = followedPrey.transform;
                followedHunterPos.y = 25;
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                GetComponent<MenuPanelController>().OpenMenu();
                clickedOnUiFirst = true;
            }
        }
        
    }

    public void LeftClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~layerMask))
        {
            GetComponent<PanelController>().HideAllHumansPanel();
            if (IsPointerOverUIObject())
            {
                //do nothing
            }
            else if (hit.collider.tag == "Selectable")
            {
                DeselectHut();
                DeselectHuman();
                SelectHuman(hit.collider.gameObject);
            }
            else if (hit.collider.tag == "Hut")
            {
                DeselectHuman();
                SelectHut(hit.collider.gameObject);
            }
            else
            {
                DeselectHuman();
                DeselectHut();
            }
        }
        else
        {
            DeselectHuman();
        }
    }

    public void SelectHut(GameObject Hut)
    {
        GetComponent<HutPanelController>().HutSelected(Hut);
    }

    public void DeselectHut()
    {
        GetComponent<HutPanelController>().HutDeselected();
    }

    public void SelectHuman(GameObject Human)
    {
        GetComponent<PanelController>().HumanDeselected();

        Human.GetComponent<HumanInfo>().isSelected = true;
        selectedUnits.Add(Human);
        ShowHumanInfo(Human);
        GetComponent<PanelController>().HumanSelected();
        Human.GetComponent<HumanInfo>().SelectHuman();
        
    }

    public void DeselectHuman()
    {
        selectedObject = null;
        selectedUnits.Clear();
        
        if (selectedInfo != null)
        {
            selectedInfo.isSelected = false;
        }
        if (units != null && units.Count != 0)
        {
            foreach (GameObject unit in units)
            {
                unit.GetComponent<HumanInfo>().DeselectHuman();
                GetComponent<PanelController>().HumanDeselected();
                HideHumanInfo(unit);
            }
        }
    }

    public void DeselectHuman(GameObject human)
    {
        GetComponent<PanelController>().NextHuman();
        selectedUnits.Remove(human);
        human.GetComponent<HumanInfo>().DeselectHuman();
        GetComponent<PanelController>().HumanDeselected();
        HideHumanInfo(human);
    }

    public bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);

        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject.layer == 5) //5 = UI layer
            {
                
                return true;
            }
        }

        return false;
    }

    public void ShowHumanInfo(GameObject unit)
    {
        unit.GetComponent<HumanInfo>().isInfoShown = true;
        HumanWithShownInfo = unit;
        //SelectionCanvas.GetComponent<GUISelection>().setIndicatorColor(unit.GetComponent<HumanInfo>().selectionIndicator, SelectionCanvas.GetComponent<GUISelection>().showInfoColor);
        GetComponent<PanelValuesManager>().SetAllHumanInformation(unit);
        GetComponent<InventoryPanelController>().SetHumanInventory(unit.GetComponent<HumanInfo>());
    }
    
    public void HideHumanInfo(GameObject unit)
    {
        unit.GetComponent<HumanInfo>().isInfoShown = false;
        HumanWithShownInfo = null;
        //SelectionCanvas.GetComponent<GUISelection>().setIndicatorColor(unit.GetComponent<HumanInfo>().selectionIndicator, SelectionCanvas.GetComponent<GUISelection>().hideInfoColor);
    }

    public int GetHumansLength()
    {
        return units.Count;
    }

}
