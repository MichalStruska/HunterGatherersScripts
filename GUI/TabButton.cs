using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class TabButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler 
{
    public TabGroup tabGroup;

    public Image background;

    public bool isInteractable;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (isInteractable)
        {
            tabGroup.OnTabSelected(this);
        }
            
    }
    
    public void OnPointerExit(PointerEventData eventData)
    {
        tabGroup.OnTabExit(this);
    }
    
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isInteractable)
        {
            tabGroup.OnTabEnter(this);
        }
        
    }

    void Awake()
    {
        background = GetComponent<Image>();
    }

    void Start()
    {

    }


    void Update()
    {
        
    }

    public void SetInteractability(bool state)
    {
        isInteractable = state;
    }
}
