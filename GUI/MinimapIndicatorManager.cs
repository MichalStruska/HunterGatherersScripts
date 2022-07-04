using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapIndicatorManager : MonoBehaviour
{

    SpriteRenderer IconSpriteRenderer;
    private Color indicatorColorDeselect = new Color(13 / 255, 255 / 255, 0 / 255);
    private Color indicatorColorSelect = new Color(255 / 255, 111 / 255, 0 / 255);

    void Start()
    {
        IconSpriteRenderer = GetComponent<SpriteRenderer>();
        IconSpriteRenderer.color = indicatorColorDeselect;
    }

    public void SetIndicatorColorDeselect()
    {
        IconSpriteRenderer.color = indicatorColorDeselect;
    }
    
    public void SetIndicatorColorSelect()
    {
        IconSpriteRenderer.color = indicatorColorSelect;
    }
}
