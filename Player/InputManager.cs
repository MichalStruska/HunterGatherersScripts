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

    void Start()
    {
        GameObject[] unitsArray = GameObject.FindGameObjectsWithTag("Selectable");
        units.AddRange(unitsArray);
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
            if (IsPointerOverUIObject())
            {
                //do nothing
            }
            //if (hit.collider.tag != "Selectable" && !IsPointerOverUIObject()) //&& !EventSystem.current.IsPointerOverGameObject())
            //{
            //    DeselectHuman();
            //}
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
        GetComponent<PanelController>().HutSelected(Hut);
    }

    public void DeselectHut()
    {
        GetComponent<PanelController>().HutDeselected();
    }

    public void SelectHuman(GameObject Human)
    {
        selectedInfo = Human.GetComponent<HumanInfo>();

        GetComponent<PanelController>().HumanDeselected();

        //if (units != null && units.Length != 0)
        //{
        //    foreach (GameObject unit in units)
        //    {
        //        if (unit.GetComponent<HumanInfo>().isSelected)
        //        {
        //            unit.GetComponent<HumanInfo>().DeselectHuman();
        //            selectedUnits.Clear();

        //        }
        //    }
        //}
        selectedInfo.isSelected = true;
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
        SelectionCanvas.GetComponent<GUISelection>().setIndicatorColor(unit.GetComponent<HumanInfo>().selectionIndicator, SelectionCanvas.GetComponent<GUISelection>().showInfoColor);
        GetComponent<PanelValuesManager>().SetAllHumanInformation(unit.GetComponent<HumanInfo>());
        GetComponent<InventoryPanelController>().SetHumanInventory(unit.GetComponent<HumanInfo>());
    }
    
    public void HideHumanInfo(GameObject unit)
    {
        unit.GetComponent<HumanInfo>().isInfoShown = false;
        HumanWithShownInfo = null;
        SelectionCanvas.GetComponent<GUISelection>().setIndicatorColor(unit.GetComponent<HumanInfo>().selectionIndicator, SelectionCanvas.GetComponent<GUISelection>().hideInfoColor);
    }

}
