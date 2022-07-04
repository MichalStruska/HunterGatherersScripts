using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class SelectionBoxManager : MonoBehaviour
{
 
    private Vector2 boxStart;
    private Vector2 boxEnd;
    public List<GameObject> units;
    private Rect selectBox;
    public RectTransform selectionBox;
    private Vector2 startPos;
    public bool huntingMode = false;
    private bool clickedOnUiFirst;

    void Start()
    {
        units = GetComponent<InputManager>().units;
    }

    void Update()
    {
        if (MenuPanelController.isGamePaused)
        {
            //do nothing
        }
        
        else if (!huntingMode)
        {

            if (Input.GetMouseButtonDown(0))
            {
                startPos = Input.mousePosition; 
            }

            clickedOnUiFirst = GetComponent<InputManager>().clickedOnUiFirst;

            if (Input.GetMouseButtonUp(0) && !clickedOnUiFirst && Vector3.Distance(Input.mousePosition, startPos) > 0)
            {
                ReleaseSelectionBox();
            }

            if (Input.GetMouseButton(0) && !clickedOnUiFirst)
            {
                UpdateSelectionBox(Input.mousePosition);
            }

            selectBox = new Rect(boxStart.x, Screen.height - boxStart.y, boxEnd.x - boxStart.x, -1 * ((Screen.height - boxStart.y) - (Screen.height - boxEnd.y)));

        }

    }
    
    void ReleaseSelectionBox()
    {
        selectionBox.gameObject.SetActive(false);

        Vector2 min = selectionBox.anchoredPosition - (selectionBox.sizeDelta / 2);
        Vector2 max = selectionBox.anchoredPosition + (selectionBox.sizeDelta / 2);

        int unitCounter = 0;
        foreach (GameObject unit in GetComponent<InputManager>().units)
        {
            if (unit.GetComponent<HumanInfo>().isUnit)
            {
                Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

                if (screenPos.x > min.x && screenPos.x < max.x && screenPos.y > min.y && screenPos.y < max.y)
                {
                    unit.GetComponent<HumanInfo>().SelectHuman();
                    unit.GetComponent<MovementManager>().unitNumber = unitCounter;
                    GetComponent<InputManager>().selectedUnits.Add(unit);
                    unitCounter++;
                }
                else
                {
                    unit.GetComponent<HumanInfo>().DeselectHuman();
                }
            }
        }
        Debug.Log("pocet " + unitCounter);
        if (unitCounter > 0)
        {
            GetComponent<PanelController>().HumanSelected();
            GetComponent<InputManager>().ShowHumanInfo(GetComponent<InputManager>().selectedUnits[0]);
            GetComponent<InputManager>().SelectHuman(GetComponent<InputManager>().selectedUnits[0]);
        }
        else
        {
            GetComponent<InputManager>().selectedUnits.Clear();
            GetComponent<InputManager>().DeselectHuman();
            // do nothing
        }
    }


    void UpdateSelectionBox(Vector2 currentMousePos)
    {
        if (!selectionBox.gameObject.activeInHierarchy)
        {
            selectionBox.gameObject.SetActive(true);
        }
        float width = currentMousePos.x - startPos.x;
        float height = currentMousePos.y - startPos.y;

        selectionBox.sizeDelta = new Vector2(Mathf.Abs(width), Mathf.Abs(height));
        selectionBox.anchoredPosition = startPos + new Vector2(width / 2, height / 2);

    }
}
